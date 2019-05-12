using System;

namespace UniRx.Operators
{
	internal class SelectObservable<T, TR> : OperatorObservableBase<TR>, ISelect<TR>
	{
		private readonly IObservable<T> source;

		private readonly Func<T, TR> selector;

		private readonly Func<T, int, TR> selectorWithIndex;

		public SelectObservable(IObservable<T> source, Func<T, TR> selector) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.selector = selector;
		}

		public SelectObservable(IObservable<T> source, Func<T, int, TR> selector) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.selectorWithIndex = selector;
		}

		public IObservable<TR> CombinePredicate(Func<TR, bool> predicate)
		{
			if (this.selector != null)
			{
				return new SelectWhereObservable<T, TR>(this.source, this.selector, predicate);
			}
			return new WhereObservable<TR>(this, predicate);
		}

		protected override IDisposable SubscribeCore(IObserver<TR> observer, IDisposable cancel)
		{
			if (this.selector != null)
			{
				return this.source.Subscribe(new SelectObservable<T, TR>.Select(this, observer, cancel));
			}
			return this.source.Subscribe(new SelectObservable<T, TR>.Select_(this, observer, cancel));
		}

		private class Select : OperatorObserverBase<T, TR>
		{
			private readonly SelectObservable<T, TR> parent;

			public Select(SelectObservable<T, TR> parent, IObserver<TR> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public override void OnNext(T value)
			{
				TR value2 = default(TR);
				try
				{
					value2 = this.parent.selector(value);
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

		private class Select_ : OperatorObserverBase<T, TR>
		{
			private readonly SelectObservable<T, TR> parent;

			private int index;

			public Select_(SelectObservable<T, TR> parent, IObserver<TR> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
				this.index = 0;
			}

			public override void OnNext(T value)
			{
				TR value2 = default(TR);
				try
				{
					value2 = this.parent.selectorWithIndex(value, this.index++);
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
