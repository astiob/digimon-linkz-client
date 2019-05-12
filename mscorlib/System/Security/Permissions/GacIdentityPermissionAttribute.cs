using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	/// <summary>Allows security actions for <see cref="T:System.Security.Permissions.GacIdentityPermission" /> to be applied to code using declarative security. This class cannot be inherited.</summary>
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class GacIdentityPermissionAttribute : CodeAccessSecurityAttribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.GacIdentityPermissionAttribute" /> class with the specified <see cref="T:System.Security.Permissions.SecurityAction" /> value.</summary>
		/// <param name="action">One of the <see cref="T:System.Security.Permissions.SecurityAction" /> values. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="action" /> parameter is not a valid <see cref="T:System.Security.Permissions.SecurityAction" /> value. </exception>
		public GacIdentityPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		/// <summary>Creates a new <see cref="T:System.Security.Permissions.GacIdentityPermission" /> object.</summary>
		/// <returns>A <see cref="T:System.Security.Permissions.GacIdentityPermission" /> that corresponds to this attribute.</returns>
		public override IPermission CreatePermission()
		{
			return new GacIdentityPermission();
		}
	}
}
