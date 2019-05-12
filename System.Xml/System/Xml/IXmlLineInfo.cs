using System;

namespace System.Xml
{
	/// <summary>Provides an interface to enable a class to return line and position information.</summary>
	public interface IXmlLineInfo
	{
		/// <summary>Gets the current line number.</summary>
		/// <returns>The current line number or 0 if no line information is available (for example, <see cref="M:System.Xml.IXmlLineInfo.HasLineInfo" /> returns false).</returns>
		int LineNumber { get; }

		/// <summary>Gets the current line position.</summary>
		/// <returns>The current line position or 0 if no line information is available (for example, <see cref="M:System.Xml.IXmlLineInfo.HasLineInfo" /> returns false).</returns>
		int LinePosition { get; }

		/// <summary>Gets a value indicating whether the class can return line information.</summary>
		/// <returns>true if <see cref="P:System.Xml.IXmlLineInfo.LineNumber" /> and <see cref="P:System.Xml.IXmlLineInfo.LinePosition" /> can be provided; otherwise, false.</returns>
		bool HasLineInfo();
	}
}
