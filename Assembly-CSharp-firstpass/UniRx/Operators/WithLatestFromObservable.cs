using System;

namespace UniRx.Operators
{
	internal class WithLatestFromObservable<TLeft, TRight, TResult> : OperatorObservableBase<TResult>
	{
		private readonly IObservable<TLeft> left;

		private readonly IObservable<TRight> right;

		private readonly Func<TLeft, TRight, TResult> selector;

		public WithLatestFromObservable(IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, TRight, TResult> selector) : base(left.IsRequiredSubscribeOnCurrentThread<TLeft>() || right.IsRequiredSubscribeOnCurrentThread<TRight>())
		{
			this.left = left;
			this.right = right;
			this.selector = selector;
		}

		protected override IDisposable SubscribeCore(IObserver<TResult> observer, IDisposable cancel)
		{
			return new WithLatestFromObservable<TLeft, TRight, TResult>.WithLatestFrom(this, observer, cancel).Run();
		}

		private class WithLatestFrom : OperatorObserverBase<TResult, TResult>
		{
			private readonly WithLatestFromObservable<TLeft, TRight, TResult> parent;

			private readonly object gate = new object();

			private volatile bool hasLatest;

			private TRight latestValue = default(TRight);

			public WithLatestFrom(WithLatestFromObservable<TLeft, TRight, TResult> parent, IObserver<TResult> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				IDisposable disposable = this.parent.left.Subscribe(new WithLatestFromObservable<TLeft, TRight, TResult>.WithLatestFrom.LeftObserver(this));
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				singleAssignmentDisposable.Disposable = this.parent.right.Subscribe(new WithLatestFromObservable<TLeft, TRight, TResult>.WithLatestFrom.RightObserver(this, singleAssignmentDisposable));
				return StableCompositeDisposable.Create(disposable, singleAssignmentDisposable);
			}

			public override void OnNext(TResult value)
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

			private class LeftObserver : IObserver<TLeft>
			{
				private readonly WithLatestFromObservable<TLeft, TRight, TResult>.WithLatestFrom parent;

				public LeftObserver(WithLatestFromObservable<TLeft, TRight, TResult>.WithLatestFrom parent)
				{
					this.parent = parent;
				}

				public void OnNext(TLeft value)
				{
					if (this.parent.hasLatest)
					{
						TResult value2 = default(TResult);
						try
						{
							value2 = this.parent.parent.selector(value, this.parent.latestValue);
						}
						catch (Exception error)
						{
							object gate = this.parent.gate;
							lock (gate)
							{
								this.parent.OnError(error);
							}
							return;
						}
						object gate2 = this.parent.gate;
						lock (gate2)
						{
							this.parent.OnNext(value2);
						}
					}
				}

				public void OnError(Exception error)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						this.parent.OnError(error);
					}
				}

				public void OnCompleted()
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						this.parent.OnCompleted();
					}
				}
			}

			private class RightObserver : IObserver<TRight>
			{
				private readonly WithLatestFromObservable<TLeft, TRight, TResult>.WithLatestFrom parent;

				private readonly IDisposable selfSubscription;

				public RightObserver(WithLatestFromObservable<TLeft, TRight, TResult>.WithLatestFrom parent, IDisposable subscription)
				{
					this.parent = parent;
					this.selfSubscription = subscription;
				}

				public void OnNext(TRight value)
				{
					this.parent.latestValue = value;
					this.parent.hasLatest = true;
				}

				public void OnError(Exception error)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						this.parent.OnError(error);
					}
				}

				public void OnCompleted()
				{
					this.selfSubscription.Dispose();
				}
			}
		}
	}
}
