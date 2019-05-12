using System;
using WebSocketSharp.Net;

namespace WebSocketSharp.Server
{
	public class HttpRequestEventArgs : EventArgs
	{
		internal HttpRequestEventArgs(HttpListenerContext context)
		{
			this.Request = context.Request;
			this.Response = context.Response;
		}

		public HttpListenerRequest Request { get; private set; }

		public HttpListenerResponse Response { get; private set; }
	}
}
