using System;
using System.Collections;

namespace System.Runtime.Remoting.Channels
{
	internal class ServerDispatchSinkProvider : IServerChannelSinkProvider, IServerFormatterSinkProvider
	{
		public ServerDispatchSinkProvider()
		{
		}

		public ServerDispatchSinkProvider(IDictionary properties, ICollection providerData)
		{
		}

		public IServerChannelSinkProvider Next
		{
			get
			{
				return null;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public IServerChannelSink CreateSink(IChannelReceiver channel)
		{
			return new ServerDispatchSink();
		}

		public void GetChannelData(IChannelDataStore channelData)
		{
		}
	}
}
