using System;
using System.Reflection;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace System.Runtime.Remoting.Proxies
{
	internal class RemotingProxy : RealProxy, IRemotingTypeInfo
	{
		private static MethodInfo _cache_GetTypeMethod = typeof(object).GetMethod("GetType");

		private static MethodInfo _cache_GetHashCodeMethod = typeof(object).GetMethod("GetHashCode");

		private IMessageSink _sink;

		private bool _hasEnvoySink;

		private ConstructionCall _ctorCall;

		internal RemotingProxy(Type type, ClientIdentity identity) : base(type, identity)
		{
			this._sink = identity.ChannelSink;
			this._hasEnvoySink = false;
			this._targetUri = identity.TargetUri;
		}

		internal RemotingProxy(Type type, string activationUrl, object[] activationAttributes) : base(type)
		{
			this._hasEnvoySink = false;
			this._ctorCall = ActivationServices.CreateConstructionCall(type, activationUrl, activationAttributes);
		}

		public override IMessage Invoke(IMessage request)
		{
			IMethodCallMessage methodCallMessage = request as IMethodCallMessage;
			if (methodCallMessage != null)
			{
				if (methodCallMessage.MethodBase == RemotingProxy._cache_GetHashCodeMethod)
				{
					return new MethodResponse(base.ObjectIdentity.GetHashCode(), null, null, methodCallMessage);
				}
				if (methodCallMessage.MethodBase == RemotingProxy._cache_GetTypeMethod)
				{
					return new MethodResponse(base.GetProxiedType(), null, null, methodCallMessage);
				}
			}
			IInternalMessage internalMessage = request as IInternalMessage;
			if (internalMessage != null)
			{
				if (internalMessage.Uri == null)
				{
					internalMessage.Uri = this._targetUri;
				}
				internalMessage.TargetIdentity = this._objectIdentity;
			}
			this._objectIdentity.NotifyClientDynamicSinks(true, request, true, false);
			IMessageSink messageSink;
			if (Thread.CurrentContext.HasExitSinks && !this._hasEnvoySink)
			{
				messageSink = Thread.CurrentContext.GetClientContextSinkChain();
			}
			else
			{
				messageSink = this._sink;
			}
			MonoMethodMessage monoMethodMessage = request as MonoMethodMessage;
			IMessage result;
			if (monoMethodMessage == null || monoMethodMessage.CallType == CallType.Sync)
			{
				result = messageSink.SyncProcessMessage(request);
			}
			else
			{
				AsyncResult asyncResult = monoMethodMessage.AsyncResult;
				IMessageCtrl messageCtrl = messageSink.AsyncProcessMessage(request, asyncResult);
				if (asyncResult != null)
				{
					asyncResult.SetMessageCtrl(messageCtrl);
				}
				result = new ReturnMessage(null, new object[0], 0, null, monoMethodMessage);
			}
			this._objectIdentity.NotifyClientDynamicSinks(false, request, true, false);
			return result;
		}

		internal void AttachIdentity(Identity identity)
		{
			this._objectIdentity = identity;
			if (identity is ClientActivatedIdentity)
			{
				ClientActivatedIdentity clientActivatedIdentity = (ClientActivatedIdentity)identity;
				this._targetContext = clientActivatedIdentity.Context;
				base.AttachServer(clientActivatedIdentity.GetServerObject());
				clientActivatedIdentity.SetClientProxy((MarshalByRefObject)this.GetTransparentProxy());
			}
			if (identity is ClientIdentity)
			{
				((ClientIdentity)identity).ClientProxy = (MarshalByRefObject)this.GetTransparentProxy();
				this._targetUri = ((ClientIdentity)identity).TargetUri;
			}
			else
			{
				this._targetUri = identity.ObjectUri;
			}
			if (this._objectIdentity.EnvoySink != null)
			{
				this._sink = this._objectIdentity.EnvoySink;
				this._hasEnvoySink = true;
			}
			else
			{
				this._sink = this._objectIdentity.ChannelSink;
			}
			this._ctorCall = null;
		}

		internal IMessage ActivateRemoteObject(IMethodMessage request)
		{
			if (this._ctorCall == null)
			{
				return new ConstructionResponse(this, null, (IMethodCallMessage)request);
			}
			this._ctorCall.CopyFrom(request);
			return ActivationServices.Activate(this, this._ctorCall);
		}

		public string TypeName
		{
			get
			{
				if (this._objectIdentity is ClientIdentity)
				{
					ObjRef objRef = this._objectIdentity.CreateObjRef(null);
					if (objRef.TypeInfo != null)
					{
						return objRef.TypeInfo.TypeName;
					}
				}
				return base.GetProxiedType().AssemblyQualifiedName;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public bool CanCastTo(Type fromType, object o)
		{
			if (this._objectIdentity is ClientIdentity)
			{
				ObjRef objRef = this._objectIdentity.CreateObjRef(null);
				if (objRef.IsReferenceToWellKnow && (fromType.IsInterface || base.GetProxiedType() == typeof(MarshalByRefObject)))
				{
					return true;
				}
				if (objRef.TypeInfo != null)
				{
					return objRef.TypeInfo.CanCastTo(fromType, o);
				}
			}
			return fromType.IsAssignableFrom(base.GetProxiedType());
		}

		~RemotingProxy()
		{
			if (this._objectIdentity != null && !(this._objectIdentity is ClientActivatedIdentity))
			{
				RemotingServices.DisposeIdentity(this._objectIdentity);
			}
		}
	}
}
