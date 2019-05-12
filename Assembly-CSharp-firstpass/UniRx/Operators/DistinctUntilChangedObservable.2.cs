using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class DistinctUntilChangedObservable<T, TKey> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly IEqualityComparer<TKey> comparer;

		private readonly Func<T, TKey> keySelector;

		public DistinctUntilChangedObservable(IObservable<T> source, Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.comparer = comparer;
			this.keySelector = keySelector;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new DistinctUntilChangedObservable<T, TKey>.DistinctUntilChanged(this, observer, cancel));
		}

		private class DistinctUntilChanged : OperatorObserverBase<T, T>
		{
			private readonly DistinctUntilChangedObservable<T, TKey> parent;

			private bool isFirst = true;

			private TKey prevKey = default(TKey);

			public DistinctUntilChanged(DistinctUntilChangedObservable<T, TKey> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public override void OnNext(T value)
			{
				TKey x;
				try
				{
					x = this.parent.keySelector(value);
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
				bool flag = false;
				if (this.isFirst)
				{
					this.isFirst = false;
				}
				else
				{
					try
					{
						flag = this.parent.comparer.Equals(x, this.prevKey);
					}
					catch (Exception error2)
					{
						try
						{
							this.observer.OnError(error2);
						}
						finally
						{
							base.Dispose();
						}
						return;
					}
				}
				if (!flag)
				{
					this.prevKey = x;
					this.observer.OnNext(value);
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
