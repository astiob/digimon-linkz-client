using Mono.Security.Cryptography;
using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Defines a wrapper object to access the cryptographic service provider (CSP) implementation of the <see cref="T:System.Security.Cryptography.RC2" /> algorithm. This class cannot be inherited.</summary>
	[ComVisible(true)]
	public sealed class RC2CryptoServiceProvider : RC2
	{
		private bool _useSalt;

		/// <summary>Gets or sets the effective size, in bits, of the secret key used by the <see cref="T:System.Security.Cryptography.RC2" /> algorithm.</summary>
		/// <returns>The effective key size, in bits, used by the <see cref="T:System.Security.Cryptography.RC2" /> algorithm.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicUnexpectedOperationException">The <see cref="P:System.Security.Cryptography.RC2CryptoServiceProvider.EffectiveKeySize" /> property was set to a value other than the <see cref="F:System.Security.Cryptography.SymmetricAlgorithm.KeySizeValue" /> property. </exception>
		public override int EffectiveKeySize
		{
			get
			{
				return base.EffectiveKeySize;
			}
			set
			{
				if (value != this.KeySizeValue)
				{
					throw new CryptographicUnexpectedOperationException(Locale.GetText("Effective key size must match key size for compatibility"));
				}
				base.EffectiveKeySize = value;
			}
		}

		/// <summary>Creates a symmetric <see cref="T:System.Security.Cryptography.RC2" /> decryptor object with the specified key (<see cref="P:System.Security.Cryptography.SymmetricAlgorithm.Key" />)and initialization vector (<see cref="P:System.Security.Cryptography.SymmetricAlgorithm.IV" />).</summary>
		/// <returns>A symmetric <see cref="T:System.Security.Cryptography.RC2" /> decryptor object.</returns>
		/// <param name="rgbKey">The secret key to use for the symmetric algorithm. </param>
		/// <param name="rgbIV">The initialization vector to use for the symmetric algorithm. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An <see cref="F:System.Security.Cryptography.CipherMode.OFB" /> cipher mode was used.-or-A <see cref="F:System.Security.Cryptography.CipherMode.CFB" /> cipher mode with a feedback size other than 8 bits was used.-or-An invalid key size was used.-or-The algorithm key size was not available.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
		{
			return new RC2Transform(this, false, rgbKey, rgbIV);
		}

		/// <summary>Creates a symmetric <see cref="T:System.Security.Cryptography.RC2" /> encryptor object with the specified key (<see cref="P:System.Security.Cryptography.SymmetricAlgorithm.Key" />) and initialization vector (<see cref="P:System.Security.Cryptography.SymmetricAlgorithm.IV" />).</summary>
		/// <returns>A symmetric <see cref="T:System.Security.Cryptography.RC2" /> encryptor object.</returns>
		/// <param name="rgbKey">The secret key to use for the symmetric algorithm. </param>
		/// <param name="rgbIV">The initialization vector to use for the symmetric algorithm. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An <see cref="F:System.Security.Cryptography.CipherMode.OFB" /> cipher mode was used.-or-A <see cref="F:System.Security.Cryptography.CipherMode.CFB" /> cipher mode with a feedback size other than 8 bits was used.-or-An invalid key size was used.-or-The algorithm key size was not available.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
		{
			return new RC2Transform(this, true, rgbKey, rgbIV);
		}

		/// <summary>Generates a random initialization vector (<see cref="P:System.Security.Cryptography.SymmetricAlgorithm.IV" />) to use for the algorithm.</summary>
		public override void GenerateIV()
		{
			this.IVValue = KeyBuilder.IV(this.BlockSizeValue >> 3);
		}

		/// <summary>Generates a random key (<see cref="P:System.Security.Cryptography.SymmetricAlgorithm.Key" />) to be used for the algorithm.</summary>
		public override void GenerateKey()
		{
			this.KeyValue = KeyBuilder.Key(this.KeySizeValue >> 3);
		}

		/// <summary>Gets or sets a value that determines whether to create a key with an 11-byte-long, zero-value salt. </summary>
		/// <returns>true if the key should be created with an 11-byte-long, zero-value salt; otherwise, false. The default is false.</returns>
		[ComVisible(false)]
		[MonoTODO("Use salt in algorithm")]
		public bool UseSalt
		{
			get
			{
				return this._useSalt;
			}
			set
			{
				this._useSalt = value;
			}
		}
	}
}
