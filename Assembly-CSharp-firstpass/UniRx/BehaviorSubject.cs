using System;
using UniRx.InternalUtil;

namespace UniRx
{
	public sealed class BehaviorSubject<T> : ISubject<T>, IDisposable, IOptimizedObservable<T>, ISubject<T, T>, IObserver<T>, IObservable<T>
	{
		private object observerLock = new object();

		private bool isStopped;

		private bool isDisposed;

		private T lastValue;

		private Exception lastError;

		private IObserver<T> outObserver = EmptyObserver<T>.Instance;

		public BehaviorSubject(T defaultValue)
		{
			this.lastValue = defaultValue;
		}

		public T Value
		{
			get
			{
				this.ThrowIfDisposed();
				if (this.lastError != null)
				{
					throw this.lastError;
				}
				return this.lastValue;
			}
		}

		public bool HasObservers
		{
			get
			{
				return !(this.outObserver is EmptyObserver<T>) && !this.isStopped && !this.isDisposed;
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
			}
			observer.OnError(error);
		}

		public void OnNext(T value)
		{
			object obj = this.observerLock;
			IObserver<T> observer;
			lock (obj)
			{
				if (this.isStopped)
				{
					return;
				}
				this.lastValue = value;
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
			T value = default(T);
			BehaviorSubject<T>.Subscription subscription = null;
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
					value = this.lastValue;
					subscription = new BehaviorSubject<T>.Subscription(this, observer);
				}
				else
				{
					ex = this.lastError;
				}
			}
			if (subscription != null)
			{
				observer.OnNext(value);
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
				this.lastValue = default(T);
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

			private BehaviorSubject<T> parent;

			private IObserver<T> unsubscribeTarget;

			public Subscription(BehaviorSubject<T> parent, IObserver<T> unsubscribeTarget)
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
