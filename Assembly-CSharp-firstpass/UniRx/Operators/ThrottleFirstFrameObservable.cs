using System;

namespace UniRx.Operators
{
	internal class ThrottleFirstFrameObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly int frameCount;

		private readonly FrameCountType frameCountType;

		public ThrottleFirstFrameObservable(IObservable<T> source, int frameCount, FrameCountType frameCountType) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.frameCount = frameCount;
			this.frameCountType = frameCountType;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new ThrottleFirstFrameObservable<T>.ThrottleFirstFrame(this, observer, cancel).Run();
		}

		private class ThrottleFirstFrame : OperatorObserverBase<T, T>
		{
			private readonly ThrottleFirstFrameObservable<T> parent;

			private readonly object gate = new object();

			private bool open = true;

			private SerialDisposable cancelable;

			private ThrottleFirstFrameObservable<T>.ThrottleFirstFrame.ThrottleFirstFrameTick tick;

			public ThrottleFirstFrame(ThrottleFirstFrameObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.tick = new ThrottleFirstFrameObservable<T>.ThrottleFirstFrame.ThrottleFirstFrameTick(this);
				this.cancelable = new SerialDisposable();
				IDisposable disposable = this.parent.source.Subscribe(this);
				return StableCompositeDisposable.Create(this.cancelable, disposable);
			}

			private void OnNext()
			{
				object obj = this.gate;
				lock (obj)
				{
					this.open = true;
				}
			}

			public override void OnNext(T value)
			{
				object obj = this.gate;
				lock (obj)
				{
					if (!this.open)
					{
						return;
					}
					this.observer.OnNext(value);
					this.open = false;
				}
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				this.cancelable.Disposable = singleAssignmentDisposable;
				singleAssignmentDisposable.Disposable = Observable.TimerFrame(this.parent.frameCount, this.parent.frameCountType).Subscribe(this.tick);
			}

			public override void OnError(Exception error)
			{
				this.cancelable.Dispose();
				object obj = this.gate;
				lock (obj)
				{
					try
					{
						this.observer.OnError(error);
					}
					finally
					{
						base.Dispose();
					}
				}
			}

			public override void OnCompleted()
			{
				this.cancelable.Dispose();
				object obj = this.gate;
				lock (obj)
				{
					try
					{
						this.observer.OnCompleted();
					}
					finally
					{
						base.Dispose();
					}
				}
			}

			private class ThrottleFirstFrameTick : IObserver<long>
			{
				private readonly ThrottleFirstFrameObservable<T>.ThrottleFirstFrame parent;

				public ThrottleFirstFrameTick(ThrottleFirstFrameObservable<T>.ThrottleFirstFrame parent)
				{
					this.parent = parent;
				}

				public void OnCompleted()
				{
				}

				public void OnError(Exception error)
				{
				}

				public void OnNext(long _)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						this.parent.open = true;
					}
				}
			}
		}
	}
}
