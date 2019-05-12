using System;

namespace UniRx.Operators
{
	internal class StartObservable<T> : OperatorObservableBase<T>
	{
		private readonly Action action;

		private readonly Func<T> function;

		private readonly IScheduler scheduler;

		private readonly TimeSpan? startAfter;

		public StartObservable(Func<T> function, TimeSpan? startAfter, IScheduler scheduler) : base(scheduler == Scheduler.CurrentThread)
		{
			this.function = function;
			this.startAfter = startAfter;
			this.scheduler = scheduler;
		}

		public StartObservable(Action action, TimeSpan? startAfter, IScheduler scheduler) : base(scheduler == Scheduler.CurrentThread)
		{
			this.action = action;
			this.startAfter = startAfter;
			this.scheduler = scheduler;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			if (this.startAfter != null)
			{
				return this.scheduler.Schedule(this.startAfter.Value, new Action(new StartObservable<T>.StartObserver(this, observer, cancel).Run));
			}
			return this.scheduler.Schedule(new Action(new StartObservable<T>.StartObserver(this, observer, cancel).Run));
		}

		private class StartObserver : OperatorObserverBase<T, T>
		{
			private readonly StartObservable<T> parent;

			public StartObserver(StartObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public void Run()
			{
				T value = default(T);
				try
				{
					if (this.parent.function != null)
					{
						value = this.parent.function();
					}
					else
					{
						this.parent.action();
					}
				}
				catch (Exception error)
				{
					try
					{
						this.observer.OnError(error);
					}
					finally
					{
						base.Dispose();
					}
					return;
				}
				this.OnNext(value);
				try
				{
					this.observer.OnCompleted();
				}
				finally
				{
					base.Dispose();
				}
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
