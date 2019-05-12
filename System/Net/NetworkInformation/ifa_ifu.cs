using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	[StructLayout(LayoutKind.Explicit)]
	internal struct ifa_ifu
	{
		[FieldOffset(0)]
		public IntPtr ifu_broadaddr;

		[FieldOffset(0)]
		public IntPtr ifu_dstaddr;
	}
}
