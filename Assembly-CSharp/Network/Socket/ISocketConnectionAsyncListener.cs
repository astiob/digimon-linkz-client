using System;

namespace Network.Socket
{
	public interface ISocketConnectionAsyncListener : ISocketConnectionSyncListener
	{
		void OnConnectionSuccess();

		void OnConnectionFailed();
	}
}
