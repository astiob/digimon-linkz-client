using System;

namespace UniRx.Operators
{
	internal class ReturnObservable<T> : OperatorObservableBase<T>
	{
		private readonly T value;

		private readonly IScheduler scheduler;

		public ReturnObservable(T value, IScheduler scheduler) : base(scheduler == Scheduler.CurrentThread)
		{
			this.value = value;
			this.scheduler = scheduler;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			observer = new ReturnObservable<T>.Return(observer, cancel);
			if (this.scheduler == Scheduler.Immediate)
			{
				observer.OnNext(this.value);
				observer.OnCompleted();
				return Disposable.Empty;
			}
			return this.scheduler.Schedule(delegate()
			{
				observer.OnNext(this.value);
				observer.OnCompleted();
			});
		}

		private class Return : OperatorObserverBase<T, T>
		{
			public Return(IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
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
