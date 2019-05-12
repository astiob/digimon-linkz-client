using System;

namespace UniRx.Operators
{
	internal class MaterializeObservable<T> : OperatorObservableBase<Notification<T>>
	{
		private readonly IObservable<T> source;

		public MaterializeObservable(IObservable<T> source) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
		}

		protected override IDisposable SubscribeCore(IObserver<Notification<T>> observer, IDisposable cancel)
		{
			return new MaterializeObservable<T>.Materialize(this, observer, cancel).Run();
		}

		private class Materialize : OperatorObserverBase<T, Notification<T>>
		{
			private readonly MaterializeObservable<T> parent;

			public Materialize(MaterializeObservable<T> parent, IObserver<Notification<T>> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				return this.parent.source.Subscribe(this);
			}

			public override void OnNext(T value)
			{
				this.observer.OnNext(Notification.CreateOnNext<T>(value));
			}

			public override void OnError(Exception error)
			{
				this.observer.OnNext(Notification.CreateOnError<T>(error));
				try
				{
					this.observer.OnCompleted();
				}
				finally
				{
					base.Dispose();
				}
			}

			public override void OnCompleted()
			{
				this.observer.OnNext(Notification.CreateOnCompleted<T>());
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
