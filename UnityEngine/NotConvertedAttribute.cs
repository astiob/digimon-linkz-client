using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Instructs the build pipeline not to convert a type or member to the target platform.</para>
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	public sealed class NotConvertedAttribute : Attribute
	{
	}
}
