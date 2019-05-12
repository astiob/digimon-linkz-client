using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdInteger : XsdDecimal
	{
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Integer;
			}
		}

		public override Type ValueType
		{
			get
			{
				return typeof(decimal);
			}
		}

		public override object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			return this.ParseValueType(s, nameTable, nsmgr);
		}

		internal override ValueType ParseValueType(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			decimal num = XmlConvert.ToDecimal(base.Normalize(s));
			if (decimal.Floor(num) != num)
			{
				throw new FormatException("Integer contains point number.");
			}
			return num;
		}
	}
}
