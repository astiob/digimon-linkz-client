using System;
using System.Collections.Specialized;
using System.Globalization;

namespace System.Net.NetworkInformation
{
	internal class MibIPGlobalStatistics : IPGlobalStatistics
	{
		private System.Collections.Specialized.StringDictionary dic;

		public MibIPGlobalStatistics(System.Collections.Specialized.StringDictionary dic)
		{
			this.dic = dic;
		}

		private long Get(string name)
		{
			return (this.dic[name] == null) ? 0L : long.Parse(this.dic[name], NumberFormatInfo.InvariantInfo);
		}

		public override int DefaultTtl
		{
			get
			{
				return (int)this.Get("DefaultTTL");
			}
		}

		public override bool ForwardingEnabled
		{
			get
			{
				return this.Get("Forwarding") != 0L;
			}
		}

		public override int NumberOfInterfaces
		{
			get
			{
				return (int)this.Get("NumIf");
			}
		}

		public override int NumberOfIPAddresses
		{
			get
			{
				return (int)this.Get("NumAddr");
			}
		}

		public override int NumberOfRoutes
		{
			get
			{
				return (int)this.Get("NumRoutes");
			}
		}

		public override long OutputPacketRequests
		{
			get
			{
				return this.Get("OutRequests");
			}
		}

		public override long OutputPacketRoutingDiscards
		{
			get
			{
				return this.Get("RoutingDiscards");
			}
		}

		public override long OutputPacketsDiscarded
		{
			get
			{
				return this.Get("OutDiscards");
			}
		}

		public override long OutputPacketsWithNoRoute
		{
			get
			{
				return this.Get("OutNoRoutes");
			}
		}

		public override long PacketFragmentFailures
		{
			get
			{
				return this.Get("FragFails");
			}
		}

		public override long PacketReassembliesRequired
		{
			get
			{
				return this.Get("ReasmReqds");
			}
		}

		public override long PacketReassemblyFailures
		{
			get
			{
				return this.Get("ReasmFails");
			}
		}

		public override long PacketReassemblyTimeout
		{
			get
			{
				return this.Get("ReasmTimeout");
			}
		}

		public override long PacketsFragmented
		{
			get
			{
				return this.Get("FragOks");
			}
		}

		public override long PacketsReassembled
		{
			get
			{
				return this.Get("ReasmOks");
			}
		}

		public override long ReceivedPackets
		{
			get
			{
				return this.Get("InReceives");
			}
		}

		public override long ReceivedPacketsDelivered
		{
			get
			{
				return this.Get("InDelivers");
			}
		}

		public override long ReceivedPacketsDiscarded
		{
			get
			{
				return this.Get("InDiscards");
			}
		}

		public override long ReceivedPacketsForwarded
		{
			get
			{
				return this.Get("ForwDatagrams");
			}
		}

		public override long ReceivedPacketsWithAddressErrors
		{
			get
			{
				return this.Get("InAddrErrors");
			}
		}

		public override long ReceivedPacketsWithHeadersErrors
		{
			get
			{
				return this.Get("InHdrErrors");
			}
		}

		public override long ReceivedPacketsWithUnknownProtocol
		{
			get
			{
				return this.Get("InUnknownProtos");
			}
		}
	}
}
