using System;

namespace System.Xml.Serialization
{
	/// <summary>Instructs the <see cref="T:System.Xml.Serialization.XmlSerializer" /> not to serialize the public field or public read/write property value.</summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
	public class SoapIgnoreAttribute : Attribute
	{
	}
}
