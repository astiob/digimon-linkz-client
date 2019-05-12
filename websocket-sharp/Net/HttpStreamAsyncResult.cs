using System;
using System.Threading;

namespace WebSocketSharp.Net
{
	internal class HttpStreamAsyncResult : IAsyncResult
	{
		private bool completed;

		private ManualResetEvent handle;

		private object locker = new object();

		internal AsyncCallback Callback;

		internal int Count;

		internal byte[] Buffer;

		internal Exception Error;

		internal int Offset;

		internal object State;

		internal int SyncRead;

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
				return this.SyncRead == this.Count;
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

		public void Complete(Exception e)
		{
			this.Error = e;
			this.Complete();
		}
	}
}
