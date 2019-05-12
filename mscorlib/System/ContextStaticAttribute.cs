using System;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Indicates that the value of a static field is unique for a particular context.</summary>
	/// <filterpriority>1</filterpriority>
	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public class ContextStaticAttribute : Attribute
	{
	}
}
