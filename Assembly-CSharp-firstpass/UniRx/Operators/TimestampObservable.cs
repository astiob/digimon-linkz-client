using System;

namespace UniRx.Operators
{
	internal class TimestampObservable<T> : OperatorObservableBase<Timestamped<T>>
	{
		private readonly IObservable<T> source;

		private readonly IScheduler scheduler;

		public TimestampObservable(IObservable<T> source, IScheduler scheduler) : base(scheduler == Scheduler.CurrentThread || source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.scheduler = scheduler;
		}

		protected override IDisposable SubscribeCore(IObserver<Timestamped<T>> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new TimestampObservable<T>.Timestamp(this, observer, cancel));
		}

		private class Timestamp : OperatorObserverBase<T, Timestamped<T>>
		{
			private readonly TimestampObservable<T> parent;

			public Timestamp(TimestampObservable<T> parent, IObserver<Timestamped<T>> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public override void OnNext(T value)
			{
				this.observer.OnNext(new Timestamped<T>(value, this.parent.scheduler.Now));
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
