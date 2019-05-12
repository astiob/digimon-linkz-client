using System;
using System.Threading;

namespace System.IO
{
	internal class FileStreamAsyncResult : IAsyncResult
	{
		private object state;

		private bool completed;

		private bool done;

		private Exception exc;

		private ManualResetEvent wh;

		private AsyncCallback cb;

		private bool completedSynch;

		public byte[] Buffer;

		public int Offset;

		public int Count;

		public int OriginalCount;

		public int BytesRead;

		private AsyncCallback realcb;

		public FileStreamAsyncResult(AsyncCallback cb, object state)
		{
			this.state = state;
			this.realcb = cb;
			if (this.realcb != null)
			{
				this.cb = new AsyncCallback(FileStreamAsyncResult.CBWrapper);
			}
			this.wh = new ManualResetEvent(false);
		}

		private static void CBWrapper(IAsyncResult ares)
		{
			FileStreamAsyncResult fileStreamAsyncResult = (FileStreamAsyncResult)ares;
			fileStreamAsyncResult.realcb.BeginInvoke(ares, null, null);
		}

		public void SetComplete(Exception e)
		{
			this.exc = e;
			this.completed = true;
			this.wh.Set();
			if (this.cb != null)
			{
				this.cb(this);
			}
		}

		public void SetComplete(Exception e, int nbytes)
		{
			this.BytesRead = nbytes;
			this.SetComplete(e);
		}

		public void SetComplete(Exception e, int nbytes, bool synch)
		{
			this.completedSynch = synch;
			this.SetComplete(e, nbytes);
		}

		public object AsyncState
		{
			get
			{
				return this.state;
			}
		}

		public bool CompletedSynchronously
		{
			get
			{
				return this.completedSynch;
			}
		}

		public WaitHandle AsyncWaitHandle
		{
			get
			{
				return this.wh;
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
