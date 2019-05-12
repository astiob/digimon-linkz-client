using System;

namespace System.Net.Sockets
{
	/// <summary>The <see cref="T:System.Net.Sockets.TransmitFileOptions" /> enumeration defines values used in file transfer requests.</summary>
	[Flags]
	public enum TransmitFileOptions
	{
		/// <summary>Use the default thread to process long file transfer requests.</summary>
		UseDefaultWorkerThread = 0,
		/// <summary>Start a transport-level disconnect after all the file data has been queued for transmission. When used with <see cref="F:System.Net.Sockets.TransmitFileOptions.ReuseSocket" />, these flags return the socket to a disconnected, reusable state after the file has been transmitted.</summary>
		Disconnect = 1,
		/// <summary>The socket handle may be reused when the request completes. This flag is valid only if <see cref="F:System.Net.Sockets.TransmitFileOptions.Disconnect" /> is also specified. When used with <see cref="F:System.Net.Sockets.TransmitFileOptions.Disconnect" />, these flags return the socket to a disconnected, reusable state after the file has been transmitted.</summary>
		ReuseSocket = 2,
		/// <summary>Complete the file transfer request immediately, without pending. If this flag is specified and the file transfer succeeds, the data has been accepted by the system but not necessarily acknowledged by the remote end. Do not use this flag with the <see cref="F:System.Net.Sockets.TransmitFileOptions.Disconnect" /> and <see cref="F:System.Net.Sockets.TransmitFileOptions.ReuseSocket" /> flags.</summary>
		WriteBehind = 4,
		/// <summary>Use system threads to process long file transfer requests.</summary>
		UseSystemThread = 16,
		/// <summary>Use kernel asynchronous procedure calls (APCs) instead of worker threads to process long file transfer requests. Long requests are defined as requests that require more than a single read from the file or a cache; the request therefore depends on the size of the file and the specified length of the send packet.</summary>
		UseKernelApc = 32
	}
}
