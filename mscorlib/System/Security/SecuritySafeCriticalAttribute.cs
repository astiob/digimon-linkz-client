using System;

namespace System.Security
{
	/// <summary>Identifies types or members as security-critical and safely accessible by transparent code.</summary>
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
	[MonoTODO("Only supported by the runtime when CoreCLR is enabled")]
	public sealed class SecuritySafeCriticalAttribute : Attribute
	{
	}
}
