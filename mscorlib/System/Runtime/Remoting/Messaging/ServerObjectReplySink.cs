using System;

namespace System.Runtime.Remoting.Messaging
{
	internal class ServerObjectReplySink : IMessageSink
	{
		private IMessageSink _replySink;

		private ServerIdentity _identity;

		public ServerObjectReplySink(ServerIdentity identity, IMessageSink replySink)
		{
			this._replySink = replySink;
			this._identity = identity;
		}

		public IMessage SyncProcessMessage(IMessage msg)
		{
			this._identity.NotifyServerDynamicSinks(false, msg, true, true);
			return this._replySink.SyncProcessMessage(msg);
		}

		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			throw new NotSupportedException();
		}

		public IMessageSink NextSink
		{
			get
			{
				return this._replySink;
			}
		}
	}
}
