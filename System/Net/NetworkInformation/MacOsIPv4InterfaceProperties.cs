using System;

namespace System.Net.NetworkInformation
{
	internal sealed class MacOsIPv4InterfaceProperties : UnixIPv4InterfaceProperties
	{
		public MacOsIPv4InterfaceProperties(MacOsNetworkInterface iface) : base(iface)
		{
		}

		public override bool IsForwardingEnabled
		{
			get
			{
				return false;
			}
		}

		public override int Mtu
		{
			get
			{
				return 0;
			}
		}
	}
}
