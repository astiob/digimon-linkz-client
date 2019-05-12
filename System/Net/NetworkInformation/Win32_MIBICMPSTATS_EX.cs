using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	internal struct Win32_MIBICMPSTATS_EX
	{
		public uint Msgs;

		public uint Errors;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public uint[] Counts;
	}
}
