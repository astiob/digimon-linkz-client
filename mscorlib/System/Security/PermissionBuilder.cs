using System;
using System.Security.Permissions;

namespace System.Security
{
	internal static class PermissionBuilder
	{
		private static object[] psNone = new object[]
		{
			PermissionState.None
		};

		public static IPermission Create(string fullname, PermissionState state)
		{
			if (fullname == null)
			{
				throw new ArgumentNullException("fullname");
			}
			SecurityElement securityElement = new SecurityElement("IPermission");
			securityElement.AddAttribute("class", fullname);
			securityElement.AddAttribute("version", "1");
			if (state == PermissionState.Unrestricted)
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			return PermissionBuilder.CreatePermission(fullname, securityElement);
		}

		public static IPermission Create(SecurityElement se)
		{
			if (se == null)
			{
				throw new ArgumentNullException("se");
			}
			string text = se.Attribute("class");
			if (text == null || text.Length == 0)
			{
				throw new ArgumentException("class");
			}
			return PermissionBuilder.CreatePermission(text, se);
		}

		public static IPermission Create(string fullname, SecurityElement se)
		{
			if (fullname == null)
			{
				throw new ArgumentNullException("fullname");
			}
			if (se == null)
			{
				throw new ArgumentNullException("se");
			}
			return PermissionBuilder.CreatePermission(fullname, se);
		}

		public static IPermission Create(Type type)
		{
			return (IPermission)Activator.CreateInstance(type, PermissionBuilder.psNone);
		}

		internal static IPermission CreatePermission(string fullname, SecurityElement se)
		{
			Type type = Type.GetType(fullname);
			if (type == null)
			{
				string text = Locale.GetText("Can't create an instance of permission class {0}.");
				throw new TypeLoadException(string.Format(text, fullname));
			}
			IPermission permission = PermissionBuilder.Create(type);
			permission.FromXml(se);
			return permission;
		}
	}
}
