using System;
using System.Collections;
using System.Text;

namespace Mono.Security.X509.Extensions
{
	public class CRLDistributionPointsExtension : X509Extension
	{
		private ArrayList dps;

		public CRLDistributionPointsExtension()
		{
			this.extnOid = "2.5.29.31";
			this.dps = new ArrayList();
		}

		public CRLDistributionPointsExtension(ASN1 asn1) : base(asn1)
		{
		}

		public CRLDistributionPointsExtension(X509Extension extension) : base(extension)
		{
		}

		protected override void Decode()
		{
			this.dps = new ArrayList();
			ASN1 asn = new ASN1(this.extnValue.Value);
			if (asn.Tag != 48)
			{
				throw new ArgumentException("Invalid CRLDistributionPoints extension");
			}
			for (int i = 0; i < asn.Count; i++)
			{
				this.dps.Add(new CRLDistributionPointsExtension.DP(asn[i]));
			}
		}

		public override string Name
		{
			get
			{
				return "CRL Distribution Points";
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 1;
			foreach (object obj in this.dps)
			{
				CRLDistributionPointsExtension.DP dp = (CRLDistributionPointsExtension.DP)obj;
				stringBuilder.Append("[");
				stringBuilder.Append(num++);
				stringBuilder.Append("]CRL Distribution Point");
				stringBuilder.Append(Environment.NewLine);
				stringBuilder.Append("\tDistribution Point Name:");
				stringBuilder.Append("\t\tFull Name:");
				stringBuilder.Append(Environment.NewLine);
				stringBuilder.Append("\t\t\t");
				stringBuilder.Append(dp.DistributionPoint);
				stringBuilder.Append(Environment.NewLine);
			}
			return stringBuilder.ToString();
		}

		internal class DP
		{
			public string DistributionPoint;

			public CRLDistributionPointsExtension.ReasonFlags Reasons;

			public string CRLIssuer;

			public DP(string dp, CRLDistributionPointsExtension.ReasonFlags reasons, string issuer)
			{
				this.DistributionPoint = dp;
				this.Reasons = reasons;
				this.CRLIssuer = issuer;
			}

			public DP(ASN1 dp)
			{
				for (int i = 0; i < dp.Count; i++)
				{
					ASN1 asn = dp[i];
					switch (asn.Tag)
					{
					case 160:
						for (int j = 0; j < asn.Count; j++)
						{
							ASN1 asn2 = asn[j];
							if (asn2.Tag == 160)
							{
								this.DistributionPoint = new GeneralNames(asn2).ToString();
							}
						}
						break;
					}
				}
			}
		}

		[Flags]
		public enum ReasonFlags
		{
			Unused = 0,
			KeyCompromise = 1,
			CACompromise = 2,
			AffiliationChanged = 3,
			Superseded = 4,
			CessationOfOperation = 5,
			CertificateHold = 6,
			PrivilegeWithdrawn = 7,
			AACompromise = 8
		}
	}
}
