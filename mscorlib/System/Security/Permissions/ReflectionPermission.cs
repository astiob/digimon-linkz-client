using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	/// <summary>Controls access to non-public types and members through the <see cref="N:System.Reflection" /> APIs. Controls some features of the <see cref="N:System.Reflection.Emit" /> APIs.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class ReflectionPermission : CodeAccessPermission, IBuiltInPermission, IUnrestrictedPermission
	{
		private const int version = 1;

		private ReflectionPermissionFlag flags;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.ReflectionPermission" /> class with either fully restricted or unrestricted permission as specified.</summary>
		/// <param name="state">One of the <see cref="T:System.Security.Permissions.PermissionState" /> values. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="state" /> parameter is not a valid value of <see cref="T:System.Security.Permissions.PermissionState" />. </exception>
		public ReflectionPermission(PermissionState state)
		{
			if (CodeAccessPermission.CheckPermissionState(state, true) == PermissionState.Unrestricted)
			{
				this.flags = ReflectionPermissionFlag.AllFlags;
			}
			else
			{
				this.flags = ReflectionPermissionFlag.NoFlags;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.ReflectionPermission" /> class with the specified access.</summary>
		/// <param name="flag">One of the <see cref="T:System.Security.Permissions.ReflectionPermissionFlag" /> values. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="flag" /> parameter is not a valid value of <see cref="T:System.Security.Permissions.ReflectionPermissionFlag" />. </exception>
		public ReflectionPermission(ReflectionPermissionFlag flag)
		{
			this.Flags = flag;
		}

		int IBuiltInPermission.GetTokenIndex()
		{
			return 4;
		}

		/// <summary>Gets or sets the type of reflection allowed for the current permission.</summary>
		/// <returns>The set flags for the current permission.</returns>
		/// <exception cref="T:System.ArgumentException">An attempt is made to set this property to an invalid value. See <see cref="T:System.Security.Permissions.ReflectionPermissionFlag" /> for the valid values. </exception>
		public ReflectionPermissionFlag Flags
		{
			get
			{
				return this.flags;
			}
			set
			{
				if ((value & (ReflectionPermissionFlag.TypeInformation | ReflectionPermissionFlag.MemberAccess | ReflectionPermissionFlag.ReflectionEmit | ReflectionPermissionFlag.RestrictedMemberAccess)) != value)
				{
					string message = string.Format(Locale.GetText("Invalid flags {0}"), value);
					throw new ArgumentException(message, "ReflectionPermissionFlag");
				}
				this.flags = value;
			}
		}

		/// <summary>Creates and returns an identical copy of the current permission.</summary>
		/// <returns>A copy of the current permission.</returns>
		public override IPermission Copy()
		{
			return new ReflectionPermission(this.flags);
		}

		/// <summary>Reconstructs a permission with a specified state from an XML encoding.</summary>
		/// <param name="esd">The XML encoding to use to reconstruct the permission. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="esd" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="esd" /> parameter is not a valid permission element.-or- The <paramref name="esd" /> parameter's version number is not valid. </exception>
		public override void FromXml(SecurityElement esd)
		{
			CodeAccessPermission.CheckSecurityElement(esd, "esd", 1, 1);
			if (CodeAccessPermission.IsUnrestricted(esd))
			{
				this.flags = ReflectionPermissionFlag.AllFlags;
			}
			else
			{
				this.flags = ReflectionPermissionFlag.NoFlags;
				string text = esd.Attributes["Flags"] as string;
				if (text.IndexOf("MemberAccess") >= 0)
				{
					this.flags |= ReflectionPermissionFlag.MemberAccess;
				}
				if (text.IndexOf("ReflectionEmit") >= 0)
				{
					this.flags |= ReflectionPermissionFlag.ReflectionEmit;
				}
				if (text.IndexOf("TypeInformation") >= 0)
				{
					this.flags |= ReflectionPermissionFlag.TypeInformation;
				}
			}
		}

		/// <summary>Creates and returns a permission that is the intersection of the current permission and the specified permission.</summary>
		/// <returns>A new permission that represents the intersection of the current permission and the specified permission. This new permission is null if the intersection is empty.</returns>
		/// <param name="target">A permission to intersect with the current permission. It must be of the same type as the current permission. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="target" /> parameter is not null and is not of the same type as the current permission. </exception>
		public override IPermission Intersect(IPermission target)
		{
			ReflectionPermission reflectionPermission = this.Cast(target);
			if (reflectionPermission == null)
			{
				return null;
			}
			if (this.IsUnrestricted())
			{
				if (reflectionPermission.Flags == ReflectionPermissionFlag.NoFlags)
				{
					return null;
				}
				return reflectionPermission.Copy();
			}
			else
			{
				if (!reflectionPermission.IsUnrestricted())
				{
					ReflectionPermission reflectionPermission2 = (ReflectionPermission)reflectionPermission.Copy();
					reflectionPermission2.Flags &= this.flags;
					return (reflectionPermission2.Flags != ReflectionPermissionFlag.NoFlags) ? reflectionPermission2 : null;
				}
				if (this.flags == ReflectionPermissionFlag.NoFlags)
				{
					return null;
				}
				return this.Copy();
			}
		}

		/// <summary>Determines whether the current permission is a subset of the specified permission.</summary>
		/// <returns>true if the current permission is a subset of the specified permission; otherwise, false.</returns>
		/// <param name="target">A permission that is to be tested for the subset relationship. This permission must be of the same type as the current permission. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="target" /> parameter is not null and is not of the same type as the current permission. </exception>
		public override bool IsSubsetOf(IPermission target)
		{
			ReflectionPermission reflectionPermission = this.Cast(target);
			if (reflectionPermission == null)
			{
				return this.flags == ReflectionPermissionFlag.NoFlags;
			}
			if (this.IsUnrestricted())
			{
				return reflectionPermission.IsUnrestricted();
			}
			return reflectionPermission.IsUnrestricted() || (this.flags & reflectionPermission.Flags) == this.flags;
		}

		/// <summary>Returns a value indicating whether the current permission is unrestricted.</summary>
		/// <returns>true if the current permission is unrestricted; otherwise, false.</returns>
		public bool IsUnrestricted()
		{
			return this.flags == ReflectionPermissionFlag.AllFlags;
		}

		/// <summary>Creates an XML encoding of the permission and its current state.</summary>
		/// <returns>An XML encoding of the permission, including any state information.</returns>
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = base.Element(1);
			if (this.IsUnrestricted())
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			else if (this.flags == ReflectionPermissionFlag.NoFlags)
			{
				securityElement.AddAttribute("Flags", "NoFlags");
			}
			else if ((this.flags & ReflectionPermissionFlag.AllFlags) == ReflectionPermissionFlag.AllFlags)
			{
				securityElement.AddAttribute("Flags", "AllFlags");
			}
			else
			{
				string text = string.Empty;
				if ((this.flags & ReflectionPermissionFlag.MemberAccess) == ReflectionPermissionFlag.MemberAccess)
				{
					text = "MemberAccess";
				}
				if ((this.flags & ReflectionPermissionFlag.ReflectionEmit) == ReflectionPermissionFlag.ReflectionEmit)
				{
					if (text.Length > 0)
					{
						text += ", ";
					}
					text += "ReflectionEmit";
				}
				if ((this.flags & ReflectionPermissionFlag.TypeInformation) == ReflectionPermissionFlag.TypeInformation)
				{
					if (text.Length > 0)
					{
						text += ", ";
					}
					text += "TypeInformation";
				}
				securityElement.AddAttribute("Flags", text);
			}
			return securityElement;
		}

		/// <summary>Creates a permission that is the union of the current permission and the specified permission.</summary>
		/// <returns>A new permission that represents the union of the current permission and the specified permission.</returns>
		/// <param name="other">A permission to combine with the current permission. It must be of the same type as the current permission. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="other" /> parameter is not null and is not of the same type as the current permission. </exception>
		public override IPermission Union(IPermission other)
		{
			ReflectionPermission reflectionPermission = this.Cast(other);
			if (other == null)
			{
				return this.Copy();
			}
			if (this.IsUnrestricted() || reflectionPermission.IsUnrestricted())
			{
				return new ReflectionPermission(PermissionState.Unrestricted);
			}
			ReflectionPermission reflectionPermission2 = (ReflectionPermission)reflectionPermission.Copy();
			reflectionPermission2.Flags |= this.flags;
			return reflectionPermission2;
		}

		private ReflectionPermission Cast(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			ReflectionPermission reflectionPermission = target as ReflectionPermission;
			if (reflectionPermission == null)
			{
				CodeAccessPermission.ThrowInvalidPermission(target, typeof(ReflectionPermission));
			}
			return reflectionPermission;
		}
	}
}
