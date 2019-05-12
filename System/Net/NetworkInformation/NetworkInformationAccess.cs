using System;

namespace System.Net.NetworkInformation
{
	/// <summary>Specifies permission to access information about network interfaces and traffic statistics.</summary>
	[Flags]
	public enum NetworkInformationAccess
	{
		/// <summary>No access to network information.</summary>
		None = 0,
		/// <summary>Read access to network information.</summary>
		Read = 1,
		/// <summary>Ping access to network information.</summary>
		Ping = 4
	}
}
