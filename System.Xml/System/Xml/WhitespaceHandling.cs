using System;

namespace System.Xml
{
	/// <summary>Specifies how white space is handled.</summary>
	public enum WhitespaceHandling
	{
		/// <summary>Return Whitespace and SignificantWhitespace nodes. This is the default.</summary>
		All,
		/// <summary>Return SignificantWhitespace nodes only.</summary>
		Significant,
		/// <summary>Return no Whitespace and no SignificantWhitespace nodes.</summary>
		None
	}
}
