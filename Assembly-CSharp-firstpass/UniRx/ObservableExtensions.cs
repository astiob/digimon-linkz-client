using System;
using UniRx.InternalUtil;

namespace UniRx
{
	public static class ObservableExtensions
	{
		public static IDisposable Subscribe<T>(this IObservable<T> source)
		{
			return source.Subscribe(ThrowObserver<T>.Instance);
		}

		public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext)
		{
			return source.Subscribe(Observer.CreateSubscribeObserver<T>(onNext, Stubs.Throw, Stubs.Nop));
		}

		public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError)
		{
			return source.Subscribe(Observer.CreateSubscribeObserver<T>(onNext, onError, Stubs.Nop));
		}

		public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action onCompleted)
		{
			return source.Subscribe(Observer.CreateSubscribeObserver<T>(onNext, Stubs.Throw, onCompleted));
		}

		public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError, Action onCompleted)
		{
			return source.Subscribe(Observer.CreateSubscribeObserver<T>(onNext, onError, onCompleted));
		}

		public static IDisposable SubscribeWithState<T, TState>(this IObservable<T> source, TState state, Action<T, TState> onNext)
		{
			return source.Subscribe(Observer.CreateSubscribeWithStateObserver<T, TState>(state, onNext, Stubs<TState>.Throw, Stubs<TState>.Ignore));
		}

		public static IDisposable SubscribeWithState<T, TState>(this IObservable<T> source, TState state, Action<T, TState> onNext, Action<Exception, TState> onError)
		{
			return source.Subscribe(Observer.CreateSubscribeWithStateObserver<T, TState>(state, onNext, onError, Stubs<TState>.Ignore));
		}

		public static IDisposable SubscribeWithState<T, TState>(this IObservable<T> source, TState state, Action<T, TState> onNext, Action<TState> onCompleted)
		{
			return source.Subscribe(Observer.CreateSubscribeWithStateObserver<T, TState>(state, onNext, Stubs<TState>.Throw, onCompleted));
		}

		public static IDisposable SubscribeWithState<T, TState>(this IObservable<T> source, TState state, Action<T, TState> onNext, Action<Exception, TState> onError, Action<TState> onCompleted)
		{
			return source.Subscribe(Observer.CreateSubscribeWithStateObserver<T, TState>(state, onNext, onError, onCompleted));
		}

		public static IDisposable SubscribeWithState2<T, TState1, TState2>(this IObservable<T> source, TState1 state1, TState2 state2, Action<T, TState1, TState2> onNext)
		{
			return source.Subscribe(Observer.CreateSubscribeWithState2Observer<T, TState1, TState2>(state1, state2, onNext, Stubs<TState1, TState2>.Throw, Stubs<TState1, TState2>.Ignore));
		}

		public static IDisposable SubscribeWithState2<T, TState1, TState2>(this IObservable<T> source, TState1 state1, TState2 state2, Action<T, TState1, TState2> onNext, Action<Exception, TState1, TState2> onError)
		{
			return source.Subscribe(Observer.CreateSubscribeWithState2Observer<T, TState1, TState2>(state1, state2, onNext, onError, Stubs<TState1, TState2>.Ignore));
		}

		public static IDisposable SubscribeWithState2<T, TState1, TState2>(this IObservable<T> source, TState1 state1, TState2 state2, Action<T, TState1, TState2> onNext, Action<TState1, TState2> onCompleted)
		{
			return source.Subscribe(Observer.CreateSubscribeWithState2Observer<T, TState1, TState2>(state1, state2, onNext, Stubs<TState1, TState2>.Throw, onCompleted));
		}

		public static IDisposable SubscribeWithState2<T, TState1, TState2>(this IObservable<T> source, TState1 state1, TState2 state2, Action<T, TState1, TState2> onNext, Action<Exception, TState1, TState2> onError, Action<TState1, TState2> onCompleted)
		{
			return source.Subscribe(Observer.CreateSubscribeWithState2Observer<T, TState1, TState2>(state1, state2, onNext, onError, onCompleted));
		}

		public static IDisposable SubscribeWithState3<T, TState1, TState2, TState3>(this IObservable<T> source, TState1 state1, TState2 state2, TState3 state3, Action<T, TState1, TState2, TState3> onNext)
		{
			return source.Subscribe(Observer.CreateSubscribeWithState3Observer<T, TState1, TState2, TState3>(state1, state2, state3, onNext, Stubs<TState1, TState2, TState3>.Throw, Stubs<TState1, TState2, TState3>.Ignore));
		}

		public static IDisposable SubscribeWithState3<T, TState1, TState2, TState3>(this IObservable<T> source, TState1 state1, TState2 state2, TState3 state3, Action<T, TState1, TState2, TState3> onNext, Action<Exception, TState1, TState2, TState3> onError)
		{
			return source.Subscribe(Observer.CreateSubscribeWithState3Observer<T, TState1, TState2, TState3>(state1, state2, state3, onNext, onError, Stubs<TState1, TState2, TState3>.Ignore));
		}

		public static IDisposable SubscribeWithState3<T, TState1, TState2, TState3>(this IObservable<T> source, TState1 state1, TState2 state2, TState3 state3, Action<T, TState1, TState2, TState3> onNext, Action<TState1, TState2, TState3> onCompleted)
		{
			return source.Subscribe(Observer.CreateSubscribeWithState3Observer<T, TState1, TState2, TState3>(state1, state2, state3, onNext, Stubs<TState1, TState2, TState3>.Throw, onCompleted));
		}

		public static IDisposable SubscribeWithState3<T, TState1, TState2, TState3>(this IObservable<T> source, TState1 state1, TState2 state2, TState3 state3, Action<T, TState1, TState2, TState3> onNext, Action<Exception, TState1, TState2, TState3> onError, Action<TState1, TState2, TState3> onCompleted)
		{
			return source.Subscribe(Observer.CreateSubscribeWithState3Observer<T, TState1, TState2, TState3>(state1, state2, state3, onNext, onError, onCompleted));
		}
	}
}
