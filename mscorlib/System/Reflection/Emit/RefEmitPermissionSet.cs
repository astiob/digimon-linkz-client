using System;
using System.Security.Permissions;

namespace System.Reflection.Emit
{
	internal struct RefEmitPermissionSet
	{
		public SecurityAction action;

		public string pset;

		public RefEmitPermissionSet(SecurityAction action, string pset)
		{
			this.action = action;
			this.pset = pset;
		}
	}
}
