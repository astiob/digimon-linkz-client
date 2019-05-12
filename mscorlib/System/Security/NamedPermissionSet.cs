using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security
{
	/// <summary>Defines a permission set that has a name and description associated with it. This class cannot be inherited.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class NamedPermissionSet : PermissionSet
	{
		private string name;

		private string description;

		internal NamedPermissionSet()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.NamedPermissionSet" /> class with the specified name from a permission set.</summary>
		/// <param name="name">The name for the named permission set. </param>
		/// <param name="permSet">The permission set from which to take the value of the new named permission set. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="name" /> parameter is null or is an empty string (""). </exception>
		public NamedPermissionSet(string name, PermissionSet permSet) : base(permSet)
		{
			this.Name = name;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.NamedPermissionSet" /> class with the specified name in either an unrestricted or a fully restricted state.</summary>
		/// <param name="name">The name for the new named permission set. </param>
		/// <param name="state">One of the <see cref="T:System.Security.Permissions.PermissionState" /> values. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="name" /> parameter is null or is an empty string (""). </exception>
		public NamedPermissionSet(string name, PermissionState state) : base(state)
		{
			this.Name = name;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.NamedPermissionSet" /> class from another named permission set.</summary>
		/// <param name="permSet">The named permission set from which to create the new instance. </param>
		public NamedPermissionSet(NamedPermissionSet permSet) : base(permSet)
		{
			this.name = permSet.name;
			this.description = permSet.description;
		}

		/// <summary>Initializes a new, empty instance of the <see cref="T:System.Security.NamedPermissionSet" /> class with the specified name.</summary>
		/// <param name="name">The name for the new named permission set. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="name" /> parameter is null or is an empty string (""). </exception>
		public NamedPermissionSet(string name) : this(name, PermissionState.Unrestricted)
		{
		}

		/// <summary>Gets or sets the text description of the current named permission set.</summary>
		/// <returns>A text description of the named permission set.</returns>
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		/// <summary>Gets or sets the name of the current named permission set.</summary>
		/// <returns>The name of the named permission set.</returns>
		/// <exception cref="T:System.ArgumentException">The name is null or is an empty string (""). </exception>
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (value == null || value == string.Empty)
				{
					throw new ArgumentException(Locale.GetText("invalid name"));
				}
				this.name = value;
			}
		}

		/// <summary>Creates a permission set copy from a named permission set.</summary>
		/// <returns>A permission set that is a copy of the permissions in the named permission set.</returns>
		public override PermissionSet Copy()
		{
			return new NamedPermissionSet(this);
		}

		/// <summary>Creates a copy of the named permission set with a different name but the same permissions.</summary>
		/// <returns>A copy of the named permission set with the new name.</returns>
		/// <param name="name">The name for the new named permission set. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="name" /> parameter is null or is an empty string (""). </exception>
		public NamedPermissionSet Copy(string name)
		{
			return new NamedPermissionSet(this)
			{
				Name = name
			};
		}

		/// <summary>Reconstructs a named permission set with a specified state from an XML encoding.</summary>
		/// <param name="et">A security element containing the XML representation of the named permission set. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="et" /> parameter is not a valid representation of a named permission set. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="et" /> parameter is null. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public override void FromXml(SecurityElement et)
		{
			base.FromXml(et);
			this.name = et.Attribute("Name");
			this.description = et.Attribute("Description");
			if (this.description == null)
			{
				this.description = string.Empty;
			}
		}

		/// <summary>Creates an XML element description of the named permission set.</summary>
		/// <returns>The XML representation of the named permission set.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = base.ToXml();
			if (this.name != null)
			{
				securityElement.AddAttribute("Name", this.name);
			}
			if (this.description != null)
			{
				securityElement.AddAttribute("Description", this.description);
			}
			return securityElement;
		}

		/// <summary>Determines whether the specified <see cref="T:System.Security.NamedPermissionSet" /> object is equal to the current <see cref="T:System.Security.NamedPermissionSet" />.</summary>
		/// <returns>true if the specified <see cref="T:System.Security.NamedPermissionSet" /> is equal to the current <see cref="T:System.Security.NamedPermissionSet" /> object; otherwise, false.</returns>
		/// <param name="obj">The <see cref="T:System.Security.NamedPermissionSet" /> object to compare with the current <see cref="T:System.Security.NamedPermissionSet" />. </param>
		[ComVisible(false)]
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			NamedPermissionSet namedPermissionSet = obj as NamedPermissionSet;
			return namedPermissionSet != null && this.name == namedPermissionSet.Name && base.Equals(obj);
		}

		/// <summary>Gets a hash code for the <see cref="T:System.Security.NamedPermissionSet" /> object that is suitable for use in hashing algorithms and data structures such as a hash table.</summary>
		/// <returns>A hash code for the current <see cref="T:System.Security.NamedPermissionSet" /> object.</returns>
		[ComVisible(false)]
		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			if (this.name != null)
			{
				num ^= this.name.GetHashCode();
			}
			return num;
		}
	}
}
