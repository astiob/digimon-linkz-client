using System;

namespace System.Net.Sockets
{
	/// <summary>Defines error codes for the <see cref="T:System.Net.Sockets.Socket" /> class.</summary>
	public enum SocketError
	{
		/// <summary>An attempt was made to access a <see cref="T:System.Net.Sockets.Socket" /> in a way that is forbidden by its access permissions.</summary>
		AccessDenied = 10013,
		/// <summary>Only one use of an address is normally permitted.</summary>
		AddressAlreadyInUse = 10048,
		/// <summary>The address family specified is not supported. This error is returned if the IPv6 address family was specified and the IPv6 stack is not installed on the local machine. This error is returned if the IPv4 address family was specified and the IPv4 stack is not installed on the local machine.</summary>
		AddressFamilyNotSupported = 10047,
		/// <summary>The selected IP address is not valid in this context.</summary>
		AddressNotAvailable = 10049,
		/// <summary>The nonblocking <see cref="T:System.Net.Sockets.Socket" /> already has an operation in progress.</summary>
		AlreadyInProgress = 10037,
		/// <summary>The connection was aborted by the .NET Framework or the underlying socket provider.</summary>
		ConnectionAborted = 10053,
		/// <summary>The remote host is actively refusing a connection.</summary>
		ConnectionRefused = 10061,
		/// <summary>The connection was reset by the remote peer.</summary>
		ConnectionReset = 10054,
		/// <summary>A required address was omitted from an operation on a <see cref="T:System.Net.Sockets.Socket" />.</summary>
		DestinationAddressRequired = 10039,
		/// <summary>A graceful shutdown is in progress.</summary>
		Disconnecting = 10101,
		/// <summary>An invalid pointer address was detected by the underlying socket provider.</summary>
		Fault = 10014,
		/// <summary>The operation failed because the remote host is down.</summary>
		HostDown = 10064,
		/// <summary>No such host is known. The name is not an official host name or alias.</summary>
		HostNotFound = 11001,
		/// <summary>There is no network route to the specified host.</summary>
		HostUnreachable = 10065,
		/// <summary>A blocking operation is in progress.</summary>
		InProgress = 10036,
		/// <summary>A blocking <see cref="T:System.Net.Sockets.Socket" /> call was canceled.</summary>
		Interrupted = 10004,
		/// <summary>An invalid argument was supplied to a <see cref="T:System.Net.Sockets.Socket" /> member.</summary>
		InvalidArgument = 10022,
		/// <summary>The application has initiated an overlapped operation that cannot be completed immediately.</summary>
		IOPending = 997,
		/// <summary>The <see cref="T:System.Net.Sockets.Socket" /> is already connected.</summary>
		IsConnected = 10056,
		/// <summary>The datagram is too long.</summary>
		MessageSize = 10040,
		/// <summary>The network is not available.</summary>
		NetworkDown = 10050,
		/// <summary>The application tried to set <see cref="F:System.Net.Sockets.SocketOptionName.KeepAlive" /> on a connection that has already timed out.</summary>
		NetworkReset = 10052,
		/// <summary>No route to the remote host exists.</summary>
		NetworkUnreachable = 10051,
		/// <summary>No free buffer space is available for a <see cref="T:System.Net.Sockets.Socket" /> operation.</summary>
		NoBufferSpaceAvailable = 10055,
		/// <summary>The requested name or IP address was not found on the name server.</summary>
		NoData = 11004,
		/// <summary>The error is unrecoverable or the requested database cannot be located.</summary>
		NoRecovery = 11003,
		/// <summary>The application tried to send or receive data, and the <see cref="T:System.Net.Sockets.Socket" /> is not connected.</summary>
		NotConnected = 10057,
		/// <summary>The underlying socket provider has not been initialized.</summary>
		NotInitialized = 10093,
		/// <summary>A <see cref="T:System.Net.Sockets.Socket" /> operation was attempted on a non-socket.</summary>
		NotSocket = 10038,
		/// <summary>The overlapped operation was aborted due to the closure of the <see cref="T:System.Net.Sockets.Socket" />.</summary>
		OperationAborted = 995,
		/// <summary>The address family is not supported by the protocol family.</summary>
		OperationNotSupported = 10045,
		/// <summary>Too many processes are using the underlying socket provider.</summary>
		ProcessLimit = 10067,
		/// <summary>The protocol family is not implemented or has not been configured.</summary>
		ProtocolFamilyNotSupported = 10046,
		/// <summary>The protocol is not implemented or has not been configured.</summary>
		ProtocolNotSupported = 10043,
		/// <summary>An unknown, invalid, or unsupported option or level was used with a <see cref="T:System.Net.Sockets.Socket" />.</summary>
		ProtocolOption = 10042,
		/// <summary>The protocol type is incorrect for this <see cref="T:System.Net.Sockets.Socket" />.</summary>
		ProtocolType = 10041,
		/// <summary>A request to send or receive data was disallowed because the <see cref="T:System.Net.Sockets.Socket" /> has already been closed.</summary>
		Shutdown = 10058,
		/// <summary>An unspecified <see cref="T:System.Net.Sockets.Socket" /> error has occurred.</summary>
		SocketError = -1,
		/// <summary>The support for the specified socket type does not exist in this address family.</summary>
		SocketNotSupported = 10044,
		/// <summary>The <see cref="T:System.Net.Sockets.Socket" /> operation succeeded.</summary>
		Success = 0,
		/// <summary>The network subsystem is unavailable.</summary>
		SystemNotReady = 10091,
		/// <summary>The connection attempt timed out, or the connected host has failed to respond.</summary>
		TimedOut = 10060,
		/// <summary>There are too many open sockets in the underlying socket provider.</summary>
		TooManyOpenSockets = 10024,
		/// <summary>The name of the host could not be resolved. Try again later.</summary>
		TryAgain = 11002,
		/// <summary>The specified class was not found.</summary>
		TypeNotFound = 10109,
		/// <summary>The version of the underlying socket provider is out of range.</summary>
		VersionNotSupported = 10092,
		/// <summary>An operation on a nonblocking socket cannot be completed immediately.</summary>
		WouldBlock = 10035
	}
}
