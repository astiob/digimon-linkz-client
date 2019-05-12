using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	internal sealed class Win32IPv4InterfaceProperties : IPv4InterfaceProperties
	{
		private Win32_IP_ADAPTER_INFO ainfo;

		private Win32_IP_PER_ADAPTER_INFO painfo;

		private Win32_MIB_IFROW mib;

		public Win32IPv4InterfaceProperties(Win32_IP_ADAPTER_INFO ainfo, Win32_MIB_IFROW mib)
		{
			this.ainfo = ainfo;
			this.mib = mib;
			int num = 0;
			Win32IPv4InterfaceProperties.GetPerAdapterInfo(mib.Index, null, ref num);
			this.painfo = new Win32_IP_PER_ADAPTER_INFO();
			int perAdapterInfo = Win32IPv4InterfaceProperties.GetPerAdapterInfo(mib.Index, this.painfo, ref num);
			if (perAdapterInfo != 0)
			{
				throw new NetworkInformationException(perAdapterInfo);
			}
		}

		[DllImport("iphlpapi.dll")]
		private static extern int GetPerAdapterInfo(int IfIndex, Win32_IP_PER_ADAPTER_INFO pPerAdapterInfo, ref int pOutBufLen);

		public override int Index
		{
			get
			{
				return this.mib.Index;
			}
		}

		public override bool IsAutomaticPrivateAddressingActive
		{
			get
			{
				return this.painfo.AutoconfigActive != 0u;
			}
		}

		public override bool IsAutomaticPrivateAddressingEnabled
		{
			get
			{
				return this.painfo.AutoconfigEnabled != 0u;
			}
		}

		public override bool IsDhcpEnabled
		{
			get
			{
				return this.ainfo.DhcpEnabled != 0u;
			}
		}

		public override bool IsForwardingEnabled
		{
			get
			{
				return Win32_FIXED_INFO.Instance.EnableRouting != 0u;
			}
		}

		public override int Mtu
		{
			get
			{
				return this.mib.Mtu;
			}
		}

		public override bool UsesWins
		{
			get
			{
				return this.ainfo.HaveWins;
			}
		}
	}
}
