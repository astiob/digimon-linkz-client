using System;

namespace System.Net.Sockets
{
	/// <summary>Describes states for a <see cref="T:System.Net.Sockets.Socket" />.</summary>
	[Flags]
	public enum SocketInformationOptions
	{
		/// <summary>The <see cref="T:System.Net.Sockets.Socket" /> is nonblocking.</summary>
		NonBlocking = 1,
		/// <summary>The <see cref="T:System.Net.Sockets.Socket" /> is connected.</summary>
		Connected = 2,
		/// <summary>The <see cref="T:System.Net.Sockets.Socket" /> is listening for new connections.</summary>
		Listening = 4,
		/// <summary>The <see cref="T:System.Net.Sockets.Socket" /> uses overlapped I/O.</summary>
		UseOnlyOverlappedIO = 8
	}
}
