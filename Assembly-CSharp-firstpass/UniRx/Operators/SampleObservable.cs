using System;

namespace UniRx.Operators
{
	internal class SampleObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly TimeSpan interval;

		private readonly IScheduler scheduler;

		public SampleObservable(IObservable<T> source, TimeSpan interval, IScheduler scheduler) : base(source.IsRequiredSubscribeOnCurrentThread<T>() || scheduler == Scheduler.CurrentThread)
		{
			this.source = source;
			this.interval = interval;
			this.scheduler = scheduler;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new SampleObservable<T>.Sample(this, observer, cancel).Run();
		}

		private class Sample : OperatorObserverBase<T, T>
		{
			private readonly SampleObservable<T> parent;

			private readonly object gate = new object();

			private T latestValue = default(T);

			private bool isUpdated;

			private bool isCompleted;

			private SingleAssignmentDisposable sourceSubscription;

			public Sample(SampleObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.sourceSubscription = new SingleAssignmentDisposable();
				this.sourceSubscription.Disposable = this.parent.source.Subscribe(this);
				ISchedulerPeriodic schedulerPeriodic = this.parent.scheduler as ISchedulerPeriodic;
				IDisposable disposable;
				if (schedulerPeriodic != null)
				{
					disposable = schedulerPeriodic.SchedulePeriodic(this.parent.interval, new Action(this.OnNextTick));
				}
				else
				{
					disposable = this.parent.scheduler.Schedule(this.parent.interval, new Action<Action<TimeSpan>>(this.OnNextRecursive));
				}
				return StableCompositeDisposable.Create(this.sourceSubscription, disposable);
			}

			private void OnNextTick()
			{
				object obj = this.gate;
				lock (obj)
				{
					if (this.isUpdated)
					{
						T value = this.latestValue;
						this.isUpdated = false;
						this.observer.OnNext(value);
					}
					if (this.isCompleted)
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
			}

			private void OnNextRecursive(Action<TimeSpan> self)
			{
				object obj = this.gate;
				lock (obj)
				{
					if (this.isUpdated)
					{
						T value = this.latestValue;
						this.isUpdated = false;
						this.observer.OnNext(value);
					}
					if (this.isCompleted)
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
				self(this.parent.interval);
			}

			public override void OnNext(T value)
			{
				object obj = this.gate;
				lock (obj)
				{
					this.latestValue = value;
					this.isUpdated = true;
				}
			}

			public override void OnError(Exception error)
			{
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
				object obj = this.gate;
				lock (obj)
				{
					this.isCompleted = true;
					this.sourceSubscription.Dispose();
				}
			}
		}
	}
}
