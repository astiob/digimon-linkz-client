using System;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

namespace System.Runtime.InteropServices
{
	/// <summary>Provides a collection of static methods that return information about the common language runtime environment.</summary>
	[ComVisible(true)]
	public class RuntimeEnvironment
	{
		/// <summary>Gets the path to the system configuration file.</summary>
		/// <returns>The path to the system configuration file.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static string SystemConfigurationFile
		{
			get
			{
				string machineConfigPath = Environment.GetMachineConfigPath();
				if (SecurityManager.SecurityEnabled)
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, machineConfigPath).Demand();
				}
				return machineConfigPath;
			}
		}

		/// <summary>Tests whether the specified assembly is loaded in the global assembly cache (GAC).</summary>
		/// <returns>true if the assembly is loaded in the GAC; otherwise, false.</returns>
		/// <param name="a">The assembly to determine if it is loaded in the GAC. </param>
		public static bool FromGlobalAccessCache(Assembly a)
		{
			return a.GlobalAssemblyCache;
		}

		/// <summary>Gets the directory where the common language runtime is installed.</summary>
		/// <returns>A string containing the path to the directory where the common language runtime is installed.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static string GetRuntimeDirectory()
		{
			return Path.GetDirectoryName(typeof(int).Assembly.Location);
		}

		/// <summary>Gets the version number of the common language runtime that is running the current process.</summary>
		/// <returns>A string containing the version number of the common language runtime.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static string GetSystemVersion()
		{
			return string.Concat(new object[]
			{
				"v",
				Environment.Version.Major,
				".",
				Environment.Version.Minor,
				".",
				Environment.Version.Build
			});
		}
	}
}
