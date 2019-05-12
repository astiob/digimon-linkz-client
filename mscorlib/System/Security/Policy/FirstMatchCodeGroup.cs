using System;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	/// <summary>Allows security policy to be defined by the union of the policy statement of a code group and that of the first child code group that matches. This class cannot be inherited.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class FirstMatchCodeGroup : CodeGroup
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Policy.FirstMatchCodeGroup" /> class.</summary>
		/// <param name="membershipCondition">A membership condition that tests evidence to determine whether this code group applies policy. </param>
		/// <param name="policy">The policy statement for the code group in the form of a permission set and attributes to grant code that matches the membership condition. </param>
		/// <exception cref="T:System.ArgumentException">The type of the <paramref name="membershipCondition" /> parameter is not valid.-or- The type of the <paramref name="policy" /> parameter is not valid. </exception>
		public FirstMatchCodeGroup(IMembershipCondition membershipCondition, PolicyStatement policy) : base(membershipCondition, policy)
		{
		}

		internal FirstMatchCodeGroup(SecurityElement e, PolicyLevel level) : base(e, level)
		{
		}

		/// <summary>Gets the merge logic.</summary>
		/// <returns>The string "First Match".</returns>
		public override string MergeLogic
		{
			get
			{
				return "First Match";
			}
		}

		/// <summary>Makes a deep copy of the code group.</summary>
		/// <returns>An equivalent copy of the code group, including its membership conditions and child code groups.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public override CodeGroup Copy()
		{
			FirstMatchCodeGroup firstMatchCodeGroup = this.CopyNoChildren();
			foreach (object obj in base.Children)
			{
				CodeGroup codeGroup = (CodeGroup)obj;
				firstMatchCodeGroup.AddChild(codeGroup.Copy());
			}
			return firstMatchCodeGroup;
		}

		/// <summary>Resolves policy for the code group and its descendants for a set of evidence.</summary>
		/// <returns>A policy statement consisting of the permissions granted by the code group with optional attributes, or null if the code group does not apply (the membership condition does not match the specified evidence).</returns>
		/// <param name="evidence">The evidence for the assembly. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="evidence" /> parameter is null. </exception>
		/// <exception cref="T:System.Security.Policy.PolicyException">More than one code group (including the parent code group and any child code groups) is marked <see cref="F:System.Security.Policy.PolicyStatementAttribute.Exclusive" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public override PolicyStatement Resolve(Evidence evidence)
		{
			if (evidence == null)
			{
				throw new ArgumentNullException("evidence");
			}
			if (!base.MembershipCondition.Check(evidence))
			{
				return null;
			}
			foreach (object obj in base.Children)
			{
				CodeGroup codeGroup = (CodeGroup)obj;
				PolicyStatement policyStatement = codeGroup.Resolve(evidence);
				if (policyStatement != null)
				{
					return policyStatement;
				}
			}
			return base.PolicyStatement;
		}

		/// <summary>Resolves matching code groups.</summary>
		/// <returns>A <see cref="T:System.Security.Policy.CodeGroup" /> that is the root of the tree of matching code groups.</returns>
		/// <param name="evidence">The evidence for the assembly. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="evidence" /> parameter is null. </exception>
		public override CodeGroup ResolveMatchingCodeGroups(Evidence evidence)
		{
			if (evidence == null)
			{
				throw new ArgumentNullException("evidence");
			}
			if (!base.MembershipCondition.Check(evidence))
			{
				return null;
			}
			foreach (object obj in base.Children)
			{
				CodeGroup codeGroup = (CodeGroup)obj;
				if (codeGroup.Resolve(evidence) != null)
				{
					return codeGroup.Copy();
				}
			}
			return this.CopyNoChildren();
		}

		private FirstMatchCodeGroup CopyNoChildren()
		{
			return new FirstMatchCodeGroup(base.MembershipCondition, base.PolicyStatement)
			{
				Name = base.Name,
				Description = base.Description
			};
		}
	}
}
