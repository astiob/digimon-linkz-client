using System;

namespace System.Xml.Serialization
{
	internal class XmlTypeMapMemberList : XmlTypeMapMemberElement
	{
		public XmlTypeMapping ListTypeMapping
		{
			get
			{
				return ((XmlTypeMapElementInfo)base.ElementInfo[0]).MappedType;
			}
		}

		public string ElementName
		{
			get
			{
				return ((XmlTypeMapElementInfo)base.ElementInfo[0]).ElementName;
			}
		}

		public string Namespace
		{
			get
			{
				return ((XmlTypeMapElementInfo)base.ElementInfo[0]).Namespace;
			}
		}
	}
}
