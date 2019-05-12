using System;
using System.Collections;
using System.Security.Permissions;
using System.Security.Policy;

namespace System.Security
{
	/// <summary>Represents a collection that can contain many different types of permissions.</summary>
	[Serializable]
	public class PermissionSet
	{
		public PermissionSet()
		{
		}

		internal PermissionSet(string xml)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.PermissionSet" /> class with the specified <see cref="T:System.Security.Permissions.PermissionState" />.</summary>
		/// <param name="state">One of the <see cref="T:System.Security.Permissions.PermissionState" /> values. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="state" /> parameter is not a valid <see cref="T:System.Security.Permissions.PermissionState" />. </exception>
		public PermissionSet(PermissionState state)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.PermissionSet" /> class with initial values taken from the <paramref name="permSet" /> parameter.</summary>
		/// <param name="permSet">The <see cref="T:System.Security.PermissionSet" /> from which to take the value of the new <see cref="T:System.Security.PermissionSet" />, or null to create an empty <see cref="T:System.Security.PermissionSet" />. </param>
		public PermissionSet(PermissionSet permSet)
		{
		}

		/// <summary>Adds a specified permission to the <see cref="T:System.Security.PermissionSet" />.</summary>
		/// <returns>The union of the permission added and any permission of the same type that already exists in the <see cref="T:System.Security.PermissionSet" />.</returns>
		/// <param name="perm">The permission to add. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public IPermission AddPermission(IPermission perm)
		{
			return perm;
		}

		/// <summary>Declares that the calling code can access the resource protected by a permission demand through the code that calls this method, even if callers higher in the stack have not been granted permission to access the resource. Using <see cref="M:System.Security.PermissionSet.Assert" /> can create security vulnerabilities.</summary>
		/// <exception cref="T:System.Security.SecurityException">The <see cref="T:System.Security.PermissionSet" /> instance asserted has not been granted to the asserting code.-or- There is already an active <see cref="M:System.Security.PermissionSet.Assert" /> for the current frame. </exception>
		public virtual void Assert()
		{
		}

		/// <summary>Creates a copy of the <see cref="T:System.Security.PermissionSet" />.</summary>
		/// <returns>A copy of the <see cref="T:System.Security.PermissionSet" />.</returns>
		public virtual PermissionSet Copy()
		{
			return new PermissionSet(this);
		}

		/// <summary>Forces a <see cref="T:System.Security.SecurityException" /> at run time if all callers higher in the call stack have not been granted the permissions specified by the current instance.</summary>
		/// <exception cref="T:System.Security.SecurityException">A caller in the call chain does not have the permission demanded. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence, ControlPolicy" />
		/// </PermissionSet>
		public virtual void Demand()
		{
		}

		/// <summary>Causes any <see cref="M:System.Security.PermissionSet.Demand" /> that passes through the calling code for any <see cref="T:System.Security.PermissionSet" /> that is not a subset of the current <see cref="T:System.Security.PermissionSet" /> to fail.</summary>
		public virtual void PermitOnly()
		{
		}

		/// <summary>Gets a permission object of the specified type, if it exists in the set.</summary>
		/// <returns>A copy of the permission object of the type specified by the <paramref name="permClass" /> parameter contained in the <see cref="T:System.Security.PermissionSet" />, or null if none exists.</returns>
		/// <param name="permClass">The <see cref="T:System.Type" /> of the desired permission object. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public virtual IPermission GetPermission(Type permClass)
		{
			return null;
		}

		/// <summary>Creates and returns a permission set that is the intersection of the current <see cref="T:System.Security.PermissionSet" /> and the specified <see cref="T:System.Security.PermissionSet" />.</summary>
		/// <returns>A new <see cref="T:System.Security.PermissionSet" /> that represents the intersection of the current <see cref="T:System.Security.PermissionSet" /> and the specified target. This object is null if the intersection is empty.</returns>
		/// <param name="other">A <see cref="T:System.Security.PermissionSet" /> to intersect with the current <see cref="T:System.Security.PermissionSet" />. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public virtual PermissionSet Intersect(PermissionSet other)
		{
			return other;
		}

		/// <summary>Causes any <see cref="M:System.Security.PermissionSet.Demand" /> that passes through the calling code for a permission that has an intersection with a permission of a type contained in the current <see cref="T:System.Security.PermissionSet" /> to fail.</summary>
		/// <exception cref="T:System.Security.SecurityException">A previous call to <see cref="M:System.Security.PermissionSet.Deny" /> has already restricted the permissions for the current stack frame. </exception>
		public virtual void Deny()
		{
		}

		/// <summary>Reconstructs a security object with a specified state from an XML encoding.</summary>
		/// <param name="et">The XML encoding to use to reconstruct the security object. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="et" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="et" /> parameter is not a valid permission element.-or- The <paramref name="et" /> parameter's version number is not supported. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public virtual void FromXml(SecurityElement et)
		{
		}

		/// <summary>Copies the permission objects of the set to the indicated location in an <see cref="T:System.Array" />.</summary>
		/// <param name="array">The target array to which to copy. </param>
		/// <param name="index">The starting position in the array to begin copying (zero based). </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="array" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="array" /> parameter has more than one dimension. </exception>
		/// <exception cref="T:System.IndexOutOfRangeException">The <paramref name="index" /> parameter is out of the range of the <paramref name="array" /> parameter. </exception>
		public virtual void CopyTo(Array array, int index)
		{
		}

		/// <summary>Creates an XML encoding of the security object and its current state.</summary>
		/// <returns>An XML encoding of the security object, including any state information.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public virtual SecurityElement ToXml()
		{
			return null;
		}

		/// <summary>Determines whether the current <see cref="T:System.Security.PermissionSet" /> is a subset of the specified <see cref="T:System.Security.PermissionSet" />.</summary>
		/// <returns>true if the current <see cref="T:System.Security.PermissionSet" /> is a subset of the <paramref name="target" /> parameter; otherwise, false.</returns>
		/// <param name="target">A <see cref="T:System.Security.PermissionSet" /> to test for the subset relationship. This must be either a <see cref="T:System.Security.PermissionSet" /> or a <see cref="T:System.Security.NamedPermissionSet" />. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public virtual bool IsSubsetOf(PermissionSet target)
		{
			return true;
		}

		internal void SetReadOnly(bool value)
		{
		}

		/// <summary>Determines whether the <see cref="T:System.Security.PermissionSet" /> is Unrestricted.</summary>
		/// <returns>true if the <see cref="T:System.Security.PermissionSet" /> is Unrestricted; otherwise, false.</returns>
		public bool IsUnrestricted()
		{
			return true;
		}

		/// <summary>Creates a <see cref="T:System.Security.PermissionSet" /> that is the union of the current <see cref="T:System.Security.PermissionSet" /> and the specified <see cref="T:System.Security.PermissionSet" />.</summary>
		/// <returns>A new <see cref="T:System.Security.PermissionSet" /> that represents the union of the current <see cref="T:System.Security.PermissionSet" /> and the specified <see cref="T:System.Security.PermissionSet" />.</returns>
		/// <param name="other">A <see cref="T:System.Security.PermissionSet" /> to form the union with the current <see cref="T:System.Security.PermissionSet" />. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public PermissionSet Union(PermissionSet other)
		{
			return new PermissionSet();
		}

		/// <summary>Returns an enumerator for the permissions of the set.</summary>
		/// <returns>An enumerator object for the permissions of the set.</returns>
		public virtual IEnumerator GetEnumerator()
		{
			yield break;
		}

		internal PolicyLevel Resolver { get; set; }

		internal bool DeclarativeSecurity { get; set; }

		/// <summary>Gets a value indicating whether the <see cref="T:System.Security.PermissionSet" /> is empty.</summary>
		/// <returns>true if the <see cref="T:System.Security.PermissionSet" /> is empty; otherwise, false.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public virtual bool IsEmpty()
		{
			return true;
		}

		internal static PermissionSet CreateFromBinaryFormat(byte[] data)
		{
			return new PermissionSet();
		}
	}
}
