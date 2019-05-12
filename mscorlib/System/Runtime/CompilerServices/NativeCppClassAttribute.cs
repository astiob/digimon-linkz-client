using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	/// <summary>Applies metadata to an assembly that indicates that a type is an unmanaged type.  This class cannot be inherited.</summary>
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Struct, Inherited = true)]
	[Serializable]
	public sealed class NativeCppClassAttribute : Attribute
	{
	}
}
