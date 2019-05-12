using System;

namespace System.Net.Security
{
	/// <summary>Indicates the security services requested for an authenticated stream.</summary>
	public enum ProtectionLevel
	{
		/// <summary>Authentication only.</summary>
		None,
		/// <summary>Sign data to help ensure the integrity of transmitted data.</summary>
		Sign,
		/// <summary>Encrypt and sign data to help ensure the confidentiality and integrity of transmitted data.</summary>
		EncryptAndSign
	}
}
