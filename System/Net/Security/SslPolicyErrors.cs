using System;

namespace System.Net.Security
{
	/// <summary>Enumerates Secure Socket Layer (SSL) policy errors.</summary>
	[Flags]
	public enum SslPolicyErrors
	{
		/// <summary>No SSL policy errors.</summary>
		None = 0,
		/// <summary>Certificate not available.</summary>
		RemoteCertificateNotAvailable = 1,
		/// <summary>Certificate name mismatch.</summary>
		RemoteCertificateNameMismatch = 2,
		/// <summary>
		///   <see cref="P:System.Security.Cryptography.X509Certificates.X509Chain.ChainStatus" /> has returned a non empty array.</summary>
		RemoteCertificateChainErrors = 4
	}
}
