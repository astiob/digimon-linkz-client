using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdID : XsdName
	{
		internal XsdID()
		{
		}

		public override XmlTokenizedType TokenizedType
		{
			get
			{
				return XmlTokenizedType.ID;
			}
		}

		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Id;
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
