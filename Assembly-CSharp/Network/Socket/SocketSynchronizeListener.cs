using Neptune.Cloud.Core;
using System;
using UnityEngine;

namespace Network.Socket
{
	public sealed class SocketSynchronizeListener : ISocketConnectionAsyncListener, ISocketConnectionSyncListener
	{
		private SocketController socketController;

		public SocketSynchronizeListener(SocketController socketController)
		{
			this.socketController = socketController;
		}

		private void DeleteSynchronizeSend()
		{
			if (null != NpCloudHandlerSystem.gameObject)
			{
				SocketSynchronizeSend component = NpCloudHandlerSystem.gameObject.GetComponent<SocketSynchronizeSend>();
				if (null != component)
				{
					UnityEngine.Object.Destroy(component);
				}
			}
		}

		public void OnClose()
		{
			this.DeleteSynchronizeSend();
		}

		public void OnExceptionRequest(string errorCode, string message)
		{
			this.DeleteSynchronizeSend();
		}

		public void OnException(short errorCode, string message)
		{
			this.DeleteSynchronizeSend();
		}

		public void OnExceptionConnectServerIncorrect(string serverAddress)
		{
			this.DeleteSynchronizeSend();
		}

		public void OnConnectionSuccess()
		{
			if (null != NpCloudHandlerSystem.gameObject && null == NpCloudHandlerSystem.gameObject.GetComponent<SocketSynchronizeSend>())
			{
				SocketSynchronizeSend socketSynchronizeSend = NpCloudHandlerSystem.gameObject.AddComponent<SocketSynchronizeSend>();
				if (null != socketSynchronizeSend)
				{
					socketSynchronizeSend.SetSocketController(this.socketController);
				}
			}
		}

		public void OnConnectionFailed()
		{
			this.DeleteSynchronizeSend();
		}
	}
}
