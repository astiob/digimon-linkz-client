using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class DistinctUntilChangedObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly IEqualityComparer<T> comparer;

		public DistinctUntilChangedObservable(IObservable<T> source, IEqualityComparer<T> comparer) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.comparer = comparer;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new DistinctUntilChangedObservable<T>.DistinctUntilChanged(this, observer, cancel));
		}

		private class DistinctUntilChanged : OperatorObserverBase<T, T>
		{
			private readonly DistinctUntilChangedObservable<T> parent;

			private bool isFirst = true;

			private T prevKey = default(T);

			public DistinctUntilChanged(DistinctUntilChangedObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public override void OnNext(T value)
			{
				try
				{
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
						flag = this.parent.comparer.Equals(value, this.prevKey);
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
					this.prevKey = value;
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
