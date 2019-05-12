using System;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Indicates that the method will allow a variable number of arguments in its invocation. This class cannot be inherited.</summary>
	/// <filterpriority>1</filterpriority>
	[AttributeUsage(AttributeTargets.Parameter)]
	[ComVisible(true)]
	public sealed class ParamArrayAttribute : Attribute
	{
	}
}
