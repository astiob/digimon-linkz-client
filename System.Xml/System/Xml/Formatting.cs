using System;

namespace System.Xml
{
	/// <summary>Specifies formatting options for the <see cref="T:System.Xml.XmlTextWriter" />.</summary>
	public enum Formatting
	{
		/// <summary>No special formatting is applied. This is the default.</summary>
		None,
		/// <summary>Causes child elements to be indented according to the <see cref="P:System.Xml.XmlTextWriter.Indentation" /> and <see cref="P:System.Xml.XmlTextWriter.IndentChar" /> settings. </summary>
		Indented
	}
}
