using System;

namespace System.Net.NetworkInformation
{
	internal struct Win32_MIB_UDPSTATS
	{
		public uint InDatagrams;

		public uint NoPorts;

		public uint InErrors;

		public uint OutDatagrams;

		public int NumAddrs;
	}
}
