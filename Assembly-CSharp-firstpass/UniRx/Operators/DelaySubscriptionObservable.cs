using System;

namespace UniRx.Operators
{
	internal class DelaySubscriptionObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly IScheduler scheduler;

		private readonly TimeSpan? dueTimeT;

		private readonly DateTimeOffset? dueTimeD;

		public DelaySubscriptionObservable(IObservable<T> source, TimeSpan dueTime, IScheduler scheduler) : base(scheduler == Scheduler.CurrentThread || source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.scheduler = scheduler;
			this.dueTimeT = new TimeSpan?(dueTime);
		}

		public DelaySubscriptionObservable(IObservable<T> source, DateTimeOffset dueTime, IScheduler scheduler) : base(scheduler == Scheduler.CurrentThread || source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.scheduler = scheduler;
			this.dueTimeD = new DateTimeOffset?(dueTime);
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			DelaySubscriptionObservable<T>.<SubscribeCore>c__AnonStorey0 <SubscribeCore>c__AnonStorey = new DelaySubscriptionObservable<T>.<SubscribeCore>c__AnonStorey0();
			<SubscribeCore>c__AnonStorey.observer = observer;
			<SubscribeCore>c__AnonStorey.$this = this;
			MultipleAssignmentDisposable d;
			if (this.dueTimeT != null)
			{
				MultipleAssignmentDisposable d = new MultipleAssignmentDisposable();
				TimeSpan dueTime = Scheduler.Normalize(this.dueTimeT.Value);
				d.Disposable = this.scheduler.Schedule(dueTime, delegate()
				{
					d.Disposable = <SubscribeCore>c__AnonStorey.$this.source.Subscribe(<SubscribeCore>c__AnonStorey.observer);
				});
				return d;
			}
			d = new MultipleAssignmentDisposable();
			d.Disposable = this.scheduler.Schedule(this.dueTimeD.Value, delegate()
			{
				d.Disposable = <SubscribeCore>c__AnonStorey.$this.source.Subscribe(<SubscribeCore>c__AnonStorey.observer);
			});
			return d;
		}
	}
}
