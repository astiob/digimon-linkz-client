using System;

namespace System.Xml
{
	/// <summary>Specifies the type of node change.</summary>
	public enum XmlNodeChangedAction
	{
		/// <summary>A node is being inserted in the tree.</summary>
		Insert,
		/// <summary>A node is being removed from the tree.</summary>
		Remove,
		/// <summary>A node value is being changed.</summary>
		Change
	}
}
