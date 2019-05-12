using System;

namespace UniRx.Operators
{
	internal class TakeUntilObservable<T, TOther> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly IObservable<TOther> other;

		public TakeUntilObservable(IObservable<T> source, IObservable<TOther> other) : base(source.IsRequiredSubscribeOnCurrentThread<T>() || other.IsRequiredSubscribeOnCurrentThread<TOther>())
		{
			this.source = source;
			this.other = other;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new TakeUntilObservable<T, TOther>.TakeUntil(this, observer, cancel).Run();
		}

		private class TakeUntil : OperatorObserverBase<T, T>
		{
			private readonly TakeUntilObservable<T, TOther> parent;

			private object gate = new object();

			private bool open;

			public TakeUntil(TakeUntilObservable<T, TOther> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				TakeUntilObservable<T, TOther>.TakeUntil.TakeUntilOther observer = new TakeUntilObservable<T, TOther>.TakeUntil.TakeUntilOther(this, singleAssignmentDisposable);
				singleAssignmentDisposable.Disposable = this.parent.other.Subscribe(observer);
				IDisposable disposable = this.parent.source.Subscribe(this);
				return StableCompositeDisposable.Create(singleAssignmentDisposable, disposable);
			}

			public override void OnNext(T value)
			{
				if (this.open)
				{
					this.observer.OnNext(value);
				}
				else
				{
					object obj = this.gate;
					lock (obj)
					{
						this.observer.OnNext(value);
					}
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

			private class TakeUntilOther : IObserver<TOther>
			{
				private readonly TakeUntilObservable<T, TOther>.TakeUntil sourceObserver;

				private readonly IDisposable subscription;

				public TakeUntilOther(TakeUntilObservable<T, TOther>.TakeUntil sourceObserver, IDisposable subscription)
				{
					this.sourceObserver = sourceObserver;
					this.subscription = subscription;
				}

				public void OnNext(TOther value)
				{
					object gate = this.sourceObserver.gate;
					lock (gate)
					{
						try
						{
							this.sourceObserver.observer.OnCompleted();
						}
						finally
						{
							this.sourceObserver.Dispose();
						}
					}
				}

				public void OnError(Exception error)
				{
					object gate = this.sourceObserver.gate;
					lock (gate)
					{
						try
						{
							this.sourceObserver.observer.OnError(error);
						}
						finally
						{
							this.sourceObserver.Dispose();
						}
					}
				}

				public void OnCompleted()
				{
					object gate = this.sourceObserver.gate;
					lock (gate)
					{
						this.sourceObserver.open = true;
						this.subscription.Dispose();
					}
				}
			}
		}
	}
}
