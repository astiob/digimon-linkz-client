using System;

namespace UniRx.Operators
{
	internal class CombineLatestObservable<TLeft, TRight, TResult> : OperatorObservableBase<TResult>
	{
		private readonly IObservable<TLeft> left;

		private readonly IObservable<TRight> right;

		private readonly Func<TLeft, TRight, TResult> selector;

		public CombineLatestObservable(IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, TRight, TResult> selector) : base(left.IsRequiredSubscribeOnCurrentThread<TLeft>() || right.IsRequiredSubscribeOnCurrentThread<TRight>())
		{
			this.left = left;
			this.right = right;
			this.selector = selector;
		}

		protected override IDisposable SubscribeCore(IObserver<TResult> observer, IDisposable cancel)
		{
			return new CombineLatestObservable<TLeft, TRight, TResult>.CombineLatest(this, observer, cancel).Run();
		}

		private class CombineLatest : OperatorObserverBase<TResult, TResult>
		{
			private readonly CombineLatestObservable<TLeft, TRight, TResult> parent;

			private readonly object gate = new object();

			private TLeft leftValue = default(TLeft);

			private bool leftStarted;

			private bool leftCompleted;

			private TRight rightValue = default(TRight);

			private bool rightStarted;

			private bool rightCompleted;

			public CombineLatest(CombineLatestObservable<TLeft, TRight, TResult> parent, IObserver<TResult> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				IDisposable disposable = this.parent.left.Subscribe(new CombineLatestObservable<TLeft, TRight, TResult>.CombineLatest.LeftObserver(this));
				IDisposable disposable2 = this.parent.right.Subscribe(new CombineLatestObservable<TLeft, TRight, TResult>.CombineLatest.RightObserver(this));
				return StableCompositeDisposable.Create(disposable, disposable2);
			}

			public void Publish()
			{
				if (!this.leftCompleted || this.leftStarted)
				{
					if (!this.rightCompleted || this.rightStarted)
					{
						if (!this.leftStarted || !this.rightStarted)
						{
							return;
						}
						TResult value;
						try
						{
							value = this.parent.selector(this.leftValue, this.rightValue);
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
							return;
						}
						this.OnNext(value);
						return;
					}
				}
				try
				{
					this.observer.OnCompleted();
				}
				finally
				{
					base.Dispose();
				}
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
				private readonly CombineLatestObservable<TLeft, TRight, TResult>.CombineLatest parent;

				public LeftObserver(CombineLatestObservable<TLeft, TRight, TResult>.CombineLatest parent)
				{
					this.parent = parent;
				}

				public void OnNext(TLeft value)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						this.parent.leftStarted = true;
						this.parent.leftValue = value;
						this.parent.Publish();
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
						this.parent.leftCompleted = true;
						if (this.parent.rightCompleted)
						{
							this.parent.OnCompleted();
						}
					}
				}
			}

			private class RightObserver : IObserver<TRight>
			{
				private readonly CombineLatestObservable<TLeft, TRight, TResult>.CombineLatest parent;

				public RightObserver(CombineLatestObservable<TLeft, TRight, TResult>.CombineLatest parent)
				{
					this.parent = parent;
				}

				public void OnNext(TRight value)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						this.parent.rightStarted = true;
						this.parent.rightValue = value;
						this.parent.Publish();
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
						this.parent.rightCompleted = true;
						if (this.parent.leftCompleted)
						{
							this.parent.OnCompleted();
						}
					}
				}
			}
		}
	}
}
