using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class ZipObservable<TLeft, TRight, TResult> : OperatorObservableBase<TResult>
	{
		private readonly IObservable<TLeft> left;

		private readonly IObservable<TRight> right;

		private readonly Func<TLeft, TRight, TResult> selector;

		public ZipObservable(IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, TRight, TResult> selector) : base(left.IsRequiredSubscribeOnCurrentThread<TLeft>() || right.IsRequiredSubscribeOnCurrentThread<TRight>())
		{
			this.left = left;
			this.right = right;
			this.selector = selector;
		}

		protected override IDisposable SubscribeCore(IObserver<TResult> observer, IDisposable cancel)
		{
			return new ZipObservable<TLeft, TRight, TResult>.Zip(this, observer, cancel).Run();
		}

		private class Zip : OperatorObserverBase<TResult, TResult>
		{
			private readonly ZipObservable<TLeft, TRight, TResult> parent;

			private readonly object gate = new object();

			private readonly Queue<TLeft> leftQ = new Queue<TLeft>();

			private bool leftCompleted;

			private readonly Queue<TRight> rightQ = new Queue<TRight>();

			private bool rightCompleted;

			public Zip(ZipObservable<TLeft, TRight, TResult> parent, IObserver<TResult> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				IDisposable disposable = this.parent.left.Subscribe(new ZipObservable<TLeft, TRight, TResult>.Zip.LeftZipObserver(this));
				IDisposable disposable2 = this.parent.right.Subscribe(new ZipObservable<TLeft, TRight, TResult>.Zip.RightZipObserver(this));
				return StableCompositeDisposable.Create(disposable, disposable2, Disposable.Create(delegate
				{
					object obj = this.gate;
					lock (obj)
					{
						this.leftQ.Clear();
						this.rightQ.Clear();
					}
				}));
			}

			private void Dequeue()
			{
				if (this.leftQ.Count != 0 && this.rightQ.Count != 0)
				{
					TLeft arg = this.leftQ.Dequeue();
					TRight arg2 = this.rightQ.Dequeue();
					TResult value;
					try
					{
						value = this.parent.selector(arg, arg2);
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

			private class LeftZipObserver : IObserver<TLeft>
			{
				private readonly ZipObservable<TLeft, TRight, TResult>.Zip parent;

				public LeftZipObserver(ZipObservable<TLeft, TRight, TResult>.Zip parent)
				{
					this.parent = parent;
				}

				public void OnNext(TLeft value)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						this.parent.leftQ.Enqueue(value);
						this.parent.Dequeue();
					}
				}

				public void OnError(Exception ex)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						this.parent.OnError(ex);
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

			private class RightZipObserver : IObserver<TRight>
			{
				private readonly ZipObservable<TLeft, TRight, TResult>.Zip parent;

				public RightZipObserver(ZipObservable<TLeft, TRight, TResult>.Zip parent)
				{
					this.parent = parent;
				}

				public void OnNext(TRight value)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						this.parent.rightQ.Enqueue(value);
						this.parent.Dequeue();
					}
				}

				public void OnError(Exception ex)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						this.parent.OnError(ex);
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
