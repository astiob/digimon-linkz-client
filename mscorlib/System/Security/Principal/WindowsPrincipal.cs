using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	/// <summary>Allows code to check the Windows group membership of a Windows user.</summary>
	[ComVisible(true)]
	[Serializable]
	public class WindowsPrincipal : IPrincipal
	{
		private WindowsIdentity _identity;

		private string[] m_roles;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Principal.WindowsPrincipal" /> class by using the specified <see cref="T:System.Security.Principal.WindowsIdentity" /> object.</summary>
		/// <param name="ntIdentity">The <see cref="T:System.Security.Principal.WindowsIdentity" /> object from which to construct the new instance of <see cref="T:System.Security.Principal.WindowsPrincipal" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="ntIdentity" /> is null. </exception>
		public WindowsPrincipal(WindowsIdentity ntIdentity)
		{
			if (ntIdentity == null)
			{
				throw new ArgumentNullException("ntIdentity");
			}
			this._identity = ntIdentity;
		}

		/// <summary>Gets the identity of the current principal.</summary>
		/// <returns>The <see cref="T:System.Security.Principal.WindowsIdentity" /> object of the current principal.</returns>
		public virtual IIdentity Identity
		{
			get
			{
				return this._identity;
			}
		}

		/// <summary>Determines whether the current principal belongs to the Windows user group with the specified relative identifier (RID).</summary>
		/// <returns>true if the current principal is a member of the specified Windows user group, that is, in a particular role; otherwise, false.</returns>
		/// <param name="rid">The RID of the Windows user group in which to check for the principal’s membership status. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public virtual bool IsInRole(int rid)
		{
			if (WindowsPrincipal.IsPosix)
			{
				return WindowsPrincipal.IsMemberOfGroupId(this.Token, (IntPtr)rid);
			}
			string role;
			switch (rid)
			{
			case 544:
				role = "BUILTIN\\Administrators";
				break;
			case 545:
				role = "BUILTIN\\Users";
				break;
			case 546:
				role = "BUILTIN\\Guests";
				break;
			case 547:
				role = "BUILTIN\\Power Users";
				break;
			case 548:
				role = "BUILTIN\\Account Operators";
				break;
			case 549:
				role = "BUILTIN\\System Operators";
				break;
			case 550:
				role = "BUILTIN\\Print Operators";
				break;
			case 551:
				role = "BUILTIN\\Backup Operators";
				break;
			case 552:
				role = "BUILTIN\\Replicator";
				break;
			default:
				return false;
			}
			return this.IsInRole(role);
		}

		/// <summary>Determines whether the current principal belongs to the Windows user group with the specified name.</summary>
		/// <returns>true if the current principal is a member of the specified Windows user group; otherwise, false.</returns>
		/// <param name="role">The name of the Windows user group for which to check membership. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public virtual bool IsInRole(string role)
		{
			if (role == null)
			{
				return false;
			}
			if (WindowsPrincipal.IsPosix)
			{
				return WindowsPrincipal.IsMemberOfGroupName(this.Token, role);
			}
			if (this.m_roles == null)
			{
				this.m_roles = WindowsIdentity._GetRoles(this.Token);
			}
			role = role.ToUpperInvariant();
			foreach (string text in this.m_roles)
			{
				if (text != null && role == text.ToUpperInvariant())
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Determines whether the current principal belongs to the Windows user group with the specified <see cref="T:System.Security.Principal.WindowsBuiltInRole" />.</summary>
		/// <returns>true if the current principal is a member of the specified Windows user group; otherwise, false.</returns>
		/// <param name="role">One of the <see cref="T:System.Security.Principal.WindowsBuiltInRole" /> values. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="role" /> is not a valid <see cref="T:System.Security.Principal.WindowsBuiltInRole" /> value.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public virtual bool IsInRole(WindowsBuiltInRole role)
		{
			if (!WindowsPrincipal.IsPosix)
			{
				return this.IsInRole((int)role);
			}
			if (role != WindowsBuiltInRole.Administrator)
			{
				return false;
			}
			string role2 = "root";
			return this.IsInRole(role2);
		}

		/// <summary>Determines whether the current principal belongs to the Windows user group with the specified security identifier (SID).</summary>
		/// <returns>true if the current principal is a member of the specified Windows user group; otherwise, false.</returns>
		/// <param name="sid">A <see cref="T:System.Security.Principal.SecurityIdentifier" />  that uniquely identifies a Windows user group.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="sid" /> is null.</exception>
		/// <exception cref="T:System.Security.SecurityException">Windows returned a Win32 error.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		[MonoTODO("not implemented")]
		[ComVisible(false)]
		public virtual bool IsInRole(SecurityIdentifier sid)
		{
			throw new NotImplementedException();
		}

		private static bool IsPosix
		{
			get
			{
				int platform = (int)Environment.Platform;
				return platform == 128 || platform == 4 || platform == 6;
			}
		}

		private IntPtr Token
		{
			get
			{
				return this._identity.Token;
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsMemberOfGroupId(IntPtr user, IntPtr group);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsMemberOfGroupName(IntPtr user, string group);
	}
}
