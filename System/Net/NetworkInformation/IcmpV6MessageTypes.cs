using System;

namespace System.Net.NetworkInformation
{
	internal class IcmpV6MessageTypes
	{
		public const int DestinationUnreachable = 1;

		public const int PacketTooBig = 2;

		public const int TimeExceeded = 3;

		public const int ParameterProblem = 4;

		public const int EchoRequest = 128;

		public const int EchoReply = 129;

		public const int GroupMembershipQuery = 130;

		public const int GroupMembershipReport = 131;

		public const int GroupMembershipReduction = 132;

		public const int RouterSolicitation = 133;

		public const int RouterAdvertisement = 134;

		public const int NeighborSolicitation = 135;

		public const int NeighborAdvertisement = 136;

		public const int Redirect = 137;

		public const int RouterRenumbering = 138;
	}
}
