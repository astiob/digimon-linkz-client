using System;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Indicates that a field of a serializable class should not be serialized. This class cannot be inherited.</summary>
	/// <filterpriority>1</filterpriority>
	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	[ComVisible(true)]
	public sealed class NonSerializedAttribute : Attribute
	{
	}
}
