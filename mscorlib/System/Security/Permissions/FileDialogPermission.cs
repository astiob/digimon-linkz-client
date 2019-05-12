using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	/// <summary>Controls the ability to access files or folders through a File dialog box. This class cannot be inherited.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class FileDialogPermission : CodeAccessPermission, IBuiltInPermission, IUnrestrictedPermission
	{
		private const int version = 1;

		private FileDialogPermissionAccess _access;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.FileDialogPermission" /> class with either restricted or unrestricted permission, as specified.</summary>
		/// <param name="state">One of the <see cref="T:System.Security.Permissions.PermissionState" /> values (Unrestricted or None). </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="state" /> parameter is not a valid value of <see cref="T:System.Security.Permissions.PermissionState" />. </exception>
		public FileDialogPermission(PermissionState state)
		{
			if (CodeAccessPermission.CheckPermissionState(state, true) == PermissionState.Unrestricted)
			{
				this._access = FileDialogPermissionAccess.OpenSave;
			}
			else
			{
				this._access = FileDialogPermissionAccess.None;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.FileDialogPermission" /> class with the specified access.</summary>
		/// <param name="access">A bitwise combination of the <see cref="T:System.Security.Permissions.FileDialogPermissionAccess" /> values. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="access" /> parameter is not a valid combination of the <see cref="T:System.Security.Permissions.FileDialogPermissionAccess" /> values. </exception>
		public FileDialogPermission(FileDialogPermissionAccess access)
		{
			this.Access = access;
		}

		int IBuiltInPermission.GetTokenIndex()
		{
			return 1;
		}

		/// <summary>Gets or sets the permitted access to files.</summary>
		/// <returns>The permitted access to files.</returns>
		/// <exception cref="T:System.ArgumentException">An attempt is made to set the <paramref name="access" /> parameter to a value that is not a valid combination of the <see cref="T:System.Security.Permissions.FileDialogPermissionAccess" /> values. </exception>
		public FileDialogPermissionAccess Access
		{
			get
			{
				return this._access;
			}
			set
			{
				if (!Enum.IsDefined(typeof(FileDialogPermissionAccess), value))
				{
					string message = string.Format(Locale.GetText("Invalid enum {0}"), value);
					throw new ArgumentException(message, "FileDialogPermissionAccess");
				}
				this._access = value;
			}
		}

		/// <summary>Creates and returns an identical copy of the current permission.</summary>
		/// <returns>A copy of the current permission.</returns>
		public override IPermission Copy()
		{
			return new FileDialogPermission(this._access);
		}

		/// <summary>Reconstructs a permission with a specified state from an XML encoding.</summary>
		/// <param name="esd">The XML encoding used to reconstruct the permission. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="esd" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="esd" /> parameter is not a valid permission element.-or- The version number of the <paramref name="esd" /> parameter is not supported. </exception>
		public override void FromXml(SecurityElement esd)
		{
			CodeAccessPermission.CheckSecurityElement(esd, "esd", 1, 1);
			if (CodeAccessPermission.IsUnrestricted(esd))
			{
				this._access = FileDialogPermissionAccess.OpenSave;
			}
			else
			{
				string text = esd.Attribute("Access");
				if (text == null)
				{
					this._access = FileDialogPermissionAccess.None;
				}
				else
				{
					this._access = (FileDialogPermissionAccess)((int)Enum.Parse(typeof(FileDialogPermissionAccess), text));
				}
			}
		}

		/// <summary>Creates and returns a permission that is the intersection of the current permission and the specified permission.</summary>
		/// <returns>A new permission that represents the intersection of the current permission and the specified permission. This new permission is null if the intersection is empty.</returns>
		/// <param name="target">A permission to intersect with the current permission. It must be the same type as the current permission. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="target" /> parameter is not null and is not of the same type as the current permission. </exception>
		public override IPermission Intersect(IPermission target)
		{
			FileDialogPermission fileDialogPermission = this.Cast(target);
			if (fileDialogPermission == null)
			{
				return null;
			}
			FileDialogPermissionAccess fileDialogPermissionAccess = this._access & fileDialogPermission._access;
			return (fileDialogPermissionAccess != FileDialogPermissionAccess.None) ? new FileDialogPermission(fileDialogPermissionAccess) : null;
		}

		/// <summary>Determines whether the current permission is a subset of the specified permission.</summary>
		/// <returns>true if the current permission is a subset of the specified permission; otherwise, false.</returns>
		/// <param name="target">A permission that is to be tested for the subset relationship. This permission must be the same type as the current permission. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="target" /> parameter is not null and is not of the same type as the current permission. </exception>
		public override bool IsSubsetOf(IPermission target)
		{
			FileDialogPermission fileDialogPermission = this.Cast(target);
			return fileDialogPermission != null && (this._access & fileDialogPermission._access) == this._access;
		}

		/// <summary>Returns a value indicating whether the current permission is unrestricted.</summary>
		/// <returns>true if the current permission is unrestricted; otherwise, false.</returns>
		public bool IsUnrestricted()
		{
			return this._access == FileDialogPermissionAccess.OpenSave;
		}

		/// <summary>Creates an XML encoding of the permission and its current state.</summary>
		/// <returns>An XML encoding of the permission, including state information.</returns>
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = base.Element(1);
			switch (this._access)
			{
			case FileDialogPermissionAccess.Open:
				securityElement.AddAttribute("Access", "Open");
				break;
			case FileDialogPermissionAccess.Save:
				securityElement.AddAttribute("Access", "Save");
				break;
			case FileDialogPermissionAccess.OpenSave:
				securityElement.AddAttribute("Unrestricted", "true");
				break;
			}
			return securityElement;
		}

		/// <summary>Creates a permission that is the union of the current permission and the specified permission.</summary>
		/// <returns>A new permission that represents the union of the current permission and the specified permission.</returns>
		/// <param name="target">A permission to combine with the current permission. It must be of the same type as the current permission. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="target" /> parameter is not null and is not of the same type as the current permission. </exception>
		public override IPermission Union(IPermission target)
		{
			FileDialogPermission fileDialogPermission = this.Cast(target);
			if (fileDialogPermission == null)
			{
				return this.Copy();
			}
			if (this.IsUnrestricted() || fileDialogPermission.IsUnrestricted())
			{
				return new FileDialogPermission(PermissionState.Unrestricted);
			}
			return new FileDialogPermission(this._access | fileDialogPermission._access);
		}

		private FileDialogPermission Cast(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			FileDialogPermission fileDialogPermission = target as FileDialogPermission;
			if (fileDialogPermission == null)
			{
				CodeAccessPermission.ThrowInvalidPermission(target, typeof(FileDialogPermission));
			}
			return fileDialogPermission;
		}
	}
}
