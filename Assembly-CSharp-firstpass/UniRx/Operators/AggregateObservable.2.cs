using System;

namespace UniRx.Operators
{
	internal class AggregateObservable<TSource, TAccumulate> : OperatorObservableBase<TAccumulate>
	{
		private readonly IObservable<TSource> source;

		private readonly TAccumulate seed;

		private readonly Func<TAccumulate, TSource, TAccumulate> accumulator;

		public AggregateObservable(IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator) : base(source.IsRequiredSubscribeOnCurrentThread<TSource>())
		{
			this.source = source;
			this.seed = seed;
			this.accumulator = accumulator;
		}

		protected override IDisposable SubscribeCore(IObserver<TAccumulate> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new AggregateObservable<TSource, TAccumulate>.Aggregate(this, observer, cancel));
		}

		private class Aggregate : OperatorObserverBase<TSource, TAccumulate>
		{
			private readonly AggregateObservable<TSource, TAccumulate> parent;

			private TAccumulate accumulation;

			public Aggregate(AggregateObservable<TSource, TAccumulate> parent, IObserver<TAccumulate> observer, IDisposable cancel) : base(observer, cancel)
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
				this.observer.OnNext(this.accumulation);
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
