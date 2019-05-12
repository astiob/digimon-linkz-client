using System;

namespace System.Xml.Serialization
{
	/// <summary>Delegate used by the <see cref="T:System.Xml.Serialization.XmlSerializer" /> class for deserialization of SOAP-encoded XML data. </summary>
	/// <param name="fixup"></param>
	public delegate void XmlSerializationFixupCallback(object fixup);
}
