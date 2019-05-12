using System;

namespace System.Runtime.InteropServices
{
	/// <summary>This attribute has been deprecated.  </summary>
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(true)]
	[Obsolete]
	public sealed class SetWin32ContextInIDispatchAttribute : Attribute
	{
	}
}
