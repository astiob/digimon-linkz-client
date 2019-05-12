using System;

namespace System.Net.NetworkInformation
{
	/// <summary>Provides Internet Control Message Protocol for Internet Protocol version 6 (ICMPv6) statistical data for the local computer.</summary>
	public abstract class IcmpV6Statistics
	{
		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) messages received because of a packet having an unreachable address in its destination.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of Destination Unreachable messages received.</returns>
		public abstract long DestinationUnreachableMessagesReceived { get; }

		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) messages sent because of a packet having an unreachable address in its destination.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of Destination Unreachable messages sent.</returns>
		public abstract long DestinationUnreachableMessagesSent { get; }

		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Echo Reply messages received.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of number of ICMP Echo Reply messages received.</returns>
		public abstract long EchoRepliesReceived { get; }

		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Echo Reply messages sent.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of number of ICMP Echo Reply messages sent.</returns>
		public abstract long EchoRepliesSent { get; }

		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Echo Request messages received.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of number of ICMP Echo Request messages received.</returns>
		public abstract long EchoRequestsReceived { get; }

		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Echo Request messages sent.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of number of ICMP Echo Request messages sent.</returns>
		public abstract long EchoRequestsSent { get; }

		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) error messages received.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of ICMP error messages received.</returns>
		public abstract long ErrorsReceived { get; }

		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) error messages sent.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of ICMP error messages sent.</returns>
		public abstract long ErrorsSent { get; }

		/// <summary>Gets the number of Internet Group management Protocol (IGMP) Group Membership Query messages received.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of Group Membership Query messages received.</returns>
		public abstract long MembershipQueriesReceived { get; }

		/// <summary>Gets the number of Internet Group management Protocol (IGMP) Group Membership Query messages sent.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of Group Membership Query messages sent.</returns>
		public abstract long MembershipQueriesSent { get; }

		/// <summary>Gets the number of Internet Group Management Protocol (IGMP) Group Membership Reduction messages received.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of Group Membership Reduction messages received.</returns>
		public abstract long MembershipReductionsReceived { get; }

		/// <summary>Gets the number of Internet Group Management Protocol (IGMP) Group Membership Reduction messages sent.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of Group Membership Reduction messages sent.</returns>
		public abstract long MembershipReductionsSent { get; }

		/// <summary>Gets the number of Internet Group Management Protocol (IGMP) Group Membership Report messages received.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of Group Membership Report messages received.</returns>
		public abstract long MembershipReportsReceived { get; }

		/// <summary>Gets the number of Internet Group Management Protocol (IGMP) Group Membership Report messages sent.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of Group Membership Report messages sent.</returns>
		public abstract long MembershipReportsSent { get; }

		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) messages received.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of ICMPv6 messages received.</returns>
		public abstract long MessagesReceived { get; }

		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) messages sent.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of ICMPv6 messages sent.</returns>
		public abstract long MessagesSent { get; }

		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Neighbor Advertisement messages received.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of ICMP Neighbor Advertisement messages received.</returns>
		public abstract long NeighborAdvertisementsReceived { get; }

		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Neighbor Advertisement messages sent.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of Neighbor Advertisement messages sent.</returns>
		public abstract long NeighborAdvertisementsSent { get; }

		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Neighbor Solicitation messages received.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of Neighbor Solicitation messages received.</returns>
		public abstract long NeighborSolicitsReceived { get; }

		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Neighbor Solicitation messages sent.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of Neighbor Solicitation messages sent.</returns>
		public abstract long NeighborSolicitsSent { get; }

		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Packet Too Big messages received.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of ICMP Packet Too Big messages received.</returns>
		public abstract long PacketTooBigMessagesReceived { get; }

		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Packet Too Big messages sent.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of ICMP Packet Too Big messages sent.</returns>
		public abstract long PacketTooBigMessagesSent { get; }

		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Parameter Problem messages received.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of ICMP Parameter Problem messages received.</returns>
		public abstract long ParameterProblemsReceived { get; }

		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Parameter Problem messages sent.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of ICMP Parameter Problem messages sent.</returns>
		public abstract long ParameterProblemsSent { get; }

		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Redirect messages received.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of ICMP Redirect messages received.</returns>
		public abstract long RedirectsReceived { get; }

		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Redirect messages sent.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of ICMP Redirect messages sent.</returns>
		public abstract long RedirectsSent { get; }

		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Router Advertisement messages received.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of Router Advertisement messages received.</returns>
		public abstract long RouterAdvertisementsReceived { get; }

		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Router Advertisement messages sent.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of Router Advertisement messages sent.</returns>
		public abstract long RouterAdvertisementsSent { get; }

		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Router Solicitation messages received.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of Router Solicitation messages received.</returns>
		public abstract long RouterSolicitsReceived { get; }

		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Router Solicitation messages sent.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of Router Solicitation messages sent.</returns>
		public abstract long RouterSolicitsSent { get; }

		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Time Exceeded messages received.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of ICMP Time Exceeded messages received.</returns>
		public abstract long TimeExceededMessagesReceived { get; }

		/// <summary>Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Time Exceeded messages sent.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the total number of ICMP Time Exceeded messages sent.</returns>
		public abstract long TimeExceededMessagesSent { get; }
	}
}
