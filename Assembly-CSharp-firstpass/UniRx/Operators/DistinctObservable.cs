using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class DistinctObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly IEqualityComparer<T> comparer;

		public DistinctObservable(IObservable<T> source, IEqualityComparer<T> comparer) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.comparer = comparer;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new DistinctObservable<T>.Distinct(this, observer, cancel));
		}

		private class Distinct : OperatorObserverBase<T, T>
		{
			private readonly HashSet<T> hashSet;

			public Distinct(DistinctObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.hashSet = ((parent.comparer != null) ? new HashSet<T>(parent.comparer) : new HashSet<T>());
			}

			public override void OnNext(T value)
			{
				T t = default(T);
				bool flag = false;
				try
				{
					flag = this.hashSet.Add(value);
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
