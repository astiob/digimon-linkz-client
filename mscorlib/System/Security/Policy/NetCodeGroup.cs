using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Policy
{
	/// <summary>Grants Web permission to the site from which the assembly was downloaded. This class cannot be inherited.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class NetCodeGroup : CodeGroup
	{
		/// <summary>Contains a value used to specify connection access for code with an unknown or unrecognized origin scheme.</summary>
		public static readonly string AbsentOriginScheme = string.Empty;

		/// <summary>Contains a value used to specify any other unspecified origin scheme.</summary>
		public static readonly string AnyOtherOriginScheme = "*";

		private Hashtable _rules = new Hashtable();

		private int _hashcode;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Policy.NetCodeGroup" /> class.</summary>
		/// <param name="membershipCondition">A membership condition that tests evidence to determine whether this code group applies code access security policy. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="membershipCondition" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The type of the <paramref name="membershipCondition" /> parameter is not valid. </exception>
		public NetCodeGroup(IMembershipCondition membershipCondition) : base(membershipCondition, null)
		{
		}

		internal NetCodeGroup(SecurityElement e, PolicyLevel level) : base(e, level)
		{
		}

		/// <summary>Gets a string representation of the attributes of the policy statement for the code group.</summary>
		/// <returns>Always null.</returns>
		public override string AttributeString
		{
			get
			{
				return null;
			}
		}

		/// <summary>Gets the logic to use for merging groups.</summary>
		/// <returns>The string "Union".</returns>
		public override string MergeLogic
		{
			get
			{
				return "Union";
			}
		}

		/// <summary>Gets the name of the <see cref="T:System.Security.NamedPermissionSet" /> for the code group.</summary>
		/// <returns>Always the string "Same site Web."</returns>
		public override string PermissionSetName
		{
			get
			{
				return "Same site Web";
			}
		}

		/// <summary>Adds the specified connection access to the current code group.</summary>
		/// <param name="originScheme">A <see cref="T:System.String" /> containing the scheme to match against the code's scheme.</param>
		/// <param name="connectAccess">A <see cref="T:System.Security.Policy.CodeConnectAccess" /> that specifies the scheme and port code can use to connect back to its origin server.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="originScheme" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="originScheme" /> contains characters that are not permitted in schemes.-or-<paramref name="originScheme" /> = <see cref="F:System.Security.Policy.NetCodeGroup.AbsentOriginScheme" /> and <paramref name="connectAccess" /> specifies <see cref="F:System.Security.Policy.CodeConnectAccess.OriginScheme" /> as its scheme.</exception>
		[MonoTODO("(2.0) missing validations")]
		public void AddConnectAccess(string originScheme, CodeConnectAccess connectAccess)
		{
			if (originScheme == null)
			{
				throw new ArgumentException("originScheme");
			}
			if (originScheme == NetCodeGroup.AbsentOriginScheme && connectAccess.Scheme == CodeConnectAccess.OriginScheme)
			{
				throw new ArgumentOutOfRangeException("connectAccess", Locale.GetText("Schema == CodeConnectAccess.OriginScheme"));
			}
			if (this._rules.ContainsKey(originScheme))
			{
				if (connectAccess != null)
				{
					CodeConnectAccess[] array = (CodeConnectAccess[])this._rules[originScheme];
					CodeConnectAccess[] array2 = new CodeConnectAccess[array.Length + 1];
					Array.Copy(array, 0, array2, 0, array.Length);
					array2[array.Length] = connectAccess;
					this._rules[originScheme] = array2;
				}
			}
			else
			{
				CodeConnectAccess[] value = new CodeConnectAccess[]
				{
					connectAccess
				};
				this._rules.Add(originScheme, value);
			}
		}

		/// <summary>Makes a deep copy of the current code group.</summary>
		/// <returns>An equivalent copy of the current code group, including its membership conditions and child code groups.</returns>
		public override CodeGroup Copy()
		{
			NetCodeGroup netCodeGroup = new NetCodeGroup(base.MembershipCondition);
			netCodeGroup.Name = base.Name;
			netCodeGroup.Description = base.Description;
			netCodeGroup.PolicyStatement = base.PolicyStatement;
			foreach (object obj in base.Children)
			{
				CodeGroup codeGroup = (CodeGroup)obj;
				netCodeGroup.AddChild(codeGroup.Copy());
			}
			return netCodeGroup;
		}

		private bool Equals(CodeConnectAccess[] rules1, CodeConnectAccess[] rules2)
		{
			for (int i = 0; i < rules1.Length; i++)
			{
				bool flag = false;
				for (int j = 0; j < rules2.Length; j++)
				{
					if (rules1[i].Equals(rules2[j]))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>Determines whether the specified code group is equivalent to the current code group.</summary>
		/// <returns>true if the specified code group is equivalent to the current code group; otherwise, false.</returns>
		/// <param name="o">The <see cref="T:System.Security.Policy.NetCodeGroup" /> object to compare with the current code group.</param>
		public override bool Equals(object o)
		{
			if (!base.Equals(o))
			{
				return false;
			}
			NetCodeGroup netCodeGroup = o as NetCodeGroup;
			if (netCodeGroup == null)
			{
				return false;
			}
			foreach (object obj in this._rules)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				CodeConnectAccess[] array = (CodeConnectAccess[])netCodeGroup._rules[dictionaryEntry.Key];
				bool flag;
				if (array != null)
				{
					flag = this.Equals((CodeConnectAccess[])dictionaryEntry.Value, array);
				}
				else
				{
					flag = (dictionaryEntry.Value == null);
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>Gets the connection access information for the current code group.</summary>
		/// <returns>A <see cref="T:System.Collections.DictionaryEntry" /> array containing connection access information.</returns>
		public DictionaryEntry[] GetConnectAccessRules()
		{
			DictionaryEntry[] array = new DictionaryEntry[this._rules.Count];
			this._rules.CopyTo(array, 0);
			return array;
		}

		/// <returns>The hash code of the current code group.</returns>
		public override int GetHashCode()
		{
			if (this._hashcode == 0)
			{
				this._hashcode = base.GetHashCode();
				foreach (object obj in this._rules)
				{
					CodeConnectAccess[] array = (CodeConnectAccess[])((DictionaryEntry)obj).Value;
					if (array != null)
					{
						foreach (CodeConnectAccess codeConnectAccess in array)
						{
							this._hashcode ^= codeConnectAccess.GetHashCode();
						}
					}
				}
			}
			return this._hashcode;
		}

		/// <summary>Resolves policy for the code group and its descendants for a set of evidence.</summary>
		/// <returns>A <see cref="T:System.Security.Policy.PolicyStatement" /> that consists of the permissions granted by the code group with optional attributes, or null if the code group does not apply (the membership condition does not match the specified evidence).</returns>
		/// <param name="evidence">The <see cref="T:System.Security.Policy.Evidence" /> for the assembly. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="evidence" /> parameter is null. </exception>
		/// <exception cref="T:System.Security.Policy.PolicyException">More than one code group (including the parent code group and any child code groups) is marked <see cref="F:System.Security.Policy.PolicyStatementAttribute.Exclusive" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
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
			PermissionSet permissionSet = null;
			if (base.PolicyStatement == null)
			{
				permissionSet = new PermissionSet(PermissionState.None);
			}
			else
			{
				permissionSet = base.PolicyStatement.PermissionSet.Copy();
			}
			if (base.Children.Count > 0)
			{
				foreach (object obj in base.Children)
				{
					CodeGroup codeGroup = (CodeGroup)obj;
					PolicyStatement policyStatement = codeGroup.Resolve(evidence);
					if (policyStatement != null)
					{
						permissionSet = permissionSet.Union(policyStatement.PermissionSet);
					}
				}
			}
			PolicyStatement policyStatement2 = base.PolicyStatement.Copy();
			policyStatement2.PermissionSet = permissionSet;
			return policyStatement2;
		}

		/// <summary>Removes all connection access information for the current code group.</summary>
		public void ResetConnectAccess()
		{
			this._rules.Clear();
		}

		/// <summary>Resolves matching code groups.</summary>
		/// <returns>A <see cref="T:System.Security.Policy.CodeGroup" />.</returns>
		/// <param name="evidence">The <see cref="T:System.Security.Policy.Evidence" /> for the assembly. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="evidence" /> parameter is null. </exception>
		public override CodeGroup ResolveMatchingCodeGroups(Evidence evidence)
		{
			if (evidence == null)
			{
				throw new ArgumentNullException("evidence");
			}
			CodeGroup codeGroup = null;
			if (base.MembershipCondition.Check(evidence))
			{
				codeGroup = this.Copy();
				foreach (object obj in base.Children)
				{
					CodeGroup codeGroup2 = (CodeGroup)obj;
					CodeGroup codeGroup3 = codeGroup2.ResolveMatchingCodeGroups(evidence);
					if (codeGroup3 != null)
					{
						codeGroup.AddChild(codeGroup3);
					}
				}
			}
			return codeGroup;
		}

		[MonoTODO("(2.0) Add new stuff (CodeConnectAccess) into XML")]
		protected override void CreateXml(SecurityElement element, PolicyLevel level)
		{
			base.CreateXml(element, level);
		}

		[MonoTODO("(2.0) Parse new stuff (CodeConnectAccess) from XML")]
		protected override void ParseXml(SecurityElement e, PolicyLevel level)
		{
			base.ParseXml(e, level);
		}
	}
}
