using System;

namespace System.Xml.Serialization
{
	[XmlType("select")]
	internal class Select
	{
		[XmlElement("typeName")]
		public string TypeName;

		[XmlElement("typeAttribute")]
		public string[] TypeAttributes;

		[XmlElement("typeMember")]
		public string TypeMember;
	}
}
