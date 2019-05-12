using System;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Indicates that a class can be serialized. This class cannot be inherited.</summary>
	/// <filterpriority>1</filterpriority>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate, Inherited = false, AllowMultiple = false)]
	[ComVisible(true)]
	public sealed class SerializableAttribute : Attribute
	{
	}
}
