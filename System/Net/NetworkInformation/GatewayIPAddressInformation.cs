using System;

namespace System.Net.NetworkInformation
{
	/// <summary>Represents the IP address of the network gateway. This class cannot be instantiated.</summary>
	public abstract class GatewayIPAddressInformation
	{
		/// <summary>Get the IP address of the gateway.</summary>
		/// <returns>An <see cref="T:System.Net.IPAddress" /> object that contains the IP address of the gateway.</returns>
		public abstract IPAddress Address { get; }
	}
}
