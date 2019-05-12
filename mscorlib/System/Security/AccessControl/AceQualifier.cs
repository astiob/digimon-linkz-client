using System;

namespace System.Security.AccessControl
{
	/// <summary>Specifies the function of an access control entry (ACE).</summary>
	public enum AceQualifier
	{
		/// <summary>Allow access.</summary>
		AccessAllowed,
		/// <summary>Deny access.</summary>
		AccessDenied,
		/// <summary>Cause a system audit.</summary>
		SystemAudit,
		/// <summary>Cause a system alarm.</summary>
		SystemAlarm
	}
}
