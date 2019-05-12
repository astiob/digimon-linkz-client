using Mono.Security;
using System;
using System.Text;

namespace System.Security.Cryptography.X509Certificates
{
	/// <summary>Defines the constraints set on a certificate. This class cannot be inherited.</summary>
	public sealed class X509BasicConstraintsExtension : X509Extension
	{
		internal const string oid = "2.5.29.19";

		internal const string friendlyName = "Basic Constraints";

		private bool _certificateAuthority;

		private bool _hasPathLengthConstraint;

		private int _pathLengthConstraint;

		private AsnDecodeStatus _status;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509BasicConstraintsExtension" /> class.</summary>
		public X509BasicConstraintsExtension()
		{
			this._oid = new Oid("2.5.29.19", "Basic Constraints");
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509BasicConstraintsExtension" /> class using an <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object and a value that identifies whether the extension is critical. </summary>
		/// <param name="encodedBasicConstraints">The encoded data to use to create the extension.</param>
		/// <param name="critical">true if the extension is critical; otherwise, false.</param>
		public X509BasicConstraintsExtension(AsnEncodedData encodedBasicConstraints, bool critical)
		{
			this._oid = new Oid("2.5.29.19", "Basic Constraints");
			this._raw = encodedBasicConstraints.RawData;
			base.Critical = critical;
			this._status = this.Decode(base.RawData);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509BasicConstraintsExtension" /> class. Parameters specify a value that indicates whether a certificate is a certificate authority (CA) certificate, a value that indicates whether the certificate has a restriction on the number of path levels it allows, the number of levels allowed in a certificate's path, and a value that indicates whether the extension is critical.  </summary>
		/// <param name="certificateAuthority">true if the certificate is a certificate authority (CA) certificate; otherwise, false.</param>
		/// <param name="hasPathLengthConstraint">true if the certificate has a restriction on the number of path levels it allows; otherwise, false.</param>
		/// <param name="pathLengthConstraint">The number of levels allowed in a certificate's path.</param>
		/// <param name="critical">true if the extension is critical; otherwise, false.</param>
		public X509BasicConstraintsExtension(bool certificateAuthority, bool hasPathLengthConstraint, int pathLengthConstraint, bool critical)
		{
			if (hasPathLengthConstraint)
			{
				if (pathLengthConstraint < 0)
				{
					throw new ArgumentOutOfRangeException("pathLengthConstraint");
				}
				this._pathLengthConstraint = pathLengthConstraint;
			}
			this._hasPathLengthConstraint = hasPathLengthConstraint;
			this._certificateAuthority = certificateAuthority;
			this._oid = new Oid("2.5.29.19", "Basic Constraints");
			base.Critical = critical;
			base.RawData = this.Encode();
		}

		/// <summary>Gets a value indicating whether a certificate is a certificate authority (CA) certificate.</summary>
		/// <returns>true if the certificate is a certificate authority (CA) certificate, otherwise, false.</returns>
		public bool CertificateAuthority
		{
			get
			{
				AsnDecodeStatus status = this._status;
				if (status != AsnDecodeStatus.Ok && status != AsnDecodeStatus.InformationNotAvailable)
				{
					throw new CryptographicException("Badly encoded extension.");
				}
				return this._certificateAuthority;
			}
		}

		/// <summary>Gets a value indicating whether a certificate has a restriction on the number of path levels it allows.</summary>
		/// <returns>true if the certificate has a restriction on the number of path levels it allows, otherwise, false.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The extension cannot be decoded. </exception>
		public bool HasPathLengthConstraint
		{
			get
			{
				AsnDecodeStatus status = this._status;
				if (status != AsnDecodeStatus.Ok && status != AsnDecodeStatus.InformationNotAvailable)
				{
					throw new CryptographicException("Badly encoded extension.");
				}
				return this._hasPathLengthConstraint;
			}
		}

		/// <summary>Gets the number of levels allowed in a certificate's path.</summary>
		/// <returns>An integer indicating the number of levels allowed in a certificate's path.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The extension cannot be decoded. </exception>
		public int PathLengthConstraint
		{
			get
			{
				AsnDecodeStatus status = this._status;
				if (status != AsnDecodeStatus.Ok && status != AsnDecodeStatus.InformationNotAvailable)
				{
					throw new CryptographicException("Badly encoded extension.");
				}
				return this._pathLengthConstraint;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509BasicConstraintsExtension" /> class using an <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object.</summary>
		/// <param name="asnEncodedData">The encoded data to use to create the extension.</param>
		public override void CopyFrom(AsnEncodedData asnEncodedData)
		{
			if (asnEncodedData == null)
			{
				throw new ArgumentNullException("asnEncodedData");
			}
			X509Extension x509Extension = asnEncodedData as X509Extension;
			if (x509Extension == null)
			{
				throw new ArgumentException(Locale.GetText("Wrong type."), "asnEncodedData");
			}
			if (x509Extension._oid == null)
			{
				this._oid = new Oid("2.5.29.19", "Basic Constraints");
			}
			else
			{
				this._oid = new Oid(x509Extension._oid);
			}
			base.RawData = x509Extension.RawData;
			base.Critical = x509Extension.Critical;
			this._status = this.Decode(base.RawData);
		}

		internal AsnDecodeStatus Decode(byte[] extension)
		{
			if (extension == null || extension.Length == 0)
			{
				return AsnDecodeStatus.BadAsn;
			}
			if (extension[0] != 48)
			{
				return AsnDecodeStatus.BadTag;
			}
			if (extension.Length < 3 && (extension.Length != 2 || extension[1] != 0))
			{
				return AsnDecodeStatus.BadLength;
			}
			try
			{
				ASN1 asn = new ASN1(extension);
				int num = 0;
				ASN1 asn2 = asn[num++];
				if (asn2 != null && asn2.Tag == 1)
				{
					this._certificateAuthority = (asn2.Value[0] == byte.MaxValue);
					asn2 = asn[num++];
				}
				if (asn2 != null && asn2.Tag == 2)
				{
					this._hasPathLengthConstraint = true;
					this._pathLengthConstraint = ASN1Convert.ToInt32(asn2);
				}
			}
			catch
			{
				return AsnDecodeStatus.BadAsn;
			}
			return AsnDecodeStatus.Ok;
		}

		internal byte[] Encode()
		{
			ASN1 asn = new ASN1(48);
			if (this._certificateAuthority)
			{
				asn.Add(new ASN1(1, new byte[]
				{
					byte.MaxValue
				}));
			}
			if (this._hasPathLengthConstraint)
			{
				if (this._pathLengthConstraint == 0)
				{
					asn.Add(new ASN1(2, new byte[1]));
				}
				else
				{
					asn.Add(ASN1Convert.FromInt32(this._pathLengthConstraint));
				}
			}
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
				if (this._oid.Value != "2.5.29.19")
				{
					return string.Format("Unknown Key Usage ({0})", this._oid.Value);
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("Subject Type=");
				if (this._certificateAuthority)
				{
					stringBuilder.Append("CA");
				}
				else
				{
					stringBuilder.Append("End Entity");
				}
				if (multiLine)
				{
					stringBuilder.Append(Environment.NewLine);
				}
				else
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("Path Length Constraint=");
				if (this._hasPathLengthConstraint)
				{
					stringBuilder.Append(this._pathLengthConstraint);
				}
				else
				{
					stringBuilder.Append("None");
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
