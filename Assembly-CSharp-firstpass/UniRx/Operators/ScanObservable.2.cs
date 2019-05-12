using System;

namespace UniRx.Operators
{
	internal class ScanObservable<TSource, TAccumulate> : OperatorObservableBase<TAccumulate>
	{
		private readonly IObservable<TSource> source;

		private readonly TAccumulate seed;

		private readonly Func<TAccumulate, TSource, TAccumulate> accumulator;

		public ScanObservable(IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator) : base(source.IsRequiredSubscribeOnCurrentThread<TSource>())
		{
			this.source = source;
			this.seed = seed;
			this.accumulator = accumulator;
		}

		protected override IDisposable SubscribeCore(IObserver<TAccumulate> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new ScanObservable<TSource, TAccumulate>.Scan(this, observer, cancel));
		}

		private class Scan : OperatorObserverBase<TSource, TAccumulate>
		{
			private readonly ScanObservable<TSource, TAccumulate> parent;

			private TAccumulate accumulation;

			private bool isFirst;

			public Scan(ScanObservable<TSource, TAccumulate> parent, IObserver<TAccumulate> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
				this.isFirst = true;
			}

			public override void OnNext(TSource value)
			{
				if (this.isFirst)
				{
					this.isFirst = false;
					this.accumulation = this.parent.seed;
				}
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
					return;
				}
				this.observer.OnNext(this.accumulation);
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
