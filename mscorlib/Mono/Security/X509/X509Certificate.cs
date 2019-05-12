using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace Mono.Security.X509
{
	internal class X509Certificate : ISerializable
	{
		private ASN1 decoder;

		private byte[] m_encodedcert;

		private DateTime m_from;

		private DateTime m_until;

		private ASN1 issuer;

		private string m_issuername;

		private string m_keyalgo;

		private byte[] m_keyalgoparams;

		private ASN1 subject;

		private string m_subject;

		private byte[] m_publickey;

		private byte[] signature;

		private string m_signaturealgo;

		private byte[] m_signaturealgoparams;

		private byte[] certhash;

		private RSA _rsa;

		private DSA _dsa;

		private int version;

		private byte[] serialnumber;

		private byte[] issuerUniqueID;

		private byte[] subjectUniqueID;

		private X509ExtensionCollection extensions;

		private static string encoding_error = Locale.GetText("Input data cannot be coded as a valid certificate.");

		public X509Certificate(byte[] data)
		{
			if (data != null)
			{
				if (data.Length > 0 && data[0] != 48)
				{
					try
					{
						data = X509Certificate.PEM("CERTIFICATE", data);
					}
					catch (Exception inner)
					{
						throw new CryptographicException(X509Certificate.encoding_error, inner);
					}
				}
				this.Parse(data);
			}
		}

		protected X509Certificate(SerializationInfo info, StreamingContext context)
		{
			this.Parse((byte[])info.GetValue("raw", typeof(byte[])));
		}

		private void Parse(byte[] data)
		{
			try
			{
				this.decoder = new ASN1(data);
				if (this.decoder.Tag != 48)
				{
					throw new CryptographicException(X509Certificate.encoding_error);
				}
				if (this.decoder[0].Tag != 48)
				{
					throw new CryptographicException(X509Certificate.encoding_error);
				}
				ASN1 asn = this.decoder[0];
				int num = 0;
				ASN1 asn2 = this.decoder[0][num];
				this.version = 1;
				if (asn2.Tag == 160 && asn2.Count > 0)
				{
					this.version += (int)asn2[0].Value[0];
					num++;
				}
				ASN1 asn3 = this.decoder[0][num++];
				if (asn3.Tag != 2)
				{
					throw new CryptographicException(X509Certificate.encoding_error);
				}
				this.serialnumber = asn3.Value;
				Array.Reverse(this.serialnumber, 0, this.serialnumber.Length);
				num++;
				this.issuer = asn.Element(num++, 48);
				this.m_issuername = X501.ToString(this.issuer);
				ASN1 asn4 = asn.Element(num++, 48);
				ASN1 time = asn4[0];
				this.m_from = ASN1Convert.ToDateTime(time);
				ASN1 time2 = asn4[1];
				this.m_until = ASN1Convert.ToDateTime(time2);
				this.subject = asn.Element(num++, 48);
				this.m_subject = X501.ToString(this.subject);
				ASN1 asn5 = asn.Element(num++, 48);
				ASN1 asn6 = asn5.Element(0, 48);
				ASN1 asn7 = asn6.Element(0, 6);
				this.m_keyalgo = ASN1Convert.ToOid(asn7);
				ASN1 asn8 = asn6[1];
				this.m_keyalgoparams = ((asn6.Count <= 1) ? null : asn8.GetBytes());
				ASN1 asn9 = asn5.Element(1, 3);
				int num2 = asn9.Length - 1;
				this.m_publickey = new byte[num2];
				Buffer.BlockCopy(asn9.Value, 1, this.m_publickey, 0, num2);
				byte[] value = this.decoder[2].Value;
				this.signature = new byte[value.Length - 1];
				Buffer.BlockCopy(value, 1, this.signature, 0, this.signature.Length);
				asn6 = this.decoder[1];
				asn7 = asn6.Element(0, 6);
				this.m_signaturealgo = ASN1Convert.ToOid(asn7);
				asn8 = asn6[1];
				if (asn8 != null)
				{
					this.m_signaturealgoparams = asn8.GetBytes();
				}
				else
				{
					this.m_signaturealgoparams = null;
				}
				ASN1 asn10 = asn.Element(num, 129);
				if (asn10 != null)
				{
					num++;
					this.issuerUniqueID = asn10.Value;
				}
				ASN1 asn11 = asn.Element(num, 130);
				if (asn11 != null)
				{
					num++;
					this.subjectUniqueID = asn11.Value;
				}
				ASN1 asn12 = asn.Element(num, 163);
				if (asn12 != null && asn12.Count == 1)
				{
					this.extensions = new X509ExtensionCollection(asn12[0]);
				}
				else
				{
					this.extensions = new X509ExtensionCollection(null);
				}
				this.m_encodedcert = (byte[])data.Clone();
			}
			catch (Exception inner)
			{
				throw new CryptographicException(X509Certificate.encoding_error, inner);
			}
		}

		private byte[] GetUnsignedBigInteger(byte[] integer)
		{
			if (integer[0] == 0)
			{
				int num = integer.Length - 1;
				byte[] array = new byte[num];
				Buffer.BlockCopy(integer, 1, array, 0, num);
				return array;
			}
			return integer;
		}

		public DSA DSA
		{
			get
			{
				if (this.m_keyalgoparams == null)
				{
					throw new CryptographicException("Missing key algorithm parameters.");
				}
				if (this._dsa == null)
				{
					DSAParameters parameters = default(DSAParameters);
					ASN1 asn = new ASN1(this.m_publickey);
					if (asn == null || asn.Tag != 2)
					{
						return null;
					}
					parameters.Y = this.GetUnsignedBigInteger(asn.Value);
					ASN1 asn2 = new ASN1(this.m_keyalgoparams);
					if (asn2 == null || asn2.Tag != 48 || asn2.Count < 3)
					{
						return null;
					}
					if (asn2[0].Tag != 2 || asn2[1].Tag != 2 || asn2[2].Tag != 2)
					{
						return null;
					}
					parameters.P = this.GetUnsignedBigInteger(asn2[0].Value);
					parameters.Q = this.GetUnsignedBigInteger(asn2[1].Value);
					parameters.G = this.GetUnsignedBigInteger(asn2[2].Value);
					this._dsa = new DSACryptoServiceProvider(parameters.Y.Length << 3);
					this._dsa.ImportParameters(parameters);
				}
				return this._dsa;
			}
			set
			{
				this._dsa = value;
				if (value != null)
				{
					this._rsa = null;
				}
			}
		}

		public X509ExtensionCollection Extensions
		{
			get
			{
				return this.extensions;
			}
		}

		public byte[] Hash
		{
			get
			{
				if (this.certhash == null)
				{
					string signaturealgo = this.m_signaturealgo;
					if (signaturealgo != null)
					{
						if (X509Certificate.<>f__switch$map13 == null)
						{
							X509Certificate.<>f__switch$map13 = new Dictionary<string, int>(5)
							{
								{
									"1.2.840.113549.1.1.2",
									0
								},
								{
									"1.2.840.113549.1.1.4",
									1
								},
								{
									"1.2.840.113549.1.1.5",
									2
								},
								{
									"1.3.14.3.2.29",
									2
								},
								{
									"1.2.840.10040.4.3",
									2
								}
							};
						}
						int num;
						if (X509Certificate.<>f__switch$map13.TryGetValue(signaturealgo, out num))
						{
							HashAlgorithm hashAlgorithm;
							switch (num)
							{
							case 0:
								hashAlgorithm = HashAlgorithm.Create("MD2");
								break;
							case 1:
								hashAlgorithm = MD5.Create();
								break;
							case 2:
								hashAlgorithm = SHA1.Create();
								break;
							default:
								goto IL_BD;
							}
							if (this.decoder == null || this.decoder.Count < 1)
							{
								return null;
							}
							byte[] bytes = this.decoder[0].GetBytes();
							this.certhash = hashAlgorithm.ComputeHash(bytes, 0, bytes.Length);
							goto IL_100;
						}
					}
					IL_BD:
					return null;
				}
				IL_100:
				return (byte[])this.certhash.Clone();
			}
		}

		public virtual string IssuerName
		{
			get
			{
				return this.m_issuername;
			}
		}

		public virtual string KeyAlgorithm
		{
			get
			{
				return this.m_keyalgo;
			}
		}

		public virtual byte[] KeyAlgorithmParameters
		{
			get
			{
				if (this.m_keyalgoparams == null)
				{
					return null;
				}
				return (byte[])this.m_keyalgoparams.Clone();
			}
			set
			{
				this.m_keyalgoparams = value;
			}
		}

		public virtual byte[] PublicKey
		{
			get
			{
				if (this.m_publickey == null)
				{
					return null;
				}
				return (byte[])this.m_publickey.Clone();
			}
		}

		public virtual RSA RSA
		{
			get
			{
				if (this._rsa == null)
				{
					RSAParameters parameters = default(RSAParameters);
					ASN1 asn = new ASN1(this.m_publickey);
					ASN1 asn2 = asn[0];
					if (asn2 == null || asn2.Tag != 2)
					{
						return null;
					}
					ASN1 asn3 = asn[1];
					if (asn3.Tag != 2)
					{
						return null;
					}
					parameters.Modulus = this.GetUnsignedBigInteger(asn2.Value);
					parameters.Exponent = asn3.Value;
					int dwKeySize = parameters.Modulus.Length << 3;
					this._rsa = new RSACryptoServiceProvider(dwKeySize);
					this._rsa.ImportParameters(parameters);
				}
				return this._rsa;
			}
			set
			{
				if (value != null)
				{
					this._dsa = null;
				}
				this._rsa = value;
			}
		}

		public virtual byte[] RawData
		{
			get
			{
				if (this.m_encodedcert == null)
				{
					return null;
				}
				return (byte[])this.m_encodedcert.Clone();
			}
		}

		public virtual byte[] SerialNumber
		{
			get
			{
				if (this.serialnumber == null)
				{
					return null;
				}
				return (byte[])this.serialnumber.Clone();
			}
		}

		public virtual byte[] Signature
		{
			get
			{
				if (this.signature == null)
				{
					return null;
				}
				string signaturealgo = this.m_signaturealgo;
				if (signaturealgo != null)
				{
					if (X509Certificate.<>f__switch$map14 == null)
					{
						X509Certificate.<>f__switch$map14 = new Dictionary<string, int>(5)
						{
							{
								"1.2.840.113549.1.1.2",
								0
							},
							{
								"1.2.840.113549.1.1.4",
								0
							},
							{
								"1.2.840.113549.1.1.5",
								0
							},
							{
								"1.3.14.3.2.29",
								0
							},
							{
								"1.2.840.10040.4.3",
								1
							}
						};
					}
					int num;
					if (X509Certificate.<>f__switch$map14.TryGetValue(signaturealgo, out num))
					{
						if (num == 0)
						{
							return (byte[])this.signature.Clone();
						}
						if (num == 1)
						{
							ASN1 asn = new ASN1(this.signature);
							if (asn == null || asn.Count != 2)
							{
								return null;
							}
							byte[] value = asn[0].Value;
							byte[] value2 = asn[1].Value;
							byte[] array = new byte[40];
							int num2 = Math.Max(0, value.Length - 20);
							int dstOffset = Math.Max(0, 20 - value.Length);
							Buffer.BlockCopy(value, num2, array, dstOffset, value.Length - num2);
							int num3 = Math.Max(0, value2.Length - 20);
							int dstOffset2 = Math.Max(20, 40 - value2.Length);
							Buffer.BlockCopy(value2, num3, array, dstOffset2, value2.Length - num3);
							return array;
						}
					}
				}
				throw new CryptographicException("Unsupported hash algorithm: " + this.m_signaturealgo);
			}
		}

		public virtual string SignatureAlgorithm
		{
			get
			{
				return this.m_signaturealgo;
			}
		}

		public virtual byte[] SignatureAlgorithmParameters
		{
			get
			{
				if (this.m_signaturealgoparams == null)
				{
					return this.m_signaturealgoparams;
				}
				return (byte[])this.m_signaturealgoparams.Clone();
			}
		}

		public virtual string SubjectName
		{
			get
			{
				return this.m_subject;
			}
		}

		public virtual DateTime ValidFrom
		{
			get
			{
				return this.m_from;
			}
		}

		public virtual DateTime ValidUntil
		{
			get
			{
				return this.m_until;
			}
		}

		public int Version
		{
			get
			{
				return this.version;
			}
		}

		public bool IsCurrent
		{
			get
			{
				return this.WasCurrent(DateTime.UtcNow);
			}
		}

		public bool WasCurrent(DateTime instant)
		{
			return instant > this.ValidFrom && instant <= this.ValidUntil;
		}

		public byte[] IssuerUniqueIdentifier
		{
			get
			{
				if (this.issuerUniqueID == null)
				{
					return null;
				}
				return (byte[])this.issuerUniqueID.Clone();
			}
		}

		public byte[] SubjectUniqueIdentifier
		{
			get
			{
				if (this.subjectUniqueID == null)
				{
					return null;
				}
				return (byte[])this.subjectUniqueID.Clone();
			}
		}

		internal bool VerifySignature(DSA dsa)
		{
			DSASignatureDeformatter dsasignatureDeformatter = new DSASignatureDeformatter(dsa);
			dsasignatureDeformatter.SetHashAlgorithm("SHA1");
			return dsasignatureDeformatter.VerifySignature(this.Hash, this.Signature);
		}

		internal string GetHashNameFromOID(string oid)
		{
			switch (oid)
			{
			case "1.2.840.113549.1.1.2":
				return "MD2";
			case "1.2.840.113549.1.1.4":
				return "MD5";
			case "1.2.840.113549.1.1.5":
			case "1.3.14.3.2.29":
				return "SHA1";
			}
			return null;
		}

		internal bool VerifySignature(RSA rsa)
		{
			RSAPKCS1SignatureDeformatter rsapkcs1SignatureDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
			string hashNameFromOID = this.GetHashNameFromOID(this.m_signaturealgo);
			if (hashNameFromOID == null)
			{
				throw new CryptographicException("Unsupported hash algorithm: " + this.m_signaturealgo);
			}
			rsapkcs1SignatureDeformatter.SetHashAlgorithm(hashNameFromOID);
			return rsapkcs1SignatureDeformatter.VerifySignature(this.Hash, this.Signature);
		}

		public bool VerifySignature(AsymmetricAlgorithm aa)
		{
			if (aa == null)
			{
				throw new ArgumentNullException("aa");
			}
			if (aa is RSA)
			{
				return this.VerifySignature(aa as RSA);
			}
			if (aa is DSA)
			{
				return this.VerifySignature(aa as DSA);
			}
			throw new NotSupportedException("Unknown Asymmetric Algorithm " + aa.ToString());
		}

		public bool CheckSignature(byte[] hash, string hashAlgorithm, byte[] signature)
		{
			RSACryptoServiceProvider rsacryptoServiceProvider = (RSACryptoServiceProvider)this.RSA;
			return rsacryptoServiceProvider.VerifyHash(hash, hashAlgorithm, signature);
		}

		public bool IsSelfSigned
		{
			get
			{
				return this.m_issuername == this.m_subject && this.VerifySignature(this.RSA);
			}
		}

		public ASN1 GetIssuerName()
		{
			return this.issuer;
		}

		public ASN1 GetSubjectName()
		{
			return this.subject;
		}

		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("raw", this.m_encodedcert);
		}

		private static byte[] PEM(string type, byte[] data)
		{
			string @string = Encoding.ASCII.GetString(data);
			string text = string.Format("-----BEGIN {0}-----", type);
			string value = string.Format("-----END {0}-----", type);
			int num = @string.IndexOf(text) + text.Length;
			int num2 = @string.IndexOf(value, num);
			string s = @string.Substring(num, num2 - num);
			return Convert.FromBase64String(s);
		}
	}
}
