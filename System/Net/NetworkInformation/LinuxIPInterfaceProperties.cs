using System;
using System.Collections.Generic;

namespace System.Net.NetworkInformation
{
	internal class LinuxIPInterfaceProperties : UnixIPInterfaceProperties
	{
		public LinuxIPInterfaceProperties(LinuxNetworkInterface iface, List<IPAddress> addresses) : base(iface, addresses)
		{
		}

		public override IPv4InterfaceProperties GetIPv4Properties()
		{
			if (this.ipv4iface_properties == null)
			{
				this.ipv4iface_properties = new LinuxIPv4InterfaceProperties(this.iface as LinuxNetworkInterface);
			}
			return this.ipv4iface_properties;
		}
	}
}
