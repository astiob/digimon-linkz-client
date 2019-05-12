using System;

namespace System.Xml.Serialization
{
	[XmlType("configuration")]
	internal class SerializationCodeGeneratorConfiguration
	{
		[XmlElement("serializer")]
		public SerializerInfo[] Serializers;
	}
}
