using Mono.Xml;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Policy
{
	/// <summary>Represents the security policy levels for the common language runtime. This class cannot be inherited.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class PolicyLevel
	{
		private string label;

		private CodeGroup root_code_group;

		private ArrayList full_trust_assemblies;

		private ArrayList named_permission_sets;

		private string _location;

		private PolicyLevelType _type;

		private Hashtable fullNames;

		private SecurityElement xml;

		internal PolicyLevel(string label, PolicyLevelType type)
		{
			this.label = label;
			this._type = type;
			this.full_trust_assemblies = new ArrayList();
			this.named_permission_sets = new ArrayList();
		}

		internal void LoadFromFile(string filename)
		{
			try
			{
				if (!File.Exists(filename))
				{
					string text = filename + ".default";
					if (File.Exists(text))
					{
						File.Copy(text, filename);
					}
				}
				if (File.Exists(filename))
				{
					using (StreamReader streamReader = File.OpenText(filename))
					{
						this.xml = this.FromString(streamReader.ReadToEnd());
					}
					try
					{
						SecurityManager.ResolvingPolicyLevel = this;
						this.FromXml(this.xml);
					}
					finally
					{
						SecurityManager.ResolvingPolicyLevel = this;
					}
				}
				else
				{
					this.CreateDefaultFullTrustAssemblies();
					this.CreateDefaultNamedPermissionSets();
					this.CreateDefaultLevel(this._type);
					this.Save();
				}
			}
			catch
			{
			}
			finally
			{
				this._location = filename;
			}
		}

		internal void LoadFromString(string xml)
		{
			this.FromXml(this.FromString(xml));
		}

		private SecurityElement FromString(string xml)
		{
			SecurityParser securityParser = new SecurityParser();
			securityParser.LoadXml(xml);
			SecurityElement securityElement = securityParser.ToXml();
			if (securityElement.Tag != "configuration")
			{
				throw new ArgumentException(Locale.GetText("missing <configuration> root element"));
			}
			SecurityElement securityElement2 = (SecurityElement)securityElement.Children[0];
			if (securityElement2.Tag != "mscorlib")
			{
				throw new ArgumentException(Locale.GetText("missing <mscorlib> tag"));
			}
			SecurityElement securityElement3 = (SecurityElement)securityElement2.Children[0];
			if (securityElement3.Tag != "security")
			{
				throw new ArgumentException(Locale.GetText("missing <security> tag"));
			}
			SecurityElement securityElement4 = (SecurityElement)securityElement3.Children[0];
			if (securityElement4.Tag != "policy")
			{
				throw new ArgumentException(Locale.GetText("missing <policy> tag"));
			}
			return (SecurityElement)securityElement4.Children[0];
		}

		/// <summary>Gets a list of <see cref="T:System.Security.Policy.StrongNameMembershipCondition" /> objects used to determine whether an assembly is a member of the group of assemblies used to evaluate security policy.</summary>
		/// <returns>A list of <see cref="T:System.Security.Policy.StrongNameMembershipCondition" /> objects used to determine whether an assembly is a member of the group of assemblies used to evaluate security policy. These assemblies are granted full trust during security policy evaluation of assemblies not in the list.</returns>
		[Obsolete("All GACed assemblies are now fully trusted and all permissions now succeed on fully trusted code.")]
		public IList FullTrustAssemblies
		{
			get
			{
				return this.full_trust_assemblies;
			}
		}

		/// <summary>Gets a descriptive label for the policy level.</summary>
		/// <returns>The label associated with the policy level.</returns>
		public string Label
		{
			get
			{
				return this.label;
			}
		}

		/// <summary>Gets a list of named permission sets defined for the policy level.</summary>
		/// <returns>A list of named permission sets defined for the policy level.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public IList NamedPermissionSets
		{
			get
			{
				return this.named_permission_sets;
			}
		}

		/// <summary>Gets or sets the root code group for the policy level.</summary>
		/// <returns>The <see cref="T:System.Security.Policy.CodeGroup" /> that is the root of the tree of policy level code groups.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value for <see cref="P:System.Security.Policy.PolicyLevel.RootCodeGroup" /> is null.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence, ControlPolicy" />
		/// </PermissionSet>
		public CodeGroup RootCodeGroup
		{
			get
			{
				return this.root_code_group;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.root_code_group = value;
			}
		}

		/// <summary>Gets the path where the policy file is stored.</summary>
		/// <returns>The path where the policy file is stored, or null if the <see cref="T:System.Security.Policy.PolicyLevel" /> does not have a storage location.</returns>
		public string StoreLocation
		{
			get
			{
				return this._location;
			}
		}

		/// <summary>Gets the type of the policy level.</summary>
		/// <returns>One of the <see cref="T:System.Security.PolicyLevelType" /> values.</returns>
		[ComVisible(false)]
		public PolicyLevelType Type
		{
			get
			{
				return this._type;
			}
		}

		/// <summary>Adds a <see cref="T:System.Security.Policy.StrongNameMembershipCondition" /> corresponding to the specified <see cref="T:System.Security.Policy.StrongName" /> to the list of <see cref="T:System.Security.Policy.StrongNameMembershipCondition" /> objects used to determine whether an assembly is a member of the group of assemblies that should not be evaluated.</summary>
		/// <param name="sn">The <see cref="T:System.Security.Policy.StrongName" /> used to create the <see cref="T:System.Security.Policy.StrongNameMembershipCondition" /> to add to the list of <see cref="T:System.Security.Policy.StrongNameMembershipCondition" /> objects used to determine whether an assembly is a member of the group of assemblies that should not be evaluated. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="sn" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Security.Policy.StrongName" /> specified by the <paramref name="sn" /> parameter already has full trust. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Obsolete("All GACed assemblies are now fully trusted and all permissions now succeed on fully trusted code.")]
		public void AddFullTrustAssembly(StrongName sn)
		{
			if (sn == null)
			{
				throw new ArgumentNullException("sn");
			}
			StrongNameMembershipCondition snMC = new StrongNameMembershipCondition(sn.PublicKey, sn.Name, sn.Version);
			this.AddFullTrustAssembly(snMC);
		}

		/// <summary>Adds the specified <see cref="T:System.Security.Policy.StrongNameMembershipCondition" /> to the list of <see cref="T:System.Security.Policy.StrongNameMembershipCondition" /> objects used to determine whether an assembly is a member of the group of assemblies that should not be evaluated.</summary>
		/// <param name="snMC">The <see cref="T:System.Security.Policy.StrongNameMembershipCondition" /> to add to the list of <see cref="T:System.Security.Policy.StrongNameMembershipCondition" /> objects used to determine whether an assembly is a member of the group of assemblies that should not be evaluated. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="snMC" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Security.Policy.StrongNameMembershipCondition" /> specified by the <paramref name="snMC" /> parameter already has full trust. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Obsolete("All GACed assemblies are now fully trusted and all permissions now succeed on fully trusted code.")]
		public void AddFullTrustAssembly(StrongNameMembershipCondition snMC)
		{
			if (snMC == null)
			{
				throw new ArgumentNullException("snMC");
			}
			foreach (object obj in this.full_trust_assemblies)
			{
				StrongNameMembershipCondition strongNameMembershipCondition = (StrongNameMembershipCondition)obj;
				if (strongNameMembershipCondition.Equals(snMC))
				{
					throw new ArgumentException(Locale.GetText("sn already has full trust."));
				}
			}
			this.full_trust_assemblies.Add(snMC);
		}

		/// <summary>Adds a <see cref="T:System.Security.NamedPermissionSet" /> to the current policy level.</summary>
		/// <param name="permSet">The <see cref="T:System.Security.NamedPermissionSet" /> to add to the current policy level. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="permSet" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="permSet" /> parameter has the same name as an existing <see cref="T:System.Security.NamedPermissionSet" /> in the <see cref="T:System.Security.Policy.PolicyLevel" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public void AddNamedPermissionSet(NamedPermissionSet permSet)
		{
			if (permSet == null)
			{
				throw new ArgumentNullException("permSet");
			}
			foreach (object obj in this.named_permission_sets)
			{
				NamedPermissionSet namedPermissionSet = (NamedPermissionSet)obj;
				if (permSet.Name == namedPermissionSet.Name)
				{
					throw new ArgumentException(Locale.GetText("This NamedPermissionSet is the same an existing NamedPermissionSet."));
				}
			}
			this.named_permission_sets.Add(permSet.Copy());
		}

		/// <summary>Replaces a <see cref="T:System.Security.NamedPermissionSet" /> in the current policy level with the specified <see cref="T:System.Security.PermissionSet" />.</summary>
		/// <returns>A copy of the <see cref="T:System.Security.NamedPermissionSet" /> that was replaced.</returns>
		/// <param name="name">The name of the <see cref="T:System.Security.NamedPermissionSet" /> to replace. </param>
		/// <param name="pSet">The <see cref="T:System.Security.PermissionSet" /> that replaces the <see cref="T:System.Security.NamedPermissionSet" /> specified by the <paramref name="name" /> parameter. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null.-or- The <paramref name="pSet" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="name" /> parameter is equal to the name of a reserved permission set.-or- The <see cref="T:System.Security.PermissionSet" /> specified by the <paramref name="pSet" /> parameter cannot be found. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public NamedPermissionSet ChangeNamedPermissionSet(string name, PermissionSet pSet)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (pSet == null)
			{
				throw new ArgumentNullException("pSet");
			}
			if (DefaultPolicies.ReservedNames.IsReserved(name))
			{
				throw new ArgumentException(Locale.GetText("Reserved name"));
			}
			foreach (object obj in this.named_permission_sets)
			{
				NamedPermissionSet namedPermissionSet = (NamedPermissionSet)obj;
				if (name == namedPermissionSet.Name)
				{
					this.named_permission_sets.Remove(namedPermissionSet);
					this.AddNamedPermissionSet(new NamedPermissionSet(name, pSet));
					return namedPermissionSet;
				}
			}
			throw new ArgumentException(Locale.GetText("PermissionSet not found"));
		}

		/// <summary>Creates a new policy level for use at the application domain policy level.</summary>
		/// <returns>The newly created <see cref="T:System.Security.Policy.PolicyLevel" />.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static PolicyLevel CreateAppDomainLevel()
		{
			UnionCodeGroup unionCodeGroup = new UnionCodeGroup(new AllMembershipCondition(), new PolicyStatement(DefaultPolicies.FullTrust));
			unionCodeGroup.Name = "All_Code";
			PolicyLevel policyLevel = new PolicyLevel("AppDomain", PolicyLevelType.AppDomain);
			policyLevel.RootCodeGroup = unionCodeGroup;
			policyLevel.Reset();
			return policyLevel;
		}

		/// <summary>Reconstructs a security object with a given state from an XML encoding.</summary>
		/// <param name="e">The XML encoding to use to reconstruct the security object. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="e" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Security.SecurityElement" /> specified by the <paramref name="e" /> parameter is invalid. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public void FromXml(SecurityElement e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			SecurityElement securityElement = e.SearchForChildByTag("SecurityClasses");
			if (securityElement != null && securityElement.Children != null && securityElement.Children.Count > 0)
			{
				this.fullNames = new Hashtable(securityElement.Children.Count);
				foreach (object obj in securityElement.Children)
				{
					SecurityElement securityElement2 = (SecurityElement)obj;
					this.fullNames.Add(securityElement2.Attributes["Name"], securityElement2.Attributes["Description"]);
				}
			}
			SecurityElement securityElement3 = e.SearchForChildByTag("FullTrustAssemblies");
			if (securityElement3 != null && securityElement3.Children != null && securityElement3.Children.Count > 0)
			{
				this.full_trust_assemblies.Clear();
				foreach (object obj2 in securityElement3.Children)
				{
					SecurityElement securityElement4 = (SecurityElement)obj2;
					if (securityElement4.Tag != "IMembershipCondition")
					{
						throw new ArgumentException(Locale.GetText("Invalid XML"));
					}
					string text = securityElement4.Attribute("class");
					if (text.IndexOf("StrongNameMembershipCondition") < 0)
					{
						throw new ArgumentException(Locale.GetText("Invalid XML - must be StrongNameMembershipCondition"));
					}
					this.full_trust_assemblies.Add(new StrongNameMembershipCondition(securityElement4));
				}
			}
			SecurityElement securityElement5 = e.SearchForChildByTag("CodeGroup");
			if (securityElement5 != null && securityElement5.Children != null && securityElement5.Children.Count > 0)
			{
				this.root_code_group = CodeGroup.CreateFromXml(securityElement5, this);
				SecurityElement securityElement6 = e.SearchForChildByTag("NamedPermissionSets");
				if (securityElement6 != null && securityElement6.Children != null && securityElement6.Children.Count > 0)
				{
					this.named_permission_sets.Clear();
					foreach (object obj3 in securityElement6.Children)
					{
						SecurityElement et = (SecurityElement)obj3;
						NamedPermissionSet namedPermissionSet = new NamedPermissionSet();
						namedPermissionSet.Resolver = this;
						namedPermissionSet.FromXml(et);
						this.named_permission_sets.Add(namedPermissionSet);
					}
				}
				return;
			}
			throw new ArgumentException(Locale.GetText("Missing Root CodeGroup"));
		}

		/// <summary>Returns the <see cref="T:System.Security.NamedPermissionSet" /> in the current policy level with the specified name.</summary>
		/// <returns>The <see cref="T:System.Security.NamedPermissionSet" /> in the current policy level with the specified name, if found; otherwise, null.</returns>
		/// <param name="name">The name of the <see cref="T:System.Security.NamedPermissionSet" /> to find. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		public NamedPermissionSet GetNamedPermissionSet(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			foreach (object obj in this.named_permission_sets)
			{
				NamedPermissionSet namedPermissionSet = (NamedPermissionSet)obj;
				if (namedPermissionSet.Name == name)
				{
					return (NamedPermissionSet)namedPermissionSet.Copy();
				}
			}
			return null;
		}

		/// <summary>Replaces the configuration file for this <see cref="T:System.Security.Policy.PolicyLevel" /> with the last backup (reflecting the state of policy prior to the last time it was saved) and returns it to the state of the last save.</summary>
		/// <exception cref="T:System.Security.Policy.PolicyException">The policy level does not have a valid configuration file. </exception>
		public void Recover()
		{
			if (this._location == null)
			{
				string text = Locale.GetText("Only file based policies may be recovered.");
				throw new PolicyException(text);
			}
			string text2 = this._location + ".backup";
			if (!File.Exists(text2))
			{
				string text3 = Locale.GetText("No policy backup exists.");
				throw new PolicyException(text3);
			}
			try
			{
				File.Copy(text2, this._location, true);
			}
			catch (Exception exception)
			{
				string text4 = Locale.GetText("Couldn't replace the policy file with it's backup.");
				throw new PolicyException(text4, exception);
			}
		}

		/// <summary>Removes an assembly with the specified <see cref="T:System.Security.Policy.StrongName" /> from the list of assemblies the policy level uses to evaluate policy.</summary>
		/// <param name="sn">The <see cref="T:System.Security.Policy.StrongName" /> of the assembly to remove from the list of assemblies used to evaluate policy. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="sn" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The assembly with the <see cref="T:System.Security.Policy.StrongName" /> specified by the <paramref name="sn" /> parameter does not have full trust. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Obsolete("All GACed assemblies are now fully trusted and all permissions now succeed on fully trusted code.")]
		public void RemoveFullTrustAssembly(StrongName sn)
		{
			if (sn == null)
			{
				throw new ArgumentNullException("sn");
			}
			StrongNameMembershipCondition snMC = new StrongNameMembershipCondition(sn.PublicKey, sn.Name, sn.Version);
			this.RemoveFullTrustAssembly(snMC);
		}

		/// <summary>Removes an assembly with the specified <see cref="T:System.Security.Policy.StrongNameMembershipCondition" /> from the list of assemblies the policy level uses to evaluate policy.</summary>
		/// <param name="snMC">The <see cref="T:System.Security.Policy.StrongNameMembershipCondition" /> of the assembly to remove from the list of assemblies used to evaluate policy. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="snMC" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Security.Policy.StrongNameMembershipCondition" /> specified by the <paramref name="snMC" /> parameter does not have full trust. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Obsolete("All GACed assemblies are now fully trusted and all permissions now succeed on fully trusted code.")]
		public void RemoveFullTrustAssembly(StrongNameMembershipCondition snMC)
		{
			if (snMC == null)
			{
				throw new ArgumentNullException("snMC");
			}
			if (((IList)this.full_trust_assemblies).Contains(snMC))
			{
				((IList)this.full_trust_assemblies).Remove(snMC);
				return;
			}
			throw new ArgumentException(Locale.GetText("sn does not have full trust."));
		}

		/// <summary>Removes the specified <see cref="T:System.Security.NamedPermissionSet" /> from the current policy level.</summary>
		/// <returns>The <see cref="T:System.Security.NamedPermissionSet" /> that was removed.</returns>
		/// <param name="permSet">The <see cref="T:System.Security.NamedPermissionSet" /> to remove from the current policy level. </param>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Security.NamedPermissionSet" /> specified by the <paramref name="permSet" /> parameter was not found. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="permSet" /> parameter is null. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public NamedPermissionSet RemoveNamedPermissionSet(NamedPermissionSet permSet)
		{
			if (permSet == null)
			{
				throw new ArgumentNullException("permSet");
			}
			return this.RemoveNamedPermissionSet(permSet.Name);
		}

		/// <summary>Removes the <see cref="T:System.Security.NamedPermissionSet" /> with the specified name from the current policy level.</summary>
		/// <returns>The <see cref="T:System.Security.NamedPermissionSet" /> that was removed.</returns>
		/// <param name="name">The name of the <see cref="T:System.Security.NamedPermissionSet" /> to remove. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="name" /> parameter is equal to the name of a reserved permission set.-or- A <see cref="T:System.Security.NamedPermissionSet" /> with the specified name cannot be found. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public NamedPermissionSet RemoveNamedPermissionSet(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (DefaultPolicies.ReservedNames.IsReserved(name))
			{
				throw new ArgumentException(Locale.GetText("Reserved name"));
			}
			foreach (object obj in this.named_permission_sets)
			{
				NamedPermissionSet namedPermissionSet = (NamedPermissionSet)obj;
				if (name == namedPermissionSet.Name)
				{
					this.named_permission_sets.Remove(namedPermissionSet);
					return namedPermissionSet;
				}
			}
			string message = string.Format(Locale.GetText("Name '{0}' cannot be found."), name);
			throw new ArgumentException(message, "name");
		}

		/// <summary>Returns the current policy level to the default state.</summary>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy" />
		/// </PermissionSet>
		public void Reset()
		{
			if (this.fullNames != null)
			{
				this.fullNames.Clear();
			}
			if (this._type != PolicyLevelType.AppDomain)
			{
				this.full_trust_assemblies.Clear();
				this.named_permission_sets.Clear();
				if (this._location != null && File.Exists(this._location))
				{
					try
					{
						File.Delete(this._location);
					}
					catch
					{
					}
				}
				this.LoadFromFile(this._location);
			}
			else
			{
				this.CreateDefaultFullTrustAssemblies();
				this.CreateDefaultNamedPermissionSets();
			}
		}

		/// <summary>Resolves policy based on evidence for the policy level, and returns the resulting <see cref="T:System.Security.Policy.PolicyStatement" />.</summary>
		/// <returns>The resulting <see cref="T:System.Security.Policy.PolicyStatement" />.</returns>
		/// <param name="evidence">The <see cref="T:System.Security.Policy.Evidence" /> used to resolve the <see cref="T:System.Security.Policy.PolicyLevel" />. </param>
		/// <exception cref="T:System.Security.Policy.PolicyException">The policy level contains multiple matching code groups marked as exclusive. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="evidence" /> parameter is null. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public PolicyStatement Resolve(Evidence evidence)
		{
			if (evidence == null)
			{
				throw new ArgumentNullException("evidence");
			}
			PolicyStatement policyStatement = this.root_code_group.Resolve(evidence);
			return (policyStatement == null) ? PolicyStatement.Empty() : policyStatement;
		}

		/// <summary>Resolves policy at the policy level and returns the root of a code group tree that matches the evidence.</summary>
		/// <returns>A <see cref="T:System.Security.Policy.CodeGroup" /> representing the root of a tree of code groups matching the specified evidence.</returns>
		/// <param name="evidence">The <see cref="T:System.Security.Policy.Evidence" /> used to resolve policy. </param>
		/// <exception cref="T:System.Security.Policy.PolicyException">The policy level contains multiple matching code groups marked as exclusive. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="evidence" /> parameter is null. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public CodeGroup ResolveMatchingCodeGroups(Evidence evidence)
		{
			if (evidence == null)
			{
				throw new ArgumentNullException("evidence");
			}
			CodeGroup codeGroup = this.root_code_group.ResolveMatchingCodeGroups(evidence);
			return (codeGroup == null) ? null : codeGroup;
		}

		/// <summary>Creates an XML encoding of the security object and its current state.</summary>
		/// <returns>An XML encoding of the security object, including any state information.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public SecurityElement ToXml()
		{
			Hashtable hashtable = new Hashtable();
			if (this.full_trust_assemblies.Count > 0 && !hashtable.Contains("StrongNameMembershipCondition"))
			{
				hashtable.Add("StrongNameMembershipCondition", typeof(StrongNameMembershipCondition).FullName);
			}
			SecurityElement securityElement = new SecurityElement("NamedPermissionSets");
			foreach (object obj in this.named_permission_sets)
			{
				NamedPermissionSet namedPermissionSet = (NamedPermissionSet)obj;
				SecurityElement securityElement2 = namedPermissionSet.ToXml();
				object key = securityElement2.Attributes["class"];
				if (!hashtable.Contains(key))
				{
					hashtable.Add(key, namedPermissionSet.GetType().FullName);
				}
				securityElement.AddChild(securityElement2);
			}
			SecurityElement securityElement3 = new SecurityElement("FullTrustAssemblies");
			foreach (object obj2 in this.full_trust_assemblies)
			{
				StrongNameMembershipCondition strongNameMembershipCondition = (StrongNameMembershipCondition)obj2;
				securityElement3.AddChild(strongNameMembershipCondition.ToXml(this));
			}
			SecurityElement securityElement4 = new SecurityElement("SecurityClasses");
			if (hashtable.Count > 0)
			{
				foreach (object obj3 in hashtable)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj3;
					SecurityElement securityElement5 = new SecurityElement("SecurityClass");
					securityElement5.AddAttribute("Name", (string)dictionaryEntry.Key);
					securityElement5.AddAttribute("Description", (string)dictionaryEntry.Value);
					securityElement4.AddChild(securityElement5);
				}
			}
			SecurityElement securityElement6 = new SecurityElement(typeof(PolicyLevel).Name);
			securityElement6.AddAttribute("version", "1");
			securityElement6.AddChild(securityElement4);
			securityElement6.AddChild(securityElement);
			if (this.root_code_group != null)
			{
				securityElement6.AddChild(this.root_code_group.ToXml(this));
			}
			securityElement6.AddChild(securityElement3);
			return securityElement6;
		}

		internal void Save()
		{
			if (this._type == PolicyLevelType.AppDomain)
			{
				throw new PolicyException(Locale.GetText("Can't save AppDomain PolicyLevel"));
			}
			if (this._location != null)
			{
				try
				{
					if (File.Exists(this._location))
					{
						File.Copy(this._location, this._location + ".backup", true);
					}
				}
				catch (Exception)
				{
				}
				finally
				{
					using (StreamWriter streamWriter = new StreamWriter(this._location))
					{
						streamWriter.Write(this.ToXml().ToString());
						streamWriter.Close();
					}
				}
			}
		}

		internal void CreateDefaultLevel(PolicyLevelType type)
		{
			PolicyStatement policy = new PolicyStatement(DefaultPolicies.FullTrust);
			switch (type)
			{
			case PolicyLevelType.User:
			case PolicyLevelType.Enterprise:
			case PolicyLevelType.AppDomain:
				this.root_code_group = new UnionCodeGroup(new AllMembershipCondition(), policy);
				this.root_code_group.Name = "All_Code";
				break;
			case PolicyLevelType.Machine:
			{
				PolicyStatement policy2 = new PolicyStatement(DefaultPolicies.Nothing);
				this.root_code_group = new UnionCodeGroup(new AllMembershipCondition(), policy2);
				this.root_code_group.Name = "All_Code";
				UnionCodeGroup unionCodeGroup = new UnionCodeGroup(new ZoneMembershipCondition(SecurityZone.MyComputer), policy);
				unionCodeGroup.Name = "My_Computer_Zone";
				this.root_code_group.AddChild(unionCodeGroup);
				UnionCodeGroup unionCodeGroup2 = new UnionCodeGroup(new ZoneMembershipCondition(SecurityZone.Intranet), new PolicyStatement(DefaultPolicies.LocalIntranet));
				unionCodeGroup2.Name = "LocalIntranet_Zone";
				this.root_code_group.AddChild(unionCodeGroup2);
				PolicyStatement policy3 = new PolicyStatement(DefaultPolicies.Internet);
				UnionCodeGroup unionCodeGroup3 = new UnionCodeGroup(new ZoneMembershipCondition(SecurityZone.Internet), policy3);
				unionCodeGroup3.Name = "Internet_Zone";
				this.root_code_group.AddChild(unionCodeGroup3);
				UnionCodeGroup unionCodeGroup4 = new UnionCodeGroup(new ZoneMembershipCondition(SecurityZone.Untrusted), policy2);
				unionCodeGroup4.Name = "Restricted_Zone";
				this.root_code_group.AddChild(unionCodeGroup4);
				UnionCodeGroup unionCodeGroup5 = new UnionCodeGroup(new ZoneMembershipCondition(SecurityZone.Trusted), policy3);
				unionCodeGroup5.Name = "Trusted_Zone";
				this.root_code_group.AddChild(unionCodeGroup5);
				break;
			}
			}
		}

		internal void CreateDefaultFullTrustAssemblies()
		{
			this.full_trust_assemblies.Clear();
			this.full_trust_assemblies.Add(DefaultPolicies.FullTrustMembership("mscorlib", DefaultPolicies.Key.Ecma));
			this.full_trust_assemblies.Add(DefaultPolicies.FullTrustMembership("System", DefaultPolicies.Key.Ecma));
			this.full_trust_assemblies.Add(DefaultPolicies.FullTrustMembership("System.Data", DefaultPolicies.Key.Ecma));
			this.full_trust_assemblies.Add(DefaultPolicies.FullTrustMembership("System.DirectoryServices", DefaultPolicies.Key.MsFinal));
			this.full_trust_assemblies.Add(DefaultPolicies.FullTrustMembership("System.Drawing", DefaultPolicies.Key.MsFinal));
			this.full_trust_assemblies.Add(DefaultPolicies.FullTrustMembership("System.Messaging", DefaultPolicies.Key.MsFinal));
			this.full_trust_assemblies.Add(DefaultPolicies.FullTrustMembership("System.ServiceProcess", DefaultPolicies.Key.MsFinal));
		}

		internal void CreateDefaultNamedPermissionSets()
		{
			this.named_permission_sets.Clear();
			try
			{
				SecurityManager.ResolvingPolicyLevel = this;
				this.named_permission_sets.Add(DefaultPolicies.LocalIntranet);
				this.named_permission_sets.Add(DefaultPolicies.Internet);
				this.named_permission_sets.Add(DefaultPolicies.SkipVerification);
				this.named_permission_sets.Add(DefaultPolicies.Execution);
				this.named_permission_sets.Add(DefaultPolicies.Nothing);
				this.named_permission_sets.Add(DefaultPolicies.Everything);
				this.named_permission_sets.Add(DefaultPolicies.FullTrust);
			}
			finally
			{
				SecurityManager.ResolvingPolicyLevel = null;
			}
		}

		internal string ResolveClassName(string className)
		{
			if (this.fullNames != null)
			{
				object obj = this.fullNames[className];
				if (obj != null)
				{
					return (string)obj;
				}
			}
			return className;
		}

		internal bool IsFullTrustAssembly(Assembly a)
		{
			AssemblyName assemblyName = a.UnprotectedGetName();
			StrongNamePublicKeyBlob blob = new StrongNamePublicKeyBlob(assemblyName.GetPublicKey());
			StrongNameMembershipCondition o = new StrongNameMembershipCondition(blob, assemblyName.Name, assemblyName.Version);
			foreach (object obj in this.full_trust_assemblies)
			{
				StrongNameMembershipCondition strongNameMembershipCondition = (StrongNameMembershipCondition)obj;
				if (strongNameMembershipCondition.Equals(o))
				{
					return true;
				}
			}
			return false;
		}
	}
}
