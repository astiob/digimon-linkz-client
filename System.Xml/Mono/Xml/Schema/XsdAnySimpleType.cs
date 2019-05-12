using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdAnySimpleType : XmlSchemaDatatype
	{
		private static XsdAnySimpleType instance;

		private static readonly char[] whitespaceArray = new char[]
		{
			' '
		};

		internal static readonly XmlSchemaFacet.Facet booleanAllowedFacets = XmlSchemaFacet.Facet.pattern | XmlSchemaFacet.Facet.whiteSpace;

		internal static readonly XmlSchemaFacet.Facet decimalAllowedFacets = XmlSchemaFacet.Facet.pattern | XmlSchemaFacet.Facet.enumeration | XmlSchemaFacet.Facet.whiteSpace | XmlSchemaFacet.Facet.maxInclusive | XmlSchemaFacet.Facet.maxExclusive | XmlSchemaFacet.Facet.minExclusive | XmlSchemaFacet.Facet.minInclusive | XmlSchemaFacet.Facet.totalDigits | XmlSchemaFacet.Facet.fractionDigits;

		internal static readonly XmlSchemaFacet.Facet durationAllowedFacets = XmlSchemaFacet.Facet.pattern | XmlSchemaFacet.Facet.enumeration | XmlSchemaFacet.Facet.whiteSpace | XmlSchemaFacet.Facet.maxInclusive | XmlSchemaFacet.Facet.maxExclusive | XmlSchemaFacet.Facet.minExclusive | XmlSchemaFacet.Facet.minInclusive;

		internal static readonly XmlSchemaFacet.Facet stringAllowedFacets = XmlSchemaFacet.Facet.length | XmlSchemaFacet.Facet.minLength | XmlSchemaFacet.Facet.maxLength | XmlSchemaFacet.Facet.pattern | XmlSchemaFacet.Facet.enumeration | XmlSchemaFacet.Facet.whiteSpace;

		protected XsdAnySimpleType()
		{
		}

		static XsdAnySimpleType()
		{
			XsdAnySimpleType.instance = new XsdAnySimpleType();
		}

		public static XsdAnySimpleType Instance
		{
			get
			{
				return XsdAnySimpleType.instance;
			}
		}

		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.AnyAtomicType;
			}
		}

		public virtual bool Bounded
		{
			get
			{
				return false;
			}
		}

		public virtual bool Finite
		{
			get
			{
				return false;
			}
		}

		public virtual bool Numeric
		{
			get
			{
				return false;
			}
		}

		public virtual XsdOrderedFacet Ordered
		{
			get
			{
				return XsdOrderedFacet.False;
			}
		}

		public override Type ValueType
		{
			get
			{
				if (XmlSchemaUtil.StrictMsCompliant)
				{
					return typeof(string);
				}
				return typeof(object);
			}
		}

		public override XmlTokenizedType TokenizedType
		{
			get
			{
				return XmlTokenizedType.None;
			}
		}

		public override object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			return base.Normalize(s);
		}

		internal override ValueType ParseValueType(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			return new StringValueType(base.Normalize(s));
		}

		internal string[] ParseListValue(string s, XmlNameTable nameTable)
		{
			return base.Normalize(s, XsdWhitespaceFacet.Collapse).Split(XsdAnySimpleType.whitespaceArray);
		}

		internal bool AllowsFacet(XmlSchemaFacet xsf)
		{
			return (this.AllowedFacets & xsf.ThisFacet) != XmlSchemaFacet.Facet.None;
		}

		internal virtual XsdOrdering Compare(object x, object y)
		{
			return XsdOrdering.Indeterminate;
		}

		internal virtual int Length(string s)
		{
			return s.Length;
		}

		internal virtual XmlSchemaFacet.Facet AllowedFacets
		{
			get
			{
				return XmlSchemaFacet.AllFacets;
			}
		}
	}
}
