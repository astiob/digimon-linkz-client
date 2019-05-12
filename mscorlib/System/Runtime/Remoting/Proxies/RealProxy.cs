using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting.Proxies
{
	/// <summary>Provides base functionality for proxies.</summary>
	[ComVisible(true)]
	public abstract class RealProxy
	{
		private Type class_to_proxy;

		internal Context _targetContext;

		private MarshalByRefObject _server;

		private int _targetDomainId = -1;

		internal string _targetUri;

		internal Identity _objectIdentity;

		private object _objTP;

		private object _stubData;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Proxies.RealProxy" /> class with default values.</summary>
		protected RealProxy()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Proxies.RealProxy" /> class that represents a remote object of the specified <see cref="T:System.Type" />.</summary>
		/// <param name="classToProxy">The <see cref="T:System.Type" /> of the remote object for which to create a proxy. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="classToProxy" /> is not an interface, and is not derived from <see cref="T:System.MarshalByRefObject" />. </exception>
		protected RealProxy(Type classToProxy) : this(classToProxy, IntPtr.Zero, null)
		{
		}

		internal RealProxy(Type classToProxy, ClientIdentity identity) : this(classToProxy, IntPtr.Zero, null)
		{
			this._objectIdentity = identity;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Proxies.RealProxy" /> class.</summary>
		/// <param name="classToProxy">The <see cref="T:System.Type" /> of the remote object for which to create a proxy. </param>
		/// <param name="stub">A stub to associate with the new proxy instance. </param>
		/// <param name="stubData">The stub data to set for the specified stub and the new proxy instance. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="classToProxy" /> is not an interface, and is not derived from <see cref="T:System.MarshalByRefObject" />. </exception>
		protected RealProxy(Type classToProxy, IntPtr stub, object stubData)
		{
			if (!classToProxy.IsMarshalByRef && !classToProxy.IsInterface)
			{
				throw new ArgumentException("object must be MarshalByRef");
			}
			this.class_to_proxy = classToProxy;
			if (stub != IntPtr.Zero)
			{
				throw new NotSupportedException("stub is not used in Mono");
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Type InternalGetProxyType(object transparentProxy);

		/// <summary>Returns the <see cref="T:System.Type" /> of the object that the current instance of <see cref="T:System.Runtime.Remoting.Proxies.RealProxy" /> represents.</summary>
		/// <returns>The <see cref="T:System.Type" /> of the object that the current instance of <see cref="T:System.Runtime.Remoting.Proxies.RealProxy" /> represents.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public Type GetProxiedType()
		{
			if (this._objTP != null)
			{
				return RealProxy.InternalGetProxyType(this._objTP);
			}
			if (this.class_to_proxy.IsInterface)
			{
				return typeof(MarshalByRefObject);
			}
			return this.class_to_proxy;
		}

		/// <summary>Creates an <see cref="T:System.Runtime.Remoting.ObjRef" /> for the specified object type, and registers it with the remoting infrastructure as a client-activated object.</summary>
		/// <returns>A new instance of <see cref="T:System.Runtime.Remoting.ObjRef" /> that is created for the specified type.</returns>
		/// <param name="requestedType">The object type that an <see cref="T:System.Runtime.Remoting.ObjRef" /> is created for. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual ObjRef CreateObjRef(Type requestedType)
		{
			return RemotingServices.Marshal((MarshalByRefObject)this.GetTransparentProxy(), null, requestedType);
		}

		/// <summary>Adds the transparent proxy of the object represented by the current instance of <see cref="T:System.Runtime.Remoting.Proxies.RealProxy" /> to the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" />.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> into which the transparent proxy is serialized. </param>
		/// <param name="context">The source and destination of the serialization. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> or <paramref name="context" /> parameter is null. </exception>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have SerializationFormatter permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			object transparentProxy = this.GetTransparentProxy();
			RemotingServices.GetObjectData(transparentProxy, info, context);
		}

		internal Identity ObjectIdentity
		{
			get
			{
				return this._objectIdentity;
			}
			set
			{
				this._objectIdentity = value;
			}
		}

		/// <summary>Requests an unmanaged reference to the object represented by the current proxy instance.</summary>
		/// <returns>A pointer to a COM Callable Wrapper if the object reference is requested for communication with unmanaged objects in the current process through COM, or a pointer to a cached or newly generated IUnknown COM interface if the object reference is requested for marshaling to a remote location.</returns>
		/// <param name="fIsMarshalled">true if the object reference is requested for marshaling to a remote location; false if the object reference is requested for communication with unmanaged objects in the current process through COM. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		[MonoTODO]
		public virtual IntPtr GetCOMIUnknown(bool fIsMarshalled)
		{
			throw new NotImplementedException();
		}

		/// <summary>Stores an unmanaged proxy of the object that is represented by the current instance.</summary>
		/// <param name="i">A pointer to the IUnknown interface for the object that is represented by the current proxy instance. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		[MonoTODO]
		public virtual void SetCOMIUnknown(IntPtr i)
		{
			throw new NotImplementedException();
		}

		/// <summary>Requests a COM interface with the specified ID.</summary>
		/// <returns>A pointer to the requested interface.</returns>
		/// <param name="iid">A reference to the requested interface. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		[MonoTODO]
		public virtual IntPtr SupportsInterface(ref Guid iid)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves stub data that is stored for the specified proxy.</summary>
		/// <returns>Stub data for the specified proxy.</returns>
		/// <param name="rp">The proxy for which stub data is requested. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have UnmanagedCode permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static object GetStubData(RealProxy rp)
		{
			return rp._stubData;
		}

		/// <summary>Sets the stub data for the specified proxy.</summary>
		/// <param name="rp">The proxy for which to set stub data. </param>
		/// <param name="stubData">The new stub data. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have UnmanagedCode permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, Infrastructure" />
		/// </PermissionSet>
		public static void SetStubData(RealProxy rp, object stubData)
		{
			rp._stubData = stubData;
		}

		/// <summary>When overridden in a derived class, invokes the method that is specified in the provided <see cref="T:System.Runtime.Remoting.Messaging.IMessage" /> on the remote object that is represented by the current instance.</summary>
		/// <returns>The message returned by the invoked method, containing the return value and any out or ref parameters.</returns>
		/// <param name="msg">A <see cref="T:System.Runtime.Remoting.Messaging.IMessage" /> that contains a <see cref="T:System.Collections.IDictionary" /> of information about the method call. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public abstract IMessage Invoke(IMessage msg);

		internal static object PrivateInvoke(RealProxy rp, IMessage msg, out Exception exc, out object[] out_args)
		{
			MonoMethodMessage monoMethodMessage = (MonoMethodMessage)msg;
			monoMethodMessage.LogicalCallContext = CallContext.CreateLogicalCallContext(true);
			CallType callType = monoMethodMessage.CallType;
			bool flag = rp is RemotingProxy;
			out_args = null;
			IMethodReturnMessage methodReturnMessage = null;
			if (callType == CallType.BeginInvoke)
			{
				monoMethodMessage.AsyncResult.CallMessage = monoMethodMessage;
			}
			if (callType == CallType.EndInvoke)
			{
				methodReturnMessage = (IMethodReturnMessage)monoMethodMessage.AsyncResult.EndInvoke();
			}
			if (monoMethodMessage.MethodBase.IsConstructor)
			{
				if (flag)
				{
					methodReturnMessage = (IMethodReturnMessage)(rp as RemotingProxy).ActivateRemoteObject((IMethodMessage)msg);
				}
				else
				{
					msg = new ConstructionCall(rp.GetProxiedType());
				}
			}
			if (methodReturnMessage == null)
			{
				bool flag2 = false;
				try
				{
					methodReturnMessage = (IMethodReturnMessage)rp.Invoke(msg);
				}
				catch (Exception e)
				{
					flag2 = true;
					if (callType != CallType.BeginInvoke)
					{
						throw;
					}
					monoMethodMessage.AsyncResult.SyncProcessMessage(new ReturnMessage(e, msg as IMethodCallMessage));
					methodReturnMessage = new ReturnMessage(null, null, 0, null, msg as IMethodCallMessage);
				}
				if (!flag && callType == CallType.BeginInvoke && !flag2)
				{
					IMessage ret = monoMethodMessage.AsyncResult.SyncProcessMessage(methodReturnMessage);
					out_args = methodReturnMessage.OutArgs;
					methodReturnMessage = new ReturnMessage(ret, null, 0, null, methodReturnMessage as IMethodCallMessage);
				}
			}
			if (methodReturnMessage.LogicalCallContext != null && methodReturnMessage.LogicalCallContext.HasInfo)
			{
				CallContext.UpdateCurrentCallContext(methodReturnMessage.LogicalCallContext);
			}
			exc = methodReturnMessage.Exception;
			if (exc != null)
			{
				out_args = null;
				throw exc.FixRemotingException();
			}
			if (methodReturnMessage is IConstructionReturnMessage)
			{
				if (out_args == null)
				{
					out_args = methodReturnMessage.OutArgs;
				}
			}
			else if (monoMethodMessage.CallType != CallType.BeginInvoke)
			{
				if (monoMethodMessage.CallType == CallType.Sync)
				{
					out_args = RealProxy.ProcessResponse(methodReturnMessage, monoMethodMessage);
				}
				else if (monoMethodMessage.CallType == CallType.EndInvoke)
				{
					out_args = RealProxy.ProcessResponse(methodReturnMessage, monoMethodMessage.AsyncResult.CallMessage);
				}
				else if (out_args == null)
				{
					out_args = methodReturnMessage.OutArgs;
				}
			}
			return methodReturnMessage.ReturnValue;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal virtual extern object InternalGetTransparentProxy(string className);

		/// <summary>Returns the transparent proxy for the current instance of <see cref="T:System.Runtime.Remoting.Proxies.RealProxy" />.</summary>
		/// <returns>The transparent proxy for the current proxy instance.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public virtual object GetTransparentProxy()
		{
			if (this._objTP == null)
			{
				IRemotingTypeInfo remotingTypeInfo = this as IRemotingTypeInfo;
				string text;
				if (remotingTypeInfo != null)
				{
					text = remotingTypeInfo.TypeName;
					if (text == null || text == typeof(MarshalByRefObject).AssemblyQualifiedName)
					{
						text = this.class_to_proxy.AssemblyQualifiedName;
					}
				}
				else
				{
					text = this.class_to_proxy.AssemblyQualifiedName;
				}
				this._objTP = this.InternalGetTransparentProxy(text);
			}
			return this._objTP;
		}

		/// <summary>Initializes a new instance of the object <see cref="T:System.Type" /> of the remote object that the current instance of <see cref="T:System.Runtime.Remoting.Proxies.RealProxy" /> represents with the specified <see cref="T:System.Runtime.Remoting.Activation.IConstructionCallMessage" />.</summary>
		/// <returns>The result of the construction request.</returns>
		/// <param name="ctorMsg">A construction call message that contains the constructor parameters for the new instance of the remote object that is represented by the current <see cref="T:System.Runtime.Remoting.Proxies.RealProxy" />. Can be null. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have UnmanagedCode permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, Infrastructure" />
		/// </PermissionSet>
		[ComVisible(true)]
		[MonoTODO]
		public IConstructionReturnMessage InitializeServerObject(IConstructionCallMessage ctorMsg)
		{
			throw new NotImplementedException();
		}

		/// <summary>Attaches the current proxy instance to the specified remote <see cref="T:System.MarshalByRefObject" />.</summary>
		/// <param name="s">The <see cref="T:System.MarshalByRefObject" /> that the current proxy instance represents. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have UnmanagedCode permission. </exception>
		protected void AttachServer(MarshalByRefObject s)
		{
			this._server = s;
		}

		/// <summary>Detaches the current proxy instance from the remote server object that it represents.</summary>
		/// <returns>The detached server object.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have UnmanagedCode permission. </exception>
		protected MarshalByRefObject DetachServer()
		{
			MarshalByRefObject server = this._server;
			this._server = null;
			return server;
		}

		/// <summary>Returns the server object that is represented by the current proxy instance.</summary>
		/// <returns>The server object that is represented by the current proxy instance.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have UnmanagedCode permission. </exception>
		protected MarshalByRefObject GetUnwrappedServer()
		{
			return this._server;
		}

		internal void SetTargetDomain(int domainId)
		{
			this._targetDomainId = domainId;
		}

		internal object GetAppDomainTarget()
		{
			if (this._server == null)
			{
				ClientActivatedIdentity clientActivatedIdentity = RemotingServices.GetIdentityForUri(this._targetUri) as ClientActivatedIdentity;
				if (clientActivatedIdentity == null)
				{
					throw new RemotingException("Server for uri '" + this._targetUri + "' not found");
				}
				this._server = clientActivatedIdentity.GetServerObject();
			}
			return this._server;
		}

		private static object[] ProcessResponse(IMethodReturnMessage mrm, MonoMethodMessage call)
		{
			MethodInfo methodInfo = (MethodInfo)call.MethodBase;
			if (mrm.ReturnValue != null && !methodInfo.ReturnType.IsInstanceOfType(mrm.ReturnValue))
			{
				throw new InvalidCastException("Return value has an invalid type");
			}
			int num;
			if (call.NeedsOutProcessing(out num))
			{
				ParameterInfo[] parameters = methodInfo.GetParameters();
				object[] array = new object[num];
				int num2 = 0;
				foreach (ParameterInfo parameterInfo in parameters)
				{
					if (parameterInfo.IsOut && !parameterInfo.ParameterType.IsByRef)
					{
						object obj = (parameterInfo.Position >= mrm.ArgCount) ? null : mrm.GetArg(parameterInfo.Position);
						if (obj != null)
						{
							object arg = call.GetArg(parameterInfo.Position);
							if (arg == null)
							{
								throw new RemotingException("Unexpected null value in local out parameter '" + parameterInfo.Name + "'");
							}
							RemotingServices.UpdateOutArgObject(parameterInfo, arg, obj);
						}
					}
					else if (parameterInfo.ParameterType.IsByRef)
					{
						object obj2 = (parameterInfo.Position >= mrm.ArgCount) ? null : mrm.GetArg(parameterInfo.Position);
						if (obj2 != null && !parameterInfo.ParameterType.GetElementType().IsInstanceOfType(obj2))
						{
							throw new InvalidCastException("Return argument '" + parameterInfo.Name + "' has an invalid type");
						}
						array[num2++] = obj2;
					}
				}
				return array;
			}
			return new object[0];
		}
	}
}
