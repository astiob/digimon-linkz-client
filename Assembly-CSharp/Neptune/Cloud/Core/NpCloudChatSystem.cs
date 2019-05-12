using System;
using System.Collections.Generic;

namespace Neptune.Cloud.Core
{
	public class NpCloudChatSystem : INpCloudManagerSystem
	{
		private Action<NpMessageParameter> mMessage;

		private Action<List<NpRoomMsgLog>> mRoomMsgLog;

		private Action<NpCloudErrorData> mRequestException;

		public Action<NpMessageParameter> Message
		{
			get
			{
				return this.mMessage;
			}
			set
			{
				this.mMessage = value;
			}
		}

		public Action<List<NpRoomMsgLog>> RoomMsgLog
		{
			get
			{
				return this.mRoomMsgLog;
			}
			set
			{
				this.mRoomMsgLog = value;
			}
		}

		public Action<NpCloudErrorData> RequestException
		{
			get
			{
				return this.mRequestException;
			}
			set
			{
				this.mRequestException = value;
			}
		}

		public void ClearData()
		{
		}

		public void SendRoomMessage(string roomId, string message)
		{
			NpChatRequestParameter npChatRequestParameter = new NpChatRequestParameter();
			npChatRequestParameter.msg.Add(message);
			npChatRequestParameter.ngcheck = 1;
			npChatRequestParameter.roomId = roomId;
			npChatRequestParameter.to = NpCloudManager.GetInstance().GetUserList(roomId);
			NpCloudServerData npCloudServerData = new NpCloudServerData();
			npCloudServerData.value = NpCloudValueType.send;
			npCloudServerData.type = NpCloudType.direct;
			npCloudServerData.content = npChatRequestParameter;
			npCloudServerData.ctrl = "9201";
			NpCloudManager.GetInstance().Request(npCloudServerData);
		}

		public void GetRoomMsgLog(string roomId, int limit)
		{
			NpChatLogRequestParameter npChatLogRequestParameter = new NpChatLogRequestParameter();
			npChatLogRequestParameter.roomId = roomId;
			npChatLogRequestParameter.limit = limit;
			NpCloudServerData npCloudServerData = new NpCloudServerData();
			npCloudServerData.content = npChatLogRequestParameter;
			npCloudServerData.type = NpCloudType.direct;
			npCloudServerData.ctrl = "9202";
			npCloudServerData.value = NpCloudValueType.chathis;
			NpCloudManager.GetInstance().Request(npCloudServerData);
		}

		public bool Receive(string command, object response, long resTime)
		{
			if (command.Equals("9201"))
			{
				NPCloudReceiveParameter<string> npcloudReceiveParameter = response as NPCloudReceiveParameter<string>;
				NpMessageParameter obj = new NpMessageParameter(npcloudReceiveParameter.option.roomId, (int)npcloudReceiveParameter.sender, npcloudReceiveParameter.body, NpUtil.MsTimestampToDateTime(npcloudReceiveParameter.resTime));
				this.mMessage(obj);
				return true;
			}
			if (command.Equals("9202"))
			{
				NPCloudReceiveParameter<List<RoomMsgLog>> npcloudReceiveParameter2 = response as NPCloudReceiveParameter<List<RoomMsgLog>>;
				List<NpRoomMsgLog> list = new List<NpRoomMsgLog>();
				foreach (RoomMsgLog roomMsgLog in npcloudReceiveParameter2.body)
				{
					DateTime sendtime = NpUtil.MsTimestampToDateTime(roomMsgLog.createdat);
					if (!string.IsNullOrEmpty(roomMsgLog.userid) || !string.IsNullOrEmpty(roomMsgLog.sendedmsg))
					{
						NpRoomMsgLog item = new NpRoomMsgLog(roomMsgLog.userid, sendtime, roomMsgLog.sendedmsg);
						list.Add(item);
					}
				}
				this.mRoomMsgLog(list);
				return true;
			}
			return false;
		}

		public void Update()
		{
		}

		public bool ReceiveException(NpCloudErrorData error)
		{
			if (error.command.Equals("9201") || error.command.Equals("9202"))
			{
				this.mRequestException(error);
				return true;
			}
			return false;
		}

		public void CloudExit(int exitCode, string message)
		{
		}
	}
}
