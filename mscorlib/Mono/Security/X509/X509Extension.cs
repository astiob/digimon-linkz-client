using System;
using System.Globalization;
using System.Text;

namespace Mono.Security.X509
{
	internal class X509Extension
	{
		protected string extnOid;

		protected bool extnCritical;

		protected ASN1 extnValue;

		protected X509Extension()
		{
			this.extnCritical = false;
		}

		public X509Extension(ASN1 asn1)
		{
			if (asn1.Tag != 48 || asn1.Count < 2)
			{
				throw new ArgumentException(Locale.GetText("Invalid X.509 extension."));
			}
			if (asn1[0].Tag != 6)
			{
				throw new ArgumentException(Locale.GetText("Invalid X.509 extension."));
			}
			this.extnOid = ASN1Convert.ToOid(asn1[0]);
			this.extnCritical = (asn1[1].Tag == 1 && asn1[1].Value[0] == byte.MaxValue);
			this.extnValue = asn1[asn1.Count - 1];
			if (this.extnValue.Tag == 4 && this.extnValue.Length > 0 && this.extnValue.Count == 0)
			{
				try
				{
					ASN1 asn2 = new ASN1(this.extnValue.Value);
					this.extnValue.Value = null;
					this.extnValue.Add(asn2);
				}
				catch
				{
				}
			}
			this.Decode();
		}

		public X509Extension(X509Extension extension)
		{
			if (extension == null)
			{
				throw new ArgumentNullException("extension");
			}
			if (extension.Value == null || extension.Value.Tag != 4 || extension.Value.Count != 1)
			{
				throw new ArgumentException(Locale.GetText("Invalid X.509 extension."));
			}
			this.extnOid = extension.Oid;
			this.extnCritical = extension.Critical;
			this.extnValue = extension.Value;
			this.Decode();
		}

		protected virtual void Decode()
		{
		}

		protected virtual void Encode()
		{
		}

		public ASN1 ASN1
		{
			get
			{
				ASN1 asn = new ASN1(48);
				asn.Add(ASN1Convert.FromOid(this.extnOid));
				if (this.extnCritical)
				{
					asn.Add(new ASN1(1, new byte[]
					{
						byte.MaxValue
					}));
				}
				this.Encode();
				asn.Add(this.extnValue);
				return asn;
			}
		}

		public string Oid
		{
			get
			{
				return this.extnOid;
			}
		}

		public bool Critical
		{
			get
			{
				return this.extnCritical;
			}
			set
			{
				this.extnCritical = value;
			}
		}

		public virtual string Name
		{
			get
			{
				return this.extnOid;
			}
		}

		public ASN1 Value
		{
			get
			{
				if (this.extnValue == null)
				{
					this.Encode();
				}
				return this.extnValue;
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			X509Extension x509Extension = obj as X509Extension;
			if (x509Extension == null)
			{
				return false;
			}
			if (this.extnCritical != x509Extension.extnCritical)
			{
				return false;
			}
			if (this.extnOid != x509Extension.extnOid)
			{
				return false;
			}
			if (this.extnValue.Length != x509Extension.extnValue.Length)
			{
				return false;
			}
			for (int i = 0; i < this.extnValue.Length; i++)
			{
				if (this.extnValue[i] != x509Extension.extnValue[i])
				{
					return false;
				}
			}
			return true;
		}

		public byte[] GetBytes()
		{
			return this.ASN1.GetBytes();
		}

		public override int GetHashCode()
		{
			return this.extnOid.GetHashCode();
		}

		private void WriteLine(StringBuilder sb, int n, int pos)
		{
			byte[] value = this.extnValue.Value;
			int num = pos;
			for (int i = 0; i < 8; i++)
			{
				if (i < n)
				{
					sb.Append(value[num++].ToString("X2", CultureInfo.InvariantCulture));
					sb.Append(" ");
				}
				else
				{
					sb.Append("   ");
				}
			}
			sb.Append("  ");
			num = pos;
			for (int j = 0; j < n; j++)
			{
				byte b = value[num++];
				if (b < 32)
				{
					sb.Append(".");
				}
				else
				{
					sb.Append(Convert.ToChar(b));
				}
			}
			sb.Append(Environment.NewLine);
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = this.extnValue.Length >> 3;
			int n = this.extnValue.Length - (num << 3);
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				this.WriteLine(stringBuilder, 8, num2);
				num2 += 8;
			}
			this.WriteLine(stringBuilder, n, num2);
			return stringBuilder.ToString();
		}
	}
}
