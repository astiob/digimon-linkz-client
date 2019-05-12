using System;

namespace UniRx.Operators
{
	internal class AggregateObservable<TSource, TAccumulate, TResult> : OperatorObservableBase<TResult>
	{
		private readonly IObservable<TSource> source;

		private readonly TAccumulate seed;

		private readonly Func<TAccumulate, TSource, TAccumulate> accumulator;

		private readonly Func<TAccumulate, TResult> resultSelector;

		public AggregateObservable(IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TResult> resultSelector) : base(source.IsRequiredSubscribeOnCurrentThread<TSource>())
		{
			this.source = source;
			this.seed = seed;
			this.accumulator = accumulator;
			this.resultSelector = resultSelector;
		}

		protected override IDisposable SubscribeCore(IObserver<TResult> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new AggregateObservable<TSource, TAccumulate, TResult>.Aggregate(this, observer, cancel));
		}

		private class Aggregate : OperatorObserverBase<TSource, TResult>
		{
			private readonly AggregateObservable<TSource, TAccumulate, TResult> parent;

			private TAccumulate accumulation;

			public Aggregate(AggregateObservable<TSource, TAccumulate, TResult> parent, IObserver<TResult> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
				this.accumulation = parent.seed;
			}

			public override void OnNext(TSource value)
			{
				try
				{
					this.accumulation = this.parent.accumulator(this.accumulation, value);
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
				TResult value;
				try
				{
					value = this.parent.resultSelector(this.accumulation);
				}
				catch (Exception error)
				{
					this.OnError(error);
					return;
				}
				this.observer.OnNext(value);
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
