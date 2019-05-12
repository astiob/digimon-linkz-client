using System;

namespace UniRx.Operators
{
	internal class TakeWhileObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly Func<T, bool> predicate;

		private readonly Func<T, int, bool> predicateWithIndex;

		public TakeWhileObservable(IObservable<T> source, Func<T, bool> predicate) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.predicate = predicate;
		}

		public TakeWhileObservable(IObservable<T> source, Func<T, int, bool> predicateWithIndex) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.predicateWithIndex = predicateWithIndex;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			if (this.predicate != null)
			{
				return new TakeWhileObservable<T>.TakeWhile(this, observer, cancel).Run();
			}
			return new TakeWhileObservable<T>.TakeWhile_(this, observer, cancel).Run();
		}

		private class TakeWhile : OperatorObserverBase<T, T>
		{
			private readonly TakeWhileObservable<T> parent;

			public TakeWhile(TakeWhileObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				return this.parent.source.Subscribe(this);
			}

			public override void OnNext(T value)
			{
				bool flag;
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
				else
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

		private class TakeWhile_ : OperatorObserverBase<T, T>
		{
			private readonly TakeWhileObservable<T> parent;

			private int index;

			public TakeWhile_(TakeWhileObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				return this.parent.source.Subscribe(this);
			}

			public override void OnNext(T value)
			{
				bool flag;
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
				else
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
