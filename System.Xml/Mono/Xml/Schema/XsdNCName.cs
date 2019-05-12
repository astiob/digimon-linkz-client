using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdNCName : XsdName
	{
		internal XsdNCName()
		{
		}

		public override XmlTokenizedType TokenizedType
		{
			get
			{
				return XmlTokenizedType.NCName;
			}
		}

		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.NCName;
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
			if (!XmlChar.IsNCName(s))
			{
				throw new ArgumentException("'" + s + "' is an invalid NCName.");
			}
			return s;
		}

		internal override ValueType ParseValueType(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			return new StringValueType(this.ParseValue(s, nameTable, nsmgr) as string);
		}
	}
}
