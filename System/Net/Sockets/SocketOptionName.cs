using System;

namespace System.Net.Sockets
{
	/// <summary>Defines configuration option names.</summary>
	public enum SocketOptionName
	{
		/// <summary>Record debugging information.</summary>
		Debug = 1,
		/// <summary>The socket is listening.</summary>
		AcceptConnection,
		/// <summary>Allows the socket to be bound to an address that is already in use.</summary>
		ReuseAddress = 4,
		/// <summary>Use keep-alives.</summary>
		KeepAlive = 8,
		/// <summary>Do not route; send the packet directly to the interface addresses.</summary>
		DontRoute = 16,
		/// <summary>Permit sending broadcast messages on the socket.</summary>
		Broadcast = 32,
		/// <summary>Bypass hardware when possible.</summary>
		UseLoopback = 64,
		/// <summary>Linger on close if unsent data is present.</summary>
		Linger = 128,
		/// <summary>Receives out-of-band data in the normal data stream.</summary>
		OutOfBandInline = 256,
		/// <summary>Close the socket gracefully without lingering.</summary>
		DontLinger = -129,
		/// <summary>Enables a socket to be bound for exclusive access.</summary>
		ExclusiveAddressUse = -5,
		/// <summary>Specifies the total per-socket buffer space reserved for sends. This is unrelated to the maximum message size or the size of a TCP window.</summary>
		SendBuffer = 4097,
		/// <summary>Specifies the total per-socket buffer space reserved for receives. This is unrelated to the maximum message size or the size of a TCP window.</summary>
		ReceiveBuffer,
		/// <summary>Specifies the low water mark for <see cref="Overload:System.Net.Sockets.Socket.Send" /> operations.</summary>
		SendLowWater,
		/// <summary>Specifies the low water mark for <see cref="Overload:System.Net.Sockets.Socket.Receive" /> operations.</summary>
		ReceiveLowWater,
		/// <summary>Send a time-out. This option applies only to synchronous methods; it has no effect on asynchronous methods such as the <see cref="M:System.Net.Sockets.Socket.BeginSend(System.Byte[],System.Int32,System.Int32,System.Net.Sockets.SocketFlags,System.AsyncCallback,System.Object)" /> method.</summary>
		SendTimeout,
		/// <summary>Receive a time-out. This option applies only to synchronous methods; it has no effect on asynchronous methods such as the <see cref="M:System.Net.Sockets.Socket.BeginSend(System.Byte[],System.Int32,System.Int32,System.Net.Sockets.SocketFlags,System.AsyncCallback,System.Object)" /> method.</summary>
		ReceiveTimeout,
		/// <summary>Get the error status and clear.</summary>
		Error,
		/// <summary>Get the socket type.</summary>
		Type,
		/// <summary>Not supported; will throw a <see cref="T:System.Net.Sockets.SocketException" /> if used.</summary>
		MaxConnections = 2147483647,
		/// <summary>Specifies the IP options to be inserted into outgoing datagrams.</summary>
		IPOptions = 1,
		/// <summary>Indicates that the application provides the IP header for outgoing datagrams.</summary>
		HeaderIncluded,
		/// <summary>Change the IP header type of the service field.</summary>
		TypeOfService,
		/// <summary>Set the IP header Time-to-Live field.</summary>
		IpTimeToLive,
		/// <summary>Set the interface for outgoing multicast packets.</summary>
		MulticastInterface = 9,
		/// <summary>An IP multicast Time to Live.</summary>
		MulticastTimeToLive,
		/// <summary>An IP multicast loopback.</summary>
		MulticastLoopback,
		/// <summary>Add an IP group membership.</summary>
		AddMembership,
		/// <summary>Drop an IP group membership.</summary>
		DropMembership,
		/// <summary>Do not fragment IP datagrams.</summary>
		DontFragment,
		/// <summary>Join a source group.</summary>
		AddSourceMembership,
		/// <summary>Drop a source group.</summary>
		DropSourceMembership,
		/// <summary>Block data from a source.</summary>
		BlockSource,
		/// <summary>Unblock a previously blocked source.</summary>
		UnblockSource,
		/// <summary>Return information about received packets.</summary>
		PacketInformation,
		/// <summary>Disables the Nagle algorithm for send coalescing.</summary>
		NoDelay = 1,
		/// <summary>Use urgent data as defined in RFC-1222. This option can be set only once; after it is set, it cannot be turned off.</summary>
		BsdUrgent,
		/// <summary>Use expedited data as defined in RFC-1222. This option can be set only once; after it is set, it cannot be turned off.</summary>
		Expedited = 2,
		/// <summary>Send UDP datagrams with checksum set to zero.</summary>
		NoChecksum = 1,
		/// <summary>Set or get the UDP checksum coverage.</summary>
		ChecksumCoverage = 20,
		/// <summary>Specifies the maximum number of router hops for an Internet Protocol version 6 (IPv6) packet. This is similar to Time to Live (TTL) for Internet Protocol version 4.</summary>
		HopLimit,
		/// <summary>Updates an accepted socket's properties by using those of an existing socket. This is equivalent to using the Winsock2 SO_UPDATE_ACCEPT_CONTEXT socket option and is supported only on connection-oriented sockets.</summary>
		UpdateAcceptContext = 28683,
		/// <summary>Updates a connected socket's properties by using those of an existing socket. This is equivalent to using the Winsock2 SO_UPDATE_CONNECT_CONTEXT socket option and is supported only on connection-oriented sockets.</summary>
		UpdateConnectContext = 28688
	}
}
