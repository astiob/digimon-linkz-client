using System;

namespace System.Net.NetworkInformation
{
	internal struct Win32_MIB_IPSTATS
	{
		public int Forwarding;

		public int DefaultTTL;

		public uint InReceives;

		public uint InHdrErrors;

		public uint InAddrErrors;

		public uint ForwDatagrams;

		public uint InUnknownProtos;

		public uint InDiscards;

		public uint InDelivers;

		public uint OutRequests;

		public uint RoutingDiscards;

		public uint OutDiscards;

		public uint OutNoRoutes;

		public uint ReasmTimeout;

		public uint ReasmReqds;

		public uint ReasmOks;

		public uint ReasmFails;

		public uint FragOks;

		public uint FragFails;

		public uint FragCreates;

		public int NumIf;

		public int NumAddr;

		public int NumRoutes;
	}
}
