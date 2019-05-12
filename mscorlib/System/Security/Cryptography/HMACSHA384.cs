using Mono.Security.Cryptography;
using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Computes a Hash-based Message Authentication Code (HMAC) using the <see cref="T:System.Security.Cryptography.SHA384" /> hash function.</summary>
	[ComVisible(true)]
	public class HMACSHA384 : HMAC
	{
		private static bool legacy_mode = Environment.GetEnvironmentVariable("legacyHMACMode") == "1";

		private bool legacy;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.HMACSHA384" /> class by using a randomly generated key.</summary>
		public HMACSHA384() : this(KeyBuilder.Key(8))
		{
			this.ProduceLegacyHmacValues = HMACSHA384.legacy_mode;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.HMACSHA384" /> class by using the specified key data.</summary>
		/// <param name="key">The secret key for <see cref="T:System.Security.Cryptography.HMACSHA384" /> encryption. The key can be any length. However, if it is more than 64 bytes long it will be hashed (using SHA-1) to derive a 64-byte key. Therefore, the recommended size of the secret key is 64 bytes. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="key" /> parameter is null. </exception>
		public HMACSHA384(byte[] key)
		{
			this.ProduceLegacyHmacValues = HMACSHA384.legacy_mode;
			base.HashName = "SHA384";
			this.HashSizeValue = 384;
			this.Key = key;
		}

		/// <summary>Provides a workaround for the .NET Framework version 2.0 implementation of the <see cref="T:System.Security.Cryptography.HMACSHA384" /> algorithm, which is inconsistent with the .NET Framework version 2.0 Service Pack 1 implementation of the algorithm.</summary>
		/// <returns>true to enable .NET Framework version 2.0 Service Pack 1 applications to interact with .NET Framework 2.0 applications; otherwise, false.</returns>
		public bool ProduceLegacyHmacValues
		{
			get
			{
				return this.legacy;
			}
			set
			{
				this.legacy = value;
				base.BlockSizeValue = ((!this.legacy) ? 128 : 64);
			}
		}
	}
}
