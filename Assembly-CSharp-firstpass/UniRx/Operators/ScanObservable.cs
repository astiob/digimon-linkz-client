using System;

namespace UniRx.Operators
{
	internal class ScanObservable<TSource> : OperatorObservableBase<TSource>
	{
		private readonly IObservable<TSource> source;

		private readonly Func<TSource, TSource, TSource> accumulator;

		public ScanObservable(IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator) : base(source.IsRequiredSubscribeOnCurrentThread<TSource>())
		{
			this.source = source;
			this.accumulator = accumulator;
		}

		protected override IDisposable SubscribeCore(IObserver<TSource> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new ScanObservable<TSource>.Scan(this, observer, cancel));
		}

		private class Scan : OperatorObserverBase<TSource, TSource>
		{
			private readonly ScanObservable<TSource> parent;

			private TSource accumulation;

			private bool isFirst;

			public Scan(ScanObservable<TSource> parent, IObserver<TSource> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
				this.isFirst = true;
			}

			public override void OnNext(TSource value)
			{
				if (this.isFirst)
				{
					this.isFirst = false;
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
						return;
					}
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
