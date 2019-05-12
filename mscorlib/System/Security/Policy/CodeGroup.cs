using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Policy
{
	/// <summary>Represents the abstract base class from which all implementations of code groups must derive.</summary>
	[ComVisible(true)]
	[Serializable]
	public abstract class CodeGroup
	{
		private PolicyStatement m_policy;

		private IMembershipCondition m_membershipCondition;

		private string m_description;

		private string m_name;

		private ArrayList m_children = new ArrayList();

		/// <summary>Initializes a new instance of <see cref="T:System.Security.Policy.CodeGroup" />.</summary>
		/// <param name="membershipCondition">A membership condition that tests evidence to determine whether this code group applies policy. </param>
		/// <param name="policy">The policy statement for the code group in the form of a permission set and attributes to grant code that matches the membership condition. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="membershipCondition" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The type of the <paramref name="membershipCondition" /> parameter is not valid.-or- The type of the <paramref name="policy" /> parameter is not valid. </exception>
		protected CodeGroup(IMembershipCondition membershipCondition, PolicyStatement policy)
		{
			if (membershipCondition == null)
			{
				throw new ArgumentNullException("membershipCondition");
			}
			if (policy != null)
			{
				this.m_policy = policy.Copy();
			}
			this.m_membershipCondition = membershipCondition.Copy();
		}

		internal CodeGroup(SecurityElement e, PolicyLevel level)
		{
			this.FromXml(e, level);
		}

		/// <summary>When overridden in a derived class, makes a deep copy of the current code group.</summary>
		/// <returns>An equivalent copy of the current code group, including its membership conditions and child code groups.</returns>
		public abstract CodeGroup Copy();

		/// <summary>When overridden in a derived class, gets the merge logic for the code group.</summary>
		/// <returns>A description of the merge logic for the code group.</returns>
		public abstract string MergeLogic { get; }

		/// <summary>When overridden in a derived class, resolves policy for the code group and its descendants for a set of evidence.</summary>
		/// <returns>A policy statement that consists of the permissions granted by the code group with optional attributes, or null if the code group does not apply (the membership condition does not match the specified evidence).</returns>
		/// <param name="evidence">The evidence for the assembly. </param>
		public abstract PolicyStatement Resolve(Evidence evidence);

		/// <summary>When overridden in a derived class, resolves matching code groups.</summary>
		/// <returns>A <see cref="T:System.Security.Policy.CodeGroup" /> that is the root of the tree of matching code groups.</returns>
		/// <param name="evidence">The evidence for the assembly. </param>
		public abstract CodeGroup ResolveMatchingCodeGroups(Evidence evidence);

		/// <summary>Gets or sets the policy statement associated with the code group.</summary>
		/// <returns>The policy statement for the code group.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public PolicyStatement PolicyStatement
		{
			get
			{
				return this.m_policy;
			}
			set
			{
				this.m_policy = value;
			}
		}

		/// <summary>Gets or sets the description of the code group.</summary>
		/// <returns>The description of the code group.</returns>
		public string Description
		{
			get
			{
				return this.m_description;
			}
			set
			{
				this.m_description = value;
			}
		}

		/// <summary>Gets or sets the code group's membership condition.</summary>
		/// <returns>The membership condition that determines to which evidence the code group is applicable.</returns>
		/// <exception cref="T:System.ArgumentNullException">An attempt is made to set this parameter to null. </exception>
		public IMembershipCondition MembershipCondition
		{
			get
			{
				return this.m_membershipCondition;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentException("value");
				}
				this.m_membershipCondition = value;
			}
		}

		/// <summary>Gets or sets the name of the code group.</summary>
		/// <returns>The name of the code group.</returns>
		public string Name
		{
			get
			{
				return this.m_name;
			}
			set
			{
				this.m_name = value;
			}
		}

		/// <summary>Gets or sets an ordered list of the child code groups of a code group.</summary>
		/// <returns>A list of child code groups.</returns>
		/// <exception cref="T:System.ArgumentNullException">An attempt is made to set this property to null. </exception>
		/// <exception cref="T:System.ArgumentException">An attempt is made to set this property with a list of children that are not <see cref="T:System.Security.Policy.CodeGroup" /> objects.</exception>
		public IList Children
		{
			get
			{
				return this.m_children;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_children = new ArrayList(value);
			}
		}

		/// <summary>Gets a string representation of the attributes of the policy statement for the code group.</summary>
		/// <returns>A string representation of the attributes of the policy statement for the code group.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public virtual string AttributeString
		{
			get
			{
				if (this.m_policy != null)
				{
					return this.m_policy.AttributeString;
				}
				return null;
			}
		}

		/// <summary>Gets the name of the named permission set for the code group.</summary>
		/// <returns>The name of a named permission set of the policy level.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public virtual string PermissionSetName
		{
			get
			{
				if (this.m_policy == null)
				{
					return null;
				}
				if (this.m_policy.PermissionSet is NamedPermissionSet)
				{
					return ((NamedPermissionSet)this.m_policy.PermissionSet).Name;
				}
				return null;
			}
		}

		/// <summary>Adds a child code group to the current code group.</summary>
		/// <param name="group">The code group to be added as a child. This new child code group is added to the end of the list. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="group" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="group" /> parameter is not a valid code group. </exception>
		public void AddChild(CodeGroup group)
		{
			if (group == null)
			{
				throw new ArgumentNullException("group");
			}
			this.m_children.Add(group.Copy());
		}

		/// <summary>Determines whether the specified code group is equivalent to the current code group.</summary>
		/// <returns>true if the specified code group is equivalent to the current code group; otherwise, false.</returns>
		/// <param name="o">The code group to compare with the current code group. </param>
		public override bool Equals(object o)
		{
			CodeGroup codeGroup = o as CodeGroup;
			return codeGroup != null && this.Equals(codeGroup, false);
		}

		/// <summary>Determines whether the specified code group is equivalent to the current code group, checking the child code groups as well, if specified.</summary>
		/// <returns>true if the specified code group is equivalent to the current code group; otherwise, false.</returns>
		/// <param name="cg">The code group to compare with the current code group. </param>
		/// <param name="compareChildren">true to compare child code groups, as well; otherwise, false. </param>
		public bool Equals(CodeGroup cg, bool compareChildren)
		{
			if (cg.Name != this.Name)
			{
				return false;
			}
			if (cg.Description != this.Description)
			{
				return false;
			}
			if (!cg.MembershipCondition.Equals(this.m_membershipCondition))
			{
				return false;
			}
			if (compareChildren)
			{
				int count = cg.Children.Count;
				if (this.Children.Count != count)
				{
					return false;
				}
				for (int i = 0; i < count; i++)
				{
					if (!((CodeGroup)this.Children[i]).Equals((CodeGroup)cg.Children[i], false))
					{
						return false;
					}
				}
			}
			return true;
		}

		/// <summary>Removes the specified child code group.</summary>
		/// <param name="group">The code group to be removed as a child. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="group" /> parameter is not an immediate child code group of the current code group. </exception>
		public void RemoveChild(CodeGroup group)
		{
			if (group != null)
			{
				this.m_children.Remove(group);
			}
		}

		/// <summary>Gets the hash code of the current code group.</summary>
		/// <returns>The hash code of the current code group.</returns>
		public override int GetHashCode()
		{
			int num = this.m_membershipCondition.GetHashCode();
			if (this.m_policy != null)
			{
				num += this.m_policy.GetHashCode();
			}
			return num;
		}

		/// <summary>Reconstructs a security object with a given state from an XML encoding.</summary>
		/// <param name="e">The XML encoding to use to reconstruct the security object. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="e" /> parameter is null. </exception>
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		/// <summary>Reconstructs a security object with a given state and policy level from an XML encoding.</summary>
		/// <param name="e">The XML encoding to use to reconstruct the security object. </param>
		/// <param name="level">The policy level within which the code group exists. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="e" /> parameter is null. </exception>
		public void FromXml(SecurityElement e, PolicyLevel level)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			string text = e.Attribute("PermissionSetName");
			PermissionSet permissionSet;
			if (text != null && level != null)
			{
				permissionSet = level.GetNamedPermissionSet(text);
			}
			else
			{
				SecurityElement securityElement = e.SearchForChildByTag("PermissionSet");
				if (securityElement != null)
				{
					Type type = Type.GetType(securityElement.Attribute("class"));
					permissionSet = (PermissionSet)Activator.CreateInstance(type, true);
					permissionSet.FromXml(securityElement);
				}
				else
				{
					permissionSet = new PermissionSet(new PermissionSet(PermissionState.None));
				}
			}
			this.m_policy = new PolicyStatement(permissionSet);
			this.m_children.Clear();
			if (e.Children != null && e.Children.Count > 0)
			{
				foreach (object obj in e.Children)
				{
					SecurityElement securityElement2 = (SecurityElement)obj;
					if (securityElement2.Tag == "CodeGroup")
					{
						this.AddChild(CodeGroup.CreateFromXml(securityElement2, level));
					}
				}
			}
			this.m_membershipCondition = null;
			SecurityElement securityElement3 = e.SearchForChildByTag("IMembershipCondition");
			if (securityElement3 != null)
			{
				string text2 = securityElement3.Attribute("class");
				Type type2 = Type.GetType(text2);
				if (type2 == null)
				{
					type2 = Type.GetType("System.Security.Policy." + text2);
				}
				this.m_membershipCondition = (IMembershipCondition)Activator.CreateInstance(type2, true);
				this.m_membershipCondition.FromXml(securityElement3, level);
			}
			this.m_name = e.Attribute("Name");
			this.m_description = e.Attribute("Description");
			this.ParseXml(e, level);
		}

		/// <summary>When overridden in a derived class, reconstructs properties and internal state specific to a derived code group from the specified <see cref="T:System.Security.SecurityElement" />.</summary>
		/// <param name="e">The XML encoding to use to reconstruct the security object. </param>
		/// <param name="level">The policy level within which the code group exists. </param>
		protected virtual void ParseXml(SecurityElement e, PolicyLevel level)
		{
		}

		/// <summary>Creates an XML encoding of the security object and its current state.</summary>
		/// <returns>An XML encoding of the security object, including any state information.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		/// <summary>Creates an XML encoding of the security object, its current state, and the policy level within which the code exists.</summary>
		/// <returns>An XML encoding of the security object, including any state information.</returns>
		/// <param name="level">The policy level within which the code group exists. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public SecurityElement ToXml(PolicyLevel level)
		{
			SecurityElement securityElement = new SecurityElement("CodeGroup");
			securityElement.AddAttribute("class", base.GetType().AssemblyQualifiedName);
			securityElement.AddAttribute("version", "1");
			if (this.Name != null)
			{
				securityElement.AddAttribute("Name", this.Name);
			}
			if (this.Description != null)
			{
				securityElement.AddAttribute("Description", this.Description);
			}
			if (this.MembershipCondition != null)
			{
				securityElement.AddChild(this.MembershipCondition.ToXml());
			}
			if (this.PolicyStatement != null && this.PolicyStatement.PermissionSet != null)
			{
				securityElement.AddChild(this.PolicyStatement.PermissionSet.ToXml());
			}
			foreach (object obj in this.Children)
			{
				CodeGroup codeGroup = (CodeGroup)obj;
				securityElement.AddChild(codeGroup.ToXml());
			}
			this.CreateXml(securityElement, level);
			return securityElement;
		}

		/// <summary>When overridden in a derived class, serializes properties and internal state specific to a derived code group and adds the serialization to the specified <see cref="T:System.Security.SecurityElement" />.</summary>
		/// <param name="element">The XML encoding to which to add the serialization. </param>
		/// <param name="level">The policy level within which the code group exists. </param>
		protected virtual void CreateXml(SecurityElement element, PolicyLevel level)
		{
		}

		internal static CodeGroup CreateFromXml(SecurityElement se, PolicyLevel level)
		{
			string text = se.Attribute("class");
			string text2 = text;
			int num = text2.IndexOf(",");
			if (num > 0)
			{
				text2 = text2.Substring(0, num);
			}
			num = text2.LastIndexOf(".");
			if (num > 0)
			{
				text2 = text2.Substring(num + 1);
			}
			string text3 = text2;
			switch (text3)
			{
			case "FileCodeGroup":
				return new FileCodeGroup(se, level);
			case "FirstMatchCodeGroup":
				return new FirstMatchCodeGroup(se, level);
			case "NetCodeGroup":
				return new NetCodeGroup(se, level);
			case "UnionCodeGroup":
				return new UnionCodeGroup(se, level);
			}
			Type type = Type.GetType(text);
			CodeGroup codeGroup = (CodeGroup)Activator.CreateInstance(type, true);
			codeGroup.FromXml(se, level);
			return codeGroup;
		}
	}
}
