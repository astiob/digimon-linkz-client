using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Provides information about the validation mode of any and anyAttribute element replacements.</summary>
	public enum XmlSchemaContentProcessing
	{
		/// <summary>Document items are not validated.</summary>
		[XmlIgnore]
		None,
		/// <summary>Document items must consist of well-formed XML and are not validated by the schema.</summary>
		[XmlEnum("skip")]
		Skip,
		/// <summary>If the associated schema is found, the document items will be validated. No errors will be thrown otherwise.</summary>
		[XmlEnum("lax")]
		Lax,
		/// <summary>The schema processor must find a schema associated with the indicated namespace to validate the document items.</summary>
		[XmlEnum("strict")]
		Strict
	}
}
