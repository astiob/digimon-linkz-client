using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	/// <summary>Hides the code from the debugger. This class cannot be inherited.</summary>
	/// <filterpriority>1</filterpriority>
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class DebuggerHiddenAttribute : Attribute
	{
	}
}
