using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace System.Runtime.Remoting.Services
{
	/// <summary>Provides APIs that are needed for communication and operation with unmanaged classes outside of the <see cref="T:System.AppDomain" />. This class cannot be inherited.</summary>
	[ComVisible(true)]
	public sealed class EnterpriseServicesHelper
	{
		/// <summary>Constructs a <see cref="T:System.Runtime.Remoting.Messaging.ReturnMessage" /> from the specified <see cref="T:System.Runtime.Remoting.Activation.IConstructionCallMessage" />.</summary>
		/// <returns>A <see cref="T:System.Runtime.Remoting.Activation.IConstructionReturnMessage" /> returned from the construction call that is specified in the <paramref name="ctorMsg" /> parameter.</returns>
		/// <param name="ctorMsg">A construction call to the object from which the new <see cref="T:System.Runtime.Remoting.Messaging.ReturnMessage" /> instance is returning. </param>
		/// <param name="retObj">A <see cref="T:System.Runtime.Remoting.ObjRef" /> that represents the object that is constructed with the construction call in <paramref name="ctorMsg" />. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		[ComVisible(true)]
		public static IConstructionReturnMessage CreateConstructionReturnMessage(IConstructionCallMessage ctorMsg, MarshalByRefObject retObj)
		{
			return new ConstructionResponse(retObj, null, ctorMsg);
		}

		/// <summary>Switches a COM Callable Wrapper (CCW) from one instance of a class to another instance of the same class.</summary>
		/// <param name="oldcp">A proxy that represents the old instance of a class that is referenced by a CCW. </param>
		/// <param name="newcp">A proxy that represents the new instance of a class that is referenced by a CCW. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have UnmanagedCode permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, Infrastructure" />
		/// </PermissionSet>
		[MonoTODO]
		public static void SwitchWrappers(RealProxy oldcp, RealProxy newcp)
		{
			throw new NotSupportedException();
		}

		/// <summary>Wraps the specified IUnknown COM interface with a Runtime Callable Wrapper (RCW).</summary>
		/// <returns>The RCW where the specified IUnknown is wrapped.</returns>
		/// <param name="punk">A pointer to the IUnknown COM interface to wrap. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have UnmanagedCode permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, Infrastructure" />
		/// </PermissionSet>
		[MonoTODO]
		public static object WrapIUnknownWithComObject(IntPtr punk)
		{
			throw new NotSupportedException();
		}
	}
}
