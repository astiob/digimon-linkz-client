using System;
using UniRx.InternalUtil;

namespace UniRx
{
	public sealed class AsyncSubject<T> : ISubject<T>, IOptimizedObservable<T>, IDisposable, ISubject<T, T>, IObserver<T>, IObservable<T>
	{
		private object observerLock = new object();

		private T lastValue;

		private bool hasValue;

		private bool isStopped;

		private bool isDisposed;

		private Exception lastError;

		private IObserver<T> outObserver = EmptyObserver<T>.Instance;

		public T Value
		{
			get
			{
				this.ThrowIfDisposed();
				if (!this.isStopped)
				{
					throw new InvalidOperationException("AsyncSubject is not completed yet");
				}
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

		public bool IsCompleted
		{
			get
			{
				return this.isStopped;
			}
		}

		public void OnCompleted()
		{
			object obj = this.observerLock;
			IObserver<T> observer;
			T value;
			bool flag;
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
				value = this.lastValue;
				flag = this.hasValue;
			}
			if (flag)
			{
				observer.OnNext(value);
				observer.OnCompleted();
			}
			else
			{
				observer.OnCompleted();
			}
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
			lock (obj)
			{
				this.ThrowIfDisposed();
				if (!this.isStopped)
				{
					this.hasValue = true;
					this.lastValue = value;
				}
			}
		}

		public IDisposable Subscribe(IObserver<T> observer)
		{
			if (observer == null)
			{
				throw new ArgumentNullException("observer");
			}
			Exception ex = null;
			T value = default(T);
			bool flag = false;
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
					return new AsyncSubject<T>.Subscription(this, observer);
				}
				ex = this.lastError;
				value = this.lastValue;
				flag = this.hasValue;
			}
			if (ex != null)
			{
				observer.OnError(ex);
			}
			else if (flag)
			{
				observer.OnNext(value);
				observer.OnCompleted();
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

			private AsyncSubject<T> parent;

			private IObserver<T> unsubscribeTarget;

			public Subscription(AsyncSubject<T> parent, IObserver<T> unsubscribeTarget)
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
