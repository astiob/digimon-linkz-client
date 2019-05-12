using System;
using System.Threading;

namespace System.Net
{
	internal class ListenerAsyncResult : IAsyncResult
	{
		private ManualResetEvent handle;

		private bool synch;

		private bool completed;

		private AsyncCallback cb;

		private object state;

		private Exception exception;

		private HttpListenerContext context;

		private object locker = new object();

		private ListenerAsyncResult forward;

		public ListenerAsyncResult(AsyncCallback cb, object state)
		{
			this.cb = cb;
			this.state = state;
		}

		internal void Complete(string error)
		{
			if (this.forward != null)
			{
				this.forward.Complete(error);
				return;
			}
			this.exception = new HttpListenerException(0, error);
			object obj = this.locker;
			lock (obj)
			{
				this.completed = true;
				if (this.handle != null)
				{
					this.handle.Set();
				}
				if (this.cb != null)
				{
					ThreadPool.QueueUserWorkItem(new WaitCallback(ListenerAsyncResult.InvokeCallback), this);
				}
			}
		}

		private static void InvokeCallback(object o)
		{
			ListenerAsyncResult listenerAsyncResult = (ListenerAsyncResult)o;
			if (listenerAsyncResult.forward != null)
			{
				listenerAsyncResult.forward.cb(listenerAsyncResult);
				return;
			}
			listenerAsyncResult.cb(listenerAsyncResult);
		}

		internal void Complete(HttpListenerContext context)
		{
			this.Complete(context, false);
		}

		internal void Complete(HttpListenerContext context, bool synch)
		{
			if (this.forward != null)
			{
				this.forward.Complete(context, synch);
				return;
			}
			this.synch = synch;
			this.context = context;
			object obj = this.locker;
			lock (obj)
			{
				AuthenticationSchemes authenticationSchemes = context.Listener.SelectAuthenticationScheme(context);
				if ((authenticationSchemes == AuthenticationSchemes.Basic || context.Listener.AuthenticationSchemes == AuthenticationSchemes.Negotiate) && context.Request.Headers["Authorization"] == null)
				{
					context.Response.StatusCode = 401;
					context.Response.Headers["WWW-Authenticate"] = string.Concat(new object[]
					{
						authenticationSchemes,
						" realm=\"",
						context.Listener.Realm,
						"\""
					});
					context.Response.OutputStream.Close();
					IAsyncResult asyncResult = context.Listener.BeginGetContext(this.cb, this.state);
					this.forward = (ListenerAsyncResult)asyncResult;
					object obj2 = this.forward.locker;
					lock (obj2)
					{
						if (this.handle != null)
						{
							this.forward.handle = this.handle;
						}
					}
					ListenerAsyncResult listenerAsyncResult = this.forward;
					int num = 0;
					while (listenerAsyncResult.forward != null)
					{
						if (num > 20)
						{
							this.Complete("Too many authentication errors");
						}
						listenerAsyncResult = listenerAsyncResult.forward;
						num++;
					}
				}
				else
				{
					this.completed = true;
					if (this.handle != null)
					{
						this.handle.Set();
					}
					if (this.cb != null)
					{
						ThreadPool.QueueUserWorkItem(new WaitCallback(ListenerAsyncResult.InvokeCallback), this);
					}
				}
			}
		}

		internal HttpListenerContext GetContext()
		{
			if (this.forward != null)
			{
				return this.forward.GetContext();
			}
			if (this.exception != null)
			{
				throw this.exception;
			}
			return this.context;
		}

		public object AsyncState
		{
			get
			{
				if (this.forward != null)
				{
					return this.forward.AsyncState;
				}
				return this.state;
			}
		}

		public WaitHandle AsyncWaitHandle
		{
			get
			{
				if (this.forward != null)
				{
					return this.forward.AsyncWaitHandle;
				}
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
				if (this.forward != null)
				{
					return this.forward.CompletedSynchronously;
				}
				return this.synch;
			}
		}

		public bool IsCompleted
		{
			get
			{
				if (this.forward != null)
				{
					return this.forward.IsCompleted;
				}
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
