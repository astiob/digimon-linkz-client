using System;

namespace System.Xml.Serialization
{
	/// <summary>Specifies various options to use when generating .NET Framework types for use with an XML Web Service.</summary>
	[Flags]
	public enum CodeGenerationOptions
	{
		/// <summary>Represents primitive types by fields and primitive types by <see cref="N:System" /> namespace types.</summary>
		[XmlIgnore]
		None = 0,
		/// <summary>Represents primitive types by properties.</summary>
		[XmlEnum("properties")]
		GenerateProperties = 1,
		/// <summary>Creates events for the asynchronous invocation of Web methods.</summary>
		[XmlEnum("newAsync")]
		GenerateNewAsync = 2,
		/// <summary>Creates Begin and End methods for the asynchronous invocation of Web methods.</summary>
		[XmlEnum("oldAsync")]
		GenerateOldAsync = 4,
		/// <summary>Generates explicitly ordered serialization code as specified through the Order property of the <see cref="T:System.Xml.Serialization.XmlAnyElementAttribute" />, <see cref="T:System.Xml.Serialization.XmlArrayAttribute" />, and <see cref="T:System.Xml.Serialization.XmlElementAttribute" /> attributes. </summary>
		[XmlEnum("order")]
		GenerateOrder = 8,
		/// <summary>Enables data binding.</summary>
		[XmlEnum("enableDataBinding")]
		EnableDataBinding = 16
	}
}
