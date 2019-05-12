using System;

namespace UniRx.Operators
{
	internal class ThrottleFrameObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly int frameCount;

		private readonly FrameCountType frameCountType;

		public ThrottleFrameObservable(IObservable<T> source, int frameCount, FrameCountType frameCountType) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.frameCount = frameCount;
			this.frameCountType = frameCountType;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new ThrottleFrameObservable<T>.ThrottleFrame(this, observer, cancel).Run();
		}

		private class ThrottleFrame : OperatorObserverBase<T, T>
		{
			private readonly ThrottleFrameObservable<T> parent;

			private readonly object gate = new object();

			private T latestValue = default(T);

			private bool hasValue;

			private SerialDisposable cancelable;

			private ulong id;

			public ThrottleFrame(ThrottleFrameObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.cancelable = new SerialDisposable();
				IDisposable disposable = this.parent.source.Subscribe(this);
				return StableCompositeDisposable.Create(this.cancelable, disposable);
			}

			public override void OnNext(T value)
			{
				object obj = this.gate;
				ulong currentid;
				lock (obj)
				{
					this.hasValue = true;
					this.latestValue = value;
					this.id += 1UL;
					currentid = this.id;
				}
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				this.cancelable.Disposable = singleAssignmentDisposable;
				singleAssignmentDisposable.Disposable = Observable.TimerFrame(this.parent.frameCount, this.parent.frameCountType).Subscribe(new ThrottleFrameObservable<T>.ThrottleFrame.ThrottleFrameTick(this, currentid));
			}

			public override void OnError(Exception error)
			{
				this.cancelable.Dispose();
				object obj = this.gate;
				lock (obj)
				{
					this.hasValue = false;
					this.id += 1UL;
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
					if (this.hasValue)
					{
						this.observer.OnNext(this.latestValue);
					}
					this.hasValue = false;
					this.id += 1UL;
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

			private class ThrottleFrameTick : IObserver<long>
			{
				private readonly ThrottleFrameObservable<T>.ThrottleFrame parent;

				private readonly ulong currentid;

				public ThrottleFrameTick(ThrottleFrameObservable<T>.ThrottleFrame parent, ulong currentid)
				{
					this.parent = parent;
					this.currentid = currentid;
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
						if (this.parent.hasValue && this.parent.id == this.currentid)
						{
							this.parent.observer.OnNext(this.parent.latestValue);
						}
						this.parent.hasValue = false;
					}
				}
			}
		}
	}
}
