using Mono.Security;
using Mono.Security.Cryptography;
using System;
using System.Text;

namespace System.Security.Cryptography.X509Certificates
{
	/// <summary>Defines a string that identifies a certificate's subject key identifier (SKI). This class cannot be inherited.</summary>
	public sealed class X509SubjectKeyIdentifierExtension : X509Extension
	{
		internal const string oid = "2.5.29.14";

		internal const string friendlyName = "Subject Key Identifier";

		private byte[] _subjectKeyIdentifier;

		private string _ski;

		private AsnDecodeStatus _status;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509SubjectKeyIdentifierExtension" /> class.</summary>
		public X509SubjectKeyIdentifierExtension()
		{
			this._oid = new Oid("2.5.29.14", "Subject Key Identifier");
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509SubjectKeyIdentifierExtension" /> class using encoded data and a value that identifies whether the extension is critical.</summary>
		/// <param name="encodedSubjectKeyIdentifier">The <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object to use to create the extension.</param>
		/// <param name="critical">true if the extension is critical; otherwise, false.</param>
		public X509SubjectKeyIdentifierExtension(AsnEncodedData encodedSubjectKeyIdentifier, bool critical)
		{
			this._oid = new Oid("2.5.29.14", "Subject Key Identifier");
			this._raw = encodedSubjectKeyIdentifier.RawData;
			base.Critical = critical;
			this._status = this.Decode(base.RawData);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509SubjectKeyIdentifierExtension" /> class using a byte array and a value that identifies whether the extension is critical.</summary>
		/// <param name="subjectKeyIdentifier">A byte array that represents data to use to create the extension.</param>
		/// <param name="critical">true if the extension is critical; otherwise, false.</param>
		public X509SubjectKeyIdentifierExtension(byte[] subjectKeyIdentifier, bool critical)
		{
			if (subjectKeyIdentifier == null)
			{
				throw new ArgumentNullException("subjectKeyIdentifier");
			}
			if (subjectKeyIdentifier.Length == 0)
			{
				throw new ArgumentException("subjectKeyIdentifier");
			}
			this._oid = new Oid("2.5.29.14", "Subject Key Identifier");
			base.Critical = critical;
			this._subjectKeyIdentifier = (byte[])subjectKeyIdentifier.Clone();
			base.RawData = this.Encode();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509SubjectKeyIdentifierExtension" /> class using a string and a value that identifies whether the extension is critical.</summary>
		/// <param name="subjectKeyIdentifier">A string, encoded in hexadecimal format, that represents the subject key identifier (SKI) for a certificate.</param>
		/// <param name="critical">true if the extension is critical; otherwise, false.</param>
		public X509SubjectKeyIdentifierExtension(string subjectKeyIdentifier, bool critical)
		{
			if (subjectKeyIdentifier == null)
			{
				throw new ArgumentNullException("subjectKeyIdentifier");
			}
			if (subjectKeyIdentifier.Length < 2)
			{
				throw new ArgumentException("subjectKeyIdentifier");
			}
			this._oid = new Oid("2.5.29.14", "Subject Key Identifier");
			base.Critical = critical;
			this._subjectKeyIdentifier = X509SubjectKeyIdentifierExtension.FromHex(subjectKeyIdentifier);
			base.RawData = this.Encode();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509SubjectKeyIdentifierExtension" /> class using a public key and a value indicating whether the extension is critical.</summary>
		/// <param name="key">A <see cref="T:System.Security.Cryptography.X509Certificates.PublicKey" />  object to create a subject key identifier (SKI) from. </param>
		/// <param name="critical">true if the extension is critical; otherwise, false.</param>
		public X509SubjectKeyIdentifierExtension(PublicKey key, bool critical) : this(key, X509SubjectKeyIdentifierHashAlgorithm.Sha1, critical)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509SubjectKeyIdentifierExtension" /> class using a public key, a hash algorithm identifier, and a value indicating whether the extension is critical. </summary>
		/// <param name="key">A <see cref="T:System.Security.Cryptography.X509Certificates.PublicKey" /> object to create a subject key identifier (SKI) from.</param>
		/// <param name="algorithm">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509SubjectKeyIdentifierHashAlgorithm" /> values that identifies which hash algorithm to use.</param>
		/// <param name="critical">true if the extension is critical; otherwise, false.</param>
		public X509SubjectKeyIdentifierExtension(PublicKey key, X509SubjectKeyIdentifierHashAlgorithm algorithm, bool critical)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			byte[] rawData = key.EncodedKeyValue.RawData;
			switch (algorithm)
			{
			case X509SubjectKeyIdentifierHashAlgorithm.Sha1:
				this._subjectKeyIdentifier = SHA1.Create().ComputeHash(rawData);
				break;
			case X509SubjectKeyIdentifierHashAlgorithm.ShortSha1:
			{
				byte[] src = SHA1.Create().ComputeHash(rawData);
				this._subjectKeyIdentifier = new byte[8];
				Buffer.BlockCopy(src, 12, this._subjectKeyIdentifier, 0, 8);
				this._subjectKeyIdentifier[0] = (64 | (this._subjectKeyIdentifier[0] & 15));
				break;
			}
			case X509SubjectKeyIdentifierHashAlgorithm.CapiSha1:
			{
				ASN1 asn = new ASN1(48);
				ASN1 asn2 = asn.Add(new ASN1(48));
				asn2.Add(new ASN1(CryptoConfig.EncodeOID(key.Oid.Value)));
				asn2.Add(new ASN1(key.EncodedParameters.RawData));
				byte[] array = new byte[rawData.Length + 1];
				Buffer.BlockCopy(rawData, 0, array, 1, rawData.Length);
				asn.Add(new ASN1(3, array));
				this._subjectKeyIdentifier = SHA1.Create().ComputeHash(asn.GetBytes());
				break;
			}
			default:
				throw new ArgumentException("algorithm");
			}
			this._oid = new Oid("2.5.29.14", "Subject Key Identifier");
			base.Critical = critical;
			base.RawData = this.Encode();
		}

		/// <summary>Gets a string that represents the subject key identifier (SKI) for a certificate.</summary>
		/// <returns>A string, encoded in hexadecimal format, that represents the subject key identifier (SKI).</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The extension cannot be decoded. </exception>
		public string SubjectKeyIdentifier
		{
			get
			{
				AsnDecodeStatus status = this._status;
				if (status != AsnDecodeStatus.Ok && status != AsnDecodeStatus.InformationNotAvailable)
				{
					throw new CryptographicException("Badly encoded extension.");
				}
				if (this._subjectKeyIdentifier != null)
				{
					this._ski = CryptoConvert.ToHex(this._subjectKeyIdentifier);
				}
				return this._ski;
			}
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509SubjectKeyIdentifierExtension" /> class by copying information from encoded data.</summary>
		/// <param name="asnEncodedData">The <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object to use to create the extension.</param>
		public override void CopyFrom(AsnEncodedData encodedData)
		{
			if (encodedData == null)
			{
				throw new ArgumentNullException("encodedData");
			}
			X509Extension x509Extension = encodedData as X509Extension;
			if (x509Extension == null)
			{
				throw new ArgumentException(Locale.GetText("Wrong type."), "encodedData");
			}
			if (x509Extension._oid == null)
			{
				this._oid = new Oid("2.5.29.14", "Subject Key Identifier");
			}
			else
			{
				this._oid = new Oid(x509Extension._oid);
			}
			base.RawData = x509Extension.RawData;
			base.Critical = x509Extension.Critical;
			this._status = this.Decode(base.RawData);
		}

		internal static byte FromHexChar(char c)
		{
			if (c >= 'a' && c <= 'f')
			{
				return (byte)(c - 'a' + '\n');
			}
			if (c >= 'A' && c <= 'F')
			{
				return (byte)(c - 'A' + '\n');
			}
			if (c >= '0' && c <= '9')
			{
				return (byte)(c - '0');
			}
			return byte.MaxValue;
		}

		internal static byte FromHexChars(char c1, char c2)
		{
			byte b = X509SubjectKeyIdentifierExtension.FromHexChar(c1);
			if (b < 255)
			{
				b = (byte)((int)b << 4 | (int)X509SubjectKeyIdentifierExtension.FromHexChar(c2));
			}
			return b;
		}

		internal static byte[] FromHex(string hex)
		{
			if (hex == null)
			{
				return null;
			}
			int num = hex.Length >> 1;
			byte[] array = new byte[num];
			int i = 0;
			int num2 = 0;
			while (i < num)
			{
				array[i++] = X509SubjectKeyIdentifierExtension.FromHexChars(hex[num2++], hex[num2++]);
			}
			return array;
		}

		internal AsnDecodeStatus Decode(byte[] extension)
		{
			if (extension == null || extension.Length == 0)
			{
				return AsnDecodeStatus.BadAsn;
			}
			this._ski = string.Empty;
			if (extension[0] != 4)
			{
				return AsnDecodeStatus.BadTag;
			}
			if (extension.Length == 2)
			{
				return AsnDecodeStatus.InformationNotAvailable;
			}
			if (extension.Length < 3)
			{
				return AsnDecodeStatus.BadLength;
			}
			try
			{
				ASN1 asn = new ASN1(extension);
				this._subjectKeyIdentifier = asn.Value;
			}
			catch
			{
				return AsnDecodeStatus.BadAsn;
			}
			return AsnDecodeStatus.Ok;
		}

		internal byte[] Encode()
		{
			ASN1 asn = new ASN1(4, this._subjectKeyIdentifier);
			return asn.GetBytes();
		}

		internal override string ToString(bool multiLine)
		{
			switch (this._status)
			{
			case AsnDecodeStatus.BadAsn:
				return string.Empty;
			case AsnDecodeStatus.BadTag:
			case AsnDecodeStatus.BadLength:
				return base.FormatUnkownData(this._raw);
			case AsnDecodeStatus.InformationNotAvailable:
				return "Information Not Available";
			default:
			{
				if (this._oid.Value != "2.5.29.14")
				{
					return string.Format("Unknown Key Usage ({0})", this._oid.Value);
				}
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < this._subjectKeyIdentifier.Length; i++)
				{
					stringBuilder.Append(this._subjectKeyIdentifier[i].ToString("x2"));
					if (i != this._subjectKeyIdentifier.Length - 1)
					{
						stringBuilder.Append(" ");
					}
				}
				if (multiLine)
				{
					stringBuilder.Append(Environment.NewLine);
				}
				return stringBuilder.ToString();
			}
			}
		}
	}
}
