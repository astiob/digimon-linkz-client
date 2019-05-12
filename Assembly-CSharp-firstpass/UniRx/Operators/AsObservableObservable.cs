using System;

namespace UniRx.Operators
{
	internal class AsObservableObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		public AsObservableObservable(IObservable<T> source) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new AsObservableObservable<T>.AsObservable(observer, cancel));
		}

		private class AsObservable : OperatorObserverBase<T, T>
		{
			public AsObservable(IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
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
