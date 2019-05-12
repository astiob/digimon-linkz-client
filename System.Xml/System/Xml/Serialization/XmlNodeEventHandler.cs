using System;

namespace System.Xml.Serialization
{
	/// <summary>Represents the method that handles the <see cref="E:System.Xml.Serialization.XmlSerializer.UnknownNode" /> event of an <see cref="T:System.Xml.Serialization.XmlSerializer" />.</summary>
	/// <param name="sender">The source of the event. </param>
	/// <param name="e">An <see cref="T:System.Xml.Serialization.XmlNodeEventArgs" /> that contains the event data. </param>
	public delegate void XmlNodeEventHandler(object sender, XmlNodeEventArgs e);
}
