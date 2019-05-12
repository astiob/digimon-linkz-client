using System;

namespace System.Net.NetworkInformation
{
	internal abstract class UnixIPv4InterfaceProperties : IPv4InterfaceProperties
	{
		protected UnixNetworkInterface iface;

		public UnixIPv4InterfaceProperties(UnixNetworkInterface iface)
		{
			this.iface = iface;
		}

		public override int Index
		{
			get
			{
				return UnixNetworkInterface.IfNameToIndex(this.iface.Name);
			}
		}

		public override bool IsAutomaticPrivateAddressingActive
		{
			get
			{
				return false;
			}
		}

		public override bool IsAutomaticPrivateAddressingEnabled
		{
			get
			{
				return false;
			}
		}

		public override bool IsDhcpEnabled
		{
			get
			{
				return false;
			}
		}

		public override bool UsesWins
		{
			get
			{
				return false;
			}
		}
	}
}
