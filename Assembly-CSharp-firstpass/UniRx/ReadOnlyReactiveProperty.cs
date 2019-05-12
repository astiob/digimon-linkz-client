using System;
using System.Collections.Generic;
using System.Threading;

namespace UniRx
{
	public class ReadOnlyReactiveProperty<T> : IReadOnlyReactiveProperty<T>, IDisposable, IOptimizedObservable<T>, IObservable<T>
	{
		private static readonly IEqualityComparer<T> defaultEqualityComparer = UnityEqualityComparer.GetDefault<T>();

		private readonly bool distinctUntilChanged = true;

		private bool canPublishValueOnSubscribe;

		private bool isDisposed;

		private Exception lastException;

		private T value = default(T);

		private Subject<T> publisher;

		private IDisposable sourceConnection;

		private bool isSourceCompleted;

		public ReadOnlyReactiveProperty(IObservable<T> source)
		{
			this.sourceConnection = source.Subscribe(new ReadOnlyReactiveProperty<T>.ReadOnlyReactivePropertyObserver(this));
		}

		public ReadOnlyReactiveProperty(IObservable<T> source, bool distinctUntilChanged)
		{
			this.distinctUntilChanged = distinctUntilChanged;
			this.sourceConnection = source.Subscribe(new ReadOnlyReactiveProperty<T>.ReadOnlyReactivePropertyObserver(this));
		}

		public ReadOnlyReactiveProperty(IObservable<T> source, T initialValue)
		{
			this.value = initialValue;
			this.canPublishValueOnSubscribe = true;
			this.sourceConnection = source.Subscribe(new ReadOnlyReactiveProperty<T>.ReadOnlyReactivePropertyObserver(this));
		}

		public ReadOnlyReactiveProperty(IObservable<T> source, T initialValue, bool distinctUntilChanged)
		{
			this.distinctUntilChanged = distinctUntilChanged;
			this.value = initialValue;
			this.canPublishValueOnSubscribe = true;
			this.sourceConnection = source.Subscribe(new ReadOnlyReactiveProperty<T>.ReadOnlyReactivePropertyObserver(this));
		}

		public T Value
		{
			get
			{
				return this.value;
			}
		}

		public bool HasValue
		{
			get
			{
				return this.canPublishValueOnSubscribe;
			}
		}

		protected virtual IEqualityComparer<T> EqualityComparer
		{
			get
			{
				return ReadOnlyReactiveProperty<T>.defaultEqualityComparer;
			}
		}

		public IDisposable Subscribe(IObserver<T> observer)
		{
			if (this.lastException != null)
			{
				observer.OnError(this.lastException);
				return Disposable.Empty;
			}
			if (this.isDisposed)
			{
				observer.OnCompleted();
				return Disposable.Empty;
			}
			if (this.isSourceCompleted)
			{
				if (this.canPublishValueOnSubscribe)
				{
					observer.OnNext(this.value);
					observer.OnCompleted();
					return Disposable.Empty;
				}
				observer.OnCompleted();
				return Disposable.Empty;
			}
			else
			{
				if (this.publisher == null)
				{
					this.publisher = new Subject<T>();
				}
				Subject<T> subject = this.publisher;
				if (subject != null)
				{
					IDisposable result = subject.Subscribe(observer);
					if (this.canPublishValueOnSubscribe)
					{
						observer.OnNext(this.value);
					}
					return result;
				}
				observer.OnCompleted();
				return Disposable.Empty;
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.isDisposed)
			{
				this.isDisposed = true;
				IDisposable disposable = this.sourceConnection;
				if (disposable != null)
				{
					disposable.Dispose();
					this.sourceConnection = null;
				}
				Subject<T> subject = this.publisher;
				if (subject != null)
				{
					try
					{
						subject.OnCompleted();
					}
					finally
					{
						subject.Dispose();
						this.publisher = null;
					}
				}
			}
		}

		public override string ToString()
		{
			return (this.value != null) ? this.value.ToString() : "(null)";
		}

		public bool IsRequiredSubscribeOnCurrentThread()
		{
			return false;
		}

		private class ReadOnlyReactivePropertyObserver : IObserver<T>
		{
			private readonly ReadOnlyReactiveProperty<T> parent;

			private int isStopped;

			public ReadOnlyReactivePropertyObserver(ReadOnlyReactiveProperty<T> parent)
			{
				this.parent = parent;
			}

			public void OnNext(T value)
			{
				if (this.parent.distinctUntilChanged && this.parent.canPublishValueOnSubscribe)
				{
					if (!this.parent.EqualityComparer.Equals(this.parent.value, value))
					{
						this.parent.value = value;
						Subject<T> publisher = this.parent.publisher;
						if (publisher != null)
						{
							publisher.OnNext(value);
						}
					}
				}
				else
				{
					this.parent.value = value;
					this.parent.canPublishValueOnSubscribe = true;
					Subject<T> publisher2 = this.parent.publisher;
					if (publisher2 != null)
					{
						publisher2.OnNext(value);
					}
				}
			}

			public void OnError(Exception error)
			{
				if (Interlocked.Increment(ref this.isStopped) == 1)
				{
					this.parent.lastException = error;
					Subject<T> publisher = this.parent.publisher;
					if (publisher != null)
					{
						publisher.OnError(error);
					}
					this.parent.Dispose();
				}
			}

			public void OnCompleted()
			{
				if (Interlocked.Increment(ref this.isStopped) == 1)
				{
					this.parent.isSourceCompleted = true;
					IDisposable sourceConnection = this.parent.sourceConnection;
					this.parent.sourceConnection = null;
					if (sourceConnection != null)
					{
						sourceConnection.Dispose();
					}
					Subject<T> publisher = this.parent.publisher;
					this.parent.publisher = null;
					if (publisher != null)
					{
						try
						{
							publisher.OnCompleted();
						}
						finally
						{
							publisher.Dispose();
						}
					}
				}
			}
		}
	}
}
