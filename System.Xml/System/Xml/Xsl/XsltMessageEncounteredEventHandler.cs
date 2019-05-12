using System;

namespace System.Xml.Xsl
{
	/// <summary>Represents the method that will handle the <see cref="E:System.Xml.Xsl.XsltArgumentList.XsltMessageEncountered" /> event.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="T:System.Xml.Xsl.XsltMessageEncounteredEventArgs" /> containing the event data.</param>
	public delegate void XsltMessageEncounteredEventHandler(object sender, XsltMessageEncounteredEventArgs e);
}
