using System;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	/// <summary>Defines the method that creates a new identity permission.</summary>
	[ComVisible(true)]
	public interface IIdentityPermissionFactory
	{
		/// <summary>Creates a new identity permission for the specified evidence.</summary>
		/// <returns>The new identity permission.</returns>
		/// <param name="evidence">The evidence from which to create the new identity permission. </param>
		IPermission CreateIdentityPermission(Evidence evidence);
	}
}
