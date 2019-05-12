using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdUnsignedLong : XsdNonNegativeInteger
	{
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.UnsignedLong;
			}
		}

		public override Type ValueType
		{
			get
			{
				return typeof(ulong);
			}
		}

		public override object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			return this.ParseValueType(s, nameTable, nsmgr);
		}

		internal override ValueType ParseValueType(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			return XmlConvert.ToUInt64(base.Normalize(s));
		}

		internal override XsdOrdering Compare(object x, object y)
		{
			if (!(x is ulong) || !(y is ulong))
			{
				return XsdOrdering.Indeterminate;
			}
			if ((ulong)x == (ulong)y)
			{
				return XsdOrdering.Equal;
			}
			if ((ulong)x < (ulong)y)
			{
				return XsdOrdering.LessThan;
			}
			return XsdOrdering.GreaterThan;
		}
	}
}
