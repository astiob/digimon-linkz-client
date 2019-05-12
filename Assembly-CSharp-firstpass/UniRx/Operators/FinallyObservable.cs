using System;

namespace UniRx.Operators
{
	internal class FinallyObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly Action finallyAction;

		public FinallyObservable(IObservable<T> source, Action finallyAction) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.finallyAction = finallyAction;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new FinallyObservable<T>.Finally(this, observer, cancel).Run();
		}

		private class Finally : OperatorObserverBase<T, T>
		{
			private readonly FinallyObservable<T> parent;

			public Finally(FinallyObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				IDisposable disposable;
				try
				{
					disposable = this.parent.source.Subscribe(this);
				}
				catch
				{
					this.parent.finallyAction();
					throw;
				}
				return StableCompositeDisposable.Create(disposable, Disposable.Create(delegate
				{
					this.parent.finallyAction();
				}));
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
