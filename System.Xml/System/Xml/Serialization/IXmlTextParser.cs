using System;

namespace System.Xml.Serialization
{
	/// <summary>Establishes a <see cref="P:System.Xml.Serialization.IXmlTextParser.Normalized" /> property for use by the .NET Framework infrastructure.</summary>
	public interface IXmlTextParser
	{
		/// <summary>Gets or sets whether white space and attribute values are normalized.</summary>
		/// <returns>true if white space attributes values are normalized; otherwise, false.</returns>
		bool Normalized { get; set; }

		/// <summary>Gets or sets how white space is handled when parsing XML.</summary>
		/// <returns>A member of the <see cref="T:System.Xml.WhitespaceHandling" /> enumeration that describes how whites pace is handled when parsing XML.</returns>
		WhitespaceHandling WhitespaceHandling { get; set; }
	}
}
