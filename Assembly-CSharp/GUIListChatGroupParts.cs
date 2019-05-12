using Master;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GUIListChatGroupParts : GUIListPartBS
{
	[SerializeField]
	private GameObject goTX_NUMBER;

	[SerializeField]
	private GameObject goTX_MEMBER;

	[SerializeField]
	private GameObject goTX_GRPNAME;

	[SerializeField]
	private GameObject goTX_NAME;

	[SerializeField]
	private GameObject goTX_APPROVALTYPE;

	[SerializeField]
	private GameObject goTX_MASTER;

	[SerializeField]
	private GameObject goTX_COMMENT;

	[SerializeField]
	private GameObject goICON_CATEGORY;

	[SerializeField]
	private UISprite spBgPlate;

	[SerializeField]
	private Color activeListColor;

	[SerializeField]
	private Color defaultListColor;

	private UILabel ngTX_NUMBER;

	private UILabel ngTX_MEMBER;

	private UILabel ngTX_GRPNAME;

	private UILabel ngTX_NAME;

	private UILabel ngTX_APPROVALTYPE;

	private UILabel ngTX_MASTER;

	private UILabel ngTX_COMMENT;

	private UISprite ngICON_CATEGORY;

	public GameObject groupMasterIcon;

	public GameObject newIcon;

	public List<GameObject> goEffList;

	private bool isTouchEndFromChild;

	public bool isSelected;

	public GUISelectChatGroupPanel selectChatGroupPanel;

	private GameWebAPI.ResponseData_ChatGroupList.lists data;

	public GameWebAPI.ResponseData_ChatGroupList.lists Data
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

	protected override void Awake()
	{
		base.Awake();
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
		this.ngTX_GRPNAME = this.goTX_GRPNAME.GetComponent<UILabel>();
		this.ngTX_MEMBER = this.goTX_MEMBER.GetComponent<UILabel>();
		this.ngTX_NUMBER = this.goTX_NUMBER.GetComponent<UILabel>();
		this.ngTX_MASTER = this.goTX_MASTER.GetComponent<UILabel>();
		this.ngTX_NAME = this.goTX_NAME.GetComponent<UILabel>();
		this.ngTX_APPROVALTYPE = this.goTX_APPROVALTYPE.GetComponent<UILabel>();
		this.ngTX_COMMENT = this.goTX_COMMENT.GetComponent<UILabel>();
		this.ngICON_CATEGORY = this.goICON_CATEGORY.GetComponent<UISprite>();
		this.ngTX_MASTER.text = StringMaster.GetString("ChatMaster");
		if (this.Data != null)
		{
			this.ngICON_CATEGORY.spriteName = ChatConstValue.SPRITE_GROUP_CATEGORY[int.Parse(this.Data.categoryId)];
			this.ngTX_GRPNAME.text = this.Data.groupName;
			if (ChatConstValue.CHAT_GROUP_MEMBER_MAX_NUM > int.Parse(this.Data.memberNum))
			{
				this.ngTX_MEMBER.text = StringMaster.GetString("ChatMember");
			}
			this.ngTX_NUMBER.text = string.Format(StringMaster.GetString("SystemFraction"), this.Data.memberNum, ChatConstValue.CHAT_GROUP_MEMBER_MAX_NUM);
			if (CMD_ChatTop.instance != null && !CMD_ChatTop.instance.searchFieldTypeSearch.activeSelf)
			{
				if (ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification.prefsLastHistoryList != null && ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification.prefsLastHistoryList.Count > 0)
				{
					var <>__AnonType = ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification.prefsLastHistoryList.Where((FaceChatNotification.UserPrefsHistoryIdList item) => item.historyData.chatGroupId == this.Data.chatGroupId).Select((FaceChatNotification.UserPrefsHistoryIdList item) => new
					{
						item.historyData.chatGroupId,
						item.historyData.chatMessageHistoryId
					}).SingleOrDefault();
					if (<>__AnonType == null)
					{
						this.newIcon.SetActive(true);
					}
					else if (this.Data.chatMessageHistoryId != <>__AnonType.chatMessageHistoryId && this.Data.chatMessageHistoryId != null)
					{
						this.newIcon.SetActive(true);
					}
				}
				else
				{
					this.newIcon.SetActive(true);
				}
			}
			if (int.Parse(this.Data.approvalType) == 2)
			{
				this.ngTX_APPROVALTYPE.text = StringMaster.GetString("ChatCategory-01");
			}
			else
			{
				this.ngTX_APPROVALTYPE.text = StringMaster.GetString("ChatCategory-02");
			}
			if (DataMng.Instance().UserId == this.Data.ownerUserId)
			{
				this.groupMasterIcon.SetActive(true);
			}
			this.ngTX_NAME.text = this.Data.ownerInfo.nickname;
			this.ngTX_COMMENT.text = this.Data.comment;
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
		this.beganPostion = pos;
		base.OnTouchBegan(touch, pos);
		this.isTouchEndFromChild = false;
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

	public override void OnTouchEnded(Touch touch, Vector2 pos, bool flag)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchEnded(touch, pos, flag);
		if (flag)
		{
			float magnitude = (this.beganPostion - pos).magnitude;
			if (magnitude < 40f && !this.isTouchEndFromChild)
			{
				this.OnTouchEndedProcess();
			}
		}
	}

	protected override void Update()
	{
		base.Update();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	private void OnChatApplyExec(int idx)
	{
		if (idx == 0)
		{
			CMD_ChatTop.instance.chatRoomInRequest();
		}
	}

	private void OnReplyToInviteJoin()
	{
		CMD_ChatReqList.instance.ReplyToInviteExec(int.Parse(this.Data.chatMemberInviteId), 1);
	}

	private void OnReplyToInviteNotJoin()
	{
		CMD_ChatReqList.instance.ReplyToInviteExec(int.Parse(this.Data.chatMemberInviteId), 0);
	}

	private void OnCancelMemberRequestExec(int idx)
	{
		if (idx == 0)
		{
			CMD_ChatReqList.instance.RequestCanselExec(int.Parse(this.Data.chatMemberRequestId));
		}
	}

	private void OnTouchEndedProcess()
	{
		ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId = int.Parse(this.Data.chatGroupId);
		ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupName = this.Data.groupName;
		ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupMemberNum = int.Parse(this.Data.memberNum);
		ClassSingleton<ChatData>.Instance.CurrentChatInfo.isMaster = (DataMng.Instance().UserId == this.Data.ownerUserId);
		if (this.Data.chatMemberInviteId != null)
		{
			CommonDialog commonDialog = GUIMain.ShowCommonDialog(null, "CMD_ChatModal");
			((CMD_ChatModal)commonDialog).SetTitle(string.Format(StringMaster.GetString("ChatInviteReplyTitle"), this.Data.ownerInfo.nickname, this.Data.groupName));
			((CMD_ChatModal)commonDialog).SetBtn_CLOSE(true, StringMaster.GetString("SystemButtonClose"), null);
			((CMD_ChatModal)commonDialog).SetBtn_YES(true, StringMaster.GetString("ChatConfirmYes"), new Action(this.OnReplyToInviteJoin));
			((CMD_ChatModal)commonDialog).SetBtn_NO(true, StringMaster.GetString("ChatConfirmNo"), new Action(this.OnReplyToInviteNotJoin));
		}
		else if (this.Data.chatMemberRequestId != null)
		{
			CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnCancelMemberRequestExec), "CMD_Confirm") as CMD_Confirm;
			cmd_Confirm.Title = StringMaster.GetString("SystemConfirm");
			cmd_Confirm.Info = StringMaster.GetString("ChatApply-04");
		}
		else if (CMD_MultiRecruitChatList.Instance != null)
		{
			this.isSelected = !this.isSelected;
			if (this.isSelected)
			{
				this.spBgPlate.color = this.activeListColor;
			}
			else
			{
				this.spBgPlate.color = this.defaultListColor;
			}
			CMD_MultiRecruitChatList.Instance.CheckEnableBtnRecruit();
		}
		else
		{
			int num = 0;
			if (ClassSingleton<ChatData>.Instance.CurrentChatInfo.joinGroupData.groupList != null)
			{
				num = ClassSingleton<ChatData>.Instance.CurrentChatInfo.joinGroupData.groupList.Where((GameWebAPI.ResponseData_ChatGroupList.lists item) => item.chatGroupId == this.Data.chatGroupId).ToList<GameWebAPI.ResponseData_ChatGroupList.lists>().Count;
			}
			if (CMD_ChatTop.instance.searchFieldTypeSearch.activeSelf && num == 0)
			{
				int num2 = 0;
				if (ClassSingleton<ChatData>.Instance.CurrentChatInfo.requestGroupData.requestList != null)
				{
					num2 = ClassSingleton<ChatData>.Instance.CurrentChatInfo.requestGroupData.requestList.Where((GameWebAPI.ResponseData_ChatGroupList.lists item) => item.chatGroupId == this.Data.chatGroupId).ToList<GameWebAPI.ResponseData_ChatGroupList.lists>().Count;
				}
				int num3 = 0;
				if (ClassSingleton<ChatData>.Instance.CurrentChatInfo.inviteGroupData.inviteList != null)
				{
					num3 = ClassSingleton<ChatData>.Instance.CurrentChatInfo.inviteGroupData.inviteList.Where((GameWebAPI.ResponseData_ChatGroupList.lists item) => item.chatGroupId == this.Data.chatGroupId).ToList<GameWebAPI.ResponseData_ChatGroupList.lists>().Count;
				}
				if (num2 > 0 || num3 > 0)
				{
					CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
					cmd_ModalMessage.Title = StringMaster.GetString("SystemConfirm");
					cmd_ModalMessage.Info = StringMaster.GetString("ChatSearch-06");
				}
				else if (ClassSingleton<ChatData>.Instance.CurrentChatInfo.isMaxJoin)
				{
					CMD_ModalMessage cmd_ModalMessage2 = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
					cmd_ModalMessage2.Title = StringMaster.GetString("SystemConfirm");
					cmd_ModalMessage2.Info = StringMaster.GetString("ChatSearch-05");
				}
				else
				{
					CMD_Confirm cmd_Confirm2 = GUIMain.ShowCommonDialog(new Action<int>(this.OnChatApplyExec), "CMD_Confirm") as CMD_Confirm;
					if (int.Parse(this.Data.approvalType) == 2)
					{
						cmd_Confirm2.Title = StringMaster.GetString("SystemConfirm");
						cmd_Confirm2.Info = StringMaster.GetString("ChatSearch-04");
					}
					else
					{
						cmd_Confirm2.Title = StringMaster.GetString("SystemConfirm");
						cmd_Confirm2.Info = StringMaster.GetString("ChatSearch-02");
					}
				}
			}
			else
			{
				CMD_ChatTop.instance.openChatWindow();
				this.newIcon.SetActive(false);
			}
		}
	}

	public void ForceSelect(bool isSelect)
	{
		this.isSelected = isSelect;
		this.spBgPlate.color = ((!isSelect) ? this.defaultListColor : this.activeListColor);
	}
}
