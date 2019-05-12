using System;

namespace System.Net.NetworkInformation
{
	/// <summary>Provides Internet Protocol (IP) statistical data.</summary>
	public abstract class IPGlobalStatistics
	{
		/// <summary>Gets the default time-to-live (TTL) value for Internet Protocol (IP) packets.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the TTL.</returns>
		public abstract int DefaultTtl { get; }

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that specifies whether Internet Protocol (IP) packet forwarding is enabled.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that specifies whether packet forwarding is enabled.</returns>
		public abstract bool ForwardingEnabled { get; }

		/// <summary>Gets the number of network interfaces.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value containing the number of network interfaces for the address family used to obtain this <see cref="T:System.Net.NetworkInformation.IPGlobalStatistics" /> instance.</returns>
		public abstract int NumberOfInterfaces { get; }

		/// <summary>Gets the number of Internet Protocol (IP) addresses assigned to the local computer.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that indicates the number of IP addresses assigned to the address family (Internet Protocol version 4 or Internet Protocol version 6) described by this object.</returns>
		public abstract int NumberOfIPAddresses { get; }

		/// <summary>Gets the number of routes in the Internet Protocol (IP) routing table.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of routes in the routing table.</returns>
		public abstract int NumberOfRoutes { get; }

		/// <summary>Gets the number of outbound Internet Protocol (IP) packets.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of outgoing packets.</returns>
		public abstract long OutputPacketRequests { get; }

		/// <summary>Gets the number of routes that have been discarded from the routing table.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of valid routes that have been discarded.</returns>
		public abstract long OutputPacketRoutingDiscards { get; }

		/// <summary>Gets the number of transmitted Internet Protocol (IP) packets that have been discarded.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of outgoing packets that have been discarded.</returns>
		public abstract long OutputPacketsDiscarded { get; }

		/// <summary>Gets the number of Internet Protocol (IP) packets for which the local computer could not determine a route to the destination address.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the number of packets that could not be sent because a route could not be found.</returns>
		public abstract long OutputPacketsWithNoRoute { get; }

		/// <summary>Gets the number of Internet Protocol (IP) packets that could not be fragmented.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of packets that required fragmentation but had the "Don't Fragment" bit set.</returns>
		public abstract long PacketFragmentFailures { get; }

		/// <summary>Gets the number of Internet Protocol (IP) packets that required reassembly.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of packet reassemblies required.</returns>
		public abstract long PacketReassembliesRequired { get; }

		/// <summary>Gets the number of Internet Protocol (IP) packets that were not successfully reassembled.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of packets that could not be reassembled.</returns>
		public abstract long PacketReassemblyFailures { get; }

		/// <summary>Gets the maximum amount of time within which all fragments of an Internet Protocol (IP) packet must arrive.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the maximum number of milliseconds within which all fragments of a packet must arrive to avoid being discarded.</returns>
		public abstract long PacketReassemblyTimeout { get; }

		/// <summary>Gets the number of Internet Protocol (IP) packets fragmented.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of fragmented packets.</returns>
		public abstract long PacketsFragmented { get; }

		/// <summary>Gets the number of Internet Protocol (IP) packets reassembled.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of fragmented packets that have been successfully reassembled.</returns>
		public abstract long PacketsReassembled { get; }

		/// <summary>Gets the number of Internet Protocol (IP) packets received.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of IP packets received.</returns>
		public abstract long ReceivedPackets { get; }

		/// <summary>Gets the number of Internet Protocol (IP) packets delivered.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of IP packets delivered.</returns>
		public abstract long ReceivedPacketsDelivered { get; }

		/// <summary>Gets the number of Internet Protocol (IP) packets that have been received and discarded.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of incoming packets that have been discarded.</returns>
		public abstract long ReceivedPacketsDiscarded { get; }

		/// <summary>Gets the number of Internet Protocol (IP) packets forwarded.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of forwarded packets.</returns>
		public abstract long ReceivedPacketsForwarded { get; }

		/// <summary>Gets the number of Internet Protocol (IP) packets with address errors that were received.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of IP packets received with errors in the address portion of the header.</returns>
		public abstract long ReceivedPacketsWithAddressErrors { get; }

		/// <summary>Gets the number of Internet Protocol (IP) packets with header errors that were received.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of IP packets received and discarded due to errors in the header.</returns>
		public abstract long ReceivedPacketsWithHeadersErrors { get; }

		/// <summary>Gets the number of Internet Protocol (IP) packets received on the local machine with an unknown protocol in the header.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that indicates the total number of IP packets received with an unknown protocol.</returns>
		public abstract long ReceivedPacketsWithUnknownProtocol { get; }
	}
}
