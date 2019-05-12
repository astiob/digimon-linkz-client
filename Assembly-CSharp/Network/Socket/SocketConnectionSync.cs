using Neptune.Cloud;
using Neptune.Cloud.Core;
using System;
using UnityEngine;

namespace Network.Socket
{
	[Obsolete("Obsolete class. use SocketConnectionAsync.")]
	public sealed class SocketConnectionSync : SocketConnection
	{
		public SocketConnectionSync(string userId, SocketController controller) : base(userId, controller)
		{
		}

		private bool ConnectServer(NpCloud socket)
		{
			bool flag = false;
			this.connectState = SocketConnection.ConnectionState.CONNECTING;
			try
			{
				flag = socket.Connect(NpCloudSocketType.TCP, base.GetSocketServerUrl(), 10, base.GetSocketConnectionHeader());
				if (!flag)
				{
					throw new Exception("NpCloud Connect に失敗");
				}
				this.connectState = SocketConnection.ConnectionState.CONNECTED;
				UnityEngine.Object.DontDestroyOnLoad(NpCloudHandlerSystem.gameObject);
				this.socketManager.PongEndCount = this.pongInfo.endCount;
				this.socketManager.PongIntervalTime = this.pongInfo.intervalTime;
			}
			catch (Exception ex)
			{
				global::Debug.LogError(string.Format("Source:{0}", ex.Source));
				global::Debug.LogError(string.Format("StackTrace:{0}", ex.StackTrace));
				global::Debug.LogError(string.Format("TargetSite:{0}", ex.TargetSite));
				global::Debug.LogError(string.Format("Error:{0}", ex.Message));
				this.connectState = SocketConnection.ConnectionState.NONE;
			}
			return flag;
		}

		public bool Connection()
		{
			global::Debug.Assert(SocketConnection.ConnectionState.NONE == this.connectState, "接続中もしくは接続処理中");
			global::Debug.Assert(!string.IsNullOrEmpty(this.hostAddress), "ソケットサーバーのURL（アドレス）が空");
			bool result = false;
			if (!string.IsNullOrEmpty(this.hostAddress))
			{
				this.socketManager = new NpCloud(this.npCloudListener, string.Empty);
				result = this.ConnectServer(this.socketManager);
			}
			return result;
		}

		public bool ReConnection()
		{
			global::Debug.Assert(this.socketManager != null && SocketConnection.ConnectionState.NONE == this.connectState, "再接続できる状態ではありません");
			return this.ConnectServer(this.socketManager);
		}
	}
}
