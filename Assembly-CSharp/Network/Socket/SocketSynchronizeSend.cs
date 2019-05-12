using Neptune.Cloud;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Network.Socket
{
	public sealed class SocketSynchronizeSend : MonoBehaviour
	{
		private SocketController socketController;

		private Dictionary<string, object> sendDataBuffer = new Dictionary<string, object>();

		private List<int> toUserBuffer = new List<int>();

		private List<string> ackDataBuffer = new List<string>();

		private void SendACK()
		{
			SocketConnection connection = this.socketController.GetConnection();
			SocketSynchronize socketSynchronize = this.socketController.GetSocketSynchronize();
			if (socketSynchronize != null)
			{
				this.toUserBuffer.Clear();
				this.ackDataBuffer.Clear();
				while (socketSynchronize.PopAckList(this.toUserBuffer, this.ackDataBuffer))
				{
					this.sendDataBuffer.Clear();
					this.sendDataBuffer.Add("ACK", this.ackDataBuffer);
					NpCloud socket = connection.GetSocket();
					socket.Request(this.toUserBuffer, this.sendDataBuffer);
					this.toUserBuffer.Clear();
					this.ackDataBuffer.Clear();
				}
			}
		}

		private void ResendSYN()
		{
			SocketConnection connection = this.socketController.GetConnection();
			SocketSynchronize socketSynchronize = this.socketController.GetSocketSynchronize();
			if (socketSynchronize != null)
			{
				List<SynchronizeInfo> resendSynPacket = socketSynchronize.GetResendSynPacket();
				for (int i = 0; i < resendSynPacket.Count; i++)
				{
					if (resendSynPacket[i].onResend(resendSynPacket[i].count))
					{
						resendSynPacket[i].sendTime = Time.realtimeSinceStartup;
						this.sendDataBuffer.Clear();
						this.sendDataBuffer.Add("SYN", resendSynPacket[i].packet);
						NpCloud socket = connection.GetSocket();
						socket.Request(resendSynPacket[i].toUserIdList, this.sendDataBuffer);
						resendSynPacket[i].count++;
					}
					else
					{
						resendSynPacket[i].delete = true;
					}
				}
				socketSynchronize.InterruptSynchronize();
			}
		}

		public void SetSocketController(SocketController socketController)
		{
			this.socketController = socketController;
		}

		private void Update()
		{
			this.SendACK();
			this.ResendSYN();
		}
	}
}
