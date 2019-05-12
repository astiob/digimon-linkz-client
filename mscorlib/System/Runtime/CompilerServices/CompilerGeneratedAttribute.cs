using System;

namespace System.Runtime.CompilerServices
{
	/// <summary>Distinguishes a compiler-generated element from a user-generated element. This class cannot be inherited.</summary>
	[AttributeUsage(AttributeTargets.All)]
	[Serializable]
	public sealed class CompilerGeneratedAttribute : Attribute
	{
	}
}
