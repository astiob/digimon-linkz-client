using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	/// <summary>Defines the identity permission for the Web site from which the code originates. This class cannot be inherited.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class SiteIdentityPermission : CodeAccessPermission, IBuiltInPermission
	{
		private const int version = 1;

		private string _site;

		private static bool[] valid = new bool[]
		{
			true,
			false,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			false,
			false,
			true,
			true,
			false,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			false,
			false,
			false,
			true,
			true,
			false,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			false,
			true,
			true
		};

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.SiteIdentityPermission" /> class with the specified <see cref="T:System.Security.Permissions.PermissionState" />.</summary>
		/// <param name="state">One of the <see cref="T:System.Security.Permissions.PermissionState" /> values.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="state" /> parameter is not a valid value of <see cref="T:System.Security.Permissions.PermissionState" />. </exception>
		public SiteIdentityPermission(PermissionState state)
		{
			CodeAccessPermission.CheckPermissionState(state, false);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.SiteIdentityPermission" /> class to represent the specified site identity.</summary>
		/// <param name="site">The site name or wildcard expression. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="site" /> parameter is not a valid string, or does not match a valid wildcard site name. </exception>
		public SiteIdentityPermission(string site)
		{
			this.Site = site;
		}

		int IBuiltInPermission.GetTokenIndex()
		{
			return 11;
		}

		/// <summary>Gets or sets the current site.</summary>
		/// <returns>The current site.</returns>
		/// <exception cref="T:System.NotSupportedException">The site identity cannot be retrieved because it has an ambiguous identity.</exception>
		public string Site
		{
			get
			{
				if (this.IsEmpty())
				{
					throw new NullReferenceException("No site.");
				}
				return this._site;
			}
			set
			{
				if (!this.IsValid(value))
				{
					throw new ArgumentException("Invalid site.");
				}
				this._site = value;
			}
		}

		/// <summary>Creates and returns an identical copy of the current permission.</summary>
		/// <returns>A copy of the current permission.</returns>
		public override IPermission Copy()
		{
			if (this.IsEmpty())
			{
				return new SiteIdentityPermission(PermissionState.None);
			}
			return new SiteIdentityPermission(this._site);
		}

		/// <summary>Reconstructs a permission with a specified state from an XML encoding.</summary>
		/// <param name="esd">The XML encoding to use to reconstruct the permission. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="esd" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="esd" /> parameter is not a valid permission element.-or- The <paramref name="esd" /> parameter's version number is not valid. </exception>
		public override void FromXml(SecurityElement esd)
		{
			CodeAccessPermission.CheckSecurityElement(esd, "esd", 1, 1);
			string text = esd.Attribute("Site");
			if (text != null)
			{
				this.Site = text;
			}
		}

		/// <summary>Creates and returns a permission that is the intersection of the current permission and the specified permission.</summary>
		/// <returns>A new permission that represents the intersection of the current permission and the specified permission. This new permission is null if the intersection is empty.</returns>
		/// <param name="target">A permission to intersect with the current permission. It must be of the same type as the current permission. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="target" /> parameter is not null and is not of the same type as the current permission. </exception>
		public override IPermission Intersect(IPermission target)
		{
			SiteIdentityPermission siteIdentityPermission = this.Cast(target);
			if (siteIdentityPermission == null || this.IsEmpty())
			{
				return null;
			}
			if (this.Match(siteIdentityPermission._site))
			{
				string site = (this._site.Length <= siteIdentityPermission._site.Length) ? siteIdentityPermission._site : this._site;
				return new SiteIdentityPermission(site);
			}
			return null;
		}

		/// <summary>Determines whether the current permission is a subset of the specified permission.</summary>
		/// <returns>true if the current permission is a subset of the specified permission; otherwise, false.</returns>
		/// <param name="target">A permission that is to be tested for the subset relationship. This permission must be of the same type as the current permission. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="target" /> parameter is not null and is not of the same type as the current permission. </exception>
		public override bool IsSubsetOf(IPermission target)
		{
			SiteIdentityPermission siteIdentityPermission = this.Cast(target);
			if (siteIdentityPermission == null)
			{
				return this.IsEmpty();
			}
			if (this._site == null && siteIdentityPermission._site == null)
			{
				return true;
			}
			if (this._site == null || siteIdentityPermission._site == null)
			{
				return false;
			}
			int num = siteIdentityPermission._site.IndexOf('*');
			if (num == -1)
			{
				return this._site == siteIdentityPermission._site;
			}
			return this._site.EndsWith(siteIdentityPermission._site.Substring(num + 1));
		}

		/// <summary>Creates an XML encoding of the permission and its current state.</summary>
		/// <returns>An XML encoding of the permission, including any state information.</returns>
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = base.Element(1);
			if (this._site != null)
			{
				securityElement.AddAttribute("Site", this._site);
			}
			return securityElement;
		}

		/// <summary>Creates a permission that is the union of the current permission and the specified permission.</summary>
		/// <returns>A new permission that represents the union of the current permission and the specified permission.</returns>
		/// <param name="target">A permission to combine with the current permission. It must be of the same type as the current permission. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="target" /> parameter is not null and is not of the same type as the current permission. -or-The permissions are not equal and one is not a subset of the other.</exception>
		public override IPermission Union(IPermission target)
		{
			SiteIdentityPermission siteIdentityPermission = this.Cast(target);
			if (siteIdentityPermission == null || siteIdentityPermission.IsEmpty())
			{
				return this.Copy();
			}
			if (this.IsEmpty())
			{
				return siteIdentityPermission.Copy();
			}
			if (this.Match(siteIdentityPermission._site))
			{
				string site = (this._site.Length >= siteIdentityPermission._site.Length) ? siteIdentityPermission._site : this._site;
				return new SiteIdentityPermission(site);
			}
			throw new ArgumentException(Locale.GetText("Cannot union two different sites."), "target");
		}

		private bool IsEmpty()
		{
			return this._site == null;
		}

		private SiteIdentityPermission Cast(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			SiteIdentityPermission siteIdentityPermission = target as SiteIdentityPermission;
			if (siteIdentityPermission == null)
			{
				CodeAccessPermission.ThrowInvalidPermission(target, typeof(SiteIdentityPermission));
			}
			return siteIdentityPermission;
		}

		private bool IsValid(string s)
		{
			if (s == null || s.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < s.Length; i++)
			{
				ushort num = (ushort)s[i];
				if (num < 33 || num > 126)
				{
					return false;
				}
				if (num == 42 && s.Length > 1 && (s[i + 1] != '.' || i > 0))
				{
					return false;
				}
				if (!SiteIdentityPermission.valid[(int)(num - 33)])
				{
					return false;
				}
			}
			return s.Length != 1 || s[0] != '.';
		}

		private bool Match(string target)
		{
			if (this._site == null || target == null)
			{
				return false;
			}
			int num = this._site.IndexOf('*');
			int num2 = target.IndexOf('*');
			if (num == -1 && num2 == -1)
			{
				return this._site == target;
			}
			if (num == -1)
			{
				return this._site.EndsWith(target.Substring(num2 + 1));
			}
			if (num2 == -1)
			{
				return target.EndsWith(this._site.Substring(num + 1));
			}
			string text = this._site.Substring(num + 1);
			target = target.Substring(num2 + 1);
			if (text.Length > target.Length)
			{
				return text.EndsWith(target);
			}
			return target.EndsWith(text);
		}
	}
}
