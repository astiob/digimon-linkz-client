using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	/// <summary>Allows security actions for <see cref="T:System.Security.Permissions.IsolatedStorageFilePermission" /> to be applied to code using declarative security. This class cannot be inherited.</summary>
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class IsolatedStorageFilePermissionAttribute : IsolatedStoragePermissionAttribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.IsolatedStorageFilePermissionAttribute" /> class with the specified <see cref="T:System.Security.Permissions.SecurityAction" />.</summary>
		/// <param name="action">One of the <see cref="T:System.Security.Permissions.SecurityAction" /> values. </param>
		public IsolatedStorageFilePermissionAttribute(SecurityAction action) : base(action)
		{
		}

		/// <summary>Creates and returns a new <see cref="T:System.Security.Permissions.IsolatedStorageFilePermission" />.</summary>
		/// <returns>An <see cref="T:System.Security.Permissions.IsolatedStorageFilePermission" /> that corresponds to this attribute.</returns>
		public override IPermission CreatePermission()
		{
			IsolatedStorageFilePermission isolatedStorageFilePermission;
			if (base.Unrestricted)
			{
				isolatedStorageFilePermission = new IsolatedStorageFilePermission(PermissionState.Unrestricted);
			}
			else
			{
				isolatedStorageFilePermission = new IsolatedStorageFilePermission(PermissionState.None);
				isolatedStorageFilePermission.UsageAllowed = base.UsageAllowed;
				isolatedStorageFilePermission.UserQuota = base.UserQuota;
			}
			return isolatedStorageFilePermission;
		}
	}
}
