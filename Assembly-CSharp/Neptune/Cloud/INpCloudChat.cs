using System;
using System.Collections.Generic;

namespace Neptune.Cloud
{
	public interface INpCloudChat
	{
		void OnFindUser(List<int> on, List<int> off);

		void OnJoinRoom(NpRoomParameter roomData);

		void OnLeaveRoom(NpLeaveParameter leaveData);

		void OnGetRoomList(List<NpRoomParameter> roomData);

		void OnMessage(NpMessageParameter msgData);

		void OnRoomMsgLog(List<NpRoomMsgLog> roomMsgLogList);
	}
}
