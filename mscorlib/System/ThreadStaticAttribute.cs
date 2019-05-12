using System;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Indicates that the value of a static field is unique for each thread.</summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	[Serializable]
	public class ThreadStaticAttribute : Attribute
	{
	}
}
