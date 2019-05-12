using System;
using System.IO;
using System.Threading;

namespace System.Net
{
	internal class FtpAsyncResult : IAsyncResult
	{
		private FtpWebResponse response;

		private ManualResetEvent waitHandle;

		private Exception exception;

		private AsyncCallback callback;

		private Stream stream;

		private object state;

		private bool completed;

		private bool synch;

		private object locker = new object();

		public FtpAsyncResult(AsyncCallback callback, object state)
		{
			this.callback = callback;
			this.state = state;
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
				object obj = this.locker;
				lock (obj)
				{
					if (this.waitHandle == null)
					{
						this.waitHandle = new ManualResetEvent(false);
					}
				}
				return this.waitHandle;
			}
		}

		public bool CompletedSynchronously
		{
			get
			{
				return this.synch;
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

		internal bool GotException
		{
			get
			{
				return this.exception != null;
			}
		}

		internal Exception Exception
		{
			get
			{
				return this.exception;
			}
		}

		internal FtpWebResponse Response
		{
			get
			{
				return this.response;
			}
			set
			{
				this.response = value;
			}
		}

		internal Stream Stream
		{
			get
			{
				return this.stream;
			}
			set
			{
				this.stream = value;
			}
		}

		internal void WaitUntilComplete()
		{
			if (this.IsCompleted)
			{
				return;
			}
			this.AsyncWaitHandle.WaitOne();
		}

		internal bool WaitUntilComplete(int timeout, bool exitContext)
		{
			return this.IsCompleted || this.AsyncWaitHandle.WaitOne(timeout, exitContext);
		}

		internal void SetCompleted(bool synch, Exception exc, FtpWebResponse response)
		{
			this.synch = synch;
			this.exception = exc;
			this.response = response;
			object obj = this.locker;
			lock (obj)
			{
				this.completed = true;
				if (this.waitHandle != null)
				{
					this.waitHandle.Set();
				}
			}
			this.DoCallback();
		}

		internal void SetCompleted(bool synch, FtpWebResponse response)
		{
			this.SetCompleted(synch, null, response);
		}

		internal void SetCompleted(bool synch, Exception exc)
		{
			this.SetCompleted(synch, exc, null);
		}

		internal void DoCallback()
		{
			if (this.callback != null)
			{
				try
				{
					this.callback(this);
				}
				catch (Exception)
				{
				}
			}
		}

		internal void Reset()
		{
			this.exception = null;
			this.synch = false;
			this.response = null;
			this.state = null;
			object obj = this.locker;
			lock (obj)
			{
				this.completed = false;
				if (this.waitHandle != null)
				{
					this.waitHandle.Reset();
				}
			}
		}
	}
}
