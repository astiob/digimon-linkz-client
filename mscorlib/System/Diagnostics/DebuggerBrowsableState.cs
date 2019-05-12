using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	/// <summary>Provides display instructions for the debugger.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	public enum DebuggerBrowsableState
	{
		/// <summary>Never show the element.</summary>
		Never,
		/// <summary>Show the element as collapsed.</summary>
		Collapsed = 2,
		/// <summary>Do not display the root element; display the child elements if the element is a collection or array of items.</summary>
		RootHidden
	}
}
