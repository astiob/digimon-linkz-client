using System;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Indicates that an enumeration can be treated as a bit field; that is, a set of flags.</summary>
	/// <filterpriority>1</filterpriority>
	[AttributeUsage(AttributeTargets.Enum, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public class FlagsAttribute : Attribute
	{
	}
}
