using System;

namespace System.Net.NetworkInformation
{
	internal class MulticastIPAddressInformationImpl : MulticastIPAddressInformation
	{
		private IPAddress address;

		private bool is_dns_eligible;

		private bool is_transient;

		public MulticastIPAddressInformationImpl(IPAddress address, bool isDnsEligible, bool isTransient)
		{
			this.address = address;
			this.is_dns_eligible = isDnsEligible;
			this.is_transient = isTransient;
		}

		public override IPAddress Address
		{
			get
			{
				return this.address;
			}
		}

		public override bool IsDnsEligible
		{
			get
			{
				return this.is_dns_eligible;
			}
		}

		public override bool IsTransient
		{
			get
			{
				return this.is_transient;
			}
		}

		public override long AddressPreferredLifetime
		{
			get
			{
				return 0L;
			}
		}

		public override long AddressValidLifetime
		{
			get
			{
				return 0L;
			}
		}

		public override long DhcpLeaseLifetime
		{
			get
			{
				return 0L;
			}
		}

		public override DuplicateAddressDetectionState DuplicateAddressDetectionState
		{
			get
			{
				return DuplicateAddressDetectionState.Invalid;
			}
		}

		public override PrefixOrigin PrefixOrigin
		{
			get
			{
				return PrefixOrigin.Other;
			}
		}

		public override SuffixOrigin SuffixOrigin
		{
			get
			{
				return SuffixOrigin.Other;
			}
		}
	}
}
