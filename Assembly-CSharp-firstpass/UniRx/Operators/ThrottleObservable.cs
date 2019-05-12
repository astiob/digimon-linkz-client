using System;

namespace UniRx.Operators
{
	internal class ThrottleObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly TimeSpan dueTime;

		private readonly IScheduler scheduler;

		public ThrottleObservable(IObservable<T> source, TimeSpan dueTime, IScheduler scheduler) : base(scheduler == Scheduler.CurrentThread || source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.dueTime = dueTime;
			this.scheduler = scheduler;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new ThrottleObservable<T>.Throttle(this, observer, cancel).Run();
		}

		private class Throttle : OperatorObserverBase<T, T>
		{
			private readonly ThrottleObservable<T> parent;

			private readonly object gate = new object();

			private T latestValue = default(T);

			private bool hasValue;

			private SerialDisposable cancelable;

			private ulong id;

			public Throttle(ThrottleObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.cancelable = new SerialDisposable();
				IDisposable disposable = this.parent.source.Subscribe(this);
				return StableCompositeDisposable.Create(this.cancelable, disposable);
			}

			private void OnNext(ulong currentid)
			{
				object obj = this.gate;
				lock (obj)
				{
					if (this.hasValue && this.id == currentid)
					{
						this.observer.OnNext(this.latestValue);
					}
					this.hasValue = false;
				}
			}

			public override void OnNext(T value)
			{
				object obj = this.gate;
				ulong currentid;
				lock (obj)
				{
					this.hasValue = true;
					this.latestValue = value;
					this.id += 1UL;
					currentid = this.id;
				}
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				this.cancelable.Disposable = singleAssignmentDisposable;
				singleAssignmentDisposable.Disposable = this.parent.scheduler.Schedule(this.parent.dueTime, delegate()
				{
					this.OnNext(currentid);
				});
			}

			public override void OnError(Exception error)
			{
				this.cancelable.Dispose();
				object obj = this.gate;
				lock (obj)
				{
					this.hasValue = false;
					this.id += 1UL;
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
					if (this.hasValue)
					{
						this.observer.OnNext(this.latestValue);
					}
					this.hasValue = false;
					this.id += 1UL;
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
