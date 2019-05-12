using System;

namespace UniRx.Operators
{
	internal abstract class NthCombineLatestObserverBase<T> : OperatorObserverBase<T, T>, ICombineLatestObservable
	{
		private readonly int length;

		private bool isAllValueStarted;

		private readonly bool[] isStarted;

		private readonly bool[] isCompleted;

		public NthCombineLatestObserverBase(int length, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
		{
			this.length = length;
			this.isAllValueStarted = false;
			this.isStarted = new bool[length];
			this.isCompleted = new bool[length];
		}

		public abstract T GetResult();

		public void Publish(int index)
		{
			this.isStarted[index] = true;
			if (this.isAllValueStarted)
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
				T value2 = default(T);
				try
				{
					value2 = this.GetResult();
				}
				catch (Exception error2)
				{
					try
					{
						this.observer.OnError(error2);
					}
					finally
					{
						base.Dispose();
					}
					return;
				}
				this.OnNext(value2);
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
