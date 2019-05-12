using System;

namespace System.Net.NetworkInformation
{
	internal class Win32UdpStatistics : UdpStatistics
	{
		private Win32_MIB_UDPSTATS info;

		public Win32UdpStatistics(Win32_MIB_UDPSTATS info)
		{
			this.info = info;
		}

		public override long DatagramsReceived
		{
			get
			{
				return (long)((ulong)this.info.InDatagrams);
			}
		}

		public override long DatagramsSent
		{
			get
			{
				return (long)((ulong)this.info.OutDatagrams);
			}
		}

		public override long IncomingDatagramsDiscarded
		{
			get
			{
				return (long)((ulong)this.info.NoPorts);
			}
		}

		public override long IncomingDatagramsWithErrors
		{
			get
			{
				return (long)((ulong)this.info.InErrors);
			}
		}

		public override int UdpListeners
		{
			get
			{
				return this.info.NumAddrs;
			}
		}
	}
}
