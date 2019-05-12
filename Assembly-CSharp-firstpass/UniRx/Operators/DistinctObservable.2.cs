using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class DistinctObservable<T, TKey> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly IEqualityComparer<TKey> comparer;

		private readonly Func<T, TKey> keySelector;

		public DistinctObservable(IObservable<T> source, Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.comparer = comparer;
			this.keySelector = keySelector;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new DistinctObservable<T, TKey>.Distinct(this, observer, cancel));
		}

		private class Distinct : OperatorObserverBase<T, T>
		{
			private readonly DistinctObservable<T, TKey> parent;

			private readonly HashSet<TKey> hashSet;

			public Distinct(DistinctObservable<T, TKey> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
				this.hashSet = ((parent.comparer != null) ? new HashSet<TKey>(parent.comparer) : new HashSet<TKey>());
			}

			public override void OnNext(T value)
			{
				TKey tkey = default(TKey);
				bool flag = false;
				try
				{
					TKey item = this.parent.keySelector(value);
					flag = this.hashSet.Add(item);
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
				if (flag)
				{
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
