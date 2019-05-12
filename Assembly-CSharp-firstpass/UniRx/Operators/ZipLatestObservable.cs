using System;

namespace UniRx.Operators
{
	internal class ZipLatestObservable<TLeft, TRight, TResult> : OperatorObservableBase<TResult>
	{
		private readonly IObservable<TLeft> left;

		private readonly IObservable<TRight> right;

		private readonly Func<TLeft, TRight, TResult> selector;

		public ZipLatestObservable(IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, TRight, TResult> selector) : base(left.IsRequiredSubscribeOnCurrentThread<TLeft>() || right.IsRequiredSubscribeOnCurrentThread<TRight>())
		{
			this.left = left;
			this.right = right;
			this.selector = selector;
		}

		protected override IDisposable SubscribeCore(IObserver<TResult> observer, IDisposable cancel)
		{
			return new ZipLatestObservable<TLeft, TRight, TResult>.ZipLatest(this, observer, cancel).Run();
		}

		private class ZipLatest : OperatorObserverBase<TResult, TResult>
		{
			private readonly ZipLatestObservable<TLeft, TRight, TResult> parent;

			private readonly object gate = new object();

			private TLeft leftValue = default(TLeft);

			private bool leftStarted;

			private bool leftCompleted;

			private TRight rightValue = default(TRight);

			private bool rightStarted;

			private bool rightCompleted;

			public ZipLatest(ZipLatestObservable<TLeft, TRight, TResult> parent, IObserver<TResult> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				IDisposable disposable = this.parent.left.Subscribe(new ZipLatestObservable<TLeft, TRight, TResult>.ZipLatest.LeftObserver(this));
				IDisposable disposable2 = this.parent.right.Subscribe(new ZipLatestObservable<TLeft, TRight, TResult>.ZipLatest.RightObserver(this));
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
						this.leftStarted = false;
						this.rightStarted = false;
						if (!this.leftCompleted)
						{
							if (!this.rightCompleted)
							{
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
				private readonly ZipLatestObservable<TLeft, TRight, TResult>.ZipLatest parent;

				public LeftObserver(ZipLatestObservable<TLeft, TRight, TResult>.ZipLatest parent)
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
				private readonly ZipLatestObservable<TLeft, TRight, TResult>.ZipLatest parent;

				public RightObserver(ZipLatestObservable<TLeft, TRight, TResult>.ZipLatest parent)
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
