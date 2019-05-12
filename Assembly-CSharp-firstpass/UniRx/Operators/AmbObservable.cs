using System;
using UniRx.InternalUtil;

namespace UniRx.Operators
{
	internal class AmbObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly IObservable<T> second;

		public AmbObservable(IObservable<T> source, IObservable<T> second) : base(source.IsRequiredSubscribeOnCurrentThread<T>() || second.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.second = second;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new AmbObservable<T>.AmbOuterObserver(this, observer, cancel).Run();
		}

		private class AmbOuterObserver : OperatorObserverBase<T, T>
		{
			private readonly AmbObservable<T> parent;

			private readonly object gate = new object();

			private SingleAssignmentDisposable leftSubscription;

			private SingleAssignmentDisposable rightSubscription;

			private AmbObservable<T>.AmbOuterObserver.AmbState choice = AmbObservable<T>.AmbOuterObserver.AmbState.Neither;

			public AmbOuterObserver(AmbObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.leftSubscription = new SingleAssignmentDisposable();
				this.rightSubscription = new SingleAssignmentDisposable();
				ICancelable cancelable = StableCompositeDisposable.Create(this.leftSubscription, this.rightSubscription);
				AmbObservable<T>.AmbOuterObserver.Amb amb = new AmbObservable<T>.AmbOuterObserver.Amb();
				amb.targetDisposable = cancelable;
				amb.targetObserver = new AmbObservable<T>.AmbOuterObserver.AmbDecisionObserver(this, AmbObservable<T>.AmbOuterObserver.AmbState.Left, this.rightSubscription, amb);
				AmbObservable<T>.AmbOuterObserver.Amb amb2 = new AmbObservable<T>.AmbOuterObserver.Amb();
				amb2.targetDisposable = cancelable;
				amb2.targetObserver = new AmbObservable<T>.AmbOuterObserver.AmbDecisionObserver(this, AmbObservable<T>.AmbOuterObserver.AmbState.Right, this.leftSubscription, amb2);
				this.leftSubscription.Disposable = this.parent.source.Subscribe(amb);
				this.rightSubscription.Disposable = this.parent.second.Subscribe(amb2);
				return cancelable;
			}

			public override void OnNext(T value)
			{
			}

			public override void OnError(Exception error)
			{
			}

			public override void OnCompleted()
			{
			}

			private enum AmbState
			{
				Left,
				Right,
				Neither
			}

			private class Amb : IObserver<T>
			{
				public IObserver<T> targetObserver;

				public IDisposable targetDisposable;

				public void OnNext(T value)
				{
					this.targetObserver.OnNext(value);
				}

				public void OnError(Exception error)
				{
					try
					{
						this.targetObserver.OnError(error);
					}
					finally
					{
						this.targetObserver = EmptyObserver<T>.Instance;
						this.targetDisposable.Dispose();
					}
				}

				public void OnCompleted()
				{
					try
					{
						this.targetObserver.OnCompleted();
					}
					finally
					{
						this.targetObserver = EmptyObserver<T>.Instance;
						this.targetDisposable.Dispose();
					}
				}
			}

			private class AmbDecisionObserver : IObserver<T>
			{
				private readonly AmbObservable<T>.AmbOuterObserver parent;

				private readonly AmbObservable<T>.AmbOuterObserver.AmbState me;

				private readonly IDisposable otherSubscription;

				private readonly AmbObservable<T>.AmbOuterObserver.Amb self;

				public AmbDecisionObserver(AmbObservable<T>.AmbOuterObserver parent, AmbObservable<T>.AmbOuterObserver.AmbState me, IDisposable otherSubscription, AmbObservable<T>.AmbOuterObserver.Amb self)
				{
					this.parent = parent;
					this.me = me;
					this.otherSubscription = otherSubscription;
					this.self = self;
				}

				public void OnNext(T value)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						if (this.parent.choice == AmbObservable<T>.AmbOuterObserver.AmbState.Neither)
						{
							this.parent.choice = this.me;
							this.otherSubscription.Dispose();
							this.self.targetObserver = this.parent.observer;
						}
						if (this.parent.choice == this.me)
						{
							this.self.targetObserver.OnNext(value);
						}
					}
				}

				public void OnError(Exception error)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						if (this.parent.choice == AmbObservable<T>.AmbOuterObserver.AmbState.Neither)
						{
							this.parent.choice = this.me;
							this.otherSubscription.Dispose();
							this.self.targetObserver = this.parent.observer;
						}
						if (this.parent.choice == this.me)
						{
							this.self.targetObserver.OnError(error);
						}
					}
				}

				public void OnCompleted()
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						if (this.parent.choice == AmbObservable<T>.AmbOuterObserver.AmbState.Neither)
						{
							this.parent.choice = this.me;
							this.otherSubscription.Dispose();
							this.self.targetObserver = this.parent.observer;
						}
						if (this.parent.choice == this.me)
						{
							this.self.targetObserver.OnCompleted();
						}
					}
				}
			}
		}
	}
}
