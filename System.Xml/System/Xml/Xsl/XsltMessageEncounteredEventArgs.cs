using System;

namespace System.Xml.Xsl
{
	/// <summary>Provides data for the <see cref="E:System.Xml.Xsl.XsltArgumentList.XsltMessageEncountered" /> event. </summary>
	public abstract class XsltMessageEncounteredEventArgs : EventArgs
	{
		/// <summary>Gets the contents of the xsl:message element.</summary>
		/// <returns>The contents of the xsl:message element.</returns>
		public abstract string Message { get; }
	}
}
