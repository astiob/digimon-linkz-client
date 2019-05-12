using System;
using System.Collections.Generic;
using UniRx.InternalUtil;

namespace UniRx
{
	public sealed class ReplaySubject<T> : ISubject<T>, IOptimizedObservable<T>, IDisposable, ISubject<T, T>, IObserver<T>, IObservable<T>
	{
		private object observerLock = new object();

		private bool isStopped;

		private bool isDisposed;

		private Exception lastError;

		private IObserver<T> outObserver = EmptyObserver<T>.Instance;

		private readonly int bufferSize;

		private readonly TimeSpan window;

		private readonly DateTimeOffset startTime;

		private readonly IScheduler scheduler;

		private Queue<TimeInterval<T>> queue = new Queue<TimeInterval<T>>();

		public ReplaySubject() : this(int.MaxValue, TimeSpan.MaxValue, Scheduler.DefaultSchedulers.Iteration)
		{
		}

		public ReplaySubject(IScheduler scheduler) : this(int.MaxValue, TimeSpan.MaxValue, scheduler)
		{
		}

		public ReplaySubject(int bufferSize) : this(bufferSize, TimeSpan.MaxValue, Scheduler.DefaultSchedulers.Iteration)
		{
		}

		public ReplaySubject(int bufferSize, IScheduler scheduler) : this(bufferSize, TimeSpan.MaxValue, scheduler)
		{
		}

		public ReplaySubject(TimeSpan window) : this(int.MaxValue, window, Scheduler.DefaultSchedulers.Iteration)
		{
		}

		public ReplaySubject(TimeSpan window, IScheduler scheduler) : this(int.MaxValue, window, scheduler)
		{
		}

		public ReplaySubject(int bufferSize, TimeSpan window, IScheduler scheduler)
		{
			if (bufferSize < 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize");
			}
			if (window < TimeSpan.Zero)
			{
				throw new ArgumentOutOfRangeException("window");
			}
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			this.bufferSize = bufferSize;
			this.window = window;
			this.scheduler = scheduler;
			this.startTime = scheduler.Now;
		}

		private void Trim()
		{
			TimeSpan timeSpan = Scheduler.Normalize(this.scheduler.Now - this.startTime);
			while (this.queue.Count > this.bufferSize)
			{
				this.queue.Dequeue();
			}
			while (this.queue.Count > 0 && timeSpan.Subtract(this.queue.Peek().Interval).CompareTo(this.window) > 0)
			{
				this.queue.Dequeue();
			}
		}

		public void OnCompleted()
		{
			object obj = this.observerLock;
			IObserver<T> observer;
			lock (obj)
			{
				this.ThrowIfDisposed();
				if (this.isStopped)
				{
					return;
				}
				observer = this.outObserver;
				this.outObserver = EmptyObserver<T>.Instance;
				this.isStopped = true;
				this.Trim();
			}
			observer.OnCompleted();
		}

		public void OnError(Exception error)
		{
			if (error == null)
			{
				throw new ArgumentNullException("error");
			}
			object obj = this.observerLock;
			IObserver<T> observer;
			lock (obj)
			{
				this.ThrowIfDisposed();
				if (this.isStopped)
				{
					return;
				}
				observer = this.outObserver;
				this.outObserver = EmptyObserver<T>.Instance;
				this.isStopped = true;
				this.lastError = error;
				this.Trim();
			}
			observer.OnError(error);
		}

		public void OnNext(T value)
		{
			object obj = this.observerLock;
			IObserver<T> observer;
			lock (obj)
			{
				this.ThrowIfDisposed();
				if (this.isStopped)
				{
					return;
				}
				this.queue.Enqueue(new TimeInterval<T>(value, this.scheduler.Now - this.startTime));
				this.Trim();
				observer = this.outObserver;
			}
			observer.OnNext(value);
		}

		public IDisposable Subscribe(IObserver<T> observer)
		{
			if (observer == null)
			{
				throw new ArgumentNullException("observer");
			}
			Exception ex = null;
			ReplaySubject<T>.Subscription subscription = null;
			object obj = this.observerLock;
			lock (obj)
			{
				this.ThrowIfDisposed();
				if (!this.isStopped)
				{
					ListObserver<T> listObserver = this.outObserver as ListObserver<T>;
					if (listObserver != null)
					{
						this.outObserver = listObserver.Add(observer);
					}
					else
					{
						IObserver<T> observer2 = this.outObserver;
						if (observer2 is EmptyObserver<T>)
						{
							this.outObserver = observer;
						}
						else
						{
							this.outObserver = new ListObserver<T>(new ImmutableList<IObserver<T>>(new IObserver<T>[]
							{
								observer2,
								observer
							}));
						}
					}
					subscription = new ReplaySubject<T>.Subscription(this, observer);
				}
				ex = this.lastError;
				this.Trim();
				foreach (TimeInterval<T> timeInterval in this.queue)
				{
					observer.OnNext(timeInterval.Value);
				}
			}
			if (subscription != null)
			{
				return subscription;
			}
			if (ex != null)
			{
				observer.OnError(ex);
			}
			else
			{
				observer.OnCompleted();
			}
			return Disposable.Empty;
		}

		public void Dispose()
		{
			object obj = this.observerLock;
			lock (obj)
			{
				this.isDisposed = true;
				this.outObserver = DisposedObserver<T>.Instance;
				this.lastError = null;
				this.queue = null;
			}
		}

		private void ThrowIfDisposed()
		{
			if (this.isDisposed)
			{
				throw new ObjectDisposedException(string.Empty);
			}
		}

		public bool IsRequiredSubscribeOnCurrentThread()
		{
			return false;
		}

		private class Subscription : IDisposable
		{
			private readonly object gate = new object();

			private ReplaySubject<T> parent;

			private IObserver<T> unsubscribeTarget;

			public Subscription(ReplaySubject<T> parent, IObserver<T> unsubscribeTarget)
			{
				this.parent = parent;
				this.unsubscribeTarget = unsubscribeTarget;
			}

			public void Dispose()
			{
				object obj = this.gate;
				lock (obj)
				{
					if (this.parent != null)
					{
						object observerLock = this.parent.observerLock;
						lock (observerLock)
						{
							ListObserver<T> listObserver = this.parent.outObserver as ListObserver<T>;
							if (listObserver != null)
							{
								this.parent.outObserver = listObserver.Remove(this.unsubscribeTarget);
							}
							else
							{
								this.parent.outObserver = EmptyObserver<T>.Instance;
							}
							this.unsubscribeTarget = null;
							this.parent = null;
						}
					}
				}
			}
		}
	}
}
