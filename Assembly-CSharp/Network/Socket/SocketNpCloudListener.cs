using Neptune.Cloud;
using System;
using System.Collections.Generic;

namespace Network.Socket
{
	public sealed class SocketNpCloudListener : INpCloud
	{
		private List<ISocketConnectionSyncListener> eventListenerList = new List<ISocketConnectionSyncListener>();

		private SocketResponseReceive responseReceive;

		public SocketNpCloudListener(SocketResponseReceive receive)
		{
			this.responseReceive = receive;
		}

		private bool IsValidParameter(Dictionary<string, object> parameter)
		{
			bool result = true;
			foreach (KeyValuePair<string, object> keyValuePair in parameter)
			{
				if ("errorMsg" == keyValuePair.Key)
				{
					result = false;
					string message = keyValuePair.Value.ToString();
					foreach (ISocketConnectionSyncListener socketConnectionSyncListener in this.eventListenerList)
					{
						socketConnectionSyncListener.OnException(-1, message);
					}
					break;
				}
			}
			return result;
		}

		public void OnConnection(bool isSuccess)
		{
			foreach (ISocketConnectionSyncListener socketConnectionSyncListener in this.eventListenerList)
			{
				ISocketConnectionAsyncListener socketConnectionAsyncListener = socketConnectionSyncListener as ISocketConnectionAsyncListener;
				if (isSuccess)
				{
					socketConnectionAsyncListener.OnConnectionSuccess();
				}
				else
				{
					socketConnectionAsyncListener.OnConnectionFailed();
				}
			}
		}

		public void OnExit()
		{
			foreach (ISocketConnectionSyncListener socketConnectionSyncListener in this.eventListenerList)
			{
				socketConnectionSyncListener.OnClose();
			}
		}

		public void OnResponse(int sender, Dictionary<string, object> parameter)
		{
			if (this.IsValidParameter(parameter))
			{
				this.responseReceive.ReceiveMessage(sender, parameter);
			}
		}

		public void OnCtrlResponse(string command, Dictionary<string, object> parameter)
		{
			if (this.IsValidParameter(parameter))
			{
				this.responseReceive.ReceiveMessage(parameter);
			}
		}

		public void OnCloudException(short errorCode, string message)
		{
			foreach (ISocketConnectionSyncListener socketConnectionSyncListener in this.eventListenerList)
			{
				if (errorCode == 712)
				{
					socketConnectionSyncListener.OnExceptionConnectServerIncorrect(message);
				}
				else
				{
					socketConnectionSyncListener.OnException(errorCode, message);
				}
			}
		}

		public void OnRequestException(NpCloudErrorData error)
		{
			foreach (ISocketConnectionSyncListener socketConnectionSyncListener in this.eventListenerList)
			{
				socketConnectionSyncListener.OnExceptionRequest(error.resultCode, error.resultMsg);
			}
		}

		public void AddListener(ISocketConnectionSyncListener listener)
		{
			this.eventListenerList.Add(listener);
		}

		public void RemoveListener(ISocketConnectionSyncListener listener)
		{
			this.eventListenerList.Remove(listener);
		}

		public void ClearListener()
		{
			this.eventListenerList.Clear();
		}
	}
}
