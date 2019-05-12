using System;
using System.IO;
using System.Threading;

namespace System.Net
{
	internal class WebAsyncResult : IAsyncResult
	{
		private ManualResetEvent handle;

		private bool synch;

		private bool isCompleted;

		private AsyncCallback cb;

		private object state;

		private int nbytes;

		private IAsyncResult innerAsyncResult;

		private bool callbackDone;

		private Exception exc;

		private HttpWebResponse response;

		private Stream writeStream;

		private byte[] buffer;

		private int offset;

		private int size;

		private object locker = new object();

		public bool EndCalled;

		public bool AsyncWriteAll;

		public WebAsyncResult(AsyncCallback cb, object state)
		{
			this.cb = cb;
			this.state = state;
		}

		public WebAsyncResult(HttpWebRequest request, AsyncCallback cb, object state)
		{
			this.cb = cb;
			this.state = state;
		}

		public WebAsyncResult(AsyncCallback cb, object state, byte[] buffer, int offset, int size)
		{
			this.cb = cb;
			this.state = state;
			this.buffer = buffer;
			this.offset = offset;
			this.size = size;
		}

		internal void SetCompleted(bool synch, Exception e)
		{
			this.synch = synch;
			this.exc = e;
			object obj = this.locker;
			lock (obj)
			{
				this.isCompleted = true;
				if (this.handle != null)
				{
					this.handle.Set();
				}
			}
		}

		internal void Reset()
		{
			this.callbackDone = false;
			this.exc = null;
			this.response = null;
			this.writeStream = null;
			this.exc = null;
			object obj = this.locker;
			lock (obj)
			{
				this.isCompleted = false;
				if (this.handle != null)
				{
					this.handle.Reset();
				}
			}
		}

		internal void SetCompleted(bool synch, int nbytes)
		{
			this.synch = synch;
			this.nbytes = nbytes;
			this.exc = null;
			object obj = this.locker;
			lock (obj)
			{
				this.isCompleted = true;
				if (this.handle != null)
				{
					this.handle.Set();
				}
			}
		}

		internal void SetCompleted(bool synch, Stream writeStream)
		{
			this.synch = synch;
			this.writeStream = writeStream;
			this.exc = null;
			object obj = this.locker;
			lock (obj)
			{
				this.isCompleted = true;
				if (this.handle != null)
				{
					this.handle.Set();
				}
			}
		}

		internal void SetCompleted(bool synch, HttpWebResponse response)
		{
			this.synch = synch;
			this.response = response;
			this.exc = null;
			object obj = this.locker;
			lock (obj)
			{
				this.isCompleted = true;
				if (this.handle != null)
				{
					this.handle.Set();
				}
			}
		}

		internal void DoCallback()
		{
			if (!this.callbackDone && this.cb != null)
			{
				this.callbackDone = true;
				this.cb(this);
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
					if (this.handle == null)
					{
						this.handle = new ManualResetEvent(this.isCompleted);
					}
				}
				return this.handle;
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
					result = this.isCompleted;
				}
				return result;
			}
		}

		internal bool GotException
		{
			get
			{
				return this.exc != null;
			}
		}

		internal Exception Exception
		{
			get
			{
				return this.exc;
			}
		}

		internal int NBytes
		{
			get
			{
				return this.nbytes;
			}
			set
			{
				this.nbytes = value;
			}
		}

		internal IAsyncResult InnerAsyncResult
		{
			get
			{
				return this.innerAsyncResult;
			}
			set
			{
				this.innerAsyncResult = value;
			}
		}

		internal Stream WriteStream
		{
			get
			{
				return this.writeStream;
			}
		}

		internal HttpWebResponse Response
		{
			get
			{
				return this.response;
			}
		}

		internal byte[] Buffer
		{
			get
			{
				return this.buffer;
			}
		}

		internal int Offset
		{
			get
			{
				return this.offset;
			}
		}

		internal int Size
		{
			get
			{
				return this.size;
			}
		}
	}
}
