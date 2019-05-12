using System;

namespace UniRx.Operators
{
	internal class TimerObservable : OperatorObservableBase<long>
	{
		private readonly DateTimeOffset? dueTimeA;

		private readonly TimeSpan? dueTimeB;

		private readonly TimeSpan? period;

		private readonly IScheduler scheduler;

		public TimerObservable(DateTimeOffset dueTime, TimeSpan? period, IScheduler scheduler) : base(scheduler == Scheduler.CurrentThread)
		{
			this.dueTimeA = new DateTimeOffset?(dueTime);
			this.period = period;
			this.scheduler = scheduler;
		}

		public TimerObservable(TimeSpan dueTime, TimeSpan? period, IScheduler scheduler) : base(scheduler == Scheduler.CurrentThread)
		{
			this.dueTimeB = new TimeSpan?(dueTime);
			this.period = period;
			this.scheduler = scheduler;
		}

		protected override IDisposable SubscribeCore(IObserver<long> observer, IDisposable cancel)
		{
			TimerObservable.<SubscribeCore>c__AnonStorey0 <SubscribeCore>c__AnonStorey = new TimerObservable.<SubscribeCore>c__AnonStorey0();
			<SubscribeCore>c__AnonStorey.$this = this;
			<SubscribeCore>c__AnonStorey.timerObserver = new TimerObservable.Timer(observer, cancel);
			TimeSpan timeSpan = (this.dueTimeA == null) ? this.dueTimeB.Value : (this.dueTimeA.Value - this.scheduler.Now);
			if (this.period == null)
			{
				return this.scheduler.Schedule(Scheduler.Normalize(timeSpan), delegate()
				{
					<SubscribeCore>c__AnonStorey.timerObserver.OnNext();
					<SubscribeCore>c__AnonStorey.timerObserver.OnCompleted();
				});
			}
			TimerObservable.<SubscribeCore>c__AnonStorey1 <SubscribeCore>c__AnonStorey2 = new TimerObservable.<SubscribeCore>c__AnonStorey1();
			<SubscribeCore>c__AnonStorey2.periodicScheduler = (this.scheduler as ISchedulerPeriodic);
			if (<SubscribeCore>c__AnonStorey2.periodicScheduler == null)
			{
				TimeSpan timeP = Scheduler.Normalize(this.period.Value);
				return this.scheduler.Schedule(Scheduler.Normalize(timeSpan), delegate(Action<TimeSpan> self)
				{
					<SubscribeCore>c__AnonStorey.timerObserver.OnNext();
					self(timeP);
				});
			}
			if (timeSpan == this.period.Value)
			{
				return <SubscribeCore>c__AnonStorey2.periodicScheduler.SchedulePeriodic(Scheduler.Normalize(timeSpan), new Action(<SubscribeCore>c__AnonStorey.timerObserver.OnNext));
			}
			SerialDisposable disposable = new SerialDisposable();
			disposable.Disposable = this.scheduler.Schedule(Scheduler.Normalize(timeSpan), delegate()
			{
				<SubscribeCore>c__AnonStorey.timerObserver.OnNext();
				TimeSpan timeSpan2 = Scheduler.Normalize(<SubscribeCore>c__AnonStorey.$this.period.Value);
				disposable.Disposable = <SubscribeCore>c__AnonStorey2.periodicScheduler.SchedulePeriodic(timeSpan2, new Action(<SubscribeCore>c__AnonStorey.timerObserver.OnNext));
			});
			return disposable;
		}

		private class Timer : OperatorObserverBase<long, long>
		{
			private long index;

			public Timer(IObserver<long> observer, IDisposable cancel) : base(observer, cancel)
			{
			}

			public void OnNext()
			{
				try
				{
					IObserver<long> observer = this.observer;
					long value;
					this.index = (value = this.index) + 1L;
					observer.OnNext(value);
				}
				catch
				{
					base.Dispose();
					throw;
				}
			}

			public override void OnNext(long value)
			{
			}

			public override void OnError(Exception error)
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

			public override void OnCompleted()
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
}
