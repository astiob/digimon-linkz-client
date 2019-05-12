using System;

namespace UniRx.Operators
{
	internal class CastObservable<TSource, TResult> : OperatorObservableBase<TResult>
	{
		private readonly IObservable<TSource> source;

		public CastObservable(IObservable<TSource> source) : base(source.IsRequiredSubscribeOnCurrentThread<TSource>())
		{
			this.source = source;
		}

		protected override IDisposable SubscribeCore(IObserver<TResult> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new CastObservable<TSource, TResult>.Cast(observer, cancel));
		}

		private class Cast : OperatorObserverBase<TSource, TResult>
		{
			public Cast(IObserver<TResult> observer, IDisposable cancel) : base(observer, cancel)
			{
			}

			public override void OnNext(TSource value)
			{
				TResult value2 = default(TResult);
				try
				{
					value2 = (TResult)((object)value);
				}
				catch (Exception error)
				{
					try
					{
						this.observer.OnError(error);
					}
					finally
					{
						base.Dispose();
					}
					return;
				}
				this.observer.OnNext(value2);
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
