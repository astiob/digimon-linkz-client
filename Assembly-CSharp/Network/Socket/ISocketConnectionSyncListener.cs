using System;

namespace Network.Socket
{
	public interface ISocketConnectionSyncListener
	{
		void OnClose();

		void OnExceptionRequest(string errorCode, string message);

		void OnException(short errorCode, string message);

		void OnExceptionConnectServerIncorrect(string serverAddress);
	}
}
