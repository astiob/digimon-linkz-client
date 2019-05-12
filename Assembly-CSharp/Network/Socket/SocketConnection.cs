using Neptune.Cloud;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Network.Socket
{
	public abstract class SocketConnection
	{
		private const string PROJECT_ID = "usdigimon";

		protected const int CONNECTION_TIME_OUT = 10;

		protected SocketConnection.ConnectionState connectState;

		private string clientUserId;

		protected SocketPongInfo pongInfo;

		protected string hostAddress;

		protected NpCloud socketManager;

		protected SocketNpCloudListener npCloudListener;

		public SocketConnection(string userId, SocketController controller)
		{
			this.clientUserId = userId;
			this.pongInfo = controller.GetPongInfo();
			this.npCloudListener = controller.GetEventListener();
		}

		protected string GetSocketServerUrl()
		{
			return string.Format("tcp://{0}/?projectid={1}&me={2}", this.hostAddress, "usdigimon", this.clientUserId);
		}

		protected Dictionary<string, object> GetSocketConnectionHeader()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("X-AppVer", WebAPIPlatformValue.GetAppVersion());
			return new Dictionary<string, object>
			{
				{
					"headers",
					dictionary
				}
			};
		}

		public IEnumerator GetSocketServerAddress(Action onFinish)
		{
			GameWebAPI.GetTcpShardingString getTcpShardingString = new GameWebAPI.GetTcpShardingString();
			getTcpShardingString.SetSendData = delegate(GameWebAPI.ReqData_GetTcpShardingString param)
			{
				param.type = string.Empty;
			};
			getTcpShardingString.OnReceived = delegate(GameWebAPI.RespData_GetTcpShardingString response)
			{
				GameWebAPI.RespData_GetTcpShardingString connectionServerInfo = response;
			};
			GameWebAPI.GetTcpShardingString request = getTcpShardingString;
			yield return AppCoroutine.Start(request.Run(delegate()
			{
				this.hostAddress = connectionServerInfo.server;
				if (onFinish != null)
				{
					onFinish();
				}
			}, null, null), true);
			yield break;
		}

		public void SetHostAddress(string address)
		{
			this.hostAddress = address;
		}

		public bool IsConnecting()
		{
			bool result = false;
			if (this.socketManager != null && this.connectState == SocketConnection.ConnectionState.CONNECTING)
			{
				result = true;
			}
			return result;
		}

		public bool IsConnected()
		{
			bool result = false;
			if (this.socketManager != null && this.connectState == SocketConnection.ConnectionState.CONNECTED)
			{
				result = this.socketManager.IsConnected;
			}
			return result;
		}

		public void Close()
		{
			if (this.socketManager != null)
			{
				this.socketManager.Exit();
			}
			this.connectState = SocketConnection.ConnectionState.NONE;
		}

		public void Delete()
		{
			if (this.socketManager != null && !this.socketManager.IsConnected)
			{
				this.socketManager.Delete();
				this.socketManager = null;
			}
			this.connectState = SocketConnection.ConnectionState.NONE;
		}

		public NpCloud GetSocket()
		{
			return this.socketManager;
		}

		protected enum ConnectionState
		{
			NONE,
			CLOSE = 0,
			CONNECTING,
			CONNECTED
		}
	}
}
