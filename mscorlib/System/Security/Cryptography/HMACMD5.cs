using Mono.Security.Cryptography;
using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Computes a Hash-based Message Authentication Code (HMAC) using the <see cref="T:System.Security.Cryptography.MD5" /> hash function.</summary>
	[ComVisible(true)]
	public class HMACMD5 : HMAC
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.HMACMD5" /> class with a randomly generated key.</summary>
		public HMACMD5() : this(KeyBuilder.Key(8))
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.HMACMD5" /> class using the supplied key.</summary>
		/// <param name="key">The secret key for <see cref="T:System.Security.Cryptography.HMACMD5" /> encryption. The key can be any length, but if it is more than 64 bytes long it will be hashed (using SHA-1) to derive a 64-byte key. Therefore, the recommended size of the secret key is 64 bytes. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="key" /> parameter is null. </exception>
		public HMACMD5(byte[] key)
		{
			base.HashName = "MD5";
			this.HashSizeValue = 128;
			this.Key = key;
		}
	}
}
