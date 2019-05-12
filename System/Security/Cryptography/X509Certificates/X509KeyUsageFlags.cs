using System;

namespace System.Security.Cryptography.X509Certificates
{
	/// <summary>Defines how the certificate key can be used. If this value is not defined, the key can be used for any purpose.</summary>
	[Flags]
	public enum X509KeyUsageFlags
	{
		/// <summary>No key usage parameters.</summary>
		None = 0,
		/// <summary>The key can be used for encryption only.</summary>
		EncipherOnly = 1,
		/// <summary>The key can be used to sign a certificate revocation list (CRL).</summary>
		CrlSign = 2,
		/// <summary>The key can be used to sign certificates.</summary>
		KeyCertSign = 4,
		/// <summary>The key can be used to determine key agreement, such as a key created using the Diffie-Hellman key agreement algorithm.</summary>
		KeyAgreement = 8,
		/// <summary>The key can be used for data encryption.</summary>
		DataEncipherment = 16,
		/// <summary>The key can be used for key encryption.</summary>
		KeyEncipherment = 32,
		/// <summary>The key can be used for authentication.</summary>
		NonRepudiation = 64,
		/// <summary>The key can be used as a digital signature.</summary>
		DigitalSignature = 128,
		/// <summary>The key can be used for decryption only.</summary>
		DecipherOnly = 32768
	}
}
