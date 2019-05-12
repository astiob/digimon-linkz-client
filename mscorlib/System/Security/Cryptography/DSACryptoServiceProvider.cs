using Mono.Security.Cryptography;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Defines a wrapper object to access the cryptographic service provider (CSP) implementation of the <see cref="T:System.Security.Cryptography.DSA" /> algorithm. This class cannot be inherited. </summary>
	[ComVisible(true)]
	public sealed class DSACryptoServiceProvider : DSA, ICspAsymmetricAlgorithm
	{
		private const int PROV_DSS_DH = 13;

		private KeyPairPersistence store;

		private bool persistKey;

		private bool persisted;

		private bool privateKeyExportable = true;

		private bool m_disposed;

		private DSAManaged dsa;

		private static bool useMachineKeyStore;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.DSACryptoServiceProvider" /> class.</summary>
		public DSACryptoServiceProvider() : this(1024, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.DSACryptoServiceProvider" /> class with the specified parameters for the cryptographic service provider (CSP).</summary>
		/// <param name="parameters">The parameters for the CSP. </param>
		public DSACryptoServiceProvider(CspParameters parameters) : this(1024, parameters)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.DSACryptoServiceProvider" /> class with the specified key size.</summary>
		/// <param name="dwKeySize">The size of the key for the asymmetric algorithm in bits. </param>
		public DSACryptoServiceProvider(int dwKeySize) : this(dwKeySize, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.DSACryptoServiceProvider" /> class with the specified key size and parameters for the cryptographic service provider (CSP).</summary>
		/// <param name="dwKeySize">The size of the key for the cryptographic algorithm in bits. </param>
		/// <param name="parameters">The parameters for the CSP. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The CSP cannot be acquired.-or- The key cannot be created. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="dwKeySize" /> is out of range.</exception>
		public DSACryptoServiceProvider(int dwKeySize, CspParameters parameters)
		{
			this.LegalKeySizesValue = new KeySizes[1];
			this.LegalKeySizesValue[0] = new KeySizes(512, 1024, 64);
			this.KeySize = dwKeySize;
			this.dsa = new DSAManaged(dwKeySize);
			this.dsa.KeyGenerated += this.OnKeyGenerated;
			this.persistKey = (parameters != null);
			if (parameters == null)
			{
				parameters = new CspParameters(13);
				if (DSACryptoServiceProvider.useMachineKeyStore)
				{
					parameters.Flags |= CspProviderFlags.UseMachineKeyStore;
				}
				this.store = new KeyPairPersistence(parameters);
			}
			else
			{
				this.store = new KeyPairPersistence(parameters);
				this.store.Load();
				if (this.store.KeyValue != null)
				{
					this.persisted = true;
					this.FromXmlString(this.store.KeyValue);
				}
			}
		}

		~DSACryptoServiceProvider()
		{
			this.Dispose(false);
		}

		/// <summary>Gets the name of the key exchange algorithm.</summary>
		/// <returns>The name of the key exchange algorithm.</returns>
		public override string KeyExchangeAlgorithm
		{
			get
			{
				return null;
			}
		}

		/// <summary>Gets the size of the key used by the asymmetric algorithm in bits.</summary>
		/// <returns>The size of the key used by the asymmetric algorithm.</returns>
		public override int KeySize
		{
			get
			{
				return this.dsa.KeySize;
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
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Security.Cryptography.DSACryptoServiceProvider" /> object contains only a public key.</summary>
		/// <returns>true if the <see cref="T:System.Security.Cryptography.DSACryptoServiceProvider" /> object contains only a public key; otherwise, false.</returns>
		[ComVisible(false)]
		public bool PublicOnly
		{
			get
			{
				return this.dsa.PublicOnly;
			}
		}

		/// <summary>Gets the name of the signature algorithm.</summary>
		/// <returns>The name of the signature algorithm.</returns>
		public override string SignatureAlgorithm
		{
			get
			{
				return "http://www.w3.org/2000/09/xmldsig#dsa-sha1";
			}
		}

		/// <summary>Gets or sets a value indicating whether the key should be persisted in the computer's key store instead of the user profile store.</summary>
		/// <returns>true if the key should be persisted in the computer key store; otherwise, false.</returns>
		public static bool UseMachineKeyStore
		{
			get
			{
				return DSACryptoServiceProvider.useMachineKeyStore;
			}
			set
			{
				DSACryptoServiceProvider.useMachineKeyStore = value;
			}
		}

		/// <summary>Exports the <see cref="T:System.Security.Cryptography.DSAParameters" />.</summary>
		/// <returns>The parameters for <see cref="T:System.Security.Cryptography.DSA" />.</returns>
		/// <param name="includePrivateParameters">true to include private parameters; otherwise, false. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The key cannot be exported. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override DSAParameters ExportParameters(bool includePrivateParameters)
		{
			if (includePrivateParameters && !this.privateKeyExportable)
			{
				throw new CryptographicException(Locale.GetText("Cannot export private key"));
			}
			return this.dsa.ExportParameters(includePrivateParameters);
		}

		/// <summary>Imports the specified <see cref="T:System.Security.Cryptography.DSAParameters" />.</summary>
		/// <param name="parameters">The parameters for <see cref="T:System.Security.Cryptography.DSA" />. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The cryptographic service provider (CSP) cannot be acquired.-or- The <paramref name="parameters" /> parameter has missing fields. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override void ImportParameters(DSAParameters parameters)
		{
			this.dsa.ImportParameters(parameters);
		}

		/// <summary>Creates the <see cref="T:System.Security.Cryptography.DSA" /> signature for the specified data.</summary>
		/// <returns>The digital signature for the specified data.</returns>
		/// <param name="rgbHash">The data to be signed. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override byte[] CreateSignature(byte[] rgbHash)
		{
			return this.dsa.CreateSignature(rgbHash);
		}

		/// <summary>Computes the hash value of the specified byte array and signs the resulting hash value.</summary>
		/// <returns>The <see cref="T:System.Security.Cryptography.DSA" /> signature for the specified data.</returns>
		/// <param name="buffer">The input data for which to compute the hash. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public byte[] SignData(byte[] buffer)
		{
			HashAlgorithm hashAlgorithm = SHA1.Create();
			byte[] rgbHash = hashAlgorithm.ComputeHash(buffer);
			return this.dsa.CreateSignature(rgbHash);
		}

		/// <summary>Signs a byte array from the specified start point to the specified end point.</summary>
		/// <returns>The <see cref="T:System.Security.Cryptography.DSA" /> signature for the specified data.</returns>
		/// <param name="buffer">The input data to sign. </param>
		/// <param name="offset">The offset into the array from which to begin using data. </param>
		/// <param name="count">The number of bytes in the array to use as data. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public byte[] SignData(byte[] buffer, int offset, int count)
		{
			HashAlgorithm hashAlgorithm = SHA1.Create();
			byte[] rgbHash = hashAlgorithm.ComputeHash(buffer, offset, count);
			return this.dsa.CreateSignature(rgbHash);
		}

		/// <summary>Computes the hash value of the specified input stream and signs the resulting hash value.</summary>
		/// <returns>The <see cref="T:System.Security.Cryptography.DSA" /> signature for the specified data.</returns>
		/// <param name="inputStream">The input data for which to compute the hash. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public byte[] SignData(Stream inputStream)
		{
			HashAlgorithm hashAlgorithm = SHA1.Create();
			byte[] rgbHash = hashAlgorithm.ComputeHash(inputStream);
			return this.dsa.CreateSignature(rgbHash);
		}

		/// <summary>Computes the signature for the specified hash value by encrypting it with the private key.</summary>
		/// <returns>The <see cref="T:System.Security.Cryptography.DSA" /> signature for the specified hash value.</returns>
		/// <param name="rgbHash">The hash value of the data to be signed. </param>
		/// <param name="str">The name of the hash algorithm used to create the hash value of the data. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="rgbHash" /> parameter is null. </exception>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The cryptographic service provider (CSP) cannot be acquired.-or- There is no private key. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public byte[] SignHash(byte[] rgbHash, string str)
		{
			if (string.Compare(str, "SHA1", true, CultureInfo.InvariantCulture) != 0)
			{
				throw new CryptographicException(Locale.GetText("Only SHA1 is supported."));
			}
			return this.dsa.CreateSignature(rgbHash);
		}

		/// <summary>Verifies the specified data using the specified signature.</summary>
		/// <returns>true if the signature verifies the data; otherwise, false.</returns>
		/// <param name="rgbData">The data that was signed. </param>
		/// <param name="rgbSignature">The signature data to verify. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public bool VerifyData(byte[] rgbData, byte[] rgbSignature)
		{
			HashAlgorithm hashAlgorithm = SHA1.Create();
			byte[] rgbHash = hashAlgorithm.ComputeHash(rgbData);
			return this.dsa.VerifySignature(rgbHash, rgbSignature);
		}

		/// <summary>Verifies the specified hash data using the specified signature.</summary>
		/// <returns>true if the signature verifies the hash; otherwise, false.</returns>
		/// <param name="rgbHash">The hash value of the data to be signed. </param>
		/// <param name="str">The name of the hash algorithm used to create the hash value of the data. </param>
		/// <param name="rgbSignature">The signature data to verify. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="rgbHash" /> parameter is null.-or- The <paramref name="rgbSignature" /> parameter is null. </exception>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The cryptographic service provider (CSP) cannot be acquired.-or- The signature cannot be verified. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public bool VerifyHash(byte[] rgbHash, string str, byte[] rgbSignature)
		{
			if (str == null)
			{
				str = "SHA1";
			}
			if (string.Compare(str, "SHA1", true, CultureInfo.InvariantCulture) != 0)
			{
				throw new CryptographicException(Locale.GetText("Only SHA1 is supported."));
			}
			return this.dsa.VerifySignature(rgbHash, rgbSignature);
		}

		/// <summary>Verifies the <see cref="T:System.Security.Cryptography.DSA" /> signature for the specified data.</summary>
		/// <returns>true if <paramref name="rgbSignature" /> matches the signature that is computed using the specified hash algorithm and key on <paramref name="rgbHash" />; otherwise, false.</returns>
		/// <param name="rgbHash">The data signed with <paramref name="rgbSignature" />. </param>
		/// <param name="rgbSignature">The signature to verify for <paramref name="rgbData" />. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override bool VerifySignature(byte[] rgbHash, byte[] rgbSignature)
		{
			return this.dsa.VerifySignature(rgbHash, rgbSignature);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Security.Cryptography.DSACryptoServiceProvider" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		protected override void Dispose(bool disposing)
		{
			if (!this.m_disposed)
			{
				if (this.persisted && !this.persistKey)
				{
					this.store.Remove();
				}
				if (this.dsa != null)
				{
					this.dsa.Clear();
				}
				this.m_disposed = true;
			}
		}

		private void OnKeyGenerated(object sender, EventArgs e)
		{
			if (this.persistKey && !this.persisted)
			{
				this.store.KeyValue = this.ToXmlString(!this.dsa.PublicOnly);
				this.store.Save();
				this.persisted = true;
			}
		}

		/// <summary>Gets a <see cref="T:System.Security.Cryptography.CspKeyContainerInfo" /> object that describes additional information about a cryptographic key pair.  </summary>
		/// <returns>A <see cref="T:System.Security.Cryptography.CspKeyContainerInfo" /> object that describes additional information about a cryptographic key pair.</returns>
		[ComVisible(false)]
		[MonoTODO("call into KeyPairPersistence to get details")]
		public CspKeyContainerInfo CspKeyContainerInfo
		{
			get
			{
				return null;
			}
		}

		/// <summary>Exports a blob containing the key information associated with a <see cref="T:System.Security.Cryptography.DSACryptoServiceProvider" /> object.  </summary>
		/// <returns>A byte array containing the key information associated with a <see cref="T:System.Security.Cryptography.DSACryptoServiceProvider" /> object.</returns>
		/// <param name="includePrivateParameters">true to include the private key; otherwise, false.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[ComVisible(false)]
		public byte[] ExportCspBlob(bool includePrivateParameters)
		{
			byte[] result;
			if (includePrivateParameters)
			{
				result = CryptoConvert.ToCapiPrivateKeyBlob(this);
			}
			else
			{
				result = CryptoConvert.ToCapiPublicKeyBlob(this);
			}
			return result;
		}

		/// <summary>Imports a blob that represents DSA key information.</summary>
		/// <param name="keyBlob">A byte array that represents a DSA key blob.</param>
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
			DSA dsa = CryptoConvert.FromCapiKeyBlobDSA(keyBlob);
			if (dsa is DSACryptoServiceProvider)
			{
				DSAParameters parameters = dsa.ExportParameters(!(dsa as DSACryptoServiceProvider).PublicOnly);
				this.ImportParameters(parameters);
			}
			else
			{
				try
				{
					DSAParameters parameters2 = dsa.ExportParameters(true);
					this.ImportParameters(parameters2);
				}
				catch
				{
					DSAParameters parameters3 = dsa.ExportParameters(false);
					this.ImportParameters(parameters3);
				}
			}
		}
	}
}
