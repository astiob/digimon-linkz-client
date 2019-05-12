using System;

namespace System.Net.NetworkInformation
{
	internal class Win32IPGlobalStatistics : IPGlobalStatistics
	{
		private Win32_MIB_IPSTATS info;

		public Win32IPGlobalStatistics(Win32_MIB_IPSTATS info)
		{
			this.info = info;
		}

		public override int DefaultTtl
		{
			get
			{
				return this.info.DefaultTTL;
			}
		}

		public override bool ForwardingEnabled
		{
			get
			{
				return this.info.Forwarding != 0;
			}
		}

		public override int NumberOfInterfaces
		{
			get
			{
				return this.info.NumIf;
			}
		}

		public override int NumberOfIPAddresses
		{
			get
			{
				return this.info.NumAddr;
			}
		}

		public override int NumberOfRoutes
		{
			get
			{
				return this.info.NumRoutes;
			}
		}

		public override long OutputPacketRequests
		{
			get
			{
				return (long)((ulong)this.info.OutRequests);
			}
		}

		public override long OutputPacketRoutingDiscards
		{
			get
			{
				return (long)((ulong)this.info.RoutingDiscards);
			}
		}

		public override long OutputPacketsDiscarded
		{
			get
			{
				return (long)((ulong)this.info.OutDiscards);
			}
		}

		public override long OutputPacketsWithNoRoute
		{
			get
			{
				return (long)((ulong)this.info.OutNoRoutes);
			}
		}

		public override long PacketFragmentFailures
		{
			get
			{
				return (long)((ulong)this.info.FragFails);
			}
		}

		public override long PacketReassembliesRequired
		{
			get
			{
				return (long)((ulong)this.info.ReasmReqds);
			}
		}

		public override long PacketReassemblyFailures
		{
			get
			{
				return (long)((ulong)this.info.ReasmFails);
			}
		}

		public override long PacketReassemblyTimeout
		{
			get
			{
				return (long)((ulong)this.info.ReasmTimeout);
			}
		}

		public override long PacketsFragmented
		{
			get
			{
				return (long)((ulong)this.info.FragOks);
			}
		}

		public override long PacketsReassembled
		{
			get
			{
				return (long)((ulong)this.info.ReasmOks);
			}
		}

		public override long ReceivedPackets
		{
			get
			{
				return (long)((ulong)this.info.InReceives);
			}
		}

		public override long ReceivedPacketsDelivered
		{
			get
			{
				return (long)((ulong)this.info.InDelivers);
			}
		}

		public override long ReceivedPacketsDiscarded
		{
			get
			{
				return (long)((ulong)this.info.InDiscards);
			}
		}

		public override long ReceivedPacketsForwarded
		{
			get
			{
				return (long)((ulong)this.info.ForwDatagrams);
			}
		}

		public override long ReceivedPacketsWithAddressErrors
		{
			get
			{
				return (long)((ulong)this.info.InAddrErrors);
			}
		}

		public override long ReceivedPacketsWithHeadersErrors
		{
			get
			{
				return (long)((ulong)this.info.InHdrErrors);
			}
		}

		public override long ReceivedPacketsWithUnknownProtocol
		{
			get
			{
				return (long)((ulong)this.info.InUnknownProtos);
			}
		}
	}
}
