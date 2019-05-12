using System;

namespace UniRx.Operators
{
	internal class IgnoreElementsObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		public IgnoreElementsObservable(IObservable<T> source) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new IgnoreElementsObservable<T>.IgnoreElements(observer, cancel));
		}

		private class IgnoreElements : OperatorObserverBase<T, T>
		{
			public IgnoreElements(IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
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
