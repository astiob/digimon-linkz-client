using System;
using System.Reflection;
using System.Security;
using UnityEngine.Internal;

namespace UnityEngine
{
	/// <summary>
	///   <para>Webplayer security related class.</para>
	/// </summary>
	public sealed class Security
	{
		/// <summary>
		///   <para>Prefetch the webplayer socket security policy from a non-default port number.</para>
		/// </summary>
		/// <param name="ip">IP address of server.</param>
		/// <param name="atPort">Port from where socket policy is read.</param>
		/// <param name="timeout">Time to wait for response.</param>
		[ExcludeFromDocs]
		public static bool PrefetchSocketPolicy(string ip, int atPort)
		{
			int timeout = 3000;
			return Security.PrefetchSocketPolicy(ip, atPort, timeout);
		}

		/// <summary>
		///   <para>Prefetch the webplayer socket security policy from a non-default port number.</para>
		/// </summary>
		/// <param name="ip">IP address of server.</param>
		/// <param name="atPort">Port from where socket policy is read.</param>
		/// <param name="timeout">Time to wait for response.</param>
		public static bool PrefetchSocketPolicy(string ip, int atPort, [DefaultValue("3000")] int timeout)
		{
			return true;
		}

		private static MethodInfo GetUnityCrossDomainHelperMethod(string methodname)
		{
			Type type = Types.GetType("UnityEngine.UnityCrossDomainHelper", "CrossDomainPolicyParser, Version=1.0.0.0, Culture=neutral");
			if (type == null)
			{
				throw new SecurityException("Cant find type UnityCrossDomainHelper");
			}
			MethodInfo method = type.GetMethod(methodname);
			if (method == null)
			{
				throw new SecurityException("Cant find " + methodname);
			}
			return method;
		}

		internal static string TokenToHex(byte[] token)
		{
			if (token == null || 8 > token.Length)
			{
				return string.Empty;
			}
			return string.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}{5:x2}{6:x2}{7:x2}", new object[]
			{
				token[0],
				token[1],
				token[2],
				token[3],
				token[4],
				token[5],
				token[6],
				token[7]
			});
		}

		/// <summary>
		///   <para>Loads an assembly and checks that it is allowed to be used in the webplayer.
		/// Note: The single argument version of this API will always issue an error message.  An authorisation key is always needed.</para>
		/// </summary>
		/// <param name="assemblyData">Assembly to verify.</param>
		/// <param name="authorizationKey">Public key used to verify assembly.</param>
		/// <returns>
		///   <para>Loaded, verified, assembly, or null if the assembly cannot be verfied.</para>
		/// </returns>
		[SecuritySafeCritical]
		public static Assembly LoadAndVerifyAssembly(byte[] assemblyData, string authorizationKey)
		{
			return null;
		}

		/// <summary>
		///   <para>Loads an assembly and checks that it is allowed to be used in the webplayer.
		/// Note: The single argument version of this API will always issue an error message.  An authorisation key is always needed.</para>
		/// </summary>
		/// <param name="assemblyData">Assembly to verify.</param>
		/// <param name="authorizationKey">Public key used to verify assembly.</param>
		/// <returns>
		///   <para>Loaded, verified, assembly, or null if the assembly cannot be verfied.</para>
		/// </returns>
		[SecuritySafeCritical]
		public static Assembly LoadAndVerifyAssembly(byte[] assemblyData)
		{
			return null;
		}

		[SecuritySafeCritical]
		private static Assembly LoadAndVerifyAssemblyInternal(byte[] assemblyData)
		{
			return null;
		}
	}
}
