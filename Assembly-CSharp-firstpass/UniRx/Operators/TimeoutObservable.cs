using System;

namespace UniRx.Operators
{
	internal class TimeoutObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly TimeSpan? dueTime;

		private readonly DateTimeOffset? dueTimeDT;

		private readonly IScheduler scheduler;

		public TimeoutObservable(IObservable<T> source, TimeSpan dueTime, IScheduler scheduler) : base(scheduler == Scheduler.CurrentThread || source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.dueTime = new TimeSpan?(dueTime);
			this.scheduler = scheduler;
		}

		public TimeoutObservable(IObservable<T> source, DateTimeOffset dueTime, IScheduler scheduler) : base(scheduler == Scheduler.CurrentThread || source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.dueTimeDT = new DateTimeOffset?(dueTime);
			this.scheduler = scheduler;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			if (this.dueTime != null)
			{
				return new TimeoutObservable<T>.Timeout(this, observer, cancel).Run();
			}
			return new TimeoutObservable<T>.Timeout_(this, observer, cancel).Run();
		}

		private class Timeout : OperatorObserverBase<T, T>
		{
			private readonly TimeoutObservable<T> parent;

			private readonly object gate = new object();

			private ulong objectId;

			private bool isTimeout;

			private SingleAssignmentDisposable sourceSubscription;

			private SerialDisposable timerSubscription;

			public Timeout(TimeoutObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
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
				return this.parent.scheduler.Schedule(this.parent.dueTime.Value, delegate()
				{
					object obj = this.gate;
					lock (obj)
					{
						if (this.objectId == timerId)
						{
							this.isTimeout = true;
						}
					}
					if (this.isTimeout)
					{
						try
						{
							this.observer.OnError(new TimeoutException());
						}
						finally
						{
							this.Dispose();
						}
					}
				});
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
		}

		private class Timeout_ : OperatorObserverBase<T, T>
		{
			private readonly TimeoutObservable<T> parent;

			private readonly object gate = new object();

			private bool isFinished;

			private SingleAssignmentDisposable sourceSubscription;

			private IDisposable timerSubscription;

			public Timeout_(TimeoutObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.sourceSubscription = new SingleAssignmentDisposable();
				this.timerSubscription = this.parent.scheduler.Schedule(this.parent.dueTimeDT.Value, new Action(this.OnNext));
				this.sourceSubscription.Disposable = this.parent.source.Subscribe(this);
				return StableCompositeDisposable.Create(this.timerSubscription, this.sourceSubscription);
			}

			private void OnNext()
			{
				object obj = this.gate;
				lock (obj)
				{
					if (this.isFinished)
					{
						return;
					}
					this.isFinished = true;
				}
				this.sourceSubscription.Dispose();
				try
				{
					this.observer.OnError(new TimeoutException());
				}
				finally
				{
					base.Dispose();
				}
			}

			public override void OnNext(T value)
			{
				object obj = this.gate;
				lock (obj)
				{
					if (!this.isFinished)
					{
						this.observer.OnNext(value);
					}
				}
			}

			public override void OnError(Exception error)
			{
				object obj = this.gate;
				lock (obj)
				{
					if (this.isFinished)
					{
						return;
					}
					this.isFinished = true;
					this.timerSubscription.Dispose();
				}
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
				lock (obj)
				{
					if (!this.isFinished)
					{
						this.isFinished = true;
						this.timerSubscription.Dispose();
					}
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
		}
	}
}
