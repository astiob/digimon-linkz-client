using System;
using System.Globalization;
using System.Text;

namespace Mono.Security.X509.Extensions
{
	public class KeyAttributesExtension : X509Extension
	{
		private byte[] keyId;

		private int kubits;

		private DateTime notBefore;

		private DateTime notAfter;

		public KeyAttributesExtension()
		{
			this.extnOid = "2.5.29.2";
		}

		public KeyAttributesExtension(ASN1 asn1) : base(asn1)
		{
		}

		public KeyAttributesExtension(X509Extension extension) : base(extension)
		{
		}

		protected override void Decode()
		{
			ASN1 asn = new ASN1(this.extnValue.Value);
			if (asn.Tag != 48)
			{
				throw new ArgumentException("Invalid KeyAttributesExtension extension");
			}
			int num = 0;
			if (num < asn.Count)
			{
				ASN1 asn2 = asn[num];
				if (asn2.Tag == 4)
				{
					num++;
					this.keyId = asn2.Value;
				}
			}
			if (num < asn.Count)
			{
				ASN1 asn3 = asn[num];
				if (asn3.Tag == 3)
				{
					num++;
					int i = 1;
					while (i < asn3.Value.Length)
					{
						this.kubits = (this.kubits << 8) + (int)asn3.Value[i++];
					}
				}
			}
			if (num < asn.Count)
			{
				ASN1 asn4 = asn[num];
				if (asn4.Tag == 48)
				{
					int num2 = 0;
					if (num2 < asn4.Count)
					{
						ASN1 asn5 = asn4[num2];
						if (asn5.Tag == 129)
						{
							num2++;
							this.notBefore = ASN1Convert.ToDateTime(asn5);
						}
					}
					if (num2 < asn4.Count)
					{
						ASN1 asn6 = asn4[num2];
						if (asn6.Tag == 130)
						{
							this.notAfter = ASN1Convert.ToDateTime(asn6);
						}
					}
				}
			}
		}

		public byte[] KeyIdentifier
		{
			get
			{
				if (this.keyId == null)
				{
					return null;
				}
				return (byte[])this.keyId.Clone();
			}
		}

		public override string Name
		{
			get
			{
				return "Key Attributes";
			}
		}

		public DateTime NotAfter
		{
			get
			{
				return this.notAfter;
			}
		}

		public DateTime NotBefore
		{
			get
			{
				return this.notBefore;
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
			if (this.keyId != null)
			{
				stringBuilder.Append("KeyID=");
				for (int i = 0; i < this.keyId.Length; i++)
				{
					stringBuilder.Append(this.keyId[i].ToString("X2", CultureInfo.InvariantCulture));
					if (i % 2 == 1)
					{
						stringBuilder.Append(" ");
					}
				}
				stringBuilder.Append(Environment.NewLine);
			}
			if (this.kubits != 0)
			{
				stringBuilder.Append("Key Usage=");
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
			}
			if (this.notBefore != DateTime.MinValue)
			{
				stringBuilder.Append("Not Before=");
				stringBuilder.Append(this.notBefore.ToString(CultureInfo.CurrentUICulture));
				stringBuilder.Append(Environment.NewLine);
			}
			if (this.notAfter != DateTime.MinValue)
			{
				stringBuilder.Append("Not After=");
				stringBuilder.Append(this.notAfter.ToString(CultureInfo.CurrentUICulture));
				stringBuilder.Append(Environment.NewLine);
			}
			return stringBuilder.ToString();
		}
	}
}
