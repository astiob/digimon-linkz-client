using System;

namespace UniRx.Operators
{
	internal class WhereSelectObservable<T, TR> : OperatorObservableBase<TR>
	{
		private readonly IObservable<T> source;

		private readonly Func<T, bool> predicate;

		private readonly Func<T, TR> selector;

		public WhereSelectObservable(IObservable<T> source, Func<T, bool> predicate, Func<T, TR> selector) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.predicate = predicate;
			this.selector = selector;
		}

		protected override IDisposable SubscribeCore(IObserver<TR> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new WhereSelectObservable<T, TR>.WhereSelect(this, observer, cancel));
		}

		private class WhereSelect : OperatorObserverBase<T, TR>
		{
			private readonly WhereSelectObservable<T, TR> parent;

			public WhereSelect(WhereSelectObservable<T, TR> parent, IObserver<TR> observer, IDisposable cancel) : base(observer, cancel)
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
					TR value2 = default(TR);
					try
					{
						value2 = this.parent.selector(value);
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
					this.observer.OnNext(value2);
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
