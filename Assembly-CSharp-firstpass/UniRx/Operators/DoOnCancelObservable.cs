using System;

namespace UniRx.Operators
{
	internal class DoOnCancelObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly Action onCancel;

		public DoOnCancelObservable(IObservable<T> source, Action onCancel) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.onCancel = onCancel;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new DoOnCancelObservable<T>.DoOnCancel(this, observer, cancel).Run();
		}

		private class DoOnCancel : OperatorObserverBase<T, T>
		{
			private readonly DoOnCancelObservable<T> parent;

			private bool isCompletedCall;

			public DoOnCancel(DoOnCancelObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				return StableCompositeDisposable.Create(this.parent.source.Subscribe(this), Disposable.Create(delegate
				{
					if (!this.isCompletedCall)
					{
						this.parent.onCancel();
					}
				}));
			}

			public override void OnNext(T value)
			{
				this.observer.OnNext(value);
			}

			public override void OnError(Exception error)
			{
				this.isCompletedCall = true;
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
				this.isCompletedCall = true;
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
