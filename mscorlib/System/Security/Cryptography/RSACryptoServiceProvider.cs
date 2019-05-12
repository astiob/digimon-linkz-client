using Mono.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Performs asymmetric encryption and decryption using the implementation of the <see cref="T:System.Security.Cryptography.RSA" /> algorithm provided by the cryptographic service provider (CSP). This class cannot be inherited.</summary>
	[ComVisible(true)]
	public sealed class RSACryptoServiceProvider : RSA, ICspAsymmetricAlgorithm
	{
		private const int PROV_RSA_FULL = 1;

		private KeyPairPersistence store;

		private bool persistKey;

		private bool persisted;

		private bool privateKeyExportable = true;

		private bool m_disposed;

		private RSAManaged rsa;

		private static bool useMachineKeyStore;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.RSACryptoServiceProvider" /> class using the default key.</summary>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The cryptographic service provider (CSP) cannot be acquired. </exception>
		public RSACryptoServiceProvider()
		{
			this.Common(1024, null);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.RSACryptoServiceProvider" /> class with the specified parameters.</summary>
		/// <param name="parameters">The parameters to be passed to the cryptographic service provider (CSP). </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The CSP cannot be acquired. </exception>
		public RSACryptoServiceProvider(CspParameters parameters)
		{
			this.Common(1024, parameters);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.RSACryptoServiceProvider" /> class with the specified key size.</summary>
		/// <param name="dwKeySize">The size of the key to use in bits. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The cryptographic service provider (CSP) cannot be acquired. </exception>
		public RSACryptoServiceProvider(int dwKeySize)
		{
			this.Common(dwKeySize, null);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.RSACryptoServiceProvider" /> class with the specified key size and parameters.</summary>
		/// <param name="dwKeySize">The size of the key to use in bits. </param>
		/// <param name="parameters">The parameters to be passed to the cryptographic service provider (CSP). </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The CSP cannot be acquired.-or- The key cannot be created. </exception>
		public RSACryptoServiceProvider(int dwKeySize, CspParameters parameters)
		{
			this.Common(dwKeySize, parameters);
		}

		private void Common(int dwKeySize, CspParameters p)
		{
			this.LegalKeySizesValue = new KeySizes[1];
			this.LegalKeySizesValue[0] = new KeySizes(384, 16384, 8);
			base.KeySize = dwKeySize;
			this.rsa = new RSAManaged(this.KeySize);
			this.rsa.KeyGenerated += this.OnKeyGenerated;
			this.persistKey = (p != null);
			if (p == null)
			{
				p = new CspParameters(1);
				if (RSACryptoServiceProvider.useMachineKeyStore)
				{
					p.Flags |= CspProviderFlags.UseMachineKeyStore;
				}
				this.store = new KeyPairPersistence(p);
			}
			else
			{
				this.store = new KeyPairPersistence(p);
				this.store.Load();
				if (this.store.KeyValue != null)
				{
					this.persisted = true;
					this.FromXmlString(this.store.KeyValue);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the key should be persisted in the computer's key store instead of the user profile store.</summary>
		/// <returns>true if the key should be persisted in the computer key store; otherwise, false.</returns>
		public static bool UseMachineKeyStore
		{
			get
			{
				return RSACryptoServiceProvider.useMachineKeyStore;
			}
			set
			{
				RSACryptoServiceProvider.useMachineKeyStore = value;
			}
		}

		~RSACryptoServiceProvider()
		{
			this.Dispose(false);
		}

		/// <summary>Gets the name of the key exchange algorithm available with this implementation of <see cref="T:System.Security.Cryptography.RSA" />.</summary>
		/// <returns>The name of the key exchange algorithm if it exists; otherwise, null.</returns>
		public override string KeyExchangeAlgorithm
		{
			get
			{
				return "RSA-PKCS1-KeyEx";
			}
		}

		/// <summary>Gets the size of the current key.</summary>
		/// <returns>The size of the key in bits.</returns>
		public override int KeySize
		{
			get
			{
				if (this.rsa == null)
				{
					return this.KeySizeValue;
				}
				return this.rsa.KeySize;
			}
		}

		/// <summary>Gets or sets a value indicating whether the key should be persisted in the cryptographic service provider (CSP).</summary>
		/// <returns>true if the key should be persisted in the CSP; otherwise, false.</returns>
		public bool PersistKeyInCsp
		{
			get
			{
				return this.persistKey;
			}
			set
			{
				this.persistKey = value;
				if (this.persistKey)
				{
					this.OnKeyGenerated(this.rsa, null);
				}
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Security.Cryptography.RSACryptoServiceProvider" /> object contains only a public key.</summary>
		/// <returns>true if the <see cref="T:System.Security.Cryptography.RSACryptoServiceProvider" /> object contains only a public key; otherwise, false.</returns>
		[ComVisible(false)]
		public bool PublicOnly
		{
			get
			{
				return this.rsa.PublicOnly;
			}
		}

		/// <summary>Gets the name of the signature algorithm available with this implementation of <see cref="T:System.Security.Cryptography.RSA" />.</summary>
		/// <returns>The name of the signature algorithm.</returns>
		public override string SignatureAlgorithm
		{
			get
			{
				return "http://www.w3.org/2000/09/xmldsig#rsa-sha1";
			}
		}

		/// <summary>Decrypts data with the <see cref="T:System.Security.Cryptography.RSA" /> algorithm.</summary>
		/// <returns>The decrypted data, which is the original plain text before encryption.</returns>
		/// <param name="rgb">The data to be decrypted. </param>
		/// <param name="fOAEP">true to perform direct <see cref="T:System.Security.Cryptography.RSA" /> decryption using OAEP padding (only available on a computer running Microsoft Windows XP or later); otherwise, false to use PKCS#1 v1.5 padding. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The cryptographic service provider (CSP) cannot be acquired.-or- The <paramref name="fOAEP" /> parameter is true and the length of the <paramref name="rgb" /> parameter is greater than <see cref="P:System.Security.Cryptography.RSACryptoServiceProvider.KeySize" />.-or- The <paramref name="fOAEP" /> parameter is true and OAEP is not supported. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="rgb " />is null.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public byte[] Decrypt(byte[] rgb, bool fOAEP)
		{
			if (this.m_disposed)
			{
				throw new ObjectDisposedException("rsa");
			}
			AsymmetricKeyExchangeDeformatter asymmetricKeyExchangeDeformatter;
			if (fOAEP)
			{
				asymmetricKeyExchangeDeformatter = new RSAOAEPKeyExchangeDeformatter(this.rsa);
			}
			else
			{
				asymmetricKeyExchangeDeformatter = new RSAPKCS1KeyExchangeDeformatter(this.rsa);
			}
			return asymmetricKeyExchangeDeformatter.DecryptKeyExchange(rgb);
		}

		/// <summary>This method is not supported in the current version.</summary>
		/// <returns>The decrypted data, which is the original plain text before encryption.</returns>
		/// <param name="rgb">The data to be decrypted. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not supported in the current version. </exception>
		public override byte[] DecryptValue(byte[] rgb)
		{
			if (!this.rsa.IsCrtPossible)
			{
				throw new CryptographicException("Incomplete private key - missing CRT.");
			}
			return this.rsa.DecryptValue(rgb);
		}

		/// <summary>Encrypts data with the <see cref="T:System.Security.Cryptography.RSA" /> algorithm.</summary>
		/// <returns>The encrypted data.</returns>
		/// <param name="rgb">The data to be encrypted. </param>
		/// <param name="fOAEP">true to perform direct <see cref="T:System.Security.Cryptography.RSA" /> encryption using Optimal Asymmetric Encryption Padding (OAEP), which is only available on a computer running Microsoft Windows XP or later; false to use PKCS#1 v1.5 padding. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The cryptographic service provider (CSP) cannot be acquired.-or- The length of the <paramref name="rgb" /> parameter is greater than the maximum allowed length.-or- The <paramref name="fOAEP" /> parameter is true and OAEP padding is not supported. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="rgb " />is null.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public byte[] Encrypt(byte[] rgb, bool fOAEP)
		{
			AsymmetricKeyExchangeFormatter asymmetricKeyExchangeFormatter;
			if (fOAEP)
			{
				asymmetricKeyExchangeFormatter = new RSAOAEPKeyExchangeFormatter(this.rsa);
			}
			else
			{
				asymmetricKeyExchangeFormatter = new RSAPKCS1KeyExchangeFormatter(this.rsa);
			}
			return asymmetricKeyExchangeFormatter.CreateKeyExchange(rgb);
		}

		/// <summary>This method is not supported in the current version.</summary>
		/// <returns>The encrypted data.</returns>
		/// <param name="rgb">The data to be encrypted. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not supported in the current version. </exception>
		public override byte[] EncryptValue(byte[] rgb)
		{
			return this.rsa.EncryptValue(rgb);
		}

		/// <summary>Exports the <see cref="T:System.Security.Cryptography.RSAParameters" />.</summary>
		/// <returns>The parameters for <see cref="T:System.Security.Cryptography.RSA" />.</returns>
		/// <param name="includePrivateParameters">true to include private parameters; otherwise, false. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The key cannot be exported. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override RSAParameters ExportParameters(bool includePrivateParameters)
		{
			if (includePrivateParameters && !this.privateKeyExportable)
			{
				throw new CryptographicException("cannot export private key");
			}
			return this.rsa.ExportParameters(includePrivateParameters);
		}

		/// <summary>Imports the specified <see cref="T:System.Security.Cryptography.RSAParameters" />.</summary>
		/// <param name="parameters">The parameters for <see cref="T:System.Security.Cryptography.RSA" />. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The cryptographic service provider (CSP) cannot be acquired.-or- The <paramref name="parameters" /> parameter has missing fields. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override void ImportParameters(RSAParameters parameters)
		{
			this.rsa.ImportParameters(parameters);
		}

		private HashAlgorithm GetHash(object halg)
		{
			if (halg == null)
			{
				throw new ArgumentNullException("halg");
			}
			HashAlgorithm result;
			if (halg is string)
			{
				result = HashAlgorithm.Create((string)halg);
			}
			else if (halg is HashAlgorithm)
			{
				result = (HashAlgorithm)halg;
			}
			else
			{
				if (!(halg is Type))
				{
					throw new ArgumentException("halg");
				}
				result = (HashAlgorithm)Activator.CreateInstance((Type)halg);
			}
			return result;
		}

		/// <summary>Computes the hash value of the specified byte array using the specified hash algorithm, and signs the resulting hash value.</summary>
		/// <returns>The <see cref="T:System.Security.Cryptography.RSA" /> signature for the specified data.</returns>
		/// <param name="buffer">The input data for which to compute the hash. </param>
		/// <param name="halg">The hash algorithm to use to create the hash value. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="halg" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="halg" /> parameter is not a valid type. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public byte[] SignData(byte[] buffer, object halg)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			return this.SignData(buffer, 0, buffer.Length, halg);
		}

		/// <summary>Computes the hash value of the specified input stream using the specified hash algorithm, and signs the resulting hash value.</summary>
		/// <returns>The <see cref="T:System.Security.Cryptography.RSA" /> signature for the specified data.</returns>
		/// <param name="inputStream">The input data for which to compute the hash. </param>
		/// <param name="halg">The hash algorithm to use to create the hash value. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="halg" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="halg" /> parameter is not a valid type. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public byte[] SignData(Stream inputStream, object halg)
		{
			HashAlgorithm hash = this.GetHash(halg);
			byte[] hashValue = hash.ComputeHash(inputStream);
			return PKCS1.Sign_v15(this, hash, hashValue);
		}

		/// <summary>Computes the hash value of a subset of the specified byte array using the specified hash algorithm, and signs the resulting hash value.</summary>
		/// <returns>The <see cref="T:System.Security.Cryptography.RSA" /> signature for the specified data.</returns>
		/// <param name="buffer">The input data for which to compute the hash. </param>
		/// <param name="offset">The offset into the array from which to begin using data. </param>
		/// <param name="count">The number of bytes in the array to use as data. </param>
		/// <param name="halg">The hash algorithm to use to create the hash value. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="halg" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="halg" /> parameter is not a valid type. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public byte[] SignData(byte[] buffer, int offset, int count, object halg)
		{
			HashAlgorithm hash = this.GetHash(halg);
			byte[] hashValue = hash.ComputeHash(buffer, offset, count);
			return PKCS1.Sign_v15(this, hash, hashValue);
		}

		private string GetHashNameFromOID(string oid)
		{
			if (oid != null)
			{
				if (RSACryptoServiceProvider.<>f__switch$map29 == null)
				{
					RSACryptoServiceProvider.<>f__switch$map29 = new Dictionary<string, int>(2)
					{
						{
							"1.3.14.3.2.26",
							0
						},
						{
							"1.2.840.113549.2.5",
							1
						}
					};
				}
				int num;
				if (RSACryptoServiceProvider.<>f__switch$map29.TryGetValue(oid, out num))
				{
					if (num == 0)
					{
						return "SHA1";
					}
					if (num == 1)
					{
						return "MD5";
					}
				}
			}
			throw new NotSupportedException(oid + " is an unsupported hash algorithm for RSA signing");
		}

		/// <summary>Computes the signature for the specified hash value by encrypting it with the private key.</summary>
		/// <returns>The <see cref="T:System.Security.Cryptography.RSA" /> signature for the specified hash value.</returns>
		/// <param name="rgbHash">The hash value of the data to be signed. </param>
		/// <param name="str">The hash algorithm identifier (OID) used to create the hash value of the data. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="rgbHash" /> parameter is null. </exception>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The cryptographic service provider (CSP) cannot be acquired.-or- There is no private key. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public byte[] SignHash(byte[] rgbHash, string str)
		{
			if (rgbHash == null)
			{
				throw new ArgumentNullException("rgbHash");
			}
			string hashName = (str != null) ? this.GetHashNameFromOID(str) : "SHA1";
			HashAlgorithm hash = HashAlgorithm.Create(hashName);
			return PKCS1.Sign_v15(this, hash, rgbHash);
		}

		/// <summary>Verifies that a digital signature is valid by determining the hash value in the signature using the provided public key and comparing it to the hash value of the provided data.</summary>
		/// <returns>true if the signature is valid; otherwise, false.</returns>
		/// <param name="buffer">The data that was signed. </param>
		/// <param name="halg">The name of the hash algorithm used to create the hash value of the data. </param>
		/// <param name="signature">The signature data to use for verification. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="halg" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="halg" /> parameter is not a valid type. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public bool VerifyData(byte[] buffer, object halg, byte[] signature)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (signature == null)
			{
				throw new ArgumentNullException("signature");
			}
			HashAlgorithm hash = this.GetHash(halg);
			byte[] hashValue = hash.ComputeHash(buffer);
			return PKCS1.Verify_v15(this, hash, hashValue, signature);
		}

		/// <summary>Verifies that a digital signature is valid by determining the hash value in the signature using the provided public key and comparing it to the provided hash value.</summary>
		/// <returns>true if the signature is valid; otherwise, false.</returns>
		/// <param name="rgbHash">The hash value of the signed data. </param>
		/// <param name="str">The hash algorithm identifier (OID) used to create the hash value of the data. </param>
		/// <param name="rgbSignature">The signature data to use for verification. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="rgbHash" /> parameter is null.-or- The <paramref name="rgbSignature" /> parameter is null. </exception>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The cryptographic service provider (CSP) cannot be acquired.-or- The signature cannot be verified. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public bool VerifyHash(byte[] rgbHash, string str, byte[] rgbSignature)
		{
			if (rgbHash == null)
			{
				throw new ArgumentNullException("rgbHash");
			}
			if (rgbSignature == null)
			{
				throw new ArgumentNullException("rgbSignature");
			}
			string hashName = (str != null) ? this.GetHashNameFromOID(str) : "SHA1";
			HashAlgorithm hash = HashAlgorithm.Create(hashName);
			return PKCS1.Verify_v15(this, hash, rgbHash, rgbSignature);
		}

		protected override void Dispose(bool disposing)
		{
			if (!this.m_disposed)
			{
				if (this.persisted && !this.persistKey)
				{
					this.store.Remove();
				}
				if (this.rsa != null)
				{
					this.rsa.Clear();
				}
				this.m_disposed = true;
			}
		}

		private void OnKeyGenerated(object sender, EventArgs e)
		{
			if (this.persistKey && !this.persisted)
			{
				this.store.KeyValue = this.ToXmlString(!this.rsa.PublicOnly);
				this.store.Save();
				this.persisted = true;
			}
		}

		/// <summary>Gets a <see cref="T:System.Security.Cryptography.CspKeyContainerInfo" /> object that describes additional information about a cryptographic key pair. </summary>
		/// <returns>A <see cref="T:System.Security.Cryptography.CspKeyContainerInfo" /> object that describes additional information about a cryptographic key pair.</returns>
		[MonoTODO("Always return null")]
		[ComVisible(false)]
		public CspKeyContainerInfo CspKeyContainerInfo
		{
			get
			{
				return null;
			}
		}

		/// <summary>Exports a blob containing the key information associated with an <see cref="T:System.Security.Cryptography.RSACryptoServiceProvider" /> object.  </summary>
		/// <returns>A byte array containing the key information associated with an <see cref="T:System.Security.Cryptography.RSACryptoServiceProvider" /> object.</returns>
		/// <param name="includePrivateParameters">true to include the private key; otherwise, false.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[ComVisible(false)]
		public byte[] ExportCspBlob(bool includePrivateParameters)
		{
			byte[] array;
			if (includePrivateParameters)
			{
				array = CryptoConvert.ToCapiPrivateKeyBlob(this);
			}
			else
			{
				array = CryptoConvert.ToCapiPublicKeyBlob(this);
			}
			array[5] = 164;
			return array;
		}

		/// <summary>Imports a blob that represents RSA key information.  </summary>
		/// <param name="keyBlob">A byte array that represents an RSA key blob.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[ComVisible(false)]
		public void ImportCspBlob(byte[] keyBlob)
		{
			if (keyBlob == null)
			{
				throw new ArgumentNullException("keyBlob");
			}
			RSA rsa = CryptoConvert.FromCapiKeyBlob(keyBlob);
			if (rsa is RSACryptoServiceProvider)
			{
				RSAParameters parameters = rsa.ExportParameters(!(rsa as RSACryptoServiceProvider).PublicOnly);
				this.ImportParameters(parameters);
			}
			else
			{
				try
				{
					RSAParameters parameters2 = rsa.ExportParameters(true);
					this.ImportParameters(parameters2);
				}
				catch
				{
					RSAParameters parameters3 = rsa.ExportParameters(false);
					this.ImportParameters(parameters3);
				}
			}
		}
	}
}
