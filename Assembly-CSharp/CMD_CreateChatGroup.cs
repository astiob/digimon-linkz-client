using DigiChat.Tools;
using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebAPIRequest;

public class CMD_CreateChatGroup : CMD
{
	private const int buttonTypeCategory = 1;

	private const int buttonTypeApproval = 2;

	private int selectedCategoryType = 1;

	private int selectedApprovalType = 1;

	private bool groupNameInputStatus;

	private bool commentInputStatus = true;

	[SerializeField]
	private UIInput groupNameInput;

	[SerializeField]
	private UILabel groupNameLabel;

	[SerializeField]
	private UIInput commentInput;

	[SerializeField]
	private UILabel commentLabel;

	[SerializeField]
	private List<GameObject> goCategoryBtnList;

	private List<GUICollider> clCategoryBtnList;

	private List<UISprite> spCategoryBtnList;

	private List<UILabel> lbCategoryBtnList;

	[SerializeField]
	private List<GameObject> goApprovalBtnList;

	private List<GUICollider> clApprovalBtnList;

	private List<UISprite> spApprovalBtnList;

	private List<UILabel> lbApprovalBtnList;

	[SerializeField]
	private UILabel lbGroupName;

	[SerializeField]
	private UILabel lbCategory;

	[SerializeField]
	private UILabel lbApproval;

	[SerializeField]
	private UILabel lbComment;

	[SerializeField]
	private UILabel submitButtonLabel;

	[SerializeField]
	private GameObject submitButton;

	[SerializeField]
	private GameObject submitButtonGray;

	[SerializeField]
	private BoxCollider submitButtonCollider;

	private bool isConfigModify;

	public bool IsConfigModify
	{
		get
		{
			return this.isConfigModify;
		}
		set
		{
			this.isConfigModify = value;
			if (this.isConfigModify)
			{
				this.SetChatGroupInfo();
				this.submitButtonLabel.text = StringMaster.GetString("ChatGroupUpdateButton");
			}
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.SetupCategoryBtn();
		this.SetupApprovalBtn();
		this.SetupLabel();
		this.SetGraySubmitButton(false);
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.Show(f, sizeX, sizeY, aT);
		base.PartsTitle.SetTitle(StringMaster.GetString("ChatTitle"));
	}

	private void SetChatGroupInfo()
	{
		long[] searchResIds = new long[]
		{
			(long)ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId
		};
		GameWebAPI.RespData_ChatGroupInfo chatGroupInfo = null;
		GameWebAPI.ChatGroupInfo request = new GameWebAPI.ChatGroupInfo
		{
			SetSendData = delegate(GameWebAPI.ReqData_ChatGroupInfo param)
			{
				param.chatGroupId = searchResIds;
			},
			OnReceived = delegate(GameWebAPI.RespData_ChatGroupInfo response)
			{
				chatGroupInfo = response;
			}
		};
		base.StartCoroutine(request.RunOneTime(delegate()
		{
			RestrictionInput.EndLoad();
			this.AfterGetChatGroupInfo(chatGroupInfo);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void AfterGetChatGroupInfo(GameWebAPI.RespData_ChatGroupInfo data)
	{
		this.groupNameInputStatus = true;
		this.commentInputStatus = true;
		this.groupNameLabel.text = data.groupList[0].groupName;
		this.groupNameInput.value = data.groupList[0].groupName;
		this.commentLabel.text = data.groupList[0].comment;
		this.commentInput.value = data.groupList[0].comment;
		this.OnClickedCategoryBtn(int.Parse(data.groupList[0].categoryId));
		this.OnClickedApprovalBtn(int.Parse(data.groupList[0].approvalType));
		this.SetGraySubmitButton(true);
	}

	public override void ClosePanel(bool animation = true)
	{
		base.ClosePanel(animation);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	private void OnClickedCategoryBtn(int idx)
	{
		if (idx > 0)
		{
			this.selectedCategoryType = idx;
			this.SetupBtnColor(1, idx);
		}
	}

	private void OnClickedApprovalBtn(int idx)
	{
		if (idx > 0)
		{
			this.selectedApprovalType = idx;
			this.SetupBtnColor(2, idx);
		}
	}

	private void SetupCategoryBtn()
	{
		this.clCategoryBtnList = new List<GUICollider>();
		this.spCategoryBtnList = new List<UISprite>();
		this.lbCategoryBtnList = new List<UILabel>();
		for (int i = 0; i < this.goCategoryBtnList.Count; i++)
		{
			GUICollider component = this.goCategoryBtnList[i].GetComponent<GUICollider>();
			UISprite component2 = this.goCategoryBtnList[i].GetComponent<UISprite>();
			this.clCategoryBtnList.Add(component);
			this.spCategoryBtnList.Add(component2);
			IEnumerator enumerator = this.goCategoryBtnList[i].transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					UILabel component3 = transform.gameObject.GetComponent<UILabel>();
					switch (i + 1)
					{
					case 1:
						component3.text = StringMaster.GetString("ChatCategory-04");
						break;
					case 2:
						component3.text = StringMaster.GetString("ChatCategory-05");
						break;
					case 3:
						component3.text = StringMaster.GetString("ChatCategory-03");
						break;
					case 4:
						component3.text = StringMaster.GetString("ChatCategory-06");
						break;
					}
					this.lbCategoryBtnList.Add(component3);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			switch (i)
			{
			case 0:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedCategoryBtn(1);
				};
				break;
			case 1:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedCategoryBtn(2);
				};
				break;
			case 2:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedCategoryBtn(3);
				};
				break;
			case 3:
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedCategoryBtn(4);
				};
				break;
			}
		}
	}

	private void SetupApprovalBtn()
	{
		this.clApprovalBtnList = new List<GUICollider>();
		this.spApprovalBtnList = new List<UISprite>();
		this.lbApprovalBtnList = new List<UILabel>();
		for (int i = 0; i < this.goApprovalBtnList.Count; i++)
		{
			GUICollider component = this.goApprovalBtnList[i].GetComponent<GUICollider>();
			UISprite component2 = this.goApprovalBtnList[i].GetComponent<UISprite>();
			this.clApprovalBtnList.Add(component);
			this.spApprovalBtnList.Add(component2);
			IEnumerator enumerator = this.goApprovalBtnList[i].transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					UILabel component3 = transform.GetComponent<UILabel>();
					int num = i + 1;
					if (num != 1)
					{
						if (num == 2)
						{
							component3.text = StringMaster.GetString("ChatCategory-01");
						}
					}
					else
					{
						component3.text = StringMaster.GetString("ChatCategory-02");
					}
					this.lbApprovalBtnList.Add(component3);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			if (i != 0)
			{
				if (i == 1)
				{
					component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
					{
						this.OnClickedApprovalBtn(2);
					};
				}
			}
			else
			{
				component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					this.OnClickedApprovalBtn(1);
				};
			}
		}
	}

	private void SetupBtnColor(int type, int idx)
	{
		List<GameObject> list = new List<GameObject>();
		List<UISprite> list2 = new List<UISprite>();
		List<UILabel> list3 = new List<UILabel>();
		idx--;
		if (type == 1)
		{
			list = this.goCategoryBtnList;
			list2 = this.spCategoryBtnList;
			list3 = this.lbCategoryBtnList;
		}
		else
		{
			list = this.goApprovalBtnList;
			list2 = this.spApprovalBtnList;
			list3 = this.lbApprovalBtnList;
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (i == idx)
			{
				list2[i].spriteName = "Common02_Btn_SupportRed";
				list3[i].color = Color.white;
			}
			else
			{
				list2[i].spriteName = "Common02_Btn_SupportWhite";
				list3[i].color = new Color(0.227450982f, 0.227450982f, 0.227450982f, 1f);
			}
		}
	}

	private void SetupLabel()
	{
		this.lbGroupName.text = StringMaster.GetString("ChatGroup-01");
		this.lbCategory.text = StringMaster.GetString("ChatGroup-02");
		this.lbApproval.text = StringMaster.GetString("ChatGroup-03");
		this.lbComment.text = StringMaster.GetString("ChatGroup-04");
		this.submitButtonLabel.text = StringMaster.GetString("ChatGroupCreateButton");
		if (!this.isConfigModify)
		{
			this.commentInput.value = StringMaster.GetString("ChatGroup-05");
		}
	}

	public void CheckInputGroupName()
	{
		this.groupNameInputStatus = true;
		if (string.IsNullOrEmpty(this.groupNameLabel.text))
		{
			this.groupNameInputStatus = false;
		}
		this.groupNameInput.value = this.groupNameInput.value.Replace(" ", string.Empty).Replace("\u3000", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty);
		this.groupNameLabel.text = this.groupNameInput.value;
		this.groupNameInput.label.text = this.groupNameInput.value;
		this.SetGraySubmitButton(false);
	}

	public void CheckInputComment()
	{
		this.commentInputStatus = true;
		if (string.IsNullOrEmpty(this.commentLabel.text))
		{
			this.commentInputStatus = false;
		}
		this.commentInput.value = this.commentInput.value.Replace(" ", string.Empty).Replace("\u3000", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty);
		this.commentLabel.text = this.commentInput.value;
		this.commentInput.label.text = this.commentInput.value;
		this.SetGraySubmitButton(false);
	}

	private void SetGraySubmitButton(bool forceFalse = false)
	{
		bool flag = true;
		if (forceFalse || (this.groupNameInputStatus && this.commentInputStatus))
		{
			flag = false;
		}
		if (flag)
		{
			this.submitButton.SetActive(false);
			this.submitButtonGray.SetActive(true);
		}
		else
		{
			this.submitButton.SetActive(true);
			this.submitButtonGray.SetActive(false);
		}
		this.submitButtonCollider.enabled = !flag;
	}

	public void OnSubmit()
	{
		if (this.groupNameInput.value.Length > 15 || this.commentInput.value.Length > 30)
		{
			this.ShowDaialogOverTheLength();
		}
		else if (TextUtil.SurrogateCheck(this.groupNameInput.value) || TextUtil.SurrogateCheck(this.commentInput.value))
		{
			this.ShowDaialogForbiddenChar();
		}
		else
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			if (this.isConfigModify)
			{
				GameWebAPI.RespData_EditChatGroupLogic chatGroup = null;
				GameWebAPI.EditChatGroupLogic request = new GameWebAPI.EditChatGroupLogic
				{
					SetSendData = delegate(GameWebAPI.ReqData_EditChatGroupLogic param)
					{
						param.chatGroupId = ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId;
						param.categoryId = this.selectedCategoryType;
						param.groupName = this.groupNameInput.value;
						param.comment = this.commentInput.value;
						param.approvalType = this.selectedApprovalType;
					},
					OnReceived = delegate(GameWebAPI.RespData_EditChatGroupLogic response)
					{
						chatGroup = response;
					}
				};
				base.StartCoroutine(request.RunOneTime(delegate()
				{
					RestrictionInput.EndLoad();
					this.AfterEditChatGroup(chatGroup);
				}, delegate(Exception noop)
				{
					RestrictionInput.EndLoad();
				}, null));
			}
			else
			{
				RequestList requestList = new RequestList();
				RequestBase addRequest = new GameWebAPI.CreateChatGroupLogic
				{
					SetSendData = delegate(GameWebAPI.ReqData_CreateChatGroupLogic param)
					{
						param.categoryId = this.selectedCategoryType;
						param.groupName = this.groupNameInput.value;
						param.comment = this.commentInput.value;
						param.approvalType = this.selectedApprovalType;
					}
				};
				requestList.AddRequest(addRequest);
				GameWebAPI.UserChatGroupList userChatGroupList = new GameWebAPI.UserChatGroupList();
				userChatGroupList.OnReceived = delegate(GameWebAPI.RespData_UserChatGroupList response)
				{
					ClassSingleton<ChatData>.Instance.CurrentChatInfo.joinGroupData = response;
				};
				addRequest = userChatGroupList;
				requestList.AddRequest(addRequest);
				base.StartCoroutine(requestList.RunOneTime(delegate()
				{
					RestrictionInput.EndLoad();
					this.AfterCreateChatGroup();
				}, delegate(Exception noop)
				{
					RestrictionInput.EndLoad();
				}, null));
			}
		}
	}

	private void AfterCreateChatGroup()
	{
		ClassSingleton<ChatData>.Instance.UpdateMaxJoinState();
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int i)
		{
			this.ClosePanel(true);
		}, "CMD_ModalMessage", null) as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("ChatConfirmTitle");
		cmd_ModalMessage.Info = string.Format(StringMaster.GetString("ChatGroupCreateSuccess"), this.groupNameInput.value);
	}

	private void AfterEditChatGroup(GameWebAPI.RespData_EditChatGroupLogic data)
	{
		if (ChatTools.CheckOnFLG(data.result))
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int i)
			{
				this.<ClosePanel>__BaseCallProxy0(true);
				ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupName = this.groupNameInput.value;
				CMD_ChatWindow.instance.UpdateChatWindowTitle = ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupName;
				CMD_ChatTop.instance.GetUserChatGroupListExec();
			}, "CMD_ModalMessage", null) as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("ChatConfirmTitle");
			cmd_ModalMessage.Info = StringMaster.GetString("ChatGroupUpdateSuccess");
		}
	}

	private void ShowDaialogOverTheLength()
	{
		CMD_Alert cmd_Alert = GUIMain.ShowCommonDialog(null, "CMD_Alert", null) as CMD_Alert;
		if (cmd_Alert == null)
		{
			return;
		}
		cmd_Alert.Title = StringMaster.GetString("MyProfile-15");
		cmd_Alert.Info = StringMaster.GetString("ChatGroupCreateFailed");
		cmd_Alert.SetDisplayButton(CMD_Alert.DisplayButton.CLOSE);
		this.SetGraySubmitButton(true);
	}

	private void ShowDaialogForbiddenChar()
	{
		CMD_Alert cmd_Alert = GUIMain.ShowCommonDialog(null, "CMD_Alert", null) as CMD_Alert;
		if (cmd_Alert == null)
		{
			return;
		}
		cmd_Alert.Title = StringMaster.GetString("MyProfile-13");
		cmd_Alert.Info = StringMaster.GetString("MyProfile-14");
		cmd_Alert.SetDisplayButton(CMD_Alert.DisplayButton.CLOSE);
		this.groupNameInput.value = string.Empty;
		this.commentInput.value = string.Empty;
		this.SetGraySubmitButton(true);
	}
}
