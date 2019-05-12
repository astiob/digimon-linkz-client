using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Prevent name mangling of constructors, methods, fields and properties.</para>
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface)]
	[NotConverted]
	[Obsolete("NotRenamedAttribute was used for the Flash buildtarget, which is not supported anymore starting Unity 5.0", true)]
	public sealed class NotRenamedAttribute : Attribute
	{
	}
}
