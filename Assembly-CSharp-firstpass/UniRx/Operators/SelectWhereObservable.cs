using System;

namespace UniRx.Operators
{
	internal class SelectWhereObservable<T, TR> : OperatorObservableBase<TR>
	{
		private readonly IObservable<T> source;

		private readonly Func<T, TR> selector;

		private readonly Func<TR, bool> predicate;

		public SelectWhereObservable(IObservable<T> source, Func<T, TR> selector, Func<TR, bool> predicate) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.selector = selector;
			this.predicate = predicate;
		}

		protected override IDisposable SubscribeCore(IObserver<TR> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new SelectWhereObservable<T, TR>.SelectWhere(this, observer, cancel));
		}

		private class SelectWhere : OperatorObserverBase<T, TR>
		{
			private readonly SelectWhereObservable<T, TR> parent;

			public SelectWhere(SelectWhereObservable<T, TR> parent, IObserver<TR> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public override void OnNext(T value)
			{
				TR tr = default(TR);
				try
				{
					tr = this.parent.selector(value);
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
				try
				{
					flag = this.parent.predicate(tr);
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
				if (flag)
				{
					this.observer.OnNext(tr);
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
