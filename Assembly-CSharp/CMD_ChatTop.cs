using DigiChat.Tools;
using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebAPIRequest;

public class CMD_ChatTop : CMD
{
	public static CMD_ChatTop instance;

	private int initFocusTabNum = 3;

	[SerializeField]
	private GameObject multiContainer;

	[SerializeField]
	private GameObject partMultiParent;

	[SerializeField]
	private GameObject partMultiList;

	[SerializeField]
	private UILabel lbMultiRequestUpdateBtn;

	[SerializeField]
	private GameObject goMultiUpdateBtn;

	[SerializeField]
	private GameObject goMultiUpdateActiveBtn;

	[SerializeField]
	private GameObject goMultiUpdateDisableBtn;

	[SerializeField]
	private BoxCollider goMultiUpdateBtnCollider;

	private GUISelectMultiRecruitListPanel csPartMultiParent;

	[SerializeField]
	private GameObject pvpContainer;

	[SerializeField]
	private GameObject partPvPParent;

	[SerializeField]
	private GameObject partPvPList;

	private GUISelectPvPListPanel csPartPvPParent;

	[SerializeField]
	private GameObject goListContainer;

	[SerializeField]
	private GameObject chatContainer;

	[SerializeField]
	public GameObject chatGroupDefaultText;

	[SerializeField]
	private GameObject partGroupParent;

	[SerializeField]
	private GameObject partGroupList;

	[SerializeField]
	private UILabel groupListNumberLabel;

	[SerializeField]
	private GameObject inviteAttentionIcon;

	[SerializeField]
	private GameObject requestAttentionIcon;

	[SerializeField]
	private UILabel viewChangeLabel;

	[SerializeField]
	private UILabel idSearchLabel;

	[SerializeField]
	private UILabel groupSearchButtonLabel;

	private UILabel ngChatGroupDefaultText;

	private GUISelectChatGroupPanel csPartGroupParent;

	[SerializeField]
	private GameObject menuWrap;

	[SerializeField]
	private UILabel createGroupLabel;

	[SerializeField]
	private UILabel nonApprovalLabel;

	[SerializeField]
	private UILabel approvalLabel;

	[SerializeField]
	private GameObject searchFieldTypeTop;

	[SerializeField]
	public GameObject searchFieldTypeSearch;

	public int recruiteChatGroupPage = 1;

	private int recruiteChatGroupPageMaxNum = 1;

	private bool isColosseumOpen;

	public static bool[] selectedRefineStatusList = new bool[]
	{
		true,
		true,
		true,
		true,
		true,
		true
	};

	public static CMD_ChatTop.SortStatusList selectedSortStatus = CMD_ChatTop.SortStatusList.MEMBER_DESC;

	private bool[] tabAlertList = new bool[4];

	public bool isGetChatGroupListMax { get; set; }

	public bool isChatPaging { get; set; }

	public bool isGetBlockList
	{
		get
		{
			return BlockManager.instance().blockList != null;
		}
	}

	public bool isRecruitListLock { get; set; }

	public bool[] TabAlertList
	{
		get
		{
			return this.tabAlertList;
		}
		set
		{
			this.tabAlertList = value;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		CMD_ChatTop.instance = this;
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.HideDLG();
		NormalTask normalTask = new NormalTask(this.PrepareConnect());
		normalTask.Add(this.RequestChatData());
		base.StartCoroutine(normalTask.Run(delegate
		{
			this.CheckColosseumOpenStatus();
			ClassSingleton<ChatData>.Instance.UpdateMaxJoinState();
			this.ShowDLG();
			this.SetTutorialAnyTime("anytime_second_tutorial_digichat");
			this.SetCommonUI();
			this.Show(f, sizeX, sizeY, aT);
			this.setTabView();
			this.setInitLabel();
			if (this.initFocusTabNum == 1)
			{
				this.openMultiRequest();
			}
			else if (this.initFocusTabNum == 2)
			{
				this.openPvPContent();
			}
			else if (this.initFocusTabNum == 3)
			{
				this.openChatContent();
			}
			else
			{
				this.openMenuContent();
			}
		}, delegate(Exception noop)
		{
			this.ClosePanel(false);
		}, null));
	}

	private IEnumerator PrepareConnect()
	{
		Singleton<TCPUtil>.Instance.PrepareTCPServer(new Action<string>(this.AfterPrepareTCPServer), "pvp");
		yield return null;
		yield break;
	}

	private void AfterPrepareTCPServer(string server)
	{
		Singleton<TCPUtil>.Instance.MakeTCPClient();
	}

	private APIRequestTask RequestChatData()
	{
		APIRequestTask apirequestTask = new APIRequestTask();
		if (BlockManager.instance().blockList == null)
		{
			apirequestTask.Add(BlockManager.instance().RequestBlockList(false));
		}
		RequestList requestList = new RequestList();
		GameWebAPI.UserChatGroupList userChatGroupList = new GameWebAPI.UserChatGroupList();
		userChatGroupList.OnReceived = delegate(GameWebAPI.RespData_UserChatGroupList response)
		{
			ClassSingleton<ChatData>.Instance.CurrentChatInfo.joinGroupData = response;
		};
		RequestBase addRequest = userChatGroupList;
		requestList.AddRequest(addRequest);
		GameWebAPI.UserChatInviteListLogic userChatInviteListLogic = new GameWebAPI.UserChatInviteListLogic();
		userChatInviteListLogic.OnReceived = delegate(GameWebAPI.RespData_UserChatInviteListLogic response)
		{
			ClassSingleton<ChatData>.Instance.CurrentChatInfo.inviteGroupData = response;
		};
		addRequest = userChatInviteListLogic;
		requestList.AddRequest(addRequest);
		GameWebAPI.UserChatRequestListLogic userChatRequestListLogic = new GameWebAPI.UserChatRequestListLogic();
		userChatRequestListLogic.OnReceived = delegate(GameWebAPI.RespData_UserChatRequestList response)
		{
			ClassSingleton<ChatData>.Instance.CurrentChatInfo.requestGroupData = response;
		};
		addRequest = userChatRequestListLogic;
		requestList.AddRequest(addRequest);
		apirequestTask.Add(new APIRequestTask(requestList, false));
		return apirequestTask;
	}

	private void setInitLabel()
	{
		base.PartsTitle.SetTitle(StringMaster.GetString("ChatTitle"));
		this.lbMultiRequestUpdateBtn.text = StringMaster.GetString("SystemUpdate");
		this.ngChatGroupDefaultText = this.chatGroupDefaultText.GetComponent<UILabel>();
		this.viewChangeLabel.text = StringMaster.GetString("SystemSortButton");
		this.idSearchLabel.text = StringMaster.GetString("ChatIdSearch-01");
		this.groupSearchButtonLabel.text = StringMaster.GetString("ChatSearchButton");
		this.createGroupLabel.text = StringMaster.GetString("ChatMenu-01");
		this.approvalLabel.text = StringMaster.GetString("ChatMenu-02");
		this.nonApprovalLabel.text = StringMaster.GetString("ChatMenu-03");
	}

	private void setTabView()
	{
		base.MultiTab.InitMultiTab(new List<Action<int>>
		{
			new Action<int>(this.OnTouchedTabMultiRequest),
			new Action<int>(this.OnTouchedTabPvP),
			new Action<int>(this.OnTouchedTabChat),
			new Action<int>(this.OnTouchedTabMenu)
		}, new List<string>
		{
			StringMaster.GetString("ChatTabMulti"),
			StringMaster.GetString("ChatTabColosseum"),
			StringMaster.GetString("ChatTabChat"),
			StringMaster.GetString("ChatTabMenu")
		});
		base.MultiTab.SetOnOffColor(ConstValue.TAB_YELLOW, Color.white);
		base.MultiTab.SetFocus(this.initFocusTabNum);
		this.DispAlertIcon(ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification.chatTabAlertList);
	}

	public void ForceSelectTab(CMD_ChatTop.TabType tabType)
	{
		this.initFocusTabNum = (int)tabType;
	}

	public void DispAlertIcon(params bool[] isActive)
	{
		base.MultiTab.SetActiveAlertIcon(isActive);
	}

	private void SetCommonUI()
	{
		this.partGroupList = this.partGroupList.transform.GetChild(0).gameObject;
		this.csPartGroupParent = this.partGroupParent.GetComponent<GUISelectChatGroupPanel>();
		this.csPartGroupParent.selectParts = this.partGroupList;
		this.csPartGroupParent.ListWindowViewRect = ChatTools.MakeChatListRectWindow();
		this.csPartMultiParent = this.partMultiParent.GetComponent<GUISelectMultiRecruitListPanel>();
		this.csPartMultiParent.selectParts = this.partMultiList;
		this.csPartMultiParent.ListWindowViewRect = ChatTools.MakeChatListRectWindow();
		this.csPartPvPParent = this.partPvPParent.GetComponent<GUISelectPvPListPanel>();
		this.csPartPvPParent.selectParts = this.partPvPList;
		this.csPartPvPParent.ListWindowViewRect = ChatTools.MakeChatListRectWindow();
	}

	public void SetChatListUI(GameWebAPI.ResponseData_ChatGroupList data)
	{
		this.partGroupParent.SetActive(true);
		this.partGroupList.SetActive(true);
		this.csPartGroupParent.initLocation = true;
		this.csPartGroupParent.AllBuild(data);
		this.partGroupList.SetActive(false);
	}

	private void GetMultiRecruitFriendList()
	{
		this.SetUpdateLock();
		GameWebAPI.MultiRoomRequestList multiRoomRequestList = new GameWebAPI.MultiRoomRequestList();
		multiRoomRequestList.SetSendData = delegate(GameWebAPI.ReqData_MultiRoomRequestList param)
		{
		};
		multiRoomRequestList.OnReceived = new Action<GameWebAPI.RespData_MultiRoomRequestList>(this.UpdateMultiRecruitFriendList);
		GameWebAPI.MultiRoomRequestList request = multiRoomRequestList;
		AppCoroutine.Start(request.RunOneTime(new Action(RestrictionInput.EndLoad), delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null), false);
	}

	private void GetPvPFriendList()
	{
		this.SetUpdateLock();
		GameWebAPI.ColosseumMockBattleRequestListLogic request = new GameWebAPI.ColosseumMockBattleRequestListLogic
		{
			OnReceived = new Action<GameWebAPI.RespData_ColosseumMockBattleRequestListLogic>(this.UpdatePvPFriendList)
		};
		AppCoroutine.Start(request.RunOneTime(new Action(RestrictionInput.EndLoad), delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null), false);
	}

	private void UpdateMultiRecruitFriendList(GameWebAPI.ResponseData_Common_MultiRoomList data)
	{
		this.partMultiParent.SetActive(true);
		this.partMultiList.SetActive(true);
		this.csPartMultiParent.initLocation = true;
		this.csPartMultiParent.AllBuild(data, null);
		this.partMultiList.SetActive(false);
		ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification.chatTabAlertList[0] = false;
		this.DispAlertIcon(ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification.chatTabAlertList);
		if (data.multiRoomList != null)
		{
			this.chatGroupDefaultText.SetActive(false);
		}
		else
		{
			this.ngChatGroupDefaultText.text = StringMaster.GetString("ChatMultiNone");
			this.chatGroupDefaultText.SetActive(true);
		}
		this.isRecruitListLock = false;
	}

	private void UpdatePvPFriendList(GameWebAPI.RespData_ColosseumMockBattleRequestListLogic data)
	{
		this.partPvPParent.SetActive(true);
		this.partPvPList.SetActive(true);
		this.csPartPvPParent.initLocation = true;
		this.csPartPvPParent.AllBuild(data);
		this.partPvPList.SetActive(false);
		ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification.chatTabAlertList[1] = false;
		this.DispAlertIcon(ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification.chatTabAlertList);
		if (data.memberList.Length > 0)
		{
			this.chatGroupDefaultText.SetActive(false);
		}
		else
		{
			this.ngChatGroupDefaultText.text = StringMaster.GetString("ChatColosseumNone");
			this.chatGroupDefaultText.SetActive(true);
		}
		this.isRecruitListLock = false;
	}

	public void OnClickMultiRequestUpdate()
	{
		this.isRecruitListLock = true;
		if (this.multiContainer.activeSelf)
		{
			this.GetMultiRecruitFriendList();
		}
		else if (this.pvpContainer.activeSelf)
		{
			this.GetPvPFriendList();
		}
	}

	private void SetUpdateLock()
	{
		if (this.goMultiUpdateActiveBtn.activeSelf)
		{
			this.goMultiUpdateActiveBtn.SetActive(false);
			this.goMultiUpdateDisableBtn.SetActive(true);
			this.lbMultiRequestUpdateBtn.color = Color.gray;
			this.goMultiUpdateBtnCollider.enabled = false;
			base.Invoke("ReleaseUpdateLock", 3f);
		}
	}

	private void ReleaseUpdateLock()
	{
		this.goMultiUpdateActiveBtn.SetActive(true);
		this.goMultiUpdateDisableBtn.SetActive(false);
		this.lbMultiRequestUpdateBtn.color = Color.white;
		this.goMultiUpdateBtnCollider.enabled = true;
	}

	public void GetUserChatGroupListExec()
	{
		ChatTools.ChatLoadDisplay(true);
		GameWebAPI.UserChatGroupList userChatGroupList = new GameWebAPI.UserChatGroupList();
		userChatGroupList.OnReceived = delegate(GameWebAPI.RespData_UserChatGroupList response)
		{
			ClassSingleton<ChatData>.Instance.CurrentChatInfo.joinGroupData = response;
		};
		GameWebAPI.UserChatGroupList request = userChatGroupList;
		AppCoroutine.Start(request.RunOneTime(delegate()
		{
			this.UpdateChatGroupList(ClassSingleton<ChatData>.Instance.CurrentChatInfo.joinGroupData);
			RestrictionInput.EndLoad();
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null), false);
	}

	public void SetChatRecruitGroupList(int page)
	{
		ChatTools.ChatLoadDisplay(true);
		List<int> categoryId = new List<int>();
		List<int> approvalType = new List<int>();
		this.recruiteChatGroupPage = page;
		for (int i = 0; i < CMD_ChatTop.selectedRefineStatusList.Length; i++)
		{
			if (CMD_ChatTop.selectedRefineStatusList[i])
			{
				if (i < 4)
				{
					categoryId.Add(i + 1);
				}
				else
				{
					approvalType.Add(i - 3);
				}
			}
		}
		GameWebAPI.RespData_ChatRecruitGroupListLogic groupList = null;
		GameWebAPI.ChatRecruitGroupListLogic request = new GameWebAPI.ChatRecruitGroupListLogic
		{
			SetSendData = delegate(GameWebAPI.ReqData_ChatRecruitGroupListLogic param)
			{
				param.categoryId = categoryId;
				param.approvalType = approvalType;
				param.sortType = (int)CMD_ChatTop.selectedSortStatus;
				param.page = this.recruiteChatGroupPage;
			},
			OnReceived = delegate(GameWebAPI.RespData_ChatRecruitGroupListLogic response)
			{
				groupList = response;
			}
		};
		base.StartCoroutine(request.RunOneTime(delegate()
		{
			this.AfterGetChatRecruitGroupList(groupList);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void AfterGetChatRecruitGroupList(GameWebAPI.RespData_ChatRecruitGroupListLogic data)
	{
		if (this.recruiteChatGroupPage == 1)
		{
			int num = data.groupNum / data.viewNum;
			int num2 = data.groupNum % data.viewNum;
			if (num == 0 || data.groupNum == data.viewNum)
			{
				num = 1;
				this.isGetChatGroupListMax = true;
			}
			else if (num2 > 0)
			{
				num++;
			}
			this.recruiteChatGroupPageMaxNum = num;
			this.UpdateChatGroupList(data);
		}
		else
		{
			if (this.recruiteChatGroupPage >= this.recruiteChatGroupPageMaxNum)
			{
				this.isGetChatGroupListMax = true;
			}
			else
			{
				this.isGetChatGroupListMax = false;
			}
			this.AddUpdateChatGroupList(data);
		}
		ChatTools.ChatLoadDisplay(false);
	}

	public void PagingChatGroupList()
	{
		this.SetChatRecruitGroupList(++this.recruiteChatGroupPage);
	}

	private void UpdateChatGroupList(GameWebAPI.ResponseData_ChatGroupList data)
	{
		this.SetChatListUI(data);
		if (data.groupList != null)
		{
			if (!this.isChatPaging)
			{
				this.groupListNumberLabel.text = string.Format(StringMaster.GetString("SystemFraction"), data.groupList.Length, ChatConstValue.CHAT_GROUP_JOIN_MAX_NUM);
			}
			this.chatGroupDefaultText.SetActive(false);
		}
		else
		{
			this.groupListNumberLabel.text = string.Format(StringMaster.GetString("SystemFraction"), 0, ChatConstValue.CHAT_GROUP_JOIN_MAX_NUM);
			this.ngChatGroupDefaultText.text = StringMaster.GetString("ChatNoneGroup");
			this.chatGroupDefaultText.SetActive(true);
		}
	}

	private void AddUpdateChatGroupList(GameWebAPI.ResponseData_ChatGroupList data)
	{
		this.csPartGroupParent.AddBuild(data);
		this.partGroupList.SetActive(false);
	}

	private void ClickSearchBtn()
	{
		this.searchFieldTypeTop.SetActive(false);
		this.searchFieldTypeSearch.SetActive(true);
		this.isGetChatGroupListMax = false;
		this.SetChatRecruitGroupList(1);
		this.isChatPaging = true;
	}

	private void ClickSearchBackBtn()
	{
		this.GetUserChatGroupListExec();
		this.searchFieldTypeTop.SetActive(true);
		this.searchFieldTypeSearch.SetActive(false);
		this.isChatPaging = false;
	}

	private void ClickSortBtn()
	{
		GUIMain.ShowCommonDialog(null, "CMD_ChatSort");
	}

	private void ClickIdSearchBtn()
	{
		GUIMain.ShowCommonDialog(null, "CMD_ChatSearch");
	}

	private void ClickInviteBackBtn()
	{
		this.openMenuContent();
	}

	public void chatRoomInRequest()
	{
		ChatTools.ChatLoadDisplay(true);
		GameWebAPI.RespData_ChatRequestMember requestMember = null;
		GameWebAPI.ChatRequestMember chatRequestMember = new GameWebAPI.ChatRequestMember();
		chatRequestMember.SetSendData = delegate(GameWebAPI.ReqData_ChatRequestMember param)
		{
			param.chatGroupId = ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId;
		};
		chatRequestMember.OnReceived = delegate(GameWebAPI.RespData_ChatRequestMember response)
		{
			requestMember = response;
		};
		GameWebAPI.ChatRequestMember request = chatRequestMember;
		APIRequestTask apirequestTask = new APIRequestTask(request, false);
		apirequestTask.Add(new NormalTask(() => this.RequestChatInfo(requestMember)));
		base.StartCoroutine(request.RunOneTime(delegate()
		{
			RestrictionInput.EndLoad();
			this.AfterChatRequestMember(requestMember);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private IEnumerator RequestChatInfo(GameWebAPI.RespData_ChatRequestMember data)
	{
		if (ChatTools.CheckOnFLG(data.result))
		{
			RequestBase request;
			if (data.approvalType == 1)
			{
				GameWebAPI.UserChatGroupList userChatGroupList = new GameWebAPI.UserChatGroupList();
				userChatGroupList.OnReceived = delegate(GameWebAPI.RespData_UserChatGroupList response)
				{
					ClassSingleton<ChatData>.Instance.CurrentChatInfo.joinGroupData = response;
				};
				request = userChatGroupList;
			}
			else
			{
				GameWebAPI.UserChatRequestListLogic userChatRequestListLogic = new GameWebAPI.UserChatRequestListLogic();
				userChatRequestListLogic.OnReceived = delegate(GameWebAPI.RespData_UserChatRequestList response)
				{
					ClassSingleton<ChatData>.Instance.CurrentChatInfo.requestGroupData = response;
				};
				request = userChatRequestListLogic;
			}
			return request.RunOneTime(new Action(ClassSingleton<ChatData>.Instance.UpdateMaxJoinState), null, null);
		}
		return null;
	}

	private void AfterChatRequestMember(GameWebAPI.RespData_ChatRequestMember data)
	{
		if (ChatTools.CheckOnFLG(data.result))
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int i)
			{
				this.ClickSearchBackBtn();
			}, "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("ChatConfirmTitle");
			cmd_ModalMessage.Info = StringMaster.GetString("ChatSearch-07");
			if (data.approvalType == 1)
			{
				this.UpdateJoinGroupData();
				this.SendJoinMessage();
			}
			else
			{
				this.UpdateRequestGroupData();
			}
		}
	}

	private void UpdateRequestGroupData()
	{
		GameWebAPI.UserChatRequestListLogic userChatRequestListLogic = new GameWebAPI.UserChatRequestListLogic();
		userChatRequestListLogic.OnReceived = delegate(GameWebAPI.RespData_UserChatRequestList response)
		{
			ClassSingleton<ChatData>.Instance.CurrentChatInfo.requestGroupData = response;
		};
		RequestBase request = userChatRequestListLogic;
		base.StartCoroutine(request.RunOneTime(delegate()
		{
			RestrictionInput.EndLoad();
			ClassSingleton<ChatData>.Instance.UpdateMaxJoinState();
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
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

	private void SendJoinMessage()
	{
		Singleton<TCPUtil>.Instance.SetTCPCallBackMethod(new Action<Dictionary<string, object>>(Singleton<TCPUtil>.Instance.GetTCPSystemReceponseData));
		Singleton<TCPUtil>.Instance.SetOnExitCallBackMethod(delegate
		{
			Singleton<TCPUtil>.Instance.SetTCPCallBackMethod(null);
			Singleton<TCPUtil>.Instance.SetOnExitCallBackMethod(null);
		});
		base.StartCoroutine(Singleton<TCPUtil>.Instance.SendSystemMessege(ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId, DataMng.Instance().UserId, DataMng.Instance().UserName));
	}

	public IEnumerator getBlockList(Action<int> action = null)
	{
		APIRequestTask task = BlockManager.instance().RequestBlockList(true);
		yield return base.StartCoroutine(task.Run(null, null, null));
		if (action != null)
		{
			action(0);
		}
		yield break;
	}

	private void openMultiRequest()
	{
		RestrictionInput.EndLoad();
		this.goListContainer.SetActive(true);
		this.multiContainer.SetActive(true);
		this.pvpContainer.SetActive(false);
		this.chatContainer.SetActive(false);
		this.menuWrap.SetActive(false);
		this.GetMultiRecruitFriendList();
		this.goMultiUpdateBtn.SetActive(true);
		this.searchFieldTypeTop.SetActive(false);
		this.searchFieldTypeSearch.SetActive(false);
	}

	private void openPvPContent()
	{
		this.csPartPvPParent.ReleaseBuild();
		this.chatGroupDefaultText.SetActive(false);
		this.goListContainer.SetActive(true);
		this.multiContainer.SetActive(false);
		this.chatContainer.SetActive(false);
		this.menuWrap.SetActive(false);
		if (this.isColosseumOpen)
		{
			this.pvpContainer.SetActive(true);
			this.GetPvPFriendList();
			this.goMultiUpdateBtn.SetActive(true);
		}
		else
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("ColosseumTitle");
			cmd_ModalMessage.Info = StringMaster.GetString("ColosseumLimit");
			this.goMultiUpdateBtn.SetActive(false);
			this.chatGroupDefaultText.SetActive(true);
			this.ngChatGroupDefaultText.text = StringMaster.GetString("ColosseumLimit");
		}
		this.searchFieldTypeTop.SetActive(false);
		this.searchFieldTypeSearch.SetActive(false);
	}

	private void openChatContent()
	{
		this.csPartGroupParent.ReleaseBuild();
		this.goListContainer.SetActive(true);
		this.multiContainer.SetActive(false);
		this.pvpContainer.SetActive(false);
		this.chatContainer.SetActive(true);
		this.menuWrap.SetActive(false);
		this.GetUserChatGroupListExec();
		this.searchFieldTypeTop.SetActive(true);
		this.searchFieldTypeSearch.SetActive(false);
		this.goMultiUpdateBtn.SetActive(false);
		this.isChatPaging = false;
	}

	private void openMenuContent()
	{
		this.setMenuNewIcon();
		this.goListContainer.SetActive(false);
		this.menuWrap.SetActive(true);
		this.searchFieldTypeTop.SetActive(false);
		this.searchFieldTypeSearch.SetActive(false);
		this.goMultiUpdateBtn.SetActive(false);
		this.isChatPaging = false;
	}

	public void openChatWindow()
	{
		if (this.isGetBlockList)
		{
			GUIMain.ShowCommonDialog(null, "CMD_ChatWindow");
		}
		else
		{
			base.StartCoroutine(this.getBlockList(delegate(int i)
			{
				GUIMain.ShowCommonDialog(null, "CMD_ChatWindow");
			}));
		}
	}

	public void setMenuNewIcon()
	{
		if (ClassSingleton<ChatData>.Instance.CurrentChatInfo.inviteGroupData.inviteList != null)
		{
			this.inviteAttentionIcon.SetActive(true);
		}
		else
		{
			this.inviteAttentionIcon.SetActive(false);
		}
		if (ClassSingleton<ChatData>.Instance.CurrentChatInfo.requestGroupData.requestList != null)
		{
			this.requestAttentionIcon.SetActive(true);
		}
		else
		{
			this.requestAttentionIcon.SetActive(false);
		}
	}

	private void OnTouchedTabMultiRequest(int i)
	{
		this.openMultiRequest();
	}

	private void OnTouchedTabPvP(int i)
	{
		this.openPvPContent();
	}

	private void OnTouchedTabChat(int i)
	{
		this.openChatContent();
	}

	private void OnTouchedTabMenu(int i)
	{
		this.openMenuContent();
	}

	public void PushedCreateGroupBtn()
	{
		if (ClassSingleton<ChatData>.Instance.CurrentChatInfo.isMaxJoin)
		{
			ChatTools.chatGroupMaxJoinDialog();
		}
		else
		{
			GUIMain.ShowCommonDialog(null, "CMD_CreateChatGroup");
		}
	}

	public void PushedRequestListBtn()
	{
		ClassSingleton<ChatData>.Instance.CurrentChatInfo.openListType = 1;
		GUIMain.ShowCommonDialog(null, "CMD_ChatReqList");
	}

	public void PushedRequestApplyingListBtn()
	{
		ClassSingleton<ChatData>.Instance.CurrentChatInfo.openListType = 2;
		GUIMain.ShowCommonDialog(null, "CMD_ChatReqList");
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		if (tutorialObserver != null)
		{
			GUIMain.BarrierON(null);
			tutorialObserver.StartSecondTutorial("second_tutorial_digichat", new Action(this.OnFinishTutorial), delegate
			{
				GUICollider.EnableAllCollider("CMD_ChatTOP");
			});
		}
		else
		{
			GUICollider.EnableAllCollider("CMD_ChatTOP");
		}
	}

	private void OnFinishTutorial()
	{
		if (PlayerPrefs.GetInt("ChatAgreement") <= 0)
		{
			CMD_AgreementChat cmd_AgreementChat = GUIMain.ShowCommonDialog(null, "CMD_AgreementChat") as CMD_AgreementChat;
			cmd_AgreementChat.SetActionAgreementPopupClosed(new Action<bool>(this.OnFinishAgreement));
		}
		else
		{
			this.OnFinishAgreement(true);
		}
	}

	private void CheckColosseumOpenStatus()
	{
		this.isColosseumOpen = DataMng.Instance().IsReleaseColosseum;
	}

	private void OnFinishAgreement(bool isAgree)
	{
		GUIMain.BarrierOFF();
		if (isAgree)
		{
			PlayerPrefs.SetInt("ChatAgreement", 1);
			PlayerPrefs.Save();
		}
		else
		{
			this.ClosePanel(false);
		}
	}

	public override void ClosePanel(bool animation = true)
	{
		this.csPartGroupParent.FadeOutAllListParts(null, false);
		if (this.menuWrap.activeSelf)
		{
			TweenPosition.Begin(this.menuWrap, 0.2f, new Vector3(-100f, 0f, 0f));
		}
		if (CMD_MultiRecruitPartyWait.Instance == null)
		{
			FarmCameraControlForCMD.On();
		}
		base.ClosePanel(animation);
	}

	public enum TabType
	{
		MULTI = 1,
		PVP,
		CHAT,
		MENU
	}

	public enum SortStatusList
	{
		MEMBER_DESC = 1,
		MEMBER_ASK
	}
}
