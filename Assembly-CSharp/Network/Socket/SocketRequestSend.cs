using Neptune.Cloud;
using System;
using System.Collections.Generic;

namespace Network.Socket
{
	public abstract class SocketRequestSend
	{
		private SocketController socketController;

		private Dictionary<string, object> sendDataBuffer = new Dictionary<string, object>();

		public SocketRequestSend(SocketController socketController)
		{
			this.socketController = socketController;
		}

		private void SendToClient(string dataType, Dictionary<string, object> data, List<int> toUserList)
		{
			SocketConnection connection = this.socketController.GetConnection();
			this.sendDataBuffer.Clear();
			this.sendDataBuffer.Add(dataType, data);
			NpCloud socket = connection.GetSocket();
			socket.Request(toUserList, this.sendDataBuffer);
		}

		protected void SendAPI(Dictionary<string, object> data)
		{
			SocketConnection connection = this.socketController.GetConnection();
			if (connection.IsConnected())
			{
				this.sendDataBuffer.Clear();
				this.sendDataBuffer.Add("activityList", data);
				NpCloud socket = connection.GetSocket();
				socket.Request(this.sendDataBuffer, "socket/ActiveController", false, 0u);
			}
		}

		protected void SendShareData(Dictionary<string, object> data, List<int> toUserList)
		{
			SocketConnection connection = this.socketController.GetConnection();
			if (connection.IsConnected())
			{
				this.SendToClient("NO-SYN", data, toUserList);
			}
		}

		protected void SendShareDataSYN(Dictionary<string, object> data, List<int> toUserList, Func<int, bool> onResend, Action<bool> onSynchronized)
		{
			SocketConnection connection = this.socketController.GetConnection();
			SocketSynchronize socketSynchronize = this.socketController.GetSocketSynchronize();
			if (connection.IsConnected() && socketSynchronize != null)
			{
				socketSynchronize.AddHash(data);
				this.SendToClient("SYN", data, toUserList);
				socketSynchronize.AddSynPacket(data, toUserList, onResend, onSynchronized);
			}
		}
	}
}
