using System;

namespace System.Net.Sockets
{
	/// <summary>Specifies the type of socket that an instance of the <see cref="T:System.Net.Sockets.Socket" /> class represents.</summary>
	public enum SocketType
	{
		/// <summary>Supports reliable, two-way, connection-based byte streams without the duplication of data and without preservation of boundaries. A Socket of this type communicates with a single peer and requires a remote host connection before communication can begin. <see cref="F:System.Net.Sockets.SocketType.Stream" /> uses the Transmission Control Protocol (<see cref="F:System.Net.Sockets.ProtocolType.Tcp" />) <see cref="T:System.Net.Sockets.ProtocolType" /> and the InterNetwork<see cref="T:System.Net.Sockets.AddressFamily" />.</summary>
		Stream = 1,
		/// <summary>Supports datagrams, which are connectionless, unreliable messages of a fixed (typically small) maximum length. Messages might be lost or duplicated and might arrive out of order. A <see cref="T:System.Net.Sockets.Socket" /> of type <see cref="F:System.Net.Sockets.SocketType.Dgram" /> requires no connection prior to sending and receiving data, and can communicate with multiple peers. <see cref="F:System.Net.Sockets.SocketType.Dgram" /> uses the Datagram Protocol (<see cref="F:System.Net.Sockets.ProtocolType.Udp" />) and the <see cref="F:System.Net.Sockets.AddressFamily.InterNetwork" /><see cref="T:System.Net.Sockets.AddressFamily" />.</summary>
		Dgram,
		/// <summary>Supports access to the underlying transport protocol. Using the <see cref="T:System.Net.Sockets.SocketType" /><see cref="F:System.Net.Sockets.SocketType.Raw" />, you can communicate using protocols like Internet Control Message Protocol (<see cref="F:System.Net.Sockets.ProtocolType.Icmp" />) and Internet Group Management Protocol (<see cref="F:System.Net.Sockets.ProtocolType.Igmp" />). Your application must provide a complete IP header when sending. Received datagrams return with the IP header and options intact.</summary>
		Raw,
		/// <summary>Supports connectionless, message-oriented, reliably delivered messages, and preserves message boundaries in data. Rdm (Reliably Delivered Messages) messages arrive unduplicated and in order. Furthermore, the sender is notified if messages are lost. If you initialize a Socket using <see cref="F:System.Net.Sockets.SocketType.Rdm" />, you do not require a remote host connection before sending and receiving data. With <see cref="F:System.Net.Sockets.SocketType.Rdm" />, you can communicate with multiple peers.</summary>
		Rdm,
		/// <summary>Provides connection-oriented and reliable two-way transfer of ordered byte streams across a network. <see cref="F:System.Net.Sockets.SocketType.Seqpacket" /> does not duplicate data, and it preserves boundaries within the data stream. A Socket of type <see cref="F:System.Net.Sockets.SocketType.Seqpacket" /> communicates with a single peer and requires a remote host connection before communication can begin.</summary>
		Seqpacket,
		/// <summary>Specifies an unknown Socket type.</summary>
		Unknown = -1
	}
}
