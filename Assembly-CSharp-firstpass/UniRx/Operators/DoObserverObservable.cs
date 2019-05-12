using System;

namespace UniRx.Operators
{
	internal class DoObserverObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly IObserver<T> observer;

		public DoObserverObservable(IObservable<T> source, IObserver<T> observer) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.observer = observer;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new DoObserverObservable<T>.Do(this, observer, cancel).Run();
		}

		private class Do : OperatorObserverBase<T, T>
		{
			private readonly DoObserverObservable<T> parent;

			public Do(DoObserverObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				return this.parent.source.Subscribe(this);
			}

			public override void OnNext(T value)
			{
				try
				{
					this.parent.observer.OnNext(value);
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
				this.observer.OnNext(value);
			}

			public override void OnError(Exception error)
			{
				try
				{
					this.parent.observer.OnError(error);
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
					this.parent.observer.OnCompleted();
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
