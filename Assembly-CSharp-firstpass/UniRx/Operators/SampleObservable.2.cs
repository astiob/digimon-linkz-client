using System;

namespace UniRx.Operators
{
	internal class SampleObservable<T, T2> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly IObservable<T2> intervalSource;

		public SampleObservable(IObservable<T> source, IObservable<T2> intervalSource) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.intervalSource = intervalSource;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new SampleObservable<T, T2>.Sample(this, observer, cancel).Run();
		}

		private class Sample : OperatorObserverBase<T, T>
		{
			private readonly SampleObservable<T, T2> parent;

			private readonly object gate = new object();

			private T latestValue = default(T);

			private bool isUpdated;

			private bool isCompleted;

			private SingleAssignmentDisposable sourceSubscription;

			public Sample(SampleObservable<T, T2> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.sourceSubscription = new SingleAssignmentDisposable();
				this.sourceSubscription.Disposable = this.parent.source.Subscribe(this);
				IDisposable disposable = this.parent.intervalSource.Subscribe(new SampleObservable<T, T2>.Sample.SampleTick(this));
				return StableCompositeDisposable.Create(this.sourceSubscription, disposable);
			}

			public override void OnNext(T value)
			{
				object obj = this.gate;
				lock (obj)
				{
					this.latestValue = value;
					this.isUpdated = true;
				}
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
					this.isCompleted = true;
					this.sourceSubscription.Dispose();
				}
			}

			private class SampleTick : IObserver<T2>
			{
				private readonly SampleObservable<T, T2>.Sample parent;

				public SampleTick(SampleObservable<T, T2>.Sample parent)
				{
					this.parent = parent;
				}

				public void OnCompleted()
				{
				}

				public void OnError(Exception error)
				{
				}

				public void OnNext(T2 _)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						if (this.parent.isUpdated)
						{
							T latestValue = this.parent.latestValue;
							this.parent.isUpdated = false;
							this.parent.observer.OnNext(latestValue);
						}
						if (this.parent.isCompleted)
						{
							try
							{
								this.parent.observer.OnCompleted();
							}
							finally
							{
								this.parent.Dispose();
							}
						}
					}
				}
			}
		}
	}
}
