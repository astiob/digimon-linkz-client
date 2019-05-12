using System;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Channels
{
	internal class ExceptionFilterSink : IMessageSink
	{
		private IMessageSink _next;

		private IMessage _call;

		public ExceptionFilterSink(IMessage call, IMessageSink next)
		{
			this._call = call;
			this._next = next;
		}

		public IMessage SyncProcessMessage(IMessage msg)
		{
			return this._next.SyncProcessMessage(ChannelServices.CheckReturnMessage(this._call, msg));
		}

		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			throw new InvalidOperationException();
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
