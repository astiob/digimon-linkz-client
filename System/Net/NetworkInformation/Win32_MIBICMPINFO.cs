using System;

namespace System.Net.NetworkInformation
{
	internal struct Win32_MIBICMPINFO
	{
		public Win32_MIBICMPSTATS InStats;

		public Win32_MIBICMPSTATS OutStats;
	}
}
