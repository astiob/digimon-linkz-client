using System;

namespace System.Net.NetworkInformation
{
	internal struct ifaddrs
	{
		public IntPtr ifa_next;

		public string ifa_name;

		public uint ifa_flags;

		public IntPtr ifa_addr;

		public IntPtr ifa_netmask;

		public ifa_ifu ifa_ifu;

		public IntPtr ifa_data;
	}
}
