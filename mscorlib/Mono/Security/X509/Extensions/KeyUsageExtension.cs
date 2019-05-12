using System;
using System.Globalization;
using System.Text;

namespace Mono.Security.X509.Extensions
{
	internal class KeyUsageExtension : X509Extension
	{
		private int kubits;

		public KeyUsageExtension(ASN1 asn1) : base(asn1)
		{
		}

		public KeyUsageExtension(X509Extension extension) : base(extension)
		{
		}

		public KeyUsageExtension()
		{
			this.extnOid = "2.5.29.15";
		}

		protected override void Decode()
		{
			ASN1 asn = new ASN1(this.extnValue.Value);
			if (asn.Tag != 3)
			{
				throw new ArgumentException("Invalid KeyUsage extension");
			}
			int i = 1;
			while (i < asn.Value.Length)
			{
				this.kubits = (this.kubits << 8) + (int)asn.Value[i++];
			}
		}

		protected override void Encode()
		{
			this.extnValue = new ASN1(4);
			ushort num = (ushort)this.kubits;
			if (num > 0)
			{
				byte b;
				for (b = 15; b > 0; b -= 1)
				{
					if ((num & 32768) == 32768)
					{
						break;
					}
					num = (ushort)(num << 1);
				}
				if (this.kubits > 255)
				{
					b -= 8;
					this.extnValue.Add(new ASN1(3, new byte[]
					{
						b,
						(byte)this.kubits,
						(byte)(this.kubits >> 8)
					}));
				}
				else
				{
					this.extnValue.Add(new ASN1(3, new byte[]
					{
						b,
						(byte)this.kubits
					}));
				}
			}
			else
			{
				ASN1 extnValue = this.extnValue;
				byte tag = 3;
				byte[] array = new byte[2];
				array[0] = 7;
				extnValue.Add(new ASN1(tag, array));
			}
		}

		public KeyUsages KeyUsage
		{
			get
			{
				return (KeyUsages)this.kubits;
			}
			set
			{
				this.kubits = Convert.ToInt32(value, CultureInfo.InvariantCulture);
			}
		}

		public override string Name
		{
			get
			{
				return "Key Usage";
			}
		}

		public bool Support(KeyUsages usage)
		{
			int num = Convert.ToInt32(usage, CultureInfo.InvariantCulture);
			return (num & this.kubits) == num;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.Support(KeyUsages.digitalSignature))
			{
				stringBuilder.Append("Digital Signature");
			}
			if (this.Support(KeyUsages.nonRepudiation))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" , ");
				}
				stringBuilder.Append("Non-Repudiation");
			}
			if (this.Support(KeyUsages.keyEncipherment))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" , ");
				}
				stringBuilder.Append("Key Encipherment");
			}
			if (this.Support(KeyUsages.dataEncipherment))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" , ");
				}
				stringBuilder.Append("Data Encipherment");
			}
			if (this.Support(KeyUsages.keyAgreement))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" , ");
				}
				stringBuilder.Append("Key Agreement");
			}
			if (this.Support(KeyUsages.keyCertSign))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" , ");
				}
				stringBuilder.Append("Certificate Signing");
			}
			if (this.Support(KeyUsages.cRLSign))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" , ");
				}
				stringBuilder.Append("CRL Signing");
			}
			if (this.Support(KeyUsages.encipherOnly))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" , ");
				}
				stringBuilder.Append("Encipher Only ");
			}
			if (this.Support(KeyUsages.decipherOnly))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" , ");
				}
				stringBuilder.Append("Decipher Only");
			}
			stringBuilder.Append("(");
			stringBuilder.Append(this.kubits.ToString("X2", CultureInfo.InvariantCulture));
			stringBuilder.Append(")");
			stringBuilder.Append(Environment.NewLine);
			return stringBuilder.ToString();
		}
	}
}
