using System;

namespace System.Net.NetworkInformation
{
	/// <summary>Specifies how an IP address network prefix was located.</summary>
	public enum PrefixOrigin
	{
		/// <summary>The prefix was located using an unspecified source.</summary>
		Other,
		/// <summary>The prefix was manually configured.</summary>
		Manual,
		/// <summary>The prefix is a well-known prefix. Well-known prefixes are specified in standard-track Request for Comments (RFC) documents and assigned by the Internet Assigned Numbers Authority (Iana) or an address registry. Such prefixes are reserved for special purposes.</summary>
		WellKnown,
		/// <summary>The prefix was supplied by a Dynamic Host Configuration Protocol (DHCP) server.</summary>
		Dhcp,
		/// <summary>The prefix was supplied by a router advertisement.</summary>
		RouterAdvertisement
	}
}
