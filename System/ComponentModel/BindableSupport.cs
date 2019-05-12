using System;

namespace System.ComponentModel
{
	/// <summary>Specifies values to indicate whether a property can be bound to a data element or another property.</summary>
	public enum BindableSupport
	{
		/// <summary>The property is not bindable at design time.</summary>
		No,
		/// <summary>The property is bindable at design time.</summary>
		Yes,
		/// <summary>The property is set to the default.</summary>
		Default
	}
}
