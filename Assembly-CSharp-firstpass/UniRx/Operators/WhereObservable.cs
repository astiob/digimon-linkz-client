using System;

namespace UniRx.Operators
{
	internal class WhereObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly Func<T, bool> predicate;

		private readonly Func<T, int, bool> predicateWithIndex;

		public WhereObservable(IObservable<T> source, Func<T, bool> predicate) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.predicate = predicate;
		}

		public WhereObservable(IObservable<T> source, Func<T, int, bool> predicateWithIndex) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.predicateWithIndex = predicateWithIndex;
		}

		public IObservable<T> CombinePredicate(Func<T, bool> combinePredicate)
		{
			if (this.predicate != null)
			{
				return new WhereObservable<T>(this.source, (T x) => this.predicate(x) && combinePredicate(x));
			}
			return new WhereObservable<T>(this, combinePredicate);
		}

		public IObservable<TR> CombineSelector<TR>(Func<T, TR> selector)
		{
			if (this.predicate != null)
			{
				return new WhereSelectObservable<T, TR>(this.source, this.predicate, selector);
			}
			return new SelectObservable<T, TR>(this, selector);
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			if (this.predicate != null)
			{
				return this.source.Subscribe(new WhereObservable<T>.Where(this, observer, cancel));
			}
			return this.source.Subscribe(new WhereObservable<T>.Where_(this, observer, cancel));
		}

		private class Where : OperatorObserverBase<T, T>
		{
			private readonly WhereObservable<T> parent;

			public Where(WhereObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public override void OnNext(T value)
			{
				bool flag = false;
				try
				{
					flag = this.parent.predicate(value);
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

		private class Where_ : OperatorObserverBase<T, T>
		{
			private readonly WhereObservable<T> parent;

			private int index;

			public Where_(WhereObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
				this.index = 0;
			}

			public override void OnNext(T value)
			{
				bool flag = false;
				try
				{
					flag = this.parent.predicateWithIndex(value, this.index++);
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
