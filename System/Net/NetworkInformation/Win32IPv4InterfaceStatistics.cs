using System;

namespace System.Net.NetworkInformation
{
	internal class Win32IPv4InterfaceStatistics : IPv4InterfaceStatistics
	{
		private Win32_MIB_IFROW info;

		public Win32IPv4InterfaceStatistics(Win32_MIB_IFROW info)
		{
			this.info = info;
		}

		public override long BytesReceived
		{
			get
			{
				return (long)this.info.InOctets;
			}
		}

		public override long BytesSent
		{
			get
			{
				return (long)this.info.OutOctets;
			}
		}

		public override long IncomingPacketsDiscarded
		{
			get
			{
				return (long)this.info.InDiscards;
			}
		}

		public override long IncomingPacketsWithErrors
		{
			get
			{
				return (long)this.info.InErrors;
			}
		}

		public override long IncomingUnknownProtocolPackets
		{
			get
			{
				return (long)this.info.InUnknownProtos;
			}
		}

		public override long NonUnicastPacketsReceived
		{
			get
			{
				return (long)this.info.InNUcastPkts;
			}
		}

		public override long NonUnicastPacketsSent
		{
			get
			{
				return (long)this.info.OutNUcastPkts;
			}
		}

		public override long OutgoingPacketsDiscarded
		{
			get
			{
				return (long)this.info.OutDiscards;
			}
		}

		public override long OutgoingPacketsWithErrors
		{
			get
			{
				return (long)this.info.OutErrors;
			}
		}

		public override long OutputQueueLength
		{
			get
			{
				return (long)this.info.OutQLen;
			}
		}

		public override long UnicastPacketsReceived
		{
			get
			{
				return (long)this.info.InUcastPkts;
			}
		}

		public override long UnicastPacketsSent
		{
			get
			{
				return (long)this.info.OutUcastPkts;
			}
		}
	}
}
