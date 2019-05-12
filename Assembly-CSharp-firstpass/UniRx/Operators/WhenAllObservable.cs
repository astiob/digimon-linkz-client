using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class WhenAllObservable<T> : OperatorObservableBase<T[]>
	{
		private readonly IObservable<T>[] sources;

		private readonly IEnumerable<IObservable<T>> sourcesEnumerable;

		public WhenAllObservable(IObservable<T>[] sources) : base(false)
		{
			this.sources = sources;
		}

		public WhenAllObservable(IEnumerable<IObservable<T>> sources) : base(false)
		{
			this.sourcesEnumerable = sources;
		}

		protected override IDisposable SubscribeCore(IObserver<T[]> observer, IDisposable cancel)
		{
			if (this.sources != null)
			{
				return new WhenAllObservable<T>.WhenAll(this.sources, observer, cancel).Run();
			}
			IList<IObservable<T>> list = this.sourcesEnumerable as IList<IObservable<T>>;
			if (list == null)
			{
				list = new List<IObservable<T>>(this.sourcesEnumerable);
			}
			return new WhenAllObservable<T>.WhenAll_(list, observer, cancel).Run();
		}

		private class WhenAll : OperatorObserverBase<T[], T[]>
		{
			private readonly IObservable<T>[] sources;

			private readonly object gate = new object();

			private int completedCount;

			private int length;

			private T[] values;

			public WhenAll(IObservable<T>[] sources, IObserver<T[]> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.sources = sources;
			}

			public IDisposable Run()
			{
				this.length = this.sources.Length;
				if (this.length == 0)
				{
					this.OnNext(new T[0]);
					try
					{
						this.observer.OnCompleted();
					}
					finally
					{
						base.Dispose();
					}
					return Disposable.Empty;
				}
				this.completedCount = 0;
				this.values = new T[this.length];
				IDisposable[] array = new IDisposable[this.length];
				for (int i = 0; i < this.length; i++)
				{
					IObservable<T> observable = this.sources[i];
					WhenAllObservable<T>.WhenAll.WhenAllCollectionObserver observer = new WhenAllObservable<T>.WhenAll.WhenAllCollectionObserver(this, i);
					array[i] = observable.Subscribe(observer);
				}
				return StableCompositeDisposable.CreateUnsafe(array);
			}

			public override void OnNext(T[] value)
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

			private class WhenAllCollectionObserver : IObserver<T>
			{
				private readonly WhenAllObservable<T>.WhenAll parent;

				private readonly int index;

				private bool isCompleted;

				public WhenAllCollectionObserver(WhenAllObservable<T>.WhenAll parent, int index)
				{
					this.parent = parent;
					this.index = index;
				}

				public void OnNext(T value)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						if (!this.isCompleted)
						{
							this.parent.values[this.index] = value;
						}
					}
				}

				public void OnError(Exception error)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						if (!this.isCompleted)
						{
							this.parent.OnError(error);
						}
					}
				}

				public void OnCompleted()
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						if (!this.isCompleted)
						{
							this.isCompleted = true;
							this.parent.completedCount++;
							if (this.parent.completedCount == this.parent.length)
							{
								this.parent.OnNext(this.parent.values);
								this.parent.OnCompleted();
							}
						}
					}
				}
			}
		}

		private class WhenAll_ : OperatorObserverBase<T[], T[]>
		{
			private readonly IList<IObservable<T>> sources;

			private readonly object gate = new object();

			private int completedCount;

			private int length;

			private T[] values;

			public WhenAll_(IList<IObservable<T>> sources, IObserver<T[]> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.sources = sources;
			}

			public IDisposable Run()
			{
				this.length = this.sources.Count;
				if (this.length == 0)
				{
					this.OnNext(new T[0]);
					try
					{
						this.observer.OnCompleted();
					}
					finally
					{
						base.Dispose();
					}
					return Disposable.Empty;
				}
				this.completedCount = 0;
				this.values = new T[this.length];
				IDisposable[] array = new IDisposable[this.length];
				for (int i = 0; i < this.length; i++)
				{
					IObservable<T> observable = this.sources[i];
					WhenAllObservable<T>.WhenAll_.WhenAllCollectionObserver observer = new WhenAllObservable<T>.WhenAll_.WhenAllCollectionObserver(this, i);
					array[i] = observable.Subscribe(observer);
				}
				return StableCompositeDisposable.CreateUnsafe(array);
			}

			public override void OnNext(T[] value)
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

			private class WhenAllCollectionObserver : IObserver<T>
			{
				private readonly WhenAllObservable<T>.WhenAll_ parent;

				private readonly int index;

				private bool isCompleted;

				public WhenAllCollectionObserver(WhenAllObservable<T>.WhenAll_ parent, int index)
				{
					this.parent = parent;
					this.index = index;
				}

				public void OnNext(T value)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						if (!this.isCompleted)
						{
							this.parent.values[this.index] = value;
						}
					}
				}

				public void OnError(Exception error)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						if (!this.isCompleted)
						{
							this.parent.OnError(error);
						}
					}
				}

				public void OnCompleted()
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						if (!this.isCompleted)
						{
							this.isCompleted = true;
							this.parent.completedCount++;
							if (this.parent.completedCount == this.parent.length)
							{
								this.parent.OnNext(this.parent.values);
								this.parent.OnCompleted();
							}
						}
					}
				}
			}
		}
	}
}
