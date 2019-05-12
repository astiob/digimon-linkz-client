using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIListChatMemberParts : GUIListPartBS
{
	[SerializeField]
	private GameObject goCONTENT_WRAP;

	[SerializeField]
	private GameObject goTX_LOGIN;

	[SerializeField]
	private GameObject goTX_NAME;

	[SerializeField]
	private GameObject goTX_COMMENT;

	[SerializeField]
	private GameObject goMONSTER_ICON;

	[SerializeField]
	private GameObject goTITLE_ICON;

	[SerializeField]
	private Color activeListColor;

	[SerializeField]
	private Color blockListColor;

	[SerializeField]
	private Color defaultListColor;

	[SerializeField]
	private Color disabledListColor;

	[SerializeField]
	private UISprite selectedListSprite;

	[SerializeField]
	private GameObject userTypeIcon;

	[SerializeField]
	private UILabel userTypeLabel;

	[SerializeField]
	private GameObject goMemberExpulsionBtn;

	private UILabel ngTX_LOGIN;

	private UILabel ngTX_NAME;

	private UILabel ngTX_COMMENT;

	private GameObject ngMONSTER_ICON;

	private GameObject ngTITLE_ICON;

	private string thumbMid;

	private UISprite userTypeIconSprite;

	public List<GameObject> goEffList;

	public GUISelectChatMemberPanel selectChatMemberPanel;

	public string lastLoginTime;

	private GameWebAPI.ResponseData_ChatUserList.respUserList data;

	private GameWebAPI.FriendList friendData;

	private bool isDestroy;

	public GameWebAPI.ResponseData_ChatUserList.respUserList Data
	{
		get
		{
			return this.data;
		}
		set
		{
			this.data = value;
			this.ShowGUI();
		}
	}

	public GameWebAPI.FriendList FriendData
	{
		get
		{
			return this.friendData;
		}
		set
		{
			this.friendData = value;
			this.ShowGUI();
		}
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
		if (this.isDestroy)
		{
			return;
		}
		this.ngTX_NAME = this.goTX_NAME.GetComponent<UILabel>();
		this.ngTX_LOGIN = this.goTX_LOGIN.GetComponent<UILabel>();
		this.ngTX_COMMENT = this.goTX_COMMENT.GetComponent<UILabel>();
		this.ngMONSTER_ICON = this.goMONSTER_ICON;
		this.ngTITLE_ICON = this.goTITLE_ICON;
		if (this.goMemberExpulsionBtn != null)
		{
			this.goMemberExpulsionBtn.SetActive(false);
		}
		this.selectedListSprite.color = this.defaultListColor;
		this.userTypeIcon.SetActive(false);
		if (CMD_ChatMenu.instance.openMemberListType == 1)
		{
			this.ngTX_NAME.text = this.FriendData.userData.nickname;
			TitleDataMng.SetTitleIcon(this.FriendData.userData.titleId, this.ngTITLE_ICON.GetComponent<UITexture>());
			this.ngTX_LOGIN.text = this.FriendData.userData.loginTime;
			this.ngTX_COMMENT.text = this.FriendData.userData.description;
			this.thumbMid = this.FriendData.monsterData.monsterId;
		}
		else
		{
			this.ngTX_NAME.text = this.Data.userInfo.nickname;
			TitleDataMng.SetTitleIcon(this.Data.userInfo.titleId, this.ngTITLE_ICON.GetComponent<UITexture>());
			this.ngTX_LOGIN.text = string.Format(StringMaster.GetString("ChatUserLastLogin"), this.Data.userInfo.loginTime);
			this.ngTX_COMMENT.text = this.Data.userInfo.description;
			this.thumbMid = this.Data.userInfo.monsterId;
			if (this.Data.userId == DataMng.Instance().UserId)
			{
				this.selectedListSprite.color = this.activeListColor;
				this.userTypeIconSprite = this.userTypeIcon.GetComponent<UISprite>();
				this.userTypeIconSprite.spriteName = "ListStatus_Waku_Green";
				this.userTypeLabel.text = StringMaster.GetString("ListUsertype_You");
				this.userTypeIcon.SetActive(true);
			}
			else if (BlockManager.instance().blockList != null && BlockManager.instance().CheckBlock(this.Data.userId))
			{
				this.selectedListSprite.color = this.blockListColor;
				this.userTypeIconSprite = this.userTypeIcon.GetComponent<UISprite>();
				this.userTypeIconSprite.spriteName = "ListStatus_Waku_Red";
				this.userTypeLabel.text = StringMaster.GetString("ListUsertype_Blocked");
				this.userTypeIcon.SetActive(true);
				if (ClassSingleton<ChatData>.Instance.CurrentChatInfo.isMaster && CMD_ChatMenu.instance.openMemberListType == 3 && this.goMemberExpulsionBtn != null)
				{
					this.goMemberExpulsionBtn.SetActive(true);
				}
			}
			else if (ClassSingleton<ChatData>.Instance.CurrentChatInfo.isMaster && CMD_ChatMenu.instance.openMemberListType == 3 && this.goMemberExpulsionBtn != null)
			{
				this.goMemberExpulsionBtn.SetActive(true);
			}
		}
	}

	private void UpdateMonsterIcon()
	{
		if (!this.isUpdate)
		{
			MonsterData monsterData = MonsterDataMng.Instance().CreateMonsterDataByMID(this.thumbMid);
			if (monsterData != null)
			{
				GUIMonsterIcon guimonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(monsterData, this.ngMONSTER_ICON.transform.localScale, this.ngMONSTER_ICON.transform.localPosition, this.ngMONSTER_ICON.transform.parent, true, true);
				int add = 1600;
				UISprite component = this.ngMONSTER_ICON.GetComponent<UISprite>();
				if (component != null)
				{
					add = component.depth;
				}
				DepthController depthController = guimonsterIcon.GetDepthController();
				depthController.AddWidgetDepth(guimonsterIcon.transform, add);
			}
			global::Debug.Log("==================================================================== GUIListChatMemberParts MID = " + this.thumbMid);
			this.isUpdate = true;
		}
	}

	public override void OnTouchBegan(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchBegan(touch, pos);
	}

	public override void OnTouchMoved(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchMoved(touch, pos);
	}

	private void OnClickedBtnSelect()
	{
	}

	protected override void Update()
	{
		base.Update();
		this.UpdateMonsterIcon();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.isDestroy = true;
	}

	private void OnChatInviteExec(int idx)
	{
		if (idx == 0)
		{
			int[] uids = new int[]
			{
				int.Parse(this.FriendData.userData.userId)
			};
			CMD_ChatInvitation.instance.ChatInviteExec(ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId, uids);
		}
	}

	private void OnHandoverOwnerExec(int idx)
	{
		if (idx == 0)
		{
			CMD_ChatInvitation.instance.SetChatHandoverGroupOwner(ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId, int.Parse(this.Data.userId));
		}
	}

	private void OnReplyToRequestApproval()
	{
		CMD_ChatInvitation.instance.ReplyToRequestExec(int.Parse(this.Data.chatMemberRequestId), 1);
	}

	private void OnReplyToRequestNotApproval()
	{
		CMD_ChatInvitation.instance.ReplyToRequestExec(int.Parse(this.Data.chatMemberRequestId), 0);
	}

	private void OnCancelMemberInviteExec(int idx)
	{
		if (idx == 0)
		{
			CMD_ChatInvitation.instance.ChatCancelMemberInviteExec(int.Parse(this.Data.chatMemberInviteId));
		}
	}

	private void OnOpenMemberProfile()
	{
		CMD_ProfileFriend.chatMemberData = this.Data;
		CMD_ProfileFriend cmd_ProfileFriend = GUIMain.ShowCommonDialog(delegate(int x)
		{
			this.ShowGUI();
		}, "CMD_ProfileFriend", null) as CMD_ProfileFriend;
		cmd_ProfileFriend.SetLastLoginTime(this.lastLoginTime);
	}

	private void ChangeFriendListColor(Color setCol, Color checkCol)
	{
		foreach (GUIListPartBS guilistPartBS in CMD_ChatInvitation.instance.partFriendListObjs)
		{
			GUIListChatMemberParts component = guilistPartBS.GetComponent<GUIListChatMemberParts>();
			if (component.selectedListSprite.color == checkCol)
			{
				component.selectedListSprite.color = setCol;
			}
		}
	}

	public void OnExpulsionDecision()
	{
		CMD_ChatInvitationModalAlert cmd_ChatInvitationModalAlert = GUIMain.ShowCommonDialog(delegate(int x)
		{
			this.ShowGUI();
		}, "CMD_ChatInvitationModalAlert", null) as CMD_ChatInvitationModalAlert;
		cmd_ChatInvitationModalAlert.SetUserData(this.Data);
	}

	private void OnCrickedInfo()
	{
		if (this.Data != null)
		{
			CMD_ChatInvitation.instance.targetUserId = this.Data.userId;
			ClassSingleton<ChatData>.Instance.CurrentChatInfo.targetNickname = this.Data.userInfo.nickname;
		}
		if (CMD_ChatMenu.instance.openMemberListType == 1)
		{
			if (this.selectedListSprite.color == this.defaultListColor)
			{
				global::Debug.Log(CMD_ChatInvitation.instance.inviteSelectCnt + " : " + CMD_ChatInvitation.instance.manyInviteSelectMaxNum);
				if (CMD_ChatInvitation.instance.inviteSelectCnt + 1 >= CMD_ChatInvitation.instance.manyInviteSelectMaxNum)
				{
					CMD_ChatInvitation.instance.inviteSelectCnt++;
					this.selectedListSprite.color = this.activeListColor;
					CMD_ChatInvitation.instance.AddInviteId(this.FriendData.userData.userId);
					this.ChangeFriendListColor(this.disabledListColor, this.defaultListColor);
				}
				else
				{
					CMD_ChatInvitation.instance.inviteSelectCnt++;
					this.selectedListSprite.color = this.activeListColor;
					CMD_ChatInvitation.instance.AddInviteId(this.FriendData.userData.userId);
				}
			}
			else if (this.selectedListSprite.color == this.activeListColor)
			{
				CMD_ChatInvitation.instance.inviteSelectCnt--;
				this.selectedListSprite.color = this.defaultListColor;
				CMD_ChatInvitation.instance.RemoveInviteId(this.FriendData.userData.userId);
				this.ChangeFriendListColor(this.defaultListColor, this.disabledListColor);
			}
			else
			{
				CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
				cmd_ModalMessage.Title = StringMaster.GetString("SystemConfirm");
				cmd_ModalMessage.Info = StringMaster.GetString("ChatInvitation-03");
			}
			CMD_ChatInvitation.instance.SetInviteNumLabel(CMD_ChatInvitation.instance.inviteSelectCnt);
		}
		else if (CMD_ChatMenu.instance.openMemberListType == 2)
		{
			CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnHandoverOwnerExec), "CMD_Confirm", null) as CMD_Confirm;
			cmd_Confirm.Title = StringMaster.GetString("SystemConfirm");
			cmd_Confirm.Info = StringMaster.GetString("ChatTransferConfirmInfo");
		}
		else if (CMD_ChatMenu.instance.openMemberListType == 4)
		{
			CommonDialog commonDialog = GUIMain.ShowCommonDialog(null, "CMD_ChatModal", null);
			((CMD_ChatModal)commonDialog).SetTitle(string.Format(StringMaster.GetString("ChatApply-01"), this.Data.userInfo.nickname));
			((CMD_ChatModal)commonDialog).SetBtn_CLOSE(true, StringMaster.GetString("SystemButtonClose"), null);
			((CMD_ChatModal)commonDialog).SetBtn_YES(true, StringMaster.GetString("SystemButtonYes"), new Action(this.OnReplyToRequestApproval));
			((CMD_ChatModal)commonDialog).SetBtn_NO(true, StringMaster.GetString("SystemButtonNo"), new Action(this.OnReplyToRequestNotApproval));
		}
		else if (CMD_ChatMenu.instance.openMemberListType == 5)
		{
			CMD_Confirm cmd_Confirm2 = GUIMain.ShowCommonDialog(new Action<int>(this.OnCancelMemberInviteExec), "CMD_Confirm", null) as CMD_Confirm;
			cmd_Confirm2.Title = StringMaster.GetString("ChatInviteCancelTitle");
			cmd_Confirm2.Info = string.Format(StringMaster.GetString("ChatInviteCancelInfo"), this.Data.userInfo.nickname, ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupName);
		}
		else if (this.Data.userId != DataMng.Instance().UserId)
		{
			if (CMD_ChatTop.instance.isGetBlockList)
			{
				this.OnOpenMemberProfile();
			}
			else
			{
				base.StartCoroutine(CMD_ChatTop.instance.getBlockList(delegate(int i)
				{
					this.OnOpenMemberProfile();
				}));
			}
		}
	}
}
