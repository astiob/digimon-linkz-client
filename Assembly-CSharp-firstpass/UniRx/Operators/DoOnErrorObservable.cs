using System;

namespace UniRx.Operators
{
	internal class DoOnErrorObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly Action<Exception> onError;

		public DoOnErrorObservable(IObservable<T> source, Action<Exception> onError) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.onError = onError;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new DoOnErrorObservable<T>.DoOnError(this, observer, cancel).Run();
		}

		private class DoOnError : OperatorObserverBase<T, T>
		{
			private readonly DoOnErrorObservable<T> parent;

			public DoOnError(DoOnErrorObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
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
					this.parent.onError(error);
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
