using System;

namespace System.Xml
{
	/// <summary>Specifies the type of validation to perform.</summary>
	public enum ValidationType
	{
		/// <summary>No validation is performed. This setting creates an XML 1.0 compliant non-validating parser.</summary>
		None,
		/// <summary>Validates if DTD or schema information is found.</summary>
		[Obsolete]
		Auto,
		/// <summary>Validates according to the DTD.</summary>
		DTD,
		/// <summary>Validate according to XML-Data Reduced (XDR) schemas, including inline XDR schemas. XDR schemas are recognized using the x-schema namespace prefix or the <see cref="P:System.Xml.XmlValidatingReader.Schemas" /> property.</summary>
		[Obsolete]
		XDR,
		/// <summary>Validate according to XML Schema definition language (XSD) schemas, including inline XML Schemas. XML Schemas are associated with namespace URIs either by using the schemaLocation attribute or the provided Schemas property.</summary>
		Schema
	}
}
