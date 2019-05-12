using System;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Contexts
{
	internal class SynchronizedContextReplySink : IMessageSink
	{
		private IMessageSink _next;

		private bool _newLock;

		private SynchronizationAttribute _att;

		public SynchronizedContextReplySink(IMessageSink next, SynchronizationAttribute att, bool newLock)
		{
			this._newLock = newLock;
			this._next = next;
			this._att = att;
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
			throw new NotSupportedException();
		}

		public IMessage SyncProcessMessage(IMessage msg)
		{
			if (this._newLock)
			{
				this._att.AcquireLock();
			}
			else
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
				if (this._newLock)
				{
					this._att.ReleaseLock();
				}
			}
			return result;
		}
	}
}
