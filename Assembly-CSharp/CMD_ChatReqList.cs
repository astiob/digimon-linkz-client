using DigiChat.Tools;
using Master;
using System;
using UnityEngine;

public sealed class CMD_ChatReqList : CMD
{
	public static CMD_ChatReqList instance;

	[SerializeField]
	private GameObject chatGroupDefaultText;

	[SerializeField]
	private GameObject partGroupParent;

	[SerializeField]
	private GameObject partGroupList;

	private UILabelEx ngChatGroupDefaultText;

	private GUISelectChatGroupPanel csPartGroupParent;

	private int reqReplyType { get; set; }

	protected override void Awake()
	{
		base.Awake();
		CMD_ChatReqList.instance = this;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.SetCommonUI();
		if (ClassSingleton<ChatData>.Instance.CurrentChatInfo.openListType == 1)
		{
			this.GetUserChatInviteListExec();
		}
		else
		{
			this.GetUserChatRequestListExec();
		}
		base.Show(f, sizeX, sizeY, aT);
		this.SetInitLabel();
	}

	private void SetInitLabel()
	{
		base.PartsTitle.SetTitle(StringMaster.GetString("ChatTitle"));
		this.ngChatGroupDefaultText = this.chatGroupDefaultText.GetComponent<UILabelEx>();
	}

	private void SetCommonUI()
	{
		this.partGroupList = this.partGroupList.transform.GetChild(0).gameObject;
		this.csPartGroupParent = this.partGroupParent.GetComponent<GUISelectChatGroupPanel>();
		this.csPartGroupParent.selectParts = this.partGroupList;
		this.csPartGroupParent.ListWindowViewRect = ConstValue.GetRectWindow3();
	}

	public void SetChatListUI(GameWebAPI.ResponseData_ChatGroupList data)
	{
		this.partGroupParent.SetActive(true);
		this.partGroupList.SetActive(true);
		this.csPartGroupParent.initLocation = true;
		this.csPartGroupParent.AllBuild(data);
		this.partGroupList.SetActive(false);
	}

	public void GetUserChatInviteListExec()
	{
		ChatTools.ChatLoadDisplay(true);
		GameWebAPI.UserChatInviteListLogic userChatInviteListLogic = new GameWebAPI.UserChatInviteListLogic();
		userChatInviteListLogic.OnReceived = delegate(GameWebAPI.RespData_UserChatInviteListLogic response)
		{
			ClassSingleton<ChatData>.Instance.CurrentChatInfo.inviteGroupData = response;
		};
		GameWebAPI.UserChatInviteListLogic request = userChatInviteListLogic;
		base.StartCoroutine(request.RunOneTime(new Action(this.AfterGetUserChatInviteList), delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void AfterGetUserChatInviteList()
	{
		ClassSingleton<ChatData>.Instance.UpdateMaxJoinState();
		RestrictionInput.EndLoad();
		this.InitUserChatInviteList();
	}

	private void InitUserChatInviteList()
	{
		this.SetChatListUI(ClassSingleton<ChatData>.Instance.CurrentChatInfo.inviteGroupData);
		if (ClassSingleton<ChatData>.Instance.CurrentChatInfo.inviteGroupData.inviteList != null)
		{
			this.chatGroupDefaultText.SetActive(false);
		}
		else
		{
			this.ngChatGroupDefaultText.SetStringKeyText("ChatNoneGroup");
			this.chatGroupDefaultText.SetActive(true);
		}
	}

	public void ReplyToInviteExec(int iid, int reply)
	{
		if (reply == 1 && ClassSingleton<ChatData>.Instance.CurrentChatInfo.isMaxJoin)
		{
			ChatTools.chatGroupMaxJoinDialog();
		}
		else
		{
			ChatTools.ChatLoadDisplay(true);
			this.reqReplyType = reply;
			GameWebAPI.RespData_ChatReplyToInviteLogic replyToInvite = null;
			GameWebAPI.ChatReplyToInviteLogic request = new GameWebAPI.ChatReplyToInviteLogic
			{
				SetSendData = delegate(GameWebAPI.ReqData_ChatReplyToInviteLogic param)
				{
					param.chatMemberInviteId = iid;
					param.reply = reply;
				},
				OnReceived = delegate(GameWebAPI.RespData_ChatReplyToInviteLogic response)
				{
					replyToInvite = response;
				}
			};
			base.StartCoroutine(request.RunOneTime(delegate()
			{
				RestrictionInput.EndLoad();
				this.AfterChatReplyToInvite(replyToInvite);
			}, delegate(Exception noop)
			{
				RestrictionInput.EndLoad();
			}, null));
		}
	}

	private void AfterChatReplyToInvite(GameWebAPI.RespData_ChatReplyToInviteLogic data)
	{
		if (ChatTools.CheckOnFLG(data.result))
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int i)
			{
				CMD_ChatModal.instance.ClosePanel(true);
				this.GetUserChatInviteListExec();
				if (this.reqReplyType == 1)
				{
					base.StartCoroutine(Singleton<TCPUtil>.Instance.SendSystemMessege(ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId, DataMng.Instance().UserId, DataMng.Instance().UserName));
				}
			}, "CMD_ModalMessage", null) as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("SystemConfirm");
			string format = (this.reqReplyType != 1) ? StringMaster.GetString("ChatInviteReplyNG") : StringMaster.GetString("ChatInviteReplyOK");
			cmd_ModalMessage.Info = string.Format(format, ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupName);
		}
	}

	public void GetUserChatRequestListExec()
	{
		ChatTools.ChatLoadDisplay(true);
		GameWebAPI.UserChatRequestListLogic userChatRequestListLogic = new GameWebAPI.UserChatRequestListLogic();
		userChatRequestListLogic.OnReceived = delegate(GameWebAPI.RespData_UserChatRequestList response)
		{
			ClassSingleton<ChatData>.Instance.CurrentChatInfo.requestGroupData = response;
		};
		GameWebAPI.UserChatRequestListLogic request = userChatRequestListLogic;
		base.StartCoroutine(request.RunOneTime(new Action(this.AfterUserChatRequestList), delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void AfterUserChatRequestList()
	{
		ClassSingleton<ChatData>.Instance.UpdateMaxJoinState();
		RestrictionInput.EndLoad();
		this.InitUserChatRequestList();
	}

	private void InitUserChatRequestList()
	{
		this.SetChatListUI(ClassSingleton<ChatData>.Instance.CurrentChatInfo.requestGroupData);
		if (ClassSingleton<ChatData>.Instance.CurrentChatInfo.requestGroupData.requestList != null)
		{
			this.chatGroupDefaultText.SetActive(false);
		}
		else
		{
			this.ngChatGroupDefaultText.SetStringKeyText("ChatNoneGroup");
			this.chatGroupDefaultText.SetActive(true);
		}
	}

	public void RequestCanselExec(int rid)
	{
		ChatTools.ChatLoadDisplay(true);
		GameWebAPI.RespData_ChatCancelMemberRequestLogic cancelMemberRequest = null;
		GameWebAPI.ChatCancelMemberRequestLogic request = new GameWebAPI.ChatCancelMemberRequestLogic
		{
			SetSendData = delegate(GameWebAPI.ReqData_ChatCancelMemberRequestLogic param)
			{
				param.chatMemberRequestId = rid;
			},
			OnReceived = delegate(GameWebAPI.RespData_ChatCancelMemberRequestLogic response)
			{
				cancelMemberRequest = response;
			}
		};
		base.StartCoroutine(request.RunOneTime(delegate()
		{
			RestrictionInput.EndLoad();
			this.AfterChatCancelMemberRequest(cancelMemberRequest);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void AfterChatCancelMemberRequest(GameWebAPI.RespData_ChatCancelMemberRequestLogic data)
	{
		if (ChatTools.CheckOnFLG(data.result))
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int i)
			{
				this.GetUserChatRequestListExec();
			}, "CMD_ModalMessage", null) as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("SystemConfirm");
			cmd_ModalMessage.Info = string.Format(StringMaster.GetString("ChatApply-05"), ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupName);
		}
	}

	public override void ClosePanel(bool animation = true)
	{
		this.csPartGroupParent.FadeOutAllListParts(null, false);
		CMD_ChatTop.instance.setMenuNewIcon();
		base.ClosePanel(animation);
	}
}
