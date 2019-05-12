using System;
using System.Globalization;
using System.Text;

namespace Mono.Security.X509.Extensions
{
	public class AuthorityKeyIdentifierExtension : X509Extension
	{
		private byte[] aki;

		public AuthorityKeyIdentifierExtension()
		{
			this.extnOid = "2.5.29.35";
		}

		public AuthorityKeyIdentifierExtension(ASN1 asn1) : base(asn1)
		{
		}

		public AuthorityKeyIdentifierExtension(X509Extension extension) : base(extension)
		{
		}

		protected override void Decode()
		{
			ASN1 asn = new ASN1(this.extnValue.Value);
			if (asn.Tag != 48)
			{
				throw new ArgumentException("Invalid AuthorityKeyIdentifier extension");
			}
			for (int i = 0; i < asn.Count; i++)
			{
				ASN1 asn2 = asn[i];
				byte tag = asn2.Tag;
				if (tag == 128)
				{
					this.aki = asn2.Value;
				}
			}
		}

		public override string Name
		{
			get
			{
				return "Authority Key Identifier";
			}
		}

		public byte[] Identifier
		{
			get
			{
				if (this.aki == null)
				{
					return null;
				}
				return (byte[])this.aki.Clone();
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.aki != null)
			{
				int i = 0;
				stringBuilder.Append("KeyID=");
				while (i < this.aki.Length)
				{
					stringBuilder.Append(this.aki[i].ToString("X2", CultureInfo.InvariantCulture));
					if (i % 2 == 1)
					{
						stringBuilder.Append(" ");
					}
					i++;
				}
			}
			return stringBuilder.ToString();
		}
	}
}
