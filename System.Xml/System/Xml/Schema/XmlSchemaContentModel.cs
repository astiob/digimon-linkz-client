using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Specifies the order and structure of the child elements of a type.</summary>
	public abstract class XmlSchemaContentModel : XmlSchemaAnnotated
	{
		/// <summary>Gets or sets the content of the type.</summary>
		/// <returns>Provides the content of the type.</returns>
		[XmlIgnore]
		public abstract XmlSchemaContent Content { get; set; }
	}
}
