using Mono.Security.Cryptography;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Mono.Security.X509
{
	public class PKCS12 : ICloneable
	{
		public const string pbeWithSHAAnd128BitRC4 = "1.2.840.113549.1.12.1.1";

		public const string pbeWithSHAAnd40BitRC4 = "1.2.840.113549.1.12.1.2";

		public const string pbeWithSHAAnd3KeyTripleDESCBC = "1.2.840.113549.1.12.1.3";

		public const string pbeWithSHAAnd2KeyTripleDESCBC = "1.2.840.113549.1.12.1.4";

		public const string pbeWithSHAAnd128BitRC2CBC = "1.2.840.113549.1.12.1.5";

		public const string pbeWithSHAAnd40BitRC2CBC = "1.2.840.113549.1.12.1.6";

		public const string keyBag = "1.2.840.113549.1.12.10.1.1";

		public const string pkcs8ShroudedKeyBag = "1.2.840.113549.1.12.10.1.2";

		public const string certBag = "1.2.840.113549.1.12.10.1.3";

		public const string crlBag = "1.2.840.113549.1.12.10.1.4";

		public const string secretBag = "1.2.840.113549.1.12.10.1.5";

		public const string safeContentsBag = "1.2.840.113549.1.12.10.1.6";

		public const string x509Certificate = "1.2.840.113549.1.9.22.1";

		public const string sdsiCertificate = "1.2.840.113549.1.9.22.2";

		public const string x509Crl = "1.2.840.113549.1.9.23.1";

		public const int CryptoApiPasswordLimit = 32;

		private static int recommendedIterationCount = 2000;

		private byte[] _password;

		private ArrayList _keyBags;

		private ArrayList _secretBags;

		private X509CertificateCollection _certs;

		private bool _keyBagsChanged;

		private bool _secretBagsChanged;

		private bool _certsChanged;

		private int _iterations;

		private ArrayList _safeBags;

		private RandomNumberGenerator _rng;

		private static int password_max_length = int.MaxValue;

		public PKCS12()
		{
			this._iterations = PKCS12.recommendedIterationCount;
			this._keyBags = new ArrayList();
			this._secretBags = new ArrayList();
			this._certs = new X509CertificateCollection();
			this._keyBagsChanged = false;
			this._secretBagsChanged = false;
			this._certsChanged = false;
			this._safeBags = new ArrayList();
		}

		public PKCS12(byte[] data) : this()
		{
			this.Password = null;
			this.Decode(data);
		}

		public PKCS12(byte[] data, string password) : this()
		{
			this.Password = password;
			this.Decode(data);
		}

		public PKCS12(byte[] data, byte[] password) : this()
		{
			this._password = password;
			this.Decode(data);
		}

		private void Decode(byte[] data)
		{
			ASN1 asn = new ASN1(data);
			if (asn.Tag != 48)
			{
				throw new ArgumentException("invalid data");
			}
			ASN1 asn2 = asn[0];
			if (asn2.Tag != 2)
			{
				throw new ArgumentException("invalid PFX version");
			}
			PKCS7.ContentInfo contentInfo = new PKCS7.ContentInfo(asn[1]);
			if (contentInfo.ContentType != "1.2.840.113549.1.7.1")
			{
				throw new ArgumentException("invalid authenticated safe");
			}
			if (asn.Count > 2)
			{
				ASN1 asn3 = asn[2];
				if (asn3.Tag != 48)
				{
					throw new ArgumentException("invalid MAC");
				}
				ASN1 asn4 = asn3[0];
				if (asn4.Tag != 48)
				{
					throw new ArgumentException("invalid MAC");
				}
				ASN1 asn5 = asn4[0];
				string a = ASN1Convert.ToOid(asn5[0]);
				if (a != "1.3.14.3.2.26")
				{
					throw new ArgumentException("unsupported HMAC");
				}
				byte[] value = asn4[1].Value;
				ASN1 asn6 = asn3[1];
				if (asn6.Tag != 4)
				{
					throw new ArgumentException("missing MAC salt");
				}
				this._iterations = 1;
				if (asn3.Count > 2)
				{
					ASN1 asn7 = asn3[2];
					if (asn7.Tag != 2)
					{
						throw new ArgumentException("invalid MAC iteration");
					}
					this._iterations = ASN1Convert.ToInt32(asn7);
				}
				byte[] value2 = contentInfo.Content[0].Value;
				byte[] actual = this.MAC(this._password, asn6.Value, this._iterations, value2);
				if (!this.Compare(value, actual))
				{
					throw new CryptographicException("Invalid MAC - file may have been tampered!");
				}
			}
			ASN1 asn8 = new ASN1(contentInfo.Content[0].Value);
			int i = 0;
			while (i < asn8.Count)
			{
				PKCS7.ContentInfo contentInfo2 = new PKCS7.ContentInfo(asn8[i]);
				string contentType = contentInfo2.ContentType;
				if (contentType != null)
				{
					if (PKCS12.<>f__switch$map5 == null)
					{
						PKCS12.<>f__switch$map5 = new Dictionary<string, int>(3)
						{
							{
								"1.2.840.113549.1.7.1",
								0
							},
							{
								"1.2.840.113549.1.7.6",
								1
							},
							{
								"1.2.840.113549.1.7.3",
								2
							}
						};
					}
					int num;
					if (PKCS12.<>f__switch$map5.TryGetValue(contentType, out num))
					{
						switch (num)
						{
						case 0:
						{
							ASN1 asn9 = new ASN1(contentInfo2.Content[0].Value);
							for (int j = 0; j < asn9.Count; j++)
							{
								ASN1 safeBag = asn9[j];
								this.ReadSafeBag(safeBag);
							}
							break;
						}
						case 1:
						{
							PKCS7.EncryptedData ed = new PKCS7.EncryptedData(contentInfo2.Content[0]);
							ASN1 asn10 = new ASN1(this.Decrypt(ed));
							for (int k = 0; k < asn10.Count; k++)
							{
								ASN1 safeBag2 = asn10[k];
								this.ReadSafeBag(safeBag2);
							}
							break;
						}
						case 2:
							throw new NotImplementedException("public key encrypted");
						default:
							goto IL_303;
						}
						i++;
						continue;
					}
				}
				IL_303:
				throw new ArgumentException("unknown authenticatedSafe");
			}
		}

		~PKCS12()
		{
			if (this._password != null)
			{
				Array.Clear(this._password, 0, this._password.Length);
			}
			this._password = null;
		}

		public string Password
		{
			set
			{
				if (value != null)
				{
					if (value.Length > 0)
					{
						int num = value.Length;
						int num2 = 0;
						if (num < PKCS12.MaximumPasswordLength)
						{
							if (value[num - 1] != '\0')
							{
								num2 = 1;
							}
						}
						else
						{
							num = PKCS12.MaximumPasswordLength;
						}
						this._password = new byte[num + num2 << 1];
						Encoding.BigEndianUnicode.GetBytes(value, 0, num, this._password, 0);
					}
					else
					{
						this._password = new byte[2];
					}
				}
				else
				{
					this._password = null;
				}
			}
		}

		public int IterationCount
		{
			get
			{
				return this._iterations;
			}
			set
			{
				this._iterations = value;
			}
		}

		public ArrayList Keys
		{
			get
			{
				if (this._keyBagsChanged)
				{
					this._keyBags.Clear();
					foreach (object obj in this._safeBags)
					{
						SafeBag safeBag = (SafeBag)obj;
						if (safeBag.BagOID.Equals("1.2.840.113549.1.12.10.1.1"))
						{
							ASN1 asn = safeBag.ASN1;
							ASN1 asn2 = asn[1];
							PKCS8.PrivateKeyInfo privateKeyInfo = new PKCS8.PrivateKeyInfo(asn2.Value);
							byte[] privateKey = privateKeyInfo.PrivateKey;
							byte b = privateKey[0];
							if (b != 2)
							{
								if (b == 48)
								{
									this._keyBags.Add(PKCS8.PrivateKeyInfo.DecodeRSA(privateKey));
								}
							}
							else
							{
								DSAParameters dsaParameters = default(DSAParameters);
								this._keyBags.Add(PKCS8.PrivateKeyInfo.DecodeDSA(privateKey, dsaParameters));
							}
							Array.Clear(privateKey, 0, privateKey.Length);
						}
						else if (safeBag.BagOID.Equals("1.2.840.113549.1.12.10.1.2"))
						{
							ASN1 asn3 = safeBag.ASN1;
							ASN1 asn4 = asn3[1];
							PKCS8.EncryptedPrivateKeyInfo encryptedPrivateKeyInfo = new PKCS8.EncryptedPrivateKeyInfo(asn4.Value);
							byte[] array = this.Decrypt(encryptedPrivateKeyInfo.Algorithm, encryptedPrivateKeyInfo.Salt, encryptedPrivateKeyInfo.IterationCount, encryptedPrivateKeyInfo.EncryptedData);
							PKCS8.PrivateKeyInfo privateKeyInfo2 = new PKCS8.PrivateKeyInfo(array);
							byte[] privateKey2 = privateKeyInfo2.PrivateKey;
							byte b = privateKey2[0];
							if (b != 2)
							{
								if (b == 48)
								{
									this._keyBags.Add(PKCS8.PrivateKeyInfo.DecodeRSA(privateKey2));
								}
							}
							else
							{
								DSAParameters dsaParameters2 = default(DSAParameters);
								this._keyBags.Add(PKCS8.PrivateKeyInfo.DecodeDSA(privateKey2, dsaParameters2));
							}
							Array.Clear(privateKey2, 0, privateKey2.Length);
							Array.Clear(array, 0, array.Length);
						}
					}
					this._keyBagsChanged = false;
				}
				return ArrayList.ReadOnly(this._keyBags);
			}
		}

		public ArrayList Secrets
		{
			get
			{
				if (this._secretBagsChanged)
				{
					this._secretBags.Clear();
					foreach (object obj in this._safeBags)
					{
						SafeBag safeBag = (SafeBag)obj;
						if (safeBag.BagOID.Equals("1.2.840.113549.1.12.10.1.5"))
						{
							ASN1 asn = safeBag.ASN1;
							ASN1 asn2 = asn[1];
							byte[] value = asn2.Value;
							this._secretBags.Add(value);
						}
					}
					this._secretBagsChanged = false;
				}
				return ArrayList.ReadOnly(this._secretBags);
			}
		}

		public X509CertificateCollection Certificates
		{
			get
			{
				if (this._certsChanged)
				{
					this._certs.Clear();
					foreach (object obj in this._safeBags)
					{
						SafeBag safeBag = (SafeBag)obj;
						if (safeBag.BagOID.Equals("1.2.840.113549.1.12.10.1.3"))
						{
							ASN1 asn = safeBag.ASN1;
							ASN1 asn2 = asn[1];
							PKCS7.ContentInfo contentInfo = new PKCS7.ContentInfo(asn2.Value);
							this._certs.Add(new X509Certificate(contentInfo.Content[0].Value));
						}
					}
					this._certsChanged = false;
				}
				return this._certs;
			}
		}

		internal RandomNumberGenerator RNG
		{
			get
			{
				if (this._rng == null)
				{
					this._rng = RandomNumberGenerator.Create();
				}
				return this._rng;
			}
		}

		private bool Compare(byte[] expected, byte[] actual)
		{
			bool result = false;
			if (expected.Length == actual.Length)
			{
				for (int i = 0; i < expected.Length; i++)
				{
					if (expected[i] != actual[i])
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		private SymmetricAlgorithm GetSymmetricAlgorithm(string algorithmOid, byte[] salt, int iterationCount)
		{
			string text = null;
			int size = 8;
			int num = 8;
			PKCS12.DeriveBytes deriveBytes = new PKCS12.DeriveBytes();
			deriveBytes.Password = this._password;
			deriveBytes.Salt = salt;
			deriveBytes.IterationCount = iterationCount;
			if (algorithmOid != null)
			{
				if (PKCS12.<>f__switch$map6 == null)
				{
					PKCS12.<>f__switch$map6 = new Dictionary<string, int>(12)
					{
						{
							"1.2.840.113549.1.5.1",
							0
						},
						{
							"1.2.840.113549.1.5.3",
							1
						},
						{
							"1.2.840.113549.1.5.4",
							2
						},
						{
							"1.2.840.113549.1.5.6",
							3
						},
						{
							"1.2.840.113549.1.5.10",
							4
						},
						{
							"1.2.840.113549.1.5.11",
							5
						},
						{
							"1.2.840.113549.1.12.1.1",
							6
						},
						{
							"1.2.840.113549.1.12.1.2",
							7
						},
						{
							"1.2.840.113549.1.12.1.3",
							8
						},
						{
							"1.2.840.113549.1.12.1.4",
							9
						},
						{
							"1.2.840.113549.1.12.1.5",
							10
						},
						{
							"1.2.840.113549.1.12.1.6",
							11
						}
					};
				}
				int num2;
				if (PKCS12.<>f__switch$map6.TryGetValue(algorithmOid, out num2))
				{
					switch (num2)
					{
					case 0:
						deriveBytes.HashName = "MD2";
						text = "DES";
						break;
					case 1:
						deriveBytes.HashName = "MD5";
						text = "DES";
						break;
					case 2:
						deriveBytes.HashName = "MD2";
						text = "RC2";
						size = 4;
						break;
					case 3:
						deriveBytes.HashName = "MD5";
						text = "RC2";
						size = 4;
						break;
					case 4:
						deriveBytes.HashName = "SHA1";
						text = "DES";
						break;
					case 5:
						deriveBytes.HashName = "SHA1";
						text = "RC2";
						size = 4;
						break;
					case 6:
						deriveBytes.HashName = "SHA1";
						text = "RC4";
						size = 16;
						num = 0;
						break;
					case 7:
						deriveBytes.HashName = "SHA1";
						text = "RC4";
						size = 5;
						num = 0;
						break;
					case 8:
						deriveBytes.HashName = "SHA1";
						text = "TripleDES";
						size = 24;
						break;
					case 9:
						deriveBytes.HashName = "SHA1";
						text = "TripleDES";
						size = 16;
						break;
					case 10:
						deriveBytes.HashName = "SHA1";
						text = "RC2";
						size = 16;
						break;
					case 11:
						deriveBytes.HashName = "SHA1";
						text = "RC2";
						size = 5;
						break;
					default:
						goto IL_25A;
					}
					SymmetricAlgorithm symmetricAlgorithm = SymmetricAlgorithm.Create(text);
					symmetricAlgorithm.Key = deriveBytes.DeriveKey(size);
					if (num > 0)
					{
						symmetricAlgorithm.IV = deriveBytes.DeriveIV(num);
						symmetricAlgorithm.Mode = CipherMode.CBC;
					}
					return symmetricAlgorithm;
				}
			}
			IL_25A:
			throw new NotSupportedException("unknown oid " + text);
		}

		public byte[] Decrypt(string algorithmOid, byte[] salt, int iterationCount, byte[] encryptedData)
		{
			SymmetricAlgorithm symmetricAlgorithm = null;
			byte[] result = null;
			try
			{
				symmetricAlgorithm = this.GetSymmetricAlgorithm(algorithmOid, salt, iterationCount);
				ICryptoTransform cryptoTransform = symmetricAlgorithm.CreateDecryptor();
				result = cryptoTransform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
			}
			finally
			{
				if (symmetricAlgorithm != null)
				{
					symmetricAlgorithm.Clear();
				}
			}
			return result;
		}

		public byte[] Decrypt(PKCS7.EncryptedData ed)
		{
			return this.Decrypt(ed.EncryptionAlgorithm.ContentType, ed.EncryptionAlgorithm.Content[0].Value, ASN1Convert.ToInt32(ed.EncryptionAlgorithm.Content[1]), ed.EncryptedContent);
		}

		public byte[] Encrypt(string algorithmOid, byte[] salt, int iterationCount, byte[] data)
		{
			byte[] result = null;
			using (SymmetricAlgorithm symmetricAlgorithm = this.GetSymmetricAlgorithm(algorithmOid, salt, iterationCount))
			{
				ICryptoTransform cryptoTransform = symmetricAlgorithm.CreateEncryptor();
				result = cryptoTransform.TransformFinalBlock(data, 0, data.Length);
			}
			return result;
		}

		private DSAParameters GetExistingParameters(out bool found)
		{
			foreach (X509Certificate x509Certificate in this.Certificates)
			{
				if (x509Certificate.KeyAlgorithmParameters != null)
				{
					DSA dsa = x509Certificate.DSA;
					if (dsa != null)
					{
						found = true;
						return dsa.ExportParameters(false);
					}
				}
			}
			found = false;
			return default(DSAParameters);
		}

		private void AddPrivateKey(PKCS8.PrivateKeyInfo pki)
		{
			byte[] privateKey = pki.PrivateKey;
			byte b = privateKey[0];
			if (b != 2)
			{
				if (b != 48)
				{
					Array.Clear(privateKey, 0, privateKey.Length);
					throw new CryptographicException("Unknown private key format");
				}
				this._keyBags.Add(PKCS8.PrivateKeyInfo.DecodeRSA(privateKey));
			}
			else
			{
				bool flag;
				DSAParameters existingParameters = this.GetExistingParameters(out flag);
				if (flag)
				{
					this._keyBags.Add(PKCS8.PrivateKeyInfo.DecodeDSA(privateKey, existingParameters));
				}
			}
			Array.Clear(privateKey, 0, privateKey.Length);
		}

		private void ReadSafeBag(ASN1 safeBag)
		{
			if (safeBag.Tag != 48)
			{
				throw new ArgumentException("invalid safeBag");
			}
			ASN1 asn = safeBag[0];
			if (asn.Tag != 6)
			{
				throw new ArgumentException("invalid safeBag id");
			}
			ASN1 asn2 = safeBag[1];
			string text = ASN1Convert.ToOid(asn);
			string text2 = text;
			if (text2 != null)
			{
				if (PKCS12.<>f__switch$map7 == null)
				{
					PKCS12.<>f__switch$map7 = new Dictionary<string, int>(6)
					{
						{
							"1.2.840.113549.1.12.10.1.1",
							0
						},
						{
							"1.2.840.113549.1.12.10.1.2",
							1
						},
						{
							"1.2.840.113549.1.12.10.1.3",
							2
						},
						{
							"1.2.840.113549.1.12.10.1.4",
							3
						},
						{
							"1.2.840.113549.1.12.10.1.5",
							4
						},
						{
							"1.2.840.113549.1.12.10.1.6",
							5
						}
					};
				}
				int num;
				if (PKCS12.<>f__switch$map7.TryGetValue(text2, out num))
				{
					switch (num)
					{
					case 0:
						this.AddPrivateKey(new PKCS8.PrivateKeyInfo(asn2.Value));
						break;
					case 1:
					{
						PKCS8.EncryptedPrivateKeyInfo encryptedPrivateKeyInfo = new PKCS8.EncryptedPrivateKeyInfo(asn2.Value);
						byte[] array = this.Decrypt(encryptedPrivateKeyInfo.Algorithm, encryptedPrivateKeyInfo.Salt, encryptedPrivateKeyInfo.IterationCount, encryptedPrivateKeyInfo.EncryptedData);
						this.AddPrivateKey(new PKCS8.PrivateKeyInfo(array));
						Array.Clear(array, 0, array.Length);
						break;
					}
					case 2:
					{
						PKCS7.ContentInfo contentInfo = new PKCS7.ContentInfo(asn2.Value);
						if (contentInfo.ContentType != "1.2.840.113549.1.9.22.1")
						{
							throw new NotSupportedException("unsupport certificate type");
						}
						X509Certificate value = new X509Certificate(contentInfo.Content[0].Value);
						this._certs.Add(value);
						break;
					}
					case 3:
						break;
					case 4:
					{
						byte[] value2 = asn2.Value;
						this._secretBags.Add(value2);
						break;
					}
					case 5:
						break;
					default:
						goto IL_1CD;
					}
					if (safeBag.Count > 2)
					{
						ASN1 asn3 = safeBag[2];
						if (asn3.Tag != 49)
						{
							throw new ArgumentException("invalid safeBag attributes id");
						}
						for (int i = 0; i < asn3.Count; i++)
						{
							ASN1 asn4 = asn3[i];
							if (asn4.Tag != 48)
							{
								throw new ArgumentException("invalid PKCS12 attributes id");
							}
							ASN1 asn5 = asn4[0];
							if (asn5.Tag != 6)
							{
								throw new ArgumentException("invalid attribute id");
							}
							string text3 = ASN1Convert.ToOid(asn5);
							ASN1 asn6 = asn4[1];
							int j = 0;
							while (j < asn6.Count)
							{
								ASN1 asn7 = asn6[j];
								text2 = text3;
								if (text2 != null)
								{
									if (PKCS12.<>f__switch$map8 == null)
									{
										PKCS12.<>f__switch$map8 = new Dictionary<string, int>(2)
										{
											{
												"1.2.840.113549.1.9.20",
												0
											},
											{
												"1.2.840.113549.1.9.21",
												1
											}
										};
									}
									if (PKCS12.<>f__switch$map8.TryGetValue(text2, out num))
									{
										if (num != 0)
										{
											if (num == 1)
											{
												if (asn7.Tag != 4)
												{
													throw new ArgumentException("invalid attribute value id");
												}
											}
										}
										else if (asn7.Tag != 30)
										{
											throw new ArgumentException("invalid attribute value id");
										}
									}
								}
								IL_31F:
								j++;
								continue;
								goto IL_31F;
							}
						}
					}
					this._safeBags.Add(new SafeBag(text, safeBag));
					return;
				}
			}
			IL_1CD:
			throw new ArgumentException("unknown safeBag oid");
		}

		private ASN1 Pkcs8ShroudedKeyBagSafeBag(AsymmetricAlgorithm aa, IDictionary attributes)
		{
			PKCS8.PrivateKeyInfo privateKeyInfo = new PKCS8.PrivateKeyInfo();
			if (aa is RSA)
			{
				privateKeyInfo.Algorithm = "1.2.840.113549.1.1.1";
				privateKeyInfo.PrivateKey = PKCS8.PrivateKeyInfo.Encode((RSA)aa);
			}
			else
			{
				if (!(aa is DSA))
				{
					throw new CryptographicException("Unknown asymmetric algorithm {0}", aa.ToString());
				}
				privateKeyInfo.Algorithm = null;
				privateKeyInfo.PrivateKey = PKCS8.PrivateKeyInfo.Encode((DSA)aa);
			}
			PKCS8.EncryptedPrivateKeyInfo encryptedPrivateKeyInfo = new PKCS8.EncryptedPrivateKeyInfo();
			encryptedPrivateKeyInfo.Algorithm = "1.2.840.113549.1.12.1.3";
			encryptedPrivateKeyInfo.IterationCount = this._iterations;
			encryptedPrivateKeyInfo.EncryptedData = this.Encrypt("1.2.840.113549.1.12.1.3", encryptedPrivateKeyInfo.Salt, this._iterations, privateKeyInfo.GetBytes());
			ASN1 asn = new ASN1(48);
			asn.Add(ASN1Convert.FromOid("1.2.840.113549.1.12.10.1.2"));
			ASN1 asn2 = new ASN1(160);
			asn2.Add(new ASN1(encryptedPrivateKeyInfo.GetBytes()));
			asn.Add(asn2);
			if (attributes != null)
			{
				ASN1 asn3 = new ASN1(49);
				IDictionaryEnumerator enumerator = attributes.GetEnumerator();
				while (enumerator.MoveNext())
				{
					string text = (string)enumerator.Key;
					string text2 = text;
					if (text2 != null)
					{
						if (PKCS12.<>f__switch$map9 == null)
						{
							PKCS12.<>f__switch$map9 = new Dictionary<string, int>(2)
							{
								{
									"1.2.840.113549.1.9.20",
									0
								},
								{
									"1.2.840.113549.1.9.21",
									1
								}
							};
						}
						int num;
						if (PKCS12.<>f__switch$map9.TryGetValue(text2, out num))
						{
							if (num != 0)
							{
								if (num == 1)
								{
									ArrayList arrayList = (ArrayList)enumerator.Value;
									if (arrayList.Count > 0)
									{
										ASN1 asn4 = new ASN1(48);
										asn4.Add(ASN1Convert.FromOid("1.2.840.113549.1.9.21"));
										ASN1 asn5 = new ASN1(49);
										foreach (object obj in arrayList)
										{
											byte[] value = (byte[])obj;
											asn5.Add(new ASN1(4)
											{
												Value = value
											});
										}
										asn4.Add(asn5);
										asn3.Add(asn4);
									}
								}
							}
							else
							{
								ArrayList arrayList2 = (ArrayList)enumerator.Value;
								if (arrayList2.Count > 0)
								{
									ASN1 asn6 = new ASN1(48);
									asn6.Add(ASN1Convert.FromOid("1.2.840.113549.1.9.20"));
									ASN1 asn7 = new ASN1(49);
									foreach (object obj2 in arrayList2)
									{
										byte[] value2 = (byte[])obj2;
										asn7.Add(new ASN1(30)
										{
											Value = value2
										});
									}
									asn6.Add(asn7);
									asn3.Add(asn6);
								}
							}
						}
					}
				}
				if (asn3.Count > 0)
				{
					asn.Add(asn3);
				}
			}
			return asn;
		}

		private ASN1 KeyBagSafeBag(AsymmetricAlgorithm aa, IDictionary attributes)
		{
			PKCS8.PrivateKeyInfo privateKeyInfo = new PKCS8.PrivateKeyInfo();
			if (aa is RSA)
			{
				privateKeyInfo.Algorithm = "1.2.840.113549.1.1.1";
				privateKeyInfo.PrivateKey = PKCS8.PrivateKeyInfo.Encode((RSA)aa);
			}
			else
			{
				if (!(aa is DSA))
				{
					throw new CryptographicException("Unknown asymmetric algorithm {0}", aa.ToString());
				}
				privateKeyInfo.Algorithm = null;
				privateKeyInfo.PrivateKey = PKCS8.PrivateKeyInfo.Encode((DSA)aa);
			}
			ASN1 asn = new ASN1(48);
			asn.Add(ASN1Convert.FromOid("1.2.840.113549.1.12.10.1.1"));
			ASN1 asn2 = new ASN1(160);
			asn2.Add(new ASN1(privateKeyInfo.GetBytes()));
			asn.Add(asn2);
			if (attributes != null)
			{
				ASN1 asn3 = new ASN1(49);
				IDictionaryEnumerator enumerator = attributes.GetEnumerator();
				while (enumerator.MoveNext())
				{
					string text = (string)enumerator.Key;
					string text2 = text;
					if (text2 != null)
					{
						if (PKCS12.<>f__switch$mapA == null)
						{
							PKCS12.<>f__switch$mapA = new Dictionary<string, int>(2)
							{
								{
									"1.2.840.113549.1.9.20",
									0
								},
								{
									"1.2.840.113549.1.9.21",
									1
								}
							};
						}
						int num;
						if (PKCS12.<>f__switch$mapA.TryGetValue(text2, out num))
						{
							if (num != 0)
							{
								if (num == 1)
								{
									ArrayList arrayList = (ArrayList)enumerator.Value;
									if (arrayList.Count > 0)
									{
										ASN1 asn4 = new ASN1(48);
										asn4.Add(ASN1Convert.FromOid("1.2.840.113549.1.9.21"));
										ASN1 asn5 = new ASN1(49);
										foreach (object obj in arrayList)
										{
											byte[] value = (byte[])obj;
											asn5.Add(new ASN1(4)
											{
												Value = value
											});
										}
										asn4.Add(asn5);
										asn3.Add(asn4);
									}
								}
							}
							else
							{
								ArrayList arrayList2 = (ArrayList)enumerator.Value;
								if (arrayList2.Count > 0)
								{
									ASN1 asn6 = new ASN1(48);
									asn6.Add(ASN1Convert.FromOid("1.2.840.113549.1.9.20"));
									ASN1 asn7 = new ASN1(49);
									foreach (object obj2 in arrayList2)
									{
										byte[] value2 = (byte[])obj2;
										asn7.Add(new ASN1(30)
										{
											Value = value2
										});
									}
									asn6.Add(asn7);
									asn3.Add(asn6);
								}
							}
						}
					}
				}
				if (asn3.Count > 0)
				{
					asn.Add(asn3);
				}
			}
			return asn;
		}

		private ASN1 SecretBagSafeBag(byte[] secret, IDictionary attributes)
		{
			ASN1 asn = new ASN1(48);
			asn.Add(ASN1Convert.FromOid("1.2.840.113549.1.12.10.1.5"));
			ASN1 asn2 = new ASN1(128, secret);
			asn.Add(asn2);
			if (attributes != null)
			{
				ASN1 asn3 = new ASN1(49);
				IDictionaryEnumerator enumerator = attributes.GetEnumerator();
				while (enumerator.MoveNext())
				{
					string text = (string)enumerator.Key;
					string text2 = text;
					if (text2 != null)
					{
						if (PKCS12.<>f__switch$mapB == null)
						{
							PKCS12.<>f__switch$mapB = new Dictionary<string, int>(2)
							{
								{
									"1.2.840.113549.1.9.20",
									0
								},
								{
									"1.2.840.113549.1.9.21",
									1
								}
							};
						}
						int num;
						if (PKCS12.<>f__switch$mapB.TryGetValue(text2, out num))
						{
							if (num != 0)
							{
								if (num == 1)
								{
									ArrayList arrayList = (ArrayList)enumerator.Value;
									if (arrayList.Count > 0)
									{
										ASN1 asn4 = new ASN1(48);
										asn4.Add(ASN1Convert.FromOid("1.2.840.113549.1.9.21"));
										ASN1 asn5 = new ASN1(49);
										foreach (object obj in arrayList)
										{
											byte[] value = (byte[])obj;
											asn5.Add(new ASN1(4)
											{
												Value = value
											});
										}
										asn4.Add(asn5);
										asn3.Add(asn4);
									}
								}
							}
							else
							{
								ArrayList arrayList2 = (ArrayList)enumerator.Value;
								if (arrayList2.Count > 0)
								{
									ASN1 asn6 = new ASN1(48);
									asn6.Add(ASN1Convert.FromOid("1.2.840.113549.1.9.20"));
									ASN1 asn7 = new ASN1(49);
									foreach (object obj2 in arrayList2)
									{
										byte[] value2 = (byte[])obj2;
										asn7.Add(new ASN1(30)
										{
											Value = value2
										});
									}
									asn6.Add(asn7);
									asn3.Add(asn6);
								}
							}
						}
					}
				}
				if (asn3.Count > 0)
				{
					asn.Add(asn3);
				}
			}
			return asn;
		}

		private ASN1 CertificateSafeBag(X509Certificate x509, IDictionary attributes)
		{
			ASN1 asn = new ASN1(4, x509.RawData);
			PKCS7.ContentInfo contentInfo = new PKCS7.ContentInfo();
			contentInfo.ContentType = "1.2.840.113549.1.9.22.1";
			contentInfo.Content.Add(asn);
			ASN1 asn2 = new ASN1(160);
			asn2.Add(contentInfo.ASN1);
			ASN1 asn3 = new ASN1(48);
			asn3.Add(ASN1Convert.FromOid("1.2.840.113549.1.12.10.1.3"));
			asn3.Add(asn2);
			if (attributes != null)
			{
				ASN1 asn4 = new ASN1(49);
				IDictionaryEnumerator enumerator = attributes.GetEnumerator();
				while (enumerator.MoveNext())
				{
					string text = (string)enumerator.Key;
					string text2 = text;
					if (text2 != null)
					{
						if (PKCS12.<>f__switch$mapC == null)
						{
							PKCS12.<>f__switch$mapC = new Dictionary<string, int>(2)
							{
								{
									"1.2.840.113549.1.9.20",
									0
								},
								{
									"1.2.840.113549.1.9.21",
									1
								}
							};
						}
						int num;
						if (PKCS12.<>f__switch$mapC.TryGetValue(text2, out num))
						{
							if (num != 0)
							{
								if (num == 1)
								{
									ArrayList arrayList = (ArrayList)enumerator.Value;
									if (arrayList.Count > 0)
									{
										ASN1 asn5 = new ASN1(48);
										asn5.Add(ASN1Convert.FromOid("1.2.840.113549.1.9.21"));
										ASN1 asn6 = new ASN1(49);
										foreach (object obj in arrayList)
										{
											byte[] value = (byte[])obj;
											asn6.Add(new ASN1(4)
											{
												Value = value
											});
										}
										asn5.Add(asn6);
										asn4.Add(asn5);
									}
								}
							}
							else
							{
								ArrayList arrayList2 = (ArrayList)enumerator.Value;
								if (arrayList2.Count > 0)
								{
									ASN1 asn7 = new ASN1(48);
									asn7.Add(ASN1Convert.FromOid("1.2.840.113549.1.9.20"));
									ASN1 asn8 = new ASN1(49);
									foreach (object obj2 in arrayList2)
									{
										byte[] value2 = (byte[])obj2;
										asn8.Add(new ASN1(30)
										{
											Value = value2
										});
									}
									asn7.Add(asn8);
									asn4.Add(asn7);
								}
							}
						}
					}
				}
				if (asn4.Count > 0)
				{
					asn3.Add(asn4);
				}
			}
			return asn3;
		}

		private byte[] MAC(byte[] password, byte[] salt, int iterations, byte[] data)
		{
			PKCS12.DeriveBytes deriveBytes = new PKCS12.DeriveBytes();
			deriveBytes.HashName = "SHA1";
			deriveBytes.Password = password;
			deriveBytes.Salt = salt;
			deriveBytes.IterationCount = iterations;
			HMACSHA1 hmacsha = (HMACSHA1)System.Security.Cryptography.HMAC.Create();
			hmacsha.Key = deriveBytes.DeriveMAC(20);
			return hmacsha.ComputeHash(data, 0, data.Length);
		}

		public byte[] GetBytes()
		{
			ASN1 asn = new ASN1(48);
			ArrayList arrayList = new ArrayList();
			foreach (object obj in this._safeBags)
			{
				SafeBag safeBag = (SafeBag)obj;
				if (safeBag.BagOID.Equals("1.2.840.113549.1.12.10.1.3"))
				{
					ASN1 asn2 = safeBag.ASN1;
					ASN1 asn3 = asn2[1];
					PKCS7.ContentInfo contentInfo = new PKCS7.ContentInfo(asn3.Value);
					arrayList.Add(new X509Certificate(contentInfo.Content[0].Value));
				}
			}
			ArrayList arrayList2 = new ArrayList();
			ArrayList arrayList3 = new ArrayList();
			foreach (X509Certificate x509Certificate in this.Certificates)
			{
				bool flag = false;
				foreach (object obj2 in arrayList)
				{
					X509Certificate x509Certificate2 = (X509Certificate)obj2;
					if (this.Compare(x509Certificate.RawData, x509Certificate2.RawData))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					arrayList2.Add(x509Certificate);
				}
			}
			foreach (object obj3 in arrayList)
			{
				X509Certificate x509Certificate3 = (X509Certificate)obj3;
				bool flag2 = false;
				foreach (X509Certificate x509Certificate4 in this.Certificates)
				{
					if (this.Compare(x509Certificate3.RawData, x509Certificate4.RawData))
					{
						flag2 = true;
					}
				}
				if (!flag2)
				{
					arrayList3.Add(x509Certificate3);
				}
			}
			foreach (object obj4 in arrayList3)
			{
				X509Certificate cert = (X509Certificate)obj4;
				this.RemoveCertificate(cert);
			}
			foreach (object obj5 in arrayList2)
			{
				X509Certificate cert2 = (X509Certificate)obj5;
				this.AddCertificate(cert2);
			}
			if (this._safeBags.Count > 0)
			{
				ASN1 asn4 = new ASN1(48);
				foreach (object obj6 in this._safeBags)
				{
					SafeBag safeBag2 = (SafeBag)obj6;
					if (safeBag2.BagOID.Equals("1.2.840.113549.1.12.10.1.3"))
					{
						asn4.Add(safeBag2.ASN1);
					}
				}
				if (asn4.Count > 0)
				{
					PKCS7.ContentInfo contentInfo2 = this.EncryptedContentInfo(asn4, "1.2.840.113549.1.12.1.3");
					asn.Add(contentInfo2.ASN1);
				}
			}
			if (this._safeBags.Count > 0)
			{
				ASN1 asn5 = new ASN1(48);
				foreach (object obj7 in this._safeBags)
				{
					SafeBag safeBag3 = (SafeBag)obj7;
					if (safeBag3.BagOID.Equals("1.2.840.113549.1.12.10.1.1") || safeBag3.BagOID.Equals("1.2.840.113549.1.12.10.1.2"))
					{
						asn5.Add(safeBag3.ASN1);
					}
				}
				if (asn5.Count > 0)
				{
					ASN1 asn6 = new ASN1(160);
					asn6.Add(new ASN1(4, asn5.GetBytes()));
					asn.Add(new PKCS7.ContentInfo("1.2.840.113549.1.7.1")
					{
						Content = asn6
					}.ASN1);
				}
			}
			if (this._safeBags.Count > 0)
			{
				ASN1 asn7 = new ASN1(48);
				foreach (object obj8 in this._safeBags)
				{
					SafeBag safeBag4 = (SafeBag)obj8;
					if (safeBag4.BagOID.Equals("1.2.840.113549.1.12.10.1.5"))
					{
						asn7.Add(safeBag4.ASN1);
					}
				}
				if (asn7.Count > 0)
				{
					PKCS7.ContentInfo contentInfo3 = this.EncryptedContentInfo(asn7, "1.2.840.113549.1.12.1.3");
					asn.Add(contentInfo3.ASN1);
				}
			}
			ASN1 asn8 = new ASN1(4, asn.GetBytes());
			ASN1 asn9 = new ASN1(160);
			asn9.Add(asn8);
			PKCS7.ContentInfo contentInfo4 = new PKCS7.ContentInfo("1.2.840.113549.1.7.1");
			contentInfo4.Content = asn9;
			ASN1 asn10 = new ASN1(48);
			if (this._password != null)
			{
				byte[] array = new byte[20];
				this.RNG.GetBytes(array);
				byte[] data = this.MAC(this._password, array, this._iterations, contentInfo4.Content[0].Value);
				ASN1 asn11 = new ASN1(48);
				asn11.Add(ASN1Convert.FromOid("1.3.14.3.2.26"));
				asn11.Add(new ASN1(5));
				ASN1 asn12 = new ASN1(48);
				asn12.Add(asn11);
				asn12.Add(new ASN1(4, data));
				asn10.Add(asn12);
				asn10.Add(new ASN1(4, array));
				asn10.Add(ASN1Convert.FromInt32(this._iterations));
			}
			ASN1 asn13 = new ASN1(2, new byte[]
			{
				3
			});
			ASN1 asn14 = new ASN1(48);
			asn14.Add(asn13);
			asn14.Add(contentInfo4.ASN1);
			if (asn10.Count > 0)
			{
				asn14.Add(asn10);
			}
			return asn14.GetBytes();
		}

		private PKCS7.ContentInfo EncryptedContentInfo(ASN1 safeBags, string algorithmOid)
		{
			byte[] array = new byte[8];
			this.RNG.GetBytes(array);
			ASN1 asn = new ASN1(48);
			asn.Add(new ASN1(4, array));
			asn.Add(ASN1Convert.FromInt32(this._iterations));
			ASN1 asn2 = new ASN1(48);
			asn2.Add(ASN1Convert.FromOid(algorithmOid));
			asn2.Add(asn);
			byte[] data = this.Encrypt(algorithmOid, array, this._iterations, safeBags.GetBytes());
			ASN1 asn3 = new ASN1(128, data);
			ASN1 asn4 = new ASN1(48);
			asn4.Add(ASN1Convert.FromOid("1.2.840.113549.1.7.1"));
			asn4.Add(asn2);
			asn4.Add(asn3);
			ASN1 asn5 = new ASN1(2, new byte[1]);
			ASN1 asn6 = new ASN1(48);
			asn6.Add(asn5);
			asn6.Add(asn4);
			ASN1 asn7 = new ASN1(160);
			asn7.Add(asn6);
			return new PKCS7.ContentInfo("1.2.840.113549.1.7.6")
			{
				Content = asn7
			};
		}

		public void AddCertificate(X509Certificate cert)
		{
			this.AddCertificate(cert, null);
		}

		public void AddCertificate(X509Certificate cert, IDictionary attributes)
		{
			bool flag = false;
			int num = 0;
			while (!flag && num < this._safeBags.Count)
			{
				SafeBag safeBag = (SafeBag)this._safeBags[num];
				if (safeBag.BagOID.Equals("1.2.840.113549.1.12.10.1.3"))
				{
					ASN1 asn = safeBag.ASN1;
					ASN1 asn2 = asn[1];
					PKCS7.ContentInfo contentInfo = new PKCS7.ContentInfo(asn2.Value);
					X509Certificate x509Certificate = new X509Certificate(contentInfo.Content[0].Value);
					if (this.Compare(cert.RawData, x509Certificate.RawData))
					{
						flag = true;
					}
				}
				num++;
			}
			if (!flag)
			{
				this._safeBags.Add(new SafeBag("1.2.840.113549.1.12.10.1.3", this.CertificateSafeBag(cert, attributes)));
				this._certsChanged = true;
			}
		}

		public void RemoveCertificate(X509Certificate cert)
		{
			this.RemoveCertificate(cert, null);
		}

		public void RemoveCertificate(X509Certificate cert, IDictionary attrs)
		{
			int num = -1;
			int num2 = 0;
			while (num == -1 && num2 < this._safeBags.Count)
			{
				SafeBag safeBag = (SafeBag)this._safeBags[num2];
				if (safeBag.BagOID.Equals("1.2.840.113549.1.12.10.1.3"))
				{
					ASN1 asn = safeBag.ASN1;
					ASN1 asn2 = asn[1];
					PKCS7.ContentInfo contentInfo = new PKCS7.ContentInfo(asn2.Value);
					X509Certificate x509Certificate = new X509Certificate(contentInfo.Content[0].Value);
					if (this.Compare(cert.RawData, x509Certificate.RawData))
					{
						if (attrs != null)
						{
							if (asn.Count == 3)
							{
								ASN1 asn3 = asn[2];
								int num3 = 0;
								for (int i = 0; i < asn3.Count; i++)
								{
									ASN1 asn4 = asn3[i];
									ASN1 asn5 = asn4[0];
									string key = ASN1Convert.ToOid(asn5);
									ArrayList arrayList = (ArrayList)attrs[key];
									if (arrayList != null)
									{
										ASN1 asn6 = asn4[1];
										if (arrayList.Count == asn6.Count)
										{
											int num4 = 0;
											for (int j = 0; j < asn6.Count; j++)
											{
												ASN1 asn7 = asn6[j];
												byte[] expected = (byte[])arrayList[j];
												if (this.Compare(expected, asn7.Value))
												{
													num4++;
												}
											}
											if (num4 == asn6.Count)
											{
												num3++;
											}
										}
									}
								}
								if (num3 == asn3.Count)
								{
									num = num2;
								}
							}
						}
						else
						{
							num = num2;
						}
					}
				}
				num2++;
			}
			if (num != -1)
			{
				this._safeBags.RemoveAt(num);
				this._certsChanged = true;
			}
		}

		private bool CompareAsymmetricAlgorithm(AsymmetricAlgorithm a1, AsymmetricAlgorithm a2)
		{
			return a1.KeySize == a2.KeySize && a1.ToXmlString(false) == a2.ToXmlString(false);
		}

		public void AddPkcs8ShroudedKeyBag(AsymmetricAlgorithm aa)
		{
			this.AddPkcs8ShroudedKeyBag(aa, null);
		}

		public void AddPkcs8ShroudedKeyBag(AsymmetricAlgorithm aa, IDictionary attributes)
		{
			bool flag = false;
			int num = 0;
			while (!flag && num < this._safeBags.Count)
			{
				SafeBag safeBag = (SafeBag)this._safeBags[num];
				if (safeBag.BagOID.Equals("1.2.840.113549.1.12.10.1.2"))
				{
					ASN1 asn = safeBag.ASN1[1];
					PKCS8.EncryptedPrivateKeyInfo encryptedPrivateKeyInfo = new PKCS8.EncryptedPrivateKeyInfo(asn.Value);
					byte[] array = this.Decrypt(encryptedPrivateKeyInfo.Algorithm, encryptedPrivateKeyInfo.Salt, encryptedPrivateKeyInfo.IterationCount, encryptedPrivateKeyInfo.EncryptedData);
					PKCS8.PrivateKeyInfo privateKeyInfo = new PKCS8.PrivateKeyInfo(array);
					byte[] privateKey = privateKeyInfo.PrivateKey;
					byte b = privateKey[0];
					AsymmetricAlgorithm a;
					if (b != 2)
					{
						if (b != 48)
						{
							Array.Clear(array, 0, array.Length);
							Array.Clear(privateKey, 0, privateKey.Length);
							throw new CryptographicException("Unknown private key format");
						}
						a = PKCS8.PrivateKeyInfo.DecodeRSA(privateKey);
					}
					else
					{
						a = PKCS8.PrivateKeyInfo.DecodeDSA(privateKey, default(DSAParameters));
					}
					Array.Clear(array, 0, array.Length);
					Array.Clear(privateKey, 0, privateKey.Length);
					if (this.CompareAsymmetricAlgorithm(aa, a))
					{
						flag = true;
					}
				}
				num++;
			}
			if (!flag)
			{
				this._safeBags.Add(new SafeBag("1.2.840.113549.1.12.10.1.2", this.Pkcs8ShroudedKeyBagSafeBag(aa, attributes)));
				this._keyBagsChanged = true;
			}
		}

		public void RemovePkcs8ShroudedKeyBag(AsymmetricAlgorithm aa)
		{
			int num = -1;
			int num2 = 0;
			while (num == -1 && num2 < this._safeBags.Count)
			{
				SafeBag safeBag = (SafeBag)this._safeBags[num2];
				if (safeBag.BagOID.Equals("1.2.840.113549.1.12.10.1.2"))
				{
					ASN1 asn = safeBag.ASN1[1];
					PKCS8.EncryptedPrivateKeyInfo encryptedPrivateKeyInfo = new PKCS8.EncryptedPrivateKeyInfo(asn.Value);
					byte[] array = this.Decrypt(encryptedPrivateKeyInfo.Algorithm, encryptedPrivateKeyInfo.Salt, encryptedPrivateKeyInfo.IterationCount, encryptedPrivateKeyInfo.EncryptedData);
					PKCS8.PrivateKeyInfo privateKeyInfo = new PKCS8.PrivateKeyInfo(array);
					byte[] privateKey = privateKeyInfo.PrivateKey;
					byte b = privateKey[0];
					AsymmetricAlgorithm a;
					if (b != 2)
					{
						if (b != 48)
						{
							Array.Clear(array, 0, array.Length);
							Array.Clear(privateKey, 0, privateKey.Length);
							throw new CryptographicException("Unknown private key format");
						}
						a = PKCS8.PrivateKeyInfo.DecodeRSA(privateKey);
					}
					else
					{
						a = PKCS8.PrivateKeyInfo.DecodeDSA(privateKey, default(DSAParameters));
					}
					Array.Clear(array, 0, array.Length);
					Array.Clear(privateKey, 0, privateKey.Length);
					if (this.CompareAsymmetricAlgorithm(aa, a))
					{
						num = num2;
					}
				}
				num2++;
			}
			if (num != -1)
			{
				this._safeBags.RemoveAt(num);
				this._keyBagsChanged = true;
			}
		}

		public void AddKeyBag(AsymmetricAlgorithm aa)
		{
			this.AddKeyBag(aa, null);
		}

		public void AddKeyBag(AsymmetricAlgorithm aa, IDictionary attributes)
		{
			bool flag = false;
			int num = 0;
			while (!flag && num < this._safeBags.Count)
			{
				SafeBag safeBag = (SafeBag)this._safeBags[num];
				if (safeBag.BagOID.Equals("1.2.840.113549.1.12.10.1.1"))
				{
					ASN1 asn = safeBag.ASN1[1];
					PKCS8.PrivateKeyInfo privateKeyInfo = new PKCS8.PrivateKeyInfo(asn.Value);
					byte[] privateKey = privateKeyInfo.PrivateKey;
					byte b = privateKey[0];
					AsymmetricAlgorithm a;
					if (b != 2)
					{
						if (b != 48)
						{
							Array.Clear(privateKey, 0, privateKey.Length);
							throw new CryptographicException("Unknown private key format");
						}
						a = PKCS8.PrivateKeyInfo.DecodeRSA(privateKey);
					}
					else
					{
						a = PKCS8.PrivateKeyInfo.DecodeDSA(privateKey, default(DSAParameters));
					}
					Array.Clear(privateKey, 0, privateKey.Length);
					if (this.CompareAsymmetricAlgorithm(aa, a))
					{
						flag = true;
					}
				}
				num++;
			}
			if (!flag)
			{
				this._safeBags.Add(new SafeBag("1.2.840.113549.1.12.10.1.1", this.KeyBagSafeBag(aa, attributes)));
				this._keyBagsChanged = true;
			}
		}

		public void RemoveKeyBag(AsymmetricAlgorithm aa)
		{
			int num = -1;
			int num2 = 0;
			while (num == -1 && num2 < this._safeBags.Count)
			{
				SafeBag safeBag = (SafeBag)this._safeBags[num2];
				if (safeBag.BagOID.Equals("1.2.840.113549.1.12.10.1.1"))
				{
					ASN1 asn = safeBag.ASN1[1];
					PKCS8.PrivateKeyInfo privateKeyInfo = new PKCS8.PrivateKeyInfo(asn.Value);
					byte[] privateKey = privateKeyInfo.PrivateKey;
					byte b = privateKey[0];
					AsymmetricAlgorithm a;
					if (b != 2)
					{
						if (b != 48)
						{
							Array.Clear(privateKey, 0, privateKey.Length);
							throw new CryptographicException("Unknown private key format");
						}
						a = PKCS8.PrivateKeyInfo.DecodeRSA(privateKey);
					}
					else
					{
						a = PKCS8.PrivateKeyInfo.DecodeDSA(privateKey, default(DSAParameters));
					}
					Array.Clear(privateKey, 0, privateKey.Length);
					if (this.CompareAsymmetricAlgorithm(aa, a))
					{
						num = num2;
					}
				}
				num2++;
			}
			if (num != -1)
			{
				this._safeBags.RemoveAt(num);
				this._keyBagsChanged = true;
			}
		}

		public void AddSecretBag(byte[] secret)
		{
			this.AddSecretBag(secret, null);
		}

		public void AddSecretBag(byte[] secret, IDictionary attributes)
		{
			bool flag = false;
			int num = 0;
			while (!flag && num < this._safeBags.Count)
			{
				SafeBag safeBag = (SafeBag)this._safeBags[num];
				if (safeBag.BagOID.Equals("1.2.840.113549.1.12.10.1.5"))
				{
					ASN1 asn = safeBag.ASN1[1];
					byte[] value = asn.Value;
					if (this.Compare(secret, value))
					{
						flag = true;
					}
				}
				num++;
			}
			if (!flag)
			{
				this._safeBags.Add(new SafeBag("1.2.840.113549.1.12.10.1.5", this.SecretBagSafeBag(secret, attributes)));
				this._secretBagsChanged = true;
			}
		}

		public void RemoveSecretBag(byte[] secret)
		{
			int num = -1;
			int num2 = 0;
			while (num == -1 && num2 < this._safeBags.Count)
			{
				SafeBag safeBag = (SafeBag)this._safeBags[num2];
				if (safeBag.BagOID.Equals("1.2.840.113549.1.12.10.1.5"))
				{
					ASN1 asn = safeBag.ASN1[1];
					byte[] value = asn.Value;
					if (this.Compare(secret, value))
					{
						num = num2;
					}
				}
				num2++;
			}
			if (num != -1)
			{
				this._safeBags.RemoveAt(num);
				this._secretBagsChanged = true;
			}
		}

		public AsymmetricAlgorithm GetAsymmetricAlgorithm(IDictionary attrs)
		{
			foreach (object obj in this._safeBags)
			{
				SafeBag safeBag = (SafeBag)obj;
				if (safeBag.BagOID.Equals("1.2.840.113549.1.12.10.1.1") || safeBag.BagOID.Equals("1.2.840.113549.1.12.10.1.2"))
				{
					ASN1 asn = safeBag.ASN1;
					if (asn.Count == 3)
					{
						ASN1 asn2 = asn[2];
						int num = 0;
						for (int i = 0; i < asn2.Count; i++)
						{
							ASN1 asn3 = asn2[i];
							ASN1 asn4 = asn3[0];
							string key = ASN1Convert.ToOid(asn4);
							ArrayList arrayList = (ArrayList)attrs[key];
							if (arrayList != null)
							{
								ASN1 asn5 = asn3[1];
								if (arrayList.Count == asn5.Count)
								{
									int num2 = 0;
									for (int j = 0; j < asn5.Count; j++)
									{
										ASN1 asn6 = asn5[j];
										byte[] expected = (byte[])arrayList[j];
										if (this.Compare(expected, asn6.Value))
										{
											num2++;
										}
									}
									if (num2 == asn5.Count)
									{
										num++;
									}
								}
							}
						}
						if (num == asn2.Count)
						{
							ASN1 asn7 = asn[1];
							AsymmetricAlgorithm result = null;
							if (safeBag.BagOID.Equals("1.2.840.113549.1.12.10.1.1"))
							{
								PKCS8.PrivateKeyInfo privateKeyInfo = new PKCS8.PrivateKeyInfo(asn7.Value);
								byte[] privateKey = privateKeyInfo.PrivateKey;
								byte b = privateKey[0];
								if (b != 2)
								{
									if (b == 48)
									{
										result = PKCS8.PrivateKeyInfo.DecodeRSA(privateKey);
									}
								}
								else
								{
									result = PKCS8.PrivateKeyInfo.DecodeDSA(privateKey, default(DSAParameters));
								}
								Array.Clear(privateKey, 0, privateKey.Length);
							}
							else if (safeBag.BagOID.Equals("1.2.840.113549.1.12.10.1.2"))
							{
								PKCS8.EncryptedPrivateKeyInfo encryptedPrivateKeyInfo = new PKCS8.EncryptedPrivateKeyInfo(asn7.Value);
								byte[] array = this.Decrypt(encryptedPrivateKeyInfo.Algorithm, encryptedPrivateKeyInfo.Salt, encryptedPrivateKeyInfo.IterationCount, encryptedPrivateKeyInfo.EncryptedData);
								PKCS8.PrivateKeyInfo privateKeyInfo2 = new PKCS8.PrivateKeyInfo(array);
								byte[] privateKey2 = privateKeyInfo2.PrivateKey;
								byte b = privateKey2[0];
								if (b != 2)
								{
									if (b == 48)
									{
										result = PKCS8.PrivateKeyInfo.DecodeRSA(privateKey2);
									}
								}
								else
								{
									result = PKCS8.PrivateKeyInfo.DecodeDSA(privateKey2, default(DSAParameters));
								}
								Array.Clear(privateKey2, 0, privateKey2.Length);
								Array.Clear(array, 0, array.Length);
							}
							return result;
						}
					}
				}
			}
			return null;
		}

		public byte[] GetSecret(IDictionary attrs)
		{
			foreach (object obj in this._safeBags)
			{
				SafeBag safeBag = (SafeBag)obj;
				if (safeBag.BagOID.Equals("1.2.840.113549.1.12.10.1.5"))
				{
					ASN1 asn = safeBag.ASN1;
					if (asn.Count == 3)
					{
						ASN1 asn2 = asn[2];
						int num = 0;
						for (int i = 0; i < asn2.Count; i++)
						{
							ASN1 asn3 = asn2[i];
							ASN1 asn4 = asn3[0];
							string key = ASN1Convert.ToOid(asn4);
							ArrayList arrayList = (ArrayList)attrs[key];
							if (arrayList != null)
							{
								ASN1 asn5 = asn3[1];
								if (arrayList.Count == asn5.Count)
								{
									int num2 = 0;
									for (int j = 0; j < asn5.Count; j++)
									{
										ASN1 asn6 = asn5[j];
										byte[] expected = (byte[])arrayList[j];
										if (this.Compare(expected, asn6.Value))
										{
											num2++;
										}
									}
									if (num2 == asn5.Count)
									{
										num++;
									}
								}
							}
						}
						if (num == asn2.Count)
						{
							ASN1 asn7 = asn[1];
							return asn7.Value;
						}
					}
				}
			}
			return null;
		}

		public X509Certificate GetCertificate(IDictionary attrs)
		{
			foreach (object obj in this._safeBags)
			{
				SafeBag safeBag = (SafeBag)obj;
				if (safeBag.BagOID.Equals("1.2.840.113549.1.12.10.1.3"))
				{
					ASN1 asn = safeBag.ASN1;
					if (asn.Count == 3)
					{
						ASN1 asn2 = asn[2];
						int num = 0;
						for (int i = 0; i < asn2.Count; i++)
						{
							ASN1 asn3 = asn2[i];
							ASN1 asn4 = asn3[0];
							string key = ASN1Convert.ToOid(asn4);
							ArrayList arrayList = (ArrayList)attrs[key];
							if (arrayList != null)
							{
								ASN1 asn5 = asn3[1];
								if (arrayList.Count == asn5.Count)
								{
									int num2 = 0;
									for (int j = 0; j < asn5.Count; j++)
									{
										ASN1 asn6 = asn5[j];
										byte[] expected = (byte[])arrayList[j];
										if (this.Compare(expected, asn6.Value))
										{
											num2++;
										}
									}
									if (num2 == asn5.Count)
									{
										num++;
									}
								}
							}
						}
						if (num == asn2.Count)
						{
							ASN1 asn7 = asn[1];
							PKCS7.ContentInfo contentInfo = new PKCS7.ContentInfo(asn7.Value);
							return new X509Certificate(contentInfo.Content[0].Value);
						}
					}
				}
			}
			return null;
		}

		public IDictionary GetAttributes(AsymmetricAlgorithm aa)
		{
			IDictionary dictionary = new Hashtable();
			foreach (object obj in this._safeBags)
			{
				SafeBag safeBag = (SafeBag)obj;
				if (safeBag.BagOID.Equals("1.2.840.113549.1.12.10.1.1") || safeBag.BagOID.Equals("1.2.840.113549.1.12.10.1.2"))
				{
					ASN1 asn = safeBag.ASN1;
					ASN1 asn2 = asn[1];
					AsymmetricAlgorithm asymmetricAlgorithm = null;
					if (safeBag.BagOID.Equals("1.2.840.113549.1.12.10.1.1"))
					{
						PKCS8.PrivateKeyInfo privateKeyInfo = new PKCS8.PrivateKeyInfo(asn2.Value);
						byte[] privateKey = privateKeyInfo.PrivateKey;
						byte b = privateKey[0];
						if (b != 2)
						{
							if (b == 48)
							{
								asymmetricAlgorithm = PKCS8.PrivateKeyInfo.DecodeRSA(privateKey);
							}
						}
						else
						{
							asymmetricAlgorithm = PKCS8.PrivateKeyInfo.DecodeDSA(privateKey, default(DSAParameters));
						}
						Array.Clear(privateKey, 0, privateKey.Length);
					}
					else if (safeBag.BagOID.Equals("1.2.840.113549.1.12.10.1.2"))
					{
						PKCS8.EncryptedPrivateKeyInfo encryptedPrivateKeyInfo = new PKCS8.EncryptedPrivateKeyInfo(asn2.Value);
						byte[] array = this.Decrypt(encryptedPrivateKeyInfo.Algorithm, encryptedPrivateKeyInfo.Salt, encryptedPrivateKeyInfo.IterationCount, encryptedPrivateKeyInfo.EncryptedData);
						PKCS8.PrivateKeyInfo privateKeyInfo2 = new PKCS8.PrivateKeyInfo(array);
						byte[] privateKey2 = privateKeyInfo2.PrivateKey;
						byte b = privateKey2[0];
						if (b != 2)
						{
							if (b == 48)
							{
								asymmetricAlgorithm = PKCS8.PrivateKeyInfo.DecodeRSA(privateKey2);
							}
						}
						else
						{
							asymmetricAlgorithm = PKCS8.PrivateKeyInfo.DecodeDSA(privateKey2, default(DSAParameters));
						}
						Array.Clear(privateKey2, 0, privateKey2.Length);
						Array.Clear(array, 0, array.Length);
					}
					if (asymmetricAlgorithm != null && this.CompareAsymmetricAlgorithm(asymmetricAlgorithm, aa) && asn.Count == 3)
					{
						ASN1 asn3 = asn[2];
						for (int i = 0; i < asn3.Count; i++)
						{
							ASN1 asn4 = asn3[i];
							ASN1 asn5 = asn4[0];
							string key = ASN1Convert.ToOid(asn5);
							ArrayList arrayList = new ArrayList();
							ASN1 asn6 = asn4[1];
							for (int j = 0; j < asn6.Count; j++)
							{
								ASN1 asn7 = asn6[j];
								arrayList.Add(asn7.Value);
							}
							dictionary.Add(key, arrayList);
						}
					}
				}
			}
			return dictionary;
		}

		public IDictionary GetAttributes(X509Certificate cert)
		{
			IDictionary dictionary = new Hashtable();
			foreach (object obj in this._safeBags)
			{
				SafeBag safeBag = (SafeBag)obj;
				if (safeBag.BagOID.Equals("1.2.840.113549.1.12.10.1.3"))
				{
					ASN1 asn = safeBag.ASN1;
					ASN1 asn2 = asn[1];
					PKCS7.ContentInfo contentInfo = new PKCS7.ContentInfo(asn2.Value);
					X509Certificate x509Certificate = new X509Certificate(contentInfo.Content[0].Value);
					if (this.Compare(cert.RawData, x509Certificate.RawData) && asn.Count == 3)
					{
						ASN1 asn3 = asn[2];
						for (int i = 0; i < asn3.Count; i++)
						{
							ASN1 asn4 = asn3[i];
							ASN1 asn5 = asn4[0];
							string key = ASN1Convert.ToOid(asn5);
							ArrayList arrayList = new ArrayList();
							ASN1 asn6 = asn4[1];
							for (int j = 0; j < asn6.Count; j++)
							{
								ASN1 asn7 = asn6[j];
								arrayList.Add(asn7.Value);
							}
							dictionary.Add(key, arrayList);
						}
					}
				}
			}
			return dictionary;
		}

		public void SaveToFile(string filename)
		{
			if (filename == null)
			{
				throw new ArgumentNullException("filename");
			}
			using (FileStream fileStream = File.Create(filename))
			{
				byte[] bytes = this.GetBytes();
				fileStream.Write(bytes, 0, bytes.Length);
			}
		}

		public object Clone()
		{
			PKCS12 pkcs;
			if (this._password != null)
			{
				pkcs = new PKCS12(this.GetBytes(), Encoding.BigEndianUnicode.GetString(this._password));
			}
			else
			{
				pkcs = new PKCS12(this.GetBytes());
			}
			pkcs.IterationCount = this.IterationCount;
			return pkcs;
		}

		public static int MaximumPasswordLength
		{
			get
			{
				return PKCS12.password_max_length;
			}
			set
			{
				if (value < 32)
				{
					string text = Locale.GetText("Maximum password length cannot be less than {0}.", new object[]
					{
						32
					});
					throw new ArgumentOutOfRangeException(text);
				}
				PKCS12.password_max_length = value;
			}
		}

		private static byte[] LoadFile(string filename)
		{
			byte[] array = null;
			using (FileStream fileStream = File.OpenRead(filename))
			{
				array = new byte[fileStream.Length];
				fileStream.Read(array, 0, array.Length);
				fileStream.Close();
			}
			return array;
		}

		public static PKCS12 LoadFromFile(string filename)
		{
			if (filename == null)
			{
				throw new ArgumentNullException("filename");
			}
			return new PKCS12(PKCS12.LoadFile(filename));
		}

		public static PKCS12 LoadFromFile(string filename, string password)
		{
			if (filename == null)
			{
				throw new ArgumentNullException("filename");
			}
			return new PKCS12(PKCS12.LoadFile(filename), password);
		}

		public class DeriveBytes
		{
			private static byte[] keyDiversifier = new byte[]
			{
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1
			};

			private static byte[] ivDiversifier = new byte[]
			{
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2
			};

			private static byte[] macDiversifier = new byte[]
			{
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3
			};

			private string _hashName;

			private int _iterations;

			private byte[] _password;

			private byte[] _salt;

			public string HashName
			{
				get
				{
					return this._hashName;
				}
				set
				{
					this._hashName = value;
				}
			}

			public int IterationCount
			{
				get
				{
					return this._iterations;
				}
				set
				{
					this._iterations = value;
				}
			}

			public byte[] Password
			{
				get
				{
					return (byte[])this._password.Clone();
				}
				set
				{
					if (value == null)
					{
						this._password = new byte[0];
					}
					else
					{
						this._password = (byte[])value.Clone();
					}
				}
			}

			public byte[] Salt
			{
				get
				{
					return (byte[])this._salt.Clone();
				}
				set
				{
					if (value != null)
					{
						this._salt = (byte[])value.Clone();
					}
					else
					{
						this._salt = null;
					}
				}
			}

			private void Adjust(byte[] a, int aOff, byte[] b)
			{
				int num = (int)((b[b.Length - 1] & byte.MaxValue) + (a[aOff + b.Length - 1] & byte.MaxValue) + 1);
				a[aOff + b.Length - 1] = (byte)num;
				num >>= 8;
				for (int i = b.Length - 2; i >= 0; i--)
				{
					num += (int)((b[i] & byte.MaxValue) + (a[aOff + i] & byte.MaxValue));
					a[aOff + i] = (byte)num;
					num >>= 8;
				}
			}

			private byte[] Derive(byte[] diversifier, int n)
			{
				HashAlgorithm hashAlgorithm = HashAlgorithm.Create(this._hashName);
				int num = hashAlgorithm.HashSize >> 3;
				int num2 = 64;
				byte[] array = new byte[n];
				byte[] array2;
				if (this._salt != null && this._salt.Length != 0)
				{
					array2 = new byte[num2 * ((this._salt.Length + num2 - 1) / num2)];
					for (int num3 = 0; num3 != array2.Length; num3++)
					{
						array2[num3] = this._salt[num3 % this._salt.Length];
					}
				}
				else
				{
					array2 = new byte[0];
				}
				byte[] array3;
				if (this._password != null && this._password.Length != 0)
				{
					array3 = new byte[num2 * ((this._password.Length + num2 - 1) / num2)];
					for (int num4 = 0; num4 != array3.Length; num4++)
					{
						array3[num4] = this._password[num4 % this._password.Length];
					}
				}
				else
				{
					array3 = new byte[0];
				}
				byte[] array4 = new byte[array2.Length + array3.Length];
				Buffer.BlockCopy(array2, 0, array4, 0, array2.Length);
				Buffer.BlockCopy(array3, 0, array4, array2.Length, array3.Length);
				byte[] array5 = new byte[num2];
				int num5 = (n + num - 1) / num;
				for (int i = 1; i <= num5; i++)
				{
					hashAlgorithm.TransformBlock(diversifier, 0, diversifier.Length, diversifier, 0);
					hashAlgorithm.TransformFinalBlock(array4, 0, array4.Length);
					byte[] array6 = hashAlgorithm.Hash;
					hashAlgorithm.Initialize();
					for (int num6 = 1; num6 != this._iterations; num6++)
					{
						array6 = hashAlgorithm.ComputeHash(array6, 0, array6.Length);
					}
					for (int num7 = 0; num7 != array5.Length; num7++)
					{
						array5[num7] = array6[num7 % array6.Length];
					}
					for (int num8 = 0; num8 != array4.Length / num2; num8++)
					{
						this.Adjust(array4, num8 * num2, array5);
					}
					if (i == num5)
					{
						Buffer.BlockCopy(array6, 0, array, (i - 1) * num, array.Length - (i - 1) * num);
					}
					else
					{
						Buffer.BlockCopy(array6, 0, array, (i - 1) * num, array6.Length);
					}
				}
				return array;
			}

			public byte[] DeriveKey(int size)
			{
				return this.Derive(PKCS12.DeriveBytes.keyDiversifier, size);
			}

			public byte[] DeriveIV(int size)
			{
				return this.Derive(PKCS12.DeriveBytes.ivDiversifier, size);
			}

			public byte[] DeriveMAC(int size)
			{
				return this.Derive(PKCS12.DeriveBytes.macDiversifier, size);
			}

			public enum Purpose
			{
				Key,
				IV,
				MAC
			}
		}
	}
}
