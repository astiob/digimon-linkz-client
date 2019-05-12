using System;

namespace System.Security
{
	/// <summary>Identifies which of the nonpublic <see cref="T:System.Security.SecurityCriticalAttribute" /> members are accessible by transparent code within the assembly. </summary>
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
	[MonoTODO("Not supported by the runtime")]
	public sealed class SecurityTreatAsSafeAttribute : Attribute
	{
	}
}
