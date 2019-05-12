using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Indicates that data should be marshaled from callee back to caller.</summary>
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	public sealed class OutAttribute : Attribute
	{
	}
}
