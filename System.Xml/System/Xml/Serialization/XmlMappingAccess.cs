using System;

namespace System.Xml.Serialization
{
	/// <summary>Specifies whether a mapping is read, write, or both.</summary>
	[Flags]
	public enum XmlMappingAccess
	{
		/// <summary>Both read and write methods are generated.</summary>
		None = 0,
		/// <summary>Read methods are generated.</summary>
		Read = 1,
		/// <summary>Write methods are generated.</summary>
		Write = 2
	}
}
