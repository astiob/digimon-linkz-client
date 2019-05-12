using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class ZipLatestObservable<T> : OperatorObservableBase<IList<T>>
	{
		private readonly IObservable<T>[] sources;

		public ZipLatestObservable(IObservable<T>[] sources) : base(true)
		{
			this.sources = sources;
		}

		protected override IDisposable SubscribeCore(IObserver<IList<T>> observer, IDisposable cancel)
		{
			return new ZipLatestObservable<T>.ZipLatest(this, observer, cancel).Run();
		}

		private class ZipLatest : OperatorObserverBase<IList<T>, IList<T>>
		{
			private readonly ZipLatestObservable<T> parent;

			private readonly object gate = new object();

			private int length;

			private T[] values;

			private bool[] isStarted;

			private bool[] isCompleted;

			public ZipLatest(ZipLatestObservable<T> parent, IObserver<IList<T>> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.length = this.parent.sources.Length;
				this.values = new T[this.length];
				this.isStarted = new bool[this.length];
				this.isCompleted = new bool[this.length];
				IDisposable[] array = new IDisposable[this.length];
				for (int i = 0; i < this.length; i++)
				{
					IObservable<T> observable = this.parent.sources[i];
					array[i] = observable.Subscribe(new ZipLatestObservable<T>.ZipLatest.ZipLatestObserver(this, i));
				}
				return StableCompositeDisposable.CreateUnsafe(array);
			}

			private void Publish(int index)
			{
				this.isStarted[index] = true;
				bool flag = false;
				bool flag2 = true;
				for (int i = 0; i < this.length; i++)
				{
					if (!this.isStarted[i])
					{
						flag2 = false;
						break;
					}
					if (i != index)
					{
						if (this.isCompleted[i])
						{
							flag = true;
						}
					}
				}
				if (!flag2)
				{
					for (int j = 0; j < this.length; j++)
					{
						if (j != index)
						{
							if (this.isCompleted[j] && !this.isStarted[j])
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
					}
					return;
				}
				this.OnNext(new List<T>(this.values));
				if (flag)
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
				Array.Clear(this.isStarted, 0, this.length);
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

			private class ZipLatestObserver : IObserver<T>
			{
				private readonly ZipLatestObservable<T>.ZipLatest parent;

				private readonly int index;

				public ZipLatestObserver(ZipLatestObservable<T>.ZipLatest parent, int index)
				{
					this.parent = parent;
					this.index = index;
				}

				public void OnNext(T value)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						this.parent.values[this.index] = value;
						this.parent.Publish(this.index);
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
						this.parent.isCompleted[this.index] = true;
						bool flag = true;
						for (int i = 0; i < this.parent.length; i++)
						{
							if (!this.parent.isCompleted[i])
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
