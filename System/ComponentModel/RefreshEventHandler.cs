using System;

namespace System.ComponentModel
{
	/// <summary>Represents the method that handles the <see cref="E:System.ComponentModel.TypeDescriptor.Refreshed" /> event raised when a <see cref="T:System.Type" /> or component is changed during design time.</summary>
	/// <param name="e">A <see cref="T:System.ComponentModel.RefreshEventArgs" /> that contains the component or <see cref="T:System.Type" /> that changed. </param>
	public delegate void RefreshEventHandler(RefreshEventArgs e);
}
