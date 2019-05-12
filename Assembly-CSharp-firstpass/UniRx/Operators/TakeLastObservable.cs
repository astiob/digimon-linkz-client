using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class TakeLastObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly int count;

		private readonly TimeSpan duration;

		private readonly IScheduler scheduler;

		public TakeLastObservable(IObservable<T> source, int count) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.count = count;
		}

		public TakeLastObservable(IObservable<T> source, TimeSpan duration, IScheduler scheduler) : base(scheduler == Scheduler.CurrentThread || source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.duration = duration;
			this.scheduler = scheduler;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			if (this.scheduler == null)
			{
				return new TakeLastObservable<T>.TakeLast(this, observer, cancel).Run();
			}
			return new TakeLastObservable<T>.TakeLast_(this, observer, cancel).Run();
		}

		private class TakeLast : OperatorObserverBase<T, T>
		{
			private readonly TakeLastObservable<T> parent;

			private readonly Queue<T> q;

			public TakeLast(TakeLastObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
				this.q = new Queue<T>();
			}

			public IDisposable Run()
			{
				return this.parent.source.Subscribe(this);
			}

			public override void OnNext(T value)
			{
				this.q.Enqueue(value);
				if (this.q.Count > this.parent.count)
				{
					this.q.Dequeue();
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
				foreach (T value in this.q)
				{
					this.observer.OnNext(value);
				}
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

		private class TakeLast_ : OperatorObserverBase<T, T>
		{
			private DateTimeOffset startTime;

			private readonly TakeLastObservable<T> parent;

			private readonly Queue<TimeInterval<T>> q;

			public TakeLast_(TakeLastObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
				this.q = new Queue<TimeInterval<T>>();
			}

			public IDisposable Run()
			{
				this.startTime = this.parent.scheduler.Now;
				return this.parent.source.Subscribe(this);
			}

			public override void OnNext(T value)
			{
				DateTimeOffset now = this.parent.scheduler.Now;
				TimeSpan timeSpan = now - this.startTime;
				this.q.Enqueue(new TimeInterval<T>(value, timeSpan));
				this.Trim(timeSpan);
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
				DateTimeOffset now = this.parent.scheduler.Now;
				TimeSpan now2 = now - this.startTime;
				this.Trim(now2);
				foreach (TimeInterval<T> timeInterval in this.q)
				{
					this.observer.OnNext(timeInterval.Value);
				}
				try
				{
					this.observer.OnCompleted();
				}
				finally
				{
					base.Dispose();
				}
			}

			private void Trim(TimeSpan now)
			{
				while (this.q.Count > 0 && now - this.q.Peek().Interval >= this.parent.duration)
				{
					this.q.Dequeue();
				}
			}
		}
	}
}
