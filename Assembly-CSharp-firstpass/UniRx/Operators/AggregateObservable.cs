using System;

namespace UniRx.Operators
{
	internal class AggregateObservable<TSource> : OperatorObservableBase<TSource>
	{
		private readonly IObservable<TSource> source;

		private readonly Func<TSource, TSource, TSource> accumulator;

		public AggregateObservable(IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator) : base(source.IsRequiredSubscribeOnCurrentThread<TSource>())
		{
			this.source = source;
			this.accumulator = accumulator;
		}

		protected override IDisposable SubscribeCore(IObserver<TSource> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new AggregateObservable<TSource>.Aggregate(this, observer, cancel));
		}

		private class Aggregate : OperatorObserverBase<TSource, TSource>
		{
			private readonly AggregateObservable<TSource> parent;

			private TSource accumulation;

			private bool seenValue;

			public Aggregate(AggregateObservable<TSource> parent, IObserver<TSource> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
				this.seenValue = false;
			}

			public override void OnNext(TSource value)
			{
				if (!this.seenValue)
				{
					this.seenValue = true;
					this.accumulation = value;
				}
				else
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
				if (!this.seenValue)
				{
					throw new InvalidOperationException("Sequence contains no elements.");
				}
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
