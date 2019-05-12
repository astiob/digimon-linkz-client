using System;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Lifetime
{
	internal class LeaseSink : IMessageSink
	{
		private IMessageSink _nextSink;

		public LeaseSink(IMessageSink nextSink)
		{
			this._nextSink = nextSink;
		}

		public IMessage SyncProcessMessage(IMessage msg)
		{
			this.RenewLease(msg);
			return this._nextSink.SyncProcessMessage(msg);
		}

		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			this.RenewLease(msg);
			return this._nextSink.AsyncProcessMessage(msg, replySink);
		}

		private void RenewLease(IMessage msg)
		{
			ServerIdentity serverIdentity = (ServerIdentity)RemotingServices.GetMessageTargetIdentity(msg);
			ILease lease = serverIdentity.Lease;
			if (lease != null && lease.CurrentLeaseTime < lease.RenewOnCallTime)
			{
				lease.Renew(lease.RenewOnCallTime);
			}
		}

		public IMessageSink NextSink
		{
			get
			{
				return this._nextSink;
			}
		}
	}
}
