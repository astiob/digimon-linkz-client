using System;

namespace System.Xml.Serialization
{
	/// <summary>Delegate that is used by the <see cref="T:System.Xml.Serialization.XmlSerializer" /> class for serialization of types from SOAP-encoded, non-root XML data. </summary>
	/// <param name="o"></param>
	public delegate void XmlSerializationWriteCallback(object o);
}
