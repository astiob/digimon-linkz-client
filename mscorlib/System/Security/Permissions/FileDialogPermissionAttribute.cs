using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	/// <summary>Allows security actions for <see cref="T:System.Security.Permissions.FileDialogPermission" /> to be applied to code using declarative security. This class cannot be inherited.</summary>
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class FileDialogPermissionAttribute : CodeAccessSecurityAttribute
	{
		private bool canOpen;

		private bool canSave;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.FileDialogPermissionAttribute" /> class with the specified <see cref="T:System.Security.Permissions.SecurityAction" />.</summary>
		/// <param name="action">One of the <see cref="T:System.Security.Permissions.SecurityAction" /> values. </param>
		public FileDialogPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		/// <summary>Gets or sets a value indicating whether permission to open files through the file dialog is declared.</summary>
		/// <returns>true if permission to open files through the file dialog is declared; otherwise, false.</returns>
		public bool Open
		{
			get
			{
				return this.canOpen;
			}
			set
			{
				this.canOpen = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether permission to save files through the file dialog is declared.</summary>
		/// <returns>true if permission to save files through the file dialog is declared; otherwise, false.</returns>
		public bool Save
		{
			get
			{
				return this.canSave;
			}
			set
			{
				this.canSave = value;
			}
		}

		/// <summary>Creates and returns a new <see cref="T:System.Security.Permissions.FileDialogPermission" />.</summary>
		/// <returns>A <see cref="T:System.Security.Permissions.FileDialogPermission" /> that corresponds to this attribute.</returns>
		public override IPermission CreatePermission()
		{
			FileDialogPermission result;
			if (base.Unrestricted)
			{
				result = new FileDialogPermission(PermissionState.Unrestricted);
			}
			else
			{
				FileDialogPermissionAccess fileDialogPermissionAccess = FileDialogPermissionAccess.None;
				if (this.canOpen)
				{
					fileDialogPermissionAccess |= FileDialogPermissionAccess.Open;
				}
				if (this.canSave)
				{
					fileDialogPermissionAccess |= FileDialogPermissionAccess.Save;
				}
				result = new FileDialogPermission(fileDialogPermissionAccess);
			}
			return result;
		}
	}
}
