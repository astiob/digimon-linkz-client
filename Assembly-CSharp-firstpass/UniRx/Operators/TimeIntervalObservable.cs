using System;

namespace UniRx.Operators
{
	internal class TimeIntervalObservable<T> : OperatorObservableBase<TimeInterval<T>>
	{
		private readonly IObservable<T> source;

		private readonly IScheduler scheduler;

		public TimeIntervalObservable(IObservable<T> source, IScheduler scheduler) : base(scheduler == Scheduler.CurrentThread || source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.scheduler = scheduler;
		}

		protected override IDisposable SubscribeCore(IObserver<TimeInterval<T>> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new TimeIntervalObservable<T>.TimeInterval(this, observer, cancel));
		}

		private class TimeInterval : OperatorObserverBase<T, TimeInterval<T>>
		{
			private readonly TimeIntervalObservable<T> parent;

			private DateTimeOffset lastTime;

			public TimeInterval(TimeIntervalObservable<T> parent, IObserver<TimeInterval<T>> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
				this.lastTime = parent.scheduler.Now;
			}

			public override void OnNext(T value)
			{
				DateTimeOffset now = this.parent.scheduler.Now;
				TimeSpan interval = now.Subtract(this.lastTime);
				this.lastTime = now;
				this.observer.OnNext(new TimeInterval<T>(value, interval));
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
