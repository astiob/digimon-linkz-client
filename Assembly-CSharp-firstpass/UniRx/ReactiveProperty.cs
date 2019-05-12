using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace UniRx
{
	[Serializable]
	public class ReactiveProperty<T> : IReactiveProperty<T>, IDisposable, IOptimizedObservable<T>, IReadOnlyReactiveProperty<T>, IObservable<T>
	{
		private static readonly IEqualityComparer<T> defaultEqualityComparer = UnityEqualityComparer.GetDefault<T>();

		[NonSerialized]
		private bool canPublishValueOnSubscribe;

		[NonSerialized]
		private bool isDisposed;

		[SerializeField]
		private T value;

		[NonSerialized]
		private Subject<T> publisher;

		[NonSerialized]
		private IDisposable sourceConnection;

		[NonSerialized]
		private Exception lastException;

		public ReactiveProperty() : this(default(T))
		{
		}

		public ReactiveProperty(T initialValue)
		{
			this.value = default(T);
			base..ctor();
			this.SetValue(initialValue);
			this.canPublishValueOnSubscribe = true;
		}

		public ReactiveProperty(IObservable<T> source)
		{
			this.value = default(T);
			base..ctor();
			this.canPublishValueOnSubscribe = false;
			this.sourceConnection = source.Subscribe(new ReactiveProperty<T>.ReactivePropertyObserver(this));
		}

		public ReactiveProperty(IObservable<T> source, T initialValue)
		{
			this.value = default(T);
			base..ctor();
			this.canPublishValueOnSubscribe = false;
			this.Value = initialValue;
			this.sourceConnection = source.Subscribe(new ReactiveProperty<T>.ReactivePropertyObserver(this));
		}

		protected virtual IEqualityComparer<T> EqualityComparer
		{
			get
			{
				return ReactiveProperty<T>.defaultEqualityComparer;
			}
		}

		public T Value
		{
			get
			{
				return this.value;
			}
			set
			{
				if (this.canPublishValueOnSubscribe)
				{
					if (!this.EqualityComparer.Equals(this.value, value))
					{
						this.SetValue(value);
						if (this.isDisposed)
						{
							return;
						}
						Subject<T> subject = this.publisher;
						if (subject != null)
						{
							subject.OnNext(this.value);
						}
					}
					return;
				}
				this.canPublishValueOnSubscribe = true;
				this.SetValue(value);
				if (this.isDisposed)
				{
					return;
				}
				Subject<T> subject2 = this.publisher;
				if (subject2 != null)
				{
					subject2.OnNext(this.value);
				}
			}
		}

		public bool HasValue
		{
			get
			{
				return this.canPublishValueOnSubscribe;
			}
		}

		protected virtual void SetValue(T value)
		{
			this.value = value;
		}

		public void SetValueAndForceNotify(T value)
		{
			this.SetValue(value);
			if (this.isDisposed)
			{
				return;
			}
			Subject<T> subject = this.publisher;
			if (subject != null)
			{
				subject.OnNext(this.value);
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

		private class ReactivePropertyObserver : IObserver<T>
		{
			private readonly ReactiveProperty<T> parent;

			private int isStopped;

			public ReactivePropertyObserver(ReactiveProperty<T> parent)
			{
				this.parent = parent;
			}

			public void OnNext(T value)
			{
				this.parent.Value = value;
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
					IDisposable sourceConnection = this.parent.sourceConnection;
					this.parent.sourceConnection = null;
					if (sourceConnection != null)
					{
						sourceConnection.Dispose();
					}
				}
			}
		}
	}
}
