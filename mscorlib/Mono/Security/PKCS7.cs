using Mono.Security.X509;
using System;
using System.Collections;
using System.Security.Cryptography;

namespace Mono.Security
{
	internal sealed class PKCS7
	{
		private PKCS7()
		{
		}

		public static ASN1 Attribute(string oid, ASN1 value)
		{
			ASN1 asn = new ASN1(48);
			asn.Add(ASN1Convert.FromOid(oid));
			ASN1 asn2 = asn.Add(new ASN1(49));
			asn2.Add(value);
			return asn;
		}

		public static ASN1 AlgorithmIdentifier(string oid)
		{
			ASN1 asn = new ASN1(48);
			asn.Add(ASN1Convert.FromOid(oid));
			asn.Add(new ASN1(5));
			return asn;
		}

		public static ASN1 AlgorithmIdentifier(string oid, ASN1 parameters)
		{
			ASN1 asn = new ASN1(48);
			asn.Add(ASN1Convert.FromOid(oid));
			asn.Add(parameters);
			return asn;
		}

		public static ASN1 IssuerAndSerialNumber(X509Certificate x509)
		{
			ASN1 asn = null;
			ASN1 asn2 = null;
			ASN1 asn3 = new ASN1(x509.RawData);
			int i = 0;
			bool flag = false;
			while (i < asn3[0].Count)
			{
				ASN1 asn4 = asn3[0][i++];
				if (asn4.Tag == 2)
				{
					asn2 = asn4;
				}
				else if (asn4.Tag == 48)
				{
					if (flag)
					{
						asn = asn4;
						break;
					}
					flag = true;
				}
			}
			ASN1 asn5 = new ASN1(48);
			asn5.Add(asn);
			asn5.Add(asn2);
			return asn5;
		}

		public class Oid
		{
			public const string rsaEncryption = "1.2.840.113549.1.1.1";

			public const string data = "1.2.840.113549.1.7.1";

			public const string signedData = "1.2.840.113549.1.7.2";

			public const string envelopedData = "1.2.840.113549.1.7.3";

			public const string signedAndEnvelopedData = "1.2.840.113549.1.7.4";

			public const string digestedData = "1.2.840.113549.1.7.5";

			public const string encryptedData = "1.2.840.113549.1.7.6";

			public const string contentType = "1.2.840.113549.1.9.3";

			public const string messageDigest = "1.2.840.113549.1.9.4";

			public const string signingTime = "1.2.840.113549.1.9.5";

			public const string countersignature = "1.2.840.113549.1.9.6";
		}

		public class ContentInfo
		{
			private string contentType;

			private ASN1 content;

			public ContentInfo()
			{
				this.content = new ASN1(160);
			}

			public ContentInfo(string oid) : this()
			{
				this.contentType = oid;
			}

			public ContentInfo(byte[] data) : this(new ASN1(data))
			{
			}

			public ContentInfo(ASN1 asn1)
			{
				if (asn1.Tag != 48 || (asn1.Count < 1 && asn1.Count > 2))
				{
					throw new ArgumentException("Invalid ASN1");
				}
				if (asn1[0].Tag != 6)
				{
					throw new ArgumentException("Invalid contentType");
				}
				this.contentType = ASN1Convert.ToOid(asn1[0]);
				if (asn1.Count > 1)
				{
					if (asn1[1].Tag != 160)
					{
						throw new ArgumentException("Invalid content");
					}
					this.content = asn1[1];
				}
			}

			public ASN1 ASN1
			{
				get
				{
					return this.GetASN1();
				}
			}

			public ASN1 Content
			{
				get
				{
					return this.content;
				}
				set
				{
					this.content = value;
				}
			}

			public string ContentType
			{
				get
				{
					return this.contentType;
				}
				set
				{
					this.contentType = value;
				}
			}

			internal ASN1 GetASN1()
			{
				ASN1 asn = new ASN1(48);
				asn.Add(ASN1Convert.FromOid(this.contentType));
				if (this.content != null && this.content.Count > 0)
				{
					asn.Add(this.content);
				}
				return asn;
			}

			public byte[] GetBytes()
			{
				return this.GetASN1().GetBytes();
			}
		}

		public class EncryptedData
		{
			private byte _version;

			private PKCS7.ContentInfo _content;

			private PKCS7.ContentInfo _encryptionAlgorithm;

			private byte[] _encrypted;

			public EncryptedData()
			{
				this._version = 0;
			}

			public EncryptedData(byte[] data) : this(new ASN1(data))
			{
			}

			public EncryptedData(ASN1 asn1) : this()
			{
				if (asn1.Tag != 48 || asn1.Count < 2)
				{
					throw new ArgumentException("Invalid EncryptedData");
				}
				if (asn1[0].Tag != 2)
				{
					throw new ArgumentException("Invalid version");
				}
				this._version = asn1[0].Value[0];
				ASN1 asn2 = asn1[1];
				if (asn2.Tag != 48)
				{
					throw new ArgumentException("missing EncryptedContentInfo");
				}
				ASN1 asn3 = asn2[0];
				if (asn3.Tag != 6)
				{
					throw new ArgumentException("missing EncryptedContentInfo.ContentType");
				}
				this._content = new PKCS7.ContentInfo(ASN1Convert.ToOid(asn3));
				ASN1 asn4 = asn2[1];
				if (asn4.Tag != 48)
				{
					throw new ArgumentException("missing EncryptedContentInfo.ContentEncryptionAlgorithmIdentifier");
				}
				this._encryptionAlgorithm = new PKCS7.ContentInfo(ASN1Convert.ToOid(asn4[0]));
				this._encryptionAlgorithm.Content = asn4[1];
				ASN1 asn5 = asn2[2];
				if (asn5.Tag != 128)
				{
					throw new ArgumentException("missing EncryptedContentInfo.EncryptedContent");
				}
				this._encrypted = asn5.Value;
			}

			public ASN1 ASN1
			{
				get
				{
					return this.GetASN1();
				}
			}

			public PKCS7.ContentInfo ContentInfo
			{
				get
				{
					return this._content;
				}
			}

			public PKCS7.ContentInfo EncryptionAlgorithm
			{
				get
				{
					return this._encryptionAlgorithm;
				}
			}

			public byte[] EncryptedContent
			{
				get
				{
					if (this._encrypted == null)
					{
						return null;
					}
					return (byte[])this._encrypted.Clone();
				}
			}

			public byte Version
			{
				get
				{
					return this._version;
				}
				set
				{
					this._version = value;
				}
			}

			internal ASN1 GetASN1()
			{
				return null;
			}

			public byte[] GetBytes()
			{
				return this.GetASN1().GetBytes();
			}
		}

		public class EnvelopedData
		{
			private byte _version;

			private PKCS7.ContentInfo _content;

			private PKCS7.ContentInfo _encryptionAlgorithm;

			private ArrayList _recipientInfos;

			private byte[] _encrypted;

			public EnvelopedData()
			{
				this._version = 0;
				this._content = new PKCS7.ContentInfo();
				this._encryptionAlgorithm = new PKCS7.ContentInfo();
				this._recipientInfos = new ArrayList();
			}

			public EnvelopedData(byte[] data) : this(new ASN1(data))
			{
			}

			public EnvelopedData(ASN1 asn1) : this()
			{
				if (asn1[0].Tag != 48 || asn1[0].Count < 3)
				{
					throw new ArgumentException("Invalid EnvelopedData");
				}
				if (asn1[0][0].Tag != 2)
				{
					throw new ArgumentException("Invalid version");
				}
				this._version = asn1[0][0].Value[0];
				ASN1 asn2 = asn1[0][1];
				if (asn2.Tag != 49)
				{
					throw new ArgumentException("missing RecipientInfos");
				}
				for (int i = 0; i < asn2.Count; i++)
				{
					ASN1 data = asn2[i];
					this._recipientInfos.Add(new PKCS7.RecipientInfo(data));
				}
				ASN1 asn3 = asn1[0][2];
				if (asn3.Tag != 48)
				{
					throw new ArgumentException("missing EncryptedContentInfo");
				}
				ASN1 asn4 = asn3[0];
				if (asn4.Tag != 6)
				{
					throw new ArgumentException("missing EncryptedContentInfo.ContentType");
				}
				this._content = new PKCS7.ContentInfo(ASN1Convert.ToOid(asn4));
				ASN1 asn5 = asn3[1];
				if (asn5.Tag != 48)
				{
					throw new ArgumentException("missing EncryptedContentInfo.ContentEncryptionAlgorithmIdentifier");
				}
				this._encryptionAlgorithm = new PKCS7.ContentInfo(ASN1Convert.ToOid(asn5[0]));
				this._encryptionAlgorithm.Content = asn5[1];
				ASN1 asn6 = asn3[2];
				if (asn6.Tag != 128)
				{
					throw new ArgumentException("missing EncryptedContentInfo.EncryptedContent");
				}
				this._encrypted = asn6.Value;
			}

			public ArrayList RecipientInfos
			{
				get
				{
					return this._recipientInfos;
				}
			}

			public ASN1 ASN1
			{
				get
				{
					return this.GetASN1();
				}
			}

			public PKCS7.ContentInfo ContentInfo
			{
				get
				{
					return this._content;
				}
			}

			public PKCS7.ContentInfo EncryptionAlgorithm
			{
				get
				{
					return this._encryptionAlgorithm;
				}
			}

			public byte[] EncryptedContent
			{
				get
				{
					if (this._encrypted == null)
					{
						return null;
					}
					return (byte[])this._encrypted.Clone();
				}
			}

			public byte Version
			{
				get
				{
					return this._version;
				}
				set
				{
					this._version = value;
				}
			}

			internal ASN1 GetASN1()
			{
				return new ASN1(48);
			}

			public byte[] GetBytes()
			{
				return this.GetASN1().GetBytes();
			}
		}

		public class RecipientInfo
		{
			private int _version;

			private string _oid;

			private byte[] _key;

			private byte[] _ski;

			private string _issuer;

			private byte[] _serial;

			public RecipientInfo()
			{
			}

			public RecipientInfo(ASN1 data)
			{
				if (data.Tag != 48)
				{
					throw new ArgumentException("Invalid RecipientInfo");
				}
				ASN1 asn = data[0];
				if (asn.Tag != 2)
				{
					throw new ArgumentException("missing Version");
				}
				this._version = (int)asn.Value[0];
				ASN1 asn2 = data[1];
				if (asn2.Tag == 128 && this._version == 3)
				{
					this._ski = asn2.Value;
				}
				else
				{
					this._issuer = X501.ToString(asn2[0]);
					this._serial = asn2[1].Value;
				}
				ASN1 asn3 = data[2];
				this._oid = ASN1Convert.ToOid(asn3[0]);
				ASN1 asn4 = data[3];
				this._key = asn4.Value;
			}

			public string Oid
			{
				get
				{
					return this._oid;
				}
			}

			public byte[] Key
			{
				get
				{
					if (this._key == null)
					{
						return null;
					}
					return (byte[])this._key.Clone();
				}
			}

			public byte[] SubjectKeyIdentifier
			{
				get
				{
					if (this._ski == null)
					{
						return null;
					}
					return (byte[])this._ski.Clone();
				}
			}

			public string Issuer
			{
				get
				{
					return this._issuer;
				}
			}

			public byte[] Serial
			{
				get
				{
					if (this._serial == null)
					{
						return null;
					}
					return (byte[])this._serial.Clone();
				}
			}

			public int Version
			{
				get
				{
					return this._version;
				}
			}
		}

		public class SignedData
		{
			private byte version;

			private string hashAlgorithm;

			private PKCS7.ContentInfo contentInfo;

			private X509CertificateCollection certs;

			private ArrayList crls;

			private PKCS7.SignerInfo signerInfo;

			private bool mda;

			private bool signed;

			public SignedData()
			{
				this.version = 1;
				this.contentInfo = new PKCS7.ContentInfo();
				this.certs = new X509CertificateCollection();
				this.crls = new ArrayList();
				this.signerInfo = new PKCS7.SignerInfo();
				this.mda = true;
				this.signed = false;
			}

			public SignedData(byte[] data) : this(new ASN1(data))
			{
			}

			public SignedData(ASN1 asn1)
			{
				if (asn1[0].Tag != 48 || asn1[0].Count < 4)
				{
					throw new ArgumentException("Invalid SignedData");
				}
				if (asn1[0][0].Tag != 2)
				{
					throw new ArgumentException("Invalid version");
				}
				this.version = asn1[0][0].Value[0];
				this.contentInfo = new PKCS7.ContentInfo(asn1[0][2]);
				int num = 3;
				this.certs = new X509CertificateCollection();
				if (asn1[0][num].Tag == 160)
				{
					for (int i = 0; i < asn1[0][num].Count; i++)
					{
						this.certs.Add(new X509Certificate(asn1[0][num][i].GetBytes()));
					}
					num++;
				}
				this.crls = new ArrayList();
				if (asn1[0][num].Tag == 161)
				{
					for (int j = 0; j < asn1[0][num].Count; j++)
					{
						this.crls.Add(asn1[0][num][j].GetBytes());
					}
					num++;
				}
				if (asn1[0][num].Count > 0)
				{
					this.signerInfo = new PKCS7.SignerInfo(asn1[0][num]);
				}
				else
				{
					this.signerInfo = new PKCS7.SignerInfo();
				}
				if (this.signerInfo.HashName != null)
				{
					this.HashName = this.OidToName(this.signerInfo.HashName);
				}
				this.mda = (this.signerInfo.AuthenticatedAttributes.Count > 0);
			}

			public ASN1 ASN1
			{
				get
				{
					return this.GetASN1();
				}
			}

			public X509CertificateCollection Certificates
			{
				get
				{
					return this.certs;
				}
			}

			public PKCS7.ContentInfo ContentInfo
			{
				get
				{
					return this.contentInfo;
				}
			}

			public ArrayList Crls
			{
				get
				{
					return this.crls;
				}
			}

			public string HashName
			{
				get
				{
					return this.hashAlgorithm;
				}
				set
				{
					this.hashAlgorithm = value;
					this.signerInfo.HashName = value;
				}
			}

			public PKCS7.SignerInfo SignerInfo
			{
				get
				{
					return this.signerInfo;
				}
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

			public bool UseAuthenticatedAttributes
			{
				get
				{
					return this.mda;
				}
				set
				{
					this.mda = value;
				}
			}

			public bool VerifySignature(AsymmetricAlgorithm aa)
			{
				if (aa == null)
				{
					return false;
				}
				RSAPKCS1SignatureDeformatter rsapkcs1SignatureDeformatter = new RSAPKCS1SignatureDeformatter(aa);
				rsapkcs1SignatureDeformatter.SetHashAlgorithm(this.hashAlgorithm);
				HashAlgorithm hashAlgorithm = HashAlgorithm.Create(this.hashAlgorithm);
				byte[] signature = this.signerInfo.Signature;
				byte[] array;
				if (this.mda)
				{
					ASN1 asn = new ASN1(49);
					foreach (object obj in this.signerInfo.AuthenticatedAttributes)
					{
						ASN1 asn2 = (ASN1)obj;
						asn.Add(asn2);
					}
					array = hashAlgorithm.ComputeHash(asn.GetBytes());
				}
				else
				{
					array = hashAlgorithm.ComputeHash(this.contentInfo.Content[0].Value);
				}
				return array != null && signature != null && rsapkcs1SignatureDeformatter.VerifySignature(array, signature);
			}

			internal string OidToName(string oid)
			{
				switch (oid)
				{
				case "1.3.14.3.2.26":
					return "SHA1";
				case "1.2.840.113549.2.2":
					return "MD2";
				case "1.2.840.113549.2.5":
					return "MD5";
				case "2.16.840.1.101.3.4.1":
					return "SHA256";
				case "2.16.840.1.101.3.4.2":
					return "SHA384";
				case "2.16.840.1.101.3.4.3":
					return "SHA512";
				}
				return oid;
			}

			internal ASN1 GetASN1()
			{
				ASN1 asn = new ASN1(48);
				byte[] data = new byte[]
				{
					this.version
				};
				asn.Add(new ASN1(2, data));
				ASN1 asn2 = asn.Add(new ASN1(49));
				if (this.hashAlgorithm != null)
				{
					string oid = CryptoConfig.MapNameToOID(this.hashAlgorithm);
					asn2.Add(PKCS7.AlgorithmIdentifier(oid));
				}
				ASN1 asn3 = this.contentInfo.ASN1;
				asn.Add(asn3);
				if (!this.signed && this.hashAlgorithm != null)
				{
					if (this.mda)
					{
						ASN1 value = PKCS7.Attribute("1.2.840.113549.1.9.3", asn3[0]);
						this.signerInfo.AuthenticatedAttributes.Add(value);
						HashAlgorithm hashAlgorithm = HashAlgorithm.Create(this.hashAlgorithm);
						byte[] data2 = hashAlgorithm.ComputeHash(asn3[1][0].Value);
						ASN1 asn4 = new ASN1(48);
						ASN1 value2 = PKCS7.Attribute("1.2.840.113549.1.9.4", asn4.Add(new ASN1(4, data2)));
						this.signerInfo.AuthenticatedAttributes.Add(value2);
					}
					else
					{
						RSAPKCS1SignatureFormatter rsapkcs1SignatureFormatter = new RSAPKCS1SignatureFormatter(this.signerInfo.Key);
						rsapkcs1SignatureFormatter.SetHashAlgorithm(this.hashAlgorithm);
						HashAlgorithm hashAlgorithm2 = HashAlgorithm.Create(this.hashAlgorithm);
						byte[] rgbHash = hashAlgorithm2.ComputeHash(asn3[1][0].Value);
						this.signerInfo.Signature = rsapkcs1SignatureFormatter.CreateSignature(rgbHash);
					}
					this.signed = true;
				}
				if (this.certs.Count > 0)
				{
					ASN1 asn5 = asn.Add(new ASN1(160));
					foreach (X509Certificate x509Certificate in this.certs)
					{
						asn5.Add(new ASN1(x509Certificate.RawData));
					}
				}
				if (this.crls.Count > 0)
				{
					ASN1 asn6 = asn.Add(new ASN1(161));
					foreach (object obj in this.crls)
					{
						byte[] data3 = (byte[])obj;
						asn6.Add(new ASN1(data3));
					}
				}
				ASN1 asn7 = asn.Add(new ASN1(49));
				if (this.signerInfo.Key != null)
				{
					asn7.Add(this.signerInfo.ASN1);
				}
				return asn;
			}

			public byte[] GetBytes()
			{
				return this.GetASN1().GetBytes();
			}
		}

		public class SignerInfo
		{
			private byte version;

			private X509Certificate x509;

			private string hashAlgorithm;

			private AsymmetricAlgorithm key;

			private ArrayList authenticatedAttributes;

			private ArrayList unauthenticatedAttributes;

			private byte[] signature;

			private string issuer;

			private byte[] serial;

			private byte[] ski;

			public SignerInfo()
			{
				this.version = 1;
				this.authenticatedAttributes = new ArrayList();
				this.unauthenticatedAttributes = new ArrayList();
			}

			public SignerInfo(byte[] data) : this(new ASN1(data))
			{
			}

			public SignerInfo(ASN1 asn1) : this()
			{
				if (asn1[0].Tag != 48 || asn1[0].Count < 5)
				{
					throw new ArgumentException("Invalid SignedData");
				}
				if (asn1[0][0].Tag != 2)
				{
					throw new ArgumentException("Invalid version");
				}
				this.version = asn1[0][0].Value[0];
				ASN1 asn2 = asn1[0][1];
				if (asn2.Tag == 128 && this.version == 3)
				{
					this.ski = asn2.Value;
				}
				else
				{
					this.issuer = X501.ToString(asn2[0]);
					this.serial = asn2[1].Value;
				}
				ASN1 asn3 = asn1[0][2];
				this.hashAlgorithm = ASN1Convert.ToOid(asn3[0]);
				int num = 3;
				ASN1 asn4 = asn1[0][num];
				if (asn4.Tag == 160)
				{
					num++;
					for (int i = 0; i < asn4.Count; i++)
					{
						this.authenticatedAttributes.Add(asn4[i]);
					}
				}
				num++;
				ASN1 asn5 = asn1[0][num++];
				if (asn5.Tag == 4)
				{
					this.signature = asn5.Value;
				}
				ASN1 asn6 = asn1[0][num];
				if (asn6 != null && asn6.Tag == 161)
				{
					for (int j = 0; j < asn6.Count; j++)
					{
						this.unauthenticatedAttributes.Add(asn6[j]);
					}
				}
			}

			public string IssuerName
			{
				get
				{
					return this.issuer;
				}
			}

			public byte[] SerialNumber
			{
				get
				{
					if (this.serial == null)
					{
						return null;
					}
					return (byte[])this.serial.Clone();
				}
			}

			public byte[] SubjectKeyIdentifier
			{
				get
				{
					if (this.ski == null)
					{
						return null;
					}
					return (byte[])this.ski.Clone();
				}
			}

			public ASN1 ASN1
			{
				get
				{
					return this.GetASN1();
				}
			}

			public ArrayList AuthenticatedAttributes
			{
				get
				{
					return this.authenticatedAttributes;
				}
			}

			public X509Certificate Certificate
			{
				get
				{
					return this.x509;
				}
				set
				{
					this.x509 = value;
				}
			}

			public string HashName
			{
				get
				{
					return this.hashAlgorithm;
				}
				set
				{
					this.hashAlgorithm = value;
				}
			}

			public AsymmetricAlgorithm Key
			{
				get
				{
					return this.key;
				}
				set
				{
					this.key = value;
				}
			}

			public byte[] Signature
			{
				get
				{
					if (this.signature == null)
					{
						return null;
					}
					return (byte[])this.signature.Clone();
				}
				set
				{
					if (value != null)
					{
						this.signature = (byte[])value.Clone();
					}
				}
			}

			public ArrayList UnauthenticatedAttributes
			{
				get
				{
					return this.unauthenticatedAttributes;
				}
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

			internal ASN1 GetASN1()
			{
				if (this.key == null || this.hashAlgorithm == null)
				{
					return null;
				}
				byte[] data = new byte[]
				{
					this.version
				};
				ASN1 asn = new ASN1(48);
				asn.Add(new ASN1(2, data));
				asn.Add(PKCS7.IssuerAndSerialNumber(this.x509));
				string oid = CryptoConfig.MapNameToOID(this.hashAlgorithm);
				asn.Add(PKCS7.AlgorithmIdentifier(oid));
				ASN1 asn2 = null;
				if (this.authenticatedAttributes.Count > 0)
				{
					asn2 = asn.Add(new ASN1(160));
					foreach (object obj in this.authenticatedAttributes)
					{
						ASN1 asn3 = (ASN1)obj;
						asn2.Add(asn3);
					}
				}
				if (this.key is RSA)
				{
					asn.Add(PKCS7.AlgorithmIdentifier("1.2.840.113549.1.1.1"));
					if (asn2 != null)
					{
						RSAPKCS1SignatureFormatter rsapkcs1SignatureFormatter = new RSAPKCS1SignatureFormatter(this.key);
						rsapkcs1SignatureFormatter.SetHashAlgorithm(this.hashAlgorithm);
						byte[] bytes = asn2.GetBytes();
						bytes[0] = 49;
						HashAlgorithm hashAlgorithm = HashAlgorithm.Create(this.hashAlgorithm);
						byte[] rgbHash = hashAlgorithm.ComputeHash(bytes);
						this.signature = rsapkcs1SignatureFormatter.CreateSignature(rgbHash);
					}
					asn.Add(new ASN1(4, this.signature));
					if (this.unauthenticatedAttributes.Count > 0)
					{
						ASN1 asn4 = asn.Add(new ASN1(161));
						foreach (object obj2 in this.unauthenticatedAttributes)
						{
							ASN1 asn5 = (ASN1)obj2;
							asn4.Add(asn5);
						}
					}
					return asn;
				}
				if (this.key is DSA)
				{
					throw new NotImplementedException("not yet");
				}
				throw new CryptographicException("Unknown assymetric algorithm");
			}

			public byte[] GetBytes()
			{
				return this.GetASN1().GetBytes();
			}
		}
	}
}
