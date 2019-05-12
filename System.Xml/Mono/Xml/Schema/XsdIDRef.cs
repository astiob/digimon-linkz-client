using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdIDRef : XsdName
	{
		internal XsdIDRef()
		{
		}

		public override XmlTokenizedType TokenizedType
		{
			get
			{
				return XmlTokenizedType.IDREF;
			}
		}

		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Idref;
			}
		}

		public override Type ValueType
		{
			get
			{
				return typeof(string);
			}
		}

		public override object ParseValue(string s, XmlNameTable nt, IXmlNamespaceResolver nsmgr)
		{
			if (!XmlChar.IsNCName(s))
			{
				throw new ArgumentException("'" + s + "' is an invalid NCName.");
			}
			return s;
		}
	}
}
