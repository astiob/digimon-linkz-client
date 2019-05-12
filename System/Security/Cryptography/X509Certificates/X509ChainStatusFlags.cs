using System;

namespace System.Security.Cryptography.X509Certificates
{
	/// <summary>Defines the status of an X509 chain.</summary>
	[Flags]
	public enum X509ChainStatusFlags
	{
		/// <summary>Specifies that the X509 chain has no errors.</summary>
		NoError = 0,
		/// <summary>Specifies that the X509 chain is not valid due to an invalid time value, such as a value that indicates an expired certificate.</summary>
		NotTimeValid = 1,
		/// <summary>Deprecated, this flag has no effect. </summary>
		NotTimeNested = 2,
		/// <summary>Specifies that the X509 chain is invalid due to a revoked certificate.</summary>
		Revoked = 4,
		/// <summary>Specifies that the X509 chain is invalid due to an invalid certificate signature.</summary>
		NotSignatureValid = 8,
		/// <summary>Specifies that the key usage is not valid.</summary>
		NotValidForUsage = 16,
		/// <summary>Specifies that the X509 chain is invalid due to an untrusted root certificate.</summary>
		UntrustedRoot = 32,
		/// <summary>Specifies that it is not possible to determine whether the certificate has been revoked. This can be due to the certificate revocation list (CRL) being offline or unavailable.</summary>
		RevocationStatusUnknown = 64,
		/// <summary>Specifies that the X509 chain could not be built.</summary>
		Cyclic = 128,
		/// <summary>Specifies that the X509 chain is invalid due to an invalid extension.</summary>
		InvalidExtension = 256,
		/// <summary>Specifies that the X509 chain is invalid due to invalid policy constraints.</summary>
		InvalidPolicyConstraints = 512,
		/// <summary>Specifies that the X509 chain is invalid due to invalid basic constraints.</summary>
		InvalidBasicConstraints = 1024,
		/// <summary>Specifies that the X509 chain is invalid due to invalid name constraints.</summary>
		InvalidNameConstraints = 2048,
		/// <summary>Specifies that the certificate does not have a supported name constant or has a name constant that is unsupported.</summary>
		HasNotSupportedNameConstraint = 4096,
		/// <summary>Specifies that the certificate has an undefined name constant.</summary>
		HasNotDefinedNameConstraint = 8192,
		/// <summary>Specifies that the certificate has an impermissible name constraint.</summary>
		HasNotPermittedNameConstraint = 16384,
		/// <summary>Specifies that the X509 chain is invalid because a certificate has excluded a name constraint.</summary>
		HasExcludedNameConstraint = 32768,
		/// <summary>Specifies that the X509 chain could not be built up to the root certificate.</summary>
		PartialChain = 65536,
		/// <summary>Specifies that the certificate trust list (CTL) is not valid because of an invalid time value, such as one that indicates that the CTL has expired.</summary>
		CtlNotTimeValid = 131072,
		/// <summary>Specifies that the certificate trust list (CTL) contains an invalid signature.</summary>
		CtlNotSignatureValid = 262144,
		/// <summary>Specifies that the certificate trust list (CTL) is not valid for this use.</summary>
		CtlNotValidForUsage = 524288,
		/// <summary>Specifies that the online certificate revocation list (CRL) the X509 chain relies on is currently offline.</summary>
		OfflineRevocation = 16777216,
		/// <summary>Specifies that there is no certificate policy extension in the certificate. This error would occur if a group policy has specified that all certificates must have a certificate policy.</summary>
		NoIssuanceChainPolicy = 33554432
	}
}
