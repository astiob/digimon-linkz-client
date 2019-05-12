using Mono.Security.Cryptography;
using System;

namespace System.Security.Cryptography
{
	/// <summary>Provides a managed implementation of the Advanced Encryption Standard (AES) symmetric algorithm. </summary>
	public sealed class AesManaged : Aes
	{
		/// <summary>Generates a random initialization vector (IV) to use for the symmetric algorithm.</summary>
		public override void GenerateIV()
		{
			this.IVValue = KeyBuilder.IV(this.BlockSizeValue >> 3);
		}

		/// <summary>Generates a random key to use for the symmetric algorithm. </summary>
		public override void GenerateKey()
		{
			this.KeyValue = KeyBuilder.Key(this.KeySizeValue >> 3);
		}

		/// <summary>Creates a symmetric decryptor object using the specified key and initialization vector (IV).</summary>
		/// <returns>A symmetric decryptor object.</returns>
		/// <param name="key">The secret key to use for the symmetric algorithm.</param>
		/// <param name="iv">The initialization vector to use for the symmetric algorithm.</param>
		public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
		{
			return new AesTransform(this, false, rgbKey, rgbIV);
		}

		/// <summary>Creates a symmetric encryptor object using the specified key and initialization vector (IV).</summary>
		/// <returns>A symmetric encryptor object.</returns>
		/// <param name="key">The secret key to use for the symmetric algorithm.</param>
		/// <param name="iv">The initialization vector to use for the symmetric algorithm.</param>
		public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
		{
			return new AesTransform(this, true, rgbKey, rgbIV);
		}

		/// <summary>Gets or sets the initialization vector (IV) to use for the symmetric algorithm. </summary>
		/// <returns>The initialization vector to use for the symmetric algorithm</returns>
		public override byte[] IV
		{
			get
			{
				return base.IV;
			}
			set
			{
				base.IV = value;
			}
		}

		/// <summary>Gets or sets the secret key used for the symmetric algorithm.</summary>
		/// <returns>The key for the symmetric algorithm.</returns>
		public override byte[] Key
		{
			get
			{
				return base.Key;
			}
			set
			{
				base.Key = value;
			}
		}

		/// <summary>Gets or sets the size, in bits, of the secret key used for the symmetric algorithm. </summary>
		/// <returns>The size, in bits, of the key used by the symmetric algorithm.</returns>
		public override int KeySize
		{
			get
			{
				return base.KeySize;
			}
			set
			{
				base.KeySize = value;
			}
		}

		/// <summary>Creates a symmetric decryptor object using the current key and initialization vector (IV).</summary>
		/// <returns>A symmetric decryptor object.</returns>
		public override ICryptoTransform CreateDecryptor()
		{
			return this.CreateDecryptor(this.Key, this.IV);
		}

		/// <summary>Creates a symmetric encryptor object using the current key and initialization vector (IV).</summary>
		/// <returns>A symmetric encryptor object.</returns>
		public override ICryptoTransform CreateEncryptor()
		{
			return this.CreateEncryptor(this.Key, this.IV);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
	}
}
