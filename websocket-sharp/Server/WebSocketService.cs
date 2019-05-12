using System;
using System.IO;
using WebSocketSharp.Net;
using WebSocketSharp.Net.WebSockets;

namespace WebSocketSharp.Server
{
	public abstract class WebSocketService : IWebSocketSession
	{
		private WebSocketContext _context;

		private Func<CookieCollection, CookieCollection, bool> _cookiesValidator;

		private Func<string, bool> _originValidator;

		private string _protocol;

		private WebSocketSessionManager _sessions;

		private DateTime _start;

		private WebSocket _websocket;

		protected WebSocketService()
		{
			this._start = DateTime.MaxValue;
		}

		protected Logger Log
		{
			get
			{
				return (this._websocket == null) ? null : this._websocket.Log;
			}
		}

		protected WebSocketSessionManager Sessions
		{
			get
			{
				return this._sessions;
			}
		}

		public WebSocketContext Context
		{
			get
			{
				return this._context;
			}
		}

		public Func<CookieCollection, CookieCollection, bool> CookiesValidator
		{
			get
			{
				return this._cookiesValidator;
			}
			set
			{
				this._cookiesValidator = value;
			}
		}

		public string ID { get; private set; }

		public Func<string, bool> OriginValidator
		{
			get
			{
				return this._originValidator;
			}
			set
			{
				this._originValidator = value;
			}
		}

		public string Protocol
		{
			get
			{
				return (this._websocket == null) ? (this._protocol ?? string.Empty) : this._websocket.Protocol;
			}
			set
			{
				if (this.State == WebSocketState.Connecting && value != null && value.Length > 0 && value.IsToken())
				{
					this._protocol = value;
				}
			}
		}

		public DateTime StartTime
		{
			get
			{
				return this._start;
			}
		}

		public WebSocketState State
		{
			get
			{
				return (this._websocket == null) ? WebSocketState.Connecting : this._websocket.ReadyState;
			}
		}

		private string checkIfValidConnectionRequest(WebSocketContext context)
		{
			return (this._originValidator == null || this._originValidator(context.Origin)) ? ((this._cookiesValidator == null || this._cookiesValidator(context.CookieCollection, context.WebSocket.CookieCollection)) ? null : "Invalid Cookies.") : "Invalid Origin header.";
		}

		private void onClose(object sender, CloseEventArgs e)
		{
			if (this.ID == null)
			{
				return;
			}
			this._sessions.Remove(this.ID);
			this.OnClose(e);
		}

		private void onError(object sender, ErrorEventArgs e)
		{
			this.OnError(e);
		}

		private void onMessage(object sender, MessageEventArgs e)
		{
			this.OnMessage(e);
		}

		private void onOpen(object sender, EventArgs e)
		{
			this.ID = this._sessions.Add(this);
			if (this.ID == null)
			{
				this._websocket.Close(CloseStatusCode.Away);
				return;
			}
			this._start = DateTime.Now;
			this.OnOpen();
		}

		internal void Start(WebSocketContext context, WebSocketSessionManager sessions)
		{
			this._context = context;
			this._sessions = sessions;
			this._websocket = context.WebSocket;
			this._websocket.Protocol = this._protocol;
			this._websocket.CustomHandshakeRequestChecker = new Func<WebSocketContext, string>(this.checkIfValidConnectionRequest);
			this._websocket.OnOpen += this.onOpen;
			this._websocket.OnMessage += this.onMessage;
			this._websocket.OnError += this.onError;
			this._websocket.OnClose += this.onClose;
			this._websocket.ConnectAsServer();
		}

		protected void Error(string message)
		{
			if (message != null && message.Length > 0)
			{
				this.OnError(new ErrorEventArgs(message));
			}
		}

		protected virtual void OnClose(CloseEventArgs e)
		{
		}

		protected virtual void OnError(ErrorEventArgs e)
		{
		}

		protected virtual void OnMessage(MessageEventArgs e)
		{
		}

		protected virtual void OnOpen()
		{
		}

		protected void Send(byte[] data)
		{
			if (this._websocket != null)
			{
				this._websocket.Send(data);
			}
		}

		protected void Send(FileInfo file)
		{
			if (this._websocket != null)
			{
				this._websocket.Send(file);
			}
		}

		protected void Send(string data)
		{
			if (this._websocket != null)
			{
				this._websocket.Send(data);
			}
		}

		protected void SendAsync(byte[] data, Action<bool> completed)
		{
			if (this._websocket != null)
			{
				this._websocket.SendAsync(data, completed);
			}
		}

		protected void SendAsync(FileInfo file, Action<bool> completed)
		{
			if (this._websocket != null)
			{
				this._websocket.SendAsync(file, completed);
			}
		}

		protected void SendAsync(string data, Action<bool> completed)
		{
			if (this._websocket != null)
			{
				this._websocket.SendAsync(data, completed);
			}
		}

		protected void SendAsync(Stream stream, int length, Action<bool> completed)
		{
			if (this._websocket != null)
			{
				this._websocket.SendAsync(stream, length, completed);
			}
		}
	}
}
