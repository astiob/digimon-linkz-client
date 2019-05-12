using System;

namespace UniRx.Operators
{
	internal class DeferObservable<T> : OperatorObservableBase<T>
	{
		private readonly Func<IObservable<T>> observableFactory;

		public DeferObservable(Func<IObservable<T>> observableFactory) : base(false)
		{
			this.observableFactory = observableFactory;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			observer = new DeferObservable<T>.Defer(observer, cancel);
			IObservable<T> observable;
			try
			{
				observable = this.observableFactory();
			}
			catch (Exception error)
			{
				observable = Observable.Throw<T>(error);
			}
			return observable.Subscribe(observer);
		}

		private class Defer : OperatorObserverBase<T, T>
		{
			public Defer(IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
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
