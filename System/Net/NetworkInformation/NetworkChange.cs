using System;

namespace System.Net.NetworkInformation
{
	/// <summary>Allows applications to receive notification when the Internet Protocol (IP) address of a network interface, also called a network card or adapter, changes.</summary>
	public sealed class NetworkChange
	{
		private NetworkChange()
		{
		}

		/// <summary>Occurs when the IP address of a network interface changes.</summary>
		public static event NetworkAddressChangedEventHandler NetworkAddressChanged;

		/// <summary>Occurs when the availability of the network changes.</summary>
		public static event NetworkAvailabilityChangedEventHandler NetworkAvailabilityChanged;
	}
}
