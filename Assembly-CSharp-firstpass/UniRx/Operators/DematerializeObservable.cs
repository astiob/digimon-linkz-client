using System;

namespace UniRx.Operators
{
	internal class DematerializeObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<Notification<T>> source;

		public DematerializeObservable(IObservable<Notification<T>> source) : base(source.IsRequiredSubscribeOnCurrentThread<Notification<T>>())
		{
			this.source = source;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new DematerializeObservable<T>.Dematerialize(this, observer, cancel).Run();
		}

		private class Dematerialize : OperatorObserverBase<Notification<T>, T>
		{
			private readonly DematerializeObservable<T> parent;

			public Dematerialize(DematerializeObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				return this.parent.source.Subscribe(this);
			}

			public override void OnNext(Notification<T> value)
			{
				switch (value.Kind)
				{
				case NotificationKind.OnNext:
					this.observer.OnNext(value.Value);
					break;
				case NotificationKind.OnError:
					try
					{
						this.observer.OnError(value.Exception);
					}
					finally
					{
						base.Dispose();
					}
					break;
				case NotificationKind.OnCompleted:
					try
					{
						this.observer.OnCompleted();
					}
					finally
					{
						base.Dispose();
					}
					break;
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
