using System;

namespace UniRx.Operators
{
	internal class CreateObservable<T, TState> : OperatorObservableBase<T>
	{
		private readonly TState state;

		private readonly Func<TState, IObserver<T>, IDisposable> subscribe;

		public CreateObservable(TState state, Func<TState, IObserver<T>, IDisposable> subscribe) : base(true)
		{
			this.state = state;
			this.subscribe = subscribe;
		}

		public CreateObservable(TState state, Func<TState, IObserver<T>, IDisposable> subscribe, bool isRequiredSubscribeOnCurrentThread) : base(isRequiredSubscribeOnCurrentThread)
		{
			this.state = state;
			this.subscribe = subscribe;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			observer = new CreateObservable<T, TState>.Create(observer, cancel);
			return this.subscribe(this.state, observer) ?? Disposable.Empty;
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
