using System;
using System.Collections;
using System.Security.Cryptography;

namespace Mono.Security.Cryptography
{
	internal sealed class PKCS8
	{
		private PKCS8()
		{
		}

		public static PKCS8.KeyInfo GetType(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			PKCS8.KeyInfo result = PKCS8.KeyInfo.Unknown;
			try
			{
				ASN1 asn = new ASN1(data);
				if (asn.Tag == 48 && asn.Count > 0)
				{
					ASN1 asn2 = asn[0];
					byte tag = asn2.Tag;
					if (tag != 2)
					{
						if (tag == 48)
						{
							result = PKCS8.KeyInfo.EncryptedPrivateKey;
						}
					}
					else
					{
						result = PKCS8.KeyInfo.PrivateKey;
					}
				}
			}
			catch
			{
				throw new CryptographicException("invalid ASN.1 data");
			}
			return result;
		}

		public enum KeyInfo
		{
			PrivateKey,
			EncryptedPrivateKey,
			Unknown
		}

		public class PrivateKeyInfo
		{
			private int _version;

			private string _algorithm;

			private byte[] _key;

			private ArrayList _list;

			public PrivateKeyInfo()
			{
				this._version = 0;
				this._list = new ArrayList();
			}

			public PrivateKeyInfo(byte[] data) : this()
			{
				this.Decode(data);
			}

			public string Algorithm
			{
				get
				{
					return this._algorithm;
				}
				set
				{
					this._algorithm = value;
				}
			}

			public ArrayList Attributes
			{
				get
				{
					return this._list;
				}
			}

			public byte[] PrivateKey
			{
				get
				{
					if (this._key == null)
					{
						return null;
					}
					return (byte[])this._key.Clone();
				}
				set
				{
					if (value == null)
					{
						throw new ArgumentNullException("PrivateKey");
					}
					this._key = (byte[])value.Clone();
				}
			}

			public int Version
			{
				get
				{
					return this._version;
				}
				set
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("negative version");
					}
					this._version = value;
				}
			}

			private void Decode(byte[] data)
			{
				ASN1 asn = new ASN1(data);
				if (asn.Tag != 48)
				{
					throw new CryptographicException("invalid PrivateKeyInfo");
				}
				ASN1 asn2 = asn[0];
				if (asn2.Tag != 2)
				{
					throw new CryptographicException("invalid version");
				}
				this._version = (int)asn2.Value[0];
				ASN1 asn3 = asn[1];
				if (asn3.Tag != 48)
				{
					throw new CryptographicException("invalid algorithm");
				}
				ASN1 asn4 = asn3[0];
				if (asn4.Tag != 6)
				{
					throw new CryptographicException("missing algorithm OID");
				}
				this._algorithm = ASN1Convert.ToOid(asn4);
				ASN1 asn5 = asn[2];
				this._key = asn5.Value;
				if (asn.Count > 3)
				{
					ASN1 asn6 = asn[3];
					for (int i = 0; i < asn6.Count; i++)
					{
						this._list.Add(asn6[i]);
					}
				}
			}

			public byte[] GetBytes()
			{
				ASN1 asn = new ASN1(48);
				asn.Add(ASN1Convert.FromOid(this._algorithm));
				asn.Add(new ASN1(5));
				ASN1 asn2 = new ASN1(48);
				asn2.Add(new ASN1(2, new byte[]
				{
					(byte)this._version
				}));
				asn2.Add(asn);
				asn2.Add(new ASN1(4, this._key));
				if (this._list.Count > 0)
				{
					ASN1 asn3 = new ASN1(160);
					foreach (object obj in this._list)
					{
						ASN1 asn4 = (ASN1)obj;
						asn3.Add(asn4);
					}
					asn2.Add(asn3);
				}
				return asn2.GetBytes();
			}

			private static byte[] RemoveLeadingZero(byte[] bigInt)
			{
				int srcOffset = 0;
				int num = bigInt.Length;
				if (bigInt[0] == 0)
				{
					srcOffset = 1;
					num--;
				}
				byte[] array = new byte[num];
				Buffer.BlockCopy(bigInt, srcOffset, array, 0, num);
				return array;
			}

			private static byte[] Normalize(byte[] bigInt, int length)
			{
				if (bigInt.Length == length)
				{
					return bigInt;
				}
				if (bigInt.Length > length)
				{
					return PKCS8.PrivateKeyInfo.RemoveLeadingZero(bigInt);
				}
				byte[] array = new byte[length];
				Buffer.BlockCopy(bigInt, 0, array, length - bigInt.Length, bigInt.Length);
				return array;
			}

			public static RSA DecodeRSA(byte[] keypair)
			{
				ASN1 asn = new ASN1(keypair);
				if (asn.Tag != 48)
				{
					throw new CryptographicException("invalid private key format");
				}
				ASN1 asn2 = asn[0];
				if (asn2.Tag != 2)
				{
					throw new CryptographicException("missing version");
				}
				if (asn.Count < 9)
				{
					throw new CryptographicException("not enough key parameters");
				}
				RSAParameters parameters = default(RSAParameters);
				parameters.Modulus = PKCS8.PrivateKeyInfo.RemoveLeadingZero(asn[1].Value);
				int num = parameters.Modulus.Length;
				int length = num >> 1;
				parameters.D = PKCS8.PrivateKeyInfo.Normalize(asn[3].Value, num);
				parameters.DP = PKCS8.PrivateKeyInfo.Normalize(asn[6].Value, length);
				parameters.DQ = PKCS8.PrivateKeyInfo.Normalize(asn[7].Value, length);
				parameters.Exponent = PKCS8.PrivateKeyInfo.RemoveLeadingZero(asn[2].Value);
				parameters.InverseQ = PKCS8.PrivateKeyInfo.Normalize(asn[8].Value, length);
				parameters.P = PKCS8.PrivateKeyInfo.Normalize(asn[4].Value, length);
				parameters.Q = PKCS8.PrivateKeyInfo.Normalize(asn[5].Value, length);
				RSA rsa = null;
				try
				{
					rsa = RSA.Create();
					rsa.ImportParameters(parameters);
				}
				catch (CryptographicException)
				{
					rsa = new RSACryptoServiceProvider(new CspParameters
					{
						Flags = CspProviderFlags.UseMachineKeyStore
					});
					rsa.ImportParameters(parameters);
				}
				return rsa;
			}

			public static byte[] Encode(RSA rsa)
			{
				RSAParameters rsaparameters = rsa.ExportParameters(true);
				ASN1 asn = new ASN1(48);
				asn.Add(new ASN1(2, new byte[1]));
				asn.Add(ASN1Convert.FromUnsignedBigInteger(rsaparameters.Modulus));
				asn.Add(ASN1Convert.FromUnsignedBigInteger(rsaparameters.Exponent));
				asn.Add(ASN1Convert.FromUnsignedBigInteger(rsaparameters.D));
				asn.Add(ASN1Convert.FromUnsignedBigInteger(rsaparameters.P));
				asn.Add(ASN1Convert.FromUnsignedBigInteger(rsaparameters.Q));
				asn.Add(ASN1Convert.FromUnsignedBigInteger(rsaparameters.DP));
				asn.Add(ASN1Convert.FromUnsignedBigInteger(rsaparameters.DQ));
				asn.Add(ASN1Convert.FromUnsignedBigInteger(rsaparameters.InverseQ));
				return asn.GetBytes();
			}

			public static DSA DecodeDSA(byte[] privateKey, DSAParameters dsaParameters)
			{
				ASN1 asn = new ASN1(privateKey);
				if (asn.Tag != 2)
				{
					throw new CryptographicException("invalid private key format");
				}
				dsaParameters.X = PKCS8.PrivateKeyInfo.Normalize(asn.Value, 20);
				DSA dsa = DSA.Create();
				dsa.ImportParameters(dsaParameters);
				return dsa;
			}

			public static byte[] Encode(DSA dsa)
			{
				return ASN1Convert.FromUnsignedBigInteger(dsa.ExportParameters(true).X).GetBytes();
			}

			public static byte[] Encode(AsymmetricAlgorithm aa)
			{
				if (aa is RSA)
				{
					return PKCS8.PrivateKeyInfo.Encode((RSA)aa);
				}
				if (aa is DSA)
				{
					return PKCS8.PrivateKeyInfo.Encode((DSA)aa);
				}
				throw new CryptographicException("Unknown asymmetric algorithm {0}", aa.ToString());
			}
		}

		public class EncryptedPrivateKeyInfo
		{
			private string _algorithm;

			private byte[] _salt;

			private int _iterations;

			private byte[] _data;

			public EncryptedPrivateKeyInfo()
			{
			}

			public EncryptedPrivateKeyInfo(byte[] data) : this()
			{
				this.Decode(data);
			}

			public string Algorithm
			{
				get
				{
					return this._algorithm;
				}
				set
				{
					this._algorithm = value;
				}
			}

			public byte[] EncryptedData
			{
				get
				{
					return (this._data != null) ? ((byte[])this._data.Clone()) : null;
				}
				set
				{
					this._data = ((value != null) ? ((byte[])value.Clone()) : null);
				}
			}

			public byte[] Salt
			{
				get
				{
					if (this._salt == null)
					{
						RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
						this._salt = new byte[8];
						randomNumberGenerator.GetBytes(this._salt);
					}
					return (byte[])this._salt.Clone();
				}
				set
				{
					this._salt = (byte[])value.Clone();
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
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("IterationCount", "Negative");
					}
					this._iterations = value;
				}
			}

			private void Decode(byte[] data)
			{
				ASN1 asn = new ASN1(data);
				if (asn.Tag != 48)
				{
					throw new CryptographicException("invalid EncryptedPrivateKeyInfo");
				}
				ASN1 asn2 = asn[0];
				if (asn2.Tag != 48)
				{
					throw new CryptographicException("invalid encryptionAlgorithm");
				}
				ASN1 asn3 = asn2[0];
				if (asn3.Tag != 6)
				{
					throw new CryptographicException("invalid algorithm");
				}
				this._algorithm = ASN1Convert.ToOid(asn3);
				if (asn2.Count > 1)
				{
					ASN1 asn4 = asn2[1];
					if (asn4.Tag != 48)
					{
						throw new CryptographicException("invalid parameters");
					}
					ASN1 asn5 = asn4[0];
					if (asn5.Tag != 4)
					{
						throw new CryptographicException("invalid salt");
					}
					this._salt = asn5.Value;
					ASN1 asn6 = asn4[1];
					if (asn6.Tag != 2)
					{
						throw new CryptographicException("invalid iterationCount");
					}
					this._iterations = ASN1Convert.ToInt32(asn6);
				}
				ASN1 asn7 = asn[1];
				if (asn7.Tag != 4)
				{
					throw new CryptographicException("invalid EncryptedData");
				}
				this._data = asn7.Value;
			}

			public byte[] GetBytes()
			{
				if (this._algorithm == null)
				{
					throw new CryptographicException("No algorithm OID specified");
				}
				ASN1 asn = new ASN1(48);
				asn.Add(ASN1Convert.FromOid(this._algorithm));
				if (this._iterations > 0 || this._salt != null)
				{
					ASN1 asn2 = new ASN1(4, this._salt);
					ASN1 asn3 = ASN1Convert.FromInt32(this._iterations);
					ASN1 asn4 = new ASN1(48);
					asn4.Add(asn2);
					asn4.Add(asn3);
					asn.Add(asn4);
				}
				ASN1 asn5 = new ASN1(4, this._data);
				ASN1 asn6 = new ASN1(48);
				asn6.Add(asn);
				asn6.Add(asn5);
				return asn6.GetBytes();
			}
		}
	}
}
