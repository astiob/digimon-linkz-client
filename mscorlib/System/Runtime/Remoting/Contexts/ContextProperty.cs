using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Contexts
{
	/// <summary>Holds the name/value pair of the property name and the object representing the property of a context.</summary>
	[ComVisible(true)]
	public class ContextProperty
	{
		private string name;

		private object prop;

		private ContextProperty(string name, object prop)
		{
			this.name = name;
			this.prop = prop;
		}

		/// <summary>Gets the name of the T:System.Runtime.Remoting.Contexts.ContextProperty class.</summary>
		/// <returns>The name of the <see cref="T:System.Runtime.Remoting.Contexts.ContextProperty" /> class.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual string Name
		{
			get
			{
				return this.name;
			}
		}

		/// <summary>Gets the object representing the property of a context.</summary>
		/// <returns>The object representing the property of a context.</returns>
		public virtual object Property
		{
			get
			{
				return this.prop;
			}
		}
	}
}
