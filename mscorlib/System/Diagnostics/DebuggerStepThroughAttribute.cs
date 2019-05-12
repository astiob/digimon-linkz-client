using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	/// <summary>Instructs the debugger to step through the code instead of stepping into the code. This class cannot be inherited.</summary>
	/// <filterpriority>1</filterpriority>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class DebuggerStepThroughAttribute : Attribute
	{
	}
}
