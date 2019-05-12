using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Network.Socket
{
	public sealed class SocketSynchronize
	{
		private int hashCreateCount;

		private HashAlgorithm hashAlgorithm = SHA1.Create();

		private StringBuilder hashBuffer = new StringBuilder();

		private List<SynchronizeInfo> synPacketList = new List<SynchronizeInfo>();

		private int synPacketNum;

		private float interval;

		private List<string> receivedDataList = new List<string>();

		private int receivedDataNum;

		private Dictionary<int, List<string>> ackList = new Dictionary<int, List<string>>();

		private int ackHashNum;

		private Action<SocketSynchronize.AlertType> alertAction;

		public SocketSynchronize(int synNum, int recvNum, int ackHashNum, float interval, Action<SocketSynchronize.AlertType> alertAction)
		{
			global::Debug.Assert(10 <= synNum, "再送用同期情報の保存数が少なすぎます");
			global::Debug.Assert(20 <= recvNum, "受信済みデータの保存数が少なすぎます");
			global::Debug.Assert(10 <= ackHashNum, "ACK用のハッシュ一覧の想定許容数が少なすぎます");
			global::Debug.Assert(3f <= interval, "再送間隔が短すぎます. 指定秒数=" + interval + "秒");
			this.synPacketNum = synNum;
			this.receivedDataNum = recvNum;
			this.ackHashNum = ackHashNum;
			this.interval = interval;
			this.alertAction = alertAction;
		}

		public void AddHash(Dictionary<string, object> sendData)
		{
			string s = string.Format("{0}_{1}", this.hashCreateCount.ToString(), DateTime.Now.ToString("ddhhmmssffff"));
			this.hashCreateCount++;
			byte[] array = this.hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(s));
			this.hashBuffer.Length = 0;
			foreach (byte b in array)
			{
				this.hashBuffer.Append(b.ToString("x2"));
			}
			sendData.Add("Hash", this.hashBuffer.ToString());
		}

		public void AddSynPacket(Dictionary<string, object> synPacket, List<int> toUserList, Func<int, bool> onResendAction, Action<bool> onSynchronizedAction)
		{
			if (toUserList.Count == 0)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			foreach (KeyValuePair<string, object> keyValuePair in synPacket)
			{
				dictionary.Add(keyValuePair.Key, keyValuePair.Value);
			}
			SynchronizeInfo item = new SynchronizeInfo
			{
				hash = (string)synPacket["Hash"],
				packet = dictionary,
				interval = this.interval,
				sendTime = Time.realtimeSinceStartup,
				toUserIdList = new List<int>(toUserList),
				onResend = onResendAction,
				onSynchronized = onSynchronizedAction
			};
			this.synPacketList.Add(item);
			if (this.synPacketList.Count > this.synPacketNum)
			{
				this.synPacketList.RemoveAt(0);
				if (this.alertAction != null)
				{
					this.alertAction(SocketSynchronize.AlertType.SYN_PACKET_LIST_BUFFER_OVER_FLOW);
				}
			}
		}

		public List<SynchronizeInfo> GetResendSynPacket()
		{
			return this.synPacketList.Where((SynchronizeInfo x) => Time.realtimeSinceStartup > x.interval + x.sendTime).ToList<SynchronizeInfo>();
		}

		public void InterruptSynchronize()
		{
			this.synPacketList.RemoveAll((SynchronizeInfo x) => x.delete);
		}

		public void RemoveSynPacket(int ackSenderUserId, List<object> ackData)
		{
			SocketSynchronize.<RemoveSynPacket>c__AnonStorey2D5 <RemoveSynPacket>c__AnonStorey2D = new SocketSynchronize.<RemoveSynPacket>c__AnonStorey2D5();
			<RemoveSynPacket>c__AnonStorey2D.ackData = ackData;
			int i;
			for (i = 0; i < <RemoveSynPacket>c__AnonStorey2D.ackData.Count; i++)
			{
				SynchronizeInfo synchronizeInfo = this.synPacketList.SingleOrDefault((SynchronizeInfo x) => x.hash == (string)<RemoveSynPacket>c__AnonStorey2D.ackData[i]);
				if (synchronizeInfo != null)
				{
					synchronizeInfo.toUserIdList.Remove(ackSenderUserId);
					if (synchronizeInfo.toUserIdList.Count == 0)
					{
						synchronizeInfo.onSynchronized(true);
						this.synPacketList.Remove(synchronizeInfo);
					}
				}
			}
		}

		public string GetHash(Dictionary<object, object> responseDataBody)
		{
			string result = string.Empty;
			foreach (KeyValuePair<object, object> keyValuePair in responseDataBody)
			{
				if ("Hash" == (string)keyValuePair.Key)
				{
					result = (string)keyValuePair.Value;
					break;
				}
			}
			return result;
		}

		public bool IsReceivedData(string hash)
		{
			return this.receivedDataList.Contains(hash);
		}

		public void DeleteHash(Dictionary<object, object> responseDataBody)
		{
			responseDataBody.Remove("Hash");
		}

		public void SetReceivedData(string hash)
		{
			if (!string.IsNullOrEmpty(hash) && !this.receivedDataList.Contains(hash))
			{
				this.receivedDataList.Add(hash);
				if (this.receivedDataList.Count > this.receivedDataNum)
				{
					this.receivedDataList.RemoveAt(0);
				}
			}
		}

		public void AddAckData(int toUserId, string hash)
		{
			if (!this.ackList.ContainsKey(toUserId))
			{
				this.ackList.Add(toUserId, new List<string>());
			}
			if (!this.ackList[toUserId].Contains(hash))
			{
				this.ackList[toUserId].Add(hash);
				if (this.ackHashNum < this.ackList[toUserId].Count && this.alertAction != null)
				{
					this.alertAction(SocketSynchronize.AlertType.ACK_HASH_LIST_BUFFER_OVER_FLOW);
				}
			}
		}

		public bool PopAckList(List<int> toUserList, List<string> ackData)
		{
			bool result = false;
			KeyValuePair<int, List<string>> keyValuePair = this.ackList.FirstOrDefault((KeyValuePair<int, List<string>> x) => 0 < x.Value.Count);
			if (!keyValuePair.Equals(default(KeyValuePair<int, List<string>>)))
			{
				toUserList.Add(keyValuePair.Key);
				ackData.AddRange(keyValuePair.Value);
				keyValuePair.Value.Clear();
				result = true;
			}
			return result;
		}

		public void DeleteLeaveUser(int leaveUserId)
		{
			this.synPacketList.RemoveAll(delegate(SynchronizeInfo x)
			{
				x.toUserIdList.Remove(leaveUserId);
				return 0 == x.toUserIdList.Count;
			});
			this.ackList.Remove(leaveUserId);
		}

		public void Clear()
		{
			this.hashBuffer.Length = 0;
			this.synPacketList.Clear();
			this.receivedDataList.Clear();
			this.ackList.Clear();
		}

		public enum AlertType
		{
			NONE,
			SYN_PACKET_LIST_BUFFER_OVER_FLOW,
			ACK_HASH_LIST_BUFFER_OVER_FLOW
		}
	}
}
