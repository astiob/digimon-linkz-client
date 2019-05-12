using System;

namespace UniRx.Operators
{
	internal class OfTypeObservable<TSource, TResult> : OperatorObservableBase<TResult>
	{
		private readonly IObservable<TSource> source;

		public OfTypeObservable(IObservable<TSource> source) : base(source.IsRequiredSubscribeOnCurrentThread<TSource>())
		{
			this.source = source;
		}

		protected override IDisposable SubscribeCore(IObserver<TResult> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new OfTypeObservable<TSource, TResult>.OfType(observer, cancel));
		}

		private class OfType : OperatorObserverBase<TSource, TResult>
		{
			public OfType(IObserver<TResult> observer, IDisposable cancel) : base(observer, cancel)
			{
			}

			public override void OnNext(TSource value)
			{
				if (value is TResult)
				{
					TResult value2 = (TResult)((object)value);
					this.observer.OnNext(value2);
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
