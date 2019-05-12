using System;
using System.Globalization;
using System.Text;

namespace Mono.Security.X509.Extensions
{
	internal class BasicConstraintsExtension : X509Extension
	{
		public const int NoPathLengthConstraint = -1;

		private bool cA;

		private int pathLenConstraint;

		public BasicConstraintsExtension()
		{
			this.extnOid = "2.5.29.19";
			this.pathLenConstraint = -1;
		}

		public BasicConstraintsExtension(ASN1 asn1) : base(asn1)
		{
		}

		public BasicConstraintsExtension(X509Extension extension) : base(extension)
		{
		}

		protected override void Decode()
		{
			this.cA = false;
			this.pathLenConstraint = -1;
			ASN1 asn = new ASN1(this.extnValue.Value);
			if (asn.Tag != 48)
			{
				throw new ArgumentException("Invalid BasicConstraints extension");
			}
			int num = 0;
			ASN1 asn2 = asn[num++];
			if (asn2 != null && asn2.Tag == 1)
			{
				this.cA = (asn2.Value[0] == byte.MaxValue);
				asn2 = asn[num++];
			}
			if (asn2 != null && asn2.Tag == 2)
			{
				this.pathLenConstraint = ASN1Convert.ToInt32(asn2);
			}
		}

		protected override void Encode()
		{
			ASN1 asn = new ASN1(48);
			if (this.cA)
			{
				asn.Add(new ASN1(1, new byte[]
				{
					byte.MaxValue
				}));
			}
			if (this.cA && this.pathLenConstraint >= 0)
			{
				asn.Add(ASN1Convert.FromInt32(this.pathLenConstraint));
			}
			this.extnValue = new ASN1(4);
			this.extnValue.Add(asn);
		}

		public bool CertificateAuthority
		{
			get
			{
				return this.cA;
			}
			set
			{
				this.cA = value;
			}
		}

		public override string Name
		{
			get
			{
				return "Basic Constraints";
			}
		}

		public int PathLenConstraint
		{
			get
			{
				return this.pathLenConstraint;
			}
			set
			{
				if (value < -1)
				{
					string text = Locale.GetText("PathLenConstraint must be positive or -1 for none ({0}).", new object[]
					{
						value
					});
					throw new ArgumentOutOfRangeException(text);
				}
				this.pathLenConstraint = value;
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Subject Type=");
			stringBuilder.Append((!this.cA) ? "End Entity" : "CA");
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append("Path Length Constraint=");
			if (this.pathLenConstraint == -1)
			{
				stringBuilder.Append("None");
			}
			else
			{
				stringBuilder.Append(this.pathLenConstraint.ToString(CultureInfo.InvariantCulture));
			}
			stringBuilder.Append(Environment.NewLine);
			return stringBuilder.ToString();
		}
	}
}
