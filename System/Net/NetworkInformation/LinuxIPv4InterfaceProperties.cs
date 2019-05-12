using System;
using System.IO;

namespace System.Net.NetworkInformation
{
	internal sealed class LinuxIPv4InterfaceProperties : UnixIPv4InterfaceProperties
	{
		public LinuxIPv4InterfaceProperties(LinuxNetworkInterface iface) : base(iface)
		{
		}

		public override bool IsForwardingEnabled
		{
			get
			{
				string path = "/proc/sys/net/ipv4/conf/" + this.iface.Name + "/forwarding";
				if (File.Exists(path))
				{
					string a = NetworkInterface.ReadLine(path);
					return a != "0";
				}
				return false;
			}
		}

		public override int Mtu
		{
			get
			{
				string path = (this.iface as LinuxNetworkInterface).IfacePath + "mtu";
				int result = 0;
				if (File.Exists(path))
				{
					string s = NetworkInterface.ReadLine(path);
					try
					{
						result = int.Parse(s);
					}
					catch
					{
					}
				}
				return result;
			}
		}
	}
}
