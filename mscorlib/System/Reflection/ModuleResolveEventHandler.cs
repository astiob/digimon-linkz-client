using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>Represents the method that will handle the <see cref="E:System.Reflection.Assembly.ModuleResolve" /> event of an <see cref="T:System.Reflection.Assembly" />.</summary>
	/// <returns>The module that resolves the request.</returns>
	/// <param name="sender">The assembly that was the source of the event. </param>
	/// <param name="e">The arguments supplied by the object describing the event. </param>
	[ComVisible(true)]
	[Serializable]
	public delegate Module ModuleResolveEventHandler(object sender, ResolveEventArgs e);
}
