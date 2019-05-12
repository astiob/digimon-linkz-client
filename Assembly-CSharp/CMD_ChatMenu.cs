using DigiChat.Tools;
using Master;
using System;
using UnityEngine;
using WebAPIRequest;

public class CMD_ChatMenu : CMD
{
	public static CMD_ChatMenu instance;

	[SerializeField]
	private UILabel menuWindowTitle;

	[SerializeField]
	private UILabel groupMemberLabel;

	[SerializeField]
	private UILabel groupNameLabel;

	[SerializeField]
	private UILabel groupMemberMaxNumLabel;

	[SerializeField]
	private UILabel chatIdTextLabel;

	[SerializeField]
	private UILabel chatIdLabel;

	[SerializeField]
	private UILabel groupConfigBtnLabel;

	[SerializeField]
	private GameObject masterDelegateBtn;

	[SerializeField]
	private GameObject masterDelegateGrayBtn;

	[SerializeField]
	private BoxCollider masterDelegateCollider;

	[SerializeField]
	private UILabel masterDelegateBtnLabel;

	[SerializeField]
	private UILabel inviteBtnLabel;

	[SerializeField]
	private UILabel groupMemberListBtnLabel;

	[SerializeField]
	private GameObject groupRequestListBtnAttention;

	[SerializeField]
	private UILabel groupRequestListBtnLabel;

	[SerializeField]
	private UILabel groupInviteListBtnLabel;

	[SerializeField]
	private GameObject groupDeleteBtn;

	[SerializeField]
	private GameObject groupDeleteGrayBtn;

	[SerializeField]
	private BoxCollider groupDeleteCollider;

	[SerializeField]
	private UILabel groupDeleteBtnLabel;

	[SerializeField]
	private UILabel groupExitBtnLabel;

	[SerializeField]
	private UILabel chatServiceButtonLabel;

	public int openMemberListType;

	protected override void Awake()
	{
		base.Awake();
		CMD_ChatMenu.instance = this;
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.Show(f, sizeX, sizeY, aT);
		if (ClassSingleton<ChatData>.Instance.CurrentChatInfo.isMaster)
		{
			this.setMenuBtn();
			this.SetReqUserBtnAttention(CMD_ChatWindow.instance.isNewRequestUser);
		}
		this.setInitLabel();
	}

	private void SetReqUserBtnAttention(bool disp)
	{
		this.groupRequestListBtnAttention.SetActive(disp);
	}

	private void setInitLabel()
	{
		this.menuWindowTitle.text = StringMaster.GetString("ChatTabMenu");
		this.groupMemberLabel.text = StringMaster.GetString("ChatLogMenu-01");
		this.groupNameLabel.text = ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupName;
		this.groupMemberMaxNumLabel.text = string.Format(StringMaster.GetString("SystemFraction"), ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupMemberNum.ToString(), ChatConstValue.CHAT_GROUP_MEMBER_MAX_NUM);
		this.chatIdTextLabel.text = StringMaster.GetString("ChatLogMenu-02");
		this.chatIdLabel.text = ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId.ToString();
		this.groupMemberListBtnLabel.text = StringMaster.GetString("ChatLogMenu-03");
		if (ClassSingleton<ChatData>.Instance.CurrentChatInfo.isMaster)
		{
			this.groupConfigBtnLabel.text = StringMaster.GetString("ChatLogMenu-06");
			this.masterDelegateBtnLabel.text = StringMaster.GetString("ChatLogMenu-07");
			this.inviteBtnLabel.text = StringMaster.GetString("ChatLogMenu-08");
			this.groupRequestListBtnLabel.text = StringMaster.GetString("ChatLogMenu-09");
			this.groupInviteListBtnLabel.text = StringMaster.GetString("ChatLogMenu-10");
			this.groupDeleteBtnLabel.text = StringMaster.GetString("ChatLogMenu-11");
		}
		else
		{
			this.groupExitBtnLabel.text = StringMaster.GetString("ChatLogMenu-04");
		}
		this.chatServiceButtonLabel.text = StringMaster.GetString("AgreementTitle");
	}

	private void setMenuBtn()
	{
		if (ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupMemberNum <= 1)
		{
			this.groupDeleteBtn.SetActive(true);
			this.groupDeleteGrayBtn.SetActive(false);
			this.groupDeleteCollider.enabled = true;
			this.masterDelegateBtn.SetActive(false);
			this.masterDelegateGrayBtn.SetActive(true);
			this.masterDelegateCollider.enabled = false;
		}
		else
		{
			this.groupDeleteBtn.SetActive(false);
			this.groupDeleteGrayBtn.SetActive(true);
			this.groupDeleteCollider.enabled = false;
			this.masterDelegateBtn.SetActive(true);
			this.masterDelegateGrayBtn.SetActive(false);
			this.masterDelegateCollider.enabled = true;
		}
	}

	public void PushedGroupEditBtn()
	{
		CMD_CreateChatGroup cmd_CreateChatGroup = GUIMain.ShowCommonDialog(null, "CMD_CreateChatGroup") as CMD_CreateChatGroup;
		cmd_CreateChatGroup.IsConfigModify = true;
		base.ClosePanel(true);
	}

	public void PushedMasterDelegateBtn()
	{
		this.openMemberListType = 2;
		GUIMain.ShowCommonDialog(null, "CMD_ChatInvitation");
		base.ClosePanel(true);
	}

	public void PushedGroupInviteBtn()
	{
		this.openMemberListType = 1;
		GUIMain.ShowCommonDialog(null, "CMD_ChatInvitation");
		base.ClosePanel(true);
	}

	public void PushedGroupMemberListBtn()
	{
		this.openMemberListType = 3;
		GUIMain.ShowCommonDialog(null, "CMD_ChatInvitation");
		base.ClosePanel(true);
	}

	public void PushedGroupRequestListBtn()
	{
		this.openMemberListType = 4;
		GUIMain.ShowCommonDialog(null, "CMD_ChatInvitation");
		base.ClosePanel(true);
	}

	public void PushedGroupInviteListBtn()
	{
		this.openMemberListType = 5;
		GUIMain.ShowCommonDialog(null, "CMD_ChatInvitation");
		base.ClosePanel(true);
	}

	public void PushedGroupExitBtn()
	{
		CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnGroupExitExec), "CMD_Confirm") as CMD_Confirm;
		cmd_Confirm.Title = StringMaster.GetString("ChatLeavingConfirmTitle");
		cmd_Confirm.Info = StringMaster.GetString("ChatLeavingConfirmInfo");
	}

	private void OnGroupExitExec(int idx)
	{
		if (idx == 0)
		{
			ChatTools.ChatLoadDisplay(true);
			GameWebAPI.RespData_ChatResignGroupLogic resignGroup = null;
			GameWebAPI.ChatResignGroupLogic chatResignGroupLogic = new GameWebAPI.ChatResignGroupLogic();
			chatResignGroupLogic.SetSendData = delegate(GameWebAPI.ReqData_ChatResignGroupLogic param)
			{
				param.chatGroupId = ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId;
			};
			chatResignGroupLogic.OnReceived = delegate(GameWebAPI.RespData_ChatResignGroupLogic response)
			{
				resignGroup = response;
			};
			GameWebAPI.ChatResignGroupLogic request = chatResignGroupLogic;
			base.StartCoroutine(request.RunOneTime(delegate()
			{
				this.AfterChatResignGroup(resignGroup);
			}, delegate(Exception noop)
			{
				RestrictionInput.EndLoad();
			}, null));
		}
	}

	private void AfterChatResignGroup(GameWebAPI.RespData_ChatResignGroupLogic data)
	{
		if (ChatTools.CheckOnFLG(data.result))
		{
			this.SendChatResignGroup("ChatLog-03", StringMaster.GetString("ChatConfirmTitle"), StringMaster.GetString("ChatLeavingSuccess"));
		}
		else if (data.resultCode == 90)
		{
			this.SendChatResignGroup("ChatLog-03", StringMaster.GetString("SystemConfirm"), StringMaster.GetString("ChatMemberKickNotice"));
		}
		else
		{
			RestrictionInput.EndLoad();
		}
	}

	public void PushedGroupDeleteBtn()
	{
		CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnGroupDeleteExec), "CMD_Confirm") as CMD_Confirm;
		cmd_Confirm.Title = StringMaster.GetString("ChatDeleteConfirmTitle");
		cmd_Confirm.Info = StringMaster.GetString("ChatDeleteConfirmInfo");
	}

	private void OnGroupDeleteExec(int idx)
	{
		if (idx == 0)
		{
			ChatTools.ChatLoadDisplay(true);
			GameWebAPI.RespData_DeleteChatGroupLogic chatGroup = null;
			GameWebAPI.DeleteChatGroupLogic deleteChatGroupLogic = new GameWebAPI.DeleteChatGroupLogic();
			deleteChatGroupLogic.SetSendData = delegate(GameWebAPI.ReqData_DeleteChatGroupLogic param)
			{
				param.chatGroupId = ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId;
			};
			deleteChatGroupLogic.OnReceived = delegate(GameWebAPI.RespData_DeleteChatGroupLogic response)
			{
				chatGroup = response;
			};
			GameWebAPI.DeleteChatGroupLogic request = deleteChatGroupLogic;
			base.StartCoroutine(request.RunOneTime(delegate()
			{
				RestrictionInput.EndLoad();
				this.AfterDeleteChatGroup(chatGroup);
			}, delegate(Exception noop)
			{
				RestrictionInput.EndLoad();
			}, null));
		}
	}

	private void AfterDeleteChatGroup(GameWebAPI.RespData_DeleteChatGroupLogic data)
	{
		if (ChatTools.CheckOnFLG(data.result))
		{
			this.UpdateJoinGroupData();
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int i)
			{
				base.ClosePanel(false);
				CMD_ChatWindow.instance.ClosePanel(true);
				CMD_ChatTop.instance.GetUserChatGroupListExec();
			}, "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("SystemConfirm");
			cmd_ModalMessage.Info = StringMaster.GetString("ChatDeleteSuccess");
		}
	}

	private void UpdateJoinGroupData()
	{
		RequestList requestList = new RequestList();
		GameWebAPI.UserChatGroupList userChatGroupList = new GameWebAPI.UserChatGroupList();
		userChatGroupList.OnReceived = delegate(GameWebAPI.RespData_UserChatGroupList response)
		{
			ClassSingleton<ChatData>.Instance.CurrentChatInfo.joinGroupData = response;
		};
		RequestBase addRequest = userChatGroupList;
		requestList.AddRequest(addRequest);
		base.StartCoroutine(requestList.RunOneTime(delegate()
		{
			RestrictionInput.EndLoad();
			ClassSingleton<ChatData>.Instance.UpdateMaxJoinState();
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void PushedChatNoticeBtn()
	{
		Application.OpenURL(WebAddress.EXT_ADR_AGREE);
	}

	private void SendChatResignGroup(string mes, string title, string info)
	{
		Singleton<TCPUtil>.Instance.SendSystemMessegeAlreadyConnected(ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId, mes, DataMng.Instance().UserName, delegate(int nop)
		{
			RestrictionInput.EndLoad();
			this.UpdateJoinGroupData();
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int noop)
			{
				this.ClosePanel(true);
				CMD_ChatWindow.instance.PushedChatReturnBtn();
				CMD_ChatTop.instance.GetUserChatGroupListExec();
			}, "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = title;
			cmd_ModalMessage.Info = info;
		});
	}
}
