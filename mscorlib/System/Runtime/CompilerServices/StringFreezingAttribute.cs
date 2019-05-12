using System;

namespace System.Runtime.CompilerServices
{
	/// <summary>Deprecated. Freezes a string literal when creating native images using the Native Image Generator (Ngen.exe). This class cannot be inherited.</summary>
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[Serializable]
	public sealed class StringFreezingAttribute : Attribute
	{
	}
}
