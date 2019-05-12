using System;
using System.Security.Cryptography;

namespace Mono.Security.X509
{
	public class X509CertificateBuilder : X509Builder
	{
		private byte version;

		private byte[] sn;

		private string issuer;

		private DateTime notBefore;

		private DateTime notAfter;

		private string subject;

		private AsymmetricAlgorithm aa;

		private byte[] issuerUniqueID;

		private byte[] subjectUniqueID;

		private X509ExtensionCollection extensions;

		public X509CertificateBuilder() : this(3)
		{
		}

		public X509CertificateBuilder(byte version)
		{
			if (version > 3)
			{
				throw new ArgumentException("Invalid certificate version");
			}
			this.version = version;
			this.extensions = new X509ExtensionCollection();
		}

		public byte Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = value;
			}
		}

		public byte[] SerialNumber
		{
			get
			{
				return this.sn;
			}
			set
			{
				this.sn = value;
			}
		}

		public string IssuerName
		{
			get
			{
				return this.issuer;
			}
			set
			{
				this.issuer = value;
			}
		}

		public DateTime NotBefore
		{
			get
			{
				return this.notBefore;
			}
			set
			{
				this.notBefore = value;
			}
		}

		public DateTime NotAfter
		{
			get
			{
				return this.notAfter;
			}
			set
			{
				this.notAfter = value;
			}
		}

		public string SubjectName
		{
			get
			{
				return this.subject;
			}
			set
			{
				this.subject = value;
			}
		}

		public AsymmetricAlgorithm SubjectPublicKey
		{
			get
			{
				return this.aa;
			}
			set
			{
				this.aa = value;
			}
		}

		public byte[] IssuerUniqueId
		{
			get
			{
				return this.issuerUniqueID;
			}
			set
			{
				this.issuerUniqueID = value;
			}
		}

		public byte[] SubjectUniqueId
		{
			get
			{
				return this.subjectUniqueID;
			}
			set
			{
				this.subjectUniqueID = value;
			}
		}

		public X509ExtensionCollection Extensions
		{
			get
			{
				return this.extensions;
			}
		}

		private ASN1 SubjectPublicKeyInfo()
		{
			ASN1 asn = new ASN1(48);
			if (this.aa is RSA)
			{
				asn.Add(PKCS7.AlgorithmIdentifier("1.2.840.113549.1.1.1"));
				RSAParameters rsaparameters = (this.aa as RSA).ExportParameters(false);
				ASN1 asn2 = new ASN1(48);
				asn2.Add(ASN1Convert.FromUnsignedBigInteger(rsaparameters.Modulus));
				asn2.Add(ASN1Convert.FromUnsignedBigInteger(rsaparameters.Exponent));
				asn.Add(new ASN1(this.UniqueIdentifier(asn2.GetBytes())));
			}
			else
			{
				if (!(this.aa is DSA))
				{
					throw new NotSupportedException("Unknown Asymmetric Algorithm " + this.aa.ToString());
				}
				DSAParameters dsaparameters = (this.aa as DSA).ExportParameters(false);
				ASN1 asn3 = new ASN1(48);
				asn3.Add(ASN1Convert.FromUnsignedBigInteger(dsaparameters.P));
				asn3.Add(ASN1Convert.FromUnsignedBigInteger(dsaparameters.Q));
				asn3.Add(ASN1Convert.FromUnsignedBigInteger(dsaparameters.G));
				asn.Add(PKCS7.AlgorithmIdentifier("1.2.840.10040.4.1", asn3));
				ASN1 asn4 = asn.Add(new ASN1(3));
				asn4.Add(ASN1Convert.FromUnsignedBigInteger(dsaparameters.Y));
			}
			return asn;
		}

		private byte[] UniqueIdentifier(byte[] id)
		{
			ASN1 asn = new ASN1(3);
			byte[] array = new byte[id.Length + 1];
			Buffer.BlockCopy(id, 0, array, 1, id.Length);
			asn.Value = array;
			return asn.GetBytes();
		}

		protected override ASN1 ToBeSigned(string oid)
		{
			ASN1 asn = new ASN1(48);
			if (this.version > 1)
			{
				byte[] data = new byte[]
				{
					this.version - 1
				};
				ASN1 asn2 = asn.Add(new ASN1(160));
				asn2.Add(new ASN1(2, data));
			}
			asn.Add(new ASN1(2, this.sn));
			asn.Add(PKCS7.AlgorithmIdentifier(oid));
			asn.Add(X501.FromString(this.issuer));
			ASN1 asn3 = asn.Add(new ASN1(48));
			asn3.Add(ASN1Convert.FromDateTime(this.notBefore));
			asn3.Add(ASN1Convert.FromDateTime(this.notAfter));
			asn.Add(X501.FromString(this.subject));
			asn.Add(this.SubjectPublicKeyInfo());
			if (this.version > 1)
			{
				if (this.issuerUniqueID != null)
				{
					asn.Add(new ASN1(161, this.UniqueIdentifier(this.issuerUniqueID)));
				}
				if (this.subjectUniqueID != null)
				{
					asn.Add(new ASN1(161, this.UniqueIdentifier(this.subjectUniqueID)));
				}
				if (this.version > 2 && this.extensions.Count > 0)
				{
					asn.Add(new ASN1(163, this.extensions.GetBytes()));
				}
			}
			return asn;
		}
	}
}
