using DigiChat.Tools;
using Master;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using WebAPIRequest;

public class CMD_ChatWindow : CMD
{
	public static CMD_ChatWindow instance;

	[SerializeField]
	public GameObject goBaseTXT;

	[SerializeField]
	private GameObject chatGroupDefaultText;

	private UILabel ngChatGroupDefaultText;

	[SerializeField]
	private GameObject partMessageParent;

	[SerializeField]
	private GameObject partMessageList;

	private GUISelectChatLogPanel csPartMessageParent;

	[SerializeField]
	private GameObject menuAttentionIcon;

	[SerializeField]
	private BoxCollider chatInputCollider;

	[SerializeField]
	private UIInput chatCommentInput;

	[SerializeField]
	private UILabel chatPlaceholder;

	[SerializeField]
	private UILabel submitButtonLabel;

	[SerializeField]
	private GameObject submitButton;

	[SerializeField]
	private GameObject submitButtonGray;

	[SerializeField]
	private BoxCollider submitButtonCollider;

	[SerializeField]
	private UILabel fixedPhraseButtonLabel;

	[SerializeField]
	private GameObject fixedPhraseButton;

	[SerializeField]
	private GameObject fixedPhraseButtonGray;

	[SerializeField]
	private BoxCollider fixedPhraseButtonCollider;

	public int nowPartsCount;

	public string nowLastMessageId;

	private int retryConnectCnt;

	private Action prepareShowData;

	private string updateChatWindowTitle = string.Empty;

	public bool isGetChatLogListMax { get; set; }

	public bool isGetPastMessage { get; set; }

	public bool isNewRequestUser { get; set; }

	public bool isSendLock { get; set; }

	public bool isTCPSuccess { get; set; }

	public bool isTCPFinished { get; set; }

	protected override void Awake()
	{
		base.Awake();
		CMD_ChatWindow.instance = this;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		this.InitializeConnect(delegate
		{
			this.ShowDLG();
			this.SetGrayButton(true);
			this.setInitLabel();
			this.SetCommonUI();
			this.Show(f, sizeX, sizeY, aT);
		});
	}

	private void InitializeConnect(Action completed)
	{
		this.prepareShowData = completed;
		Singleton<TCPUtil>.Instance.PrepareTCPServer(new Action<string>(this.AfterPrepareTCPServer), "chat");
	}

	private void AfterPrepareTCPServer(string noop)
	{
		Singleton<TCPUtil>.Instance.MakeTCPClient();
		if (ClassSingleton<ChatData>.Instance.CurrentChatInfo.isMaster)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			GameWebAPI.RespData_ChatMemberRequestList memberRequestList = null;
			GameWebAPI.ChatMemberRequestListLogic chatMemberRequestListLogic = new GameWebAPI.ChatMemberRequestListLogic();
			chatMemberRequestListLogic.SetSendData = delegate(GameWebAPI.ReqData_ChatMemberRequestList param)
			{
				param.chatGroupId = ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId;
			};
			chatMemberRequestListLogic.OnReceived = delegate(GameWebAPI.RespData_ChatMemberRequestList response)
			{
				memberRequestList = response;
			};
			GameWebAPI.ChatMemberRequestListLogic request = chatMemberRequestListLogic;
			base.StartCoroutine(request.Run(delegate()
			{
				RestrictionInput.EndLoad();
				this.prepareShowData();
				this.AfterGetChatMemberRequestList(memberRequestList);
			}, null, null));
		}
		else
		{
			this.prepareShowData();
		}
	}

	private void AfterGetChatMemberRequestList(GameWebAPI.RespData_ChatMemberRequestList data)
	{
		if (data.requestList != null)
		{
			this.isNewRequestUser = true;
			this.SetMenuBtnAttention(true);
		}
		else
		{
			this.isNewRequestUser = false;
			this.SetMenuBtnAttention(false);
		}
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		Singleton<TCPUtil>.Instance.SetAfterConnectTCPMethod(new Action<bool>(this.AfterConnectTCP));
		Singleton<TCPUtil>.Instance.SetExceptionMethod(new Action<short, string>(this.CatchTcpExceptionAlert));
		Singleton<TCPUtil>.Instance.SetTCPCallBackMethod(new Action<Dictionary<string, object>>(this.GetTCPReceponseData));
		Singleton<TCPUtil>.Instance.ConnectTCPServerAsync(DataMng.Instance().UserId);
	}

	public string UpdateChatWindowTitle
	{
		get
		{
			return this.updateChatWindowTitle;
		}
		set
		{
			this.updateChatWindowTitle = value;
			base.PartsTitle.SetTitle(this.updateChatWindowTitle);
		}
	}

	private void setInitLabel()
	{
		this.UpdateChatWindowTitle = ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupName;
		this.ngChatGroupDefaultText = this.chatGroupDefaultText.GetComponent<UILabel>();
		this.ngChatGroupDefaultText.text = StringMaster.GetString("ChatLogNone");
		this.chatPlaceholder.text = StringMaster.GetString("SystemInput");
		this.submitButtonLabel.text = StringMaster.GetString("ChatLog-09");
		this.fixedPhraseButtonLabel.text = StringMaster.GetString("ChatLog-10");
	}

	private void SetCommonUI()
	{
		this.csPartMessageParent = this.partMessageParent.GetComponent<GUISelectChatLogPanel>();
		this.csPartMessageParent.selectParts = this.partMessageList;
		Rect listWindowViewRect = default(Rect);
		listWindowViewRect.xMin = -445f;
		listWindowViewRect.xMax = 445f;
		listWindowViewRect.yMin = -240f - GUIMain.VerticalSpaceSize;
		listWindowViewRect.yMax = 255f + GUIMain.VerticalSpaceSize;
		this.csPartMessageParent.ListWindowViewRect = listWindowViewRect;
	}

	private void SetGrayButton(bool isGray)
	{
		if (this.isSendLock)
		{
			isGray = true;
		}
		if (isGray)
		{
			this.submitButton.SetActive(false);
			this.submitButtonGray.SetActive(true);
		}
		else
		{
			this.submitButton.SetActive(true);
			this.submitButtonGray.SetActive(false);
		}
		this.submitButtonLabel.color = ((!isGray) ? Color.white : Color.gray);
		this.submitButtonCollider.enabled = !isGray;
	}

	public void CheckInputContent()
	{
		bool grayButton = false;
		if (string.IsNullOrEmpty(this.chatCommentInput.value) || Regex.IsMatch(this.chatCommentInput.value, "^\\s+$") || this.chatCommentInput.value.Length > 100)
		{
			grayButton = true;
		}
		this.chatCommentInput.value = this.chatCommentInput.value.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty);
		this.chatCommentInput.label.text = this.chatCommentInput.value;
		this.SetGrayButton(grayButton);
	}

	private void SetMenuBtnAttention(bool disp)
	{
		this.menuAttentionIcon.SetActive(disp);
	}

	private void AfterConnectTCP(bool result)
	{
		if (result)
		{
			this.setChatMessageHistory(true);
		}
		else
		{
			RestrictionInput.EndLoad();
			Action<int> action = delegate(int noop)
			{
				this.ClosePanel(true);
			};
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(action, "CMD_ModalMessage", null) as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("AlertNetworkErrorTitle");
			cmd_ModalMessage.Info = StringMaster.GetString("AlertNetworkErrorInfo");
		}
	}

	public void setChatMessageHistory(bool isNewMessage)
	{
		this.retryConnectCnt = 0;
		ChatTools.ChatLoadDisplay(true);
		this.isGetPastMessage = (false == isNewMessage);
		int messageId = 0;
		if (!isNewMessage)
		{
			messageId = int.Parse(this.nowLastMessageId);
		}
		GameWebAPI.RespData_ChatNewMessageHistoryLogic messageHistory = null;
		GameWebAPI.ChatNewMessageHistoryLogic request = new GameWebAPI.ChatNewMessageHistoryLogic
		{
			SetSendData = delegate(GameWebAPI.ReqData_ChatNewMessageHistoryLogic param)
			{
				param.chatGroupId = ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId;
				param.limit = ChatConstValue.CHATLOG_VIEW_LIST_INIT_NUM;
				param.borderMessageId = messageId;
			},
			OnReceived = delegate(GameWebAPI.RespData_ChatNewMessageHistoryLogic response)
			{
				messageHistory = response;
			}
		};
		base.StartCoroutine(request.RunOneTime(delegate()
		{
			RestrictionInput.EndLoad();
			this.AfterChatNewMessageHistory(messageHistory);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void AfterChatNewMessageHistory(GameWebAPI.RespData_ChatNewMessageHistoryLogic data)
	{
		if (this.isGetPastMessage)
		{
			if (data.result != null)
			{
				this.csPartMessageParent.PastListBuild(data);
			}
			else
			{
				this.isGetChatLogListMax = true;
			}
		}
		else if (data.result != null)
		{
			this.chatGroupDefaultText.SetActive(false);
			this.partMessageParent.SetActive(true);
			this.partMessageList.SetActive(true);
			this.csPartMessageParent.initMaxLocation = true;
			this.csPartMessageParent.AllBuild(data);
			this.partMessageList.SetActive(false);
		}
		else if (data.resultCode == 90)
		{
			this.UpdateJoinGroupData();
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int noop)
			{
				CMD_ChatWindow.instance.PushedChatReturnBtn();
				CMD_ChatTop.instance.GetUserChatGroupListExec();
			}, "CMD_ModalMessage", null) as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("SystemConfirm");
			cmd_ModalMessage.Info = StringMaster.GetString("ChatMemberKickNotice");
		}
		else
		{
			ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupLastHistoryId = "0";
			this.chatGroupDefaultText.SetActive(true);
		}
		this.isGetPastMessage = false;
	}

	public void chatDisconnect()
	{
		Singleton<TCPUtil>.Instance.TCPDisConnect(false);
	}

	public void PushedChatReturnBtn()
	{
		this.ClosePanel(true);
	}

	public void PushedSubmitBtn()
	{
		this.chatCommentInput.value = this.chatCommentInput.value.Trim();
		base.StartCoroutine(Singleton<TCPUtil>.Instance.SendChatMessage(ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId, this.chatCommentInput.value, ChatTools.SetMessageType(), null));
		this.SetSendLock();
		this.chatCommentInput.value = null;
		this.SetGrayButton(true);
	}

	public void SetSendLock()
	{
		this.fixedPhraseButton.SetActive(false);
		this.fixedPhraseButtonGray.SetActive(true);
		this.fixedPhraseButtonLabel.color = Color.gray;
		this.fixedPhraseButtonCollider.enabled = false;
		this.isSendLock = true;
		this.CheckInputContent();
		base.Invoke("ReleaseSendLock", 3f);
	}

	private void ReleaseSendLock()
	{
		this.fixedPhraseButton.SetActive(true);
		this.fixedPhraseButtonGray.SetActive(false);
		this.fixedPhraseButtonLabel.color = Color.white;
		this.fixedPhraseButtonCollider.enabled = true;
		this.isSendLock = false;
		this.CheckInputContent();
	}

	public void GetTCPReceponseData(Dictionary<string, object> data)
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		foreach (KeyValuePair<string, object> keyValuePair in data)
		{
			Dictionary<object, object> dictionary = (Dictionary<object, object>)keyValuePair.Value;
			GameWebAPI.Common_MessageData common_MessageData = new GameWebAPI.Common_MessageData();
			foreach (KeyValuePair<object, object> keyValuePair2 in dictionary)
			{
				if (keyValuePair2.Value != null)
				{
					string text = keyValuePair2.Key.ToString();
					switch (text)
					{
					case "errorCode":
						AlertManager.ShowAlertDialog(null, keyValuePair2.Value.ToString());
						continue;
					case "mhi":
						common_MessageData.chatMessageHistoryId = keyValuePair2.Value.ToString();
						continue;
					case "cgi":
						common_MessageData.chatGroupId = keyValuePair2.Value.ToString();
						continue;
					case "tp":
						common_MessageData.type = Convert.ToInt32(keyValuePair2.Value);
						continue;
					case "ui":
						common_MessageData.userId = keyValuePair2.Value.ToString();
						continue;
					case "msg":
						common_MessageData.message = keyValuePair2.Value.ToString();
						continue;
					case "ng":
						common_MessageData.ngwordFlg = Convert.ToInt32(keyValuePair2.Value);
						continue;
					case "ct":
						common_MessageData.createTime = keyValuePair2.Value.ToString();
						continue;
					case "uim":
					{
						Dictionary<object, object> dictionary2 = (Dictionary<object, object>)keyValuePair2.Value;
						common_MessageData.userInfo = new GameWebAPI.Common_MessageData.respUserInfo();
						foreach (KeyValuePair<object, object> keyValuePair3 in dictionary2)
						{
							if (keyValuePair3.Value != null)
							{
								if (keyValuePair3.Key.ToString() == "nn")
								{
									common_MessageData.userInfo.nickname = keyValuePair3.Value.ToString();
								}
								else if (keyValuePair3.Key.ToString() == "mi")
								{
									common_MessageData.userInfo.monsterId = keyValuePair3.Value.ToString();
								}
								else if (keyValuePair3.Key.ToString() == "ti")
								{
									common_MessageData.userInfo.titleId = keyValuePair3.Value.ToString();
								}
							}
						}
						continue;
					}
					case "chatGroupId":
						common_MessageData.chatGroupId = keyValuePair2.Value.ToString();
						continue;
					case "target":
						common_MessageData.target = keyValuePair2.Value.ToString();
						continue;
					case "resultCode":
						common_MessageData.resultCode = keyValuePair2.Value.ToString();
						continue;
					case "rc":
						common_MessageData.resultCode = keyValuePair2.Value.ToString();
						continue;
					}
					global::Debug.LogError(string.Concat(new object[]
					{
						"例外データ -> ",
						keyValuePair2.Key.ToString(),
						":",
						keyValuePair2.Value
					}));
				}
			}
			if (common_MessageData.resultCode != null && int.Parse(common_MessageData.resultCode) == 90)
			{
				this.UpdateJoinGroupData();
				CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int noop)
				{
					CMD_ChatWindow.instance.PushedChatReturnBtn();
					CMD_ChatTop.instance.GetUserChatGroupListExec();
				}, "CMD_ModalMessage", null) as CMD_ModalMessage;
				cmd_ModalMessage.Title = StringMaster.GetString("SystemConfirm");
				cmd_ModalMessage.Info = StringMaster.GetString("ChatMemberKickNotice");
				break;
			}
			if (common_MessageData.chatGroupId != null && ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId == int.Parse(common_MessageData.chatGroupId))
			{
				this.AfterGetChatMessageHistoryData(common_MessageData);
			}
		}
	}

	private void AfterGetChatMessageHistoryData(GameWebAPI.Common_MessageData data)
	{
		if (data.chatMessageHistoryId == null)
		{
			this.setChatMessageHistory(true);
		}
		else if (this.nowPartsCount <= 0)
		{
			this.setChatMessageHistory(true);
		}
		else
		{
			this.chatGroupDefaultText.SetActive(false);
			this.csPartMessageParent.LogPartsBuild(data);
		}
	}

	public void PushedConfigBtn()
	{
		if (ClassSingleton<ChatData>.Instance.CurrentChatInfo.isMaster)
		{
			GUIMain.ShowCommonDialog(null, "CMD_ChatMenu", null);
		}
		else
		{
			GUIMain.ShowCommonDialog(null, "CMD_ChatMenuM", null);
		}
	}

	public void PushedFixedPhraseBtn()
	{
		GUIMain.ShowCommonDialog(null, "CMD_ChatFixedPhrase", null);
	}

	public override void ClosePanel(bool animation = true)
	{
		ChatTools.UpdatePrefsHistoryIdList(ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId.ToString(), ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupLastHistoryId);
		ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification.StartGetHistoryIdListSingle();
		this.chatDisconnect();
		ChatTools.SetSystemMessageTCP();
		if (this.csPartMessageParent != null)
		{
			this.csPartMessageParent.FadeOutAllListParts(null, false);
		}
		base.ClosePanel(animation);
	}

	private void CatchTcpExceptionAlert(short errCode, string errMes)
	{
		global::Debug.Log(string.Concat(new object[]
		{
			"TCP Exception-> ",
			errCode,
			" : ",
			errMes
		}));
		if (CMDWebWindow.instance != null)
		{
			CMDWebWindow.instance.ClosePanel(true);
		}
		ChatTools.ChatLoadDisplay(false);
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int i)
		{
			if (this.retryConnectCnt < 2)
			{
				Singleton<TCPUtil>.Instance.TCPReConnection(0);
				this.retryConnectCnt++;
			}
			else
			{
				this.retryConnectCnt = 0;
				this.chatDisconnect();
				base.closeAll();
			}
		}, "CMD_ModalMessage", null) as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("AlertNetworkErrorTitle");
		cmd_ModalMessage.Info = StringMaster.GetString("ChatLogError");
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
