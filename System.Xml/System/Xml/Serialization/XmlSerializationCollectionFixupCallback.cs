using System;

namespace System.Xml.Serialization
{
	/// <summary>Delegate used by the <see cref="T:System.Xml.Serialization.XmlSerializer" /> class for deserialization of SOAP-encoded XML data types that map to collections or enumerations. </summary>
	/// <param name="collection"></param>
	/// <param name="collectionItems"></param>
	public delegate void XmlSerializationCollectionFixupCallback(object collection, object collectionItems);
}
