using System;

namespace UniRx.Operators
{
	internal abstract class NthZipLatestObserverBase<T> : OperatorObserverBase<T, T>, IZipLatestObservable
	{
		private readonly int length;

		private readonly bool[] isStarted;

		private readonly bool[] isCompleted;

		public NthZipLatestObserverBase(int length, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
		{
			this.length = length;
			this.isStarted = new bool[length];
			this.isCompleted = new bool[length];
		}

		public abstract T GetResult();

		public void Publish(int index)
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
			if (flag2)
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
				return;
			}
			else
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
			}
		}

		public void Done(int index)
		{
			this.isCompleted[index] = true;
			bool flag = true;
			for (int i = 0; i < this.length; i++)
			{
				if (!this.isCompleted[i])
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
