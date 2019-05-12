using System;

namespace UniRx.Operators
{
	internal class DoOnSubscribeObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly Action onSubscribe;

		public DoOnSubscribeObservable(IObservable<T> source, Action onSubscribe) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.onSubscribe = onSubscribe;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new DoOnSubscribeObservable<T>.DoOnSubscribe(this, observer, cancel).Run();
		}

		private class DoOnSubscribe : OperatorObserverBase<T, T>
		{
			private readonly DoOnSubscribeObservable<T> parent;

			public DoOnSubscribe(DoOnSubscribeObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				try
				{
					this.parent.onSubscribe();
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
					return Disposable.Empty;
				}
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
