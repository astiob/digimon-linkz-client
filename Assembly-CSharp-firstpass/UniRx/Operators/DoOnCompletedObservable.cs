using System;

namespace UniRx.Operators
{
	internal class DoOnCompletedObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly Action onCompleted;

		public DoOnCompletedObservable(IObservable<T> source, Action onCompleted) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.onCompleted = onCompleted;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new DoOnCompletedObservable<T>.DoOnCompleted(this, observer, cancel).Run();
		}

		private class DoOnCompleted : OperatorObserverBase<T, T>
		{
			private readonly DoOnCompletedObservable<T> parent;

			public DoOnCompleted(DoOnCompletedObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
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
					this.parent.onCompleted();
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
