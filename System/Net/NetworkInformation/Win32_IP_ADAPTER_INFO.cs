using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	[StructLayout(LayoutKind.Sequential)]
	internal class Win32_IP_ADAPTER_INFO
	{
		private const int MAX_ADAPTER_NAME_LENGTH = 256;

		private const int MAX_ADAPTER_DESCRIPTION_LENGTH = 128;

		private const int MAX_ADAPTER_ADDRESS_LENGTH = 8;

		public IntPtr Next;

		public int ComboIndex;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string AdapterName;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 132)]
		public string Description;

		public uint AddressLength;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public byte[] Address;

		public uint Index;

		public uint Type;

		public uint DhcpEnabled;

		public IntPtr CurrentIpAddress;

		public Win32_IP_ADDR_STRING IpAddressList;

		public Win32_IP_ADDR_STRING GatewayList;

		public Win32_IP_ADDR_STRING DhcpServer;

		public bool HaveWins;

		public Win32_IP_ADDR_STRING PrimaryWinsServer;

		public Win32_IP_ADDR_STRING SecondaryWinsServer;

		public long LeaseObtained;

		public long LeaseExpires;
	}
}
