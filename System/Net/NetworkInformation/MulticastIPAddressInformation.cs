using System;

namespace System.Net.NetworkInformation
{
	/// <summary>Provides information about a network interface's multicast address.</summary>
	public abstract class MulticastIPAddressInformation : IPAddressInformation
	{
		/// <summary>Gets the number of seconds remaining during which this address is the preferred address.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the number of seconds left for this address to remain preferred.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">This property is not valid on computers running operating systems earlier than Windows XP. </exception>
		public abstract long AddressPreferredLifetime { get; }

		/// <summary>Gets the number of seconds remaining during which this address is valid.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that specifies the number of seconds left for this address to remain assigned.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">This property is not valid on computers running operating systems earlier than Windows XP. </exception>
		public abstract long AddressValidLifetime { get; }

		/// <summary>Specifies the amount of time remaining on the Dynamic Host Configuration Protocol (DHCP) lease for this IP address.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that contains the number of seconds remaining before the computer must release the <see cref="T:System.Net.IPAddress" /> instance.</returns>
		public abstract long DhcpLeaseLifetime { get; }

		/// <summary>Gets a value that indicates the state of the duplicate address detection algorithm.</summary>
		/// <returns>One of the <see cref="T:System.Net.NetworkInformation.DuplicateAddressDetectionState" /> values that indicates the progress of the algorithm in determining the uniqueness of this IP address.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">This property is not valid on computers running operating systems earlier than Windows XP. </exception>
		public abstract DuplicateAddressDetectionState DuplicateAddressDetectionState { get; }

		/// <summary>Gets a value that identifies the source of a Multicast Internet Protocol (IP) address prefix.</summary>
		/// <returns>One of the <see cref="T:System.Net.NetworkInformation.PrefixOrigin" /> values that identifies how the prefix information was obtained.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">This property is not valid on computers running operating systems earlier than Windows XP. </exception>
		public abstract PrefixOrigin PrefixOrigin { get; }

		/// <summary>Gets a value that identifies the source of a Multicast Internet Protocol (IP) address suffix.</summary>
		/// <returns>One of the <see cref="T:System.Net.NetworkInformation.SuffixOrigin" /> values that identifies how the suffix information was obtained.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">This property is not valid on computers running operating systems earlier than Windows XP. </exception>
		public abstract SuffixOrigin SuffixOrigin { get; }
	}
}
