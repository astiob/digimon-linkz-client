using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	/// <summary>Allows security actions for <see cref="T:System.Security.Permissions.SiteIdentityPermission" /> to be applied to code using declarative security. This class cannot be inherited.</summary>
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class SiteIdentityPermissionAttribute : CodeAccessSecurityAttribute
	{
		private string site;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.SiteIdentityPermissionAttribute" /> class with the specified <see cref="T:System.Security.Permissions.SecurityAction" />.</summary>
		/// <param name="action">One of the <see cref="T:System.Security.Permissions.SecurityAction" /> values. </param>
		public SiteIdentityPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		/// <summary>Gets or sets the site name of the calling code.</summary>
		/// <returns>The site name to compare against the site name specified by the security provider.</returns>
		public string Site
		{
			get
			{
				return this.site;
			}
			set
			{
				this.site = value;
			}
		}

		/// <summary>Creates and returns a new instance of <see cref="T:System.Security.Permissions.SiteIdentityPermission" />.</summary>
		/// <returns>A <see cref="T:System.Security.Permissions.SiteIdentityPermission" /> that corresponds to this attribute.</returns>
		public override IPermission CreatePermission()
		{
			SiteIdentityPermission result;
			if (base.Unrestricted)
			{
				result = new SiteIdentityPermission(PermissionState.Unrestricted);
			}
			else if (this.site == null)
			{
				result = new SiteIdentityPermission(PermissionState.None);
			}
			else
			{
				result = new SiteIdentityPermission(this.site);
			}
			return result;
		}
	}
}
