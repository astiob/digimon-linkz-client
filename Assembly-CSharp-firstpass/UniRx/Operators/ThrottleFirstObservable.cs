using System;

namespace UniRx.Operators
{
	internal class ThrottleFirstObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly TimeSpan dueTime;

		private readonly IScheduler scheduler;

		public ThrottleFirstObservable(IObservable<T> source, TimeSpan dueTime, IScheduler scheduler) : base(scheduler == Scheduler.CurrentThread || source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.dueTime = dueTime;
			this.scheduler = scheduler;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new ThrottleFirstObservable<T>.ThrottleFirst(this, observer, cancel).Run();
		}

		private class ThrottleFirst : OperatorObserverBase<T, T>
		{
			private readonly ThrottleFirstObservable<T> parent;

			private readonly object gate = new object();

			private bool open = true;

			private SerialDisposable cancelable;

			public ThrottleFirst(ThrottleFirstObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.cancelable = new SerialDisposable();
				IDisposable disposable = this.parent.source.Subscribe(this);
				return StableCompositeDisposable.Create(this.cancelable, disposable);
			}

			private void OnNext()
			{
				object obj = this.gate;
				lock (obj)
				{
					this.open = true;
				}
			}

			public override void OnNext(T value)
			{
				object obj = this.gate;
				lock (obj)
				{
					if (!this.open)
					{
						return;
					}
					this.observer.OnNext(value);
					this.open = false;
				}
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				this.cancelable.Disposable = singleAssignmentDisposable;
				singleAssignmentDisposable.Disposable = this.parent.scheduler.Schedule(this.parent.dueTime, new Action(this.OnNext));
			}

			public override void OnError(Exception error)
			{
				this.cancelable.Dispose();
				object obj = this.gate;
				lock (obj)
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
			}

			public override void OnCompleted()
			{
				this.cancelable.Dispose();
				object obj = this.gate;
				lock (obj)
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
}
