using System;
using System.Collections;

namespace UniRx.Operators
{
	internal class FromCoroutineObservable<T> : OperatorObservableBase<T>
	{
		private readonly Func<IObserver<T>, CancellationToken, IEnumerator> coroutine;

		public FromCoroutineObservable(Func<IObserver<T>, CancellationToken, IEnumerator> coroutine) : base(false)
		{
			this.coroutine = coroutine;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			FromCoroutineObservable<T>.FromCoroutine arg = new FromCoroutineObservable<T>.FromCoroutine(observer, cancel);
			BooleanDisposable booleanDisposable = new BooleanDisposable();
			CancellationToken arg2 = new CancellationToken(booleanDisposable);
			MainThreadDispatcher.SendStartCoroutine(this.coroutine(arg, arg2));
			return booleanDisposable;
		}

		private class FromCoroutine : OperatorObserverBase<T, T>
		{
			public FromCoroutine(IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
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
