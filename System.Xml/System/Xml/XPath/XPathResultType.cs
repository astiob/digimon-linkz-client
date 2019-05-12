using System;

namespace System.Xml.XPath
{
	/// <summary>Specifies the return type of the XPath expression.</summary>
	public enum XPathResultType
	{
		/// <summary>A numeric value.</summary>
		Number,
		/// <summary>A <see cref="T:System.String" /> value.</summary>
		String,
		/// <summary>A <see cref="T:System.Boolean" />true or false value.</summary>
		Boolean,
		/// <summary>A node collection.</summary>
		NodeSet,
		/// <summary>A tree fragment.</summary>
		[MonoFIX("MS.NET: 1")]
		Navigator,
		/// <summary>Any of the XPath node types.</summary>
		Any,
		/// <summary>The expression does not evaluate to the correct XPath type.</summary>
		Error
	}
}
