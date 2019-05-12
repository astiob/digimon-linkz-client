using System;

namespace UniRx.Operators
{
	internal class SkipObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly int count;

		private readonly TimeSpan duration;

		internal readonly IScheduler scheduler;

		public SkipObservable(IObservable<T> source, int count) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.count = count;
		}

		public SkipObservable(IObservable<T> source, TimeSpan duration, IScheduler scheduler) : base(scheduler == Scheduler.CurrentThread || source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.duration = duration;
			this.scheduler = scheduler;
		}

		public IObservable<T> Combine(int count)
		{
			return new SkipObservable<T>(this.source, this.count + count);
		}

		public IObservable<T> Combine(TimeSpan duration)
		{
			return (!(duration <= this.duration)) ? new SkipObservable<T>(this.source, duration, this.scheduler) : this;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			if (this.scheduler == null)
			{
				return this.source.Subscribe(new SkipObservable<T>.Skip(this, observer, cancel));
			}
			return new SkipObservable<T>.Skip_(this, observer, cancel).Run();
		}

		private class Skip : OperatorObserverBase<T, T>
		{
			private int remaining;

			public Skip(SkipObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.remaining = parent.count;
			}

			public override void OnNext(T value)
			{
				if (this.remaining <= 0)
				{
					this.observer.OnNext(value);
				}
				else
				{
					this.remaining--;
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

		private class Skip_ : OperatorObserverBase<T, T>
		{
			private readonly SkipObservable<T> parent;

			private volatile bool open;

			public Skip_(SkipObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				IDisposable disposable = this.parent.scheduler.Schedule(this.parent.duration, new Action(this.Tick));
				IDisposable disposable2 = this.parent.source.Subscribe(this);
				return StableCompositeDisposable.Create(disposable, disposable2);
			}

			private void Tick()
			{
				this.open = true;
			}

			public override void OnNext(T value)
			{
				if (this.open)
				{
					this.observer.OnNext(value);
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
