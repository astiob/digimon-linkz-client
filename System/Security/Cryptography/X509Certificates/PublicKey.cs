using Mono.Security;
using Mono.Security.Cryptography;
using Mono.Security.X509;
using System;
using System.Collections.Generic;

namespace System.Security.Cryptography.X509Certificates
{
	/// <summary>Represents a certificate's public key information. This class cannot be inherited.</summary>
	public sealed class PublicKey
	{
		private const string rsaOid = "1.2.840.113549.1.1.1";

		private const string dsaOid = "1.2.840.10040.4.1";

		private AsymmetricAlgorithm _key;

		private AsnEncodedData _keyValue;

		private AsnEncodedData _params;

		private Oid _oid;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.PublicKey" /> class using an object identifier (OID) object of the public key, an ASN.1-encoded representation of the public key parameters, and an ASN.1-encoded representation of the public key value. </summary>
		/// <param name="oid">An object identifier (OID) object that represents the public key.</param>
		/// <param name="parameters">An ASN.1-encoded representation of the public key parameters.</param>
		/// <param name="keyValue">An ASN.1-encoded representation of the public key value.</param>
		public PublicKey(Oid oid, AsnEncodedData parameters, AsnEncodedData keyValue)
		{
			if (oid == null)
			{
				throw new ArgumentNullException("oid");
			}
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}
			if (keyValue == null)
			{
				throw new ArgumentNullException("keyValue");
			}
			this._oid = new Oid(oid);
			this._params = new AsnEncodedData(parameters);
			this._keyValue = new AsnEncodedData(keyValue);
		}

		internal PublicKey(X509Certificate certificate)
		{
			bool flag = true;
			if (certificate.KeyAlgorithm == "1.2.840.113549.1.1.1")
			{
				RSACryptoServiceProvider rsacryptoServiceProvider = certificate.RSA as RSACryptoServiceProvider;
				if (rsacryptoServiceProvider != null && rsacryptoServiceProvider.PublicOnly)
				{
					this._key = certificate.RSA;
					flag = false;
				}
				else
				{
					RSAManaged rsamanaged = certificate.RSA as RSAManaged;
					if (rsamanaged != null && rsamanaged.PublicOnly)
					{
						this._key = certificate.RSA;
						flag = false;
					}
				}
				if (flag)
				{
					RSAParameters parameters = certificate.RSA.ExportParameters(false);
					this._key = RSA.Create();
					(this._key as RSA).ImportParameters(parameters);
				}
			}
			else
			{
				DSACryptoServiceProvider dsacryptoServiceProvider = certificate.DSA as DSACryptoServiceProvider;
				if (dsacryptoServiceProvider != null && dsacryptoServiceProvider.PublicOnly)
				{
					this._key = certificate.DSA;
					flag = false;
				}
				if (flag)
				{
					DSAParameters parameters2 = certificate.DSA.ExportParameters(false);
					this._key = DSA.Create();
					(this._key as DSA).ImportParameters(parameters2);
				}
			}
			this._oid = new Oid(certificate.KeyAlgorithm);
			this._keyValue = new AsnEncodedData(this._oid, certificate.PublicKey);
			this._params = new AsnEncodedData(this._oid, certificate.KeyAlgorithmParameters);
		}

		/// <summary>Gets the ASN.1-encoded representation of the public key value.</summary>
		/// <returns>The ASN.1-encoded representation of the public key value.</returns>
		public AsnEncodedData EncodedKeyValue
		{
			get
			{
				return this._keyValue;
			}
		}

		/// <summary>Gets the ASN.1-encoded representation of the public key parameters.</summary>
		/// <returns>The ASN.1-encoded representation of the public key parameters.</returns>
		public AsnEncodedData EncodedParameters
		{
			get
			{
				return this._params;
			}
		}

		/// <summary>Gets an <see cref="T:System.Security.Cryptography.RSACryptoServiceProvider" /> or <see cref="T:System.Security.Cryptography.DSACryptoServiceProvider" /> object representing the public key.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.AsymmetricAlgorithm" /> object representing the public key.</returns>
		/// <exception cref="T:System.NotSupportedException">The key algorithm is not supported.</exception>
		public AsymmetricAlgorithm Key
		{
			get
			{
				if (this._key == null)
				{
					string value = this._oid.Value;
					if (value != null)
					{
						if (PublicKey.<>f__switch$map9 == null)
						{
							PublicKey.<>f__switch$map9 = new Dictionary<string, int>(2)
							{
								{
									"1.2.840.113549.1.1.1",
									0
								},
								{
									"1.2.840.10040.4.1",
									1
								}
							};
						}
						int num;
						if (PublicKey.<>f__switch$map9.TryGetValue(value, out num))
						{
							if (num == 0)
							{
								this._key = PublicKey.DecodeRSA(this._keyValue.RawData);
								goto IL_D7;
							}
							if (num == 1)
							{
								this._key = PublicKey.DecodeDSA(this._keyValue.RawData, this._params.RawData);
								goto IL_D7;
							}
						}
					}
					string text = Locale.GetText("Cannot decode public key from unknown OID '{0}'.", new object[]
					{
						this._oid.Value
					});
					throw new NotSupportedException(text);
				}
				IL_D7:
				return this._key;
			}
		}

		/// <summary>Gets an object identifier (OID) object of the public key.</summary>
		/// <returns>An object identifier (OID) object of the public key.</returns>
		public Oid Oid
		{
			get
			{
				return this._oid;
			}
		}

		private static byte[] GetUnsignedBigInteger(byte[] integer)
		{
			if (integer[0] != 0)
			{
				return integer;
			}
			int num = integer.Length - 1;
			byte[] array = new byte[num];
			Buffer.BlockCopy(integer, 1, array, 0, num);
			return array;
		}

		internal static DSA DecodeDSA(byte[] rawPublicKey, byte[] rawParameters)
		{
			DSAParameters parameters = default(DSAParameters);
			try
			{
				ASN1 asn = new ASN1(rawPublicKey);
				if (asn.Tag != 2)
				{
					throw new CryptographicException(Locale.GetText("Missing DSA Y integer."));
				}
				parameters.Y = PublicKey.GetUnsignedBigInteger(asn.Value);
				ASN1 asn2 = new ASN1(rawParameters);
				if (asn2 == null || asn2.Tag != 48 || asn2.Count < 3)
				{
					throw new CryptographicException(Locale.GetText("Missing DSA parameters."));
				}
				if (asn2[0].Tag != 2 || asn2[1].Tag != 2 || asn2[2].Tag != 2)
				{
					throw new CryptographicException(Locale.GetText("Invalid DSA parameters."));
				}
				parameters.P = PublicKey.GetUnsignedBigInteger(asn2[0].Value);
				parameters.Q = PublicKey.GetUnsignedBigInteger(asn2[1].Value);
				parameters.G = PublicKey.GetUnsignedBigInteger(asn2[2].Value);
			}
			catch (Exception inner)
			{
				string text = Locale.GetText("Error decoding the ASN.1 structure.");
				throw new CryptographicException(text, inner);
			}
			DSA dsa = new DSACryptoServiceProvider(parameters.Y.Length << 3);
			dsa.ImportParameters(parameters);
			return dsa;
		}

		internal static RSA DecodeRSA(byte[] rawPublicKey)
		{
			RSAParameters parameters = default(RSAParameters);
			try
			{
				ASN1 asn = new ASN1(rawPublicKey);
				if (asn.Count == 0)
				{
					throw new CryptographicException(Locale.GetText("Missing RSA modulus and exponent."));
				}
				ASN1 asn2 = asn[0];
				if (asn2 == null || asn2.Tag != 2)
				{
					throw new CryptographicException(Locale.GetText("Missing RSA modulus."));
				}
				ASN1 asn3 = asn[1];
				if (asn3.Tag != 2)
				{
					throw new CryptographicException(Locale.GetText("Missing RSA public exponent."));
				}
				parameters.Modulus = PublicKey.GetUnsignedBigInteger(asn2.Value);
				parameters.Exponent = asn3.Value;
			}
			catch (Exception inner)
			{
				string text = Locale.GetText("Error decoding the ASN.1 structure.");
				throw new CryptographicException(text, inner);
			}
			int dwKeySize = parameters.Modulus.Length << 3;
			RSA rsa = new RSACryptoServiceProvider(dwKeySize);
			rsa.ImportParameters(parameters);
			return rsa;
		}
	}
}
