using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdName : XsdToken
	{
		internal XsdName()
		{
		}

		public override XmlTokenizedType TokenizedType
		{
			get
			{
				return XmlTokenizedType.CDATA;
			}
		}

		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Name;
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
			if (!XmlChar.IsName(s))
			{
				throw new ArgumentException("'" + s + "' is an invalid name.");
			}
			return s;
		}

		internal override ValueType ParseValueType(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			return new StringValueType(this.ParseValue(s, nameTable, nsmgr) as string);
		}
	}
}
