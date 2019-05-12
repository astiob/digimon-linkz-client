using System;

namespace UniRx.Operators
{
	internal class DoOnTerminateObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly Action onTerminate;

		public DoOnTerminateObservable(IObservable<T> source, Action onTerminate) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.onTerminate = onTerminate;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new DoOnTerminateObservable<T>.DoOnTerminate(this, observer, cancel).Run();
		}

		private class DoOnTerminate : OperatorObserverBase<T, T>
		{
			private readonly DoOnTerminateObservable<T> parent;

			public DoOnTerminate(DoOnTerminateObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				return this.parent.source.Subscribe(this);
			}

			public override void OnNext(T value)
			{
				this.observer.OnNext(value);
			}

			public override void OnError(Exception error)
			{
				try
				{
					this.parent.onTerminate();
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
					this.parent.onTerminate();
				}
				catch (Exception error)
				{
					this.observer.OnError(error);
					base.Dispose();
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
