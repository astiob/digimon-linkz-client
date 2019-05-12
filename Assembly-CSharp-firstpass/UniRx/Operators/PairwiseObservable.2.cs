using System;

namespace UniRx.Operators
{
	internal class PairwiseObservable<T> : OperatorObservableBase<Pair<T>>
	{
		private readonly IObservable<T> source;

		public PairwiseObservable(IObservable<T> source) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
		}

		protected override IDisposable SubscribeCore(IObserver<Pair<T>> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new PairwiseObservable<T>.Pairwise(observer, cancel));
		}

		private class Pairwise : OperatorObserverBase<T, Pair<T>>
		{
			private T prev = default(T);

			private bool isFirst = true;

			public Pairwise(IObserver<Pair<T>> observer, IDisposable cancel) : base(observer, cancel)
			{
			}

			public override void OnNext(T value)
			{
				if (this.isFirst)
				{
					this.isFirst = false;
					this.prev = value;
					return;
				}
				Pair<T> value2 = new Pair<T>(this.prev, value);
				this.prev = value;
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
