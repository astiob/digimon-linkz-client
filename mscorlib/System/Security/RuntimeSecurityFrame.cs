using System;
using System.Reflection;

namespace System.Security
{
	internal class RuntimeSecurityFrame
	{
		public AppDomain domain;

		public MethodInfo method;

		public RuntimeDeclSecurityEntry assert;

		public RuntimeDeclSecurityEntry deny;

		public RuntimeDeclSecurityEntry permitonly;
	}
}
