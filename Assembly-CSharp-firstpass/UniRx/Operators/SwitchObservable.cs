using System;

namespace UniRx.Operators
{
	internal class SwitchObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<IObservable<T>> sources;

		public SwitchObservable(IObservable<IObservable<T>> sources) : base(true)
		{
			this.sources = sources;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new SwitchObservable<T>.SwitchObserver(this, observer, cancel).Run();
		}

		private class SwitchObserver : OperatorObserverBase<IObservable<T>, T>
		{
			private readonly SwitchObservable<T> parent;

			private readonly object gate = new object();

			private readonly SerialDisposable innerSubscription = new SerialDisposable();

			private bool isStopped;

			private ulong latest;

			private bool hasLatest;

			public SwitchObserver(SwitchObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				IDisposable disposable = this.parent.sources.Subscribe(this);
				return StableCompositeDisposable.Create(disposable, this.innerSubscription);
			}

			public override void OnNext(IObservable<T> value)
			{
				ulong id = 0UL;
				object obj = this.gate;
				lock (obj)
				{
					id = (this.latest += 1UL);
					this.hasLatest = true;
				}
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				this.innerSubscription.Disposable = singleAssignmentDisposable;
				singleAssignmentDisposable.Disposable = value.Subscribe(new SwitchObservable<T>.SwitchObserver.Switch(this, id));
			}

			public override void OnError(Exception error)
			{
				object obj = this.gate;
				lock (obj)
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
				object obj = this.gate;
				lock (obj)
				{
					this.isStopped = true;
					if (!this.hasLatest)
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

			private class Switch : IObserver<T>
			{
				private readonly SwitchObservable<T>.SwitchObserver parent;

				private readonly ulong id;

				public Switch(SwitchObservable<T>.SwitchObserver observer, ulong id)
				{
					this.parent = observer;
					this.id = id;
				}

				public void OnNext(T value)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						if (this.parent.latest == this.id)
						{
							this.parent.observer.OnNext(value);
						}
					}
				}

				public void OnError(Exception error)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						if (this.parent.latest == this.id)
						{
							this.parent.observer.OnError(error);
						}
					}
				}

				public void OnCompleted()
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						if (this.parent.latest == this.id)
						{
							this.parent.hasLatest = false;
							if (this.parent.isStopped)
							{
								this.parent.observer.OnCompleted();
							}
						}
					}
				}
			}
		}
	}
}
