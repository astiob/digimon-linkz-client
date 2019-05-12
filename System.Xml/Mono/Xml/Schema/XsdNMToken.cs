using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdNMToken : XsdToken
	{
		internal XsdNMToken()
		{
		}

		public override XmlTokenizedType TokenizedType
		{
			get
			{
				return XmlTokenizedType.NMTOKEN;
			}
		}

		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.NmToken;
			}
		}

		public override Type ValueType
		{
			get
			{
				return typeof(string);
			}
		}

		public override object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			if (!XmlChar.IsNmToken(s))
			{
				throw new ArgumentException("'" + s + "' is an invalid NMTOKEN.");
			}
			return s;
		}

		internal override ValueType ParseValueType(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			return new StringValueType(this.ParseValue(s, nameTable, nsmgr) as string);
		}
	}
}
