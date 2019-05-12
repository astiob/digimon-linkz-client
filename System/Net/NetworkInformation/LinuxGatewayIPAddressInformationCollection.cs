using System;

namespace System.Net.NetworkInformation
{
	internal class LinuxGatewayIPAddressInformationCollection : GatewayIPAddressInformationCollection
	{
		public static readonly LinuxGatewayIPAddressInformationCollection Empty = new LinuxGatewayIPAddressInformationCollection(true);

		private bool is_readonly;

		private LinuxGatewayIPAddressInformationCollection(bool isReadOnly)
		{
			this.is_readonly = isReadOnly;
		}

		public LinuxGatewayIPAddressInformationCollection(IPAddressCollection col)
		{
			foreach (IPAddress address in col)
			{
				this.Add(new GatewayIPAddressInformationImpl(address));
			}
			this.is_readonly = true;
		}

		public override bool IsReadOnly
		{
			get
			{
				return this.is_readonly;
			}
		}
	}
}
