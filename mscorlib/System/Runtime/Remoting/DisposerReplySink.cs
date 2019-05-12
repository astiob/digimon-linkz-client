using System;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting
{
	internal class DisposerReplySink : IMessageSink
	{
		private IMessageSink _next;

		private IDisposable _disposable;

		public DisposerReplySink(IMessageSink next, IDisposable disposable)
		{
			this._next = next;
			this._disposable = disposable;
		}

		public IMessage SyncProcessMessage(IMessage msg)
		{
			this._disposable.Dispose();
			return this._next.SyncProcessMessage(msg);
		}

		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			throw new NotSupportedException();
		}

		public IMessageSink NextSink
		{
			get
			{
				return this._next;
			}
		}
	}
}
