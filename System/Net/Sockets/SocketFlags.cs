using System;

namespace System.Net.Sockets
{
	/// <summary>Specifies socket send and receive behaviors.</summary>
	[Flags]
	public enum SocketFlags
	{
		/// <summary>Use no flags for this call.</summary>
		None = 0,
		/// <summary>Process out-of-band data.</summary>
		OutOfBand = 1,
		/// <summary>Peek at the incoming message.</summary>
		Peek = 2,
		/// <summary>Send without using routing tables.</summary>
		DontRoute = 4,
		/// <summary>Provides a standard value for the number of WSABUF structures that are used to send and receive data.</summary>
		MaxIOVectorLength = 16,
		/// <summary>The message was too large to fit into the specified buffer and was truncated.</summary>
		Truncated = 256,
		/// <summary>Indicates that the control data did not fit into an internal 64-KB buffer and was truncated.</summary>
		ControlDataTruncated = 512,
		/// <summary>Indicates a broadcast packet.</summary>
		Broadcast = 1024,
		/// <summary>Indicates a multicast packet.</summary>
		Multicast = 2048,
		/// <summary>Partial send or receive for message.</summary>
		Partial = 32768
	}
}
