using System;
using System.Collections.Generic;

namespace System.Net.NetworkInformation
{
	internal class MacOsIPInterfaceProperties : UnixIPInterfaceProperties
	{
		public MacOsIPInterfaceProperties(MacOsNetworkInterface iface, List<IPAddress> addresses) : base(iface, addresses)
		{
		}

		public override IPv4InterfaceProperties GetIPv4Properties()
		{
			if (this.ipv4iface_properties == null)
			{
				this.ipv4iface_properties = new MacOsIPv4InterfaceProperties(this.iface as MacOsNetworkInterface);
			}
			return this.ipv4iface_properties;
		}
	}
}
