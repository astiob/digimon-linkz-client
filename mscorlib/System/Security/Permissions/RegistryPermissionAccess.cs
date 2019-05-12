using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	/// <summary>Specifies the permitted access to registry keys and values.</summary>
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum RegistryPermissionAccess
	{
		/// <summary>No access to registry variables. <see cref="F:System.Security.Permissions.RegistryPermissionAccess.NoAccess" /> represents no valid <see cref="T:System.Security.Permissions.RegistryPermissionAccess" /> values and causes an <see cref="T:System.ArgumentException" /> when used as the parameter for <see cref="M:System.Security.Permissions.RegistryPermission.GetPathList(System.Security.Permissions.RegistryPermissionAccess)" />, which expects a single value.</summary>
		NoAccess = 0,
		/// <summary>Read access to registry variables.</summary>
		Read = 1,
		/// <summary>Write access to registry variables.</summary>
		Write = 2,
		/// <summary>Create access to registry variables.</summary>
		Create = 4,
		/// <summary>
		///   <see cref="F:System.Security.Permissions.RegistryPermissionAccess.Create" />, <see cref="F:System.Security.Permissions.RegistryPermissionAccess.Read" />, and <see cref="F:System.Security.Permissions.RegistryPermissionAccess.Write" /> access to registry variables. <see cref="F:System.Security.Permissions.RegistryPermissionAccess.AllAccess" /> represents multiple <see cref="T:System.Security.Permissions.RegistryPermissionAccess" /> values and causes an <see cref="T:System.ArgumentException" /> when used as the <paramref name="access" /> parameter for the <see cref="M:System.Security.Permissions.RegistryPermission.GetPathList(System.Security.Permissions.RegistryPermissionAccess)" /> method, which expects a single value.</summary>
		AllAccess = 7
	}
}
