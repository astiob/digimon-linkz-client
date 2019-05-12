using System;

namespace System.Xml
{
	/// <summary>Enables a class to return an <see cref="T:System.Xml.XmlNode" /> from the current context or position.</summary>
	public interface IHasXmlNode
	{
		/// <summary>Returns the <see cref="T:System.Xml.XmlNode" /> for the current position.</summary>
		/// <returns>The XmlNode for the current position.</returns>
		XmlNode GetNode();
	}
}
