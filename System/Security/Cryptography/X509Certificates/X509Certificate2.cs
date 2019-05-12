using Mono.Security;
using Mono.Security.Cryptography;
using Mono.Security.X509;
using System;
using System.IO;
using System.Text;

namespace System.Security.Cryptography.X509Certificates
{
	/// <summary>Represents an X.509 certificate. This class can be inherited.</summary>
	public class X509Certificate2 : X509Certificate
	{
		private bool _archived;

		private X509ExtensionCollection _extensions;

		private string _name = string.Empty;

		private string _serial;

		private PublicKey _publicKey;

		private X500DistinguishedName issuer_name;

		private X500DistinguishedName subject_name;

		private Oid signature_algorithm;

		private X509Certificate _cert;

		private static string empty_error = Locale.GetText("Certificate instance is empty.");

		private static byte[] commonName = new byte[]
		{
			85,
			4,
			3
		};

		private static byte[] email = new byte[]
		{
			42,
			134,
			72,
			134,
			247,
			13,
			1,
			9,
			1
		};

		private static byte[] signedData = new byte[]
		{
			42,
			134,
			72,
			134,
			247,
			13,
			1,
			7,
			2
		};

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> class.</summary>
		public X509Certificate2()
		{
			this._cert = null;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> class using information from a byte array.</summary>
		/// <param name="rawData">A byte array containing data from an X.509 certificate. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		public X509Certificate2(byte[] rawData)
		{
			this.Import(rawData, null, X509KeyStorageFlags.DefaultKeySet);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> class using a byte array and a password.</summary>
		/// <param name="rawData">A byte array containing data from an X.509 certificate. </param>
		/// <param name="password">The password required to access the X.509 certificate data. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		public X509Certificate2(byte[] rawData, string password)
		{
			this.Import(rawData, password, X509KeyStorageFlags.DefaultKeySet);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> class using a byte array and a password.</summary>
		/// <param name="rawData">A byte array that contains data from an X.509 certificate. </param>
		/// <param name="password">The password required to access the X.509 certificate data. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		public X509Certificate2(byte[] rawData, SecureString password)
		{
			this.Import(rawData, password, X509KeyStorageFlags.DefaultKeySet);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> class using a byte array, a password, and a key storage flag.</summary>
		/// <param name="rawData">A byte array containing data from an X.509 certificate. </param>
		/// <param name="password">The password required to access the X.509 certificate data. </param>
		/// <param name="keyStorageFlags">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509KeyStorageFlags" /> values. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		public X509Certificate2(byte[] rawData, string password, X509KeyStorageFlags keyStorageFlags)
		{
			this.Import(rawData, password, keyStorageFlags);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> class using a byte array, a password, and a key storage flag.</summary>
		/// <param name="rawData">A byte array that contains data from an X.509 certificate. </param>
		/// <param name="password">The password required to access the X.509 certificate data. </param>
		/// <param name="keyStorageFlags">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509KeyStorageFlags" /> values that controls where and how to import the private key. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		public X509Certificate2(byte[] rawData, SecureString password, X509KeyStorageFlags keyStorageFlags)
		{
			this.Import(rawData, password, keyStorageFlags);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> class using a certificate file name.</summary>
		/// <param name="fileName">The name of a certificate file. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		public X509Certificate2(string fileName)
		{
			this.Import(fileName, string.Empty, X509KeyStorageFlags.DefaultKeySet);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> class using a certificate file name and a password used to access the certificate.</summary>
		/// <param name="fileName">The name of a certificate file. </param>
		/// <param name="password">The password required to access the X.509 certificate data. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		public X509Certificate2(string fileName, string password)
		{
			this.Import(fileName, password, X509KeyStorageFlags.DefaultKeySet);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> class using a certificate file name and a password.</summary>
		/// <param name="fileName">The name of a certificate file. </param>
		/// <param name="password">The password required to access the X.509 certificate data. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		public X509Certificate2(string fileName, SecureString password)
		{
			this.Import(fileName, password, X509KeyStorageFlags.DefaultKeySet);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> class using a certificate file name, a password used to access the certificate, and a key storage flag.</summary>
		/// <param name="fileName">The name of a certificate file. </param>
		/// <param name="password">The password required to access the X.509 certificate data. </param>
		/// <param name="keyStorageFlags">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509KeyStorageFlags" /> values. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		public X509Certificate2(string fileName, string password, X509KeyStorageFlags keyStorageFlags)
		{
			this.Import(fileName, password, keyStorageFlags);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> class using a certificate file name, a password, and a key storage flag.</summary>
		/// <param name="fileName">The name of a certificate file. </param>
		/// <param name="password">The password required to access the X.509 certificate data. </param>
		/// <param name="keyStorageFlags">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509KeyStorageFlags" /> values that controls where and how to import the private key.. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		public X509Certificate2(string fileName, SecureString password, X509KeyStorageFlags keyStorageFlags)
		{
			this.Import(fileName, password, keyStorageFlags);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> class using an unmanaged handle.</summary>
		/// <param name="handle">A pointer to a certificate context in unmanaged code. The C structure is called PCCERT_CONTEXT.</param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		public X509Certificate2(IntPtr handle) : base(handle)
		{
			this._cert = new X509Certificate(base.GetRawCertData());
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> class using an <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object.</summary>
		/// <param name="certificate">An <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object.</param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:The certificate file does not exist.The certificate is invalid.The certificate's password is incorrect.</exception>
		public X509Certificate2(X509Certificate certificate) : base(certificate)
		{
			this._cert = new X509Certificate(base.GetRawCertData());
		}

		/// <summary>Gets or sets a value indicating that an X.509 certificate is archived.</summary>
		/// <returns>true if the certificate is archived, false if the certificate is not archived.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate is unreadable. </exception>
		public bool Archived
		{
			get
			{
				if (this._cert == null)
				{
					throw new CryptographicException(X509Certificate2.empty_error);
				}
				return this._archived;
			}
			set
			{
				if (this._cert == null)
				{
					throw new CryptographicException(X509Certificate2.empty_error);
				}
				this._archived = value;
			}
		}

		/// <summary>Gets a collection of <see cref="T:System.Security.Cryptography.X509Certificates.X509Extension" /> objects.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.X509Certificates.X509ExtensionCollection" /> object.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate is unreadable. </exception>
		public X509ExtensionCollection Extensions
		{
			get
			{
				if (this._cert == null)
				{
					throw new CryptographicException(X509Certificate2.empty_error);
				}
				if (this._extensions == null)
				{
					this._extensions = new X509ExtensionCollection(this._cert);
				}
				return this._extensions;
			}
		}

		/// <summary>Gets or sets the associated alias for a certificate.</summary>
		/// <returns>The certificate's friendly name.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate is unreadable. </exception>
		public string FriendlyName
		{
			get
			{
				if (this._cert == null)
				{
					throw new CryptographicException(X509Certificate2.empty_error);
				}
				return this._name;
			}
			set
			{
				if (this._cert == null)
				{
					throw new CryptographicException(X509Certificate2.empty_error);
				}
				this._name = value;
			}
		}

		/// <summary>Gets a value that indicates whether an <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> object contains a private key. </summary>
		/// <returns>true if the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> object contains a private key; otherwise, false. </returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate context is invalid.</exception>
		public bool HasPrivateKey
		{
			get
			{
				return this.PrivateKey != null;
			}
		}

		/// <summary>Gets the distinguished name of the certificate issuer.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.X509Certificates.X500DistinguishedName" /> object that contains the name of the certificate issuer.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate context is invalid.</exception>
		public X500DistinguishedName IssuerName
		{
			get
			{
				if (this._cert == null)
				{
					throw new CryptographicException(X509Certificate2.empty_error);
				}
				if (this.issuer_name == null)
				{
					this.issuer_name = new X500DistinguishedName(this._cert.GetIssuerName().GetBytes());
				}
				return this.issuer_name;
			}
		}

		/// <summary>Gets the date in local time after which a certificate is no longer valid.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> object that represents the expiration date for the certificate .</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate is unreadable. </exception>
		public DateTime NotAfter
		{
			get
			{
				if (this._cert == null)
				{
					throw new CryptographicException(X509Certificate2.empty_error);
				}
				return this._cert.ValidUntil.ToLocalTime();
			}
		}

		/// <summary>Gets the date in local time on which a certificate becomes valid.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> object that represents the effective date of the certificate.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate is unreadable. </exception>
		public DateTime NotBefore
		{
			get
			{
				if (this._cert == null)
				{
					throw new CryptographicException(X509Certificate2.empty_error);
				}
				return this._cert.ValidFrom.ToLocalTime();
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Security.Cryptography.AsymmetricAlgorithm" /> object that represents the private key associated with a certificate.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.AsymmetricAlgorithm" /> object, which is either an RSA or DSA cryptographic service provider.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The key value is not an RSA or DSA key, or the key is unreadable. </exception>
		/// <exception cref="T:System.ArgumentNullException">The value being set for this property is null.</exception>
		/// <exception cref="T:System.NotSupportedException">The key algorithm for this private key is not supported.</exception>
		/// <exception cref="T:System.Security.Cryptography.CryptographicUnexpectedOperationException">The X.509 keys do not match.</exception>
		/// <exception cref="T:System.ArgumentException">The cryptographic service provider key is null.</exception>
		public AsymmetricAlgorithm PrivateKey
		{
			get
			{
				if (this._cert == null)
				{
					throw new CryptographicException(X509Certificate2.empty_error);
				}
				try
				{
					if (this._cert.RSA != null)
					{
						RSACryptoServiceProvider rsacryptoServiceProvider = this._cert.RSA as RSACryptoServiceProvider;
						if (rsacryptoServiceProvider != null)
						{
							return (!rsacryptoServiceProvider.PublicOnly) ? rsacryptoServiceProvider : null;
						}
						RSAManaged rsamanaged = this._cert.RSA as RSAManaged;
						if (rsamanaged != null)
						{
							return (!rsamanaged.PublicOnly) ? rsamanaged : null;
						}
						this._cert.RSA.ExportParameters(true);
						return this._cert.RSA;
					}
					else if (this._cert.DSA != null)
					{
						DSACryptoServiceProvider dsacryptoServiceProvider = this._cert.DSA as DSACryptoServiceProvider;
						if (dsacryptoServiceProvider != null)
						{
							return (!dsacryptoServiceProvider.PublicOnly) ? dsacryptoServiceProvider : null;
						}
						this._cert.DSA.ExportParameters(true);
						return this._cert.DSA;
					}
				}
				catch
				{
				}
				return null;
			}
			set
			{
				if (this._cert == null)
				{
					throw new CryptographicException(X509Certificate2.empty_error);
				}
				if (value == null)
				{
					this._cert.RSA = null;
					this._cert.DSA = null;
				}
				else if (value is RSA)
				{
					this._cert.RSA = (RSA)value;
				}
				else
				{
					if (!(value is DSA))
					{
						throw new NotSupportedException();
					}
					this._cert.DSA = (DSA)value;
				}
			}
		}

		/// <summary>Gets a <see cref="P:System.Security.Cryptography.X509Certificates.X509Certificate2.PublicKey" /> object associated with a certificate.</summary>
		/// <returns>A <see cref="P:System.Security.Cryptography.X509Certificates.X509Certificate2.PublicKey" /> object.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The key value is not an RSA or DSA key, or the key is unreadable. </exception>
		public PublicKey PublicKey
		{
			get
			{
				if (this._cert == null)
				{
					throw new CryptographicException(X509Certificate2.empty_error);
				}
				if (this._publicKey == null)
				{
					try
					{
						this._publicKey = new PublicKey(this._cert);
					}
					catch (Exception inner)
					{
						string text = Locale.GetText("Unable to decode public key.");
						throw new CryptographicException(text, inner);
					}
				}
				return this._publicKey;
			}
		}

		/// <summary>Gets the raw data of a certificate.</summary>
		/// <returns>The raw data of the certificate as a byte array.</returns>
		public byte[] RawData
		{
			get
			{
				if (this._cert == null)
				{
					throw new CryptographicException(X509Certificate2.empty_error);
				}
				return base.GetRawCertData();
			}
		}

		/// <summary>Gets the serial number of a certificate.</summary>
		/// <returns>The serial number of the certificate.</returns>
		public string SerialNumber
		{
			get
			{
				if (this._cert == null)
				{
					throw new CryptographicException(X509Certificate2.empty_error);
				}
				if (this._serial == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					byte[] serialNumber = this._cert.SerialNumber;
					for (int i = serialNumber.Length - 1; i >= 0; i--)
					{
						stringBuilder.Append(serialNumber[i].ToString("X2"));
					}
					this._serial = stringBuilder.ToString();
				}
				return this._serial;
			}
		}

		/// <summary>Gets the algorithm used to create the signature of a certificate.</summary>
		/// <returns>Returns the object identifier (<see cref="T:System.Security.Cryptography.Oid" />) of the signature algorithm.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate is unreadable. </exception>
		public Oid SignatureAlgorithm
		{
			get
			{
				if (this._cert == null)
				{
					throw new CryptographicException(X509Certificate2.empty_error);
				}
				if (this.signature_algorithm == null)
				{
					this.signature_algorithm = new Oid(this._cert.SignatureAlgorithm);
				}
				return this.signature_algorithm;
			}
		}

		/// <summary>Gets the subject distinguished name from a certificate.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.X509Certificates.X500DistinguishedName" /> object that represents the name of the certificate subject.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate context is invalid.</exception>
		public X500DistinguishedName SubjectName
		{
			get
			{
				if (this._cert == null)
				{
					throw new CryptographicException(X509Certificate2.empty_error);
				}
				if (this.subject_name == null)
				{
					this.subject_name = new X500DistinguishedName(this._cert.GetSubjectName().GetBytes());
				}
				return this.subject_name;
			}
		}

		/// <summary>Gets the thumbprint of a certificate.</summary>
		/// <returns>The thumbprint of the certificate.</returns>
		public string Thumbprint
		{
			get
			{
				return base.GetCertHashString();
			}
		}

		/// <summary>Gets the X.509 format version of a certificate.</summary>
		/// <returns>The certificate format.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate is unreadable. </exception>
		public int Version
		{
			get
			{
				if (this._cert == null)
				{
					throw new CryptographicException(X509Certificate2.empty_error);
				}
				return this._cert.Version;
			}
		}

		/// <summary>Gets the subject and issuer names from a certificate.</summary>
		/// <returns>The name of the certificate.</returns>
		/// <param name="nameType">The <see cref="T:System.Security.Cryptography.X509Certificates.X509NameType" /> value for the subject. </param>
		/// <param name="forIssuer">true to include the issuer name; otherwise, false. </param>
		[MonoTODO("always return String.Empty for UpnName, DnsFromAlternativeName and UrlName")]
		public string GetNameInfo(X509NameType nameType, bool forIssuer)
		{
			switch (nameType)
			{
			case X509NameType.SimpleName:
			{
				if (this._cert == null)
				{
					throw new CryptographicException(X509Certificate2.empty_error);
				}
				ASN1 asn = (!forIssuer) ? this._cert.GetSubjectName() : this._cert.GetIssuerName();
				ASN1 asn2 = this.Find(X509Certificate2.commonName, asn);
				if (asn2 != null)
				{
					return this.GetValueAsString(asn2);
				}
				if (asn.Count == 0)
				{
					return string.Empty;
				}
				ASN1 asn3 = asn[asn.Count - 1];
				if (asn3.Count == 0)
				{
					return string.Empty;
				}
				return this.GetValueAsString(asn3[0]);
			}
			case X509NameType.EmailName:
			{
				ASN1 asn4 = this.Find(X509Certificate2.email, (!forIssuer) ? this._cert.GetSubjectName() : this._cert.GetIssuerName());
				if (asn4 != null)
				{
					return this.GetValueAsString(asn4);
				}
				return string.Empty;
			}
			case X509NameType.UpnName:
				return string.Empty;
			case X509NameType.DnsName:
			{
				ASN1 asn5 = this.Find(X509Certificate2.commonName, (!forIssuer) ? this._cert.GetSubjectName() : this._cert.GetIssuerName());
				if (asn5 != null)
				{
					return this.GetValueAsString(asn5);
				}
				return string.Empty;
			}
			case X509NameType.DnsFromAlternativeName:
				return string.Empty;
			case X509NameType.UrlName:
				return string.Empty;
			default:
				throw new ArgumentException("nameType");
			}
		}

		private ASN1 Find(byte[] oid, ASN1 dn)
		{
			if (dn.Count == 0)
			{
				return null;
			}
			for (int i = 0; i < dn.Count; i++)
			{
				ASN1 asn = dn[i];
				for (int j = 0; j < asn.Count; j++)
				{
					ASN1 asn2 = asn[j];
					if (asn2.Count == 2)
					{
						ASN1 asn3 = asn2[0];
						if (asn3 != null)
						{
							if (asn3.CompareValue(oid))
							{
								return asn2;
							}
						}
					}
				}
			}
			return null;
		}

		private string GetValueAsString(ASN1 pair)
		{
			if (pair.Count != 2)
			{
				return string.Empty;
			}
			ASN1 asn = pair[1];
			if (asn.Value == null || asn.Length == 0)
			{
				return string.Empty;
			}
			if (asn.Tag == 30)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 1; i < asn.Value.Length; i += 2)
				{
					stringBuilder.Append((char)asn.Value[i]);
				}
				return stringBuilder.ToString();
			}
			return Encoding.UTF8.GetString(asn.Value);
		}

		private void ImportPkcs12(byte[] rawData, string password)
		{
			PKCS12 pkcs = (password != null) ? new PKCS12(rawData, password) : new PKCS12(rawData);
			if (pkcs.Certificates.Count > 0)
			{
				this._cert = pkcs.Certificates[0];
			}
			else
			{
				this._cert = null;
			}
			if (pkcs.Keys.Count > 0)
			{
				this._cert.RSA = (pkcs.Keys[0] as RSA);
				this._cert.DSA = (pkcs.Keys[0] as DSA);
			}
		}

		/// <summary>Populates an <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> object with data from a byte array.</summary>
		/// <param name="rawData">A byte array containing data from an X.509 certificate. </param>
		public override void Import(byte[] rawData)
		{
			this.Import(rawData, null, X509KeyStorageFlags.DefaultKeySet);
		}

		/// <summary>Populates an <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> object using data from a byte array, a password, and flags for determining how to import the private key.</summary>
		/// <param name="rawData">A byte array containing data from an X.509 certificate. </param>
		/// <param name="password">The password required to access the X.509 certificate data. </param>
		/// <param name="keyStorageFlags">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509KeyStorageFlags" /> values used to control where and how to import the private key. </param>
		[MonoTODO("missing KeyStorageFlags support")]
		public override void Import(byte[] rawData, string password, X509KeyStorageFlags keyStorageFlags)
		{
			base.Import(rawData, password, keyStorageFlags);
			if (password == null)
			{
				try
				{
					this._cert = new X509Certificate(rawData);
				}
				catch (Exception inner)
				{
					try
					{
						this.ImportPkcs12(rawData, null);
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
					this.ImportPkcs12(rawData, password);
				}
				catch
				{
					this._cert = new X509Certificate(rawData);
				}
			}
		}

		/// <summary>Populates an <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> object using data from a byte array, a password, and a key storage flag.</summary>
		/// <param name="rawData">A byte array that contains data from an X.509 certificate. </param>
		/// <param name="password">The password required to access the X.509 certificate data. </param>
		/// <param name="keyStorageFlags">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509KeyStorageFlags" /> values that controls where and how to import the private key. </param>
		[MonoTODO("SecureString is incomplete")]
		public override void Import(byte[] rawData, SecureString password, X509KeyStorageFlags keyStorageFlags)
		{
			this.Import(rawData, null, keyStorageFlags);
		}

		/// <summary>Populates an <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> object with information from a certificate file.</summary>
		/// <param name="fileName">The name of a certificate. </param>
		public override void Import(string fileName)
		{
			byte[] rawData = X509Certificate2.Load(fileName);
			this.Import(rawData, null, X509KeyStorageFlags.DefaultKeySet);
		}

		/// <summary>Populates an <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> object with information from a certificate file, a password, and a <see cref="T:System.Security.Cryptography.X509Certificates.X509KeyStorageFlags" /> value.</summary>
		/// <param name="fileName">The name of a certificate file. </param>
		/// <param name="password">The password required to access the X.509 certificate data. </param>
		/// <param name="keyStorageFlags">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509KeyStorageFlags" /> values used to control where and how to import the private key. </param>
		[MonoTODO("missing KeyStorageFlags support")]
		public override void Import(string fileName, string password, X509KeyStorageFlags keyStorageFlags)
		{
			byte[] rawData = X509Certificate2.Load(fileName);
			this.Import(rawData, password, keyStorageFlags);
		}

		/// <summary>Populates an <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> object with information from a certificate file, a password, and a key storage flag.</summary>
		/// <param name="fileName">The name of a certificate file. </param>
		/// <param name="password">The password required to access the X.509 certificate data. </param>
		/// <param name="keyStorageFlags">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509KeyStorageFlags" /> values that controls where and how to import the private key. </param>
		[MonoTODO("SecureString is incomplete")]
		public override void Import(string fileName, SecureString password, X509KeyStorageFlags keyStorageFlags)
		{
			byte[] rawData = X509Certificate2.Load(fileName);
			this.Import(rawData, null, keyStorageFlags);
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

		/// <summary>Resets the state of an <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> object.</summary>
		public override void Reset()
		{
			this._cert = null;
			this._archived = false;
			this._extensions = null;
			this._name = string.Empty;
			this._serial = null;
			this._publicKey = null;
			this.issuer_name = null;
			this.subject_name = null;
			this.signature_algorithm = null;
			base.Reset();
		}

		/// <summary>Displays an X.509 certificate in text format.</summary>
		/// <returns>The certificate information.</returns>
		public override string ToString()
		{
			if (this._cert == null)
			{
				return "System.Security.Cryptography.X509Certificates.X509Certificate2";
			}
			return base.ToString(true);
		}

		/// <summary>Displays an X.509 certificate in text format.</summary>
		/// <returns>The certificate information.</returns>
		/// <param name="verbose">true to display the public key, private key, extensions, and so forth; false to display information that is similar to the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> class, including thumbprint, serial number, subject and issuer names, and so on. </param>
		public override string ToString(bool verbose)
		{
			if (this._cert == null)
			{
				return "System.Security.Cryptography.X509Certificates.X509Certificate2";
			}
			if (!verbose)
			{
				return base.ToString(true);
			}
			string newLine = Environment.NewLine;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("[Version]{0}  V{1}{0}{0}", newLine, this.Version);
			stringBuilder.AppendFormat("[Subject]{0}  {1}{0}{0}", newLine, base.Subject);
			stringBuilder.AppendFormat("[Issuer]{0}  {1}{0}{0}", newLine, base.Issuer);
			stringBuilder.AppendFormat("[Serial Number]{0}  {1}{0}{0}", newLine, this.SerialNumber);
			stringBuilder.AppendFormat("[Not Before]{0}  {1}{0}{0}", newLine, this.NotBefore);
			stringBuilder.AppendFormat("[Not After]{0}  {1}{0}{0}", newLine, this.NotAfter);
			stringBuilder.AppendFormat("[Thumbprint]{0}  {1}{0}{0}", newLine, this.Thumbprint);
			stringBuilder.AppendFormat("[Signature Algorithm]{0}  {1}({2}){0}{0}", newLine, this.SignatureAlgorithm.FriendlyName, this.SignatureAlgorithm.Value);
			AsymmetricAlgorithm key = this.PublicKey.Key;
			stringBuilder.AppendFormat("[Public Key]{0}  Algorithm: ", newLine);
			if (key is RSA)
			{
				stringBuilder.Append("RSA");
			}
			else if (key is DSA)
			{
				stringBuilder.Append("DSA");
			}
			else
			{
				stringBuilder.Append(key.ToString());
			}
			stringBuilder.AppendFormat("{0}  Length: {1}{0}  Key Blob: ", newLine, key.KeySize);
			X509Certificate2.AppendBuffer(stringBuilder, this.PublicKey.EncodedKeyValue.RawData);
			stringBuilder.AppendFormat("{0}  Parameters: ", newLine);
			X509Certificate2.AppendBuffer(stringBuilder, this.PublicKey.EncodedParameters.RawData);
			stringBuilder.Append(newLine);
			return stringBuilder.ToString();
		}

		private static void AppendBuffer(StringBuilder sb, byte[] buffer)
		{
			if (buffer == null)
			{
				return;
			}
			for (int i = 0; i < buffer.Length; i++)
			{
				sb.Append(buffer[i].ToString("x2"));
				if (i < buffer.Length - 1)
				{
					sb.Append(" ");
				}
			}
		}

		/// <summary>Performs a X.509 chain validation using basic validation policy.</summary>
		/// <returns>true if the validation succeeds; false if the validation fails.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate is unreadable. </exception>
		[MonoTODO("by default this depends on the incomplete X509Chain")]
		public bool Verify()
		{
			if (this._cert == null)
			{
				throw new CryptographicException(X509Certificate2.empty_error);
			}
			X509Chain x509Chain = (X509Chain)CryptoConfig.CreateFromName("X509Chain");
			return x509Chain.Build(this);
		}

		/// <summary>Indicates the type of certificate contained in a byte array.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.X509Certificates.X509ContentType" /> object.</returns>
		/// <param name="rawData">A byte array containing data from an X.509 certificate. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="rawData" /> has a zero length or is null. </exception>
		[MonoTODO("Detection limited to Cert, Pfx, Pkcs12, Pkcs7 and Unknown")]
		public static X509ContentType GetCertContentType(byte[] rawData)
		{
			if (rawData == null || rawData.Length == 0)
			{
				throw new ArgumentException("rawData");
			}
			X509ContentType result = X509ContentType.Unknown;
			try
			{
				ASN1 asn = new ASN1(rawData);
				if (asn.Tag != 48)
				{
					string text = Locale.GetText("Unable to decode certificate.");
					throw new CryptographicException(text);
				}
				if (asn.Count == 0)
				{
					return result;
				}
				if (asn.Count == 3)
				{
					byte tag = asn[0].Tag;
					if (tag != 2)
					{
						if (tag == 48)
						{
							if (asn[1].Tag == 48 && asn[2].Tag == 3)
							{
								result = X509ContentType.Cert;
							}
						}
					}
					else if (asn[1].Tag == 48 && asn[2].Tag == 48)
					{
						result = X509ContentType.Pfx;
					}
				}
				if (asn[0].Tag == 6 && asn[0].CompareValue(X509Certificate2.signedData))
				{
					result = X509ContentType.Pkcs7;
				}
			}
			catch (Exception inner)
			{
				string text2 = Locale.GetText("Unable to decode certificate.");
				throw new CryptographicException(text2, inner);
			}
			return result;
		}

		/// <summary>Indicates the type of certificate contained in a file.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.X509Certificates.X509ContentType" /> object.</returns>
		/// <param name="fileName">The name of a certificate file. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="fileName" /> is null.</exception>
		[MonoTODO("Detection limited to Cert, Pfx, Pkcs12 and Unknown")]
		public static X509ContentType GetCertContentType(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (fileName.Length == 0)
			{
				throw new ArgumentException("fileName");
			}
			byte[] rawData = X509Certificate2.Load(fileName);
			return X509Certificate2.GetCertContentType(rawData);
		}

		internal X509Certificate MonoCertificate
		{
			get
			{
				return this._cert;
			}
		}
	}
}
