using System;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Contexts
{
	internal class SynchronizedClientContextSink : IMessageSink
	{
		private IMessageSink _next;

		private SynchronizationAttribute _att;

		public SynchronizedClientContextSink(IMessageSink next, SynchronizationAttribute att)
		{
			this._att = att;
			this._next = next;
		}

		public IMessageSink NextSink
		{
			get
			{
				return this._next;
			}
		}

		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			if (this._att.IsReEntrant)
			{
				this._att.ReleaseLock();
				replySink = new SynchronizedContextReplySink(replySink, this._att, true);
			}
			return this._next.AsyncProcessMessage(msg, replySink);
		}

		public IMessage SyncProcessMessage(IMessage msg)
		{
			if (this._att.IsReEntrant)
			{
				this._att.ReleaseLock();
			}
			IMessage result;
			try
			{
				result = this._next.SyncProcessMessage(msg);
			}
			finally
			{
				if (this._att.IsReEntrant)
				{
					this._att.AcquireLock();
				}
			}
			return result;
		}
	}
}
