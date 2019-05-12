using System;

namespace UniRx.Operators
{
	internal class RepeatObservable<T> : OperatorObservableBase<T>
	{
		private readonly T value;

		private readonly int? repeatCount;

		private readonly IScheduler scheduler;

		public RepeatObservable(T value, int? repeatCount, IScheduler scheduler) : base(scheduler == Scheduler.CurrentThread)
		{
			this.value = value;
			this.repeatCount = repeatCount;
			this.scheduler = scheduler;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			RepeatObservable<T>.<SubscribeCore>c__AnonStorey0 <SubscribeCore>c__AnonStorey = new RepeatObservable<T>.<SubscribeCore>c__AnonStorey0();
			<SubscribeCore>c__AnonStorey.observer = observer;
			<SubscribeCore>c__AnonStorey.$this = this;
			<SubscribeCore>c__AnonStorey.observer = new RepeatObservable<T>.Repeat(<SubscribeCore>c__AnonStorey.observer, cancel);
			if (this.repeatCount == null)
			{
				return this.scheduler.Schedule(delegate(Action self)
				{
					<SubscribeCore>c__AnonStorey.observer.OnNext(<SubscribeCore>c__AnonStorey.$this.value);
					self();
				});
			}
			if (this.scheduler == Scheduler.Immediate)
			{
				int num = this.repeatCount.Value;
				for (int i = 0; i < num; i++)
				{
					<SubscribeCore>c__AnonStorey.observer.OnNext(this.value);
				}
				<SubscribeCore>c__AnonStorey.observer.OnCompleted();
				return Disposable.Empty;
			}
			int currentCount = this.repeatCount.Value;
			return this.scheduler.Schedule(delegate(Action self)
			{
				if (currentCount > 0)
				{
					<SubscribeCore>c__AnonStorey.observer.OnNext(<SubscribeCore>c__AnonStorey.$this.value);
					currentCount--;
				}
				if (currentCount == 0)
				{
					<SubscribeCore>c__AnonStorey.observer.OnCompleted();
					return;
				}
				self();
			});
		}

		private class Repeat : OperatorObserverBase<T, T>
		{
			public Repeat(IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
			}

			public override void OnNext(T value)
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
