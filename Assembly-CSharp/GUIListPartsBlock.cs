using System;

public class GUIListPartsBlock : GUIListPartsFriend
{
	protected override void OnTouchEndedProcess()
	{
		CMD_ProfileFriend.friendData = base.Data;
		CMD_ProfileFriend cmd_ProfileFriend = GUIMain.ShowCommonDialog(delegate(int idx)
		{
			if (CMD_BlockList.Instance != null)
			{
				CMD_BlockList.Instance.BuildBlockList();
			}
		}, "CMD_ProfileFriend", null) as CMD_ProfileFriend;
		cmd_ProfileFriend.SetLastLoginTime(this.lastLoginTime);
	}
}
