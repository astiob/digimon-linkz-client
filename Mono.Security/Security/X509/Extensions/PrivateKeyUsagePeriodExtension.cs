using System;
using System.Globalization;
using System.Text;

namespace Mono.Security.X509.Extensions
{
	public class PrivateKeyUsagePeriodExtension : X509Extension
	{
		private DateTime notBefore;

		private DateTime notAfter;

		public PrivateKeyUsagePeriodExtension()
		{
			this.extnOid = "2.5.29.16";
		}

		public PrivateKeyUsagePeriodExtension(ASN1 asn1) : base(asn1)
		{
		}

		public PrivateKeyUsagePeriodExtension(X509Extension extension) : base(extension)
		{
		}

		protected override void Decode()
		{
			ASN1 asn = new ASN1(this.extnValue.Value);
			if (asn.Tag != 48)
			{
				throw new ArgumentException("Invalid PrivateKeyUsagePeriod extension");
			}
			for (int i = 0; i < asn.Count; i++)
			{
				byte tag = asn[i].Tag;
				if (tag != 128)
				{
					if (tag != 129)
					{
						throw new ArgumentException("Invalid PrivateKeyUsagePeriod extension");
					}
					this.notAfter = ASN1Convert.ToDateTime(asn[i]);
				}
				else
				{
					this.notBefore = ASN1Convert.ToDateTime(asn[i]);
				}
			}
		}

		public override string Name
		{
			get
			{
				return "Private Key Usage Period";
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.notBefore != DateTime.MinValue)
			{
				stringBuilder.Append("Not Before: ");
				stringBuilder.Append(this.notBefore.ToString(CultureInfo.CurrentUICulture));
				stringBuilder.Append(Environment.NewLine);
			}
			if (this.notAfter != DateTime.MinValue)
			{
				stringBuilder.Append("Not After: ");
				stringBuilder.Append(this.notAfter.ToString(CultureInfo.CurrentUICulture));
				stringBuilder.Append(Environment.NewLine);
			}
			return stringBuilder.ToString();
		}
	}
}
