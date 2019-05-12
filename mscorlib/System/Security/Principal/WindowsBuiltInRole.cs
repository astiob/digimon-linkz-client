using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	/// <summary>Specifies common roles to be used with <see cref="M:System.Security.Principal.WindowsPrincipal.IsInRole(System.String)" />.</summary>
	[ComVisible(true)]
	[Serializable]
	public enum WindowsBuiltInRole
	{
		/// <summary>Administrators have complete and unrestricted access to the computer or domain.</summary>
		Administrator = 544,
		/// <summary>Users are prevented from making accidental or intentional system-wide changes. Thus, users can run certified applications, but not most legacy applications.</summary>
		User,
		/// <summary>Guests are more restricted than users.</summary>
		Guest,
		/// <summary>Power users possess most administrative permissions with some restrictions. Thus, power users can run legacy applications, in addition to certified applications.</summary>
		PowerUser,
		/// <summary>Account operators manage the user accounts on a computer or domain.</summary>
		AccountOperator,
		/// <summary>System operators manage a particular computer.</summary>
		SystemOperator,
		/// <summary>Print operators can take control of a printer.</summary>
		PrintOperator,
		/// <summary>Backup operators can override security restrictions for the sole purpose of backing up or restoring files.</summary>
		BackupOperator,
		/// <summary>Replicators support file replication in a domain.</summary>
		Replicator
	}
}
