using System;

namespace System.Net.NetworkInformation
{
	internal class LinuxIPv4InterfaceStatistics : IPv4InterfaceStatistics
	{
		private LinuxNetworkInterface linux;

		public LinuxIPv4InterfaceStatistics(LinuxNetworkInterface parent)
		{
			this.linux = parent;
		}

		private long Read(string file)
		{
			long result;
			try
			{
				result = long.Parse(NetworkInterface.ReadLine(this.linux.IfacePath + file));
			}
			catch
			{
				result = 0L;
			}
			return result;
		}

		public override long BytesReceived
		{
			get
			{
				return this.Read("statistics/rx_bytes");
			}
		}

		public override long BytesSent
		{
			get
			{
				return this.Read("statistics/tx_bytes");
			}
		}

		public override long IncomingPacketsDiscarded
		{
			get
			{
				return this.Read("statistics/rx_dropped");
			}
		}

		public override long IncomingPacketsWithErrors
		{
			get
			{
				return this.Read("statistics/rx_errors");
			}
		}

		public override long IncomingUnknownProtocolPackets
		{
			get
			{
				return 0L;
			}
		}

		public override long NonUnicastPacketsReceived
		{
			get
			{
				return this.Read("statistics/multicast");
			}
		}

		public override long NonUnicastPacketsSent
		{
			get
			{
				return this.Read("statistics/multicast");
			}
		}

		public override long OutgoingPacketsDiscarded
		{
			get
			{
				return this.Read("statistics/tx_dropped");
			}
		}

		public override long OutgoingPacketsWithErrors
		{
			get
			{
				return this.Read("statistics/tx_errors");
			}
		}

		public override long OutputQueueLength
		{
			get
			{
				return 1024L;
			}
		}

		public override long UnicastPacketsReceived
		{
			get
			{
				return this.Read("statistics/rx_packets");
			}
		}

		public override long UnicastPacketsSent
		{
			get
			{
				return this.Read("statistics/tx_packets");
			}
		}
	}
}
