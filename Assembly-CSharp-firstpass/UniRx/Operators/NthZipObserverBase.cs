using System;
using System.Collections;

namespace UniRx.Operators
{
	internal abstract class NthZipObserverBase<T> : OperatorObserverBase<T, T>, IZipObservable
	{
		private ICollection[] queues;

		private bool[] isDone;

		private int length;

		public NthZipObserverBase(IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
		{
		}

		protected void SetQueue(ICollection[] queues)
		{
			this.queues = queues;
			this.length = queues.Length;
			this.isDone = new bool[this.length];
		}

		public abstract T GetResult();

		public void Dequeue(int index)
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
				T value = default(T);
				try
				{
					value = this.GetResult();
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
			bool flag2 = true;
			for (int j = 0; j < this.length; j++)
			{
				if (j != index)
				{
					if (!this.isDone[j])
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

		public void Done(int index)
		{
			this.isDone[index] = true;
			bool flag = true;
			for (int i = 0; i < this.length; i++)
			{
				if (!this.isDone[i])
				{
					flag = false;
					break;
				}
			}
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
			}
		}

		public void Fail(Exception error)
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
}
