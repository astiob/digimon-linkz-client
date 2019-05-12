using System;

namespace System
{
	/// <summary>Defines host name types for the <see cref="M:System.Uri.CheckHostName(System.String)" /> method.</summary>
	/// <filterpriority>2</filterpriority>
	public enum UriHostNameType
	{
		/// <summary>The type of the host name is not supplied.</summary>
		Unknown,
		/// <summary>The host is set, but the type cannot be determined.</summary>
		Basic,
		/// <summary>The host name is a domain name system (DNS) style host name.</summary>
		Dns,
		/// <summary>The host name is an Internet Protocol (IP) version 4 host address.</summary>
		IPv4,
		/// <summary>The host name is an Internet Protocol (IP) version 6 host address.</summary>
		IPv6
	}
}
