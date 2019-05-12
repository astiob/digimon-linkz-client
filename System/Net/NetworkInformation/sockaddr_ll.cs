using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	internal struct sockaddr_ll
	{
		public ushort sll_family;

		public ushort sll_protocol;

		public int sll_ifindex;

		public ushort sll_hatype;

		public byte sll_pkttype;

		public byte sll_halen;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public byte[] sll_addr;
	}
}
