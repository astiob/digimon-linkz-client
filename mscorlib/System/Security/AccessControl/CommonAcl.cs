using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	/// <summary>Represents an access control list (ACL) and is the base class for the <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> and <see cref="T:System.Security.AccessControl.SystemAcl" /> classes.</summary>
	public abstract class CommonAcl : GenericAcl
	{
		private const int default_capacity = 10;

		private bool is_container;

		private bool is_ds;

		private byte revision;

		private List<GenericAce> list;

		internal CommonAcl(bool isContainer, bool isDS, byte revision) : this(isContainer, isDS, revision, 10)
		{
		}

		internal CommonAcl(bool isContainer, bool isDS, byte revision, int capacity)
		{
			this.is_container = isContainer;
			this.is_ds = isDS;
			this.revision = revision;
			this.list = new List<GenericAce>(capacity);
		}

		/// <summary>Gets the length, in bytes, of the binary representation of the current <see cref="T:System.Security.AccessControl.CommonAcl" /> object. This length should be used before marshaling the access control list (ACL) into a binary array by using the <see cref="M:System.Security.AccessControl.CommonAcl.GetBinaryForm" /> method.</summary>
		/// <returns>The length, in bytes, of the binary representation of the current <see cref="T:System.Security.AccessControl.CommonAcl" /> object.</returns>
		[MonoTODO]
		public sealed override int BinaryLength
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>Gets the number of access control entries (ACEs) in the current <see cref="T:System.Security.AccessControl.CommonAcl" /> object.</summary>
		/// <returns>The number of ACEs in the current <see cref="T:System.Security.AccessControl.CommonAcl" /> object.</returns>
		public sealed override int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		/// <summary>Gets a Boolean value that specifies whether the access control entries (ACEs) in the current <see cref="T:System.Security.AccessControl.CommonAcl" /> object are in canonical order.</summary>
		/// <returns>true if the ACEs in the current <see cref="T:System.Security.AccessControl.CommonAcl" /> object are in canonical order; otherwise, false.</returns>
		[MonoTODO]
		public bool IsCanonical
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>Sets whether the <see cref="T:System.Security.AccessControl.CommonAcl" /> object is a container. </summary>
		/// <returns>true if the current <see cref="T:System.Security.AccessControl.CommonAcl" /> object is a container.</returns>
		public bool IsContainer
		{
			get
			{
				return this.is_container;
			}
		}

		/// <summary>Sets whether the current <see cref="T:System.Security.AccessControl.CommonAcl" /> object is a directory object access control list (ACL).</summary>
		/// <returns>true if the current <see cref="T:System.Security.AccessControl.CommonAcl" /> object is a directory object ACL.</returns>
		public bool IsDS
		{
			get
			{
				return this.is_ds;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Security.AccessControl.CommonAce" /> at the specified index.</summary>
		/// <returns>The <see cref="T:System.Security.AccessControl.CommonAce" /> at the specified index.</returns>
		/// <param name="index">The zero-based index of the <see cref="T:System.Security.AccessControl.CommonAce" /> to get or set.</param>
		public sealed override GenericAce this[int index]
		{
			get
			{
				return this.list[index];
			}
			set
			{
				this.list[index] = value;
			}
		}

		/// <summary>Gets the revision level of the <see cref="T:System.Security.AccessControl.CommonAcl" />.</summary>
		/// <returns>A byte value that specifies the revision level of the <see cref="T:System.Security.AccessControl.CommonAcl" />.</returns>
		public sealed override byte Revision
		{
			get
			{
				return this.revision;
			}
		}

		/// <summary>Marshals the contents of the <see cref="T:System.Security.AccessControl.CommonAcl" /> object into the specified byte array beginning at the specified offset.</summary>
		/// <param name="binaryForm">The byte array into which the contents of the <see cref="T:System.Security.AccessControl.CommonAcl" /> is marshaled.</param>
		/// <param name="offset">The offset at which to start marshaling.</param>
		[MonoTODO]
		public sealed override void GetBinaryForm(byte[] binaryForm, int offset)
		{
			throw new NotImplementedException();
		}

		/// <summary>Removes all access control entries (ACEs) contained by this <see cref="T:System.Security.AccessControl.CommonAcl" /> object that are associated with the specified <see cref="T:System.Security.Principal.SecurityIdentifier" /> object.</summary>
		/// <param name="sid">The <see cref="T:System.Security.Principal.SecurityIdentifier" /> object to check for.</param>
		[MonoTODO]
		public void Purge(SecurityIdentifier sid)
		{
			throw new NotImplementedException();
		}

		/// <summary>Removes all inherited access control entries (ACEs) from this <see cref="T:System.Security.AccessControl.CommonAcl" /> object.</summary>
		[MonoTODO]
		public void RemoveInheritedAces()
		{
			throw new NotImplementedException();
		}
	}
}
