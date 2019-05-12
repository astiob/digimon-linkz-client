using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	/// <summary>Allows security actions for <see cref="T:System.Security.Permissions.PrincipalPermission" /> to be applied to code using declarative security. This class cannot be inherited.</summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class PrincipalPermissionAttribute : CodeAccessSecurityAttribute
	{
		private bool authenticated;

		private string name;

		private string role;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.PrincipalPermissionAttribute" /> class with the specified <see cref="T:System.Security.Permissions.SecurityAction" />.</summary>
		/// <param name="action">One of the <see cref="T:System.Security.Permissions.SecurityAction" /> values. </param>
		public PrincipalPermissionAttribute(SecurityAction action) : base(action)
		{
			this.authenticated = true;
		}

		/// <summary>Gets or sets a value indicating whether the current principal has been authenticated by the underlying role-based security provider.</summary>
		/// <returns>true if the current principal has been authenticated; otherwise, false.</returns>
		public bool Authenticated
		{
			get
			{
				return this.authenticated;
			}
			set
			{
				this.authenticated = value;
			}
		}

		/// <summary>Gets or sets the name of the identity associated with the current principal.</summary>
		/// <returns>A name to match against that provided by the underlying role-based security provider.</returns>
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		/// <summary>Gets or sets membership in a specified security role.</summary>
		/// <returns>The name of a role from the underlying role-based security provider.</returns>
		public string Role
		{
			get
			{
				return this.role;
			}
			set
			{
				this.role = value;
			}
		}

		/// <summary>Creates and returns a new <see cref="T:System.Security.Permissions.PrincipalPermission" />.</summary>
		/// <returns>A <see cref="T:System.Security.Permissions.PrincipalPermission" /> that corresponds to this attribute.</returns>
		public override IPermission CreatePermission()
		{
			PrincipalPermission result;
			if (base.Unrestricted)
			{
				result = new PrincipalPermission(PermissionState.Unrestricted);
			}
			else
			{
				result = new PrincipalPermission(this.name, this.role, this.authenticated);
			}
			return result;
		}
	}
}
