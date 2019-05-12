using Mono.Security.Cryptography;
using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Creates Optimal Asymmetric Encryption Padding (OAEP) key exchange data using <see cref="T:System.Security.Cryptography.RSA" />.</summary>
	[ComVisible(true)]
	public class RSAOAEPKeyExchangeFormatter : AsymmetricKeyExchangeFormatter
	{
		private RSA rsa;

		private RandomNumberGenerator random;

		private byte[] param;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.RSAOAEPKeyExchangeFormatter" /> class.</summary>
		public RSAOAEPKeyExchangeFormatter()
		{
			this.rsa = null;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.RSAOAEPKeyExchangeFormatter" /> class with the specified key.</summary>
		/// <param name="key">The instance of the <see cref="T:System.Security.Cryptography.RSA" /> algorithm that holds the public key. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key " />is null.</exception>
		public RSAOAEPKeyExchangeFormatter(AsymmetricAlgorithm key)
		{
			this.SetKey(key);
		}

		/// <summary>Gets or sets the parameter used to create padding in the key exchange creation process.</summary>
		/// <returns>The parameter value.</returns>
		public byte[] Parameter
		{
			get
			{
				return this.param;
			}
			set
			{
				this.param = value;
			}
		}

		/// <summary>Gets the parameters for the Optimal Asymmetric Encryption Padding (OAEP) key exchange.</summary>
		/// <returns>An XML string containing the parameters of the OAEP key exchange operation.</returns>
		public override string Parameters
		{
			get
			{
				return null;
			}
		}

		/// <summary>Gets or sets the random number generator algorithm to use in the creation of the key exchange.</summary>
		/// <returns>The instance of a random number generator algorithm to use.</returns>
		public RandomNumberGenerator Rng
		{
			get
			{
				return this.random;
			}
			set
			{
				this.random = value;
			}
		}

		/// <summary>Creates the encrypted key exchange data from the specified input data.</summary>
		/// <returns>The encrypted key exchange data to be sent to the intended recipient.</returns>
		/// <param name="rgbData">The secret information to be passed in the key exchange. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicUnexpectedOperationException">The key is missing.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override byte[] CreateKeyExchange(byte[] rgbData)
		{
			if (this.random == null)
			{
				this.random = RandomNumberGenerator.Create();
			}
			if (this.rsa == null)
			{
				string text = Locale.GetText("No RSA key specified");
				throw new CryptographicUnexpectedOperationException(text);
			}
			SHA1 hash = SHA1.Create();
			return PKCS1.Encrypt_OAEP(this.rsa, hash, this.random, rgbData);
		}

		/// <summary>Creates the encrypted key exchange data from the specified input data.</summary>
		/// <returns>The encrypted key exchange data to be sent to the intended recipient.</returns>
		/// <param name="rgbData">The secret information to be passed in the key exchange. </param>
		/// <param name="symAlgType">This parameter is not used in the current version. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override byte[] CreateKeyExchange(byte[] rgbData, Type symAlgType)
		{
			return this.CreateKeyExchange(rgbData);
		}

		/// <summary>Sets the public key to use for encrypting the key exchange data.</summary>
		/// <param name="key">The instance of the <see cref="T:System.Security.Cryptography.RSA" /> algorithm that holds the public key. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key " />is null.</exception>
		public override void SetKey(AsymmetricAlgorithm key)
		{
			this.rsa = (RSA)key;
		}
	}
}
