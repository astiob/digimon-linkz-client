using System;

namespace System.Net.Sockets
{
	/// <summary>Defines socket option levels for the <see cref="M:System.Net.Sockets.Socket.SetSocketOption(System.Net.Sockets.SocketOptionLevel,System.Net.Sockets.SocketOptionName,System.Int32)" /> and <see cref="M:System.Net.Sockets.Socket.GetSocketOption(System.Net.Sockets.SocketOptionLevel,System.Net.Sockets.SocketOptionName)" /> methods.</summary>
	public enum SocketOptionLevel
	{
		/// <summary>
		///   <see cref="T:System.Net.Sockets.Socket" /> options apply to all sockets.</summary>
		Socket = 65535,
		/// <summary>
		///   <see cref="T:System.Net.Sockets.Socket" /> options apply only to IP sockets.</summary>
		IP = 0,
		/// <summary>
		///   <see cref="T:System.Net.Sockets.Socket" /> options apply only to IPv6 sockets.</summary>
		IPv6 = 41,
		/// <summary>
		///   <see cref="T:System.Net.Sockets.Socket" /> options apply only to TCP sockets.</summary>
		Tcp = 6,
		/// <summary>
		///   <see cref="T:System.Net.Sockets.Socket" /> options apply only to UDP sockets.</summary>
		Udp = 17
	}
}
