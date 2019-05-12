using System;

namespace System.Runtime.Versioning
{
	/// <summary>Provides methods to aid developers in writing version-safe code. This class cannot be inherited.</summary>
	public static class VersioningHelper
	{
		private static int GetDomainId()
		{
			return 0;
		}

		private static int GetProcessId()
		{
			return 0;
		}

		private static string SafeName(string name, bool process, bool appdomain)
		{
			if (process && appdomain)
			{
				return string.Concat(new string[]
				{
					name,
					"_",
					VersioningHelper.GetProcessId().ToString(),
					"_",
					VersioningHelper.GetDomainId().ToString()
				});
			}
			if (process)
			{
				return name + "_" + VersioningHelper.GetProcessId().ToString();
			}
			if (appdomain)
			{
				return name + "_" + VersioningHelper.GetDomainId().ToString();
			}
			return name;
		}

		private static string ConvertFromMachine(string name, ResourceScope to, Type type)
		{
			switch (to)
			{
			case ResourceScope.Machine:
				return VersioningHelper.SafeName(name, false, false);
			case ResourceScope.Process:
				return VersioningHelper.SafeName(name, true, false);
			case ResourceScope.AppDomain:
				return VersioningHelper.SafeName(name, true, true);
			}
			throw new ArgumentException("to");
		}

		private static string ConvertFromProcess(string name, ResourceScope to, Type type)
		{
			if (to < ResourceScope.Process || to >= ResourceScope.Private)
			{
				throw new ArgumentException("to");
			}
			bool appdomain = (to & ResourceScope.AppDomain) == ResourceScope.AppDomain;
			return VersioningHelper.SafeName(name, false, appdomain);
		}

		private static string ConvertFromAppDomain(string name, ResourceScope to, Type type)
		{
			if (to < ResourceScope.AppDomain || to >= ResourceScope.Private)
			{
				throw new ArgumentException("to");
			}
			return VersioningHelper.SafeName(name, false, false);
		}

		/// <summary>Returns a version-safe name based on the specified resource name and the intended resource consumption source.</summary>
		/// <returns>A version-safe name.</returns>
		/// <param name="name">The name of the resource.</param>
		/// <param name="from">The scope of the resource.</param>
		/// <param name="to">The desired resource consumption scope.</param>
		[MonoTODO("process id is always 0")]
		public static string MakeVersionSafeName(string name, ResourceScope from, ResourceScope to)
		{
			return VersioningHelper.MakeVersionSafeName(name, from, to, null);
		}

		/// <summary>Returns a version-safe name based on the specified resource name, the intended resource consumption scope, and the type using the resource.</summary>
		/// <returns>A version-safe name.</returns>
		/// <param name="name">The name of the resource.</param>
		/// <param name="from">The beginning of the scope range.</param>
		/// <param name="to">The end of the scope range.</param>
		/// <param name="type">The <see cref="T:System.Type" /> of the resource.</param>
		/// <exception cref="T:System.ArgumentException">The values for <paramref name="from " />and <paramref name="to " />are invalid. The resource type in the <see cref="T:System.Runtime.Versioning.ResourceScope" />  enumeration is going from a more restrictive resource type to a more general resource type.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type " />is null.</exception>
		[MonoTODO("type?")]
		public static string MakeVersionSafeName(string name, ResourceScope from, ResourceScope to, Type type)
		{
			if ((from & ResourceScope.Private) != ResourceScope.None)
			{
				to &= ~(ResourceScope.Private | ResourceScope.Assembly);
			}
			else if ((from & ResourceScope.Assembly) != ResourceScope.None)
			{
				to &= ~ResourceScope.Assembly;
			}
			string name2 = (name != null) ? name : string.Empty;
			switch (from)
			{
			case ResourceScope.Machine:
				break;
			case ResourceScope.Process:
				goto IL_8F;
			default:
				switch (from)
				{
				case ResourceScope.Machine | ResourceScope.Private:
					break;
				case ResourceScope.Process | ResourceScope.Private:
					goto IL_8F;
				default:
					switch (from)
					{
					case ResourceScope.Machine | ResourceScope.Assembly:
						goto IL_86;
					case ResourceScope.Process | ResourceScope.Assembly:
						goto IL_8F;
					case ResourceScope.AppDomain | ResourceScope.Assembly:
						goto IL_98;
					}
					throw new ArgumentException("from");
				case ResourceScope.AppDomain | ResourceScope.Private:
					goto IL_98;
				}
				break;
			case ResourceScope.AppDomain:
				goto IL_98;
			}
			IL_86:
			return VersioningHelper.ConvertFromMachine(name2, to, type);
			IL_8F:
			return VersioningHelper.ConvertFromProcess(name2, to, type);
			IL_98:
			return VersioningHelper.ConvertFromAppDomain(name2, to, type);
		}
	}
}
