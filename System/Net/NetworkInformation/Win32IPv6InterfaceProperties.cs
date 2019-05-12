using System;

namespace System.Net.NetworkInformation
{
	internal class Win32IPv6InterfaceProperties : IPv6InterfaceProperties
	{
		private Win32_MIB_IFROW mib;

		public Win32IPv6InterfaceProperties(Win32_MIB_IFROW mib)
		{
			this.mib = mib;
		}

		public override int Index
		{
			get
			{
				return this.mib.Index;
			}
		}

		public override int Mtu
		{
			get
			{
				return this.mib.Mtu;
			}
		}
	}
}
