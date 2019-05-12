using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	internal class Win32NetworkInterface2 : NetworkInterface
	{
		private Win32_IP_ADAPTER_ADDRESSES addr;

		private Win32_MIB_IFROW mib4;

		private Win32_MIB_IFROW mib6;

		private Win32IPv4InterfaceStatistics ip4stats;

		private IPInterfaceProperties ip_if_props;

		private Win32NetworkInterface2(Win32_IP_ADAPTER_ADDRESSES addr)
		{
			this.addr = addr;
			this.mib4 = default(Win32_MIB_IFROW);
			this.mib4.Index = addr.Alignment.IfIndex;
			if (Win32NetworkInterface2.GetIfEntry(ref this.mib4) != 0)
			{
				this.mib4.Index = -1;
			}
			this.mib6 = default(Win32_MIB_IFROW);
			this.mib6.Index = addr.Ipv6IfIndex;
			if (Win32NetworkInterface2.GetIfEntry(ref this.mib6) != 0)
			{
				this.mib6.Index = -1;
			}
			this.ip4stats = new Win32IPv4InterfaceStatistics(this.mib4);
			this.ip_if_props = new Win32IPInterfaceProperties2(addr, this.mib4, this.mib6);
		}

		[DllImport("iphlpapi.dll", SetLastError = true)]
		private static extern int GetAdaptersInfo(byte[] info, ref int size);

		[DllImport("iphlpapi.dll", SetLastError = true)]
		private static extern int GetAdaptersAddresses(uint family, uint flags, IntPtr reserved, byte[] info, ref int size);

		[DllImport("iphlpapi.dll", SetLastError = true)]
		private static extern int GetIfEntry(ref Win32_MIB_IFROW row);

		public static NetworkInterface[] ImplGetAllNetworkInterfaces()
		{
			Win32_IP_ADAPTER_ADDRESSES[] adaptersAddresses = Win32NetworkInterface2.GetAdaptersAddresses();
			NetworkInterface[] array = new NetworkInterface[adaptersAddresses.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Win32NetworkInterface2(adaptersAddresses[i]);
			}
			return array;
		}

		public static Win32_IP_ADAPTER_INFO GetAdapterInfoByIndex(int index)
		{
			foreach (Win32_IP_ADAPTER_INFO win32_IP_ADAPTER_INFO in Win32NetworkInterface2.GetAdaptersInfo())
			{
				if ((ulong)win32_IP_ADAPTER_INFO.Index == (ulong)((long)index))
				{
					return win32_IP_ADAPTER_INFO;
				}
			}
			return null;
		}

		private unsafe static Win32_IP_ADAPTER_INFO[] GetAdaptersInfo()
		{
			byte[] array = null;
			int num = 0;
			Win32NetworkInterface2.GetAdaptersInfo(array, ref num);
			array = new byte[num];
			int adaptersInfo = Win32NetworkInterface2.GetAdaptersInfo(array, ref num);
			if (adaptersInfo != 0)
			{
				throw new NetworkInformationException(adaptersInfo);
			}
			List<Win32_IP_ADAPTER_INFO> list = new List<Win32_IP_ADAPTER_INFO>();
			fixed (byte* value = ref (array != null && array.Length != 0) ? ref array[0] : ref *null)
			{
				IntPtr intPtr = (IntPtr)((void*)value);
				while (intPtr != IntPtr.Zero)
				{
					Win32_IP_ADAPTER_INFO win32_IP_ADAPTER_INFO = new Win32_IP_ADAPTER_INFO();
					Marshal.PtrToStructure(intPtr, win32_IP_ADAPTER_INFO);
					list.Add(win32_IP_ADAPTER_INFO);
					intPtr = win32_IP_ADAPTER_INFO.Next;
				}
			}
			return list.ToArray();
		}

		private unsafe static Win32_IP_ADAPTER_ADDRESSES[] GetAdaptersAddresses()
		{
			byte[] array = null;
			int num = 0;
			Win32NetworkInterface2.GetAdaptersAddresses(0u, 0u, IntPtr.Zero, array, ref num);
			array = new byte[num];
			int adaptersAddresses = Win32NetworkInterface2.GetAdaptersAddresses(0u, 0u, IntPtr.Zero, array, ref num);
			if (adaptersAddresses != 0)
			{
				throw new NetworkInformationException(adaptersAddresses);
			}
			List<Win32_IP_ADAPTER_ADDRESSES> list = new List<Win32_IP_ADAPTER_ADDRESSES>();
			fixed (byte* value = ref (array != null && array.Length != 0) ? ref array[0] : ref *null)
			{
				IntPtr intPtr = (IntPtr)((void*)value);
				while (intPtr != IntPtr.Zero)
				{
					Win32_IP_ADAPTER_ADDRESSES win32_IP_ADAPTER_ADDRESSES = new Win32_IP_ADAPTER_ADDRESSES();
					Marshal.PtrToStructure(intPtr, win32_IP_ADAPTER_ADDRESSES);
					list.Add(win32_IP_ADAPTER_ADDRESSES);
					intPtr = win32_IP_ADAPTER_ADDRESSES.Next;
				}
			}
			return list.ToArray();
		}

		public override IPInterfaceProperties GetIPProperties()
		{
			return this.ip_if_props;
		}

		public override IPv4InterfaceStatistics GetIPv4Statistics()
		{
			return this.ip4stats;
		}

		public override PhysicalAddress GetPhysicalAddress()
		{
			byte[] array = new byte[this.addr.PhysicalAddressLength];
			Array.Copy(this.addr.PhysicalAddress, 0, array, 0, array.Length);
			return new PhysicalAddress(array);
		}

		public override bool Supports(NetworkInterfaceComponent networkInterfaceComponent)
		{
			if (networkInterfaceComponent != NetworkInterfaceComponent.IPv4)
			{
				return networkInterfaceComponent == NetworkInterfaceComponent.IPv6 && this.mib6.Index >= 0;
			}
			return this.mib4.Index >= 0;
		}

		public override string Description
		{
			get
			{
				return this.addr.Description;
			}
		}

		public override string Id
		{
			get
			{
				return this.addr.AdapterName;
			}
		}

		public override bool IsReceiveOnly
		{
			get
			{
				return this.addr.IsReceiveOnly;
			}
		}

		public override string Name
		{
			get
			{
				return this.addr.FriendlyName;
			}
		}

		public override NetworkInterfaceType NetworkInterfaceType
		{
			get
			{
				return this.addr.IfType;
			}
		}

		public override OperationalStatus OperationalStatus
		{
			get
			{
				return this.addr.OperStatus;
			}
		}

		public override long Speed
		{
			get
			{
				return (long)((ulong)((this.mib6.Index < 0) ? this.mib4.Speed : this.mib6.Speed));
			}
		}

		public override bool SupportsMulticast
		{
			get
			{
				return !this.addr.NoMulticast;
			}
		}
	}
}
