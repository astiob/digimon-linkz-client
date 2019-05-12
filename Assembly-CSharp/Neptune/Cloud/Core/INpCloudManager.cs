using System;
using System.Collections.Generic;

namespace Neptune.Cloud.Core
{
	public interface INpCloudManager
	{
		void OnJoinRoom(NpRoomParameter roomData);

		void OnLeaveRoom(NpLeaveParameter leaveData);

		void OnMessage(NpMessageParameter msgData);

		void OnRoomMsgLog(List<NpRoomMsgLog> roomMsgLogList);

		void OnResponse(int sender, Dictionary<string, object> parameter);

		void OnCtrlResponse(string command, Dictionary<string, object> parameter);

		void OnFindUser(List<int> on, List<int> off);

		void OnRequestException(NpCloudErrorData errorData);
	}
}
