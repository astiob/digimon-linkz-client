using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	internal struct in6_addr
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public byte[] u6_addr8;
	}
}
