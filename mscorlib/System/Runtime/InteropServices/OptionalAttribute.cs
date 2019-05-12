using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Indicates that a parameter is optional.</summary>
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	[ComVisible(true)]
	public sealed class OptionalAttribute : Attribute
	{
	}
}
