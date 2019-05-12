using System;
using System.Runtime.InteropServices;

namespace System.Security.AccessControl
{
	/// <summary>Specifies the access control rights that can be applied to named system semaphore objects.</summary>
	[Flags]
	[ComVisible(false)]
	public enum SemaphoreRights
	{
		/// <summary>The right to release a named semaphore.</summary>
		Modify = 2,
		/// <summary>The right to delete a named semaphore.</summary>
		Delete = 65536,
		/// <summary>The right to open and copy the access rules and audit rules for a named semaphore.</summary>
		ReadPermissions = 131072,
		/// <summary>The right to change the security and audit rules associated with a named semaphore.</summary>
		ChangePermissions = 262144,
		/// <summary>The right to change the owner of a named semaphore.</summary>
		TakeOwnership = 524288,
		/// <summary>The right to wait on a named semaphore.</summary>
		Synchronize = 1048576,
		/// <summary>The right to exert full control over a named semaphore, and to modify its access rules and audit rules.</summary>
		FullControl = 2031619
	}
}
