using System;

namespace UniRx.Operators
{
	internal class RefCountObservable<T> : OperatorObservableBase<T>
	{
		private readonly IConnectableObservable<T> source;

		private readonly object gate = new object();

		private int refCount;

		private IDisposable connection;

		public RefCountObservable(IConnectableObservable<T> source) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new RefCountObservable<T>.RefCount(this, observer, cancel).Run();
		}

		private class RefCount : OperatorObserverBase<T, T>
		{
			private readonly RefCountObservable<T> parent;

			public RefCount(RefCountObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				IDisposable subcription = this.parent.source.Subscribe(this);
				object gate = this.parent.gate;
				lock (gate)
				{
					if (++this.parent.refCount == 1)
					{
						this.parent.connection = this.parent.source.Connect();
					}
				}
				return Disposable.Create(delegate
				{
					subcription.Dispose();
					object gate2 = this.parent.gate;
					lock (gate2)
					{
						if (--this.parent.refCount == 0)
						{
							this.parent.connection.Dispose();
						}
					}
				});
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
