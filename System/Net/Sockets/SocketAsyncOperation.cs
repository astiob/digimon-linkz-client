using System;

namespace System.Net.Sockets
{
	/// <summary>The type of asynchronous socket operation most recently performed with this context object.</summary>
	public enum SocketAsyncOperation
	{
		/// <summary>None of the socket operations.</summary>
		None,
		/// <summary>A socket Accept operation. </summary>
		Accept,
		/// <summary>A socket Connect operation.</summary>
		Connect,
		/// <summary>A socket Disconnect operation.</summary>
		Disconnect,
		/// <summary>A socket Receive operation.</summary>
		Receive,
		/// <summary>A socket ReceiveFrom operation.</summary>
		ReceiveFrom,
		/// <summary>A socket ReceiveMessageFrom operation.</summary>
		ReceiveMessageFrom,
		/// <summary>A socket Send operation.</summary>
		Send,
		/// <summary>A socket SendPackets operation.</summary>
		SendPackets,
		/// <summary>A socket SendTo operation.</summary>
		SendTo
	}
}
