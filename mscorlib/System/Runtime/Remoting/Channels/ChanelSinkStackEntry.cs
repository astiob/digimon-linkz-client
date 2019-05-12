using System;

namespace System.Runtime.Remoting.Channels
{
	internal class ChanelSinkStackEntry
	{
		public IChannelSinkBase Sink;

		public object State;

		public ChanelSinkStackEntry Next;

		public ChanelSinkStackEntry(IChannelSinkBase sink, object state, ChanelSinkStackEntry next)
		{
			this.Sink = sink;
			this.State = state;
			this.Next = next;
		}
	}
}
