using System;

namespace System.Net.NetworkInformation
{
	internal class LinuxUnicastIPAddressInformation : UnicastIPAddressInformation
	{
		private IPAddress address;

		public LinuxUnicastIPAddressInformation(IPAddress address)
		{
			this.address = address;
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
				byte[] addressBytes = this.address.GetAddressBytes();
				return addressBytes[0] != 169 || addressBytes[1] != 254;
			}
		}

		[MonoTODO("Always returns false")]
		public override bool IsTransient
		{
			get
			{
				return false;
			}
		}

		public override long AddressPreferredLifetime
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override long AddressValidLifetime
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override long DhcpLeaseLifetime
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override DuplicateAddressDetectionState DuplicateAddressDetectionState
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override IPAddress IPv4Mask
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override PrefixOrigin PrefixOrigin
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override SuffixOrigin SuffixOrigin
		{
			get
			{
				throw new NotImplementedException();
			}
		}
	}
}
