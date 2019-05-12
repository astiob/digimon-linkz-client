using System;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting
{
	internal class ClientActivatedIdentity : ServerIdentity
	{
		private MarshalByRefObject _targetThis;

		public ClientActivatedIdentity(string objectUri, Type objectType) : base(objectUri, null, objectType)
		{
		}

		public MarshalByRefObject GetServerObject()
		{
			return this._serverObject;
		}

		public MarshalByRefObject GetClientProxy()
		{
			return this._targetThis;
		}

		public void SetClientProxy(MarshalByRefObject obj)
		{
			this._targetThis = obj;
		}

		public override void OnLifetimeExpired()
		{
			base.OnLifetimeExpired();
			RemotingServices.DisposeIdentity(this);
		}

		public override IMessage SyncObjectProcessMessage(IMessage msg)
		{
			if (this._serverSink == null)
			{
				bool flag = this._targetThis != null;
				this._serverSink = this._context.CreateServerObjectSinkChain((!flag) ? this._serverObject : this._targetThis, flag);
			}
			return this._serverSink.SyncProcessMessage(msg);
		}

		public override IMessageCtrl AsyncObjectProcessMessage(IMessage msg, IMessageSink replySink)
		{
			if (this._serverSink == null)
			{
				bool flag = this._targetThis != null;
				this._serverSink = this._context.CreateServerObjectSinkChain((!flag) ? this._serverObject : this._targetThis, flag);
			}
			return this._serverSink.AsyncProcessMessage(msg, replySink);
		}
	}
}
