using System;
using System.Threading;

namespace System.Net
{
	internal class HttpStreamAsyncResult : IAsyncResult
	{
		private object locker = new object();

		private ManualResetEvent handle;

		private bool completed;

		internal byte[] Buffer;

		internal int Offset;

		internal int Count;

		internal AsyncCallback Callback;

		internal object State;

		internal int SynchRead;

		internal Exception Error;

		public void Complete(Exception e)
		{
			this.Error = e;
			this.Complete();
		}

		public void Complete()
		{
			object obj = this.locker;
			lock (obj)
			{
				if (!this.completed)
				{
					this.completed = true;
					if (this.handle != null)
					{
						this.handle.Set();
					}
					if (this.Callback != null)
					{
						this.Callback.BeginInvoke(this, null, null);
					}
				}
			}
		}

		public object AsyncState
		{
			get
			{
				return this.State;
			}
		}

		public WaitHandle AsyncWaitHandle
		{
			get
			{
				object obj = this.locker;
				lock (obj)
				{
					if (this.handle == null)
					{
						this.handle = new ManualResetEvent(this.completed);
					}
				}
				return this.handle;
			}
		}

		public bool CompletedSynchronously
		{
			get
			{
				return this.SynchRead == this.Count;
			}
		}

		public bool IsCompleted
		{
			get
			{
				object obj = this.locker;
				bool result;
				lock (obj)
				{
					result = this.completed;
				}
				return result;
			}
		}
	}
}
