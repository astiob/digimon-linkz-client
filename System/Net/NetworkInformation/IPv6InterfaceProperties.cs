using System;

namespace System.Net.NetworkInformation
{
	/// <summary>Provides information about network interfaces that support Internet Protocol version 6 (IPv6).</summary>
	public abstract class IPv6InterfaceProperties
	{
		/// <summary>Gets the index of the network interface associated with the Internet Protocol version 6 (IPv6) address.</summary>
		/// <returns>An <see cref="T:System.Int32" /> value that contains the index of the IPv6 interface.</returns>
		public abstract int Index { get; }

		/// <summary>Gets the maximum transmission unit (MTU) for this network interface.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the MTU.</returns>
		public abstract int Mtu { get; }
	}
}
