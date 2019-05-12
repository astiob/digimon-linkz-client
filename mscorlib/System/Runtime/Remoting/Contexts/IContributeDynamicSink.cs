using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Contexts
{
	/// <summary>Indicates that the implementing property will be registered at runtime through the <see cref="M:System.Runtime.Remoting.Contexts.Context.RegisterDynamicProperty(System.Runtime.Remoting.Contexts.IDynamicProperty,System.ContextBoundObject,System.Runtime.Remoting.Contexts.Context)" /> method.</summary>
	[ComVisible(true)]
	public interface IContributeDynamicSink
	{
		/// <summary>Returns the message sink that will be notified of call start and finish events through the <see cref="T:System.Runtime.Remoting.Contexts.IDynamicMessageSink" /> interface.</summary>
		/// <returns>A dynamic sink that exposes the <see cref="T:System.Runtime.Remoting.Contexts.IDynamicMessageSink" /> interface.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		IDynamicMessageSink GetDynamicSink();
	}
}
