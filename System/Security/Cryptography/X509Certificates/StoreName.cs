using System;

namespace System.Security.Cryptography.X509Certificates
{
	/// <summary>Specifies the name of the X.509 certificate store to open.</summary>
	public enum StoreName
	{
		/// <summary>The X.509 certificate store for other users.</summary>
		AddressBook = 1,
		/// <summary>The X.509 certificate store for third-party certificate authorities (CAs).</summary>
		AuthRoot,
		/// <summary>The X.509 certificate store for intermediate certificate authorities (CAs). </summary>
		CertificateAuthority,
		/// <summary>The X.509 certificate store for revoked certificates.</summary>
		Disallowed,
		/// <summary>The X.509 certificate store for personal certificates.</summary>
		My,
		/// <summary>The X.509 certificate store for trusted root certificate authorities (CAs).</summary>
		Root,
		/// <summary>The X.509 certificate store for directly trusted people and resources.</summary>
		TrustedPeople,
		/// <summary>The X.509 certificate store for directly trusted publishers.</summary>
		TrustedPublisher
	}
}
