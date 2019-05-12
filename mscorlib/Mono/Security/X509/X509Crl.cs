using Mono.Security.X509.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Mono.Security.X509
{
	internal class X509Crl
	{
		private string issuer;

		private byte version;

		private DateTime thisUpdate;

		private DateTime nextUpdate;

		private ArrayList entries;

		private string signatureOID;

		private byte[] signature;

		private X509ExtensionCollection extensions;

		private byte[] encoded;

		private byte[] hash_value;

		public X509Crl(byte[] crl)
		{
			if (crl == null)
			{
				throw new ArgumentNullException("crl");
			}
			this.encoded = (byte[])crl.Clone();
			this.Parse(this.encoded);
		}

		private void Parse(byte[] crl)
		{
			string text = "Input data cannot be coded as a valid CRL.";
			try
			{
				ASN1 asn = new ASN1(this.encoded);
				if (asn.Tag != 48 || asn.Count != 3)
				{
					throw new CryptographicException(text);
				}
				ASN1 asn2 = asn[0];
				if (asn2.Tag != 48 || asn2.Count < 3)
				{
					throw new CryptographicException(text);
				}
				int num = 0;
				if (asn2[num].Tag == 2)
				{
					this.version = asn2[num++].Value[0] + 1;
				}
				else
				{
					this.version = 1;
				}
				this.signatureOID = ASN1Convert.ToOid(asn2[num++][0]);
				this.issuer = X501.ToString(asn2[num++]);
				this.thisUpdate = ASN1Convert.ToDateTime(asn2[num++]);
				ASN1 asn3 = asn2[num++];
				if (asn3.Tag == 23 || asn3.Tag == 24)
				{
					this.nextUpdate = ASN1Convert.ToDateTime(asn3);
					asn3 = asn2[num++];
				}
				this.entries = new ArrayList();
				if (asn3 != null && asn3.Tag == 48)
				{
					ASN1 asn4 = asn3;
					for (int i = 0; i < asn4.Count; i++)
					{
						this.entries.Add(new X509Crl.X509CrlEntry(asn4[i]));
					}
				}
				else
				{
					num--;
				}
				ASN1 asn5 = asn2[num];
				if (asn5 != null && asn5.Tag == 160 && asn5.Count == 1)
				{
					this.extensions = new X509ExtensionCollection(asn5[0]);
				}
				else
				{
					this.extensions = new X509ExtensionCollection(null);
				}
				string b = ASN1Convert.ToOid(asn[1][0]);
				if (this.signatureOID != b)
				{
					throw new CryptographicException(text + " [Non-matching signature algorithms in CRL]");
				}
				byte[] value = asn[2].Value;
				this.signature = new byte[value.Length - 1];
				Buffer.BlockCopy(value, 1, this.signature, 0, this.signature.Length);
			}
			catch
			{
				throw new CryptographicException(text);
			}
		}

		public ArrayList Entries
		{
			get
			{
				return ArrayList.ReadOnly(this.entries);
			}
		}

		public X509Crl.X509CrlEntry this[int index]
		{
			get
			{
				return (X509Crl.X509CrlEntry)this.entries[index];
			}
		}

		public X509Crl.X509CrlEntry this[byte[] serialNumber]
		{
			get
			{
				return this.GetCrlEntry(serialNumber);
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
				if (this.hash_value == null)
				{
					ASN1 asn = new ASN1(this.encoded);
					byte[] bytes = asn[0].GetBytes();
					HashAlgorithm hashAlgorithm = HashAlgorithm.Create(this.GetHashName());
					this.hash_value = hashAlgorithm.ComputeHash(bytes);
				}
				return this.hash_value;
			}
		}

		public string IssuerName
		{
			get
			{
				return this.issuer;
			}
		}

		public DateTime NextUpdate
		{
			get
			{
				return this.nextUpdate;
			}
		}

		public DateTime ThisUpdate
		{
			get
			{
				return this.thisUpdate;
			}
		}

		public string SignatureAlgorithm
		{
			get
			{
				return this.signatureOID;
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
		}

		public byte[] RawData
		{
			get
			{
				return (byte[])this.encoded.Clone();
			}
		}

		public byte Version
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
				return this.WasCurrent(DateTime.Now);
			}
		}

		public bool WasCurrent(DateTime instant)
		{
			if (this.nextUpdate == DateTime.MinValue)
			{
				return instant >= this.thisUpdate;
			}
			return instant >= this.thisUpdate && instant <= this.nextUpdate;
		}

		public byte[] GetBytes()
		{
			return (byte[])this.encoded.Clone();
		}

		private bool Compare(byte[] array1, byte[] array2)
		{
			if (array1 == null && array2 == null)
			{
				return true;
			}
			if (array1 == null || array2 == null)
			{
				return false;
			}
			if (array1.Length != array2.Length)
			{
				return false;
			}
			for (int i = 0; i < array1.Length; i++)
			{
				if (array1[i] != array2[i])
				{
					return false;
				}
			}
			return true;
		}

		public X509Crl.X509CrlEntry GetCrlEntry(X509Certificate x509)
		{
			if (x509 == null)
			{
				throw new ArgumentNullException("x509");
			}
			return this.GetCrlEntry(x509.SerialNumber);
		}

		public X509Crl.X509CrlEntry GetCrlEntry(byte[] serialNumber)
		{
			if (serialNumber == null)
			{
				throw new ArgumentNullException("serialNumber");
			}
			for (int i = 0; i < this.entries.Count; i++)
			{
				X509Crl.X509CrlEntry x509CrlEntry = (X509Crl.X509CrlEntry)this.entries[i];
				if (this.Compare(serialNumber, x509CrlEntry.SerialNumber))
				{
					return x509CrlEntry;
				}
			}
			return null;
		}

		public bool VerifySignature(X509Certificate x509)
		{
			if (x509 == null)
			{
				throw new ArgumentNullException("x509");
			}
			if (x509.Version >= 3)
			{
				X509Extension x509Extension = x509.Extensions["2.5.29.15"];
				if (x509Extension != null)
				{
					KeyUsageExtension keyUsageExtension = new KeyUsageExtension(x509Extension);
					if (!keyUsageExtension.Support(KeyUsages.cRLSign))
					{
						return false;
					}
				}
				x509Extension = x509.Extensions["2.5.29.19"];
				if (x509Extension != null)
				{
					BasicConstraintsExtension basicConstraintsExtension = new BasicConstraintsExtension(x509Extension);
					if (!basicConstraintsExtension.CertificateAuthority)
					{
						return false;
					}
				}
			}
			if (this.issuer != x509.SubjectName)
			{
				return false;
			}
			string text = this.signatureOID;
			if (text != null)
			{
				if (X509Crl.<>f__switch$map11 == null)
				{
					X509Crl.<>f__switch$map11 = new Dictionary<string, int>(1)
					{
						{
							"1.2.840.10040.4.3",
							0
						}
					};
				}
				int num;
				if (X509Crl.<>f__switch$map11.TryGetValue(text, out num))
				{
					if (num == 0)
					{
						return this.VerifySignature(x509.DSA);
					}
				}
			}
			return this.VerifySignature(x509.RSA);
		}

		private string GetHashName()
		{
			string text = this.signatureOID;
			switch (text)
			{
			case "1.2.840.113549.1.1.2":
				return "MD2";
			case "1.2.840.113549.1.1.4":
				return "MD5";
			case "1.2.840.10040.4.3":
			case "1.2.840.113549.1.1.5":
				return "SHA1";
			}
			throw new CryptographicException("Unsupported hash algorithm: " + this.signatureOID);
		}

		internal bool VerifySignature(DSA dsa)
		{
			if (this.signatureOID != "1.2.840.10040.4.3")
			{
				throw new CryptographicException("Unsupported hash algorithm: " + this.signatureOID);
			}
			DSASignatureDeformatter dsasignatureDeformatter = new DSASignatureDeformatter(dsa);
			dsasignatureDeformatter.SetHashAlgorithm("SHA1");
			ASN1 asn = new ASN1(this.signature);
			if (asn == null || asn.Count != 2)
			{
				return false;
			}
			byte[] value = asn[0].Value;
			byte[] value2 = asn[1].Value;
			byte[] array = new byte[40];
			int num = Math.Max(0, value.Length - 20);
			int dstOffset = Math.Max(0, 20 - value.Length);
			Buffer.BlockCopy(value, num, array, dstOffset, value.Length - num);
			int num2 = Math.Max(0, value2.Length - 20);
			int dstOffset2 = Math.Max(20, 40 - value2.Length);
			Buffer.BlockCopy(value2, num2, array, dstOffset2, value2.Length - num2);
			return dsasignatureDeformatter.VerifySignature(this.Hash, array);
		}

		internal bool VerifySignature(RSA rsa)
		{
			RSAPKCS1SignatureDeformatter rsapkcs1SignatureDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
			rsapkcs1SignatureDeformatter.SetHashAlgorithm(this.GetHashName());
			return rsapkcs1SignatureDeformatter.VerifySignature(this.Hash, this.signature);
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

		public static X509Crl CreateFromFile(string filename)
		{
			byte[] array = null;
			using (FileStream fileStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				array = new byte[fileStream.Length];
				fileStream.Read(array, 0, array.Length);
				fileStream.Close();
			}
			return new X509Crl(array);
		}

		public class X509CrlEntry
		{
			private byte[] sn;

			private DateTime revocationDate;

			private X509ExtensionCollection extensions;

			internal X509CrlEntry(byte[] serialNumber, DateTime revocationDate, X509ExtensionCollection extensions)
			{
				this.sn = serialNumber;
				this.revocationDate = revocationDate;
				if (extensions == null)
				{
					this.extensions = new X509ExtensionCollection();
				}
				else
				{
					this.extensions = extensions;
				}
			}

			internal X509CrlEntry(ASN1 entry)
			{
				this.sn = entry[0].Value;
				Array.Reverse(this.sn);
				this.revocationDate = ASN1Convert.ToDateTime(entry[1]);
				this.extensions = new X509ExtensionCollection(entry[2]);
			}

			public byte[] SerialNumber
			{
				get
				{
					return (byte[])this.sn.Clone();
				}
			}

			public DateTime RevocationDate
			{
				get
				{
					return this.revocationDate;
				}
			}

			public X509ExtensionCollection Extensions
			{
				get
				{
					return this.extensions;
				}
			}

			public byte[] GetBytes()
			{
				ASN1 asn = new ASN1(48);
				asn.Add(new ASN1(2, this.sn));
				asn.Add(ASN1Convert.FromDateTime(this.revocationDate));
				if (this.extensions.Count > 0)
				{
					asn.Add(new ASN1(this.extensions.GetBytes()));
				}
				return asn.GetBytes();
			}
		}
	}
}
