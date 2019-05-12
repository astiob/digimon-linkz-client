using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Mono.Security.X509.Extensions
{
	public class ExtendedKeyUsageExtension : X509Extension
	{
		private ArrayList keyPurpose;

		public ExtendedKeyUsageExtension()
		{
			this.extnOid = "2.5.29.37";
			this.keyPurpose = new ArrayList();
		}

		public ExtendedKeyUsageExtension(ASN1 asn1) : base(asn1)
		{
		}

		public ExtendedKeyUsageExtension(X509Extension extension) : base(extension)
		{
		}

		protected override void Decode()
		{
			this.keyPurpose = new ArrayList();
			ASN1 asn = new ASN1(this.extnValue.Value);
			if (asn.Tag != 48)
			{
				throw new ArgumentException("Invalid ExtendedKeyUsage extension");
			}
			for (int i = 0; i < asn.Count; i++)
			{
				this.keyPurpose.Add(ASN1Convert.ToOid(asn[i]));
			}
		}

		protected override void Encode()
		{
			ASN1 asn = new ASN1(48);
			foreach (object obj in this.keyPurpose)
			{
				string oid = (string)obj;
				asn.Add(ASN1Convert.FromOid(oid));
			}
			this.extnValue = new ASN1(4);
			this.extnValue.Add(asn);
		}

		public ArrayList KeyPurpose
		{
			get
			{
				return this.keyPurpose;
			}
		}

		public override string Name
		{
			get
			{
				return "Extended Key Usage";
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object obj in this.keyPurpose)
			{
				string text = (string)obj;
				string text2 = text;
				if (text2 == null)
				{
					goto IL_12E;
				}
				if (ExtendedKeyUsageExtension.<>f__switch$map14 == null)
				{
					ExtendedKeyUsageExtension.<>f__switch$map14 = new Dictionary<string, int>(6)
					{
						{
							"1.3.6.1.5.5.7.3.1",
							0
						},
						{
							"1.3.6.1.5.5.7.3.2",
							1
						},
						{
							"1.3.6.1.5.5.7.3.3",
							2
						},
						{
							"1.3.6.1.5.5.7.3.4",
							3
						},
						{
							"1.3.6.1.5.5.7.3.8",
							4
						},
						{
							"1.3.6.1.5.5.7.3.9",
							5
						}
					};
				}
				int num;
				if (!ExtendedKeyUsageExtension.<>f__switch$map14.TryGetValue(text2, out num))
				{
					goto IL_12E;
				}
				switch (num)
				{
				case 0:
					stringBuilder.Append("Server Authentication");
					break;
				case 1:
					stringBuilder.Append("Client Authentication");
					break;
				case 2:
					stringBuilder.Append("Code Signing");
					break;
				case 3:
					stringBuilder.Append("Email Protection");
					break;
				case 4:
					stringBuilder.Append("Time Stamping");
					break;
				case 5:
					stringBuilder.Append("OCSP Signing");
					break;
				default:
					goto IL_12E;
				}
				IL_13F:
				stringBuilder.AppendFormat(" ({0}){1}", text, Environment.NewLine);
				continue;
				IL_12E:
				stringBuilder.Append("unknown");
				goto IL_13F;
			}
			return stringBuilder.ToString();
		}
	}
}
