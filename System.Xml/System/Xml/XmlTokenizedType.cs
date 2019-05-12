using System;

namespace System.Xml
{
	/// <summary>Represents the XML type for the string. This allows the string to be read as a particular XML type, for example a CDATA section type.</summary>
	public enum XmlTokenizedType
	{
		/// <summary>CDATA type.</summary>
		CDATA,
		/// <summary>ID type.</summary>
		ID,
		/// <summary>IDREF type.</summary>
		IDREF,
		/// <summary>IDREFS type.</summary>
		IDREFS,
		/// <summary>ENTITY type.</summary>
		ENTITY,
		/// <summary>ENTITIES type.</summary>
		ENTITIES,
		/// <summary>NMTOKEN type.</summary>
		NMTOKEN,
		/// <summary>NMTOKENS type.</summary>
		NMTOKENS,
		/// <summary>NOTATION type.</summary>
		NOTATION,
		/// <summary>ENUMERATION type.</summary>
		ENUMERATION,
		/// <summary>QName type.</summary>
		QName,
		/// <summary>NCName type.</summary>
		NCName,
		/// <summary>No type.</summary>
		None
	}
}
