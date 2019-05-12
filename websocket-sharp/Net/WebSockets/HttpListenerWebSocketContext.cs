using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Security.Principal;

namespace WebSocketSharp.Net.WebSockets
{
	public class HttpListenerWebSocketContext : WebSocketContext
	{
		private HttpListenerContext _context;

		private WsStream _stream;

		private WebSocket _websocket;

		internal HttpListenerWebSocketContext(HttpListenerContext context, Logger logger)
		{
			this._context = context;
			this._stream = WsStream.CreateServerStream(context);
			this._websocket = new WebSocket(this, logger ?? new Logger());
		}

		internal WsStream Stream
		{
			get
			{
				return this._stream;
			}
		}

		public override CookieCollection CookieCollection
		{
			get
			{
				return this._context.Request.Cookies;
			}
		}

		public override NameValueCollection Headers
		{
			get
			{
				return this._context.Request.Headers;
			}
		}

		public override string Host
		{
			get
			{
				return this._context.Request.Headers["Host"];
			}
		}

		public override bool IsAuthenticated
		{
			get
			{
				return this._context.Request.IsAuthenticated;
			}
		}

		public override bool IsLocal
		{
			get
			{
				return this._context.Request.IsLocal;
			}
		}

		public override bool IsSecureConnection
		{
			get
			{
				return this._context.Request.IsSecureConnection;
			}
		}

		public override bool IsWebSocketRequest
		{
			get
			{
				return this._context.Request.IsWebSocketRequest;
			}
		}

		public override string Origin
		{
			get
			{
				return this._context.Request.Headers["Origin"];
			}
		}

		public override string Path
		{
			get
			{
				return this._context.Request.Url.GetAbsolutePath();
			}
		}

		public override NameValueCollection QueryString
		{
			get
			{
				return this._context.Request.QueryString;
			}
		}

		public override Uri RequestUri
		{
			get
			{
				return this._context.Request.Url;
			}
		}

		public override string SecWebSocketKey
		{
			get
			{
				return this._context.Request.Headers["Sec-WebSocket-Key"];
			}
		}

		public override IEnumerable<string> SecWebSocketProtocols
		{
			get
			{
				string protocols = this._context.Request.Headers["Sec-WebSocket-Protocol"];
				if (protocols != null)
				{
					foreach (string protocol in protocols.Split(new char[]
					{
						','
					}))
					{
						yield return protocol.Trim();
					}
				}
				yield break;
			}
		}

		public override string SecWebSocketVersion
		{
			get
			{
				return this._context.Request.Headers["Sec-WebSocket-Version"];
			}
		}

		public override IPEndPoint ServerEndPoint
		{
			get
			{
				return this._context.Connection.LocalEndPoint;
			}
		}

		public override IPrincipal User
		{
			get
			{
				return this._context.User;
			}
		}

		public override IPEndPoint UserEndPoint
		{
			get
			{
				return this._context.Connection.RemoteEndPoint;
			}
		}

		public override WebSocket WebSocket
		{
			get
			{
				return this._websocket;
			}
		}

		internal void Close()
		{
			this._context.Connection.Close(true);
		}

		internal void Close(HttpStatusCode code)
		{
			this._context.Response.Close(code);
		}

		public override string ToString()
		{
			return this._context.Request.ToString();
		}
	}
}
