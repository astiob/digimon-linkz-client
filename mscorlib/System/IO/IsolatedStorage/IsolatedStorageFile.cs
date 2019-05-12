using Mono.Security.Cryptography;
using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading;

namespace System.IO.IsolatedStorage
{
	/// <summary>Represents an isolated storage area containing files and directories.</summary>
	[ComVisible(true)]
	public sealed class IsolatedStorageFile : IsolatedStorage, IDisposable
	{
		private bool _resolved;

		private ulong _maxSize;

		private Evidence _fullEvidences;

		private static Mutex mutex = new Mutex();

		private DirectoryInfo directory;

		private IsolatedStorageFile(IsolatedStorageScope scope)
		{
			this.storage_scope = scope;
		}

		internal IsolatedStorageFile(IsolatedStorageScope scope, string location)
		{
			this.storage_scope = scope;
			this.directory = new DirectoryInfo(location);
			if (!this.directory.Exists)
			{
				string text = Locale.GetText("Invalid storage.");
				throw new IsolatedStorageException(text);
			}
		}

		/// <summary>Gets the enumerator for the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFile" /> stores within an isolated storage scope.</summary>
		/// <returns>Enumerator for the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFile" /> stores within the specified isolated storage scope.</returns>
		/// <param name="scope">Represents the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageScope" /> for which to return isolated stores. User and User|Roaming are the only IsolatedStorageScope combinations supported. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.IsolatedStorageFilePermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static IEnumerator GetEnumerator(IsolatedStorageScope scope)
		{
			IsolatedStorageFile.Demand(scope);
			if (scope != IsolatedStorageScope.User && scope != (IsolatedStorageScope.User | IsolatedStorageScope.Roaming) && scope != IsolatedStorageScope.Machine)
			{
				string text = Locale.GetText("Invalid scope, only User, User|Roaming and Machine are valid");
				throw new ArgumentException(text);
			}
			return new IsolatedStorageFileEnumerator(scope, IsolatedStorageFile.GetIsolatedStorageRoot(scope));
		}

		/// <summary>Obtains isolated storage corresponding to the given application domain and the assembly evidence objects and types.</summary>
		/// <returns>An <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFile" /> object representing the parameters.</returns>
		/// <param name="scope">A bitwise combination of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageScope" /> values. </param>
		/// <param name="domainEvidence">An <see cref="T:System.Security.Policy.Evidence" /> object containing the application domain identity. </param>
		/// <param name="domainEvidenceType">The identity <see cref="T:System.Type" /> to choose from the application domain evidence. </param>
		/// <param name="assemblyEvidence">An <see cref="T:System.Security.Policy.Evidence" /> object containing the code assembly identity. </param>
		/// <param name="assemblyEvidenceType">The identity <see cref="T:System.Type" /> to choose from the application code assembly evidence. </param>
		/// <exception cref="T:System.Security.SecurityException">Sufficient isolated storage permissions have not been granted. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="domainEvidence" /> or <paramref name="assemblyEvidence" /> identity has not been passed in. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="scope" /> is invalid. </exception>
		/// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">An isolated storage location cannot be initialized.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.IsolatedStorageFilePermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, Evidence domainEvidence, Type domainEvidenceType, Evidence assemblyEvidence, Type assemblyEvidenceType)
		{
			IsolatedStorageFile.Demand(scope);
			bool flag = (scope & IsolatedStorageScope.Domain) != IsolatedStorageScope.None;
			if (flag && domainEvidence == null)
			{
				throw new ArgumentNullException("domainEvidence");
			}
			bool flag2 = (scope & IsolatedStorageScope.Assembly) != IsolatedStorageScope.None;
			if (flag2 && assemblyEvidence == null)
			{
				throw new ArgumentNullException("assemblyEvidence");
			}
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(scope);
			if (flag)
			{
				if (domainEvidenceType == null)
				{
					isolatedStorageFile._domainIdentity = IsolatedStorageFile.GetDomainIdentityFromEvidence(domainEvidence);
				}
				else
				{
					isolatedStorageFile._domainIdentity = IsolatedStorageFile.GetTypeFromEvidence(domainEvidence, domainEvidenceType);
				}
				if (isolatedStorageFile._domainIdentity == null)
				{
					throw new IsolatedStorageException(Locale.GetText("Couldn't find domain identity."));
				}
			}
			if (flag2)
			{
				if (assemblyEvidenceType == null)
				{
					isolatedStorageFile._assemblyIdentity = IsolatedStorageFile.GetAssemblyIdentityFromEvidence(assemblyEvidence);
				}
				else
				{
					isolatedStorageFile._assemblyIdentity = IsolatedStorageFile.GetTypeFromEvidence(assemblyEvidence, assemblyEvidenceType);
				}
				if (isolatedStorageFile._assemblyIdentity == null)
				{
					throw new IsolatedStorageException(Locale.GetText("Couldn't find assembly identity."));
				}
			}
			isolatedStorageFile.PostInit();
			return isolatedStorageFile;
		}

		/// <summary>Obtains the isolated storage corresponding to the given application domain and assembly evidence objects.</summary>
		/// <returns>An <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFile" /> representing the parameters.</returns>
		/// <param name="scope">A bitwise combination of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageScope" /> values. </param>
		/// <param name="domainIdentity">An <see cref="T:System.Object" /> that contains evidence for the application domain identity. </param>
		/// <param name="assemblyIdentity">An <see cref="T:System.Object" /> that contains evidence for the code assembly identity. </param>
		/// <exception cref="T:System.Security.SecurityException">Sufficient isolated storage permissions have not been granted. </exception>
		/// <exception cref="T:System.ArgumentNullException">Neither the <paramref name="domainIdentity" /> nor <paramref name="assemblyIdentity" /> have been passed in. This verifies that the correct constructor is being used.-or- Either <paramref name="domainIdentity" /> or <paramref name="assemblyIdentity" /> are null. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="scope" /> is invalid. </exception>
		/// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">An isolated storage location cannot be initialized.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.IsolatedStorageFilePermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, object domainIdentity, object assemblyIdentity)
		{
			IsolatedStorageFile.Demand(scope);
			if ((scope & IsolatedStorageScope.Domain) != IsolatedStorageScope.None && domainIdentity == null)
			{
				throw new ArgumentNullException("domainIdentity");
			}
			bool flag = (scope & IsolatedStorageScope.Assembly) != IsolatedStorageScope.None;
			if (flag && assemblyIdentity == null)
			{
				throw new ArgumentNullException("assemblyIdentity");
			}
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(scope);
			if (flag)
			{
				isolatedStorageFile._fullEvidences = Assembly.GetCallingAssembly().UnprotectedGetEvidence();
			}
			isolatedStorageFile._domainIdentity = domainIdentity;
			isolatedStorageFile._assemblyIdentity = assemblyIdentity;
			isolatedStorageFile.PostInit();
			return isolatedStorageFile;
		}

		/// <summary>Obtains isolated storage corresponding to the isolated storage scope given the application domain and assembly evidence types.</summary>
		/// <returns>An <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFile" /> object representing the parameters.</returns>
		/// <param name="scope">A bitwise combination of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageScope" /> values. </param>
		/// <param name="domainEvidenceType">The type of the <see cref="T:System.Security.Policy.Evidence" /> that you can chose from the list of <see cref="T:System.Security.Policy.Evidence" /> present in the domain of the calling application. null lets the <see cref="T:System.IO.IsolatedStorage.IsolatedStorage" /> object choose the evidence. </param>
		/// <param name="assemblyEvidenceType">The type of the <see cref="T:System.Security.Policy.Evidence" /> that you can chose from the list of <see cref="T:System.Security.Policy.Evidence" /> present in the domain of the calling application. null lets the <see cref="T:System.IO.IsolatedStorage.IsolatedStorage" /> object choose the evidence. </param>
		/// <exception cref="T:System.Security.SecurityException">Sufficient isolated storage permissions have not been granted. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="scope" /> is invalid. </exception>
		/// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The evidence type provided is missing in the assembly evidence list. -or-An isolated storage location cannot be initialized.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.IsolatedStorageFilePermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, Type domainEvidenceType, Type assemblyEvidenceType)
		{
			IsolatedStorageFile.Demand(scope);
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(scope);
			if ((scope & IsolatedStorageScope.Domain) != IsolatedStorageScope.None)
			{
				if (domainEvidenceType == null)
				{
					domainEvidenceType = typeof(Url);
				}
				isolatedStorageFile._domainIdentity = IsolatedStorageFile.GetTypeFromEvidence(AppDomain.CurrentDomain.Evidence, domainEvidenceType);
			}
			if ((scope & IsolatedStorageScope.Assembly) != IsolatedStorageScope.None)
			{
				Evidence evidence = Assembly.GetCallingAssembly().UnprotectedGetEvidence();
				isolatedStorageFile._fullEvidences = evidence;
				if ((scope & IsolatedStorageScope.Domain) != IsolatedStorageScope.None)
				{
					if (assemblyEvidenceType == null)
					{
						assemblyEvidenceType = typeof(Url);
					}
					isolatedStorageFile._assemblyIdentity = IsolatedStorageFile.GetTypeFromEvidence(evidence, assemblyEvidenceType);
				}
				else
				{
					isolatedStorageFile._assemblyIdentity = IsolatedStorageFile.GetAssemblyIdentityFromEvidence(evidence);
				}
			}
			isolatedStorageFile.PostInit();
			return isolatedStorageFile;
		}

		/// <summary>Obtains isolated storage corresponding to the given application identity.</summary>
		/// <returns>An <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFile" /> object representing the parameters.</returns>
		/// <param name="scope">A bitwise combination of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageScope" /> values. </param>
		/// <param name="applicationIdentity">An <see cref="T:System.Object" /> that contains evidence for the application identity. </param>
		/// <exception cref="T:System.Security.SecurityException">Sufficient isolated storage permissions have not been granted. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="applicationEvidence" /> identity has not been passed in. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="scope" /> is invalid. </exception>
		/// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">An isolated storage location cannot be initialized.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.IsolatedStorageFilePermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, object applicationIdentity)
		{
			IsolatedStorageFile.Demand(scope);
			if (applicationIdentity == null)
			{
				throw new ArgumentNullException("applicationIdentity");
			}
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(scope);
			isolatedStorageFile._applicationIdentity = applicationIdentity;
			isolatedStorageFile._fullEvidences = Assembly.GetCallingAssembly().UnprotectedGetEvidence();
			isolatedStorageFile.PostInit();
			return isolatedStorageFile;
		}

		/// <summary>Obtains isolated storage corresponding to the isolation scope and the application identity object.</summary>
		/// <returns>An <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFile" /> object representing the parameters.</returns>
		/// <param name="scope">A bitwise combination of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageScope" /> values. </param>
		/// <param name="applicationEvidenceType">An <see cref="T:System.Security.Policy.Evidence" /> object containing the application identity. </param>
		/// <exception cref="T:System.Security.SecurityException">Sufficient isolated storage permissions have not been granted. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="domainEvidence" /> or <paramref name="assemblyEvidence" /> identity has not been passed in. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="scope" /> is invalid. </exception>
		/// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">An isolated storage location cannot be initialized.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.IsolatedStorageFilePermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, Type applicationEvidenceType)
		{
			IsolatedStorageFile.Demand(scope);
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(scope);
			isolatedStorageFile.InitStore(scope, applicationEvidenceType);
			isolatedStorageFile._fullEvidences = Assembly.GetCallingAssembly().UnprotectedGetEvidence();
			isolatedStorageFile.PostInit();
			return isolatedStorageFile;
		}

		/// <summary>Obtains machine-scoped isolated storage corresponding to the calling code's application identity.</summary>
		/// <returns>An <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFile" /> object corresponding to the isolated storage scope based on the calling code's application identity.</returns>
		/// <exception cref="T:System.Security.SecurityException">Sufficient isolated storage permissions have not been granted. </exception>
		/// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The application identity of the caller cannot be determined.-or- The granted permission set for the <see cref="T:System.AppDomain" /> cannot be determined.-or-An isolated storage location cannot be initialized.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.IsolatedStorageFilePermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static IsolatedStorageFile GetMachineStoreForApplication()
		{
			IsolatedStorageScope scope = IsolatedStorageScope.Machine | IsolatedStorageScope.Application;
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(scope);
			isolatedStorageFile.InitStore(scope, null);
			isolatedStorageFile._fullEvidences = Assembly.GetCallingAssembly().UnprotectedGetEvidence();
			isolatedStorageFile.PostInit();
			return isolatedStorageFile;
		}

		/// <summary>Obtains machine-scoped isolated storage corresponding to the calling code's assembly identity.</summary>
		/// <returns>An <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFile" /> object corresponding to the isolated storage scope based on the calling code's assembly identity.</returns>
		/// <exception cref="T:System.Security.SecurityException">Sufficient isolated storage permissions have not been granted. </exception>
		/// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">An isolated storage location cannot be initialized.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.IsolatedStorageFilePermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static IsolatedStorageFile GetMachineStoreForAssembly()
		{
			IsolatedStorageScope scope = IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine;
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(scope);
			Evidence evidence = Assembly.GetCallingAssembly().UnprotectedGetEvidence();
			isolatedStorageFile._fullEvidences = evidence;
			isolatedStorageFile._assemblyIdentity = IsolatedStorageFile.GetAssemblyIdentityFromEvidence(evidence);
			isolatedStorageFile.PostInit();
			return isolatedStorageFile;
		}

		/// <summary>Obtains machine-scoped isolated storage corresponding to the application domain identity and the assembly identity.</summary>
		/// <returns>An <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFile" /> object corresponding to the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageScope" />, based on a combination of the application domain identity and the assembly identity.</returns>
		/// <exception cref="T:System.Security.SecurityException">Sufficient isolated storage permissions have not been granted. </exception>
		/// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The store failed to open.-or- The assembly specified has insufficient permissions to create isolated stores.-or-An isolated storage location cannot be initialized. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.IsolatedStorageFilePermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static IsolatedStorageFile GetMachineStoreForDomain()
		{
			IsolatedStorageScope scope = IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine;
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(scope);
			isolatedStorageFile._domainIdentity = IsolatedStorageFile.GetDomainIdentityFromEvidence(AppDomain.CurrentDomain.Evidence);
			Evidence evidence = Assembly.GetCallingAssembly().UnprotectedGetEvidence();
			isolatedStorageFile._fullEvidences = evidence;
			isolatedStorageFile._assemblyIdentity = IsolatedStorageFile.GetAssemblyIdentityFromEvidence(evidence);
			isolatedStorageFile.PostInit();
			return isolatedStorageFile;
		}

		/// <summary>Obtains user-scoped isolated storage corresponding to the calling code's application identity.</summary>
		/// <returns>An <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFile" /> object corresponding to the isolated storage scope based on the calling code's assembly identity.</returns>
		/// <exception cref="T:System.Security.SecurityException">Sufficient isolated storage permissions have not been granted. </exception>
		/// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">An isolated storage location cannot be initialized.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.IsolatedStorageFilePermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static IsolatedStorageFile GetUserStoreForApplication()
		{
			IsolatedStorageScope scope = IsolatedStorageScope.User | IsolatedStorageScope.Application;
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(scope);
			isolatedStorageFile.InitStore(scope, null);
			isolatedStorageFile._fullEvidences = Assembly.GetCallingAssembly().UnprotectedGetEvidence();
			isolatedStorageFile.PostInit();
			return isolatedStorageFile;
		}

		/// <summary>Obtains user-scoped isolated storage corresponding to the calling code's assembly identity.</summary>
		/// <returns>An <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFile" /> object corresponding to the isolated storage scope based on the calling code's assembly identity.</returns>
		/// <exception cref="T:System.Security.SecurityException">Sufficient isolated storage permissions have not been granted. </exception>
		/// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">An isolated storage location cannot be initialized.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.IsolatedStorageFilePermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static IsolatedStorageFile GetUserStoreForAssembly()
		{
			IsolatedStorageScope scope = IsolatedStorageScope.User | IsolatedStorageScope.Assembly;
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(scope);
			Evidence evidence = Assembly.GetCallingAssembly().UnprotectedGetEvidence();
			isolatedStorageFile._fullEvidences = evidence;
			isolatedStorageFile._assemblyIdentity = IsolatedStorageFile.GetAssemblyIdentityFromEvidence(evidence);
			isolatedStorageFile.PostInit();
			return isolatedStorageFile;
		}

		/// <summary>Obtains user-scoped isolated storage corresponding to the application domain identity and assembly identity.</summary>
		/// <returns>An <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFile" /> object corresponding to the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageScope" />, based on a combination of the application domain identity and the assembly identity.</returns>
		/// <exception cref="T:System.Security.SecurityException">Sufficient isolated storage permissions have not been granted. </exception>
		/// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The store failed to open.-or- The assembly specified has insufficient permissions to create isolated stores. -or-An isolated storage location cannot be initialized.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.IsolatedStorageFilePermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static IsolatedStorageFile GetUserStoreForDomain()
		{
			IsolatedStorageScope scope = IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly;
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(scope);
			isolatedStorageFile._domainIdentity = IsolatedStorageFile.GetDomainIdentityFromEvidence(AppDomain.CurrentDomain.Evidence);
			Evidence evidence = Assembly.GetCallingAssembly().UnprotectedGetEvidence();
			isolatedStorageFile._fullEvidences = evidence;
			isolatedStorageFile._assemblyIdentity = IsolatedStorageFile.GetAssemblyIdentityFromEvidence(evidence);
			isolatedStorageFile.PostInit();
			return isolatedStorageFile;
		}

		/// <summary>Removes the specified isolated storage scope for all identities.</summary>
		/// <param name="scope">A bitwise combination of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageScope" /> values. </param>
		/// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store cannot be removed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.IsolatedStorageFilePermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static void Remove(IsolatedStorageScope scope)
		{
			string isolatedStorageRoot = IsolatedStorageFile.GetIsolatedStorageRoot(scope);
			Directory.Delete(isolatedStorageRoot, true);
		}

		internal static string GetIsolatedStorageRoot(IsolatedStorageScope scope)
		{
			string text = null;
			if ((scope & IsolatedStorageScope.User) != IsolatedStorageScope.None)
			{
				if ((scope & IsolatedStorageScope.Roaming) != IsolatedStorageScope.None)
				{
					text = Environment.InternalGetFolderPath(Environment.SpecialFolder.LocalApplicationData);
				}
				else
				{
					text = Environment.InternalGetFolderPath(Environment.SpecialFolder.ApplicationData);
				}
			}
			else if ((scope & IsolatedStorageScope.Machine) != IsolatedStorageScope.None)
			{
				text = Environment.InternalGetFolderPath(Environment.SpecialFolder.CommonApplicationData);
			}
			if (text == null)
			{
				string text2 = Locale.GetText("Couldn't access storage location for '{0}'.");
				throw new IsolatedStorageException(string.Format(text2, scope));
			}
			return Path.Combine(text, ".isolated-storage");
		}

		private static void Demand(IsolatedStorageScope scope)
		{
			if (SecurityManager.SecurityEnabled)
			{
				new IsolatedStorageFilePermission(PermissionState.None)
				{
					UsageAllowed = IsolatedStorageFile.ScopeToContainment(scope)
				}.Demand();
			}
		}

		private static IsolatedStorageContainment ScopeToContainment(IsolatedStorageScope scope)
		{
			switch (scope)
			{
			case IsolatedStorageScope.User | IsolatedStorageScope.Assembly:
				return IsolatedStorageContainment.AssemblyIsolationByUser;
			default:
				switch (scope)
				{
				case IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming:
					return IsolatedStorageContainment.AssemblyIsolationByRoamingUser;
				default:
					switch (scope)
					{
					case IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine:
						return IsolatedStorageContainment.AssemblyIsolationByMachine;
					default:
						if (scope == (IsolatedStorageScope.User | IsolatedStorageScope.Application))
						{
							return IsolatedStorageContainment.ApplicationIsolationByUser;
						}
						if (scope == (IsolatedStorageScope.User | IsolatedStorageScope.Roaming | IsolatedStorageScope.Application))
						{
							return IsolatedStorageContainment.ApplicationIsolationByRoamingUser;
						}
						if (scope != (IsolatedStorageScope.Machine | IsolatedStorageScope.Application))
						{
							return IsolatedStorageContainment.UnrestrictedIsolatedStorage;
						}
						return IsolatedStorageContainment.ApplicationIsolationByMachine;
					case IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine:
						return IsolatedStorageContainment.DomainIsolationByMachine;
					}
					break;
				case IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming:
					return IsolatedStorageContainment.DomainIsolationByRoamingUser;
				}
				break;
			case IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly:
				return IsolatedStorageContainment.DomainIsolationByUser;
			}
		}

		internal static ulong GetDirectorySize(DirectoryInfo di)
		{
			ulong num = 0UL;
			foreach (FileInfo fileInfo in di.GetFiles())
			{
				num += (ulong)fileInfo.Length;
			}
			foreach (DirectoryInfo di2 in di.GetDirectories())
			{
				num += IsolatedStorageFile.GetDirectorySize(di2);
			}
			return num;
		}

		~IsolatedStorageFile()
		{
		}

		private void PostInit()
		{
			string text = IsolatedStorageFile.GetIsolatedStorageRoot(base.Scope);
			string path;
			if (this._applicationIdentity != null)
			{
				path = string.Format("a{0}{1}", this.SeparatorInternal, this.GetNameFromIdentity(this._applicationIdentity));
			}
			else if (this._domainIdentity != null)
			{
				path = string.Format("d{0}{1}{0}{2}", this.SeparatorInternal, this.GetNameFromIdentity(this._domainIdentity), this.GetNameFromIdentity(this._assemblyIdentity));
			}
			else
			{
				if (this._assemblyIdentity == null)
				{
					throw new IsolatedStorageException(Locale.GetText("No code identity available."));
				}
				path = string.Format("d{0}none{0}{1}", this.SeparatorInternal, this.GetNameFromIdentity(this._assemblyIdentity));
			}
			text = Path.Combine(text, path);
			this.directory = new DirectoryInfo(text);
			if (!this.directory.Exists)
			{
				try
				{
					this.directory.Create();
					this.SaveIdentities(text);
				}
				catch (IOException)
				{
				}
			}
		}

		/// <summary>Gets the current size of the isolated storage.</summary>
		/// <returns>The total number of bytes of storage currently in use within the isolated storage scope.</returns>
		/// <exception cref="T:System.InvalidOperationException">The property is unavailable. The current store has a roaming scope or is not open. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The current object size is undefined.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[CLSCompliant(false)]
		public override ulong CurrentSize
		{
			get
			{
				return IsolatedStorageFile.GetDirectorySize(this.directory);
			}
		}

		/// <summary>Gets a value representing the maximum amount of space available for isolated storage within the limits established by the quota.</summary>
		/// <returns>The limit of isolated storage space in bytes.</returns>
		/// <exception cref="T:System.InvalidOperationException">The property is unavailable. <see cref="P:System.IO.IsolatedStorage.IsolatedStorageFile.MaximumSize" /> cannot be determined without evidence from the assembly's creation. The evidence could not be determined when the object was created. </exception>
		[CLSCompliant(false)]
		public override ulong MaximumSize
		{
			get
			{
				if (!SecurityManager.SecurityEnabled)
				{
					return 9223372036854775807UL;
				}
				if (this._resolved)
				{
					return this._maxSize;
				}
				Evidence evidence;
				if (this._fullEvidences != null)
				{
					evidence = this._fullEvidences;
				}
				else
				{
					evidence = new Evidence();
					if (this._assemblyIdentity != null)
					{
						evidence.AddHost(this._assemblyIdentity);
					}
				}
				if (evidence.Count < 1)
				{
					throw new InvalidOperationException(Locale.GetText("Couldn't get the quota from the available evidences."));
				}
				PermissionSet permissionSet = null;
				PermissionSet permissionSet2 = SecurityManager.ResolvePolicy(evidence, null, null, null, out permissionSet);
				IsolatedStoragePermission permission = this.GetPermission(permissionSet2);
				if (permission == null)
				{
					if (!permissionSet2.IsUnrestricted())
					{
						throw new InvalidOperationException(Locale.GetText("No quota from the available evidences."));
					}
					this._maxSize = 9223372036854775807UL;
				}
				else
				{
					this._maxSize = (ulong)permission.UserQuota;
				}
				this._resolved = true;
				return this._maxSize;
			}
		}

		internal string Root
		{
			get
			{
				return this.directory.FullName;
			}
		}

		/// <summary>Closes a store previously opened with <see cref="M:System.IO.IsolatedStorage.IsolatedStorageFile.GetStore(System.IO.IsolatedStorage.IsolatedStorageScope,System.Type,System.Type)" />, <see cref="M:System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly" />, or <see cref="M:System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForDomain" />.</summary>
		public void Close()
		{
		}

		/// <summary>Creates a directory in the isolated storage scope.</summary>
		/// <param name="dir">The relative path of the directory to create within the isolated storage scope. </param>
		/// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The current code has insufficient permissions to create isolated storage directory. </exception>
		/// <exception cref="T:System.ArgumentNullException">The directory path is null. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public void CreateDirectory(string dir)
		{
			if (dir == null)
			{
				throw new ArgumentNullException("dir");
			}
			if (dir.IndexOfAny(Path.PathSeparatorChars) < 0)
			{
				if (this.directory.GetFiles(dir).Length > 0)
				{
					throw new IOException(Locale.GetText("Directory name already exists as a file."));
				}
				this.directory.CreateSubdirectory(dir);
			}
			else
			{
				string[] array = dir.Split(Path.PathSeparatorChars);
				DirectoryInfo directoryInfo = this.directory;
				for (int i = 0; i < array.Length; i++)
				{
					if (directoryInfo.GetFiles(array[i]).Length > 0)
					{
						throw new IOException(Locale.GetText("Part of the directory name already exists as a file."));
					}
					directoryInfo = directoryInfo.CreateSubdirectory(array[i]);
				}
			}
		}

		/// <summary>Deletes a directory in the isolated storage scope.</summary>
		/// <param name="dir">The relative path of the directory to delete within the isolated storage scope. </param>
		/// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The directory could not be deleted. </exception>
		/// <exception cref="T:System.ArgumentNullException">The directory path was null. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public void DeleteDirectory(string dir)
		{
			try
			{
				DirectoryInfo directoryInfo = this.directory.CreateSubdirectory(dir);
				directoryInfo.Delete();
			}
			catch
			{
				throw new IsolatedStorageException(Locale.GetText("Could not delete directory '{0}'", new object[]
				{
					dir
				}));
			}
		}

		/// <summary>Deletes a file in the isolated storage scope.</summary>
		/// <param name="file">The relative path of the file to delete within the isolated storage scope. </param>
		/// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The target file is open or the path is incorrect. </exception>
		/// <exception cref="T:System.ArgumentNullException">The file path is null. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public void DeleteFile(string file)
		{
			File.Delete(Path.Combine(this.directory.FullName, file));
		}

		/// <summary>Releases all resources used by the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFile" />. </summary>
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		/// <summary>Enumerates directories in an isolated storage scope that match a given pattern.</summary>
		/// <returns>An <see cref="T:System.Array" /> of the relative paths of directories in the isolated storage scope that match <paramref name="searchPattern" />. A zero-length array specifies that there are no directories that match.</returns>
		/// <param name="searchPattern">A search pattern. Both single-character ("?") and multi-character ("*") wildcards are supported. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="searchPattern" /> was null. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have permission to enumerate directories resolved from <paramref name="searchPattern" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The isolated store has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The isolated store is closed.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The directory or directories specified by <paramref name="searchPattern" /> are not found.</exception>
		/// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store has been removed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public string[] GetDirectoryNames(string searchPattern)
		{
			if (searchPattern == null)
			{
				throw new ArgumentNullException("searchPattern");
			}
			string directoryName = Path.GetDirectoryName(searchPattern);
			string fileName = Path.GetFileName(searchPattern);
			DirectoryInfo[] directories;
			if (directoryName == null || directoryName.Length == 0)
			{
				directories = this.directory.GetDirectories(searchPattern);
			}
			else
			{
				DirectoryInfo[] directories2 = this.directory.GetDirectories(directoryName);
				if (directories2.Length != 1 || !(directories2[0].Name == directoryName) || directories2[0].FullName.IndexOf(this.directory.FullName) < 0)
				{
					throw new SecurityException();
				}
				directories = directories2[0].GetDirectories(fileName);
			}
			return this.GetNames(directories);
		}

		private string[] GetNames(FileSystemInfo[] afsi)
		{
			string[] array = new string[afsi.Length];
			for (int num = 0; num != afsi.Length; num++)
			{
				array[num] = afsi[num].Name;
			}
			return array;
		}

		/// <summary>Enumerates files in isolated storage scope that match a given pattern.</summary>
		/// <returns>An <see cref="T:System.Array" /> of relative paths of files in the isolated storage scope that match <paramref name="searchPattern" />. A zero-length array specifies that there are no files that match.</returns>
		/// <param name="searchPattern">A search pattern. Both single-character ("?") and multi-character ("*") wildcards are supported. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="searchPattern" /> was null. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The file path specified by <paramref name="searchPattern" /> cannot be found.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public string[] GetFileNames(string searchPattern)
		{
			if (searchPattern == null)
			{
				throw new ArgumentNullException("searchPattern");
			}
			string directoryName = Path.GetDirectoryName(searchPattern);
			string fileName = Path.GetFileName(searchPattern);
			FileInfo[] files;
			if (directoryName == null || directoryName.Length == 0)
			{
				files = this.directory.GetFiles(searchPattern);
			}
			else
			{
				DirectoryInfo[] directories = this.directory.GetDirectories(directoryName);
				if (directories.Length != 1 || !(directories[0].Name == directoryName) || directories[0].FullName.IndexOf(this.directory.FullName) < 0)
				{
					throw new SecurityException();
				}
				files = directories[0].GetFiles(fileName);
			}
			return this.GetNames(files);
		}

		/// <summary>Removes the isolated storage scope and all its contents.</summary>
		/// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The isolated store cannot be deleted. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public override void Remove()
		{
			this.directory.Delete(true);
		}

		protected override IsolatedStoragePermission GetPermission(PermissionSet ps)
		{
			if (ps == null)
			{
				return null;
			}
			return (IsolatedStoragePermission)ps.GetPermission(typeof(IsolatedStorageFilePermission));
		}

		private string GetNameFromIdentity(object identity)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(identity.ToString());
			SHA1 sha = SHA1.Create();
			byte[] src = sha.ComputeHash(bytes, 0, bytes.Length);
			byte[] array = new byte[10];
			Buffer.BlockCopy(src, 0, array, 0, array.Length);
			return CryptoConvert.ToHex(array);
		}

		private static object GetTypeFromEvidence(Evidence e, Type t)
		{
			foreach (object obj in e)
			{
				if (obj.GetType() == t)
				{
					return obj;
				}
			}
			return null;
		}

		internal static object GetAssemblyIdentityFromEvidence(Evidence e)
		{
			object typeFromEvidence = IsolatedStorageFile.GetTypeFromEvidence(e, typeof(Publisher));
			if (typeFromEvidence != null)
			{
				return typeFromEvidence;
			}
			typeFromEvidence = IsolatedStorageFile.GetTypeFromEvidence(e, typeof(StrongName));
			if (typeFromEvidence != null)
			{
				return typeFromEvidence;
			}
			return IsolatedStorageFile.GetTypeFromEvidence(e, typeof(Url));
		}

		internal static object GetDomainIdentityFromEvidence(Evidence e)
		{
			object typeFromEvidence = IsolatedStorageFile.GetTypeFromEvidence(e, typeof(ApplicationDirectory));
			if (typeFromEvidence != null)
			{
				return typeFromEvidence;
			}
			return IsolatedStorageFile.GetTypeFromEvidence(e, typeof(Url));
		}

		private void SaveIdentities(string root)
		{
			IsolatedStorageFile.Identities identities = new IsolatedStorageFile.Identities(this._applicationIdentity, this._assemblyIdentity, this._domainIdentity);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			IsolatedStorageFile.mutex.WaitOne();
			try
			{
				using (FileStream fileStream = File.Create(root + ".storage"))
				{
					binaryFormatter.Serialize(fileStream, identities);
				}
			}
			finally
			{
				IsolatedStorageFile.mutex.ReleaseMutex();
			}
		}

		[Serializable]
		private struct Identities
		{
			public object Application;

			public object Assembly;

			public object Domain;

			public Identities(object application, object assembly, object domain)
			{
				this.Application = application;
				this.Assembly = assembly;
				this.Domain = domain;
			}
		}
	}
}
