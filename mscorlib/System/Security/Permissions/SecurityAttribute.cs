using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	/// <summary>Specifies the base attribute class for declarative security from which <see cref="T:System.Security.Permissions.CodeAccessSecurityAttribute" /> is derived.</summary>
	[ComVisible(true)]
	[Obsolete("CAS support is not available with Silverlight applications.")]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public abstract class SecurityAttribute : Attribute
	{
		private SecurityAction m_Action;

		private bool m_Unrestricted;

		/// <summary>Initializes a new instance of <see cref="T:System.Security.Permissions.SecurityAttribute" /> with the specified <see cref="T:System.Security.Permissions.SecurityAction" />.</summary>
		/// <param name="action">One of the <see cref="T:System.Security.Permissions.SecurityAction" /> values. </param>
		protected SecurityAttribute(SecurityAction action)
		{
			this.Action = action;
		}

		/// <summary>When overridden in a derived class, creates a permission object that can then be serialized into binary form and persistently stored along with the <see cref="T:System.Security.Permissions.SecurityAction" /> in an assembly's metadata.</summary>
		/// <returns>A serializable permission object.</returns>
		public abstract IPermission CreatePermission();

		/// <summary>Gets or sets a value indicating whether full (unrestricted) permission to the resource protected by the attribute is declared.</summary>
		/// <returns>true if full permission to the protected resource is declared; otherwise, false.</returns>
		public bool Unrestricted
		{
			get
			{
				return this.m_Unrestricted;
			}
			set
			{
				this.m_Unrestricted = value;
			}
		}

		/// <summary>Gets or sets a security action.</summary>
		/// <returns>One of the <see cref="T:System.Security.Permissions.SecurityAction" /> values.</returns>
		public SecurityAction Action
		{
			get
			{
				return this.m_Action;
			}
			set
			{
				this.m_Action = value;
			}
		}
	}
}
