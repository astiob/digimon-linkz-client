using System;

namespace System.Security.Authentication
{
	/// <summary>Specifies the algorithm used to create keys shared by the client and server.</summary>
	public enum ExchangeAlgorithmType
	{
		/// <summary>No key exchange algorithm is used.</summary>
		None,
		/// <summary>The Diffie Hellman ephemeral key exchange algorithm.</summary>
		DiffieHellman = 43522,
		/// <summary>The RSA public-key exchange algorithm.</summary>
		RsaKeyX = 41984,
		/// <summary>The RSA public-key signature algorithm.</summary>
		RsaSign = 9216
	}
}
