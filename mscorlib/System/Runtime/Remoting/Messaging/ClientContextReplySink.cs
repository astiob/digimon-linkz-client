using System;
using System.Runtime.Remoting.Contexts;

namespace System.Runtime.Remoting.Messaging
{
	internal class ClientContextReplySink : IMessageSink
	{
		private IMessageSink _replySink;

		private Context _context;

		public ClientContextReplySink(Context ctx, IMessageSink replySink)
		{
			this._replySink = replySink;
			this._context = ctx;
		}

		public IMessage SyncProcessMessage(IMessage msg)
		{
			Context.NotifyGlobalDynamicSinks(false, msg, true, true);
			this._context.NotifyDynamicSinks(false, msg, true, true);
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
