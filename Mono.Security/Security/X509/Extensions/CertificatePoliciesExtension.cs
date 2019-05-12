using System;
using System.Collections;
using System.Text;

namespace Mono.Security.X509.Extensions
{
	public class CertificatePoliciesExtension : X509Extension
	{
		private Hashtable policies;

		public CertificatePoliciesExtension()
		{
			this.extnOid = "2.5.29.32";
			this.policies = new Hashtable();
		}

		public CertificatePoliciesExtension(ASN1 asn1) : base(asn1)
		{
		}

		public CertificatePoliciesExtension(X509Extension extension) : base(extension)
		{
		}

		protected override void Decode()
		{
			this.policies = new Hashtable();
			ASN1 asn = new ASN1(this.extnValue.Value);
			if (asn.Tag != 48)
			{
				throw new ArgumentException("Invalid CertificatePolicies extension");
			}
			for (int i = 0; i < asn.Count; i++)
			{
				this.policies.Add(ASN1Convert.ToOid(asn[i][0]), null);
			}
		}

		public override string Name
		{
			get
			{
				return "Certificate Policies";
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 1;
			foreach (object obj in this.policies)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				stringBuilder.Append("[");
				stringBuilder.Append(num++);
				stringBuilder.Append("]Certificate Policy:");
				stringBuilder.Append(Environment.NewLine);
				stringBuilder.Append("\tPolicyIdentifier=");
				stringBuilder.Append((string)dictionaryEntry.Key);
				stringBuilder.Append(Environment.NewLine);
			}
			return stringBuilder.ToString();
		}
	}
}
