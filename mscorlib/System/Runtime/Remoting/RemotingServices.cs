using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Services;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace System.Runtime.Remoting
{
	/// <summary>Provides several methods for using and publishing remoted objects and proxies. This class cannot be inherited.</summary>
	[ComVisible(true)]
	public sealed class RemotingServices
	{
		private static Hashtable uri_hash = new Hashtable();

		private static BinaryFormatter _serializationFormatter;

		private static BinaryFormatter _deserializationFormatter;

		internal static string app_id;

		private static int next_id = 1;

		private static readonly BindingFlags methodBindings = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		private static readonly MethodInfo FieldSetterMethod;

		private static readonly MethodInfo FieldGetterMethod;

		private RemotingServices()
		{
		}

		static RemotingServices()
		{
			RemotingSurrogateSelector selector = new RemotingSurrogateSelector();
			StreamingContext context = new StreamingContext(StreamingContextStates.Remoting, null);
			RemotingServices._serializationFormatter = new BinaryFormatter(selector, context);
			RemotingServices._deserializationFormatter = new BinaryFormatter(null, context);
			RemotingServices._serializationFormatter.AssemblyFormat = FormatterAssemblyStyle.Full;
			RemotingServices._deserializationFormatter.AssemblyFormat = FormatterAssemblyStyle.Full;
			RemotingServices.RegisterInternalChannels();
			RemotingServices.app_id = Guid.NewGuid().ToString().Replace('-', '_') + "/";
			RemotingServices.CreateWellKnownServerIdentity(typeof(RemoteActivator), "RemoteActivationService.rem", WellKnownObjectMode.Singleton);
			RemotingServices.FieldSetterMethod = typeof(object).GetMethod("FieldSetter", BindingFlags.Instance | BindingFlags.NonPublic);
			RemotingServices.FieldGetterMethod = typeof(object).GetMethod("FieldGetter", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern object InternalExecute(MethodBase method, object obj, object[] parameters, out object[] out_args);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern MethodBase GetVirtualMethod(Type type, MethodBase method);

		/// <summary>Returns a Boolean value that indicates whether the given object is a transparent proxy or a real object.</summary>
		/// <returns>A Boolean value that indicates whether the object specified in the <paramref name="proxy" /> parameter is a transparent proxy or a real object.</returns>
		/// <param name="proxy">The reference to the object to check. </param>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsTransparentProxy(object proxy);

		internal static IMethodReturnMessage InternalExecuteMessage(MarshalByRefObject target, IMethodCallMessage reqMsg)
		{
			Type type = target.GetType();
			MethodBase methodBase;
			if (reqMsg.MethodBase.DeclaringType == type || reqMsg.MethodBase == RemotingServices.FieldSetterMethod || reqMsg.MethodBase == RemotingServices.FieldGetterMethod)
			{
				methodBase = reqMsg.MethodBase;
			}
			else
			{
				methodBase = RemotingServices.GetVirtualMethod(type, reqMsg.MethodBase);
				if (methodBase == null)
				{
					throw new RemotingException(string.Format("Cannot resolve method {0}:{1}", type, reqMsg.MethodName));
				}
			}
			if (reqMsg.MethodBase.IsGenericMethod)
			{
				Type[] genericArguments = reqMsg.MethodBase.GetGenericArguments();
				methodBase = ((MethodInfo)methodBase).MakeGenericMethod(genericArguments);
			}
			object oldContext = CallContext.SetCurrentCallContext(reqMsg.LogicalCallContext);
			ReturnMessage result;
			try
			{
				object[] array;
				object ret = RemotingServices.InternalExecute(methodBase, target, reqMsg.Args, out array);
				ParameterInfo[] parameters = methodBase.GetParameters();
				object[] array2 = new object[parameters.Length];
				int outArgsCount = 0;
				int num = 0;
				foreach (ParameterInfo parameterInfo in parameters)
				{
					if (parameterInfo.IsOut && !parameterInfo.ParameterType.IsByRef)
					{
						array2[outArgsCount++] = reqMsg.GetArg(parameterInfo.Position);
					}
					else if (parameterInfo.ParameterType.IsByRef)
					{
						array2[outArgsCount++] = array[num++];
					}
					else
					{
						array2[outArgsCount++] = null;
					}
				}
				result = new ReturnMessage(ret, array2, outArgsCount, CallContext.CreateLogicalCallContext(true), reqMsg);
			}
			catch (Exception e)
			{
				result = new ReturnMessage(e, reqMsg);
			}
			CallContext.RestoreCallContext(oldContext);
			return result;
		}

		/// <summary>Connects to the specified remote object, and executes the provided <see cref="T:System.Runtime.Remoting.Messaging.IMethodCallMessage" /> on it.</summary>
		/// <returns>The response of the remote method.</returns>
		/// <param name="target">The remote object whose method you want to call. </param>
		/// <param name="reqMsg">A method call message to the specified remote object's method. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">The method was called from a context other than the native context of the object.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static IMethodReturnMessage ExecuteMessage(MarshalByRefObject target, IMethodCallMessage reqMsg)
		{
			if (RemotingServices.IsTransparentProxy(target))
			{
				RealProxy realProxy = RemotingServices.GetRealProxy(target);
				return (IMethodReturnMessage)realProxy.Invoke(reqMsg);
			}
			return RemotingServices.InternalExecuteMessage(target, reqMsg);
		}

		/// <summary>Creates a proxy for a well-known object, given the <see cref="T:System.Type" /> and URL.</summary>
		/// <returns>A proxy to the remote object that points to an endpoint served by the specified well-known object.</returns>
		/// <param name="classToProxy">The <see cref="T:System.Type" /> of a well-known object on the server end to which you want to connect. </param>
		/// <param name="url">The URL of the server class. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have permission to configure remoting types and channels. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence, RemotingConfiguration" />
		/// </PermissionSet>
		[ComVisible(true)]
		public static object Connect(Type classToProxy, string url)
		{
			ObjRef objRef = new ObjRef(classToProxy, url, null);
			return RemotingServices.GetRemoteObject(objRef, classToProxy);
		}

		/// <summary>Creates a proxy for a well-known object, given the <see cref="T:System.Type" />, URL, and channel-specific data.</summary>
		/// <returns>A proxy that points to an endpoint that is served by the requested well-known object.</returns>
		/// <param name="classToProxy">The <see cref="T:System.Type" /> of the well-known object to which you want to connect. </param>
		/// <param name="url">The URL of the well-known object. </param>
		/// <param name="data">Channel specific data. Can be null. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have permission to configure remoting types and channels. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence, RemotingConfiguration" />
		/// </PermissionSet>
		[ComVisible(true)]
		public static object Connect(Type classToProxy, string url, object data)
		{
			ObjRef objRef = new ObjRef(classToProxy, url, data);
			return RemotingServices.GetRemoteObject(objRef, classToProxy);
		}

		/// <summary>Stops an object from receiving any further messages through the registered remoting channels.</summary>
		/// <returns>true if the object was disconnected from the registered remoting channels successfully; otherwise, false.</returns>
		/// <param name="obj">Object to disconnect from its channel. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="obj" /> parameter is a proxy. </exception>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have permission to configure remoting types and channels. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static bool Disconnect(MarshalByRefObject obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			ServerIdentity serverIdentity;
			if (RemotingServices.IsTransparentProxy(obj))
			{
				RealProxy realProxy = RemotingServices.GetRealProxy(obj);
				if (!realProxy.GetProxiedType().IsContextful || !(realProxy.ObjectIdentity is ServerIdentity))
				{
					throw new ArgumentException("The obj parameter is a proxy.");
				}
				serverIdentity = (realProxy.ObjectIdentity as ServerIdentity);
			}
			else
			{
				serverIdentity = obj.ObjectIdentity;
				obj.ObjectIdentity = null;
			}
			if (serverIdentity == null || !serverIdentity.IsConnected)
			{
				return false;
			}
			LifetimeServices.StopTrackingLifetime(serverIdentity);
			RemotingServices.DisposeIdentity(serverIdentity);
			TrackingServices.NotifyDisconnectedObject(obj);
			return true;
		}

		/// <summary>Returns the <see cref="T:System.Type" /> of the object with the specified URI.</summary>
		/// <returns>The <see cref="T:System.Type" /> of the object with the specified URI.</returns>
		/// <param name="URI">The URI of the object whose <see cref="T:System.Type" /> is requested. </param>
		/// <exception cref="T:System.Security.SecurityException">Either the immediate caller does not have infrastructure permission, or at least one of the callers higher in the callstack does not have permission to retrieve the type information of non-public members. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static Type GetServerTypeForUri(string URI)
		{
			ServerIdentity serverIdentity = RemotingServices.GetIdentityForUri(URI) as ServerIdentity;
			if (serverIdentity == null)
			{
				return null;
			}
			return serverIdentity.ObjectType;
		}

		/// <summary>Retrieves the URI for the specified object.</summary>
		/// <returns>The URI of the specified object if it has one, or null if the object has not yet been marshaled.</returns>
		/// <param name="obj">The <see cref="T:System.MarshalByRefObject" /> for which a URI is requested. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static string GetObjectUri(MarshalByRefObject obj)
		{
			Identity objectIdentity = RemotingServices.GetObjectIdentity(obj);
			if (objectIdentity is ClientIdentity)
			{
				return ((ClientIdentity)objectIdentity).TargetUri;
			}
			if (objectIdentity != null)
			{
				return objectIdentity.ObjectUri;
			}
			return null;
		}

		/// <summary>Takes a <see cref="T:System.Runtime.Remoting.ObjRef" /> and creates a proxy object out of it.</summary>
		/// <returns>A proxy to the object that the given <see cref="T:System.Runtime.Remoting.ObjRef" /> represents.</returns>
		/// <param name="objectRef">The <see cref="T:System.Runtime.Remoting.ObjRef" /> that represents the remote object for which the proxy is being created. </param>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Runtime.Remoting.ObjRef" /> instance specified in the <paramref name="objectRef" /> parameter is not well-formed. </exception>
		/// <exception cref="T:System.Security.SecurityException">At least one of the callers higher in the callstack does not have permission to configure remoting types and channels. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure" />
		/// </PermissionSet>
		public static object Unmarshal(ObjRef objectRef)
		{
			return RemotingServices.Unmarshal(objectRef, true);
		}

		/// <summary>Takes a <see cref="T:System.Runtime.Remoting.ObjRef" /> and creates a proxy object out of it, refining it to the type on the server.</summary>
		/// <returns>A proxy to the object that the given <see cref="T:System.Runtime.Remoting.ObjRef" /> represents.</returns>
		/// <param name="objectRef">The <see cref="T:System.Runtime.Remoting.ObjRef" /> that represents the remote object for which the proxy is being created. </param>
		/// <param name="fRefine">true to refine the proxy to the type on the server; otherwise, false. </param>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Runtime.Remoting.ObjRef" /> instance specified in the <paramref name="objectRef" /> parameter is not well-formed. </exception>
		/// <exception cref="T:System.Security.SecurityException">At least one of the callers higher in the callstack does not have permission to configure remoting types and channels. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure" />
		/// </PermissionSet>
		public static object Unmarshal(ObjRef objectRef, bool fRefine)
		{
			Type type = (!fRefine) ? typeof(MarshalByRefObject) : objectRef.ServerType;
			if (type == null)
			{
				type = typeof(MarshalByRefObject);
			}
			if (objectRef.IsReferenceToWellKnow)
			{
				object remoteObject = RemotingServices.GetRemoteObject(objectRef, type);
				TrackingServices.NotifyUnmarshaledObject(remoteObject, objectRef);
				return remoteObject;
			}
			object obj;
			if (type.IsContextful)
			{
				ProxyAttribute proxyAttribute = (ProxyAttribute)Attribute.GetCustomAttribute(type, typeof(ProxyAttribute), true);
				if (proxyAttribute != null)
				{
					obj = proxyAttribute.CreateProxy(objectRef, type, null, null).GetTransparentProxy();
					TrackingServices.NotifyUnmarshaledObject(obj, objectRef);
					return obj;
				}
			}
			obj = RemotingServices.GetProxyForRemoteObject(objectRef, type);
			TrackingServices.NotifyUnmarshaledObject(obj, objectRef);
			return obj;
		}

		/// <summary>Takes a <see cref="T:System.MarshalByRefObject" />, registers it with the remoting infrastructure, and converts it into an instance of the <see cref="T:System.Runtime.Remoting.ObjRef" /> class.</summary>
		/// <returns>An instance of the <see cref="T:System.Runtime.Remoting.ObjRef" /> class that represents the object specified in the <paramref name="Obj" /> parameter.</returns>
		/// <param name="Obj">The object to convert. </param>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">The <paramref name="Obj" /> parameter is an object proxy. </exception>
		/// <exception cref="T:System.Security.SecurityException">At least one of the callers higher in the callstack does not have permission to configure remoting types and channels. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration" />
		/// </PermissionSet>
		public static ObjRef Marshal(MarshalByRefObject Obj)
		{
			return RemotingServices.Marshal(Obj, null, null);
		}

		/// <summary>Converts the given <see cref="T:System.MarshalByRefObject" /> into an instance of the <see cref="T:System.Runtime.Remoting.ObjRef" /> class with the specified URI.</summary>
		/// <returns>An instance of the <see cref="T:System.Runtime.Remoting.ObjRef" /> class that represents the object specified in the <paramref name="Obj" /> parameter.</returns>
		/// <param name="Obj">The object to convert. </param>
		/// <param name="URI">The specified URI with which to initialize the new <see cref="T:System.Runtime.Remoting.ObjRef" />. Can be null. </param>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">
		///   <paramref name="Obj" /> is an object proxy, and the <paramref name="URI" /> parameter is not null. </exception>
		/// <exception cref="T:System.Security.SecurityException">At least one of the callers higher in the callstack does not have permission to configure remoting types and channels. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration" />
		/// </PermissionSet>
		public static ObjRef Marshal(MarshalByRefObject Obj, string URI)
		{
			return RemotingServices.Marshal(Obj, URI, null);
		}

		/// <summary>Takes a <see cref="T:System.MarshalByRefObject" /> and converts it into an instance of the <see cref="T:System.Runtime.Remoting.ObjRef" /> class with the specified URI, and the provided <see cref="T:System.Type" />.</summary>
		/// <returns>An instance of the <see cref="T:System.Runtime.Remoting.ObjRef" /> class that represents the object specified in the <paramref name="Obj" /> parameter.</returns>
		/// <param name="Obj">The object to convert into a <see cref="T:System.Runtime.Remoting.ObjRef" />. </param>
		/// <param name="ObjURI">The URI the object specified in the <paramref name="Obj" /> parameter is marshaled with. Can be null. </param>
		/// <param name="RequestedType">The <see cref="T:System.Type" /><paramref name="Obj" /> is marshaled as. Can be null. </param>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">
		///   <paramref name="Obj" /> is a proxy of a remote object, and the <paramref name="ObjUri" /> parameter is not null. </exception>
		/// <exception cref="T:System.Security.SecurityException">At least one of the callers higher in the callstack does not have permission to configure remoting types and channels. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration" />
		/// </PermissionSet>
		public static ObjRef Marshal(MarshalByRefObject Obj, string ObjURI, Type RequestedType)
		{
			if (RemotingServices.IsTransparentProxy(Obj))
			{
				RealProxy realProxy = RemotingServices.GetRealProxy(Obj);
				Identity objectIdentity = realProxy.ObjectIdentity;
				if (objectIdentity != null)
				{
					if (realProxy.GetProxiedType().IsContextful && !objectIdentity.IsConnected)
					{
						ClientActivatedIdentity clientActivatedIdentity = (ClientActivatedIdentity)objectIdentity;
						if (ObjURI == null)
						{
							ObjURI = RemotingServices.NewUri();
						}
						clientActivatedIdentity.ObjectUri = ObjURI;
						RemotingServices.RegisterServerIdentity(clientActivatedIdentity);
						clientActivatedIdentity.StartTrackingLifetime((ILease)Obj.InitializeLifetimeService());
						return clientActivatedIdentity.CreateObjRef(RequestedType);
					}
					if (ObjURI != null)
					{
						throw new RemotingException("It is not possible marshal a proxy of a remote object.");
					}
					ObjRef objRef = realProxy.ObjectIdentity.CreateObjRef(RequestedType);
					TrackingServices.NotifyMarshaledObject(Obj, objRef);
					return objRef;
				}
			}
			if (RequestedType == null)
			{
				RequestedType = Obj.GetType();
			}
			if (ObjURI == null)
			{
				if (Obj.ObjectIdentity == null)
				{
					ObjURI = RemotingServices.NewUri();
					RemotingServices.CreateClientActivatedServerIdentity(Obj, RequestedType, ObjURI);
				}
			}
			else
			{
				ClientActivatedIdentity clientActivatedIdentity2 = RemotingServices.GetIdentityForUri("/" + ObjURI) as ClientActivatedIdentity;
				if (clientActivatedIdentity2 == null || Obj != clientActivatedIdentity2.GetServerObject())
				{
					RemotingServices.CreateClientActivatedServerIdentity(Obj, RequestedType, ObjURI);
				}
			}
			ObjRef objRef2;
			if (RemotingServices.IsTransparentProxy(Obj))
			{
				objRef2 = RemotingServices.GetRealProxy(Obj).ObjectIdentity.CreateObjRef(RequestedType);
			}
			else
			{
				objRef2 = Obj.CreateObjRef(RequestedType);
			}
			TrackingServices.NotifyMarshaledObject(Obj, objRef2);
			return objRef2;
		}

		private static string NewUri()
		{
			int num = Interlocked.Increment(ref RemotingServices.next_id);
			return string.Concat(new object[]
			{
				RemotingServices.app_id,
				Environment.TickCount.ToString("x"),
				"_",
				num,
				".rem"
			});
		}

		/// <summary>Returns the real proxy backing the specified transparent proxy.</summary>
		/// <returns>The real proxy instance backing the transparent proxy.</returns>
		/// <param name="proxy">A transparent proxy. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static RealProxy GetRealProxy(object proxy)
		{
			if (!RemotingServices.IsTransparentProxy(proxy))
			{
				throw new RemotingException("Cannot get the real proxy from an object that is not a transparent proxy.");
			}
			return ((TransparentProxy)proxy)._rp;
		}

		/// <summary>Returns the method base from the given <see cref="T:System.Runtime.Remoting.Messaging.IMethodMessage" />.</summary>
		/// <returns>The method base extracted from the <paramref name="msg" /> parameter.</returns>
		/// <param name="msg">The method message to extract the method base from. </param>
		/// <exception cref="T:System.Security.SecurityException">Either the immediate caller does not have infrastructure permission, or at least one of the callers higher in the callstack does not have permission to retrieve the type information of non-public members. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static MethodBase GetMethodBaseFromMethodMessage(IMethodMessage msg)
		{
			Type type = Type.GetType(msg.TypeName);
			if (type == null)
			{
				throw new RemotingException("Type '" + msg.TypeName + "' not found.");
			}
			return RemotingServices.GetMethodBaseFromName(type, msg.MethodName, (Type[])msg.MethodSignature);
		}

		internal static MethodBase GetMethodBaseFromName(Type type, string methodName, Type[] signature)
		{
			if (type.IsInterface)
			{
				return RemotingServices.FindInterfaceMethod(type, methodName, signature);
			}
			MethodBase method;
			if (signature == null)
			{
				method = type.GetMethod(methodName, RemotingServices.methodBindings);
			}
			else
			{
				method = type.GetMethod(methodName, RemotingServices.methodBindings, null, signature, null);
			}
			if (method != null)
			{
				return method;
			}
			if (methodName == "FieldSetter")
			{
				return RemotingServices.FieldSetterMethod;
			}
			if (methodName == "FieldGetter")
			{
				return RemotingServices.FieldGetterMethod;
			}
			if (signature == null)
			{
				return type.GetConstructor(RemotingServices.methodBindings, null, Type.EmptyTypes, null);
			}
			return type.GetConstructor(RemotingServices.methodBindings, null, signature, null);
		}

		private static MethodBase FindInterfaceMethod(Type type, string methodName, Type[] signature)
		{
			MethodBase methodBase;
			if (signature == null)
			{
				methodBase = type.GetMethod(methodName, RemotingServices.methodBindings);
			}
			else
			{
				methodBase = type.GetMethod(methodName, RemotingServices.methodBindings, null, signature, null);
			}
			if (methodBase != null)
			{
				return methodBase;
			}
			foreach (Type type2 in type.GetInterfaces())
			{
				methodBase = RemotingServices.FindInterfaceMethod(type2, methodName, signature);
				if (methodBase != null)
				{
					return methodBase;
				}
			}
			return null;
		}

		/// <summary>Serializes the specified marshal by reference object into the provided <see cref="T:System.Runtime.Serialization.SerializationInfo" />.</summary>
		/// <param name="obj">The object to serialize. </param>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> into which the object is serialized. </param>
		/// <param name="context">The source and destination of the serialization. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> or <paramref name="info" /> parameter is null. </exception>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			ObjRef objRef = RemotingServices.Marshal((MarshalByRefObject)obj);
			objRef.GetObjectData(info, context);
		}

		/// <summary>Returns the <see cref="T:System.Runtime.Remoting.ObjRef" /> that represents the remote object from the specified proxy.</summary>
		/// <returns>A <see cref="T:System.Runtime.Remoting.ObjRef" /> that represents the remote object the specified proxy is connected to, or null if the object or proxy have not been marshaled.</returns>
		/// <param name="obj">A proxy connected to the object you want to create a <see cref="T:System.Runtime.Remoting.ObjRef" /> for. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static ObjRef GetObjRefForProxy(MarshalByRefObject obj)
		{
			Identity objectIdentity = RemotingServices.GetObjectIdentity(obj);
			if (objectIdentity == null)
			{
				return null;
			}
			return objectIdentity.CreateObjRef(null);
		}

		/// <summary>Returns a lifetime service object that controls the lifetime policy of the specified object.</summary>
		/// <returns>The object that controls the lifetime of <paramref name="obj" />.</returns>
		/// <param name="obj">The object to obtain lifetime service for. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration" />
		/// </PermissionSet>
		public static object GetLifetimeService(MarshalByRefObject obj)
		{
			if (obj == null)
			{
				return null;
			}
			return obj.GetLifetimeService();
		}

		/// <summary>Returns a chain of envoy sinks that should be used when sending messages to the remote object represented by the specified proxy.</summary>
		/// <returns>A chain of envoy sinks associated with the specified proxy.</returns>
		/// <param name="obj">The proxy of the remote object that requested envoy sinks are associated with. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static IMessageSink GetEnvoyChainForProxy(MarshalByRefObject obj)
		{
			if (RemotingServices.IsTransparentProxy(obj))
			{
				return ((ClientIdentity)RemotingServices.GetRealProxy(obj).ObjectIdentity).EnvoySink;
			}
			throw new ArgumentException("obj must be a proxy.", "obj");
		}

		/// <summary>Logs the stage in a remoting exchange to an external debugger.</summary>
		/// <param name="stage">An internally defined constant that identifies the stage in a remoting exchange.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		[Obsolete("It existed for only internal use in .NET and unimplemented in mono")]
		[Conditional("REMOTING_PERF")]
		[MonoTODO]
		public static void LogRemotingStage(int stage)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves a session ID for a message.</summary>
		/// <returns>A session ID string that uniquely identifies the current session.</returns>
		/// <param name="msg">The <see cref="T:System.Runtime.Remoting.Messaging.IMethodMessage" /> for which a session ID is requested. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static string GetSessionIdForMethodMessage(IMethodMessage msg)
		{
			return msg.Uri;
		}

		/// <summary>Returns a Boolean value that indicates whether the method in the given message is overloaded.</summary>
		/// <returns>true if the method called in <paramref name="msg" /> is overloaded; otherwise, false.</returns>
		/// <param name="msg">The message that contains a call to the method in question. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static bool IsMethodOverloaded(IMethodMessage msg)
		{
			MonoType monoType = (MonoType)msg.MethodBase.DeclaringType;
			return monoType.GetMethodsByName(msg.MethodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, false, monoType).Length > 1;
		}

		/// <summary>Returns a Boolean value that indicates whether the object specified by the given transparent proxy is contained in a different application domain than the object that called the current method.</summary>
		/// <returns>true if the object is out of the current application domain; otherwise, false.</returns>
		/// <param name="tp">The object to check. </param>
		public static bool IsObjectOutOfAppDomain(object tp)
		{
			MarshalByRefObject marshalByRefObject = tp as MarshalByRefObject;
			if (marshalByRefObject == null)
			{
				return false;
			}
			Identity objectIdentity = RemotingServices.GetObjectIdentity(marshalByRefObject);
			return objectIdentity is ClientIdentity;
		}

		/// <summary>Returns a Boolean value that indicates whether the object represented by the given proxy is contained in a different context than the object that called the current method.</summary>
		/// <returns>true if the object is out of the current context; otherwise, false.</returns>
		/// <param name="tp">The object to check. </param>
		public static bool IsObjectOutOfContext(object tp)
		{
			MarshalByRefObject marshalByRefObject = tp as MarshalByRefObject;
			if (marshalByRefObject == null)
			{
				return false;
			}
			Identity objectIdentity = RemotingServices.GetObjectIdentity(marshalByRefObject);
			if (objectIdentity == null)
			{
				return false;
			}
			ServerIdentity serverIdentity = objectIdentity as ServerIdentity;
			return serverIdentity == null || serverIdentity.Context != Thread.CurrentContext;
		}

		/// <summary>Returns a Boolean value that indicates whether the client that called the method specified in the given message is waiting for the server to finish processing the method before continuing execution.</summary>
		/// <returns>true if the method is one way; otherwise, false.</returns>
		/// <param name="method">The method in question. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static bool IsOneWay(MethodBase method)
		{
			return method.IsDefined(typeof(OneWayAttribute), false);
		}

		internal static bool IsAsyncMessage(IMessage msg)
		{
			return msg is MonoMethodMessage && (((MonoMethodMessage)msg).IsAsync || RemotingServices.IsOneWay(((MonoMethodMessage)msg).MethodBase));
		}

		/// <summary>Sets the URI for the subsequent call to the <see cref="M:System.Runtime.Remoting.RemotingServices.Marshal(System.MarshalByRefObject)" /> method.</summary>
		/// <param name="obj">The object to set a URI for. </param>
		/// <param name="uri">The URI to assign to the specified object. </param>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">
		///   <paramref name="obj" /> is not a local object, has already been marshaled, or the current method has already been called on. </exception>
		/// <exception cref="T:System.Security.SecurityException">At least one of the callers higher in the callstack does not have permission to configure remoting types and channels. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration" />
		/// </PermissionSet>
		public static void SetObjectUriForMarshal(MarshalByRefObject obj, string uri)
		{
			if (RemotingServices.IsTransparentProxy(obj))
			{
				RealProxy realProxy = RemotingServices.GetRealProxy(obj);
				Identity objectIdentity = realProxy.ObjectIdentity;
				if (objectIdentity != null && !(objectIdentity is ServerIdentity) && !realProxy.GetProxiedType().IsContextful)
				{
					throw new RemotingException("SetObjectUriForMarshal method should only be called for MarshalByRefObjects that exist in the current AppDomain.");
				}
			}
			RemotingServices.Marshal(obj, uri);
		}

		internal static object CreateClientProxy(ActivatedClientTypeEntry entry, object[] activationAttributes)
		{
			if (entry.ContextAttributes != null || activationAttributes != null)
			{
				ArrayList arrayList = new ArrayList();
				if (entry.ContextAttributes != null)
				{
					arrayList.AddRange(entry.ContextAttributes);
				}
				if (activationAttributes != null)
				{
					arrayList.AddRange(activationAttributes);
				}
				return RemotingServices.CreateClientProxy(entry.ObjectType, entry.ApplicationUrl, arrayList.ToArray());
			}
			return RemotingServices.CreateClientProxy(entry.ObjectType, entry.ApplicationUrl, null);
		}

		internal static object CreateClientProxy(Type objectType, string url, object[] activationAttributes)
		{
			string text = url;
			if (!text.EndsWith("/"))
			{
				text += "/";
			}
			text += "RemoteActivationService.rem";
			string text2;
			RemotingServices.GetClientChannelSinkChain(text, null, out text2);
			RemotingProxy remotingProxy = new RemotingProxy(objectType, text, activationAttributes);
			return remotingProxy.GetTransparentProxy();
		}

		internal static object CreateClientProxy(WellKnownClientTypeEntry entry)
		{
			return RemotingServices.Connect(entry.ObjectType, entry.ObjectUrl, null);
		}

		internal static object CreateClientProxyForContextBound(Type type, object[] activationAttributes)
		{
			if (type.IsContextful)
			{
				ProxyAttribute proxyAttribute = (ProxyAttribute)Attribute.GetCustomAttribute(type, typeof(ProxyAttribute), true);
				if (proxyAttribute != null)
				{
					return proxyAttribute.CreateInstance(type);
				}
			}
			RemotingProxy remotingProxy = new RemotingProxy(type, ChannelServices.CrossContextUrl, activationAttributes);
			return remotingProxy.GetTransparentProxy();
		}

		internal static Identity GetIdentityForUri(string uri)
		{
			string text = RemotingServices.GetNormalizedUri(uri);
			Hashtable obj = RemotingServices.uri_hash;
			Identity result;
			lock (obj)
			{
				Identity identity = (Identity)RemotingServices.uri_hash[text];
				if (identity == null)
				{
					text = RemotingServices.RemoveAppNameFromUri(uri);
					if (text != null)
					{
						identity = (Identity)RemotingServices.uri_hash[text];
					}
				}
				result = identity;
			}
			return result;
		}

		private static string RemoveAppNameFromUri(string uri)
		{
			string text = RemotingConfiguration.ApplicationName;
			if (text == null)
			{
				return null;
			}
			text = "/" + text + "/";
			if (uri.StartsWith(text))
			{
				return uri.Substring(text.Length);
			}
			return null;
		}

		internal static Identity GetObjectIdentity(MarshalByRefObject obj)
		{
			if (RemotingServices.IsTransparentProxy(obj))
			{
				return RemotingServices.GetRealProxy(obj).ObjectIdentity;
			}
			return obj.ObjectIdentity;
		}

		internal static ClientIdentity GetOrCreateClientIdentity(ObjRef objRef, Type proxyType, out object clientProxy)
		{
			object channelData = (objRef.ChannelInfo == null) ? null : objRef.ChannelInfo.ChannelData;
			string uri;
			IMessageSink clientChannelSinkChain = RemotingServices.GetClientChannelSinkChain(objRef.URI, channelData, out uri);
			if (uri == null)
			{
				uri = objRef.URI;
			}
			Hashtable obj = RemotingServices.uri_hash;
			ClientIdentity result;
			lock (obj)
			{
				clientProxy = null;
				string normalizedUri = RemotingServices.GetNormalizedUri(objRef.URI);
				ClientIdentity clientIdentity = RemotingServices.uri_hash[normalizedUri] as ClientIdentity;
				if (clientIdentity != null)
				{
					clientProxy = clientIdentity.ClientProxy;
					if (clientProxy != null)
					{
						return clientIdentity;
					}
					RemotingServices.DisposeIdentity(clientIdentity);
				}
				clientIdentity = new ClientIdentity(uri, objRef);
				clientIdentity.ChannelSink = clientChannelSinkChain;
				RemotingServices.uri_hash[normalizedUri] = clientIdentity;
				if (proxyType != null)
				{
					RemotingProxy remotingProxy = new RemotingProxy(proxyType, clientIdentity);
					CrossAppDomainSink crossAppDomainSink = clientChannelSinkChain as CrossAppDomainSink;
					if (crossAppDomainSink != null)
					{
						remotingProxy.SetTargetDomain(crossAppDomainSink.TargetDomainId);
					}
					clientProxy = remotingProxy.GetTransparentProxy();
					clientIdentity.ClientProxy = (MarshalByRefObject)clientProxy;
				}
				result = clientIdentity;
			}
			return result;
		}

		private static IMessageSink GetClientChannelSinkChain(string url, object channelData, out string objectUri)
		{
			IMessageSink messageSink = ChannelServices.CreateClientChannelSinkChain(url, channelData, out objectUri);
			if (messageSink != null)
			{
				return messageSink;
			}
			if (url != null)
			{
				string message = string.Format("Cannot create channel sink to connect to URL {0}. An appropriate channel has probably not been registered.", url);
				throw new RemotingException(message);
			}
			string message2 = string.Format("Cannot create channel sink to connect to the remote object. An appropriate channel has probably not been registered.", url);
			throw new RemotingException(message2);
		}

		internal static ClientActivatedIdentity CreateContextBoundObjectIdentity(Type objectType)
		{
			return new ClientActivatedIdentity(null, objectType)
			{
				ChannelSink = ChannelServices.CrossContextChannel
			};
		}

		internal static ClientActivatedIdentity CreateClientActivatedServerIdentity(MarshalByRefObject realObject, Type objectType, string objectUri)
		{
			ClientActivatedIdentity clientActivatedIdentity = new ClientActivatedIdentity(objectUri, objectType);
			clientActivatedIdentity.AttachServerObject(realObject, Context.DefaultContext);
			RemotingServices.RegisterServerIdentity(clientActivatedIdentity);
			clientActivatedIdentity.StartTrackingLifetime((ILease)realObject.InitializeLifetimeService());
			return clientActivatedIdentity;
		}

		internal static ServerIdentity CreateWellKnownServerIdentity(Type objectType, string objectUri, WellKnownObjectMode mode)
		{
			ServerIdentity serverIdentity;
			if (mode == WellKnownObjectMode.SingleCall)
			{
				serverIdentity = new SingleCallIdentity(objectUri, Context.DefaultContext, objectType);
			}
			else
			{
				serverIdentity = new SingletonIdentity(objectUri, Context.DefaultContext, objectType);
			}
			RemotingServices.RegisterServerIdentity(serverIdentity);
			return serverIdentity;
		}

		private static void RegisterServerIdentity(ServerIdentity identity)
		{
			Hashtable obj = RemotingServices.uri_hash;
			lock (obj)
			{
				if (RemotingServices.uri_hash.ContainsKey(identity.ObjectUri))
				{
					throw new RemotingException("Uri already in use: " + identity.ObjectUri + ".");
				}
				RemotingServices.uri_hash[identity.ObjectUri] = identity;
			}
		}

		internal static object GetProxyForRemoteObject(ObjRef objref, Type classToProxy)
		{
			ClientActivatedIdentity clientActivatedIdentity = RemotingServices.GetIdentityForUri(objref.URI) as ClientActivatedIdentity;
			if (clientActivatedIdentity != null)
			{
				return clientActivatedIdentity.GetServerObject();
			}
			return RemotingServices.GetRemoteObject(objref, classToProxy);
		}

		internal static object GetRemoteObject(ObjRef objRef, Type proxyType)
		{
			object result;
			RemotingServices.GetOrCreateClientIdentity(objRef, proxyType, out result);
			return result;
		}

		internal static object GetServerObject(string uri)
		{
			ClientActivatedIdentity clientActivatedIdentity = RemotingServices.GetIdentityForUri(uri) as ClientActivatedIdentity;
			if (clientActivatedIdentity == null)
			{
				throw new RemotingException("Server for uri '" + uri + "' not found");
			}
			return clientActivatedIdentity.GetServerObject();
		}

		internal static byte[] SerializeCallData(object obj)
		{
			LogicalCallContext logicalCallContext = CallContext.CreateLogicalCallContext(false);
			if (logicalCallContext != null)
			{
				obj = new RemotingServices.CACD
				{
					d = obj,
					c = logicalCallContext
				};
			}
			if (obj == null)
			{
				return null;
			}
			MemoryStream memoryStream = new MemoryStream();
			RemotingServices._serializationFormatter.Serialize(memoryStream, obj);
			return memoryStream.ToArray();
		}

		internal static object DeserializeCallData(byte[] array)
		{
			if (array == null)
			{
				return null;
			}
			MemoryStream serializationStream = new MemoryStream(array);
			object obj = RemotingServices._deserializationFormatter.Deserialize(serializationStream);
			if (obj is RemotingServices.CACD)
			{
				RemotingServices.CACD cacd = (RemotingServices.CACD)obj;
				obj = cacd.d;
				CallContext.UpdateCurrentCallContext((LogicalCallContext)cacd.c);
			}
			return obj;
		}

		internal static byte[] SerializeExceptionData(Exception ex)
		{
			byte[] result;
			try
			{
				int num = 4;
				do
				{
					try
					{
						MemoryStream memoryStream = new MemoryStream();
						RemotingServices._serializationFormatter.Serialize(memoryStream, ex);
						return memoryStream.ToArray();
					}
					catch (Exception ex2)
					{
						if (ex2 is ThreadAbortException)
						{
							Thread.ResetAbort();
							num = 5;
							ex = ex2;
						}
						else if (num == 2)
						{
							ex = new Exception();
							ex.SetMessage(ex2.Message);
							ex.SetStackTrace(ex2.StackTrace);
						}
						else
						{
							ex = ex2;
						}
					}
					num--;
				}
				while (num > 0);
				result = null;
			}
			catch (Exception ex3)
			{
				byte[] array = RemotingServices.SerializeExceptionData(ex3);
				Thread.ResetAbort();
				result = array;
			}
			return result;
		}

		internal static object GetDomainProxy(AppDomain domain)
		{
			byte[] array = null;
			Context currentContext = Thread.CurrentContext;
			try
			{
				array = (byte[])AppDomain.InvokeInDomain(domain, typeof(AppDomain).GetMethod("GetMarshalledDomainObjRef", BindingFlags.Instance | BindingFlags.NonPublic), domain, null);
			}
			finally
			{
				AppDomain.InternalSetContext(currentContext);
			}
			byte[] array2 = new byte[array.Length];
			array.CopyTo(array2, 0);
			MemoryStream mem = new MemoryStream(array2);
			ObjRef objectRef = (ObjRef)CADSerializer.DeserializeObject(mem);
			return (AppDomain)RemotingServices.Unmarshal(objectRef);
		}

		private static void RegisterInternalChannels()
		{
			CrossAppDomainChannel.RegisterCrossAppDomainChannel();
		}

		internal static void DisposeIdentity(Identity ident)
		{
			Hashtable obj = RemotingServices.uri_hash;
			lock (obj)
			{
				if (!ident.Disposed)
				{
					ClientIdentity clientIdentity = ident as ClientIdentity;
					if (clientIdentity != null)
					{
						RemotingServices.uri_hash.Remove(RemotingServices.GetNormalizedUri(clientIdentity.TargetUri));
					}
					else
					{
						RemotingServices.uri_hash.Remove(ident.ObjectUri);
					}
					ident.Disposed = true;
				}
			}
		}

		internal static Identity GetMessageTargetIdentity(IMessage msg)
		{
			if (msg is IInternalMessage)
			{
				return ((IInternalMessage)msg).TargetIdentity;
			}
			Hashtable obj = RemotingServices.uri_hash;
			Identity result;
			lock (obj)
			{
				string normalizedUri = RemotingServices.GetNormalizedUri(((IMethodMessage)msg).Uri);
				result = (RemotingServices.uri_hash[normalizedUri] as ServerIdentity);
			}
			return result;
		}

		internal static void SetMessageTargetIdentity(IMessage msg, Identity ident)
		{
			if (msg is IInternalMessage)
			{
				((IInternalMessage)msg).TargetIdentity = ident;
			}
		}

		internal static bool UpdateOutArgObject(ParameterInfo pi, object local, object remote)
		{
			if (pi.ParameterType.IsArray && ((Array)local).Rank == 1)
			{
				Array array = (Array)local;
				if (array.Rank == 1)
				{
					Array.Copy((Array)remote, array, array.Length);
					return true;
				}
			}
			return false;
		}

		private static string GetNormalizedUri(string uri)
		{
			if (uri.StartsWith("/"))
			{
				return uri.Substring(1);
			}
			return uri;
		}

		[Serializable]
		private class CACD
		{
			public object d;

			public object c;
		}
	}
}
