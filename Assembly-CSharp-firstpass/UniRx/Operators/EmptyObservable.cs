using System;

namespace UniRx.Operators
{
	internal class EmptyObservable<T> : OperatorObservableBase<T>
	{
		private readonly IScheduler scheduler;

		public EmptyObservable(IScheduler scheduler) : base(false)
		{
			this.scheduler = scheduler;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			observer = new EmptyObservable<T>.Empty(observer, cancel);
			if (this.scheduler == Scheduler.Immediate)
			{
				observer.OnCompleted();
				return Disposable.Empty;
			}
			return this.scheduler.Schedule(new Action(observer.OnCompleted));
		}

		private class Empty : OperatorObserverBase<T, T>
		{
			public Empty(IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
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
