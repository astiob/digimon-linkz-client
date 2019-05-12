using System;
using System.Globalization;
using System.Text;

namespace Mono.Security.X509.Extensions
{
	public class NetscapeCertTypeExtension : X509Extension
	{
		private int ctbits;

		public NetscapeCertTypeExtension()
		{
			this.extnOid = "2.16.840.1.113730.1.1";
		}

		public NetscapeCertTypeExtension(ASN1 asn1) : base(asn1)
		{
		}

		public NetscapeCertTypeExtension(X509Extension extension) : base(extension)
		{
		}

		protected override void Decode()
		{
			ASN1 asn = new ASN1(this.extnValue.Value);
			if (asn.Tag != 3)
			{
				throw new ArgumentException("Invalid NetscapeCertType extension");
			}
			int i = 1;
			while (i < asn.Value.Length)
			{
				this.ctbits = (this.ctbits << 8) + (int)asn.Value[i++];
			}
		}

		public override string Name
		{
			get
			{
				return "NetscapeCertType";
			}
		}

		public bool Support(NetscapeCertTypeExtension.CertTypes usage)
		{
			int num = Convert.ToInt32(usage, CultureInfo.InvariantCulture);
			return (num & this.ctbits) == num;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.Support(NetscapeCertTypeExtension.CertTypes.SslClient))
			{
				stringBuilder.Append("SSL Client Authentication");
			}
			if (this.Support(NetscapeCertTypeExtension.CertTypes.SslServer))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" , ");
				}
				stringBuilder.Append("SSL Server Authentication");
			}
			if (this.Support(NetscapeCertTypeExtension.CertTypes.Smime))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" , ");
				}
				stringBuilder.Append("SMIME");
			}
			if (this.Support(NetscapeCertTypeExtension.CertTypes.ObjectSigning))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" , ");
				}
				stringBuilder.Append("Object Signing");
			}
			if (this.Support(NetscapeCertTypeExtension.CertTypes.SslCA))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" , ");
				}
				stringBuilder.Append("SSL CA");
			}
			if (this.Support(NetscapeCertTypeExtension.CertTypes.SmimeCA))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" , ");
				}
				stringBuilder.Append("SMIME CA");
			}
			if (this.Support(NetscapeCertTypeExtension.CertTypes.ObjectSigningCA))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" , ");
				}
				stringBuilder.Append("Object Signing CA");
			}
			stringBuilder.Append("(");
			stringBuilder.Append(this.ctbits.ToString("X2", CultureInfo.InvariantCulture));
			stringBuilder.Append(")");
			stringBuilder.Append(Environment.NewLine);
			return stringBuilder.ToString();
		}

		[Flags]
		public enum CertTypes
		{
			SslClient = 128,
			SslServer = 64,
			Smime = 32,
			ObjectSigning = 16,
			SslCA = 4,
			SmimeCA = 2,
			ObjectSigningCA = 1
		}
	}
}
