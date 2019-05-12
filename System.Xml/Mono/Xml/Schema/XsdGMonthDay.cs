using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdGMonthDay : XsdAnySimpleType
	{
		internal XsdGMonthDay()
		{
			this.WhitespaceValue = XsdWhitespaceFacet.Collapse;
		}

		internal override XmlSchemaFacet.Facet AllowedFacets
		{
			get
			{
				return XsdAnySimpleType.durationAllowedFacets;
			}
		}

		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.GMonthDay;
			}
		}

		public override Type ValueType
		{
			get
			{
				return typeof(DateTime);
			}
		}

		public override object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			return this.ParseValueType(s, nameTable, nsmgr);
		}

		internal override ValueType ParseValueType(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			return DateTime.ParseExact(base.Normalize(s), "--MM-dd", null);
		}

		internal override XsdOrdering Compare(object x, object y)
		{
			if (!(x is DateTime) || !(y is DateTime))
			{
				return XsdOrdering.Indeterminate;
			}
			int num = DateTime.Compare((DateTime)x, (DateTime)y);
			if (num < 0)
			{
				return XsdOrdering.LessThan;
			}
			if (num > 0)
			{
				return XsdOrdering.GreaterThan;
			}
			return XsdOrdering.Equal;
		}
	}
}
