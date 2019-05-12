using System;

namespace System.Xml.Serialization
{
	/// <summary>Represents the method that handles the <see cref="E:System.Xml.Serialization.XmlSerializer.UnknownAttribute" /></summary>
	/// <param name="sender">The source of the event. </param>
	/// <param name="e">An <see cref="T:System.Xml.Serialization.XmlAttributeEventArgs" /> that contains the event data. </param>
	public delegate void XmlAttributeEventHandler(object sender, XmlAttributeEventArgs e);
}
