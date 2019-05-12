using System;

public class ChatData : ClassSingleton<ChatData>
{
	private ChatData.ChatInfo currentInfo = new ChatData.ChatInfo();

	public ChatData.ChatInfo CurrentChatInfo
	{
		get
		{
			return this.currentInfo;
		}
	}

	public void UpdateMaxJoinState()
	{
		int num = 0;
		if (this.currentInfo.joinGroupData.groupList != null)
		{
			num += this.currentInfo.joinGroupData.groupList.Length;
		}
		if (this.currentInfo.requestGroupData.requestList != null)
		{
			num += this.currentInfo.requestGroupData.requestList.Length;
		}
		if (num >= ChatConstValue.CHAT_GROUP_JOIN_MAX_NUM)
		{
			ClassSingleton<ChatData>.Instance.CurrentChatInfo.isMaxJoin = true;
		}
		else
		{
			ClassSingleton<ChatData>.Instance.CurrentChatInfo.isMaxJoin = false;
		}
	}

	public class ChatInfo
	{
		public int groupId;

		public string groupName;

		public int groupMemberNum;

		public bool isMaster;

		public int groupMessageType;

		public string groupLastHistoryId;

		public bool isMaxJoin;

		public string targetNickname;

		public int openListType;

		public GameWebAPI.ResponseData_ChatGroupList joinGroupData;

		public GameWebAPI.ResponseData_ChatGroupList inviteGroupData;

		public GameWebAPI.ResponseData_ChatGroupList requestGroupData;
	}
}
