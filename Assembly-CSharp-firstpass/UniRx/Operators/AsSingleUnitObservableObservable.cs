using System;

namespace UniRx.Operators
{
	internal class AsSingleUnitObservableObservable<T> : OperatorObservableBase<Unit>
	{
		private readonly IObservable<T> source;

		public AsSingleUnitObservableObservable(IObservable<T> source) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
		}

		protected override IDisposable SubscribeCore(IObserver<Unit> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new AsSingleUnitObservableObservable<T>.AsSingleUnitObservable(observer, cancel));
		}

		private class AsSingleUnitObservable : OperatorObserverBase<T, Unit>
		{
			public AsSingleUnitObservable(IObserver<Unit> observer, IDisposable cancel) : base(observer, cancel)
			{
			}

			public override void OnNext(T value)
			{
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
				this.observer.OnNext(Unit.Default);
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
