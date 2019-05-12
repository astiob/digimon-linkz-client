using Mono.Security.Cryptography;
using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Computes a Hash-based Message Authentication Code (HMAC) using the <see cref="T:System.Security.Cryptography.SHA1" /> hash function.</summary>
	[ComVisible(true)]
	public class HMACSHA1 : HMAC
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.HMACSHA1" /> class with a randomly generated key.</summary>
		public HMACSHA1() : this(KeyBuilder.Key(8))
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.HMACSHA1" /> class with the specified key data.</summary>
		/// <param name="key">The secret key for <see cref="T:System.Security.Cryptography.HMACSHA1" /> encryption. The key can be any length, but if it is more than 64 bytes long it will be hashed (using SHA-1) to derive a 64-byte key. Therefore, the recommended size of the secret key is 64 bytes. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="key" /> parameter is null. </exception>
		public HMACSHA1(byte[] key)
		{
			base.HashName = "SHA1";
			this.HashSizeValue = 160;
			this.Key = key;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.HMACSHA1" /> class with the specified key data and a value that specifies whether to use the managed version of the SHA-1 algorithm.</summary>
		/// <param name="key">The secret key for <see cref="T:System.Security.Cryptography.HMACSHA1" /> encryption. The key can be any length but if it is more than 64 bytes long, it will be hashed (using SHA-1) to derive a 64-byte key. Therefore, the recommended size of the secret key is 64 bytes.</param>
		/// <param name="useManagedSha1">true to use the managed implementation of the SHA-1 algorithm (the <see cref="T:System.Security.Cryptography.SHA1Managed" /> class); false to use the unmanaged implementation (the <see cref="T:System.Security.Cryptography.SHA1CryptoServiceProvider" /> class).   </param>
		public HMACSHA1(byte[] key, bool useManagedSha1)
		{
			base.HashName = "System.Security.Cryptography.SHA1" + ((!useManagedSha1) ? "CryptoServiceProvider" : "Managed");
			this.HashSizeValue = 160;
			this.Key = key;
		}
	}
}
