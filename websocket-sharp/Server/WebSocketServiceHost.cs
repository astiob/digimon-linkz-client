using System;
using WebSocketSharp.Net.WebSockets;

namespace WebSocketSharp.Server
{
	public abstract class WebSocketServiceHost
	{
		public abstract bool KeepClean { get; set; }

		public abstract string Path { get; }

		public abstract WebSocketSessionManager Sessions { get; }

		public abstract Type Type { get; }

		internal void StartSession(WebSocketContext context)
		{
			WebSocketService webSocketService = this.CreateSession();
			webSocketService.Start(context, this.Sessions);
		}

		protected abstract WebSocketService CreateSession();
	}
}
