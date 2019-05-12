using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting
{
	/// <summary>Wraps marshal-by-value object references, allowing them to be returned through an indirection.</summary>
	[ClassInterface(ClassInterfaceType.AutoDual)]
	[ComVisible(true)]
	public class ObjectHandle : MarshalByRefObject, IObjectHandle
	{
		private object _wrapped;

		/// <summary>Initializes an instance of the <see cref="T:System.Runtime.Remoting.ObjectHandle" /> class, wrapping the given object <paramref name="o" />.</summary>
		/// <param name="o">The object that is wrapped by the new <see cref="T:System.Runtime.Remoting.ObjectHandle" />. </param>
		public ObjectHandle(object o)
		{
			this._wrapped = o;
		}

		/// <summary>Initializes the lifetime lease of the wrapped object.</summary>
		/// <returns>An initialized <see cref="T:System.Runtime.Remoting.Lifetime.ILease" /> that allows you to control the lifetime of the wrapped object.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure" />
		/// </PermissionSet>
		public override object InitializeLifetimeService()
		{
			return base.InitializeLifetimeService();
		}

		/// <summary>Returns the wrapped object.</summary>
		/// <returns>The wrapped object.</returns>
		public object Unwrap()
		{
			return this._wrapped;
		}
	}
}
