using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Channels
{
	/// <summary>Provides required functions and properties for the sender channels.</summary>
	[ComVisible(true)]
	public interface IChannelSender : IChannel
	{
		/// <summary>Returns a channel message sink that delivers messages to the specified URL or channel data object.</summary>
		/// <returns>A channel message sink that delivers messages to the specified URL or channel data object, or null if the channel cannot connect to the given endpoint.</returns>
		/// <param name="url">The URL to which the new sink will deliver messages. Can be null. </param>
		/// <param name="remoteChannelData">The channel data object of the remote host to which the new sink will deliver messages. Can be null. </param>
		/// <param name="objectURI">When this method returns, contains a URI of the new channel message sink that delivers messages to the specified URL or channel data object. This parameter is passed uninitialized. </param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		IMessageSink CreateMessageSink(string url, object remoteChannelData, out string objectURI);
	}
}
