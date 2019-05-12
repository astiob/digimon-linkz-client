using System;
using System.Threading;

namespace System.IO
{
	internal class StreamAsyncResult : IAsyncResult
	{
		private object state;

		private bool completed;

		private bool done;

		private Exception exc;

		private int nbytes = -1;

		private ManualResetEvent wh;

		public StreamAsyncResult(object state)
		{
			this.state = state;
		}

		public void SetComplete(Exception e)
		{
			this.exc = e;
			this.completed = true;
			lock (this)
			{
				if (this.wh != null)
				{
					this.wh.Set();
				}
			}
		}

		public void SetComplete(Exception e, int nbytes)
		{
			this.nbytes = nbytes;
			this.SetComplete(e);
		}

		public object AsyncState
		{
			get
			{
				return this.state;
			}
		}

		public WaitHandle AsyncWaitHandle
		{
			get
			{
				WaitHandle result;
				lock (this)
				{
					if (this.wh == null)
					{
						this.wh = new ManualResetEvent(this.completed);
					}
					result = this.wh;
				}
				return result;
			}
		}

		public virtual bool CompletedSynchronously
		{
			get
			{
				return true;
			}
		}

		public bool IsCompleted
		{
			get
			{
				return this.completed;
			}
		}

		public Exception Exception
		{
			get
			{
				return this.exc;
			}
		}

		public int NBytes
		{
			get
			{
				return this.nbytes;
			}
		}

		public bool Done
		{
			get
			{
				return this.done;
			}
			set
			{
				this.done = value;
			}
		}
	}
}
