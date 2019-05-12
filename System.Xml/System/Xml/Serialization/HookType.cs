using System;

namespace System.Xml.Serialization
{
	[XmlType("hookType")]
	internal enum HookType
	{
		attributes,
		elements,
		unknownAttribute,
		unknownElement,
		member,
		type
	}
}
