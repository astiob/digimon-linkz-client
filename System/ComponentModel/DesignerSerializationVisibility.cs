using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel
{
	/// <summary>Specifies the visibility a property has to the design-time serializer.</summary>
	[ComVisible(true)]
	public enum DesignerSerializationVisibility
	{
		/// <summary>The code generator does not produce code for the object.</summary>
		Hidden,
		/// <summary>The code generator produces code for the object.</summary>
		Visible,
		/// <summary>The code generator produces code for the contents of the object, rather than for the object itself.</summary>
		Content
	}
}
