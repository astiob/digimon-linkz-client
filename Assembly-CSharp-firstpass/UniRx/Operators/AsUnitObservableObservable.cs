using System;

namespace UniRx.Operators
{
	internal class AsUnitObservableObservable<T> : OperatorObservableBase<Unit>
	{
		private readonly IObservable<T> source;

		public AsUnitObservableObservable(IObservable<T> source) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
		}

		protected override IDisposable SubscribeCore(IObserver<Unit> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new AsUnitObservableObservable<T>.AsUnitObservable(observer, cancel));
		}

		private class AsUnitObservable : OperatorObserverBase<T, Unit>
		{
			public AsUnitObservable(IObserver<Unit> observer, IDisposable cancel) : base(observer, cancel)
			{
			}

			public override void OnNext(T value)
			{
				this.observer.OnNext(Unit.Default);
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
