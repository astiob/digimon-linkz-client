using System;

namespace UniRx.Operators
{
	internal class SynchronizeObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly object gate;

		public SynchronizeObservable(IObservable<T> source, object gate) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.gate = gate;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new SynchronizeObservable<T>.Synchronize(this, observer, cancel));
		}

		private class Synchronize : OperatorObserverBase<T, T>
		{
			private readonly SynchronizeObservable<T> parent;

			public Synchronize(SynchronizeObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public override void OnNext(T value)
			{
				object gate = this.parent.gate;
				lock (gate)
				{
					this.observer.OnNext(value);
				}
			}

			public override void OnError(Exception error)
			{
				object gate = this.parent.gate;
				lock (gate)
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
			}

			public override void OnCompleted()
			{
				object gate = this.parent.gate;
				lock (gate)
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
}
