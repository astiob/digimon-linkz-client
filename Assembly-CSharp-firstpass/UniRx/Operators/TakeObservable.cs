using System;

namespace UniRx.Operators
{
	internal class TakeObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly int count;

		private readonly TimeSpan duration;

		internal readonly IScheduler scheduler;

		public TakeObservable(IObservable<T> source, int count) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.count = count;
		}

		public TakeObservable(IObservable<T> source, TimeSpan duration, IScheduler scheduler) : base(scheduler == Scheduler.CurrentThread || source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.duration = duration;
			this.scheduler = scheduler;
		}

		public IObservable<T> Combine(int count)
		{
			return (this.count > count) ? new TakeObservable<T>(this.source, count) : this;
		}

		public IObservable<T> Combine(TimeSpan duration)
		{
			return (!(this.duration <= duration)) ? new TakeObservable<T>(this.source, duration, this.scheduler) : this;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			if (this.scheduler == null)
			{
				return this.source.Subscribe(new TakeObservable<T>.Take(this, observer, cancel));
			}
			return new TakeObservable<T>.Take_(this, observer, cancel).Run();
		}

		private class Take : OperatorObserverBase<T, T>
		{
			private int rest;

			public Take(TakeObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.rest = parent.count;
			}

			public override void OnNext(T value)
			{
				if (this.rest > 0)
				{
					this.rest--;
					this.observer.OnNext(value);
					if (this.rest == 0)
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

		private class Take_ : OperatorObserverBase<T, T>
		{
			private readonly TakeObservable<T> parent;

			private readonly object gate = new object();

			public Take_(TakeObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
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

			public override void OnNext(T value)
			{
				object obj = this.gate;
				lock (obj)
				{
					this.observer.OnNext(value);
				}
			}

			public override void OnError(Exception error)
			{
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
