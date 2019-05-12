using System;

namespace System.Xml.Schema
{
	/// <summary>Specifies schema validation options used by the <see cref="T:System.Xml.Schema.XmlSchemaValidator" /> and <see cref="T:System.Xml.XmlReader" /> classes.</summary>
	[Flags]
	public enum XmlSchemaValidationFlags
	{
		/// <summary>Do not process identity constraints, inline schemas, schema location hints, or report schema validation warnings.</summary>
		None = 0,
		/// <summary>Process inline schemas encountered during validation.</summary>
		ProcessInlineSchema = 1,
		/// <summary>Process schema location hints (xsi:schemaLocation, xsi:noNamespaceSchemaLocation) encountered during validation.</summary>
		ProcessSchemaLocation = 2,
		/// <summary>Report schema validation warnings encountered during validation.</summary>
		ReportValidationWarnings = 4,
		/// <summary>Process identity constraints (xs:ID, xs:IDREF, xs:key, xs:keyref, xs:unique) encountered during validation.</summary>
		ProcessIdentityConstraints = 8,
		/// <summary>Allow xml:* attributes even if they are not defined in the schema. The attributes will be validated based on their data type.</summary>
		[Obsolete("It is really idiotic idea to include such validation option that breaks W3C XML Schema specification compliance and interoperability.")]
		AllowXmlAttributes = 16
	}
}
