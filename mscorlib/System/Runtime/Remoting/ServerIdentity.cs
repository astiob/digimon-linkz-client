using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Services;

namespace System.Runtime.Remoting
{
	internal abstract class ServerIdentity : Identity
	{
		protected Type _objectType;

		protected MarshalByRefObject _serverObject;

		protected IMessageSink _serverSink;

		protected Context _context;

		protected Lease _lease;

		public ServerIdentity(string objectUri, Context context, Type objectType) : base(objectUri)
		{
			this._objectType = objectType;
			this._context = context;
		}

		public Type ObjectType
		{
			get
			{
				return this._objectType;
			}
		}

		public void StartTrackingLifetime(ILease lease)
		{
			if (lease != null && lease.CurrentState == LeaseState.Null)
			{
				lease = null;
			}
			if (lease != null)
			{
				if (!(lease is Lease))
				{
					lease = new Lease();
				}
				this._lease = (Lease)lease;
				LifetimeServices.TrackLifetime(this);
			}
		}

		public virtual void OnLifetimeExpired()
		{
			this.DisposeServerObject();
		}

		public override ObjRef CreateObjRef(Type requestedType)
		{
			if (this._objRef != null)
			{
				this._objRef.UpdateChannelInfo();
				return this._objRef;
			}
			if (requestedType == null)
			{
				requestedType = this._objectType;
			}
			this._objRef = new ObjRef();
			this._objRef.TypeInfo = new TypeInfo(requestedType);
			this._objRef.URI = this._objectUri;
			if (this._envoySink != null && !(this._envoySink is EnvoyTerminatorSink))
			{
				this._objRef.EnvoyInfo = new EnvoyInfo(this._envoySink);
			}
			return this._objRef;
		}

		public void AttachServerObject(MarshalByRefObject serverObject, Context context)
		{
			this.DisposeServerObject();
			this._context = context;
			this._serverObject = serverObject;
			if (RemotingServices.IsTransparentProxy(serverObject))
			{
				RealProxy realProxy = RemotingServices.GetRealProxy(serverObject);
				if (realProxy.ObjectIdentity == null)
				{
					realProxy.ObjectIdentity = this;
				}
			}
			else
			{
				if (this._objectType.IsContextful)
				{
					this._envoySink = context.CreateEnvoySink(serverObject);
				}
				this._serverObject.ObjectIdentity = this;
			}
		}

		public Lease Lease
		{
			get
			{
				return this._lease;
			}
		}

		public Context Context
		{
			get
			{
				return this._context;
			}
			set
			{
				this._context = value;
			}
		}

		public abstract IMessage SyncObjectProcessMessage(IMessage msg);

		public abstract IMessageCtrl AsyncObjectProcessMessage(IMessage msg, IMessageSink replySink);

		protected void DisposeServerObject()
		{
			if (this._serverObject != null)
			{
				MarshalByRefObject serverObject = this._serverObject;
				this._serverObject.ObjectIdentity = null;
				this._serverObject = null;
				this._serverSink = null;
				TrackingServices.NotifyDisconnectedObject(serverObject);
			}
		}
	}
}
