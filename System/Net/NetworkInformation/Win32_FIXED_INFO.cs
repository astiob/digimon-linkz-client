using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	[StructLayout(LayoutKind.Sequential)]
	internal class Win32_FIXED_INFO
	{
		private const int MAX_HOSTNAME_LEN = 128;

		private const int MAX_DOMAIN_NAME_LEN = 128;

		private const int MAX_SCOPE_ID_LEN = 256;

		private static Win32_FIXED_INFO fixed_info;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 132)]
		public string HostName;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 132)]
		public string DomainName;

		public IntPtr CurrentDnsServer;

		public Win32_IP_ADDR_STRING DnsServerList;

		public NetBiosNodeType NodeType;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string ScopeId;

		public uint EnableRouting;

		public uint EnableProxy;

		public uint EnableDns;

		[DllImport("iphlpapi.dll", SetLastError = true)]
		private static extern int GetNetworkParams(byte[] bytes, ref int size);

		public static Win32_FIXED_INFO Instance
		{
			get
			{
				if (Win32_FIXED_INFO.fixed_info == null)
				{
					Win32_FIXED_INFO.fixed_info = Win32_FIXED_INFO.GetInstance();
				}
				return Win32_FIXED_INFO.fixed_info;
			}
		}

		private unsafe static Win32_FIXED_INFO GetInstance()
		{
			int num = 0;
			Win32_FIXED_INFO.GetNetworkParams(null, ref num);
			byte[] array = new byte[num];
			Win32_FIXED_INFO.GetNetworkParams(array, ref num);
			Win32_FIXED_INFO win32_FIXED_INFO = new Win32_FIXED_INFO();
			fixed (byte* value = ref (array != null && array.Length != 0) ? ref array[0] : ref *null)
			{
				Marshal.PtrToStructure((IntPtr)((void*)value), win32_FIXED_INFO);
			}
			return win32_FIXED_INFO;
		}
	}
}
