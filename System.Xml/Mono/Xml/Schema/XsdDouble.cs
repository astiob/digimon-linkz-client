using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdDouble : XsdAnySimpleType
	{
		internal XsdDouble()
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

		public override bool Bounded
		{
			get
			{
				return true;
			}
		}

		public override bool Finite
		{
			get
			{
				return true;
			}
		}

		public override bool Numeric
		{
			get
			{
				return true;
			}
		}

		public override XsdOrderedFacet Ordered
		{
			get
			{
				return XsdOrderedFacet.Total;
			}
		}

		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Double;
			}
		}

		public override Type ValueType
		{
			get
			{
				return typeof(double);
			}
		}

		public override object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			return this.ParseValueType(s, nameTable, nsmgr);
		}

		internal override ValueType ParseValueType(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			return XmlConvert.ToDouble(base.Normalize(s));
		}

		internal override XsdOrdering Compare(object x, object y)
		{
			if (!(x is double) || !(y is double))
			{
				return XsdOrdering.Indeterminate;
			}
			if ((double)x == (double)y)
			{
				return XsdOrdering.Equal;
			}
			if ((double)x < (double)y)
			{
				return XsdOrdering.LessThan;
			}
			return XsdOrdering.GreaterThan;
		}
	}
}
