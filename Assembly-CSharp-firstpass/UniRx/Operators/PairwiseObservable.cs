using System;

namespace UniRx.Operators
{
	internal class PairwiseObservable<T, TR> : OperatorObservableBase<TR>
	{
		private readonly IObservable<T> source;

		private readonly Func<T, T, TR> selector;

		public PairwiseObservable(IObservable<T> source, Func<T, T, TR> selector) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.selector = selector;
		}

		protected override IDisposable SubscribeCore(IObserver<TR> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new PairwiseObservable<T, TR>.Pairwise(this, observer, cancel));
		}

		private class Pairwise : OperatorObserverBase<T, TR>
		{
			private readonly PairwiseObservable<T, TR> parent;

			private T prev = default(T);

			private bool isFirst = true;

			public Pairwise(PairwiseObservable<T, TR> parent, IObserver<TR> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public override void OnNext(T value)
			{
				if (this.isFirst)
				{
					this.isFirst = false;
					this.prev = value;
					return;
				}
				TR value2;
				try
				{
					value2 = this.parent.selector(this.prev, value);
					this.prev = value;
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
