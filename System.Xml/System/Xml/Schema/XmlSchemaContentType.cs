using System;

namespace System.Xml.Schema
{
	/// <summary>Enumerations for the content model of the complex type. This represents the content in the post-schema-validation information set (infoset).</summary>
	public enum XmlSchemaContentType
	{
		/// <summary>Text-only content.</summary>
		TextOnly,
		/// <summary>Empty content.</summary>
		Empty,
		/// <summary>Element-only content.</summary>
		ElementOnly,
		/// <summary>Mixed content.</summary>
		Mixed
	}
}
