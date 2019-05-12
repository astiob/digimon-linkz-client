using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Indicator of how the attribute is used.</summary>
	public enum XmlSchemaUse
	{
		/// <summary>Attribute use not specified.</summary>
		[XmlIgnore]
		None,
		/// <summary>Attribute is optional.</summary>
		[XmlEnum("optional")]
		Optional,
		/// <summary>Attribute cannot be used.</summary>
		[XmlEnum("prohibited")]
		Prohibited,
		/// <summary>Attribute must appear once.</summary>
		[XmlEnum("required")]
		Required
	}
}
