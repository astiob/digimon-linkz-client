using System;

namespace System.Security.Cryptography.X509Certificates
{
	/// <summary>Specifies the location of the X.509 certificate store.</summary>
	public enum StoreLocation
	{
		/// <summary>The X.509 certificate store used by the current user.</summary>
		CurrentUser = 1,
		/// <summary>The X.509 certificate store assigned to the local machine.</summary>
		LocalMachine
	}
}
