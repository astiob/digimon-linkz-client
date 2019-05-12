using System;
using System.Collections.Generic;

namespace Network.Socket
{
	public sealed class SocketResponseReceive
	{
		private SocketSynchronize synchronize;

		private List<ISocketReceiveCallback> callbackList = new List<ISocketReceiveCallback>();

		public SocketResponseReceive(SocketSynchronize sync)
		{
			this.synchronize = sync;
		}

		private void CallApiResponse(int activityId, List<object> response)
		{
			foreach (ISocketReceiveCallback socketReceiveCallback in this.callbackList)
			{
				socketReceiveCallback.APIResponse(activityId, response);
			}
		}

		private void CallApiResponse(int activityId, Dictionary<string, object> response)
		{
			foreach (ISocketReceiveCallback socketReceiveCallback in this.callbackList)
			{
				socketReceiveCallback.APIResponse(activityId, response);
			}
		}

		private void CallClientMessage(string dataType, Dictionary<object, object> response)
		{
			foreach (ISocketReceiveCallback socketReceiveCallback in this.callbackList)
			{
				socketReceiveCallback.ClientMessage(dataType, response);
			}
		}

		private bool SynchronizeData(int senderUserId, string dataType, KeyValuePair<string, object> message, ref string hash)
		{
			bool result = false;
			if (this.synchronize != null)
			{
				if ("SYN" == dataType)
				{
					Dictionary<object, object> responseDataBody = (Dictionary<object, object>)message.Value;
					hash = this.synchronize.GetHash(responseDataBody);
					if (string.IsNullOrEmpty(hash))
					{
						throw new Exception("ハッシュが未設定の送信データが送られてきました");
					}
					this.synchronize.AddAckData(senderUserId, hash);
					if (this.synchronize.IsReceivedData(hash))
					{
						result = true;
					}
					this.synchronize.DeleteHash(responseDataBody);
				}
				else if ("ACK" == dataType)
				{
					this.synchronize.RemoveSynPacket(senderUserId, (List<object>)message.Value);
					result = true;
				}
			}
			return result;
		}

		public void ReceiveMessage(Dictionary<string, object> messageList)
		{
			int num = -1;
			try
			{
				foreach (KeyValuePair<string, object> keyValuePair in messageList)
				{
					num = int.Parse(keyValuePair.Key);
					if (typeof(List<object>) == keyValuePair.Value.GetType())
					{
						this.CallApiResponse(num, (List<object>)keyValuePair.Value);
					}
					else
					{
						this.CallApiResponse(num, (Dictionary<string, object>)keyValuePair.Value);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("ReceiveMessage : APIレスポンスの処理で失敗. ActivityID={0}. ErrorMessage={1}", new object[]
				{
					num,
					ex.Message
				});
				throw ex;
			}
		}

		public void ReceiveMessage(int senderUserId, Dictionary<string, object> messageList)
		{
			string text = string.Empty;
			try
			{
				foreach (KeyValuePair<string, object> message in messageList)
				{
					text = message.Key;
					bool flag = false;
					string empty = string.Empty;
					if ("NO-SYN" != text)
					{
						Debug.Log("<color=purple>dataType</color> : " + text);
						flag = this.SynchronizeData(senderUserId, text, message, ref empty);
					}
					if (!flag)
					{
						this.CallClientMessage(text, (Dictionary<object, object>)message.Value);
						if (this.synchronize != null)
						{
							this.synchronize.SetReceivedData(empty);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("ReceiveMessage : クライアント間通信の受信処理で失敗. Key={0}. ErrorMessage={1}. Sender={2}", new object[]
				{
					text,
					ex.Message,
					senderUserId
				});
				throw ex;
			}
		}

		public void AddCallback(ISocketReceiveCallback receive)
		{
			this.callbackList.Add(receive);
		}

		public void RemoveCallback(ISocketReceiveCallback receive)
		{
			this.callbackList.Remove(receive);
		}

		public void ClearCallback()
		{
			this.callbackList.Clear();
		}
	}
}
