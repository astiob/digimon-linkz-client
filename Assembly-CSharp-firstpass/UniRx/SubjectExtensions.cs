using System;

namespace UniRx
{
	public static class SubjectExtensions
	{
		public static ISubject<T> Synchronize<T>(this ISubject<T> subject)
		{
			return new SubjectExtensions.AnonymousSubject<T>(subject.Synchronize<T>(), subject);
		}

		public static ISubject<T> Synchronize<T>(this ISubject<T> subject, object gate)
		{
			return new SubjectExtensions.AnonymousSubject<T>(subject.Synchronize(gate), subject);
		}

		private class AnonymousSubject<T, U> : ISubject<T, U>, IObserver<T>, IObservable<U>
		{
			private readonly IObserver<T> observer;

			private readonly IObservable<U> observable;

			public AnonymousSubject(IObserver<T> observer, IObservable<U> observable)
			{
				this.observer = observer;
				this.observable = observable;
			}

			public void OnCompleted()
			{
				this.observer.OnCompleted();
			}

			public void OnError(Exception error)
			{
				if (error == null)
				{
					throw new ArgumentNullException("error");
				}
				this.observer.OnError(error);
			}

			public void OnNext(T value)
			{
				this.observer.OnNext(value);
			}

			public IDisposable Subscribe(IObserver<U> observer)
			{
				if (observer == null)
				{
					throw new ArgumentNullException("observer");
				}
				return this.observable.Subscribe(observer);
			}
		}

		private class AnonymousSubject<T> : SubjectExtensions.AnonymousSubject<T, T>, ISubject<T>, ISubject<T, T>, IObserver<T>, IObservable<T>
		{
			public AnonymousSubject(IObserver<T> observer, IObservable<T> observable) : base(observer, observable)
			{
			}
		}
	}
}
