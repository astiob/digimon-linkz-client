using System;

namespace UniRx.Operators
{
	internal class TimeoutFrameObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly int frameCount;

		private readonly FrameCountType frameCountType;

		public TimeoutFrameObservable(IObservable<T> source, int frameCount, FrameCountType frameCountType) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.frameCount = frameCount;
			this.frameCountType = frameCountType;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new TimeoutFrameObservable<T>.TimeoutFrame(this, observer, cancel).Run();
		}

		private class TimeoutFrame : OperatorObserverBase<T, T>
		{
			private readonly TimeoutFrameObservable<T> parent;

			private readonly object gate = new object();

			private ulong objectId;

			private bool isTimeout;

			private SingleAssignmentDisposable sourceSubscription;

			private SerialDisposable timerSubscription;

			public TimeoutFrame(TimeoutFrameObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.sourceSubscription = new SingleAssignmentDisposable();
				this.timerSubscription = new SerialDisposable();
				this.timerSubscription.Disposable = this.RunTimer(this.objectId);
				this.sourceSubscription.Disposable = this.parent.source.Subscribe(this);
				return StableCompositeDisposable.Create(this.timerSubscription, this.sourceSubscription);
			}

			private IDisposable RunTimer(ulong timerId)
			{
				return Observable.TimerFrame(this.parent.frameCount, this.parent.frameCountType).Subscribe(new TimeoutFrameObservable<T>.TimeoutFrame.TimeoutFrameTick(this, timerId));
			}

			public override void OnNext(T value)
			{
				object obj = this.gate;
				bool flag;
				ulong timerId;
				lock (obj)
				{
					flag = this.isTimeout;
					this.objectId += 1UL;
					timerId = this.objectId;
				}
				if (flag)
				{
					return;
				}
				this.timerSubscription.Disposable = Disposable.Empty;
				this.observer.OnNext(value);
				this.timerSubscription.Disposable = this.RunTimer(timerId);
			}

			public override void OnError(Exception error)
			{
				object obj = this.gate;
				bool flag;
				lock (obj)
				{
					flag = this.isTimeout;
					this.objectId += 1UL;
				}
				if (flag)
				{
					return;
				}
				this.timerSubscription.Dispose();
				try
				{
					this.observer.OnError(error);
				}
				finally
				{
					base.Dispose();
				}
			}

			public override void OnCompleted()
			{
				object obj = this.gate;
				bool flag;
				lock (obj)
				{
					flag = this.isTimeout;
					this.objectId += 1UL;
				}
				if (flag)
				{
					return;
				}
				this.timerSubscription.Dispose();
				try
				{
					this.observer.OnCompleted();
				}
				finally
				{
					base.Dispose();
				}
			}

			private class TimeoutFrameTick : IObserver<long>
			{
				private readonly TimeoutFrameObservable<T>.TimeoutFrame parent;

				private readonly ulong timerId;

				public TimeoutFrameTick(TimeoutFrameObservable<T>.TimeoutFrame parent, ulong timerId)
				{
					this.parent = parent;
					this.timerId = timerId;
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
						if (this.parent.objectId == this.timerId)
						{
							this.parent.isTimeout = true;
						}
					}
					if (this.parent.isTimeout)
					{
						try
						{
							this.parent.observer.OnError(new TimeoutException());
						}
						finally
						{
							this.parent.Dispose();
						}
					}
				}
			}
		}
	}
}
