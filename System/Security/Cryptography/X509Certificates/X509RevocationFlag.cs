using System;

namespace System.Security.Cryptography.X509Certificates
{
	/// <summary>Specifies which X509 certificates in the chain should be checked for revocation.</summary>
	public enum X509RevocationFlag
	{
		/// <summary>Only the end certificate is checked for revocation.</summary>
		EndCertificateOnly,
		/// <summary>The entire chain of certificates is checked for revocation.</summary>
		EntireChain,
		/// <summary>The entire chain, except the root certificate, is checked for revocation.</summary>
		ExcludeRoot
	}
}
