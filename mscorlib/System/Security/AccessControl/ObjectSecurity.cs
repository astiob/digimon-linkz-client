using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	/// <summary>Provides the ability to control access to objects without direct manipulation of Access Control Lists (ACLs). This class is the abstract base class for the <see cref="T:System.Security.AccessControl.CommonObjectSecurity" /> and <see cref="T:System.Security.AccessControl.DirectoryObjectSecurity" /> classes.</summary>
	public abstract class ObjectSecurity
	{
		private bool is_container;

		private bool is_ds;

		private bool access_rules_modified;

		private bool audit_rules_modified;

		private bool group_modified;

		private bool owner_modified;

		internal ObjectSecurity()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.AccessControl.ObjectSecurity" /> class.</summary>
		/// <param name="isContainer">true if the new <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object is a container object.</param>
		/// <param name="isDS">True if the new <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object is a directory object.</param>
		protected ObjectSecurity(bool isContainer, bool isDS)
		{
			this.is_container = isContainer;
			this.is_ds = isDS;
		}

		/// <summary>Gets the <see cref="T:System.Type" /> of the securable object associated with this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object.</summary>
		/// <returns>The type of the securable object associated with this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object.</returns>
		public abstract Type AccessRightType { get; }

		/// <summary>Gets the <see cref="T:System.Type" /> of the object associated with the access rules of this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object. The <see cref="T:System.Type" /> object must be an object that can be cast as a <see cref="T:System.Security.Principal.SecurityIdentifier" /> object.</summary>
		/// <returns>The type of the object associated with the access rules of this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object.</returns>
		public abstract Type AccessRuleType { get; }

		/// <summary>Gets the <see cref="T:System.Type" /> object associated with the audit rules of this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object. The <see cref="T:System.Type" /> object must be an object that can be cast as a <see cref="T:System.Security.Principal.SecurityIdentifier" /> object.</summary>
		/// <returns>The type of the object associated with the audit rules of this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object.</returns>
		public abstract Type AuditRuleType { get; }

		/// <summary>Gets a Boolean value that specifies whether the access rules associated with this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object are in canonical order.</summary>
		/// <returns>true if the access rules are in canonical order; otherwise, false.</returns>
		[MonoTODO]
		public bool AreAccessRulesCanonical
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>Gets a Boolean value that specifies whether the Discretionary Access Control List (DACL) associated with this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object is protected.</summary>
		/// <returns>true if the DACL is protected; otherwise, false.</returns>
		[MonoTODO]
		public bool AreAccessRulesProtected
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>Gets a Boolean value that specifies whether the audit rules associated with this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object are in canonical order.</summary>
		/// <returns>true if the audit rules are in canonical order; otherwise, false.</returns>
		[MonoTODO]
		public bool AreAuditRulesCanonical
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>Gets a Boolean value that specifies whether the System Access Control List (SACL) associated with this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object is protected.</summary>
		/// <returns>true if the SACL is protected; otherwise, false.</returns>
		[MonoTODO]
		public bool AreAuditRulesProtected
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>Gets or sets a Boolean value that specifies whether the access rules associated with this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object have been modified.</summary>
		/// <returns>true if the access rules associated with this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object have been modified; otherwise, false.</returns>
		protected bool AccessRulesModified
		{
			get
			{
				return this.access_rules_modified;
			}
			set
			{
				this.access_rules_modified = value;
			}
		}

		/// <summary>Gets or sets a Boolean value that specifies whether the audit rules associated with this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object have been modified.</summary>
		/// <returns>true if the audit rules associated with this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object have been modified; otherwise, false.</returns>
		protected bool AuditRulesModified
		{
			get
			{
				return this.audit_rules_modified;
			}
			set
			{
				this.audit_rules_modified = value;
			}
		}

		/// <summary>Gets or sets a Boolean value that specifies whether the group associated with the securable object has been modified. </summary>
		/// <returns>true if the group associated with the securable object has been modified; otherwise, false.</returns>
		protected bool GroupModified
		{
			get
			{
				return this.group_modified;
			}
			set
			{
				this.group_modified = value;
			}
		}

		/// <summary>Gets a Boolean value that specifies whether this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object is a container object.</summary>
		/// <returns>true if the <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object is a container object; otherwise, false.</returns>
		protected bool IsContainer
		{
			get
			{
				return this.is_container;
			}
		}

		/// <summary>Gets a Boolean value that specifies whether this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object is a directory object.</summary>
		/// <returns>true if the <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object is a directory object; otherwise, false.</returns>
		protected bool IsDS
		{
			get
			{
				return this.is_ds;
			}
		}

		/// <summary>Gets or sets a Boolean value that specifies whether the owner of the securable object has been modified.</summary>
		/// <returns>true if the owner of the securable object has been modified; otherwise, false.</returns>
		protected bool OwnerModified
		{
			get
			{
				return this.owner_modified;
			}
			set
			{
				this.owner_modified = value;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.AccessControl.AccessRule" /> class with the specified values.</summary>
		/// <returns>The <see cref="T:System.Security.AccessControl.AccessRule" /> object that this method creates.</returns>
		/// <param name="identityReference">The identity to which the access rule applies.  It must be an object that can be cast as a <see cref="T:System.Security.Principal.SecurityIdentifier" />.</param>
		/// <param name="accessMask">The access mask of this rule. The access mask is a 32-bit collection of anonymous bits, the meaning of which is defined by the individual integrators.</param>
		/// <param name="isInherited">true if this rule is inherited from a parent container.</param>
		/// <param name="inheritanceFlags">Specifies the inheritance properties of the access rule.</param>
		/// <param name="propagationFlags">Specifies whether inherited access rules are automatically propagated. The propagation flags are ignored if <paramref name="inheritanceFlags" /> is set to <see cref="F:System.Security.AccessControl.InheritanceFlags.None" />.</param>
		/// <param name="type">Specifies the valid access control type.</param>
		public abstract AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type);

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.AccessControl.AuditRule" /> class with the specified values.</summary>
		/// <returns>The <see cref="T:System.Security.AccessControl.AuditRule" /> object that this method creates.</returns>
		/// <param name="identityReference">The identity to which the audit rule applies.  It must be an object that can be cast as a <see cref="T:System.Security.Principal.SecurityIdentifier" />.</param>
		/// <param name="accessMask">The access mask of this rule. The access mask is a 32-bit collection of anonymous bits, the meaning of which is defined by the individual integrators.</param>
		/// <param name="isInherited">true if this rule is inherited from a parent container.</param>
		/// <param name="inheritanceFlags">Specifies the inheritance properties of the audit rule.</param>
		/// <param name="propagationFlags">Specifies whether inherited audit rules are automatically propagated. The propagation flags are ignored if <paramref name="inheritanceFlags" /> is set to <see cref="F:System.Security.AccessControl.InheritanceFlags.None" />.</param>
		/// <param name="flags">Specifies the conditions for which the rule is audited.</param>
		public abstract AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags);

		/// <summary>Gets the primary group associated with the specified owner.</summary>
		/// <returns>The primary group associated with the specified owner.</returns>
		/// <param name="targetType">The owner for which to get the primary group. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		[MonoTODO]
		public IdentityReference GetGroup(Type targetType)
		{
			throw new NotImplementedException();
		}

		/// <summary>Gets the owner associated with the specified primary group.</summary>
		/// <returns>The owner associated with the specified group.</returns>
		/// <param name="targetType">The primary group for which to get the owner.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		[MonoTODO]
		public IdentityReference GetOwner(Type targetType)
		{
			throw new NotImplementedException();
		}

		/// <summary>Returns an array of byte values that represents the security descriptor information for this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object.</summary>
		/// <returns>An array of byte values that represents the security descriptor for this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object. This method returns null if there is no security information in this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object.</returns>
		[MonoTODO]
		public byte[] GetSecurityDescriptorBinaryForm()
		{
			throw new NotImplementedException();
		}

		/// <summary>Returns the Security Descriptor Definition Language (SDDL) representation of the specified sections of the security descriptor associated with this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object.</summary>
		/// <returns>The SDDL representation of the specified sections of the security descriptor associated with this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object.</returns>
		/// <param name="includeSections">Specifies which sections (access rules, audit rules, primary group, owner) of the security descriptor to get.</param>
		[MonoTODO]
		public string GetSecurityDescriptorSddlForm(AccessControlSections includeSections)
		{
			throw new NotImplementedException();
		}

		/// <summary>Returns a Boolean value that specifies whether the security descriptor associated with this  <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object can be converted to the Security Descriptor Definition Language (SDDL) format.</summary>
		/// <returns>true if the security descriptor associated with this  <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object can be converted to the Security Descriptor Definition Language (SDDL) format; otherwise, false.</returns>
		[MonoTODO]
		public static bool IsSddlConversionSupported()
		{
			throw new NotImplementedException();
		}

		/// <summary>Applies the specified modification to the Discretionary Access Control List (DACL) associated with this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object.</summary>
		/// <returns>true if the DACL is successfully modified; otherwise, false.</returns>
		/// <param name="modification">The modification to apply to the DACL.</param>
		/// <param name="rule">The access rule to modify.</param>
		/// <param name="modified">true if the DACL is successfully modified; otherwise, false.</param>
		[MonoTODO]
		public virtual bool ModifyAccessRule(AccessControlModification modification, AccessRule rule, out bool modified)
		{
			throw new NotImplementedException();
		}

		/// <summary>Applies the specified modification to the System Access Control List (SACL) associated with this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object.</summary>
		/// <returns>true if the SACL is successfully modified; otherwise, false.</returns>
		/// <param name="modification">The modification to apply to the SACL.</param>
		/// <param name="rule">The audit rule to modify.</param>
		/// <param name="modified">true if the SACL is successfully modified; otherwise, false.</param>
		[MonoTODO]
		public virtual bool ModifyAuditRule(AccessControlModification modification, AuditRule rule, out bool modified)
		{
			throw new NotImplementedException();
		}

		/// <summary>Removes all access rules associated with the specified <see cref="T:System.Security.Principal.IdentityReference" />.</summary>
		/// <param name="identity">The <see cref="T:System.Security.Principal.IdentityReference" /> for which to remove all access rules.</param>
		/// <exception cref="T:System.InvalidOperationException">All access rules are not in canonical order.</exception>
		[MonoTODO]
		public virtual void PurgeAccessRules(IdentityReference identity)
		{
			throw new NotImplementedException();
		}

		/// <summary>Removes all audit rules associated with the specified <see cref="T:System.Security.Principal.IdentityReference" />.</summary>
		/// <param name="identity">The <see cref="T:System.Security.Principal.IdentityReference" /> for which to remove all audit rules.</param>
		/// <exception cref="T:System.InvalidOperationException">All audit rules are not in canonical order.</exception>
		[MonoTODO]
		public virtual void PurgeAuditRules(IdentityReference identity)
		{
			throw new NotImplementedException();
		}

		/// <summary>Sets or removes protection of the access rules associated with this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object. Protected access rules cannot be modified by parent objects through inheritance.</summary>
		/// <param name="isProtected">true to protect the access rules associated with this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object from inheritance; false to allow inheritance.</param>
		/// <param name="preserveInheritance">true to preserve inherited access rules; false to remove inherited access rules. This parameter is ignored if <paramref name="isProtected" /> is false.</param>
		/// <exception cref="T:System.InvalidOperationException">This method attempts to remove inherited rules from a non-canonical Discretionary Access Control List (DACL).</exception>
		[MonoTODO]
		public void SetAccessRuleProtection(bool isProtected, bool preserveInheritance)
		{
			throw new NotImplementedException();
		}

		/// <summary>Sets or removes protection of the audit rules associated with this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object. Protected audit rules cannot be modified by parent objects through inheritance.</summary>
		/// <param name="isProtected">true to protect the audit rules associated with this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object from inheritance; false to allow inheritance.</param>
		/// <param name="preserveInheritance">true to preserve inherited audit rules; false to remove inherited audit rules. This parameter is ignored if <paramref name="isProtected" /> is false.</param>
		/// <exception cref="T:System.InvalidOperationException">This method attempts to remove inherited rules from a non-canonical System Access Control List (SACL).</exception>
		[MonoTODO]
		public void SetAuditRuleProtection(bool isProtected, bool preserveInheritance)
		{
			throw new NotImplementedException();
		}

		/// <summary>Sets the primary group for the security descriptor associated with this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object.</summary>
		/// <param name="identity">The primary group to set.</param>
		[MonoTODO]
		public void SetGroup(IdentityReference identity)
		{
			throw new NotImplementedException();
		}

		/// <summary>Sets the owner for the security descriptor associated with this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object.</summary>
		/// <param name="identity">The owner to set.</param>
		[MonoTODO]
		public void SetOwner(IdentityReference identity)
		{
			throw new NotImplementedException();
		}

		/// <summary>Sets the security descriptor for this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object from the specified array of byte values.</summary>
		/// <param name="binaryForm">The array of bytes from which to set the security descriptor.</param>
		[MonoTODO]
		public void SetSecurityDescriptorBinaryForm(byte[] binaryForm)
		{
			throw new NotImplementedException();
		}

		/// <summary>Sets the specified sections of the security descriptor for this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object from the specified array of byte values.</summary>
		/// <param name="binaryForm">The array of bytes from which to set the security descriptor.</param>
		/// <param name="includeSections">The sections (access rules, audit rules, owner, primary group) of the security descriptor to set.</param>
		[MonoTODO]
		public void SetSecurityDescriptorBinaryForm(byte[] binaryForm, AccessControlSections includeSections)
		{
			throw new NotImplementedException();
		}

		/// <summary>Sets the security descriptor for this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object from the specified Security Descriptor Definition Language (SDDL) string.</summary>
		/// <param name="sddlForm">The SDDL string from which to set the security descriptor.</param>
		[MonoTODO]
		public void SetSecurityDescriptorSddlForm(string sddlForm)
		{
			throw new NotImplementedException();
		}

		/// <summary>Sets the specified sections of the security descriptor for this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object from the specified Security Descriptor Definition Language (SDDL) string.</summary>
		/// <param name="sddlForm">The SDDL string from which to set the security descriptor.</param>
		/// <param name="includeSections">The sections (access rules, audit rules, owner, primary group) of the security descriptor to set.</param>
		[MonoTODO]
		public void SetSecurityDescriptorSddlForm(string sddlForm, AccessControlSections includeSections)
		{
			throw new NotImplementedException();
		}

		/// <summary>Applies the specified modification to the Discretionary Access Control List (DACL) associated with this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object.</summary>
		/// <returns>true if the DACL is successfully modified; otherwise, false.</returns>
		/// <param name="modification">The modification to apply to the DACL.</param>
		/// <param name="rule">The access rule to modify.</param>
		/// <param name="modified">true if the DACL is successfully modified; otherwise, false.</param>
		protected abstract bool ModifyAccess(AccessControlModification modification, AccessRule rule, out bool modified);

		/// <summary>Applies the specified modification to the System Access Control List (SACL) associated with this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object.</summary>
		/// <returns>true if the SACL is successfully modified; otherwise, false.</returns>
		/// <param name="modification">The modification to apply to the SACL.</param>
		/// <param name="rule">The audit rule to modify.</param>
		/// <param name="modified">true if the SACL is successfully modified; otherwise, false.</param>
		protected abstract bool ModifyAudit(AccessControlModification modification, AuditRule rule, out bool modified);

		/// <summary>Saves the specified sections of the security descriptor associated with this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object to permanent storage. We recommend that the values of the <paramref name="includeSections" /> parameters passed to the constructor and persist methods be identical. For more information, see Remarks.</summary>
		/// <param name="handle">The handle used to retrieve the persisted information.</param>
		/// <param name="includeSections">One of the <see cref="T:System.Security.AccessControl.AccessControlSections" /> enumeration values that specifies the sections of the security descriptor (access rules, audit rules, owner, primary group) of the securable object to save.</param>
		[MonoTODO]
		protected virtual void Persist(SafeHandle handle, AccessControlSections includeSections)
		{
			throw new NotImplementedException();
		}

		/// <summary>Saves the specified sections of the security descriptor associated with this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object to permanent storage. We recommend that the values of the <paramref name="includeSections" /> parameters passed to the constructor and persist methods be identical. For more information, see Remarks.</summary>
		/// <param name="name">The name used to retrieve the persisted information.</param>
		/// <param name="includeSections">One of the <see cref="T:System.Security.AccessControl.AccessControlSections" /> enumeration values that specifies the sections of the security descriptor (access rules, audit rules, owner, primary group) of the securable object to save.</param>
		[MonoTODO]
		protected virtual void Persist(string name, AccessControlSections includeSections)
		{
			throw new NotImplementedException();
		}

		/// <summary>Saves the specified sections of the security descriptor associated with this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object to permanent storage. We recommend that the values of the <paramref name="includeSections" /> parameters passed to the constructor and persist methods be identical. For more information, see Remarks.</summary>
		/// <param name="enableOwnershipPrivilege">true to enable the privilege that allows the caller to take ownership of the object.</param>
		/// <param name="name">The name used to retrieve the persisted information.</param>
		/// <param name="includeSections">One of the <see cref="T:System.Security.AccessControl.AccessControlSections" /> enumeration values that specifies the sections of the security descriptor (access rules, audit rules, owner, primary group) of the securable object to save.</param>
		[MonoTODO]
		protected virtual void Persist(bool enableOwnershipPrivilege, string name, AccessControlSections includeSections)
		{
			throw new NotImplementedException();
		}

		/// <summary>Locks this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object for read access.</summary>
		[MonoTODO]
		protected void ReadLock()
		{
			throw new NotImplementedException();
		}

		/// <summary>Unlocks this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object for read access.</summary>
		[MonoTODO]
		protected void ReadUnlock()
		{
			throw new NotImplementedException();
		}

		/// <summary>Locks this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object for write access.</summary>
		[MonoTODO]
		protected void WriteLock()
		{
			throw new NotImplementedException();
		}

		/// <summary>Unlocks this <see cref="T:System.Security.AccessControl.ObjectSecurity" /> object for write access.</summary>
		[MonoTODO]
		protected void WriteUnlock()
		{
			throw new NotImplementedException();
		}
	}
}
