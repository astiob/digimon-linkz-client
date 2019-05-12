using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	/// <summary>Represents an audit rule for a cryptographic key. An audit rule represents a combination of a user's identity and an access mask. An audit rule also contains information about the how the rule is inherited by child objects, how that inheritance is propagated, and for what conditions it is audited.</summary>
	public sealed class CryptoKeyAuditRule : AuditRule
	{
		private CryptoKeyRights rights;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.AccessControl.CryptoKeyAuditRule" /> class using the specified values. </summary>
		/// <param name="identity">The identity to which the audit rule applies. This parameter must be an object that can be cast as a <see cref="T:System.Security.Principal.SecurityIdentifier" />.</param>
		/// <param name="cryptoKeyRights">The cryptographic key operation for which this audit rule generates audits.</param>
		/// <param name="flags">The conditions that generate audits.</param>
		public CryptoKeyAuditRule(IdentityReference identity, CryptoKeyRights cryptoKeyRights, AuditFlags flags) : base(identity, 0, false, InheritanceFlags.None, PropagationFlags.None, flags)
		{
			this.rights = cryptoKeyRights;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.AccessControl.CryptoKeyAuditRule" /> class using the specified values. </summary>
		/// <param name="identity">The identity to which the audit rule applies.</param>
		/// <param name="cryptoKeyRights">The cryptographic key operation for which this audit rule generates audits.</param>
		/// <param name="flags">The conditions that generate audits.</param>
		public CryptoKeyAuditRule(string identity, CryptoKeyRights cryptoKeyRights, AuditFlags flags) : this(new SecurityIdentifier(identity), cryptoKeyRights, flags)
		{
		}

		/// <summary>Gets the cryptographic key operation for which this audit rule generates audits.</summary>
		/// <returns>The cryptographic key operation for which this audit rule generates audits.</returns>
		public CryptoKeyRights CryptoKeyRights
		{
			get
			{
				return this.rights;
			}
		}
	}
}
