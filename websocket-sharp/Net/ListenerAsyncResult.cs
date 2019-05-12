using System;
using System.Threading;

namespace WebSocketSharp.Net
{
	internal class ListenerAsyncResult : IAsyncResult
	{
		private AsyncCallback _callback;

		private bool _completed;

		private HttpListenerContext _context;

		private Exception _exception;

		private ManualResetEvent _waitHandle;

		private object _state;

		private object _sync;

		private bool _syncCompleted;

		internal bool EndCalled;

		internal bool InGet;

		public ListenerAsyncResult(AsyncCallback callback, object state)
		{
			this._callback = callback;
			this._state = state;
			this._sync = new object();
		}

		public object AsyncState
		{
			get
			{
				return this._state;
			}
		}

		public WaitHandle AsyncWaitHandle
		{
			get
			{
				object sync = this._sync;
				WaitHandle result;
				lock (sync)
				{
					ManualResetEvent manualResetEvent;
					if ((manualResetEvent = this._waitHandle) == null)
					{
						manualResetEvent = (this._waitHandle = new ManualResetEvent(this._completed));
					}
					result = manualResetEvent;
				}
				return result;
			}
		}

		public bool CompletedSynchronously
		{
			get
			{
				return this._syncCompleted;
			}
		}

		public bool IsCompleted
		{
			get
			{
				object sync = this._sync;
				bool completed;
				lock (sync)
				{
					completed = this._completed;
				}
				return completed;
			}
		}

		private static void invokeCallback(object state)
		{
			try
			{
				ListenerAsyncResult listenerAsyncResult = (ListenerAsyncResult)state;
				listenerAsyncResult._callback(listenerAsyncResult);
			}
			catch
			{
			}
		}

		internal void Complete(Exception exception)
		{
			this._exception = ((!this.InGet || !(exception is ObjectDisposedException)) ? exception : new HttpListenerException(500, "Listener closed"));
			object sync = this._sync;
			lock (sync)
			{
				this._completed = true;
				if (this._waitHandle != null)
				{
					this._waitHandle.Set();
				}
				if (this._callback != null)
				{
					ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(ListenerAsyncResult.invokeCallback), this);
				}
			}
		}

		internal void Complete(HttpListenerContext context)
		{
			this.Complete(context, false);
		}

		internal void Complete(HttpListenerContext context, bool syncCompleted)
		{
			HttpListener listener = context.Listener;
			AuthenticationSchemes authenticationSchemes = listener.SelectAuthenticationScheme(context);
			if (authenticationSchemes == AuthenticationSchemes.None)
			{
				context.Response.Close(HttpStatusCode.Forbidden);
				listener.BeginGetContext(this);
				return;
			}
			string text = context.Request.Headers["Authorization"];
			if (authenticationSchemes == AuthenticationSchemes.Basic && (text == null || !text.StartsWith("basic", StringComparison.OrdinalIgnoreCase)))
			{
				context.Response.CloseWithAuthChallenge(HttpUtility.CreateBasicAuthChallenge(listener.Realm));
				listener.BeginGetContext(this);
				return;
			}
			if (authenticationSchemes == AuthenticationSchemes.Digest && (text == null || !text.StartsWith("digest", StringComparison.OrdinalIgnoreCase)))
			{
				context.Response.CloseWithAuthChallenge(HttpUtility.CreateDigestAuthChallenge(listener.Realm));
				listener.BeginGetContext(this);
				return;
			}
			this._context = context;
			this._syncCompleted = syncCompleted;
			object sync = this._sync;
			lock (sync)
			{
				this._completed = true;
				if (this._waitHandle != null)
				{
					this._waitHandle.Set();
				}
				if (this._callback != null)
				{
					ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(ListenerAsyncResult.invokeCallback), this);
				}
			}
		}

		internal HttpListenerContext GetContext()
		{
			if (this._exception != null)
			{
				throw this._exception;
			}
			return this._context;
		}
	}
}
