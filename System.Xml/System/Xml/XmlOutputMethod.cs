using System;

namespace System.Xml
{
	/// <summary>Specifies the method used to serialize the <see cref="T:System.Xml.XmlWriter" /> output. </summary>
	public enum XmlOutputMethod
	{
		/// <summary>Serialize according to the XML 1.0 rules.</summary>
		Xml,
		/// <summary>Serialize according to the HTML rules specified by XSLT.</summary>
		Html,
		/// <summary>Serialize text blocks only.</summary>
		Text,
		/// <summary>Use the XSLT rules to choose between the <see cref="F:System.Xml.XmlOutputMethod.Xml" /> and <see cref="F:System.Xml.XmlOutputMethod.Html" /> output methods at runtime.</summary>
		AutoDetect
	}
}
