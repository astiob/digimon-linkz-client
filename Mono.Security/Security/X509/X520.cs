using System;
using System.Text;

namespace Mono.Security.X509
{
	public class X520
	{
		public abstract class AttributeTypeAndValue
		{
			private string oid;

			private string attrValue;

			private int upperBound;

			private byte encoding;

			protected AttributeTypeAndValue(string oid, int upperBound)
			{
				this.oid = oid;
				this.upperBound = upperBound;
				this.encoding = byte.MaxValue;
			}

			protected AttributeTypeAndValue(string oid, int upperBound, byte encoding)
			{
				this.oid = oid;
				this.upperBound = upperBound;
				this.encoding = encoding;
			}

			public string Value
			{
				get
				{
					return this.attrValue;
				}
				set
				{
					if (this.attrValue != null && this.attrValue.Length > this.upperBound)
					{
						string text = Locale.GetText("Value length bigger than upperbound ({0}).");
						throw new FormatException(string.Format(text, this.upperBound));
					}
					this.attrValue = value;
				}
			}

			public ASN1 ASN1
			{
				get
				{
					return this.GetASN1();
				}
			}

			internal ASN1 GetASN1(byte encoding)
			{
				byte b = encoding;
				if (b == 255)
				{
					b = this.SelectBestEncoding();
				}
				ASN1 asn = new ASN1(48);
				asn.Add(ASN1Convert.FromOid(this.oid));
				byte b2 = b;
				switch (b2)
				{
				case 19:
					asn.Add(new ASN1(19, Encoding.ASCII.GetBytes(this.attrValue)));
					break;
				default:
					if (b2 == 30)
					{
						asn.Add(new ASN1(30, Encoding.BigEndianUnicode.GetBytes(this.attrValue)));
					}
					break;
				case 22:
					asn.Add(new ASN1(22, Encoding.ASCII.GetBytes(this.attrValue)));
					break;
				}
				return asn;
			}

			internal ASN1 GetASN1()
			{
				return this.GetASN1(this.encoding);
			}

			public byte[] GetBytes(byte encoding)
			{
				return this.GetASN1(encoding).GetBytes();
			}

			public byte[] GetBytes()
			{
				return this.GetASN1().GetBytes();
			}

			private byte SelectBestEncoding()
			{
				foreach (char c in this.attrValue)
				{
					char c2 = c;
					if (c2 == '@' || c2 == '_')
					{
						return 30;
					}
					if (c > '\u007f')
					{
						return 30;
					}
				}
				return 19;
			}
		}

		public class Name : X520.AttributeTypeAndValue
		{
			public Name() : base("2.5.4.41", 32768)
			{
			}
		}

		public class CommonName : X520.AttributeTypeAndValue
		{
			public CommonName() : base("2.5.4.3", 64)
			{
			}
		}

		public class SerialNumber : X520.AttributeTypeAndValue
		{
			public SerialNumber() : base("2.5.4.5", 64, 19)
			{
			}
		}

		public class LocalityName : X520.AttributeTypeAndValue
		{
			public LocalityName() : base("2.5.4.7", 128)
			{
			}
		}

		public class StateOrProvinceName : X520.AttributeTypeAndValue
		{
			public StateOrProvinceName() : base("2.5.4.8", 128)
			{
			}
		}

		public class OrganizationName : X520.AttributeTypeAndValue
		{
			public OrganizationName() : base("2.5.4.10", 64)
			{
			}
		}

		public class OrganizationalUnitName : X520.AttributeTypeAndValue
		{
			public OrganizationalUnitName() : base("2.5.4.11", 64)
			{
			}
		}

		public class EmailAddress : X520.AttributeTypeAndValue
		{
			public EmailAddress() : base("1.2.840.113549.1.9.1", 128, 22)
			{
			}
		}

		public class DomainComponent : X520.AttributeTypeAndValue
		{
			public DomainComponent() : base("0.9.2342.19200300.100.1.25", int.MaxValue, 22)
			{
			}
		}

		public class UserId : X520.AttributeTypeAndValue
		{
			public UserId() : base("0.9.2342.19200300.100.1.1", 256)
			{
			}
		}

		public class Oid : X520.AttributeTypeAndValue
		{
			public Oid(string oid) : base(oid, int.MaxValue)
			{
			}
		}

		public class Title : X520.AttributeTypeAndValue
		{
			public Title() : base("2.5.4.12", 64)
			{
			}
		}

		public class CountryName : X520.AttributeTypeAndValue
		{
			public CountryName() : base("2.5.4.6", 2, 19)
			{
			}
		}

		public class DnQualifier : X520.AttributeTypeAndValue
		{
			public DnQualifier() : base("2.5.4.46", 2, 19)
			{
			}
		}

		public class Surname : X520.AttributeTypeAndValue
		{
			public Surname() : base("2.5.4.4", 32768)
			{
			}
		}

		public class GivenName : X520.AttributeTypeAndValue
		{
			public GivenName() : base("2.5.4.42", 16)
			{
			}
		}

		public class Initial : X520.AttributeTypeAndValue
		{
			public Initial() : base("2.5.4.43", 5)
			{
			}
		}
	}
}
