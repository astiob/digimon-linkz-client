using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	internal class XmlSchemaSerializer : XmlSerializer
	{
		protected override void Serialize(object o, XmlSerializationWriter writer)
		{
			XmlSchemaSerializationWriter xmlSchemaSerializationWriter = writer as XmlSchemaSerializationWriter;
			xmlSchemaSerializationWriter.WriteRoot_XmlSchema((XmlSchema)o);
		}

		protected override XmlSerializationWriter CreateWriter()
		{
			return new XmlSchemaSerializationWriter();
		}
	}
}
