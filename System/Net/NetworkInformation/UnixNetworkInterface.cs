using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	internal abstract class UnixNetworkInterface : NetworkInterface
	{
		protected IPv4InterfaceStatistics ipv4stats;

		protected IPInterfaceProperties ipproperties;

		private string name;

		private int index;

		protected List<IPAddress> addresses;

		private byte[] macAddress;

		private NetworkInterfaceType type;

		internal UnixNetworkInterface(string name)
		{
			this.name = name;
			this.addresses = new List<IPAddress>();
		}

		[DllImport("libc")]
		private static extern int if_nametoindex(string ifname);

		public static int IfNameToIndex(string ifname)
		{
			return UnixNetworkInterface.if_nametoindex(ifname);
		}

		internal void AddAddress(IPAddress address)
		{
			this.addresses.Add(address);
		}

		internal void SetLinkLayerInfo(int index, byte[] macAddress, NetworkInterfaceType type)
		{
			this.index = index;
			this.macAddress = macAddress;
			this.type = type;
		}

		public override PhysicalAddress GetPhysicalAddress()
		{
			if (this.macAddress != null)
			{
				return new PhysicalAddress(this.macAddress);
			}
			return PhysicalAddress.None;
		}

		public override bool Supports(NetworkInterfaceComponent networkInterfaceComponent)
		{
			bool flag = networkInterfaceComponent == NetworkInterfaceComponent.IPv4;
			bool flag2 = !flag && networkInterfaceComponent == NetworkInterfaceComponent.IPv6;
			foreach (IPAddress ipaddress in this.addresses)
			{
				if (flag && ipaddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
				{
					return true;
				}
				if (flag2 && ipaddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
				{
					return true;
				}
			}
			return false;
		}

		public override string Description
		{
			get
			{
				return this.name;
			}
		}

		public override string Id
		{
			get
			{
				return this.name;
			}
		}

		public override bool IsReceiveOnly
		{
			get
			{
				return false;
			}
		}

		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		public override NetworkInterfaceType NetworkInterfaceType
		{
			get
			{
				return this.type;
			}
		}

		[MonoTODO("Parse dmesg?")]
		public override long Speed
		{
			get
			{
				return 1000000L;
			}
		}
	}
}
