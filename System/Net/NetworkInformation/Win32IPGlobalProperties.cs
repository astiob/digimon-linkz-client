using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	internal class Win32IPGlobalProperties : IPGlobalProperties
	{
		public const int AF_INET = 2;

		public const int AF_INET6 = 23;

		private unsafe void FillTcpTable(out List<Win32IPGlobalProperties.Win32_MIB_TCPROW> tab4, out List<Win32IPGlobalProperties.Win32_MIB_TCP6ROW> tab6)
		{
			tab4 = new List<Win32IPGlobalProperties.Win32_MIB_TCPROW>();
			int num = 0;
			Win32IPGlobalProperties.GetTcpTable(null, ref num, true);
			byte[] array = new byte[num];
			Win32IPGlobalProperties.GetTcpTable(array, ref num, true);
			int num2 = Marshal.SizeOf(typeof(Win32IPGlobalProperties.Win32_MIB_TCPROW));
			fixed (byte* ptr = ref (array != null && array.Length != 0) ? ref array[0] : ref *null)
			{
				int num3 = Marshal.ReadInt32((IntPtr)((void*)ptr));
				for (int i = 0; i < num3; i++)
				{
					Win32IPGlobalProperties.Win32_MIB_TCPROW win32_MIB_TCPROW = new Win32IPGlobalProperties.Win32_MIB_TCPROW();
					Marshal.PtrToStructure((IntPtr)((void*)(ptr + i * num2 + 4)), win32_MIB_TCPROW);
					tab4.Add(win32_MIB_TCPROW);
				}
			}
			tab6 = new List<Win32IPGlobalProperties.Win32_MIB_TCP6ROW>();
			if (Environment.OSVersion.Version.Major >= 6)
			{
				int num4 = 0;
				Win32IPGlobalProperties.GetTcp6Table(null, ref num4, true);
				byte[] array2 = new byte[num4];
				Win32IPGlobalProperties.GetTcp6Table(array2, ref num4, true);
				int num5 = Marshal.SizeOf(typeof(Win32IPGlobalProperties.Win32_MIB_TCP6ROW));
				fixed (byte* ptr2 = ref (array2 != null && array2.Length != 0) ? ref array2[0] : ref *null)
				{
					int num6 = Marshal.ReadInt32((IntPtr)((void*)ptr2));
					for (int j = 0; j < num6; j++)
					{
						Win32IPGlobalProperties.Win32_MIB_TCP6ROW win32_MIB_TCP6ROW = new Win32IPGlobalProperties.Win32_MIB_TCP6ROW();
						Marshal.PtrToStructure((IntPtr)((void*)(ptr2 + j * num5 + 4)), win32_MIB_TCP6ROW);
						tab6.Add(win32_MIB_TCP6ROW);
					}
				}
			}
		}

		private bool IsListenerState(TcpState state)
		{
			switch (state)
			{
			case TcpState.Listen:
			case TcpState.SynSent:
			case TcpState.FinWait1:
			case TcpState.FinWait2:
			case TcpState.CloseWait:
				return true;
			}
			return false;
		}

		public override TcpConnectionInformation[] GetActiveTcpConnections()
		{
			List<Win32IPGlobalProperties.Win32_MIB_TCPROW> list = null;
			List<Win32IPGlobalProperties.Win32_MIB_TCP6ROW> list2 = null;
			this.FillTcpTable(out list, out list2);
			int count = list.Count;
			TcpConnectionInformation[] array = new TcpConnectionInformation[count + list2.Count];
			for (int i = 0; i < count; i++)
			{
				array[i] = list[i].TcpInfo;
			}
			for (int j = 0; j < list2.Count; j++)
			{
				array[count + j] = list2[j].TcpInfo;
			}
			return array;
		}

		public override IPEndPoint[] GetActiveTcpListeners()
		{
			List<Win32IPGlobalProperties.Win32_MIB_TCPROW> list = null;
			List<Win32IPGlobalProperties.Win32_MIB_TCP6ROW> list2 = null;
			this.FillTcpTable(out list, out list2);
			List<IPEndPoint> list3 = new List<IPEndPoint>();
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				if (this.IsListenerState(list[i].State))
				{
					list3.Add(list[i].LocalEndPoint);
				}
				i++;
			}
			int j = 0;
			int count2 = list2.Count;
			while (j < count2)
			{
				if (this.IsListenerState(list2[j].State))
				{
					list3.Add(list2[j].LocalEndPoint);
				}
				j++;
			}
			return list3.ToArray();
		}

		public unsafe override IPEndPoint[] GetActiveUdpListeners()
		{
			List<IPEndPoint> list = new List<IPEndPoint>();
			int num = 0;
			Win32IPGlobalProperties.GetUdpTable(null, ref num, true);
			byte[] array = new byte[num];
			Win32IPGlobalProperties.GetUdpTable(array, ref num, true);
			int num2 = Marshal.SizeOf(typeof(Win32IPGlobalProperties.Win32_MIB_UDPROW));
			fixed (byte* ptr = ref (array != null && array.Length != 0) ? ref array[0] : ref *null)
			{
				int num3 = Marshal.ReadInt32((IntPtr)((void*)ptr));
				for (int i = 0; i < num3; i++)
				{
					Win32IPGlobalProperties.Win32_MIB_UDPROW win32_MIB_UDPROW = new Win32IPGlobalProperties.Win32_MIB_UDPROW();
					Marshal.PtrToStructure((IntPtr)((void*)(ptr + i * num2 + 4)), win32_MIB_UDPROW);
					list.Add(win32_MIB_UDPROW.LocalEndPoint);
				}
			}
			if (Environment.OSVersion.Version.Major >= 6)
			{
				int num4 = 0;
				Win32IPGlobalProperties.GetUdp6Table(null, ref num4, true);
				byte[] array2 = new byte[num4];
				Win32IPGlobalProperties.GetUdp6Table(array2, ref num4, true);
				int num5 = Marshal.SizeOf(typeof(Win32IPGlobalProperties.Win32_MIB_UDP6ROW));
				fixed (byte* ptr2 = ref (array2 != null && array2.Length != 0) ? ref array2[0] : ref *null)
				{
					int num6 = Marshal.ReadInt32((IntPtr)((void*)ptr2));
					for (int j = 0; j < num6; j++)
					{
						Win32IPGlobalProperties.Win32_MIB_UDP6ROW win32_MIB_UDP6ROW = new Win32IPGlobalProperties.Win32_MIB_UDP6ROW();
						Marshal.PtrToStructure((IntPtr)((void*)(ptr2 + j * num5 + 4)), win32_MIB_UDP6ROW);
						list.Add(win32_MIB_UDP6ROW.LocalEndPoint);
					}
				}
			}
			return list.ToArray();
		}

		public override IcmpV4Statistics GetIcmpV4Statistics()
		{
			if (!System.Net.Sockets.Socket.SupportsIPv4)
			{
				throw new NetworkInformationException();
			}
			Win32_MIBICMPINFO info;
			Win32IPGlobalProperties.GetIcmpStatistics(out info, 2);
			return new Win32IcmpV4Statistics(info);
		}

		public override IcmpV6Statistics GetIcmpV6Statistics()
		{
			if (!System.Net.Sockets.Socket.OSSupportsIPv6)
			{
				throw new NetworkInformationException();
			}
			Win32_MIB_ICMP_EX info;
			Win32IPGlobalProperties.GetIcmpStatisticsEx(out info, 23);
			return new Win32IcmpV6Statistics(info);
		}

		public override IPGlobalStatistics GetIPv4GlobalStatistics()
		{
			if (!System.Net.Sockets.Socket.SupportsIPv4)
			{
				throw new NetworkInformationException();
			}
			Win32_MIB_IPSTATS info;
			Win32IPGlobalProperties.GetIPStatisticsEx(out info, 2);
			return new Win32IPGlobalStatistics(info);
		}

		public override IPGlobalStatistics GetIPv6GlobalStatistics()
		{
			if (!System.Net.Sockets.Socket.OSSupportsIPv6)
			{
				throw new NetworkInformationException();
			}
			Win32_MIB_IPSTATS info;
			Win32IPGlobalProperties.GetIPStatisticsEx(out info, 23);
			return new Win32IPGlobalStatistics(info);
		}

		public override TcpStatistics GetTcpIPv4Statistics()
		{
			if (!System.Net.Sockets.Socket.SupportsIPv4)
			{
				throw new NetworkInformationException();
			}
			Win32_MIB_TCPSTATS info;
			Win32IPGlobalProperties.GetTcpStatisticsEx(out info, 2);
			return new Win32TcpStatistics(info);
		}

		public override TcpStatistics GetTcpIPv6Statistics()
		{
			if (!System.Net.Sockets.Socket.OSSupportsIPv6)
			{
				throw new NetworkInformationException();
			}
			Win32_MIB_TCPSTATS info;
			Win32IPGlobalProperties.GetTcpStatisticsEx(out info, 23);
			return new Win32TcpStatistics(info);
		}

		public override UdpStatistics GetUdpIPv4Statistics()
		{
			if (!System.Net.Sockets.Socket.SupportsIPv4)
			{
				throw new NetworkInformationException();
			}
			Win32_MIB_UDPSTATS info;
			Win32IPGlobalProperties.GetUdpStatisticsEx(out info, 2);
			return new Win32UdpStatistics(info);
		}

		public override UdpStatistics GetUdpIPv6Statistics()
		{
			if (!System.Net.Sockets.Socket.OSSupportsIPv6)
			{
				throw new NetworkInformationException();
			}
			Win32_MIB_UDPSTATS info;
			Win32IPGlobalProperties.GetUdpStatisticsEx(out info, 23);
			return new Win32UdpStatistics(info);
		}

		public override string DhcpScopeName
		{
			get
			{
				return Win32_FIXED_INFO.Instance.ScopeId;
			}
		}

		public override string DomainName
		{
			get
			{
				return Win32_FIXED_INFO.Instance.DomainName;
			}
		}

		public override string HostName
		{
			get
			{
				return Win32_FIXED_INFO.Instance.HostName;
			}
		}

		public override bool IsWinsProxy
		{
			get
			{
				return Win32_FIXED_INFO.Instance.EnableProxy != 0u;
			}
		}

		public override NetBiosNodeType NodeType
		{
			get
			{
				return Win32_FIXED_INFO.Instance.NodeType;
			}
		}

		[DllImport("Iphlpapi.dll")]
		private static extern int GetTcpTable(byte[] pTcpTable, ref int pdwSize, bool bOrder);

		[DllImport("Iphlpapi.dll")]
		private static extern int GetTcp6Table(byte[] TcpTable, ref int SizePointer, bool Order);

		[DllImport("Iphlpapi.dll")]
		private static extern int GetUdpTable(byte[] pUdpTable, ref int pdwSize, bool bOrder);

		[DllImport("Iphlpapi.dll")]
		private static extern int GetUdp6Table(byte[] Udp6Table, ref int SizePointer, bool Order);

		[DllImport("Iphlpapi.dll")]
		private static extern int GetTcpStatisticsEx(out Win32_MIB_TCPSTATS pStats, int dwFamily);

		[DllImport("Iphlpapi.dll")]
		private static extern int GetUdpStatisticsEx(out Win32_MIB_UDPSTATS pStats, int dwFamily);

		[DllImport("Iphlpapi.dll")]
		private static extern int GetIcmpStatistics(out Win32_MIBICMPINFO pStats, int dwFamily);

		[DllImport("Iphlpapi.dll")]
		private static extern int GetIcmpStatisticsEx(out Win32_MIB_ICMP_EX pStats, int dwFamily);

		[DllImport("Iphlpapi.dll")]
		private static extern int GetIPStatisticsEx(out Win32_MIB_IPSTATS pStats, int dwFamily);

		[StructLayout(LayoutKind.Explicit)]
		private struct Win32_IN6_ADDR
		{
			[FieldOffset(0)]
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public byte[] Bytes;
		}

		[StructLayout(LayoutKind.Sequential)]
		private class Win32_MIB_TCPROW
		{
			public TcpState State;

			public uint LocalAddr;

			public int LocalPort;

			public uint RemoteAddr;

			public int RemotePort;

			public IPEndPoint LocalEndPoint
			{
				get
				{
					return new IPEndPoint((long)((ulong)this.LocalAddr), this.LocalPort);
				}
			}

			public IPEndPoint RemoteEndPoint
			{
				get
				{
					return new IPEndPoint((long)((ulong)this.RemoteAddr), this.RemotePort);
				}
			}

			public TcpConnectionInformation TcpInfo
			{
				get
				{
					return new TcpConnectionInformationImpl(this.LocalEndPoint, this.RemoteEndPoint, this.State);
				}
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		private class Win32_MIB_TCP6ROW
		{
			public TcpState State;

			public Win32IPGlobalProperties.Win32_IN6_ADDR LocalAddr;

			public uint LocalScopeId;

			public int LocalPort;

			public Win32IPGlobalProperties.Win32_IN6_ADDR RemoteAddr;

			public uint RemoteScopeId;

			public int RemotePort;

			public IPEndPoint LocalEndPoint
			{
				get
				{
					return new IPEndPoint(new IPAddress(this.LocalAddr.Bytes, (long)((ulong)this.LocalScopeId)), this.LocalPort);
				}
			}

			public IPEndPoint RemoteEndPoint
			{
				get
				{
					return new IPEndPoint(new IPAddress(this.RemoteAddr.Bytes, (long)((ulong)this.RemoteScopeId)), this.RemotePort);
				}
			}

			public TcpConnectionInformation TcpInfo
			{
				get
				{
					return new TcpConnectionInformationImpl(this.LocalEndPoint, this.RemoteEndPoint, this.State);
				}
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		private class Win32_MIB_UDPROW
		{
			public uint LocalAddr;

			public int LocalPort;

			public IPEndPoint LocalEndPoint
			{
				get
				{
					return new IPEndPoint((long)((ulong)this.LocalAddr), this.LocalPort);
				}
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		private class Win32_MIB_UDP6ROW
		{
			public Win32IPGlobalProperties.Win32_IN6_ADDR LocalAddr;

			public uint LocalScopeId;

			public int LocalPort;

			public IPEndPoint LocalEndPoint
			{
				get
				{
					return new IPEndPoint(new IPAddress(this.LocalAddr.Bytes, (long)((ulong)this.LocalScopeId)), this.LocalPort);
				}
			}
		}
	}
}
