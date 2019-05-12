using System;

namespace System.Net.NetworkInformation
{
	internal struct Win32_MIB_ICMP_EX
	{
		public Win32_MIBICMPSTATS_EX InStats;

		public Win32_MIBICMPSTATS_EX OutStats;
	}
}
