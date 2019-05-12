using Neptune.Cloud;
using Neptune.Cloud.Core;
using System;
using UnityEngine;

namespace Network.Socket
{
	public sealed class SocketConnectionAsync : SocketConnection
	{
		public SocketConnectionAsync(string userId, SocketController controller) : base(userId, controller)
		{
		}

		private void ConnectServer(NpCloud socket, SocketNpCloudListener listener)
		{
			this.connectState = SocketConnection.ConnectionState.CONNECTING;
			try
			{
				socket.ConnectAsync(NpCloudSocketType.TCP, base.GetSocketServerUrl(), 10, delegate(bool res)
				{
					this.OnConnectionResult(res, listener);
				}, base.GetSocketConnectionHeader());
			}
			catch (Exception ex)
			{
				global::Debug.LogError(string.Format("Source:{0}", ex.Source));
				global::Debug.LogError(string.Format("StackTrace:{0}", ex.StackTrace));
				global::Debug.LogError(string.Format("TargetSite:{0}", ex.TargetSite));
				global::Debug.LogError(string.Format("Error:{0}", ex.Message));
				this.OnConnectionResult(false, listener);
			}
		}

		private void OnConnectionResult(bool isSuccess, SocketNpCloudListener listener)
		{
			if (isSuccess)
			{
				this.connectState = SocketConnection.ConnectionState.CONNECTED;
				UnityEngine.Object.DontDestroyOnLoad(NpCloudHandlerSystem.gameObject);
				this.socketManager.PongEndCount = this.pongInfo.endCount;
				this.socketManager.PongIntervalTime = this.pongInfo.intervalTime;
			}
			else
			{
				this.connectState = SocketConnection.ConnectionState.NONE;
			}
			listener.OnConnection(isSuccess);
		}

		public void Connection()
		{
			global::Debug.Assert(SocketConnection.ConnectionState.NONE == this.connectState, "接続中もしくは接続処理中");
			global::Debug.Assert(!string.IsNullOrEmpty(this.hostAddress), "ソケットサーバーのURL（アドレス）が空");
			if (!string.IsNullOrEmpty(this.hostAddress))
			{
				this.socketManager = new NpCloud(this.npCloudListener, string.Empty);
				this.ConnectServer(this.socketManager, this.npCloudListener);
			}
		}

		public void ReConnection()
		{
			global::Debug.Assert(this.socketManager != null && SocketConnection.ConnectionState.NONE == this.connectState, "接続中もしくは接続処理中");
			this.ConnectServer(this.socketManager, this.npCloudListener);
		}
	}
}
