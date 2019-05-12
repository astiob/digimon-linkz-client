using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class ZipObservable<T> : OperatorObservableBase<IList<T>>
	{
		private readonly IObservable<T>[] sources;

		public ZipObservable(IObservable<T>[] sources) : base(true)
		{
			this.sources = sources;
		}

		protected override IDisposable SubscribeCore(IObserver<IList<T>> observer, IDisposable cancel)
		{
			return new ZipObservable<T>.Zip(this, observer, cancel).Run();
		}

		private class Zip : OperatorObserverBase<IList<T>, IList<T>>
		{
			private readonly ZipObservable<T> parent;

			private readonly object gate = new object();

			private Queue<T>[] queues;

			private bool[] isDone;

			private int length;

			public Zip(ZipObservable<T> parent, IObserver<IList<T>> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.length = this.parent.sources.Length;
				this.queues = new Queue<T>[this.length];
				this.isDone = new bool[this.length];
				for (int i = 0; i < this.length; i++)
				{
					this.queues[i] = new Queue<T>();
				}
				IDisposable[] array = new IDisposable[this.length + 1];
				for (int j = 0; j < this.length; j++)
				{
					IObservable<T> observable = this.parent.sources[j];
					array[j] = observable.Subscribe(new ZipObservable<T>.Zip.ZipObserver(this, j));
				}
				array[this.length] = Disposable.Create(delegate
				{
					object obj = this.gate;
					lock (obj)
					{
						for (int k = 0; k < this.length; k++)
						{
							Queue<T> queue = this.queues[k];
							queue.Clear();
						}
					}
				});
				return StableCompositeDisposable.CreateUnsafe(array);
			}

			private void Dequeue(int index)
			{
				bool flag = true;
				for (int i = 0; i < this.length; i++)
				{
					if (this.queues[i].Count == 0)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					T[] array = new T[this.length];
					for (int j = 0; j < this.length; j++)
					{
						array[j] = this.queues[j].Dequeue();
					}
					this.OnNext(array);
					return;
				}
				bool flag2 = true;
				for (int k = 0; k < this.length; k++)
				{
					if (k != index)
					{
						if (!this.isDone[k])
						{
							flag2 = false;
							break;
						}
					}
				}
				if (flag2)
				{
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

			public override void OnNext(IList<T> value)
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

			private class ZipObserver : IObserver<T>
			{
				private readonly ZipObservable<T>.Zip parent;

				private readonly int index;

				public ZipObserver(ZipObservable<T>.Zip parent, int index)
				{
					this.parent = parent;
					this.index = index;
				}

				public void OnNext(T value)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						this.parent.queues[this.index].Enqueue(value);
						this.parent.Dequeue(this.index);
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
						this.parent.isDone[this.index] = true;
						bool flag = true;
						for (int i = 0; i < this.parent.length; i++)
						{
							if (!this.parent.isDone[i])
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							this.parent.OnCompleted();
						}
					}
				}
			}
		}
	}
}
