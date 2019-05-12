using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;

namespace System.Security
{
	/// <summary>Provides the main access point for classes interacting with the security system. This class cannot be inherited.</summary>
	[ComVisible(true)]
	public static class SecurityManager
	{
		private static object _lockObject;

		private static ArrayList _hierarchy;

		private static IPermission _unmanagedCode;

		private static Hashtable _declsecCache;

		private static PolicyLevel _level;

		private static SecurityPermission _execution = new SecurityPermission(SecurityPermissionFlag.Execution);

		static SecurityManager()
		{
			SecurityManager._lockObject = new object();
		}

		/// <summary>Gets or sets a value indicating whether code must have <see cref="F:System.Security.Permissions.SecurityPermissionFlag.Execution" /> in order to execute.</summary>
		/// <returns>true if code must have <see cref="F:System.Security.Permissions.SecurityPermissionFlag.Execution" /> in order to execute; otherwise, false.</returns>
		/// <exception cref="T:System.Security.SecurityException">The code that calls this method does not have <see cref="F:System.Security.Permissions.SecurityPermissionFlag.ControlPolicy" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlPolicy" />
		/// </PermissionSet>
		public static extern bool CheckExecutionRights { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>Gets or sets a value indicating whether security is enabled.</summary>
		/// <returns>true if security is enabled; otherwise, false.</returns>
		/// <exception cref="T:System.Security.SecurityException">The code that calls this method does not have <see cref="F:System.Security.Permissions.SecurityPermissionFlag.ControlPolicy" />. </exception>
		[Obsolete("The security manager cannot be turned off on MS runtime")]
		public static extern bool SecurityEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>Gets the granted zone identity and URL identity permission sets for the current assembly.</summary>
		/// <param name="zone">An output parameter that contains a <see cref="T:System.Collections.ArrayList" /> of granted <see cref="P:System.Security.Permissions.ZoneIdentityPermissionAttribute.Zone" /> objects.</param>
		/// <param name="origin">An output parameter that contains a <see cref="T:System.Collections.ArrayList" /> of granted <see cref="T:System.Security.Permissions.UrlIdentityPermission" /> objects.</param>
		/// <exception cref="T:System.Security.SecurityException">The request for <see cref="T:System.Security.Permissions.StrongNameIdentityPermission" /> failed.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.StrongNameIdentityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PublicKeyBlob="00000000000000000400000000000000" Name="System.Windows.Forms" />
		/// </PermissionSet>
		[MonoTODO("CAS support is experimental (and unsupported). This method only works in FullTrust.")]
		public static void GetZoneAndOrigin(out ArrayList zone, out ArrayList origin)
		{
			zone = new ArrayList();
			origin = new ArrayList();
		}

		/// <summary>Determines whether a permission is granted to the caller.</summary>
		/// <returns>true if the permissions granted to the caller include the permission <paramref name="perm" />; otherwise, false.</returns>
		/// <param name="perm">The permission to test against the grant of the caller. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static bool IsGranted(IPermission perm)
		{
			return perm == null || !SecurityManager.SecurityEnabled || SecurityManager.IsGranted(Assembly.GetCallingAssembly(), perm);
		}

		internal static bool IsGranted(Assembly a, IPermission perm)
		{
			PermissionSet grantedPermissionSet = a.GrantedPermissionSet;
			if (grantedPermissionSet != null && !grantedPermissionSet.IsUnrestricted())
			{
				CodeAccessPermission target = (CodeAccessPermission)grantedPermissionSet.GetPermission(perm.GetType());
				if (!perm.IsSubsetOf(target))
				{
					return false;
				}
			}
			PermissionSet deniedPermissionSet = a.DeniedPermissionSet;
			if (deniedPermissionSet != null && !deniedPermissionSet.IsEmpty())
			{
				if (deniedPermissionSet.IsUnrestricted())
				{
					return false;
				}
				CodeAccessPermission codeAccessPermission = (CodeAccessPermission)a.DeniedPermissionSet.GetPermission(perm.GetType());
				if (codeAccessPermission != null && perm.IsSubsetOf(codeAccessPermission))
				{
					return false;
				}
			}
			return true;
		}

		internal static IPermission CheckPermissionSet(Assembly a, PermissionSet ps, bool noncas)
		{
			if (ps.IsEmpty())
			{
				return null;
			}
			foreach (object obj in ps)
			{
				IPermission permission = (IPermission)obj;
				if (!noncas && permission is CodeAccessPermission)
				{
					if (!SecurityManager.IsGranted(a, permission))
					{
						return permission;
					}
				}
				else
				{
					try
					{
						permission.Demand();
					}
					catch (SecurityException)
					{
						return permission;
					}
				}
			}
			return null;
		}

		internal static IPermission CheckPermissionSet(AppDomain ad, PermissionSet ps)
		{
			if (ps == null || ps.IsEmpty())
			{
				return null;
			}
			PermissionSet grantedPermissionSet = ad.GrantedPermissionSet;
			if (grantedPermissionSet == null)
			{
				return null;
			}
			if (grantedPermissionSet.IsUnrestricted())
			{
				return null;
			}
			if (ps.IsUnrestricted())
			{
				return new SecurityPermission(SecurityPermissionFlag.NoFlags);
			}
			foreach (object obj in ps)
			{
				IPermission permission = (IPermission)obj;
				if (permission is CodeAccessPermission)
				{
					CodeAccessPermission codeAccessPermission = (CodeAccessPermission)grantedPermissionSet.GetPermission(permission.GetType());
					if (codeAccessPermission == null)
					{
						if ((!grantedPermissionSet.IsUnrestricted() || !(permission is IUnrestrictedPermission)) && !permission.IsSubsetOf(null))
						{
							return permission;
						}
					}
					else if (!permission.IsSubsetOf(codeAccessPermission))
					{
						return permission;
					}
				}
				else
				{
					try
					{
						permission.Demand();
					}
					catch (SecurityException)
					{
						return permission;
					}
				}
			}
			return null;
		}

		/// <summary>Loads a <see cref="T:System.Security.Policy.PolicyLevel" /> from the specified file.</summary>
		/// <returns>The loaded <see cref="T:System.Security.Policy.PolicyLevel" />.</returns>
		/// <param name="path">The physical file path to a file containing the security policy information. </param>
		/// <param name="type">One of the <see cref="T:System.Security.PolicyLevelType" /> values. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="path" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The file indicated by the <paramref name="path" /> parameter does not exist. </exception>
		/// <exception cref="T:System.Security.SecurityException">The code that calls this method does not have <see cref="F:System.Security.Permissions.SecurityPermissionFlag.ControlPolicy" />.-or- The code that calls this method does not have <see cref="F:System.Security.Permissions.FileIOPermissionAccess.Read" />.-or- The code that calls this method does not have <see cref="F:System.Security.Permissions.FileIOPermissionAccess.Write" />.-or- The code that calls this method does not have <see cref="F:System.Security.Permissions.FileIOPermissionAccess.PathDiscovery" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence, ControlPolicy" />
		/// </PermissionSet>
		public static PolicyLevel LoadPolicyLevelFromFile(string path, PolicyLevelType type)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			PolicyLevel policyLevel = null;
			try
			{
				policyLevel = new PolicyLevel(type.ToString(), type);
				policyLevel.LoadFromFile(path);
			}
			catch (Exception innerException)
			{
				throw new ArgumentException(Locale.GetText("Invalid policy XML"), innerException);
			}
			return policyLevel;
		}

		/// <summary>Loads a <see cref="T:System.Security.Policy.PolicyLevel" /> from the specified string.</summary>
		/// <returns>The loaded <see cref="T:System.Security.Policy.PolicyLevel" />.</returns>
		/// <param name="str">The XML representation of a security policy level in the same form in which it appears in a configuration file. </param>
		/// <param name="type">One of the <see cref="T:System.Security.PolicyLevelType" /> values. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="str" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="str" /> parameter is not valid. </exception>
		/// <exception cref="T:System.Security.SecurityException">The code that calls this method does not have <see cref="F:System.Security.Permissions.SecurityPermissionFlag.ControlPolicy" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlPolicy" />
		/// </PermissionSet>
		public static PolicyLevel LoadPolicyLevelFromString(string str, PolicyLevelType type)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			PolicyLevel policyLevel = null;
			try
			{
				policyLevel = new PolicyLevel(type.ToString(), type);
				policyLevel.LoadFromString(str);
			}
			catch (Exception innerException)
			{
				throw new ArgumentException(Locale.GetText("Invalid policy XML"), innerException);
			}
			return policyLevel;
		}

		/// <summary>Provides an enumerator to access the security policy hierarchy by levels, such as computer policy and user policy.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for <see cref="T:System.Security.Policy.PolicyLevel" /> objects that compose the security policy hierarchy.</returns>
		/// <exception cref="T:System.Security.SecurityException">The code that calls this method does not have <see cref="F:System.Security.Permissions.SecurityPermissionFlag.ControlPolicy" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlPolicy" />
		/// </PermissionSet>
		public static IEnumerator PolicyHierarchy()
		{
			return SecurityManager.Hierarchy;
		}

		/// <summary>Determines what permissions to grant to code based on the specified evidence.</summary>
		/// <returns>The set of permissions that can be granted by the security system.</returns>
		/// <param name="evidence">The evidence set used to evaluate policy. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public static PermissionSet ResolvePolicy(Evidence evidence)
		{
			if (evidence == null)
			{
				return new PermissionSet(PermissionState.None);
			}
			PermissionSet permissionSet = null;
			IEnumerator hierarchy = SecurityManager.Hierarchy;
			while (hierarchy.MoveNext())
			{
				object obj = hierarchy.Current;
				PolicyLevel pl = (PolicyLevel)obj;
				if (SecurityManager.ResolvePolicyLevel(ref permissionSet, pl, evidence))
				{
					break;
				}
			}
			SecurityManager.ResolveIdentityPermissions(permissionSet, evidence);
			return permissionSet;
		}

		/// <summary>Determines what permissions to grant to code based on the specified evidence.</summary>
		/// <returns>The set of permissions that is appropriate for all of the provided evidence.</returns>
		/// <param name="evidences">An array of <see cref="T:System.Security.Policy.Evidence" /> objects used to evaluate policy. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[MonoTODO("(2.0) more tests are needed")]
		public static PermissionSet ResolvePolicy(Evidence[] evidences)
		{
			if (evidences == null || evidences.Length == 0 || (evidences.Length == 1 && evidences[0].Count == 0))
			{
				return new PermissionSet(PermissionState.None);
			}
			PermissionSet permissionSet = SecurityManager.ResolvePolicy(evidences[0]);
			for (int i = 1; i < evidences.Length; i++)
			{
				permissionSet = permissionSet.Intersect(SecurityManager.ResolvePolicy(evidences[i]));
			}
			return permissionSet;
		}

		/// <summary>Determines what permissions to grant to code based on the specified evidence, excluding the policy for the <see cref="T:System.AppDomain" /> level.</summary>
		/// <returns>The set of permissions that can be granted by the security system.</returns>
		/// <param name="evidence">The evidence set used to evaluate policy.</param>
		public static PermissionSet ResolveSystemPolicy(Evidence evidence)
		{
			if (evidence == null)
			{
				return new PermissionSet(PermissionState.None);
			}
			PermissionSet permissionSet = null;
			IEnumerator hierarchy = SecurityManager.Hierarchy;
			while (hierarchy.MoveNext())
			{
				object obj = hierarchy.Current;
				PolicyLevel policyLevel = (PolicyLevel)obj;
				if (policyLevel.Type == PolicyLevelType.AppDomain)
				{
					break;
				}
				if (SecurityManager.ResolvePolicyLevel(ref permissionSet, policyLevel, evidence))
				{
					break;
				}
			}
			SecurityManager.ResolveIdentityPermissions(permissionSet, evidence);
			return permissionSet;
		}

		/// <summary>Determines what permissions to grant to code based on the specified evidence and requests.</summary>
		/// <returns>The set of permissions that would be granted by the security system.</returns>
		/// <param name="evidence">The evidence set used to evaluate policy. </param>
		/// <param name="reqdPset">The required permissions the code needs to run. </param>
		/// <param name="optPset">The optional permissions that will be used if granted, but aren't required for the code to run. </param>
		/// <param name="denyPset">The denied permissions that must never be granted to the code even if policy otherwise permits it. </param>
		/// <param name="denied">An output parameter that contains the set of permissions not granted. </param>
		/// <exception cref="T:System.Security.Policy.PolicyException">Policy fails to grant the minimum required permissions specified by the <paramref name="reqdPset" /> parameter. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public static PermissionSet ResolvePolicy(Evidence evidence, PermissionSet reqdPset, PermissionSet optPset, PermissionSet denyPset, out PermissionSet denied)
		{
			PermissionSet permissionSet = SecurityManager.ResolvePolicy(evidence);
			if (reqdPset != null && !reqdPset.IsSubsetOf(permissionSet))
			{
				throw new PolicyException(Locale.GetText("Policy doesn't grant the minimal permissions required to execute the assembly."));
			}
			if (SecurityManager.CheckExecutionRights)
			{
				bool flag = false;
				if (permissionSet != null)
				{
					if (permissionSet.IsUnrestricted())
					{
						flag = true;
					}
					else
					{
						IPermission permission = permissionSet.GetPermission(typeof(SecurityPermission));
						flag = SecurityManager._execution.IsSubsetOf(permission);
					}
				}
				if (!flag)
				{
					throw new PolicyException(Locale.GetText("Policy doesn't grant the right to execute the assembly."));
				}
			}
			denied = denyPset;
			return permissionSet;
		}

		/// <summary>Gets a collection of code groups matching the specified evidence.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> enumeration of the set of code groups matching the evidence.</returns>
		/// <param name="evidence">The evidence set against which the policy is evaluated. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public static IEnumerator ResolvePolicyGroups(Evidence evidence)
		{
			if (evidence == null)
			{
				throw new ArgumentNullException("evidence");
			}
			ArrayList arrayList = new ArrayList();
			IEnumerator hierarchy = SecurityManager.Hierarchy;
			while (hierarchy.MoveNext())
			{
				object obj = hierarchy.Current;
				PolicyLevel policyLevel = (PolicyLevel)obj;
				CodeGroup value = policyLevel.ResolveMatchingCodeGroups(evidence);
				arrayList.Add(value);
			}
			return arrayList.GetEnumerator();
		}

		/// <summary>Saves the modified security policy state.</summary>
		/// <exception cref="T:System.Security.SecurityException">The code that calls this method does not have <see cref="F:System.Security.Permissions.SecurityPermissionFlag.ControlPolicy" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy" />
		/// </PermissionSet>
		public static void SavePolicy()
		{
			IEnumerator hierarchy = SecurityManager.Hierarchy;
			while (hierarchy.MoveNext())
			{
				object obj = hierarchy.Current;
				PolicyLevel policyLevel = obj as PolicyLevel;
				policyLevel.Save();
			}
		}

		/// <summary>Saves a modified security policy level loaded with <see cref="M:System.Security.SecurityManager.LoadPolicyLevelFromFile(System.String,System.Security.PolicyLevelType)" />.</summary>
		/// <param name="level">The <see cref="T:System.Security.Policy.PolicyLevel" /> object to be saved. </param>
		/// <exception cref="T:System.Security.SecurityException">The code that calls this method does not have <see cref="F:System.Security.Permissions.SecurityPermissionFlag.ControlPolicy" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy" />
		/// </PermissionSet>
		public static void SavePolicyLevel(PolicyLevel level)
		{
			level.Save();
		}

		private static IEnumerator Hierarchy
		{
			get
			{
				object lockObject = SecurityManager._lockObject;
				lock (lockObject)
				{
					if (SecurityManager._hierarchy == null)
					{
						SecurityManager.InitializePolicyHierarchy();
					}
				}
				return SecurityManager._hierarchy.GetEnumerator();
			}
		}

		private static void InitializePolicyHierarchy()
		{
			string directoryName = Path.GetDirectoryName(Environment.GetMachineConfigPath());
			string path = Path.Combine(Environment.InternalGetFolderPath(Environment.SpecialFolder.ApplicationData), "mono");
			PolicyLevel policyLevel = new PolicyLevel("Enterprise", PolicyLevelType.Enterprise);
			SecurityManager._level = policyLevel;
			policyLevel.LoadFromFile(Path.Combine(directoryName, "enterprisesec.config"));
			PolicyLevel policyLevel2 = new PolicyLevel("Machine", PolicyLevelType.Machine);
			SecurityManager._level = policyLevel2;
			policyLevel2.LoadFromFile(Path.Combine(directoryName, "security.config"));
			PolicyLevel policyLevel3 = new PolicyLevel("User", PolicyLevelType.User);
			SecurityManager._level = policyLevel3;
			policyLevel3.LoadFromFile(Path.Combine(path, "security.config"));
			SecurityManager._hierarchy = ArrayList.Synchronized(new ArrayList
			{
				policyLevel,
				policyLevel2,
				policyLevel3
			});
			SecurityManager._level = null;
		}

		internal static bool ResolvePolicyLevel(ref PermissionSet ps, PolicyLevel pl, Evidence evidence)
		{
			PolicyStatement policyStatement = pl.Resolve(evidence);
			if (policyStatement != null)
			{
				if (ps == null)
				{
					ps = policyStatement.PermissionSet;
				}
				else
				{
					ps = ps.Intersect(policyStatement.PermissionSet);
					if (ps == null)
					{
						ps = new PermissionSet(PermissionState.None);
					}
				}
				if ((policyStatement.Attributes & PolicyStatementAttribute.LevelFinal) == PolicyStatementAttribute.LevelFinal)
				{
					return true;
				}
			}
			return false;
		}

		internal static void ResolveIdentityPermissions(PermissionSet ps, Evidence evidence)
		{
			if (ps.IsUnrestricted())
			{
				return;
			}
			IEnumerator hostEnumerator = evidence.GetHostEnumerator();
			while (hostEnumerator.MoveNext())
			{
				object obj = hostEnumerator.Current;
				IIdentityPermissionFactory identityPermissionFactory = obj as IIdentityPermissionFactory;
				if (identityPermissionFactory != null)
				{
					IPermission perm = identityPermissionFactory.CreateIdentityPermission(evidence);
					ps.AddPermission(perm);
				}
			}
		}

		internal static PolicyLevel ResolvingPolicyLevel
		{
			get
			{
				return SecurityManager._level;
			}
			set
			{
				SecurityManager._level = value;
			}
		}

		internal static PermissionSet Decode(IntPtr permissions, int length)
		{
			PermissionSet permissionSet = null;
			object lockObject = SecurityManager._lockObject;
			lock (lockObject)
			{
				if (SecurityManager._declsecCache == null)
				{
					SecurityManager._declsecCache = new Hashtable();
				}
				object key = (int)permissions;
				permissionSet = (PermissionSet)SecurityManager._declsecCache[key];
				if (permissionSet == null)
				{
					byte[] array = new byte[length];
					Marshal.Copy(permissions, array, 0, length);
					permissionSet = SecurityManager.Decode(array);
					permissionSet.DeclarativeSecurity = true;
					SecurityManager._declsecCache.Add(key, permissionSet);
				}
			}
			return permissionSet;
		}

		internal static PermissionSet Decode(byte[] encodedPermissions)
		{
			if (encodedPermissions == null || encodedPermissions.Length < 1)
			{
				throw new SecurityException("Invalid metadata format.");
			}
			byte b = encodedPermissions[0];
			if (b == 46)
			{
				return PermissionSet.CreateFromBinaryFormat(encodedPermissions);
			}
			if (b != 60)
			{
				throw new SecurityException(Locale.GetText("Unknown metadata format."));
			}
			string @string = Encoding.Unicode.GetString(encodedPermissions);
			return new PermissionSet(@string);
		}

		private static IPermission UnmanagedCode
		{
			get
			{
				object lockObject = SecurityManager._lockObject;
				lock (lockObject)
				{
					if (SecurityManager._unmanagedCode == null)
					{
						SecurityManager._unmanagedCode = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
					}
				}
				return SecurityManager._unmanagedCode;
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern bool GetLinkDemandSecurity(MethodBase method, RuntimeDeclSecurityActions* cdecl, RuntimeDeclSecurityActions* mdecl);

		internal unsafe static void ReflectedLinkDemandInvoke(MethodBase mb)
		{
			RuntimeDeclSecurityActions runtimeDeclSecurityActions;
			RuntimeDeclSecurityActions runtimeDeclSecurityActions2;
			if (!SecurityManager.GetLinkDemandSecurity(mb, &runtimeDeclSecurityActions, &runtimeDeclSecurityActions2))
			{
				return;
			}
			PermissionSet permissionSet = null;
			if (runtimeDeclSecurityActions.cas.size > 0)
			{
				permissionSet = SecurityManager.Decode(runtimeDeclSecurityActions.cas.blob, runtimeDeclSecurityActions.cas.size);
			}
			if (runtimeDeclSecurityActions.noncas.size > 0)
			{
				PermissionSet permissionSet2 = SecurityManager.Decode(runtimeDeclSecurityActions.noncas.blob, runtimeDeclSecurityActions.noncas.size);
				permissionSet = ((permissionSet != null) ? permissionSet.Union(permissionSet2) : permissionSet2);
			}
			if (runtimeDeclSecurityActions2.cas.size > 0)
			{
				PermissionSet permissionSet3 = SecurityManager.Decode(runtimeDeclSecurityActions2.cas.blob, runtimeDeclSecurityActions2.cas.size);
				permissionSet = ((permissionSet != null) ? permissionSet.Union(permissionSet3) : permissionSet3);
			}
			if (runtimeDeclSecurityActions2.noncas.size > 0)
			{
				PermissionSet permissionSet4 = SecurityManager.Decode(runtimeDeclSecurityActions2.noncas.blob, runtimeDeclSecurityActions2.noncas.size);
				permissionSet = ((permissionSet != null) ? permissionSet.Union(permissionSet4) : permissionSet4);
			}
			if (permissionSet != null)
			{
				permissionSet.Demand();
			}
		}

		internal unsafe static bool ReflectedLinkDemandQuery(MethodBase mb)
		{
			RuntimeDeclSecurityActions runtimeDeclSecurityActions;
			RuntimeDeclSecurityActions runtimeDeclSecurityActions2;
			return !SecurityManager.GetLinkDemandSecurity(mb, &runtimeDeclSecurityActions, &runtimeDeclSecurityActions2) || SecurityManager.LinkDemand(mb.ReflectedType.Assembly, &runtimeDeclSecurityActions, &runtimeDeclSecurityActions2);
		}

		private unsafe static bool LinkDemand(Assembly a, RuntimeDeclSecurityActions* klass, RuntimeDeclSecurityActions* method)
		{
			bool result;
			try
			{
				bool flag = true;
				if (klass->cas.size > 0)
				{
					PermissionSet ps = SecurityManager.Decode(klass->cas.blob, klass->cas.size);
					flag = (SecurityManager.CheckPermissionSet(a, ps, false) == null);
				}
				if (flag && klass->noncas.size > 0)
				{
					PermissionSet ps = SecurityManager.Decode(klass->noncas.blob, klass->noncas.size);
					flag = (SecurityManager.CheckPermissionSet(a, ps, true) == null);
				}
				if (flag && method->cas.size > 0)
				{
					PermissionSet ps = SecurityManager.Decode(method->cas.blob, method->cas.size);
					flag = (SecurityManager.CheckPermissionSet(a, ps, false) == null);
				}
				if (flag && method->noncas.size > 0)
				{
					PermissionSet ps = SecurityManager.Decode(method->noncas.blob, method->noncas.size);
					flag = (SecurityManager.CheckPermissionSet(a, ps, true) == null);
				}
				result = flag;
			}
			catch (SecurityException)
			{
				result = false;
			}
			return result;
		}

		private static bool LinkDemandFullTrust(Assembly a)
		{
			PermissionSet grantedPermissionSet = a.GrantedPermissionSet;
			if (grantedPermissionSet != null && !grantedPermissionSet.IsUnrestricted())
			{
				return false;
			}
			PermissionSet deniedPermissionSet = a.DeniedPermissionSet;
			return deniedPermissionSet == null || deniedPermissionSet.IsEmpty();
		}

		private static bool LinkDemandUnmanaged(Assembly a)
		{
			return SecurityManager.IsGranted(a, SecurityManager.UnmanagedCode);
		}

		private static void LinkDemandSecurityException(int securityViolation, IntPtr methodHandle)
		{
			RuntimeMethodHandle handle = new RuntimeMethodHandle(methodHandle);
			MethodInfo methodInfo = (MethodInfo)MethodBase.GetMethodFromHandle(handle);
			Assembly assembly = methodInfo.DeclaringType.Assembly;
			AssemblyName assemblyName = null;
			PermissionSet grant = null;
			PermissionSet refused = null;
			object demanded = null;
			IPermission permThatFailed = null;
			if (assembly != null)
			{
				assemblyName = assembly.UnprotectedGetName();
				grant = assembly.GrantedPermissionSet;
				refused = assembly.DeniedPermissionSet;
			}
			string text;
			switch (securityViolation)
			{
			case 1:
				text = Locale.GetText("Permissions refused to call this method.");
				goto IL_E5;
			case 2:
				text = Locale.GetText("Partially trusted callers aren't allowed to call into this assembly.");
				demanded = DefaultPolicies.FullTrust;
				goto IL_E5;
			case 4:
				text = Locale.GetText("Calling internal calls is restricted to ECMA signed assemblies.");
				goto IL_E5;
			case 8:
				text = Locale.GetText("Calling unmanaged code isn't allowed from this assembly.");
				demanded = SecurityManager._unmanagedCode;
				permThatFailed = SecurityManager._unmanagedCode;
				goto IL_E5;
			}
			text = Locale.GetText("JIT time LinkDemand failed.");
			IL_E5:
			throw new SecurityException(text, assemblyName, grant, refused, methodInfo, SecurityAction.LinkDemand, demanded, permThatFailed, null);
		}

		private static void InheritanceDemandSecurityException(int securityViolation, Assembly a, Type t, MethodInfo method)
		{
			AssemblyName assemblyName = null;
			PermissionSet grant = null;
			PermissionSet refused = null;
			if (a != null)
			{
				assemblyName = a.UnprotectedGetName();
				grant = a.GrantedPermissionSet;
				refused = a.DeniedPermissionSet;
			}
			string message;
			if (securityViolation != 1)
			{
				if (securityViolation != 2)
				{
					message = Locale.GetText("Load time InheritDemand failed.");
				}
				else
				{
					message = Locale.GetText("Method override refused.");
				}
			}
			else
			{
				message = string.Format(Locale.GetText("Class inheritance refused for {0}."), t);
			}
			throw new SecurityException(message, assemblyName, grant, refused, method, SecurityAction.InheritanceDemand, null, null, null);
		}

		private static void ThrowException(Exception ex)
		{
			throw ex;
		}

		private unsafe static bool InheritanceDemand(AppDomain ad, Assembly a, RuntimeDeclSecurityActions* actions)
		{
			bool result;
			try
			{
				bool flag = true;
				if (actions->cas.size > 0)
				{
					PermissionSet ps = SecurityManager.Decode(actions->cas.blob, actions->cas.size);
					flag = (SecurityManager.CheckPermissionSet(a, ps, false) == null);
					if (flag)
					{
						flag = (SecurityManager.CheckPermissionSet(ad, ps) == null);
					}
				}
				if (actions->noncas.size > 0)
				{
					PermissionSet ps = SecurityManager.Decode(actions->noncas.blob, actions->noncas.size);
					flag = (SecurityManager.CheckPermissionSet(a, ps, true) == null);
					if (flag)
					{
						flag = (SecurityManager.CheckPermissionSet(ad, ps) == null);
					}
				}
				result = flag;
			}
			catch (SecurityException)
			{
				result = false;
			}
			return result;
		}

		private static void DemandUnmanaged()
		{
			SecurityManager.UnmanagedCode.Demand();
		}

		private static void InternalDemand(IntPtr permissions, int length)
		{
			PermissionSet permissionSet = SecurityManager.Decode(permissions, length);
			permissionSet.Demand();
		}

		private static void InternalDemandChoice(IntPtr permissions, int length)
		{
			throw new SecurityException("SecurityAction.DemandChoice was removed from 2.0");
		}
	}
}
