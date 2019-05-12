using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class CombineLatestObservable<T> : OperatorObservableBase<IList<T>>
	{
		private readonly IObservable<T>[] sources;

		public CombineLatestObservable(IObservable<T>[] sources) : base(true)
		{
			this.sources = sources;
		}

		protected override IDisposable SubscribeCore(IObserver<IList<T>> observer, IDisposable cancel)
		{
			return new CombineLatestObservable<T>.CombineLatest(this, observer, cancel).Run();
		}

		private class CombineLatest : OperatorObserverBase<IList<T>, IList<T>>
		{
			private readonly CombineLatestObservable<T> parent;

			private readonly object gate = new object();

			private int length;

			private T[] values;

			private bool[] isStarted;

			private bool[] isCompleted;

			private bool isAllValueStarted;

			public CombineLatest(CombineLatestObservable<T> parent, IObserver<IList<T>> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.length = this.parent.sources.Length;
				this.values = new T[this.length];
				this.isStarted = new bool[this.length];
				this.isCompleted = new bool[this.length];
				this.isAllValueStarted = false;
				IDisposable[] array = new IDisposable[this.length];
				for (int i = 0; i < this.length; i++)
				{
					IObservable<T> observable = this.parent.sources[i];
					array[i] = observable.Subscribe(new CombineLatestObservable<T>.CombineLatest.CombineLatestObserver(this, i));
				}
				return StableCompositeDisposable.CreateUnsafe(array);
			}

			private void Publish(int index)
			{
				this.isStarted[index] = true;
				if (this.isAllValueStarted)
				{
					this.OnNext(new List<T>(this.values));
					return;
				}
				bool flag = true;
				for (int i = 0; i < this.length; i++)
				{
					if (!this.isStarted[i])
					{
						flag = false;
						break;
					}
				}
				this.isAllValueStarted = flag;
				if (this.isAllValueStarted)
				{
					this.OnNext(new List<T>(this.values));
					return;
				}
				bool flag2 = true;
				for (int j = 0; j < this.length; j++)
				{
					if (j != index)
					{
						if (!this.isCompleted[j])
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

			private class CombineLatestObserver : IObserver<T>
			{
				private readonly CombineLatestObservable<T>.CombineLatest parent;

				private readonly int index;

				public CombineLatestObserver(CombineLatestObservable<T>.CombineLatest parent, int index)
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
