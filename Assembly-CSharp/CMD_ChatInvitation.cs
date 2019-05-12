using DigiChat.Tools;
using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WebAPIRequest;

public class CMD_ChatInvitation : CMD
{
	public static CMD_ChatInvitation instance;

	[SerializeField]
	private GameObject chatFriendDefaultText;

	private UILabel ngChatFriendDefaultText;

	[SerializeField]
	private GameObject partFriendParent;

	[SerializeField]
	private GameObject partFriendList;

	private GUISelectChatMemberPanel csPartFriendParent;

	public List<GUIListPartBS> partFriendListObjs;

	[SerializeField]
	private GameObject goInviteDecisionBtn;

	[SerializeField]
	private GameObject goMemberNumBox;

	[SerializeField]
	private UILabel memberNumLabel;

	public int inviteSelectCnt;

	public int manyInviteSelectMaxNum;

	private List<GameWebAPI.FriendList> friendList;

	private GameWebAPI.RespData_ChatWholeGroupMemberList allMemberData;

	private GameWebAPI.ResponseData_ChatUserList userListData;

	private List<GameWebAPI.ResponseData_ChatUserList.respUserList> handoverListData;

	private List<int> inviteIdParamList = new List<int>();

	public string targetUserId { get; set; }

	protected override void Awake()
	{
		base.Awake();
		CMD_ChatInvitation.instance = this;
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.Init();
		int num = ChatConstValue.CHAT_GROUP_MEMBER_MAX_NUM - ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupMemberNum;
		if (num < 10)
		{
			this.manyInviteSelectMaxNum = num;
		}
		else
		{
			this.manyInviteSelectMaxNum = 10;
		}
		if (CMD_ChatMenu.instance.openMemberListType == 1)
		{
			ChatTools.ChatLoadDisplay(true);
			base.HideDLG();
			base.PartsTitle.SetTitle(StringMaster.GetString("ChatLogMenu-08"));
			this.ngChatFriendDefaultText = this.chatFriendDefaultText.GetComponent<UILabel>();
			this.ngChatFriendDefaultText.text = StringMaster.GetString("ChatInvitation-01");
			this.memberNumLabel.text = string.Format(StringMaster.GetString("SystemFraction"), 0, this.manyInviteSelectMaxNum);
			NGUITools.SetActiveSelf(this.goMemberNumBox.gameObject, true);
			NGUITools.SetActiveSelf(this.goInviteDecisionBtn.gameObject, true);
			base.StartCoroutine(this.InitFriendUI(f, sizeX, sizeY, aT));
		}
		else
		{
			this.ngChatFriendDefaultText = this.chatFriendDefaultText.GetComponent<UILabel>();
			this.ngChatFriendDefaultText.text = StringMaster.GetString("ChatUserNone");
			this.goInviteDecisionBtn.SetActive(false);
			this.SetCommonUI();
			if (CMD_ChatMenu.instance.openMemberListType == 4)
			{
				base.PartsTitle.SetTitle(StringMaster.GetString("ChatLogMenu-09"));
				this.SetGroupRequestUserList(ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId);
			}
			else if (CMD_ChatMenu.instance.openMemberListType == 5)
			{
				base.PartsTitle.SetTitle(StringMaster.GetString("ChatLogMenu-10"));
				this.SetGroupInviteUserList(ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId);
			}
			else if (CMD_ChatMenu.instance.openMemberListType == 2)
			{
				base.PartsTitle.SetTitle(StringMaster.GetString("ChatLogMenu-07"));
				this.SetChatMemberList(ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId);
			}
			else
			{
				base.PartsTitle.SetTitle(StringMaster.GetString("ChatLogMenu-03"));
				this.SetChatMemberList(ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId);
				this.goMemberNumBox.SetActive(true);
			}
			base.Show(f, sizeX, sizeY, aT);
			ChatTools.ChatLoadDisplay(false);
		}
	}

	private void Init()
	{
		NGUITools.SetActiveSelf(this.partFriendList.gameObject, false);
		NGUITools.SetActiveSelf(this.partFriendParent.gameObject, false);
		NGUITools.SetActiveSelf(this.goInviteDecisionBtn.gameObject, false);
		NGUITools.SetActiveSelf(this.goMemberNumBox.gameObject, false);
	}

	private void SetCommonUI()
	{
		this.csPartFriendParent = this.partFriendParent.GetComponent<GUISelectChatMemberPanel>();
		this.csPartFriendParent.selectParts = this.partFriendList;
		this.csPartFriendParent.ListWindowViewRect = ChatTools.MakeChatListMemberRectWindow();
	}

	private void SetCommonUserListUI()
	{
		this.chatFriendDefaultText.SetActive(false);
		this.partFriendParent.SetActive(true);
		this.partFriendList.SetActive(true);
		this.csPartFriendParent.initLocation = true;
		this.chatFriendDefaultText.SetActive(false);
		if (CMD_ChatMenu.instance.openMemberListType == 2)
		{
			this.csPartFriendParent.HandoverListBuild(this.handoverListData);
		}
		else
		{
			this.csPartFriendParent.AllBuild(this.userListData);
		}
		this.partFriendList.SetActive(false);
	}

	private IEnumerator InitFriendUI(Action<int> f, float sizeX, float sizeY, float aT)
	{
		APIRequestTask task = this.RequestChatWholeGroupMemberList(ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId);
		task.Add(APIUtil.Instance().RequestFriendData(false));
		yield return base.StartCoroutine(task.Run(delegate
		{
			RestrictionInput.EndLoad();
			this.ShowDLG();
			this.MakeInviteData();
			this.<Show>__BaseCallProxy0(f, sizeX, sizeY, aT);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
			this.<ClosePanel>__BaseCallProxy1(false);
		}, null));
		yield break;
	}

	private void MakeInviteData()
	{
		this.InitFriendData();
		this.SetCommonUI();
		this.InitFriendList();
	}

	private void InitFriendData()
	{
		this.friendList = new List<GameWebAPI.FriendList>();
		GameWebAPI.FriendList[] array = DataMng.Instance().RespDataFR_FriendList.friendList;
		GameWebAPI.FriendList[] array2 = array;
		int i = 0;
		while (i < array2.Length)
		{
			GameWebAPI.FriendList obj = array2[i];
			if (this.allMemberData.member.memberList == null)
			{
				goto IL_83;
			}
			int count = this.allMemberData.member.memberList.Where((GameWebAPI.RespData_ChatWholeGroupMemberList.respMember.respMemberList item) => item.userId == obj.userData.userId).ToList<GameWebAPI.RespData_ChatWholeGroupMemberList.respMember.respMemberList>().Count;
			if (count <= 0)
			{
				goto IL_83;
			}
			IL_131:
			i++;
			continue;
			IL_83:
			if (this.allMemberData.request.requestList != null)
			{
				count = this.allMemberData.request.requestList.Where((GameWebAPI.RespData_ChatWholeGroupMemberList.respRequest.respRequestList item) => item.userId == obj.userData.userId).ToList<GameWebAPI.RespData_ChatWholeGroupMemberList.respRequest.respRequestList>().Count;
				if (count > 0)
				{
					goto IL_131;
				}
			}
			if (this.allMemberData.invite.inviteList != null)
			{
				count = this.allMemberData.invite.inviteList.Where((GameWebAPI.RespData_ChatWholeGroupMemberList.respInvite.respInviteList item) => item.userId == obj.userData.userId).ToList<GameWebAPI.RespData_ChatWholeGroupMemberList.respInvite.respInviteList>().Count;
				if (count > 0)
				{
					goto IL_131;
				}
			}
			this.friendList.Add(obj);
			goto IL_131;
		}
		this.friendList.Sort(delegate(GameWebAPI.FriendList a, GameWebAPI.FriendList b)
		{
			if (a.userData.loginTimeSort < b.userData.loginTimeSort)
			{
				return -1;
			}
			if (a.userData.loginTimeSort > b.userData.loginTimeSort)
			{
				return 1;
			}
			return 0;
		});
	}

	public void SetInviteNumLabel(int num)
	{
		this.memberNumLabel.text = string.Format(StringMaster.GetString("SystemFraction"), num, this.manyInviteSelectMaxNum);
	}

	private void InitFriendList()
	{
		if (this.friendList.Count > 0)
		{
			this.chatFriendDefaultText.SetActive(false);
			this.partFriendParent.SetActive(true);
			this.partFriendList.SetActive(true);
			this.csPartFriendParent.initLocation = true;
			this.chatFriendDefaultText.SetActive(false);
			this.csPartFriendParent.AllFriendBuild(this.friendList);
			this.partFriendList.SetActive(false);
			this.goInviteDecisionBtn.SetActive(true);
		}
		else
		{
			this.chatFriendDefaultText.SetActive(true);
		}
	}

	public void ChatInviteExec(int cgid, int[] uids)
	{
		ChatTools.ChatLoadDisplay(true);
		GameWebAPI.RespData_ChatInviteMemberLogic inviteMember = null;
		GameWebAPI.ChatInviteMemberLogic request = new GameWebAPI.ChatInviteMemberLogic
		{
			SetSendData = delegate(GameWebAPI.ReqData_ChatInviteMemberLogic param)
			{
				param.chatGroupId = cgid;
				param.inviteUserIds = uids;
			},
			OnReceived = delegate(GameWebAPI.RespData_ChatInviteMemberLogic response)
			{
				inviteMember = response;
			}
		};
		base.StartCoroutine(request.RunOneTime(delegate()
		{
			this.AfterChatInviteMember(inviteMember);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void AfterChatInviteMember(GameWebAPI.RespData_ChatInviteMemberLogic data)
	{
		ChatTools.ChatLoadDisplay(false);
		string text = string.Empty;
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("SystemConfirm");
		if (data.inviteUserIds != null)
		{
			if (data.failureUserIds != null || data.inviteUserIds.Length > 1)
			{
				text = string.Format(StringMaster.GetString("ChatInvitation-06"), data.inviteUserIds.Length);
			}
			else
			{
				List<GameWebAPI.FriendList> list = new List<GameWebAPI.FriendList>();
				list = this.friendList.Where((GameWebAPI.FriendList item) => int.Parse(item.userData.userId) == data.inviteUserIds[0]).ToList<GameWebAPI.FriendList>();
				text = string.Format(StringMaster.GetString("ChatInvitation-05"), list[0].userData.nickname);
			}
		}
		if (data.failureUserIds != null)
		{
			if (data.inviteUserIds != null)
			{
				text += "\n";
			}
			if (data.inviteUserIds != null || data.failureUserIds.Length > 1)
			{
				text += string.Format(StringMaster.GetString("ChatInvitation-08"), data.failureUserIds.Length);
			}
			else
			{
				List<GameWebAPI.FriendList> list2 = new List<GameWebAPI.FriendList>();
				list2 = this.friendList.Where((GameWebAPI.FriendList item) => int.Parse(item.userData.userId) == data.failureUserIds[0]).ToList<GameWebAPI.FriendList>();
				text += string.Format(StringMaster.GetString("ChatInvitation-07"), list2[0].userData.nickname);
			}
		}
		cmd_ModalMessage.Info = text;
		base.ClosePanel(true);
	}

	public void SetChatMemberList(int cgid)
	{
		ChatTools.ChatLoadDisplay(true);
		GameWebAPI.RespData_ChatGroupMemberList groupMemberList = null;
		GameWebAPI.ChatGroupMemberList request = new GameWebAPI.ChatGroupMemberList
		{
			SetSendData = delegate(GameWebAPI.ReqData_ChatGroupMemberList param)
			{
				param.chatGroupId = cgid;
			},
			OnReceived = delegate(GameWebAPI.RespData_ChatGroupMemberList response)
			{
				groupMemberList = response;
			}
		};
		base.StartCoroutine(request.RunOneTime(delegate()
		{
			RestrictionInput.EndLoad();
			this.AfterGetChatGroupMemberList(groupMemberList);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void AfterGetChatGroupMemberList(GameWebAPI.RespData_ChatGroupMemberList data)
	{
		int num = (data.memberList == null) ? 0 : data.memberList.Length;
		this.memberNumLabel.text = string.Format(StringMaster.GetString("SystemFraction"), num, ChatConstValue.CHAT_GROUP_MEMBER_MAX_NUM);
		if (CMD_ChatMenu.instance.openMemberListType == 2)
		{
			this.InitHandoverData(data);
			this.InitHandoverList();
		}
		else
		{
			this.userListData = data;
			this.InitMemberList();
		}
	}

	private void InitMemberList()
	{
		this.SetCommonUserListUI();
		if (this.userListData.memberList != null)
		{
			this.chatFriendDefaultText.SetActive(false);
		}
		else
		{
			this.chatFriendDefaultText.SetActive(true);
		}
	}

	private void InitHandoverList()
	{
		this.SetCommonUserListUI();
		if (this.handoverListData.Count > 0)
		{
			this.chatFriendDefaultText.SetActive(false);
		}
		else
		{
			this.chatFriendDefaultText.SetActive(true);
		}
	}

	private void InitHandoverData(GameWebAPI.ResponseData_ChatUserList tmpList)
	{
		this.handoverListData = new List<GameWebAPI.ResponseData_ChatUserList.respUserList>();
		GameWebAPI.ResponseData_ChatUserList.respUserList[] memberList = tmpList.memberList;
		int i = 0;
		while (i < memberList.Length)
		{
			GameWebAPI.ResponseData_ChatUserList.respUserList obj = memberList[i];
			if (BlockManager.instance().blockList == null)
			{
				goto IL_6E;
			}
			int count = BlockManager.instance().blockList.Where((GameWebAPI.FriendList item) => item.userData.userId == obj.userId).ToList<GameWebAPI.FriendList>().Count;
			if (count <= 0)
			{
				goto IL_6E;
			}
			IL_80:
			i++;
			continue;
			IL_6E:
			this.handoverListData.Add(obj);
			goto IL_80;
		}
	}

	public void SetGroupRequestUserList(int cgid)
	{
		ChatTools.ChatLoadDisplay(true);
		GameWebAPI.ChatMemberRequestListLogic request = new GameWebAPI.ChatMemberRequestListLogic
		{
			SetSendData = delegate(GameWebAPI.ReqData_ChatMemberRequestList param)
			{
				param.chatGroupId = cgid;
			},
			OnReceived = delegate(GameWebAPI.RespData_ChatMemberRequestList response)
			{
				this.userListData = response;
			}
		};
		base.StartCoroutine(request.RunOneTime(delegate()
		{
			RestrictionInput.EndLoad();
			this.InitRequestMemberList();
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void InitRequestMemberList()
	{
		this.SetCommonUserListUI();
		if (this.userListData.requestList != null)
		{
			this.chatFriendDefaultText.SetActive(false);
		}
		else
		{
			this.chatFriendDefaultText.SetActive(true);
		}
	}

	public void SetGroupInviteUserList(int cgid)
	{
		ChatTools.ChatLoadDisplay(true);
		GameWebAPI.ChatMemberInviteList request = new GameWebAPI.ChatMemberInviteList
		{
			SetSendData = delegate(GameWebAPI.ReqData_ChatMemberInviteList param)
			{
				param.chatGroupId = cgid;
			},
			OnReceived = delegate(GameWebAPI.RespData_ChatMemberInviteList response)
			{
				this.userListData = response;
			}
		};
		base.StartCoroutine(request.RunOneTime(delegate()
		{
			RestrictionInput.EndLoad();
			this.InitInviteUserList();
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void InitInviteUserList()
	{
		this.SetCommonUserListUI();
		if (this.userListData.inviteList != null)
		{
			this.chatFriendDefaultText.SetActive(false);
		}
		else
		{
			this.chatFriendDefaultText.SetActive(true);
		}
	}

	public void ChatCancelMemberInviteExec(int cmiid)
	{
		ChatTools.ChatLoadDisplay(true);
		GameWebAPI.ChatCancelMemberInvite request = new GameWebAPI.ChatCancelMemberInvite
		{
			SetSendData = delegate(GameWebAPI.ReqData_ChatCancelMemberInvite param)
			{
				param.chatMemberInviteId = cmiid;
			}
		};
		base.StartCoroutine(request.RunOneTime(delegate()
		{
			RestrictionInput.EndLoad();
			this.AfterChatCancelMemberInvite();
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void AfterChatCancelMemberInvite()
	{
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int index)
		{
			this.SetGroupInviteUserList(ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId);
		}, "CMD_ModalMessage", null) as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("SystemConfirm");
		cmd_ModalMessage.Info = string.Format(StringMaster.GetString("ChatInviteCancel"), ClassSingleton<ChatData>.Instance.CurrentChatInfo.targetNickname);
	}

	private APIRequestTask RequestChatWholeGroupMemberList(int cgid)
	{
		GameWebAPI.ChatWholeGroupMemberList request = new GameWebAPI.ChatWholeGroupMemberList
		{
			SetSendData = delegate(GameWebAPI.ReqData_ChatWholeGroupMemberList param)
			{
				param.chatGroupId = cgid;
			},
			OnReceived = delegate(GameWebAPI.RespData_ChatWholeGroupMemberList response)
			{
				this.allMemberData = response;
			}
		};
		return new APIRequestTask(request, false);
	}

	public void SetChatHandoverGroupOwner(int cgid, int uid)
	{
		ChatTools.ChatLoadDisplay(true);
		GameWebAPI.ChatHandoverGroupOwnerLogic request = new GameWebAPI.ChatHandoverGroupOwnerLogic
		{
			SetSendData = delegate(GameWebAPI.ReqData_ChatHandoverGroupOwnerLogic param)
			{
				param.chatGroupId = cgid;
				param.toUserId = uid;
			}
		};
		base.StartCoroutine(request.RunOneTime(delegate()
		{
			RestrictionInput.EndLoad();
			this.AfterChatHandoverGroupOwner();
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void AfterChatHandoverGroupOwner()
	{
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int index)
		{
			this.SetChangeOwnerType();
			Singleton<TCPUtil>.Instance.SendSystemMessegeAlreadyConnected(ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId, "ChatLog-02", ClassSingleton<ChatData>.Instance.CurrentChatInfo.targetNickname, null);
			this.<ClosePanel>__BaseCallProxy1(true);
		}, "CMD_ModalMessage", null) as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("SystemConfirm");
		cmd_ModalMessage.Info = string.Format(StringMaster.GetString("ChatTransferSuccess"), ClassSingleton<ChatData>.Instance.CurrentChatInfo.targetNickname);
		CMD_ChatTop.instance.GetUserChatGroupListExec();
	}

	public void ReplyToRequestExec(int crmid, int reply)
	{
		ChatTools.ChatLoadDisplay(true);
		GameWebAPI.ChatReplyToRequestLogic request = new GameWebAPI.ChatReplyToRequestLogic
		{
			SetSendData = delegate(GameWebAPI.ReqData_ChatReplyToRequestLogic param)
			{
				param.chatMemberRequestId = crmid;
				param.reply = reply;
			}
		};
		base.StartCoroutine(request.RunOneTime(delegate()
		{
			RestrictionInput.EndLoad();
			this.AfterChatReplyToRequest(reply);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void AfterChatReplyToRequest(int reply)
	{
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int index)
		{
			CMD_ChatModal.instance.ClosePanel(false);
			this.SetGroupRequestUserList(ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId);
			if (reply == 1)
			{
				Singleton<TCPUtil>.Instance.SendSystemMessegeAlreadyConnected(ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId, "ChatLog-01", ClassSingleton<ChatData>.Instance.CurrentChatInfo.targetNickname, null);
			}
		}, "CMD_ModalMessage", null) as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("SystemConfirm");
		string format = string.Empty;
		if (reply == 1)
		{
			format = StringMaster.GetString("ChatApply-02");
		}
		else
		{
			format = StringMaster.GetString("ChatApply-03");
		}
		cmd_ModalMessage.Info = string.Format(format, ClassSingleton<ChatData>.Instance.CurrentChatInfo.targetNickname);
	}

	private void SetChangeOwnerType()
	{
		ClassSingleton<ChatData>.Instance.CurrentChatInfo.isMaster = false;
	}

	public List<int> AddInviteId(string uid)
	{
		this.inviteIdParamList.Add(int.Parse(uid));
		return this.inviteIdParamList;
	}

	public List<int> RemoveInviteId(string uid)
	{
		this.inviteIdParamList.Remove(int.Parse(uid));
		return this.inviteIdParamList;
	}

	public override void ClosePanel(bool animation = true)
	{
		this.inviteIdParamList.Clear();
		this.csPartFriendParent.FadeOutAllListParts(null, false);
		base.ClosePanel(animation);
	}

	public void PushedInviteDecision()
	{
		if (this.inviteIdParamList.Count > 0)
		{
			CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(delegate(int i)
			{
				if (i == 0)
				{
					this.ChatInviteExec(ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId, this.inviteIdParamList.ToArray());
				}
			}, "CMD_Confirm", null) as CMD_Confirm;
			cmd_Confirm.Title = StringMaster.GetString("ChatLogMenu-08");
			cmd_Confirm.Info = string.Format(StringMaster.GetString("ChatInvitation-04"), this.inviteIdParamList.Count.ToString());
		}
		else
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("SystemConfirm");
			cmd_ModalMessage.Info = StringMaster.GetString("ChatInvitation-02");
		}
	}

	public void PushedExpulsionDecision(GameWebAPI.ResponseData_ChatUserList.respUserList data)
	{
		base.StartCoroutine(Singleton<TCPUtil>.Instance.SendChatExpulsion(ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId, data.userId, delegate(int nop)
		{
			string mes = "ChatMemberKick";
			string nickname = data.userInfo.nickname;
			string @string = StringMaster.GetString("ChatMemberKickTitle");
			string string2 = StringMaster.GetString("ChatMemberKickText");
			this.SendChatMemberKick(mes, nickname, @string, string2);
		}));
	}

	private void SendChatMemberKick(string mes, string uname, string title, string info)
	{
		Singleton<TCPUtil>.Instance.SendSystemMessegeAlreadyConnected(ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId, mes, uname, delegate(int nop)
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int noop)
			{
				this.ClosePanel(true);
			}, "CMD_ModalMessage", null) as CMD_ModalMessage;
			cmd_ModalMessage.Title = title;
			cmd_ModalMessage.Info = info;
		});
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
}
