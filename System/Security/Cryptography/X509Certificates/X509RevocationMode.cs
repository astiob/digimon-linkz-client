using System;

namespace System.Security.Cryptography.X509Certificates
{
	/// <summary>Specifies the mode used to check for X509 certificate revocation.</summary>
	public enum X509RevocationMode
	{
		/// <summary>No revocation check is performed on the certificate.</summary>
		NoCheck,
		/// <summary>A revocation check is made using an online certificate revocation list (CRL).</summary>
		Online,
		/// <summary>A revocation check is made using a cached certificate revocation list (CRL).</summary>
		Offline
	}
}
