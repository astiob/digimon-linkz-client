using System;
using System.Globalization;
using System.Text;

namespace Mono.Security.X509.Extensions
{
	internal class SubjectKeyIdentifierExtension : X509Extension
	{
		private byte[] ski;

		public SubjectKeyIdentifierExtension()
		{
			this.extnOid = "2.5.29.14";
		}

		public SubjectKeyIdentifierExtension(ASN1 asn1) : base(asn1)
		{
		}

		public SubjectKeyIdentifierExtension(X509Extension extension) : base(extension)
		{
		}

		protected override void Decode()
		{
			ASN1 asn = new ASN1(this.extnValue.Value);
			if (asn.Tag != 4)
			{
				throw new ArgumentException("Invalid SubjectKeyIdentifier extension");
			}
			this.ski = asn.Value;
		}

		public override string Name
		{
			get
			{
				return "Subject Key Identifier";
			}
		}

		public byte[] Identifier
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

		public override string ToString()
		{
			if (this.ski == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.ski.Length; i++)
			{
				stringBuilder.Append(this.ski[i].ToString("X2", CultureInfo.InvariantCulture));
				if (i % 2 == 1)
				{
					stringBuilder.Append(" ");
				}
			}
			return stringBuilder.ToString();
		}
	}
}
