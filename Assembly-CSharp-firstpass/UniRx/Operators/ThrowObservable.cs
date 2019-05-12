using System;

namespace UniRx.Operators
{
	internal class ThrowObservable<T> : OperatorObservableBase<T>
	{
		private readonly Exception error;

		private readonly IScheduler scheduler;

		public ThrowObservable(Exception error, IScheduler scheduler) : base(scheduler == Scheduler.CurrentThread)
		{
			this.error = error;
			this.scheduler = scheduler;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			observer = new ThrowObservable<T>.Throw(observer, cancel);
			if (this.scheduler == Scheduler.Immediate)
			{
				observer.OnError(this.error);
				return Disposable.Empty;
			}
			return this.scheduler.Schedule(delegate()
			{
				observer.OnError(this.error);
				observer.OnCompleted();
			});
		}

		private class Throw : OperatorObserverBase<T, T>
		{
			public Throw(IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
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
