using System;

namespace System.Runtime.CompilerServices
{
	/// <summary>Indicates that a native enumeration is not qualified by the enumeration type name. This class cannot be inherited.</summary>
	[AttributeUsage(AttributeTargets.Enum)]
	[Serializable]
	public sealed class ScopelessEnumAttribute : Attribute
	{
	}
}
