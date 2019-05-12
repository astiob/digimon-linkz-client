using System;
using System.Runtime.InteropServices;

namespace System.Security
{
	/// <summary>Allows strong-named assemblies to be called by partially trusted code. Without this declaration, only fully trusted callers are able to use such assemblies. This class cannot be inherited.</summary>
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
	[ComVisible(true)]
	public sealed class AllowPartiallyTrustedCallersAttribute : Attribute
	{
	}
}
