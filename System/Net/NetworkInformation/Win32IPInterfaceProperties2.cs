using System;

namespace System.Net.NetworkInformation
{
	internal class Win32IPInterfaceProperties2 : IPInterfaceProperties
	{
		private readonly Win32_IP_ADAPTER_ADDRESSES addr;

		private readonly Win32_MIB_IFROW mib4;

		private readonly Win32_MIB_IFROW mib6;

		public Win32IPInterfaceProperties2(Win32_IP_ADAPTER_ADDRESSES addr, Win32_MIB_IFROW mib4, Win32_MIB_IFROW mib6)
		{
			this.addr = addr;
			this.mib4 = mib4;
			this.mib6 = mib6;
		}

		public override IPv4InterfaceProperties GetIPv4Properties()
		{
			Win32_IP_ADAPTER_INFO adapterInfoByIndex = Win32NetworkInterface2.GetAdapterInfoByIndex(this.mib4.Index);
			return (adapterInfoByIndex == null) ? null : new Win32IPv4InterfaceProperties(adapterInfoByIndex, this.mib4);
		}

		public override IPv6InterfaceProperties GetIPv6Properties()
		{
			Win32_IP_ADAPTER_INFO adapterInfoByIndex = Win32NetworkInterface2.GetAdapterInfoByIndex(this.mib6.Index);
			return (adapterInfoByIndex == null) ? null : new Win32IPv6InterfaceProperties(this.mib6);
		}

		public override IPAddressInformationCollection AnycastAddresses
		{
			get
			{
				return IPAddressInformationImplCollection.Win32FromAnycast(this.addr.FirstAnycastAddress);
			}
		}

		public override IPAddressCollection DhcpServerAddresses
		{
			get
			{
				Win32_IP_ADAPTER_INFO adapterInfoByIndex = Win32NetworkInterface2.GetAdapterInfoByIndex(this.mib4.Index);
				return (adapterInfoByIndex == null) ? Win32IPAddressCollection.Empty : new Win32IPAddressCollection(new Win32_IP_ADDR_STRING[]
				{
					adapterInfoByIndex.DhcpServer
				});
			}
		}

		public override IPAddressCollection DnsAddresses
		{
			get
			{
				return Win32IPAddressCollection.FromDnsServer(this.addr.FirstDnsServerAddress);
			}
		}

		public override string DnsSuffix
		{
			get
			{
				return this.addr.DnsSuffix;
			}
		}

		public override GatewayIPAddressInformationCollection GatewayAddresses
		{
			get
			{
				Win32_IP_ADAPTER_INFO adapterInfoByIndex = Win32NetworkInterface2.GetAdapterInfoByIndex(this.mib4.Index);
				return (adapterInfoByIndex == null) ? Win32GatewayIPAddressInformationCollection.Empty : new Win32GatewayIPAddressInformationCollection(new Win32_IP_ADDR_STRING[]
				{
					adapterInfoByIndex.GatewayList
				});
			}
		}

		public override bool IsDnsEnabled
		{
			get
			{
				return Win32_FIXED_INFO.Instance.EnableDns != 0u;
			}
		}

		public override bool IsDynamicDnsEnabled
		{
			get
			{
				return this.addr.DdnsEnabled;
			}
		}

		public override MulticastIPAddressInformationCollection MulticastAddresses
		{
			get
			{
				return MulticastIPAddressInformationImplCollection.Win32FromMulticast(this.addr.FirstMulticastAddress);
			}
		}

		public override UnicastIPAddressInformationCollection UnicastAddresses
		{
			get
			{
				Win32_IP_ADAPTER_INFO adapterInfoByIndex = Win32NetworkInterface2.GetAdapterInfoByIndex(this.mib4.Index);
				return (adapterInfoByIndex == null) ? UnicastIPAddressInformationImplCollection.Empty : UnicastIPAddressInformationImplCollection.Win32FromUnicast((int)adapterInfoByIndex.Index, this.addr.FirstUnicastAddress);
			}
		}

		public override IPAddressCollection WinsServersAddresses
		{
			get
			{
				Win32_IP_ADAPTER_INFO adapterInfoByIndex = Win32NetworkInterface2.GetAdapterInfoByIndex(this.mib4.Index);
				return (adapterInfoByIndex == null) ? Win32IPAddressCollection.Empty : new Win32IPAddressCollection(new Win32_IP_ADDR_STRING[]
				{
					adapterInfoByIndex.PrimaryWinsServer,
					adapterInfoByIndex.SecondaryWinsServer
				});
			}
		}
	}
}
