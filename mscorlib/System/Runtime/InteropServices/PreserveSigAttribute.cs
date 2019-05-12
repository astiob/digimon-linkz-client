using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Indicates that the HRESULT or retval signature transformation that takes place during COM interop calls should be suppressed.</summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	[ComVisible(true)]
	public sealed class PreserveSigAttribute : Attribute
	{
	}
}
