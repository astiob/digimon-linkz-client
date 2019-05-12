using System;

namespace UniRx.Operators
{
	internal class CreateSafeObservable<T> : OperatorObservableBase<T>
	{
		private readonly Func<IObserver<T>, IDisposable> subscribe;

		public CreateSafeObservable(Func<IObserver<T>, IDisposable> subscribe) : base(true)
		{
			this.subscribe = subscribe;
		}

		public CreateSafeObservable(Func<IObserver<T>, IDisposable> subscribe, bool isRequiredSubscribeOnCurrentThread) : base(isRequiredSubscribeOnCurrentThread)
		{
			this.subscribe = subscribe;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			observer = new CreateSafeObservable<T>.CreateSafe(observer, cancel);
			return this.subscribe(observer) ?? Disposable.Empty;
		}

		private class CreateSafe : OperatorObserverBase<T, T>
		{
			public CreateSafe(IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
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
