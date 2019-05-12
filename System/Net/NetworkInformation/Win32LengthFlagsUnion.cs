using System;

namespace System.Net.NetworkInformation
{
	internal struct Win32LengthFlagsUnion
	{
		private const int IP_ADAPTER_ADDRESS_DNS_ELIGIBLE = 1;

		private const int IP_ADAPTER_ADDRESS_TRANSIENT = 2;

		public uint Length;

		public uint Flags;

		public bool IsDnsEligible
		{
			get
			{
				return (this.Flags & 1u) != 0u;
			}
		}

		public bool IsTransient
		{
			get
			{
				return (this.Flags & 2u) != 0u;
			}
		}
	}
}
