using System;

namespace UniRx.Operators
{
	internal class CreateObservable<T> : OperatorObservableBase<T>
	{
		private readonly Func<IObserver<T>, IDisposable> subscribe;

		public CreateObservable(Func<IObserver<T>, IDisposable> subscribe) : base(true)
		{
			this.subscribe = subscribe;
		}

		public CreateObservable(Func<IObserver<T>, IDisposable> subscribe, bool isRequiredSubscribeOnCurrentThread) : base(isRequiredSubscribeOnCurrentThread)
		{
			this.subscribe = subscribe;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			observer = new CreateObservable<T>.Create(observer, cancel);
			return this.subscribe(observer) ?? Disposable.Empty;
		}

		private class Create : OperatorObserverBase<T, T>
		{
			public Create(IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
			}

			public override void OnNext(T value)
			{
				this.observer.OnNext(value);
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
