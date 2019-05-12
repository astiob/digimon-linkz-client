using System;

namespace System.Threading
{
	internal class LockQueue
	{
		private ReaderWriterLock rwlock;

		private int lockCount;

		public LockQueue(ReaderWriterLock rwlock)
		{
			this.rwlock = rwlock;
		}

		public bool Wait(int timeout)
		{
			bool flag = false;
			bool result;
			try
			{
				lock (this)
				{
					this.lockCount++;
					Monitor.Exit(this.rwlock);
					flag = true;
					result = Monitor.Wait(this, timeout);
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Enter(this.rwlock);
					this.lockCount--;
				}
			}
			return result;
		}

		public bool IsEmpty
		{
			get
			{
				bool result;
				lock (this)
				{
					result = (this.lockCount == 0);
				}
				return result;
			}
		}

		public void Pulse()
		{
			lock (this)
			{
				Monitor.Pulse(this);
			}
		}

		public void PulseAll()
		{
			lock (this)
			{
				Monitor.PulseAll(this);
			}
		}
	}
}
