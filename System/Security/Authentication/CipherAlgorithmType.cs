using System;

namespace System.Security.Authentication
{
	/// <summary>Defines the possible cipher algorithms for the <see cref="T:System.Net.Security.SslStream" /> class.</summary>
	public enum CipherAlgorithmType
	{
		/// <summary>No encryption algorithm is used.</summary>
		None,
		/// <summary>The Advanced Encryption Standard (AES) algorithm.</summary>
		Aes = 26129,
		/// <summary>The Advanced Encryption Standard (AES) algorithm with a 128 bit key.</summary>
		Aes128 = 26126,
		/// <summary>The Advanced Encryption Standard (AES) algorithm with a 192 bit key.</summary>
		Aes192,
		/// <summary>The Advanced Encryption Standard (AES) algorithm with a 256 bit key.</summary>
		Aes256,
		/// <summary>The Data Encryption Standard (DES) algorithm.</summary>
		Des = 26113,
		/// <summary>Rivest's Code 2 (RC2) algorithm.</summary>
		Rc2,
		/// <summary>Rivest's Code 4 (RC4) algorithm.</summary>
		Rc4 = 26625,
		/// <summary>The Triple Data Encryption Standard (3DES) algorithm.</summary>
		TripleDes = 26115
	}
}
