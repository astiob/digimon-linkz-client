using System;

namespace UniRx.Operators
{
	internal class RangeObservable : OperatorObservableBase<int>
	{
		private readonly int start;

		private readonly int count;

		private readonly IScheduler scheduler;

		public RangeObservable(int start, int count, IScheduler scheduler) : base(scheduler == Scheduler.CurrentThread)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count < 0");
			}
			this.start = start;
			this.count = count;
			this.scheduler = scheduler;
		}

		protected override IDisposable SubscribeCore(IObserver<int> observer, IDisposable cancel)
		{
			RangeObservable.<SubscribeCore>c__AnonStorey1 <SubscribeCore>c__AnonStorey = new RangeObservable.<SubscribeCore>c__AnonStorey1();
			<SubscribeCore>c__AnonStorey.observer = observer;
			<SubscribeCore>c__AnonStorey.$this = this;
			<SubscribeCore>c__AnonStorey.observer = new RangeObservable.Range(<SubscribeCore>c__AnonStorey.observer, cancel);
			if (this.scheduler == Scheduler.Immediate)
			{
				for (int j = 0; j < this.count; j++)
				{
					int value = this.start + j;
					<SubscribeCore>c__AnonStorey.observer.OnNext(value);
				}
				<SubscribeCore>c__AnonStorey.observer.OnCompleted();
				return Disposable.Empty;
			}
			int i = 0;
			return this.scheduler.Schedule(delegate(Action self)
			{
				if (i < <SubscribeCore>c__AnonStorey.$this.count)
				{
					int value2 = <SubscribeCore>c__AnonStorey.$this.start + i;
					<SubscribeCore>c__AnonStorey.observer.OnNext(value2);
					i++;
					self();
				}
				else
				{
					<SubscribeCore>c__AnonStorey.observer.OnCompleted();
				}
			});
		}

		private class Range : OperatorObserverBase<int, int>
		{
			public Range(IObserver<int> observer, IDisposable cancel) : base(observer, cancel)
			{
			}

			public override void OnNext(int value)
			{
				try
				{
					this.observer.OnNext(value);
				}
				catch
				{
					base.Dispose();
					throw;
				}
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
