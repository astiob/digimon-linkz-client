using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Abstract class for all facets that are used when simple types are derived by restriction.</summary>
	public abstract class XmlSchemaFacet : XmlSchemaAnnotated
	{
		internal static readonly XmlSchemaFacet.Facet AllFacets = XmlSchemaFacet.Facet.length | XmlSchemaFacet.Facet.minLength | XmlSchemaFacet.Facet.maxLength | XmlSchemaFacet.Facet.pattern | XmlSchemaFacet.Facet.enumeration | XmlSchemaFacet.Facet.whiteSpace | XmlSchemaFacet.Facet.maxInclusive | XmlSchemaFacet.Facet.maxExclusive | XmlSchemaFacet.Facet.minExclusive | XmlSchemaFacet.Facet.minInclusive | XmlSchemaFacet.Facet.totalDigits | XmlSchemaFacet.Facet.fractionDigits;

		private bool isFixed;

		private string val;

		internal virtual XmlSchemaFacet.Facet ThisFacet
		{
			get
			{
				return XmlSchemaFacet.Facet.None;
			}
		}

		/// <summary>Gets or sets the value attribute of the facet.</summary>
		/// <returns>The value attribute.</returns>
		[XmlAttribute("value")]
		public string Value
		{
			get
			{
				return this.val;
			}
			set
			{
				this.val = value;
			}
		}

		/// <summary>Gets or sets information that indicates that this facet is fixed.</summary>
		/// <returns>If true, value is fixed; otherwise, false. The default is false.Optional.</returns>
		[DefaultValue(false)]
		[XmlAttribute("fixed")]
		public virtual bool IsFixed
		{
			get
			{
				return this.isFixed;
			}
			set
			{
				this.isFixed = value;
			}
		}

		[Flags]
		protected internal enum Facet
		{
			None = 0,
			length = 1,
			minLength = 2,
			maxLength = 4,
			pattern = 8,
			enumeration = 16,
			whiteSpace = 32,
			maxInclusive = 64,
			maxExclusive = 128,
			minExclusive = 256,
			minInclusive = 512,
			totalDigits = 1024,
			fractionDigits = 2048
		}
	}
}
