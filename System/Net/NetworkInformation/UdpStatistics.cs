using System;

namespace System.Net.NetworkInformation
{
	/// <summary>Provides User Datagram Protocol (UDP) statistical data.</summary>
	public abstract class UdpStatistics
	{
		/// <summary>Gets the number of User Datagram Protocol (UDP) datagrams that were received.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of datagrams that were delivered to UDP users.</returns>
		public abstract long DatagramsReceived { get; }

		/// <summary>Gets the number of User Datagram Protocol (UDP) datagrams that were sent.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of datagrams that were sent.</returns>
		public abstract long DatagramsSent { get; }

		/// <summary>Gets the number of User Datagram Protocol (UDP) datagrams that were received and discarded because of port errors.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of received UDP datagrams that were discarded because there was no listening application at the destination port.</returns>
		public abstract long IncomingDatagramsDiscarded { get; }

		/// <summary>Gets the number of User Datagram Protocol (UDP) datagrams that were received and discarded because of errors other than bad port information.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of received UDP datagrams that could not be delivered for reasons other than the lack of an application at the destination port.</returns>
		public abstract long IncomingDatagramsWithErrors { get; }

		/// <summary>Gets the number of local endpoints that are listening for User Datagram Protocol (UDP) datagrams.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of sockets that are listening for UDP datagrams.</returns>
		public abstract int UdpListeners { get; }
	}
}
