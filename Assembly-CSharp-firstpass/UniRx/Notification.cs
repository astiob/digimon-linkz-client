using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace UniRx
{
	[Serializable]
	public abstract class Notification<T> : IEquatable<Notification<T>>
	{
		protected internal Notification()
		{
		}

		public abstract T Value { get; }

		public abstract bool HasValue { get; }

		public abstract Exception Exception { get; }

		public abstract NotificationKind Kind { get; }

		public abstract bool Equals(Notification<T> other);

		public static bool operator ==(Notification<T> left, Notification<T> right)
		{
			return object.ReferenceEquals(left, right) || (left != null && right != null && left.Equals(right));
		}

		public static bool operator !=(Notification<T> left, Notification<T> right)
		{
			return !(left == right);
		}

		public override bool Equals(object obj)
		{
			return this.Equals(obj as Notification<T>);
		}

		public abstract void Accept(IObserver<T> observer);

		public abstract TResult Accept<TResult>(IObserver<T, TResult> observer);

		public abstract void Accept(Action<T> onNext, Action<Exception> onError, Action onCompleted);

		public abstract TResult Accept<TResult>(Func<T, TResult> onNext, Func<Exception, TResult> onError, Func<TResult> onCompleted);

		public IObservable<T> ToObservable()
		{
			return this.ToObservable(Scheduler.Immediate);
		}

		public IObservable<T> ToObservable(IScheduler scheduler)
		{
			Notification<T>.<ToObservable>c__AnonStorey0 <ToObservable>c__AnonStorey = new Notification<T>.<ToObservable>c__AnonStorey0();
			<ToObservable>c__AnonStorey.scheduler = scheduler;
			<ToObservable>c__AnonStorey.$this = this;
			if (<ToObservable>c__AnonStorey.scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			return Observable.Create<T>((IObserver<T> observer) => <ToObservable>c__AnonStorey.scheduler.Schedule(delegate()
			{
				<ToObservable>c__AnonStorey.Accept(observer);
				if (<ToObservable>c__AnonStorey.Kind == NotificationKind.OnNext)
				{
					observer.OnCompleted();
				}
			}));
		}

		[DebuggerDisplay("OnNext({Value})")]
		[Serializable]
		internal sealed class OnNextNotification : Notification<T>
		{
			private T value;

			public OnNextNotification(T value)
			{
				this.value = value;
			}

			public override T Value
			{
				get
				{
					return this.value;
				}
			}

			public override Exception Exception
			{
				get
				{
					return null;
				}
			}

			public override bool HasValue
			{
				get
				{
					return true;
				}
			}

			public override NotificationKind Kind
			{
				get
				{
					return NotificationKind.OnNext;
				}
			}

			public override int GetHashCode()
			{
				return EqualityComparer<T>.Default.GetHashCode(this.Value);
			}

			public override bool Equals(Notification<T> other)
			{
				return object.ReferenceEquals(this, other) || (!object.ReferenceEquals(other, null) && other.Kind == NotificationKind.OnNext && EqualityComparer<T>.Default.Equals(this.Value, other.Value));
			}

			public override string ToString()
			{
				return string.Format(CultureInfo.CurrentCulture, "OnNext({0})", new object[]
				{
					this.Value
				});
			}

			public override void Accept(IObserver<T> observer)
			{
				if (observer == null)
				{
					throw new ArgumentNullException("observer");
				}
				observer.OnNext(this.Value);
			}

			public override TResult Accept<TResult>(IObserver<T, TResult> observer)
			{
				if (observer == null)
				{
					throw new ArgumentNullException("observer");
				}
				return observer.OnNext(this.Value);
			}

			public override void Accept(Action<T> onNext, Action<Exception> onError, Action onCompleted)
			{
				if (onNext == null)
				{
					throw new ArgumentNullException("onNext");
				}
				if (onError == null)
				{
					throw new ArgumentNullException("onError");
				}
				if (onCompleted == null)
				{
					throw new ArgumentNullException("onCompleted");
				}
				onNext(this.Value);
			}

			public override TResult Accept<TResult>(Func<T, TResult> onNext, Func<Exception, TResult> onError, Func<TResult> onCompleted)
			{
				if (onNext == null)
				{
					throw new ArgumentNullException("onNext");
				}
				if (onError == null)
				{
					throw new ArgumentNullException("onError");
				}
				if (onCompleted == null)
				{
					throw new ArgumentNullException("onCompleted");
				}
				return onNext(this.Value);
			}
		}

		[DebuggerDisplay("OnError({Exception})")]
		[Serializable]
		internal sealed class OnErrorNotification : Notification<T>
		{
			private Exception exception;

			public OnErrorNotification(Exception exception)
			{
				this.exception = exception;
			}

			public override T Value
			{
				get
				{
					throw this.exception;
				}
			}

			public override Exception Exception
			{
				get
				{
					return this.exception;
				}
			}

			public override bool HasValue
			{
				get
				{
					return false;
				}
			}

			public override NotificationKind Kind
			{
				get
				{
					return NotificationKind.OnError;
				}
			}

			public override int GetHashCode()
			{
				return this.Exception.GetHashCode();
			}

			public override bool Equals(Notification<T> other)
			{
				return object.ReferenceEquals(this, other) || (!object.ReferenceEquals(other, null) && other.Kind == NotificationKind.OnError && object.Equals(this.Exception, other.Exception));
			}

			public override string ToString()
			{
				return string.Format(CultureInfo.CurrentCulture, "OnError({0})", new object[]
				{
					this.Exception.GetType().FullName
				});
			}

			public override void Accept(IObserver<T> observer)
			{
				if (observer == null)
				{
					throw new ArgumentNullException("observer");
				}
				observer.OnError(this.Exception);
			}

			public override TResult Accept<TResult>(IObserver<T, TResult> observer)
			{
				if (observer == null)
				{
					throw new ArgumentNullException("observer");
				}
				return observer.OnError(this.Exception);
			}

			public override void Accept(Action<T> onNext, Action<Exception> onError, Action onCompleted)
			{
				if (onNext == null)
				{
					throw new ArgumentNullException("onNext");
				}
				if (onError == null)
				{
					throw new ArgumentNullException("onError");
				}
				if (onCompleted == null)
				{
					throw new ArgumentNullException("onCompleted");
				}
				onError(this.Exception);
			}

			public override TResult Accept<TResult>(Func<T, TResult> onNext, Func<Exception, TResult> onError, Func<TResult> onCompleted)
			{
				if (onNext == null)
				{
					throw new ArgumentNullException("onNext");
				}
				if (onError == null)
				{
					throw new ArgumentNullException("onError");
				}
				if (onCompleted == null)
				{
					throw new ArgumentNullException("onCompleted");
				}
				return onError(this.Exception);
			}
		}

		[DebuggerDisplay("OnCompleted()")]
		[Serializable]
		internal sealed class OnCompletedNotification : Notification<T>
		{
			public override T Value
			{
				get
				{
					throw new InvalidOperationException("No Value");
				}
			}

			public override Exception Exception
			{
				get
				{
					return null;
				}
			}

			public override bool HasValue
			{
				get
				{
					return false;
				}
			}

			public override NotificationKind Kind
			{
				get
				{
					return NotificationKind.OnCompleted;
				}
			}

			public override int GetHashCode()
			{
				return typeof(T).GetHashCode() ^ 8510;
			}

			public override bool Equals(Notification<T> other)
			{
				return object.ReferenceEquals(this, other) || (!object.ReferenceEquals(other, null) && other.Kind == NotificationKind.OnCompleted);
			}

			public override string ToString()
			{
				return "OnCompleted()";
			}

			public override void Accept(IObserver<T> observer)
			{
				if (observer == null)
				{
					throw new ArgumentNullException("observer");
				}
				observer.OnCompleted();
			}

			public override TResult Accept<TResult>(IObserver<T, TResult> observer)
			{
				if (observer == null)
				{
					throw new ArgumentNullException("observer");
				}
				return observer.OnCompleted();
			}

			public override void Accept(Action<T> onNext, Action<Exception> onError, Action onCompleted)
			{
				if (onNext == null)
				{
					throw new ArgumentNullException("onNext");
				}
				if (onError == null)
				{
					throw new ArgumentNullException("onError");
				}
				if (onCompleted == null)
				{
					throw new ArgumentNullException("onCompleted");
				}
				onCompleted();
			}

			public override TResult Accept<TResult>(Func<T, TResult> onNext, Func<Exception, TResult> onError, Func<TResult> onCompleted)
			{
				if (onNext == null)
				{
					throw new ArgumentNullException("onNext");
				}
				if (onError == null)
				{
					throw new ArgumentNullException("onError");
				}
				if (onCompleted == null)
				{
					throw new ArgumentNullException("onCompleted");
				}
				return onCompleted();
			}
		}
	}
}
