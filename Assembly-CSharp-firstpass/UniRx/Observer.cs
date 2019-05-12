using System;
using System.Threading;
using UniRx.Operators;

namespace UniRx
{
	public static class Observer
	{
		internal static IObserver<T> CreateSubscribeObserver<T>(Action<T> onNext, Action<Exception> onError, Action onCompleted)
		{
			if (onNext == Stubs<T>.Ignore)
			{
				return new Observer.Subscribe_<T>(onError, onCompleted);
			}
			return new Observer.Subscribe<T>(onNext, onError, onCompleted);
		}

		internal static IObserver<T> CreateSubscribeWithStateObserver<T, TState>(TState state, Action<T, TState> onNext, Action<Exception, TState> onError, Action<TState> onCompleted)
		{
			return new Observer.Subscribe<T, TState>(state, onNext, onError, onCompleted);
		}

		internal static IObserver<T> CreateSubscribeWithState2Observer<T, TState1, TState2>(TState1 state1, TState2 state2, Action<T, TState1, TState2> onNext, Action<Exception, TState1, TState2> onError, Action<TState1, TState2> onCompleted)
		{
			return new Observer.Subscribe<T, TState1, TState2>(state1, state2, onNext, onError, onCompleted);
		}

		internal static IObserver<T> CreateSubscribeWithState3Observer<T, TState1, TState2, TState3>(TState1 state1, TState2 state2, TState3 state3, Action<T, TState1, TState2, TState3> onNext, Action<Exception, TState1, TState2, TState3> onError, Action<TState1, TState2, TState3> onCompleted)
		{
			return new Observer.Subscribe<T, TState1, TState2, TState3>(state1, state2, state3, onNext, onError, onCompleted);
		}

		public static IObserver<T> Create<T>(Action<T> onNext)
		{
			return Observer.Create<T>(onNext, Stubs.Throw, Stubs.Nop);
		}

		public static IObserver<T> Create<T>(Action<T> onNext, Action<Exception> onError)
		{
			return Observer.Create<T>(onNext, onError, Stubs.Nop);
		}

		public static IObserver<T> Create<T>(Action<T> onNext, Action onCompleted)
		{
			return Observer.Create<T>(onNext, Stubs.Throw, onCompleted);
		}

		public static IObserver<T> Create<T>(Action<T> onNext, Action<Exception> onError, Action onCompleted)
		{
			if (onNext == Stubs<T>.Ignore)
			{
				return new Observer.EmptyOnNextAnonymousObserver<T>(onError, onCompleted);
			}
			return new Observer.AnonymousObserver<T>(onNext, onError, onCompleted);
		}

		public static IObserver<T> CreateAutoDetachObserver<T>(IObserver<T> observer, IDisposable disposable)
		{
			return new Observer.AutoDetachObserver<T>(observer, disposable);
		}

		private class AnonymousObserver<T> : IObserver<T>
		{
			private readonly Action<T> onNext;

			private readonly Action<Exception> onError;

			private readonly Action onCompleted;

			private int isStopped;

			public AnonymousObserver(Action<T> onNext, Action<Exception> onError, Action onCompleted)
			{
				this.onNext = onNext;
				this.onError = onError;
				this.onCompleted = onCompleted;
			}

			public void OnNext(T value)
			{
				if (this.isStopped == 0)
				{
					this.onNext(value);
				}
			}

			public void OnError(Exception error)
			{
				if (Interlocked.Increment(ref this.isStopped) == 1)
				{
					this.onError(error);
				}
			}

			public void OnCompleted()
			{
				if (Interlocked.Increment(ref this.isStopped) == 1)
				{
					this.onCompleted();
				}
			}
		}

		private class EmptyOnNextAnonymousObserver<T> : IObserver<T>
		{
			private readonly Action<Exception> onError;

			private readonly Action onCompleted;

			private int isStopped;

			public EmptyOnNextAnonymousObserver(Action<Exception> onError, Action onCompleted)
			{
				this.onError = onError;
				this.onCompleted = onCompleted;
			}

			public void OnNext(T value)
			{
			}

			public void OnError(Exception error)
			{
				if (Interlocked.Increment(ref this.isStopped) == 1)
				{
					this.onError(error);
				}
			}

			public void OnCompleted()
			{
				if (Interlocked.Increment(ref this.isStopped) == 1)
				{
					this.onCompleted();
				}
			}
		}

		private class Subscribe<T> : IObserver<T>
		{
			private readonly Action<T> onNext;

			private readonly Action<Exception> onError;

			private readonly Action onCompleted;

			private int isStopped;

			public Subscribe(Action<T> onNext, Action<Exception> onError, Action onCompleted)
			{
				this.onNext = onNext;
				this.onError = onError;
				this.onCompleted = onCompleted;
			}

			public void OnNext(T value)
			{
				if (this.isStopped == 0)
				{
					this.onNext(value);
				}
			}

			public void OnError(Exception error)
			{
				if (Interlocked.Increment(ref this.isStopped) == 1)
				{
					this.onError(error);
				}
			}

			public void OnCompleted()
			{
				if (Interlocked.Increment(ref this.isStopped) == 1)
				{
					this.onCompleted();
				}
			}
		}

		private class Subscribe_<T> : IObserver<T>
		{
			private readonly Action<Exception> onError;

			private readonly Action onCompleted;

			private int isStopped;

			public Subscribe_(Action<Exception> onError, Action onCompleted)
			{
				this.onError = onError;
				this.onCompleted = onCompleted;
			}

			public void OnNext(T value)
			{
			}

			public void OnError(Exception error)
			{
				if (Interlocked.Increment(ref this.isStopped) == 1)
				{
					this.onError(error);
				}
			}

			public void OnCompleted()
			{
				if (Interlocked.Increment(ref this.isStopped) == 1)
				{
					this.onCompleted();
				}
			}
		}

		private class Subscribe<T, TState> : IObserver<T>
		{
			private readonly TState state;

			private readonly Action<T, TState> onNext;

			private readonly Action<Exception, TState> onError;

			private readonly Action<TState> onCompleted;

			private int isStopped;

			public Subscribe(TState state, Action<T, TState> onNext, Action<Exception, TState> onError, Action<TState> onCompleted)
			{
				this.state = state;
				this.onNext = onNext;
				this.onError = onError;
				this.onCompleted = onCompleted;
			}

			public void OnNext(T value)
			{
				if (this.isStopped == 0)
				{
					this.onNext(value, this.state);
				}
			}

			public void OnError(Exception error)
			{
				if (Interlocked.Increment(ref this.isStopped) == 1)
				{
					this.onError(error, this.state);
				}
			}

			public void OnCompleted()
			{
				if (Interlocked.Increment(ref this.isStopped) == 1)
				{
					this.onCompleted(this.state);
				}
			}
		}

		private class Subscribe<T, TState1, TState2> : IObserver<T>
		{
			private readonly TState1 state1;

			private readonly TState2 state2;

			private readonly Action<T, TState1, TState2> onNext;

			private readonly Action<Exception, TState1, TState2> onError;

			private readonly Action<TState1, TState2> onCompleted;

			private int isStopped;

			public Subscribe(TState1 state1, TState2 state2, Action<T, TState1, TState2> onNext, Action<Exception, TState1, TState2> onError, Action<TState1, TState2> onCompleted)
			{
				this.state1 = state1;
				this.state2 = state2;
				this.onNext = onNext;
				this.onError = onError;
				this.onCompleted = onCompleted;
			}

			public void OnNext(T value)
			{
				if (this.isStopped == 0)
				{
					this.onNext(value, this.state1, this.state2);
				}
			}

			public void OnError(Exception error)
			{
				if (Interlocked.Increment(ref this.isStopped) == 1)
				{
					this.onError(error, this.state1, this.state2);
				}
			}

			public void OnCompleted()
			{
				if (Interlocked.Increment(ref this.isStopped) == 1)
				{
					this.onCompleted(this.state1, this.state2);
				}
			}
		}

		private class Subscribe<T, TState1, TState2, TState3> : IObserver<T>
		{
			private readonly TState1 state1;

			private readonly TState2 state2;

			private readonly TState3 state3;

			private readonly Action<T, TState1, TState2, TState3> onNext;

			private readonly Action<Exception, TState1, TState2, TState3> onError;

			private readonly Action<TState1, TState2, TState3> onCompleted;

			private int isStopped;

			public Subscribe(TState1 state1, TState2 state2, TState3 state3, Action<T, TState1, TState2, TState3> onNext, Action<Exception, TState1, TState2, TState3> onError, Action<TState1, TState2, TState3> onCompleted)
			{
				this.state1 = state1;
				this.state2 = state2;
				this.state3 = state3;
				this.onNext = onNext;
				this.onError = onError;
				this.onCompleted = onCompleted;
			}

			public void OnNext(T value)
			{
				if (this.isStopped == 0)
				{
					this.onNext(value, this.state1, this.state2, this.state3);
				}
			}

			public void OnError(Exception error)
			{
				if (Interlocked.Increment(ref this.isStopped) == 1)
				{
					this.onError(error, this.state1, this.state2, this.state3);
				}
			}

			public void OnCompleted()
			{
				if (Interlocked.Increment(ref this.isStopped) == 1)
				{
					this.onCompleted(this.state1, this.state2, this.state3);
				}
			}
		}

		private class AutoDetachObserver<T> : OperatorObserverBase<T, T>
		{
			public AutoDetachObserver(IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
			}

			public override void OnNext(T value)
			{
				try
				{
					this.observer.OnNext(value);
				}
				catch
				{
					base.Dispose();
					throw;
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
	}
}
