using Mono.Security.Authenticode;
using Mono.Security.X509;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

namespace System.Security.Cryptography.X509Certificates
{
	/// <summary>Provides methods that help you use X.509 v.3 certificates.</summary>
	[ComVisible(true)]
	[MonoTODO("X509ContentType.SerializedCert isn't supported (anywhere in the class)")]
	[Serializable]
	public class X509Certificate : ISerializable, IDeserializationCallback
	{
		private X509Certificate x509;

		private bool hideDates;

		private byte[] cachedCertificateHash;

		private string issuer_name;

		private string subject_name;

		internal X509Certificate(byte[] data, bool dates)
		{
			if (data != null)
			{
				this.Import(data, null, X509KeyStorageFlags.DefaultKeySet);
				this.hideDates = !dates;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class defined from a sequence of bytes representing an X.509v3 certificate.</summary>
		/// <param name="data">A byte array containing data from an X.509 certificate.</param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="rawData" /> parameter is null.-or-The length of the <paramref name="rawData" /> parameter is 0.</exception>
		public X509Certificate(byte[] data) : this(data, true)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class using a handle to an unmanaged PCCERT_CONTEXT structure.</summary>
		/// <param name="handle">A handle to an unmanaged PCCERT_CONTEXT structure.</param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		/// <exception cref="T:System.ArgumentException">The handle parameter does not represent a valid PCCERT_CONTEXT structure.</exception>
		public X509Certificate(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentException("Invalid handle.");
			}
			throw new NotSupportedException();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class using another <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class.</summary>
		/// <param name="cert">A <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class from which to initialize this class. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		/// <exception cref="T:System.ArgumentNullException">The value of the <paramref name="cert" /> parameter is null.</exception>
		public X509Certificate(X509Certificate cert)
		{
			if (cert == null)
			{
				throw new ArgumentNullException("cert");
			}
			if (cert != null)
			{
				byte[] rawCertData = cert.GetRawCertData();
				if (rawCertData != null)
				{
					this.x509 = new X509Certificate(rawCertData);
				}
				this.hideDates = false;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class. </summary>
		public X509Certificate()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class using a byte array and a password.</summary>
		/// <param name="rawData">A byte array containing data from an X.509 certificate.</param>
		/// <param name="password">The password required to access the X.509 certificate data.</param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="rawData" /> parameter is null.-or-The length of the <paramref name="rawData" /> parameter is 0.</exception>
		public X509Certificate(byte[] rawData, string password)
		{
			this.Import(rawData, password, X509KeyStorageFlags.DefaultKeySet);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class using a byte array and a password.</summary>
		/// <param name="rawData">A byte array that contains data from an X.509 certificate.</param>
		/// <param name="password">The password required to access the X.509 certificate data.</param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="rawData" /> parameter is null.-or-The length of the <paramref name="rawData" /> parameter is 0.</exception>
		[MonoTODO("SecureString support is incomplete")]
		public X509Certificate(byte[] rawData, SecureString password)
		{
			this.Import(rawData, password, X509KeyStorageFlags.DefaultKeySet);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class using a byte array, a password, and a key storage flag.</summary>
		/// <param name="rawData">A byte array containing data from an X.509 certificate. </param>
		/// <param name="password">The password required to access the X.509 certificate data. </param>
		/// <param name="keyStorageFlags">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509KeyStorageFlags" /> values. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="rawData" /> parameter is null.-or-The length of the <paramref name="rawData" /> parameter is 0.</exception>
		public X509Certificate(byte[] rawData, string password, X509KeyStorageFlags keyStorageFlags)
		{
			this.Import(rawData, password, keyStorageFlags);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class using a byte array, a password, and a key storage flag.</summary>
		/// <param name="rawData">A byte array that contains data from an X.509 certificate. </param>
		/// <param name="password">The password required to access the X.509 certificate data. </param>
		/// <param name="keyStorageFlags">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509KeyStorageFlags" /> values that controls where and how to import the private key. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="rawData" /> parameter is null.-or-The length of the <paramref name="rawData" /> parameter is 0.</exception>
		[MonoTODO("SecureString support is incomplete")]
		public X509Certificate(byte[] rawData, SecureString password, X509KeyStorageFlags keyStorageFlags)
		{
			this.Import(rawData, password, keyStorageFlags);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class using the name of a PKCS7 signed file. </summary>
		/// <param name="fileName">The name of a PKCS7 signed file.</param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="fileName" /> parameter is null.</exception>
		public X509Certificate(string fileName)
		{
			this.Import(fileName, null, X509KeyStorageFlags.DefaultKeySet);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class using the name of a PKCS7 signed file and a password to access the certificate.</summary>
		/// <param name="fileName">The name of a PKCS7 signed file. </param>
		/// <param name="password">The password required to access the X.509 certificate data. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="fileName" /> parameter is null.</exception>
		public X509Certificate(string fileName, string password)
		{
			this.Import(fileName, password, X509KeyStorageFlags.DefaultKeySet);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class using a certificate file name and a password.</summary>
		/// <param name="fileName">The name of a certificate file. </param>
		/// <param name="password">The password required to access the X.509 certificate data. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="fileName" /> parameter is null.</exception>
		[MonoTODO("SecureString support is incomplete")]
		public X509Certificate(string fileName, SecureString password)
		{
			this.Import(fileName, password, X509KeyStorageFlags.DefaultKeySet);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class using the name of a PKCS7 signed file, a password to access the certificate, and a key storage flag. </summary>
		/// <param name="fileName">The name of a PKCS7 signed file. </param>
		/// <param name="password">The password required to access the X.509 certificate data. </param>
		/// <param name="keyStorageFlags">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509KeyStorageFlags" /> values. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="fileName" /> parameter is null.</exception>
		public X509Certificate(string fileName, string password, X509KeyStorageFlags keyStorageFlags)
		{
			this.Import(fileName, password, keyStorageFlags);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class using a certificate file name, a password, and a key storage flag. </summary>
		/// <param name="fileName">The name of a certificate file. </param>
		/// <param name="password">The password required to access the X.509 certificate data. </param>
		/// <param name="keyStorageFlags">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509KeyStorageFlags" /> values that controls where and how to import the private key. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="fileName" /> parameter is null.</exception>
		[MonoTODO("SecureString support is incomplete")]
		public X509Certificate(string fileName, SecureString password, X509KeyStorageFlags keyStorageFlags)
		{
			this.Import(fileName, password, keyStorageFlags);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class using a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object and a <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that describes serialization information.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure that describes how serialization should be performed.</param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		public X509Certificate(SerializationInfo info, StreamingContext context)
		{
			byte[] rawData = (byte[])info.GetValue("RawData", typeof(byte[]));
			this.Import(rawData, null, X509KeyStorageFlags.DefaultKeySet);
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and is called back by the deserialization event when deserialization is complete.  </summary>
		/// <param name="sender">The source of the deserialization event.</param>
		void IDeserializationCallback.OnDeserialization(object sender)
		{
		}

		/// <summary>Gets serialization information with all the data needed to recreate an instance of the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object.</summary>
		/// <param name="info">The object to populate with serialization information.</param>
		/// <param name="context">The destination context of the serialization.</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("RawData", this.x509.RawData);
		}

		private string tostr(byte[] data)
		{
			if (data != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < data.Length; i++)
				{
					stringBuilder.Append(data[i].ToString("X2"));
				}
				return stringBuilder.ToString();
			}
			return null;
		}

		/// <summary>Creates an X.509v3 certificate using the name of a PKCS7 signed file.</summary>
		/// <returns>The newly created X.509 certificate.</returns>
		/// <param name="filename">The path of the PKCS7 signed file from which to create the X.509 certificate. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="filename" /> parameter is null. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Create" />
		/// </PermissionSet>
		public static X509Certificate CreateFromCertFile(string filename)
		{
			byte[] data = X509Certificate.Load(filename);
			return new X509Certificate(data);
		}

		/// <summary>Creates an X.509v3 certificate from the specified signed file.</summary>
		/// <returns>The newly created X.509 certificate.</returns>
		/// <param name="filename">The path of the signed file from which to create the X.509 certificate. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Create" />
		/// </PermissionSet>
		[MonoTODO("Incomplete - minimal validation in this version")]
		public static X509Certificate CreateFromSignedFile(string filename)
		{
			try
			{
				AuthenticodeDeformatter authenticodeDeformatter = new AuthenticodeDeformatter(filename);
				if (authenticodeDeformatter.SigningCertificate != null)
				{
					return new X509Certificate(authenticodeDeformatter.SigningCertificate.RawData);
				}
			}
			catch (SecurityException)
			{
				throw;
			}
			catch (Exception inner)
			{
				string text = Locale.GetText("Couldn't extract digital signature from {0}.", new object[]
				{
					filename
				});
				throw new COMException(text, inner);
			}
			throw new CryptographicException(Locale.GetText("{0} isn't signed.", new object[]
			{
				filename
			}));
		}

		private void InitFromHandle(IntPtr handle)
		{
			if (handle != IntPtr.Zero)
			{
				X509Certificate.CertificateContext certificateContext = (X509Certificate.CertificateContext)Marshal.PtrToStructure(handle, typeof(X509Certificate.CertificateContext));
				byte[] array = new byte[certificateContext.cbCertEncoded];
				Marshal.Copy(certificateContext.pbCertEncoded, array, 0, (int)certificateContext.cbCertEncoded);
				this.x509 = new X509Certificate(array);
			}
		}

		/// <summary>Compares two <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> objects for equality.</summary>
		/// <returns>true if the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object is equal to the object specified by the <paramref name="other" /> parameter; otherwise, false.</returns>
		/// <param name="other">An <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object to compare to the current object.</param>
		public virtual bool Equals(X509Certificate other)
		{
			if (other == null)
			{
				return false;
			}
			if (other.x509 == null)
			{
				if (this.x509 == null)
				{
					return true;
				}
				throw new CryptographicException(Locale.GetText("Certificate instance is empty."));
			}
			else
			{
				byte[] rawData = other.x509.RawData;
				if (rawData == null)
				{
					return this.x509 == null || this.x509.RawData == null;
				}
				if (this.x509 == null)
				{
					return false;
				}
				if (this.x509.RawData == null)
				{
					return false;
				}
				if (rawData.Length == this.x509.RawData.Length)
				{
					for (int i = 0; i < rawData.Length; i++)
					{
						if (rawData[i] != this.x509.RawData[i])
						{
							return false;
						}
					}
					return true;
				}
				return false;
			}
		}

		/// <summary>Returns the hash value for the X.509v3 certificate as an array of bytes.</summary>
		/// <returns>The hash value for the X.509 certificate.</returns>
		public virtual byte[] GetCertHash()
		{
			if (this.x509 == null)
			{
				throw new CryptographicException(Locale.GetText("Certificate instance is empty."));
			}
			if (this.cachedCertificateHash == null && this.x509 != null)
			{
				SHA1 sha = SHA1.Create();
				this.cachedCertificateHash = sha.ComputeHash(this.x509.RawData);
			}
			return this.cachedCertificateHash;
		}

		/// <summary>Returns the hash value for the X.509v3 certificate as a hexadecimal string.</summary>
		/// <returns>The hexadecimal string representation of the X.509 certificate hash value.</returns>
		public virtual string GetCertHashString()
		{
			return this.tostr(this.GetCertHash());
		}

		/// <summary>Returns the effective date of this X.509v3 certificate.</summary>
		/// <returns>The effective date for this X.509 certificate.</returns>
		public virtual string GetEffectiveDateString()
		{
			if (this.hideDates)
			{
				return null;
			}
			if (this.x509 == null)
			{
				throw new CryptographicException(Locale.GetText("Certificate instance is empty."));
			}
			return this.x509.ValidFrom.ToLocalTime().ToString();
		}

		/// <summary>Returns the expiration date of this X.509v3 certificate.</summary>
		/// <returns>The expiration date for this X.509 certificate.</returns>
		public virtual string GetExpirationDateString()
		{
			if (this.hideDates)
			{
				return null;
			}
			if (this.x509 == null)
			{
				throw new CryptographicException(Locale.GetText("Certificate instance is empty."));
			}
			return this.x509.ValidUntil.ToLocalTime().ToString();
		}

		/// <summary>Returns the name of the format of this X.509v3 certificate.</summary>
		/// <returns>The format of this X.509 certificate.</returns>
		public virtual string GetFormat()
		{
			return "X509";
		}

		/// <summary>Returns the hash code for the X.509v3 certificate as an integer.</summary>
		/// <returns>The hash code for the X.509 certificate as an integer.</returns>
		public override int GetHashCode()
		{
			if (this.x509 == null)
			{
				return 0;
			}
			if (this.cachedCertificateHash == null)
			{
				this.GetCertHash();
			}
			if (this.cachedCertificateHash != null && this.cachedCertificateHash.Length >= 4)
			{
				return (int)this.cachedCertificateHash[0] << 24 | (int)this.cachedCertificateHash[1] << 16 | (int)this.cachedCertificateHash[2] << 8 | (int)this.cachedCertificateHash[3];
			}
			return 0;
		}

		/// <summary>Returns the name of the certification authority that issued the X.509v3 certificate.</summary>
		/// <returns>The name of the certification authority that issued the X.509 certificate.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		[Obsolete("Use the Issuer property.")]
		public virtual string GetIssuerName()
		{
			if (this.x509 == null)
			{
				throw new CryptographicException(Locale.GetText("Certificate instance is empty."));
			}
			return this.x509.IssuerName;
		}

		/// <summary>Returns the key algorithm information for this X.509v3 certificate.</summary>
		/// <returns>The key algorithm information for this X.509 certificate as a string.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate context is invalid.</exception>
		public virtual string GetKeyAlgorithm()
		{
			if (this.x509 == null)
			{
				throw new CryptographicException(Locale.GetText("Certificate instance is empty."));
			}
			return this.x509.KeyAlgorithm;
		}

		/// <summary>Returns the key algorithm parameters for the X.509v3 certificate.</summary>
		/// <returns>The key algorithm parameters for the X.509 certificate as an array of bytes.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate context is invalid.</exception>
		public virtual byte[] GetKeyAlgorithmParameters()
		{
			if (this.x509 == null)
			{
				throw new CryptographicException(Locale.GetText("Certificate instance is empty."));
			}
			byte[] keyAlgorithmParameters = this.x509.KeyAlgorithmParameters;
			if (keyAlgorithmParameters == null)
			{
				throw new CryptographicException(Locale.GetText("Parameters not part of the certificate"));
			}
			return keyAlgorithmParameters;
		}

		/// <summary>Returns the key algorithm parameters for the X.509v3 certificate.</summary>
		/// <returns>The key algorithm parameters for the X.509 certificate as a hexadecimal string.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate context is invalid.</exception>
		public virtual string GetKeyAlgorithmParametersString()
		{
			return this.tostr(this.GetKeyAlgorithmParameters());
		}

		/// <summary>Returns the name of the principal to which the certificate was issued.</summary>
		/// <returns>The name of the principal to which the certificate was issued.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate context is invalid.</exception>
		[Obsolete("Use the Subject property.")]
		public virtual string GetName()
		{
			if (this.x509 == null)
			{
				throw new CryptographicException(Locale.GetText("Certificate instance is empty."));
			}
			return this.x509.SubjectName;
		}

		/// <summary>Returns the public key for the X.509v3 certificate.</summary>
		/// <returns>The public key for the X.509 certificate as an array of bytes.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate context is invalid.</exception>
		public virtual byte[] GetPublicKey()
		{
			if (this.x509 == null)
			{
				throw new CryptographicException(Locale.GetText("Certificate instance is empty."));
			}
			return this.x509.PublicKey;
		}

		/// <summary>Returns the public key for the X.509v3 certificate.</summary>
		/// <returns>The public key for the X.509 certificate as a hexadecimal string.</returns>
		public virtual string GetPublicKeyString()
		{
			return this.tostr(this.GetPublicKey());
		}

		/// <summary>Returns the raw data for the entire X.509v3 certificate.</summary>
		/// <returns>A byte array containing the X.509 certificate data.</returns>
		public virtual byte[] GetRawCertData()
		{
			if (this.x509 == null)
			{
				throw new CryptographicException(Locale.GetText("Certificate instance is empty."));
			}
			return this.x509.RawData;
		}

		/// <summary>Returns the raw data for the entire X.509v3 certificate.</summary>
		/// <returns>The X.509 certificate data as a hexadecimal string.</returns>
		public virtual string GetRawCertDataString()
		{
			if (this.x509 == null)
			{
				throw new CryptographicException(Locale.GetText("Certificate instance is empty."));
			}
			return this.tostr(this.x509.RawData);
		}

		/// <summary>Returns the serial number of the X.509v3 certificate.</summary>
		/// <returns>The serial number of the X.509 certificate as an array of bytes.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate context is invalid.</exception>
		public virtual byte[] GetSerialNumber()
		{
			if (this.x509 == null)
			{
				throw new CryptographicException(Locale.GetText("Certificate instance is empty."));
			}
			return this.x509.SerialNumber;
		}

		/// <summary>Returns the serial number of the X.509v3 certificate.</summary>
		/// <returns>The serial number of the X.509 certificate as a hexadecimal string.</returns>
		public virtual string GetSerialNumberString()
		{
			byte[] serialNumber = this.GetSerialNumber();
			Array.Reverse(serialNumber);
			return this.tostr(serialNumber);
		}

		/// <summary>Returns a string representation of the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object.</summary>
		/// <returns>A string representation of the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object.</returns>
		public override string ToString()
		{
			return base.ToString();
		}

		/// <summary>Returns a string representation of the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object, with extra information, if specified.</summary>
		/// <returns>A string representation of the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object.</returns>
		/// <param name="fVerbose">true to produce the verbose form of the string representation; otherwise, false. </param>
		public virtual string ToString(bool fVerbose)
		{
			if (!fVerbose || this.x509 == null)
			{
				return base.ToString();
			}
			string newLine = Environment.NewLine;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("[Subject]{0}  {1}{0}{0}", newLine, this.Subject);
			stringBuilder.AppendFormat("[Issuer]{0}  {1}{0}{0}", newLine, this.Issuer);
			stringBuilder.AppendFormat("[Not Before]{0}  {1}{0}{0}", newLine, this.GetEffectiveDateString());
			stringBuilder.AppendFormat("[Not After]{0}  {1}{0}{0}", newLine, this.GetExpirationDateString());
			stringBuilder.AppendFormat("[Thumbprint]{0}  {1}{0}", newLine, this.GetCertHashString());
			stringBuilder.Append(newLine);
			return stringBuilder.ToString();
		}

		private static byte[] Load(string fileName)
		{
			byte[] array = null;
			using (FileStream fileStream = File.OpenRead(fileName))
			{
				array = new byte[fileStream.Length];
				fileStream.Read(array, 0, array.Length);
				fileStream.Close();
			}
			return array;
		}

		/// <summary>Gets the name of the certificate authority that issued the X.509v3 certificate.</summary>
		/// <returns>The name of the certificate authority that issued the X.509v3 certificate.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate handle is invalid.</exception>
		public string Issuer
		{
			get
			{
				if (this.x509 == null)
				{
					throw new CryptographicException(Locale.GetText("Certificate instance is empty."));
				}
				if (this.issuer_name == null)
				{
					this.issuer_name = X501.ToString(this.x509.GetIssuerName(), true, ", ", true);
				}
				return this.issuer_name;
			}
		}

		/// <summary>Gets the subject distinguished name from the certificate.</summary>
		/// <returns>The subject distinguished name from the certificate.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate handle is invalid.</exception>
		public string Subject
		{
			get
			{
				if (this.x509 == null)
				{
					throw new CryptographicException(Locale.GetText("Certificate instance is empty."));
				}
				if (this.subject_name == null)
				{
					this.subject_name = X501.ToString(this.x509.GetSubjectName(), true, ", ", true);
				}
				return this.subject_name;
			}
		}

		/// <summary>Gets a handle to a Microsoft Cryptographic API certificate context described by an unmanaged PCCERT_CONTEXT structure. </summary>
		/// <returns>An <see cref="T:System.IntPtr" /> structure that represents an unmanaged PCCERT_CONTEXT structure.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[ComVisible(false)]
		public IntPtr Handle
		{
			get
			{
				return IntPtr.Zero;
			}
		}

		/// <summary>Compares two <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> objects for equality.</summary>
		/// <returns>true if the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object is equal to the object specified by the <paramref name="other" /> parameter; otherwise, false.</returns>
		/// <param name="obj">An <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object to compare to the current object. </param>
		[ComVisible(false)]
		public override bool Equals(object obj)
		{
			X509Certificate x509Certificate = obj as X509Certificate;
			return x509Certificate != null && this.Equals(x509Certificate);
		}

		/// <summary>Exports the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object to a byte array in a format described by one of the <see cref="T:System.Security.Cryptography.X509Certificates.X509ContentType" /> values. </summary>
		/// <returns>An array of bytes that represents the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object.</returns>
		/// <param name="contentType">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509ContentType" /> values that describes how to format the output data. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">A value other than <see cref="F:System.Security.Cryptography.X509Certificates.X509ContentType.Cert" />, <see cref="F:System.Security.Cryptography.X509Certificates.X509ContentType.SerializedCert" />, or <see cref="F:System.Security.Cryptography.X509Certificates.X509ContentType.Pkcs12" /> was passed to the <paramref name="contentType" /> parameter.-or-The certificate could not be exported.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Open, Export" />
		/// </PermissionSet>
		[ComVisible(false)]
		[MonoTODO("X509ContentType.Pfx/Pkcs12 and SerializedCert are not supported")]
		public virtual byte[] Export(X509ContentType contentType)
		{
			return this.Export(contentType, null);
		}

		/// <summary>Exports the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object to a byte array in a format described by one of the <see cref="T:System.Security.Cryptography.X509Certificates.X509ContentType" /> values, and using the specified password.</summary>
		/// <returns>An array of bytes that represents the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object.</returns>
		/// <param name="contentType">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509ContentType" /> values that describes how to format the output data.</param>
		/// <param name="password">The password required to access the X.509 certificate data.</param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">A value other than <see cref="F:System.Security.Cryptography.X509Certificates.X509ContentType.Cert" />, <see cref="F:System.Security.Cryptography.X509Certificates.X509ContentType.SerializedCert" />, or <see cref="F:System.Security.Cryptography.X509Certificates.X509ContentType.Pkcs12" /> was passed to the <paramref name="contentType" /> parameter.-or-The certificate could not be exported.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Open, Export" />
		/// </PermissionSet>
		[ComVisible(false)]
		[MonoTODO("X509ContentType.Pfx/Pkcs12 and SerializedCert are not supported")]
		public virtual byte[] Export(X509ContentType contentType, string password)
		{
			byte[] password2 = (password != null) ? Encoding.UTF8.GetBytes(password) : null;
			return this.Export(contentType, password2);
		}

		/// <summary>Exports the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object to a byte array using the specified format and a password.</summary>
		/// <returns>A byte array that represents the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object.</returns>
		/// <param name="contentType">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509ContentType" /> values that describes how to format the output data.</param>
		/// <param name="password">The password required to access the X.509 certificate data.</param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">A value other than <see cref="F:System.Security.Cryptography.X509Certificates.X509ContentType.Cert" />, <see cref="F:System.Security.Cryptography.X509Certificates.X509ContentType.SerializedCert" />, or <see cref="F:System.Security.Cryptography.X509Certificates.X509ContentType.Pkcs12" /> was passed to the <paramref name="contentType" /> parameter.-or-The certificate could not be exported.</exception>
		[MonoTODO("X509ContentType.Pfx/Pkcs12 and SerializedCert are not supported. SecureString support is incomplete.")]
		public virtual byte[] Export(X509ContentType contentType, SecureString password)
		{
			byte[] password2 = (password != null) ? password.GetBuffer() : null;
			return this.Export(contentType, password2);
		}

		internal byte[] Export(X509ContentType contentType, byte[] password)
		{
			if (this.x509 == null)
			{
				throw new CryptographicException(Locale.GetText("Certificate instance is empty."));
			}
			byte[] rawData;
			try
			{
				switch (contentType)
				{
				case X509ContentType.Cert:
					rawData = this.x509.RawData;
					break;
				case X509ContentType.SerializedCert:
					throw new NotSupportedException();
				case X509ContentType.Pfx:
					throw new NotSupportedException();
				default:
				{
					string text = Locale.GetText("This certificate format '{0}' cannot be exported.", new object[]
					{
						contentType
					});
					throw new CryptographicException(text);
				}
				}
			}
			finally
			{
				if (password != null)
				{
					Array.Clear(password, 0, password.Length);
				}
			}
			return rawData;
		}

		/// <summary>Populates the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object with data from a byte array.</summary>
		/// <param name="rawData">A byte array containing data from an X.509 certificate. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="rawData" /> parameter is null.-or-The length of the <paramref name="rawData" /> parameter is 0.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Create" />
		/// </PermissionSet>
		[ComVisible(false)]
		public virtual void Import(byte[] rawData)
		{
			this.Import(rawData, null, X509KeyStorageFlags.DefaultKeySet);
		}

		/// <summary>Populates the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object using data from a byte array, a password, and flags for determining how the private key is imported.</summary>
		/// <param name="rawData">A byte array containing data from an X.509 certificate. </param>
		/// <param name="password">The password required to access the X.509 certificate data. </param>
		/// <param name="keyStorageFlags">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509KeyStorageFlags" /> values that controls where and how the private key is imported. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="rawData" /> parameter is null.-or-The length of the <paramref name="rawData" /> parameter is 0.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Create" />
		/// </PermissionSet>
		[MonoTODO("missing KeyStorageFlags support")]
		[ComVisible(false)]
		public virtual void Import(byte[] rawData, string password, X509KeyStorageFlags keyStorageFlags)
		{
			this.Reset();
			if (password == null)
			{
				try
				{
					this.x509 = new X509Certificate(rawData);
				}
				catch (Exception inner)
				{
					try
					{
						PKCS12 pkcs = new PKCS12(rawData);
						if (pkcs.Certificates.Count > 0)
						{
							this.x509 = pkcs.Certificates[0];
						}
						else
						{
							this.x509 = null;
						}
					}
					catch
					{
						string text = Locale.GetText("Unable to decode certificate.");
						throw new CryptographicException(text, inner);
					}
				}
			}
			else
			{
				try
				{
					PKCS12 pkcs2 = new PKCS12(rawData, password);
					if (pkcs2.Certificates.Count > 0)
					{
						this.x509 = pkcs2.Certificates[0];
					}
					else
					{
						this.x509 = null;
					}
				}
				catch
				{
					this.x509 = new X509Certificate(rawData);
				}
			}
		}

		/// <summary>Populates an <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object using data from a byte array, a password, and a key storage flag.</summary>
		/// <param name="rawData">A byte array that contains data from an X.509 certificate. </param>
		/// <param name="password">The password required to access the X.509 certificate data. </param>
		/// <param name="keyStorageFlags">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509KeyStorageFlags" /> values that controls where and how to import the private key. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="rawData" /> parameter is null.-or-The length of the <paramref name="rawData" /> parameter is 0.</exception>
		[MonoTODO("SecureString support is incomplete")]
		public virtual void Import(byte[] rawData, SecureString password, X509KeyStorageFlags keyStorageFlags)
		{
			this.Import(rawData, null, keyStorageFlags);
		}

		/// <summary>Populates the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object with information from a certificate file.</summary>
		/// <param name="fileName">The name of a certificate file represented as a string. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="fileName" /> parameter is null.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Create" />
		/// </PermissionSet>
		[ComVisible(false)]
		public virtual void Import(string fileName)
		{
			byte[] rawData = X509Certificate.Load(fileName);
			this.Import(rawData, null, X509KeyStorageFlags.DefaultKeySet);
		}

		/// <summary>Populates the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object with information from a certificate file, a password, and a <see cref="T:System.Security.Cryptography.X509Certificates.X509KeyStorageFlags" /> value.</summary>
		/// <param name="fileName">The name of a certificate file represented as a string. </param>
		/// <param name="password">The password required to access the X.509 certificate data. </param>
		/// <param name="keyStorageFlags">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509KeyStorageFlags" /> values that controls where and how the private key is imported. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="fileName" /> parameter is null.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Create" />
		/// </PermissionSet>
		[MonoTODO("missing KeyStorageFlags support")]
		[ComVisible(false)]
		public virtual void Import(string fileName, string password, X509KeyStorageFlags keyStorageFlags)
		{
			byte[] rawData = X509Certificate.Load(fileName);
			this.Import(rawData, password, keyStorageFlags);
		}

		/// <summary>Populates an <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object with information from a certificate file, a password, and a key storage flag.</summary>
		/// <param name="fileName">The name of a certificate file. </param>
		/// <param name="password">The password required to access the X.509 certificate data. </param>
		/// <param name="keyStorageFlags">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509KeyStorageFlags" /> values that controls where and how to import the private key. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="fileName" /> parameter is null.</exception>
		[MonoTODO("SecureString support is incomplete, missing KeyStorageFlags support")]
		public virtual void Import(string fileName, SecureString password, X509KeyStorageFlags keyStorageFlags)
		{
			byte[] rawData = X509Certificate.Load(fileName);
			this.Import(rawData, null, keyStorageFlags);
		}

		/// <summary>Resets the state of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> object.</summary>
		[ComVisible(false)]
		public virtual void Reset()
		{
			this.x509 = null;
			this.issuer_name = null;
			this.subject_name = null;
			this.hideDates = false;
			this.cachedCertificateHash = null;
		}

		internal struct CertificateContext
		{
			public uint dwCertEncodingType;

			public IntPtr pbCertEncoded;

			public uint cbCertEncoded;

			public IntPtr pCertInfo;

			public IntPtr hCertStore;
		}
	}
}
