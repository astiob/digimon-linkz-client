using System;

namespace System.Security.AccessControl
{
	/// <summary>Represents an Access Control Entry (ACE), and is the base class for all other ACE classes.</summary>
	public abstract class GenericAce
	{
		private InheritanceFlags inheritance;

		private PropagationFlags propagation;

		private AceFlags aceflags;

		private AceType ace_type;

		internal GenericAce(InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
		{
			this.inheritance = inheritanceFlags;
			this.propagation = propagationFlags;
		}

		internal GenericAce(AceType type)
		{
			if (type <= AceType.SystemAlarmCallbackObject)
			{
				throw new ArgumentOutOfRangeException("type");
			}
			this.ace_type = type;
		}

		/// <summary>Gets or sets the <see cref="T:System.Security.AccessControl.AceFlags" /> associated with this <see cref="T:System.Security.AccessControl.GenericAce" /> object.</summary>
		/// <returns>The <see cref="T:System.Security.AccessControl.AceFlags" /> associated with this <see cref="T:System.Security.AccessControl.GenericAce" /> object.</returns>
		public AceFlags AceFlags
		{
			get
			{
				return this.aceflags;
			}
			set
			{
				this.aceflags = value;
			}
		}

		/// <summary>Gets the type of this Access Control Entry (ACE).</summary>
		/// <returns>The type of this ACE.</returns>
		public AceType AceType
		{
			get
			{
				return this.ace_type;
			}
		}

		/// <summary>Gets the audit information associated with this Access Control Entry (ACE).</summary>
		/// <returns>The audit information associated with this Access Control Entry (ACE).</returns>
		public AuditFlags AuditFlags
		{
			get
			{
				AuditFlags auditFlags = AuditFlags.None;
				if ((byte)(this.aceflags & AceFlags.SuccessfulAccess) != 0)
				{
					auditFlags |= AuditFlags.Success;
				}
				if ((byte)(this.aceflags & AceFlags.FailedAccess) != 0)
				{
					auditFlags |= AuditFlags.Failure;
				}
				return auditFlags;
			}
		}

		/// <summary>Gets the length, in bytes, of the binary representation of the current <see cref="T:System.Security.AccessControl.GenericAce" /> object. This length should be used before marshaling the ACL into a binary array with the <see cref="M:System.Security.AccessControl.GenericAce.GetBinaryForm" /> method.</summary>
		/// <returns>The length, in bytes, of the binary representation of the current <see cref="T:System.Security.AccessControl.GenericAce" /> object.</returns>
		public abstract int BinaryLength { get; }

		/// <summary>Gets flags that specify the inheritance properties of this Access Control Entry (ACE).</summary>
		/// <returns>Flags that specify the inheritance properties of this ACE.</returns>
		public InheritanceFlags InheritanceFlags
		{
			get
			{
				return this.inheritance;
			}
		}

		/// <summary>Gets a Boolean value that specifies whether this Access Control Entry (ACE) is inherited or is set explicitly.</summary>
		/// <returns>true if this ACE is inherited; otherwise, false.</returns>
		[MonoTODO]
		public bool IsInherited
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets flags that specify the inheritance propagation properties of this Access Control Entry (ACE).</summary>
		/// <returns>Flags that specify the inheritance propagation properties of this ACE.</returns>
		public PropagationFlags PropagationFlags
		{
			get
			{
				return this.propagation;
			}
		}

		/// <summary>Creates a deep copy of this Access Control Entry (ACE).</summary>
		/// <returns>The <see cref="T:System.Security.AccessControl.GenericAce" /> object that this method creates.</returns>
		[MonoTODO]
		public GenericAce Copy()
		{
			throw new NotImplementedException();
		}

		/// <summary>Creates a <see cref="T:System.Security.AccessControl.GenericAce" /> object from the specified binary data.</summary>
		/// <returns>The <see cref="T:System.Security.AccessControl.GenericAce" /> object this method creates.</returns>
		/// <param name="binaryForm">The binary data from which to create the new <see cref="T:System.Security.AccessControl.GenericAce" /> object.</param>
		/// <param name="offset">The offset at which to begin unmarshaling.</param>
		[MonoTODO]
		public static GenericAce CreateFromBinaryForm(byte[] binaryForm, int offset)
		{
			throw new NotImplementedException();
		}

		/// <summary>Determines whether the specified <see cref="T:System.Security.AccessControl.GenericAce" /> object is equal to the current <see cref="T:System.Security.AccessControl.GenericAce" /> object.</summary>
		/// <returns>true if the specified <see cref="T:System.Security.AccessControl.GenericAce" /> object is equal to the current <see cref="T:System.Security.AccessControl.GenericAce" /> object; otherwise, false.</returns>
		/// <param name="o">The <see cref="T:System.Security.AccessControl.GenericAce" /> object to compare to the current <see cref="T:System.Security.AccessControl.GenericAce" /> object.</param>
		[MonoTODO]
		public sealed override bool Equals(object o)
		{
			throw new NotImplementedException();
		}

		/// <summary>Marshals the contents of the <see cref="T:System.Security.AccessControl.GenericAce" /> object into the specified byte array beginning at the specified offset.</summary>
		/// <param name="binaryForm">The byte array into which the contents of the <see cref="T:System.Security.AccessControl.GenericAce" /> is marshaled.</param>
		/// <param name="offset">The offset at which to start marshaling.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is negative or too high to allow the entire <see cref="T:System.Security.AccessControl.GenericAcl" /> to be copied into <paramref name="array" />.</exception>
		[MonoTODO]
		public abstract void GetBinaryForm(byte[] binaryForm, int offset);

		/// <summary>Serves as a hash function for the <see cref="T:System.Security.AccessControl.GenericAce" /> class. The  <see cref="M:System.Security.AccessControl.GenericAce.GetHashCode" /> method is suitable for use in hashing algorithms and data structures like a hash table.</summary>
		/// <returns>A hash code for the current <see cref="T:System.Security.AccessControl.GenericAce" /> object.</returns>
		[MonoTODO]
		public sealed override int GetHashCode()
		{
			throw new NotImplementedException();
		}

		/// <summary>Determines whether the specified <see cref="T:System.Security.AccessControl.GenericAce" /> objects are considered equal.</summary>
		/// <returns>true if the two <see cref="T:System.Security.AccessControl.GenericAce" /> objects are equal; otherwise, false.</returns>
		/// <param name="left">The first <see cref="T:System.Security.AccessControl.GenericAce" /> object to compare.</param>
		/// <param name="right">The second <see cref="T:System.Security.AccessControl.GenericAce" /> to compare.</param>
		[MonoTODO]
		public static bool operator ==(GenericAce left, GenericAce right)
		{
			throw new NotImplementedException();
		}

		/// <summary>Determines whether the specified <see cref="T:System.Security.AccessControl.GenericAce" /> objects are considered unequal.</summary>
		/// <returns>true if the two <see cref="T:System.Security.AccessControl.GenericAce" /> objects are unequal; otherwise, false.</returns>
		/// <param name="left">The first <see cref="T:System.Security.AccessControl.GenericAce" /> object to compare.</param>
		/// <param name="right">The second <see cref="T:System.Security.AccessControl.GenericAce" /> to compare.</param>
		[MonoTODO]
		public static bool operator !=(GenericAce left, GenericAce right)
		{
			throw new NotImplementedException();
		}
	}
}
