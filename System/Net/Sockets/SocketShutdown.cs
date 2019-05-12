using System;

namespace System.Net.Sockets
{
	/// <summary>Defines constants that are used by the <see cref="M:System.Net.Sockets.Socket.Shutdown(System.Net.Sockets.SocketShutdown)" /> method.</summary>
	public enum SocketShutdown
	{
		/// <summary>Disables a <see cref="T:System.Net.Sockets.Socket" /> for receiving. This field is constant.</summary>
		Receive,
		/// <summary>Disables a <see cref="T:System.Net.Sockets.Socket" /> for sending. This field is constant.</summary>
		Send,
		/// <summary>Disables a <see cref="T:System.Net.Sockets.Socket" /> for both sending and receiving. This field is constant.</summary>
		Both
	}
}
