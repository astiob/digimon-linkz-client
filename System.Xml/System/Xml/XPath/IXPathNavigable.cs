using System;

namespace System.Xml.XPath
{
	/// <summary>Provides an accessor to the <see cref="T:System.Xml.XPath.XPathNavigator" /> class.</summary>
	public interface IXPathNavigable
	{
		/// <summary>Returns a new <see cref="T:System.Xml.XPath.XPathNavigator" /> object. </summary>
		/// <returns>An <see cref="T:System.Xml.XPath.XPathNavigator" /> object.</returns>
		XPathNavigator CreateNavigator();
	}
}
