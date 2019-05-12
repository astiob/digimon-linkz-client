using System;

namespace System.Xml
{
	/// <summary>Describes the document order of a node compared to a second node.</summary>
	public enum XmlNodeOrder
	{
		/// <summary>The current node of this navigator is before the current node of the supplied navigator.</summary>
		Before,
		/// <summary>The current node of this navigator is after the current node of the supplied navigator.</summary>
		After,
		/// <summary>The two navigators are positioned on the same node.</summary>
		Same,
		/// <summary>The node positions cannot be determined in document order, relative to each other. This could occur if the two nodes reside in different trees.</summary>
		Unknown
	}
}
