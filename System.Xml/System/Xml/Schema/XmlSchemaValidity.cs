using System;

namespace System.Xml.Schema
{
	/// <summary>Represents the validity of an XML item validated by the <see cref="T:System.Xml.Schema.XmlSchemaValidator" /> class.</summary>
	public enum XmlSchemaValidity
	{
		/// <summary>The validity of the XML item is not known.</summary>
		NotKnown,
		/// <summary>The XML item is valid.</summary>
		Valid,
		/// <summary>The XML item is invalid.</summary>
		Invalid
	}
}
