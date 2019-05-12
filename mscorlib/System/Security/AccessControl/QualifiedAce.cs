using System;

namespace System.Security.AccessControl
{
	/// <summary>Represents an Access Control Entry (ACE) that contains a qualifier. The qualifier, represented by an <see cref="T:System.Security.AccessControl.AceQualifier" /> object, specifies whether the ACE allows access, denies access, causes system audits, or causes system alarms. The <see cref="T:System.Security.AccessControl.QualifiedAce" /> class is the abstract base class for the <see cref="T:System.Security.AccessControl.CommonAce" /> and <see cref="T:System.Security.AccessControl.ObjectAce" /> classes.</summary>
	public abstract class QualifiedAce : KnownAce
	{
		private AceQualifier ace_qualifier;

		private bool is_callback;

		private byte[] opaque;

		internal QualifiedAce(InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AceQualifier aceQualifier, bool isCallback, byte[] opaque) : base(inheritanceFlags, propagationFlags)
		{
			this.ace_qualifier = aceQualifier;
			this.is_callback = isCallback;
			this.SetOpaque(opaque);
		}

		/// <summary>Gets a value that specifies whether the ACE allows access, denies access, causes system audits, or causes system alarms.</summary>
		/// <returns>A value that specifies whether the ACE allows access, denies access, causes system audits, or causes system alarms.</returns>
		public AceQualifier AceQualifier
		{
			get
			{
				return this.ace_qualifier;
			}
		}

		/// <summary>Specifies whether this <see cref="T:System.Security.AccessControl.QualifiedAce" /> object contains callback data.</summary>
		/// <returns>true if this <see cref="T:System.Security.AccessControl.QualifiedAce" /> object contains callback data; otherwise, false.</returns>
		public bool IsCallback
		{
			get
			{
				return this.is_callback;
			}
		}

		/// <summary>Gets the length of the opaque callback data associated with this <see cref="T:System.Security.AccessControl.QualifiedAce" /> object. This property is valid only for callback Access Control Entries (ACEs).</summary>
		/// <returns>The length of the opaque callback data.</returns>
		public int OpaqueLength
		{
			get
			{
				return this.opaque.Length;
			}
		}

		/// <summary>Returns the opaque callback data associated with this <see cref="T:System.Security.AccessControl.QualifiedAce" /> object. </summary>
		/// <returns>An array of byte values that represents the opaque callback data associated with this <see cref="T:System.Security.AccessControl.QualifiedAce" /> object.</returns>
		public byte[] GetOpaque()
		{
			return (byte[])this.opaque.Clone();
		}

		/// <summary>Sets the opaque callback data associated with this <see cref="T:System.Security.AccessControl.QualifiedAce" /> object.</summary>
		/// <param name="opaque">An array of byte values that represents the opaque callback data for this <see cref="T:System.Security.AccessControl.QualifiedAce" /> object.</param>
		public void SetOpaque(byte[] opaque)
		{
			if (opaque == null)
			{
				throw new ArgumentNullException("opaque");
			}
			this.opaque = (byte[])opaque.Clone();
		}
	}
}
