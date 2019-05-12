using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	/// <summary>Encapsulates all Access Control Entry (ACE) types currently defined by Microsoft Corporation. All <see cref="T:System.Security.AccessControl.KnownAce" /> objects contain a 32-bit access mask and a <see cref="T:System.Security.Principal.SecurityIdentifier" /> object.</summary>
	public abstract class KnownAce : GenericAce
	{
		private int access_mask;

		private SecurityIdentifier identifier;

		internal KnownAce(InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags) : base(inheritanceFlags, propagationFlags)
		{
		}

		/// <summary>Gets or sets the access mask for this <see cref="T:System.Security.AccessControl.KnownAce" /> object.</summary>
		/// <returns>The access mask for this <see cref="T:System.Security.AccessControl.KnownAce" /> object.</returns>
		public int AccessMask
		{
			get
			{
				return this.access_mask;
			}
			set
			{
				this.access_mask = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Security.Principal.SecurityIdentifier" /> object associated with this <see cref="T:System.Security.AccessControl.KnownAce" /> object.</summary>
		/// <returns>The <see cref="T:System.Security.Principal.SecurityIdentifier" /> object associated with this <see cref="T:System.Security.AccessControl.KnownAce" /> object.</returns>
		public SecurityIdentifier SecurityIdentifier
		{
			get
			{
				return this.identifier;
			}
			set
			{
				this.identifier = value;
			}
		}
	}
}
