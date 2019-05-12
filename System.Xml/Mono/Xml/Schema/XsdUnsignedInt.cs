using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdUnsignedInt : XsdUnsignedLong
	{
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.UnsignedInt;
			}
		}

		public override Type ValueType
		{
			get
			{
				return typeof(uint);
			}
		}

		public override object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			return this.ParseValueType(s, nameTable, nsmgr);
		}

		internal override ValueType ParseValueType(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			return XmlConvert.ToUInt32(base.Normalize(s));
		}

		internal override XsdOrdering Compare(object x, object y)
		{
			if (!(x is uint) || !(y is uint))
			{
				return XsdOrdering.Indeterminate;
			}
			if ((uint)x == (uint)y)
			{
				return XsdOrdering.Equal;
			}
			if ((uint)x < (uint)y)
			{
				return XsdOrdering.LessThan;
			}
			return XsdOrdering.GreaterThan;
		}
	}
}
