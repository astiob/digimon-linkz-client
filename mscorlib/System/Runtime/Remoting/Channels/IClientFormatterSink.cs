using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Channels
{
	/// <summary>Marks a sink as a client formatter sink that serializes messages into a stream.</summary>
	[ComVisible(true)]
	public interface IClientFormatterSink : IChannelSinkBase, IClientChannelSink, IMessageSink
	{
	}
}
