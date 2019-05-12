using System;
using UnityEngine;

public class GUIMenuPartsFriend : GUIListPartsFriend
{
	protected override void Awake()
	{
		base.Awake();
		CMD_ProfileFriend.friendData = CMD_PartsFriendCheckScreen.Data;
	}

	protected override void OnTouchEndedProcess()
	{
		CMD_PartsFriendCheckScreen cmd_PartsFriendCheckScreen = UnityEngine.Object.FindObjectOfType<CMD_PartsFriendCheckScreen>();
		cmd_PartsFriendCheckScreen.ClosePanel(true);
		base.Data = CMD_PartsFriendCheckScreen.Data;
		CMD_FriendTop.instance.MenuPartsOperate(this, base.IDX);
		CMD_FriendTop.instance.selectUserlastLoginTime = this.lastLoginTime;
	}
}
