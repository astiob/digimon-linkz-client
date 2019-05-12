using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	internal class Win32UnicastIPAddressInformation : UnicastIPAddressInformation
	{
		private int if_index;

		private Win32_IP_ADAPTER_UNICAST_ADDRESS info;

		public Win32UnicastIPAddressInformation(int ifIndex, Win32_IP_ADAPTER_UNICAST_ADDRESS info)
		{
			this.if_index = ifIndex;
			this.info = info;
		}

		public override IPAddress Address
		{
			get
			{
				return this.info.Address.GetIPAddress();
			}
		}

		public override bool IsDnsEligible
		{
			get
			{
				return this.info.LengthFlags.IsDnsEligible;
			}
		}

		public override bool IsTransient
		{
			get
			{
				return this.info.LengthFlags.IsTransient;
			}
		}

		public override long AddressPreferredLifetime
		{
			get
			{
				return (long)((ulong)this.info.PreferredLifetime);
			}
		}

		public override long AddressValidLifetime
		{
			get
			{
				return (long)((ulong)this.info.ValidLifetime);
			}
		}

		public override long DhcpLeaseLifetime
		{
			get
			{
				return (long)((ulong)this.info.LeaseLifetime);
			}
		}

		public override DuplicateAddressDetectionState DuplicateAddressDetectionState
		{
			get
			{
				return this.info.DadState;
			}
		}

		public override IPAddress IPv4Mask
		{
			get
			{
				Win32_IP_ADAPTER_INFO adapterInfoByIndex = Win32NetworkInterface2.GetAdapterInfoByIndex(this.if_index);
				if (adapterInfoByIndex == null)
				{
					throw new Exception("huh? " + this.if_index);
				}
				if (this.Address == null)
				{
					return null;
				}
				string b = this.Address.ToString();
				Win32_IP_ADDR_STRING win32_IP_ADDR_STRING = adapterInfoByIndex.IpAddressList;
				while (!(win32_IP_ADDR_STRING.IpAddress == b))
				{
					if (win32_IP_ADDR_STRING.Next == IntPtr.Zero)
					{
						return null;
					}
					win32_IP_ADDR_STRING = (Win32_IP_ADDR_STRING)Marshal.PtrToStructure(win32_IP_ADDR_STRING.Next, typeof(Win32_IP_ADDR_STRING));
				}
				return IPAddress.Parse(win32_IP_ADDR_STRING.IpMask);
			}
		}

		public override PrefixOrigin PrefixOrigin
		{
			get
			{
				return this.info.PrefixOrigin;
			}
		}

		public override SuffixOrigin SuffixOrigin
		{
			get
			{
				return this.info.SuffixOrigin;
			}
		}
	}
}
