using System;
using System.Collections.Generic;

namespace UniRx.InternalUtil
{
	internal sealed class AsyncLock : IDisposable
	{
		private readonly Queue<Action> queue = new Queue<Action>();

		private bool isAcquired;

		private bool hasFaulted;

		public void Wait(Action action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			bool flag = false;
			object obj = this.queue;
			lock (obj)
			{
				if (!this.hasFaulted)
				{
					this.queue.Enqueue(action);
					flag = !this.isAcquired;
					this.isAcquired = true;
				}
			}
			if (flag)
			{
				for (;;)
				{
					Action action2 = null;
					object obj2 = this.queue;
					lock (obj2)
					{
						if (this.queue.Count <= 0)
						{
							this.isAcquired = false;
							break;
						}
						action2 = this.queue.Dequeue();
					}
					try
					{
						action2();
					}
					catch
					{
						object obj3 = this.queue;
						lock (obj3)
						{
							this.queue.Clear();
							this.hasFaulted = true;
						}
						throw;
					}
				}
			}
		}

		public void Dispose()
		{
			object obj = this.queue;
			lock (obj)
			{
				this.queue.Clear();
				this.hasFaulted = true;
			}
		}
	}
}
