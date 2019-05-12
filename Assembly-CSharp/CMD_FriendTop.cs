using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CMD_FriendTop : CMD
{
	private const int FRIEND_LIST_TAB = 1;

	private const int REQUEST_LIST_TAB = 2;

	private const int ACCEPT_LIST_TAB = 3;

	private const string BUTTON_LOCK_SPRITE = "Common02_Btn_BaseG";

	private const string BUTTON_POSITIVE_SPRITE = "Common02_Btn_BaseON1";

	private const string BUTTON_NEGATIVE_SPRITE = "Common02_Btn_BaseON2";

	private const string BUTTON_ON_SPRITE = "Common02_Btn_SupportRed";

	private const string BUTTON_CANCEL_SPRITE = "Common02_Btn_BaseOFF";

	public static CMD_FriendTop instance;

	[SerializeField]
	private UILabel ngTX_FR_NUM;

	[SerializeField]
	private GameObject goListParts;

	[SerializeField]
	private GameObject goPartsSearchButton;

	[SerializeField]
	private GUISelectPanelFriend csSelectPanel;

	[SerializeField]
	private UILabel ngTX_EMPTY_SHOW;

	[SerializeField]
	private GUICollider coSelectModeButton;

	[SerializeField]
	private GUICollider coSelectButton1;

	[SerializeField]
	private GUICollider coSelectButton2;

	[SerializeField]
	private UILabel ngSelectModeButton;

	[SerializeField]
	private UILabel ngSelectButton1;

	[SerializeField]
	private UILabel ngSelectButton2;

	[SerializeField]
	private UISprite spSelectModeButton;

	[SerializeField]
	private UISprite spSelectButton1;

	[SerializeField]
	private UISprite spSelectButton2;

	private List<GameWebAPI.FriendList> friendList;

	private List<GameWebAPI.FriendList> friendReqList;

	private List<GameWebAPI.FriendList> friendUnAproveList;

	private float listCreateAfterWaitTime;

	public string selectUserlastLoginTime;

	private GameWebAPI.RespDataFR_FriendSearchUserCode friendSearchResult;

	public static Action onWindowOpened;

	private CMD_FriendTop.SELECT_MODE currentSelectMode;

	private Dictionary<GameWebAPI.FriendList, GUIListPartsFriend> selectFriendDict = new Dictionary<GameWebAPI.FriendList, GUIListPartsFriend>();

	private int currentListNum;

	private GameWebAPI.FriendList selectFriendData;

	public CMD_ProfileFriend.OperationType ProfOpeType { get; set; }

	protected override void Awake()
	{
		this.ngTX_EMPTY_SHOW.text = string.Empty;
		this.ngTX_EMPTY_SHOW.gameObject.SetActive(false);
		this.ngSelectModeButton.text = StringMaster.GetString("Friend-05");
		base.Awake();
		CMD_FriendTop.instance = this;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.SetTutorialAnyTime("anytime_second_tutorial_friend");
		base.HideDLG();
		this.listCreateAfterWaitTime = aT;
		AppCoroutine.Start(this.InitFriendUI(f, sizeX, sizeY, aT), false);
	}

	private IEnumerator InitFriendUI(Action<int> f, float sizeX, float sizeY, float aT)
	{
		APIRequestTask task = APIUtil.Instance().RequestFriendData(false);
		task.Add(BlockManager.instance().RequestBlockList(false));
		yield return AppCoroutine.Start(task.Run(delegate
		{
			base.ShowDLG();
			this.InitData();
			this.SetCommonUI();
			this.InitTab();
			this.ShowFriendCount();
			base.Show(f, sizeX, sizeY, aT);
		}, delegate(Exception noop)
		{
			base.ClosePanel(false);
		}, null), false);
		RestrictionInput.EndLoad();
		yield break;
	}

	private IEnumerator ReloadFriendUI()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		APIRequestTask task = APIUtil.Instance().RequestFriendData(false);
		task.Add(BlockManager.instance().RequestBlockList(false));
		yield return AppCoroutine.Start(task.Run(delegate
		{
			this.InitData();
			this.InitList(base.MultiTab.GetCurentTabIdx(), this.listCreateAfterWaitTime);
			this.ShowFriendCount();
		}, null, null), false);
		RestrictionInput.EndLoad();
		GUIMain.BarrierOFF();
		yield break;
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
		if (CMD_FriendTop.onWindowOpened != null)
		{
			CMD_FriendTop.onWindowOpened();
			CMD_FriendTop.onWindowOpened = null;
		}
	}

	private void CloseAndFarmCamOn(bool animation)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	protected override void Update()
	{
		this.UpdateEmptyShow();
		base.Update();
	}

	public override void ClosePanel(bool animation = true)
	{
		FarmRoot.Instance.HideFriendFarm(delegate
		{
			this.CloseAndFarmCamOn(animation);
			if (this.csSelectPanel != null && animation)
			{
				this.csSelectPanel.FadeOutAllListParts(null, false);
				this.csSelectPanel.SetHideScrollBarAllWays(true);
			}
			CMD_PartsFriendIDSearch.DeleteInputId();
		});
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		CMD_FriendTop.instance = null;
	}

	private void OnClickedSearch()
	{
		global::Debug.Log("============================================= FRIEND TOP SEARCH TAPPED!!");
		GUIMain.ShowCommonDialog(new Action<int>(this.OnClosedSearch), "CMD_PartsFriendIDSearch");
	}

	private void OnClosedSearch(int idx)
	{
		if (idx == 0)
		{
			int searchUserCode;
			if (!int.TryParse(CMD_PartsFriendIDSearch.InputValue, out searchUserCode))
			{
				this.OpenAlert();
				return;
			}
			this.ExecFriendSearch(searchUserCode);
		}
	}

	private void ExecFriendSearch(int searchUserCode)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		GameWebAPI.RequestFR_FriendSearchUserCode request = new GameWebAPI.RequestFR_FriendSearchUserCode
		{
			SetSendData = delegate(GameWebAPI.FR_Req_FriendSearchUserCode param)
			{
				param.userCode = searchUserCode;
			},
			OnReceived = delegate(GameWebAPI.RespDataFR_FriendSearchUserCode response)
			{
				this.friendSearchResult = response;
			}
		};
		AppCoroutine.Start(request.RunOneTime(new Action(this.EndSearch), delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null), false);
	}

	private void EndSearch()
	{
		if (this.friendSearchResult.friendList == null || 0 >= this.friendSearchResult.friendList.Length)
		{
			this.OpenAlert();
		}
		else
		{
			this.selectFriendData = this.friendSearchResult.friendList[0];
			if (BlockManager.instance().CheckBlock(this.friendSearchResult.friendList[0].userData.userId))
			{
				CMD_PartsFriendCheckScreen.Data = this.selectFriendData;
				CMD_PartsFriendCheckScreen.Exp = StringMaster.GetString("FriendSearch-05");
				GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseSearchAndOpenProfile), "CMD_PartsFriendCheckScreen");
			}
			else
			{
				CMD_PartsFriendCheckScreen.Data = this.selectFriendData;
				CMD_PartsFriendCheckScreen.Exp = StringMaster.GetString("FriendSearch-04");
				GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseSearchAndReq), "CMD_PartsFriendCheckScreen");
			}
		}
		RestrictionInput.EndLoad();
	}

	private void OpenAlert()
	{
		AlertManager.ShowAlertDialog(null, "C-US02");
	}

	private void OnCloseSearchAndReq(int selectButtonIndex)
	{
		if (selectButtonIndex == 0)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
			APIRequestTask task = APIUtil.Instance().RequestFriendApplication(this.selectFriendData.userData.userId, false);
			AppCoroutine.Start(task.Run(new Action(this.EndFriendRequest), delegate(Exception noop)
			{
				RestrictionInput.EndLoad();
			}, null), false);
		}
	}

	private void EndFriendRequest()
	{
		int curentTabIdx = base.MultiTab.GetCurentTabIdx();
		if (this.selectFriendData != null)
		{
			bool flag = true;
			for (int i = 0; i < this.friendReqList.Count; i++)
			{
				if (this.friendReqList[i].userData.userId == this.selectFriendData.userData.userId)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				this.friendReqList.Add(this.selectFriendData);
			}
		}
		if (curentTabIdx == 2)
		{
			this.InitList(2, 0f);
		}
		CMD_PartsFriendIDSearch.DeleteInputId();
		RestrictionInput.EndLoad();
	}

	private void OnCloseSearchAndOpenProfile(int idx)
	{
		if (idx == 0)
		{
			this.OpenFriendProfile();
		}
	}

	private void InitData()
	{
		this.friendList = new List<GameWebAPI.FriendList>();
		this.friendReqList = new List<GameWebAPI.FriendList>();
		this.friendUnAproveList = new List<GameWebAPI.FriendList>();
		GameWebAPI.FriendList[] array = DataMng.Instance().RespDataFR_FriendList.friendList;
		for (int i = 0; i < array.Length; i++)
		{
			this.friendList.Add(array[i]);
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
		array = DataMng.Instance().RespDataFR_FriendRequestList.friendList;
		for (int i = 0; i < array.Length; i++)
		{
			this.friendReqList.Add(array[i]);
		}
		this.friendReqList.Sort(delegate(GameWebAPI.FriendList a, GameWebAPI.FriendList b)
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
		array = DataMng.Instance().RespDataFR_FriendUnapprovedList.friendList;
		for (int i = 0; i < array.Length; i++)
		{
			this.friendUnAproveList.Add(array[i]);
		}
		this.friendUnAproveList.Sort(delegate(GameWebAPI.FriendList a, GameWebAPI.FriendList b)
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

	private void SetCommonUI()
	{
		this.csSelectPanel.selectParts = this.goListParts;
		Rect listWindowViewRect = default(Rect);
		listWindowViewRect.xMin = -568f;
		listWindowViewRect.xMax = 568f;
		listWindowViewRect.yMin = -238f - GUIMain.VerticalSpaceSize;
		listWindowViewRect.yMax = 260f + GUIMain.VerticalSpaceSize;
		this.csSelectPanel.ListWindowViewRect = listWindowViewRect;
		this.goListParts.transform.localPosition = new Vector3(0f, 1000f, 0f);
	}

	private void InitTab()
	{
		if (base.MultiTab != null)
		{
			List<Action<int>> actions = new List<Action<int>>
			{
				new Action<int>(this.TabCallBack),
				new Action<int>(this.TabCallBack),
				new Action<int>(this.TabCallBack)
			};
			List<string> tabLabelTexts = new List<string>
			{
				StringMaster.GetString("FriendTitle"),
				StringMaster.GetString("Friend-02"),
				StringMaster.GetString("Friend-03")
			};
			base.MultiTab.InitMultiTab(actions, tabLabelTexts);
			base.MultiTab.SetOnOffColor(ConstValue.TAB_YELLOW, Color.white);
			base.MultiTab.OnTabTouchEnded(1, true);
			global::Debug.Log("=====================================_TAB_INIT_853");
		}
	}

	private void TabCallBack(int idx)
	{
		global::Debug.Log("=====================================_TAB_CALLBACK_853 IDX = " + idx.ToString());
		this.ReturnFromSelectMode();
		this.InitList(idx, 0f);
	}

	private void InitList(int idx, float afterWaitTime = 0f)
	{
		List<GameWebAPI.FriendList> list = null;
		string text = string.Empty;
		switch (idx)
		{
		case 1:
			base.PartsTitle.SetTitle(StringMaster.GetString("FriendTitle"));
			list = this.friendList;
			text = StringMaster.GetString("Friend-01");
			this.goPartsSearchButton.SetActive(true);
			break;
		case 2:
			base.PartsTitle.SetTitle(StringMaster.GetString("FriendTitle") + StringMaster.GetString("Friend-02"));
			list = this.friendReqList;
			text = StringMaster.GetString("FriendApply-02");
			this.goPartsSearchButton.SetActive(false);
			break;
		case 3:
			base.PartsTitle.SetTitle(StringMaster.GetString("FriendTitle") + StringMaster.GetString("Friend-03"));
			list = this.friendUnAproveList;
			text = StringMaster.GetString("FriendApproval-02");
			this.goPartsSearchButton.SetActive(false);
			break;
		}
		this.currentListNum = list.Count;
		if (this.currentListNum == 0)
		{
			this.ngTX_EMPTY_SHOW.text = text;
			this.spSelectModeButton.gameObject.SetActive(false);
		}
		else
		{
			this.ngTX_EMPTY_SHOW.text = string.Empty;
			this.spSelectModeButton.gameObject.SetActive(true);
			this.coSelectModeButton.GetComponent<Collider>().enabled = true;
			this.spSelectModeButton.spriteName = "Common02_Btn_SupportRed";
		}
		this.csSelectPanel.initLocation = true;
		this.csSelectPanel.StartFadeEfcCT = 0;
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
		AppCoroutine.Start(this.csSelectPanel.AllBuild(list, 0f, afterWaitTime, delegate
		{
			RestrictionInput.EndLoad();
		}), false);
		ClassSingleton<PartsMenuFriendIconAccessor>.Instance.partsMenuFriendIcon.FrinedListCheck();
		PartsMenu.SetMenuButtonAlertBadge();
		base.MultiTab.SetActiveAlertIcon(new bool[]
		{
			DataMng.Instance().RespDataMP_MyPage.userNewsCountList.newFriend > 0,
			default(bool),
			DataMng.Instance().RespDataMP_MyPage.userNewsCountList.friendApplication > 0
		});
		if (DataMng.Instance().RespDataMP_MyPage.userNewsCountList.newFriend > 0)
		{
			DataMng.Instance().RespDataMP_MyPage.userNewsCountList.newFriend = 0;
		}
	}

	private void UpdateEmptyShow()
	{
		if (base.GetActionStatus() == CommonDialog.ACT_STATUS.OPEN)
		{
			this.ngTX_EMPTY_SHOW.gameObject.SetActive(true);
		}
		else
		{
			this.ngTX_EMPTY_SHOW.gameObject.SetActive(false);
		}
	}

	private void ShowFriendCount()
	{
		GameWebAPI.RespDataFR_FriendInfo respDataFR_FriendInfo = DataMng.Instance().RespDataFR_FriendInfo;
		this.ngTX_FR_NUM.text = string.Format(StringMaster.GetString("SystemFraction"), respDataFR_FriendInfo.friendInfo.friendCount, respDataFR_FriendInfo.friendInfo.friendMaxCount.ToString());
	}

	public void ListPartsOperate(GUIListPartsFriend csParts, int idx)
	{
		this.selectFriendData = ((!(csParts != null)) ? null : csParts.Data);
		int curentTabIdx = base.MultiTab.GetCurentTabIdx();
		CMD_FriendTop.SELECT_MODE select_MODE = this.currentSelectMode;
		if (select_MODE != CMD_FriendTop.SELECT_MODE.NONE)
		{
			this.ListPartsSelectOperate(csParts, curentTabIdx);
		}
		else
		{
			this.ListPartsMenuOperate(curentTabIdx);
		}
	}

	private void ListPartsMenuOperate(int curSel)
	{
		switch (curSel)
		{
		case 1:
		{
			CommonDialog commonDialog = GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseFrend), "CMD_PartsFriendMENU1");
			commonDialog.gameObject.transform.FindChild("PartsBtn00/TXT_BTN").GetComponent<UILabel>().text = StringMaster.GetString("Friend-16");
			commonDialog.gameObject.transform.FindChild("PartsBtn01/TXT_BTN").GetComponent<UILabel>().text = StringMaster.GetString("Friend-15");
			commonDialog.gameObject.transform.FindChild("PartsBtn02/TXT_BTN").GetComponent<UILabel>().text = StringMaster.GetString("SystemButtonClose");
			commonDialog.gameObject.transform.FindChild("PartsBtn03/TXT_BTN").GetComponent<UILabel>().text = StringMaster.GetString("Friend-14");
			break;
		}
		case 2:
		{
			CommonDialog commonDialog2 = GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseFrendReq), "CMD_PartsFriendMENU2");
			commonDialog2.gameObject.transform.FindChild("PartsBtn00/TXT_BTN").GetComponent<UILabel>().text = StringMaster.GetString("Friend-16");
			commonDialog2.gameObject.transform.FindChild("PartsBtn01/TXT_BTN").GetComponent<UILabel>().text = StringMaster.GetString("Friend-17");
			commonDialog2.gameObject.transform.FindChild("PartsBtn02/TXT_BTN").GetComponent<UILabel>().text = StringMaster.GetString("SystemButtonClose");
			commonDialog2.gameObject.transform.FindChild("PartsBtn03/TXT_BTN").GetComponent<UILabel>().text = StringMaster.GetString("Friend-14");
			break;
		}
		case 3:
		{
			CommonDialog commonDialog3 = GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseFrendUnAprv), "CMD_PartsFriendMENU3");
			commonDialog3.gameObject.transform.FindChild("PartsBtn00/TXT_BTN").GetComponent<UILabel>().text = StringMaster.GetString("Friend-16");
			commonDialog3.gameObject.transform.FindChild("PartsBtn01/TXT_BTN").GetComponent<UILabel>().text = StringMaster.GetString("FriendApproval-04");
			commonDialog3.gameObject.transform.FindChild("PartsBtn02/TXT_BTN").GetComponent<UILabel>().text = StringMaster.GetString("FriendApproval-03");
			commonDialog3.gameObject.transform.FindChild("PartsBtn03/TXT_BTN").GetComponent<UILabel>().text = StringMaster.GetString("SystemButtonClose");
			commonDialog3.gameObject.transform.FindChild("PartsBtn04/TXT_BTN").GetComponent<UILabel>().text = StringMaster.GetString("Friend-14");
			bool flag = BlockManager.instance().CheckBlock(this.selectFriendData.userData.userId);
			if (flag)
			{
				Transform[] array = new Transform[]
				{
					commonDialog3.gameObject.transform.FindChild("PartsBtn00"),
					commonDialog3.gameObject.transform.FindChild("PartsBtn01")
				};
				foreach (Transform transform in array)
				{
					BoxCollider component = transform.GetComponent<BoxCollider>();
					component.enabled = false;
					UIWidget[] componentsInChildren = transform.GetComponentsInChildren<UIWidget>();
					foreach (UIWidget uiwidget in componentsInChildren)
					{
						uiwidget.color = Color.gray;
					}
				}
			}
			break;
		}
		}
	}

	private void ListPartsSelectOperate(GUIListPartsFriend csParts, int curSel)
	{
		if (this.selectFriendDict.ContainsKey(this.selectFriendData))
		{
			this.selectFriendDict.Remove(this.selectFriendData);
			csParts.ChangeSelectItem(false);
			if (this.selectFriendDict.Count == 0)
			{
				switch (this.currentSelectMode)
				{
				case CMD_FriendTop.SELECT_MODE.FRIEND_RELEASE:
					this.spSelectButton1.spriteName = "Common02_Btn_BaseG";
					this.coSelectButton1.GetComponent<Collider>().enabled = false;
					break;
				case CMD_FriendTop.SELECT_MODE.REQ_RELEASE:
					this.spSelectButton1.spriteName = "Common02_Btn_BaseG";
					this.coSelectButton1.GetComponent<Collider>().enabled = false;
					break;
				case CMD_FriendTop.SELECT_MODE.ACCEPT_REJECT:
					this.spSelectButton1.spriteName = "Common02_Btn_BaseG";
					this.spSelectButton2.spriteName = "Common02_Btn_BaseG";
					this.coSelectButton1.GetComponent<Collider>().enabled = false;
					this.coSelectButton2.GetComponent<Collider>().enabled = false;
					break;
				}
			}
			else if (this.selectFriendDict.Count == 9)
			{
				List<GUIListPartBS> partObjs = this.csSelectPanel.partObjs;
				foreach (GUIListPartBS guilistPartBS in partObjs)
				{
					GUIListPartsFriend guilistPartsFriend = guilistPartBS as GUIListPartsFriend;
					if (!this.selectFriendDict.ContainsValue(guilistPartsFriend))
					{
						guilistPartsFriend.ChangeUnselectItem(false);
					}
				}
			}
		}
		else
		{
			this.selectFriendDict.Add(this.selectFriendData, csParts);
			csParts.ChangeSelectItem(true);
			if (this.selectFriendDict.Count == 1)
			{
				switch (this.currentSelectMode)
				{
				case CMD_FriendTop.SELECT_MODE.FRIEND_RELEASE:
					this.spSelectButton1.spriteName = "Common02_Btn_BaseON2";
					this.coSelectButton1.GetComponent<Collider>().enabled = true;
					break;
				case CMD_FriendTop.SELECT_MODE.REQ_RELEASE:
					this.spSelectButton1.spriteName = "Common02_Btn_BaseON2";
					this.coSelectButton1.GetComponent<Collider>().enabled = true;
					break;
				case CMD_FriendTop.SELECT_MODE.ACCEPT_REJECT:
					this.spSelectButton1.spriteName = "Common02_Btn_BaseON1";
					this.spSelectButton2.spriteName = "Common02_Btn_BaseON2";
					this.coSelectButton1.GetComponent<Collider>().enabled = true;
					this.coSelectButton2.GetComponent<Collider>().enabled = true;
					break;
				}
			}
			else if (this.selectFriendDict.Count == 10)
			{
				List<GUIListPartBS> partObjs2 = this.csSelectPanel.partObjs;
				foreach (GUIListPartBS guilistPartBS2 in partObjs2)
				{
					GUIListPartsFriend guilistPartsFriend2 = guilistPartBS2 as GUIListPartsFriend;
					if (!this.selectFriendDict.ContainsValue(guilistPartsFriend2))
					{
						guilistPartsFriend2.ChangeUnselectItem(true);
					}
				}
			}
		}
	}

	public void MenuPartsOperate(GUIListPartsFriend csParts, int idx)
	{
		this.selectFriendData = ((!(csParts != null)) ? null : csParts.Data);
		this.OpenFriendProfile();
	}

	private void OnCloseFrend(int idx)
	{
		if (idx == 0)
		{
			this.ShowFriendFarm(this.selectFriendData, null);
		}
		else if (idx == 1)
		{
			CMD_PartsFriendCheckScreen.Data = this.selectFriendData;
			CMD_PartsFriendCheckScreen.Exp = StringMaster.GetString("Friend-11");
			GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseFriendBreakConfirm), "CMD_PartsFriendCheckScreen");
		}
		else if (idx == 3)
		{
			this.OpenFriendProfile();
		}
	}

	private void OnCloseFriendBreakConfirm(int selectButtonIndex)
	{
		if (selectButtonIndex == 0)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
			string[] friendUserIds;
			Action onSuccess;
			if (this.selectFriendDict != null && this.selectFriendDict.Count > 0)
			{
				friendUserIds = this.GetSelectFriendIdList();
				onSuccess = new Action(this.CorrespondFriendBreakEnd);
			}
			else
			{
				friendUserIds = new string[]
				{
					this.selectFriendData.userData.userId
				};
				onSuccess = new Action(this.EndFriendBreak);
			}
			APIRequestTask apirequestTask = APIUtil.Instance().RequestFriendBreak(friendUserIds, true);
			apirequestTask.SetAfterBehavior(TaskBase.AfterAlertClosed.RETURN);
			AppCoroutine.Start(apirequestTask.Run(onSuccess, new Action<Exception>(this.OnCommonFailedRecieve), new Func<Exception, IEnumerator>(this.OnCommonAlertedRecieve)), false);
		}
	}

	private void EndFriendBreak()
	{
		this.OnCommonRecieve();
	}

	private void OnCloseFrendReq(int idx)
	{
		if (idx == 0)
		{
			this.ShowFriendFarm(this.selectFriendData, null);
		}
		else if (idx == 1)
		{
			CMD_PartsFriendCheckScreen.Data = this.selectFriendData;
			CMD_PartsFriendCheckScreen.Exp = StringMaster.GetString("FriendApply-07");
			GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseFriendReqCancelConfirm), "CMD_PartsFriendCheckScreen");
		}
		else if (idx == 3)
		{
			this.OpenFriendProfile();
		}
	}

	private void OnCloseFriendReqCancelConfirm(int selectButtonIndex)
	{
		if (selectButtonIndex == 0)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
			string[] friendUserIds;
			Action onSuccess;
			if (this.selectFriendDict != null && this.selectFriendDict.Count > 0)
			{
				friendUserIds = this.GetSelectFriendIdList();
				onSuccess = new Action(this.CorrespondApplyReleaseEnd);
			}
			else
			{
				friendUserIds = new string[]
				{
					this.selectFriendData.userData.userId
				};
				onSuccess = new Action(this.EndFriendReqCancel);
			}
			APIRequestTask apirequestTask = APIUtil.Instance().RequestFriendApplicationCancel(friendUserIds, true);
			apirequestTask.SetAfterBehavior(TaskBase.AfterAlertClosed.RETURN);
			AppCoroutine.Start(apirequestTask.Run(onSuccess, new Action<Exception>(this.OnCommonFailedRecieve), new Func<Exception, IEnumerator>(this.OnCommonAlertedRecieve)), false);
		}
	}

	private void EndFriendReqCancel()
	{
		this.OnCommonRecieve();
	}

	private void OnCloseFrendUnAprv(int idx)
	{
		if (idx == 0)
		{
			this.ShowFriendFarm(this.selectFriendData, null);
		}
		else if (idx == 1)
		{
			CMD_PartsFriendCheckScreen.Data = this.selectFriendData;
			CMD_PartsFriendCheckScreen.Exp = StringMaster.GetString("FriendApproval-14");
			GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseFriendApproveConfirm), "CMD_PartsFriendCheckScreen");
		}
		else if (idx == 2)
		{
			CMD_PartsFriendCheckScreen.Data = this.selectFriendData;
			CMD_PartsFriendCheckScreen.Exp = StringMaster.GetString("FriendApproval-09");
			GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseFriendRejectConfirm), "CMD_PartsFriendCheckScreen");
		}
		else if (idx == 4)
		{
			this.OpenFriendProfile();
		}
	}

	private void OnCloseFriendApproveConfirm(int selectButtonIndex)
	{
		if (selectButtonIndex == 0)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
			string[] friendUserIds;
			Action onSuccess;
			if (this.selectFriendDict != null && this.selectFriendDict.Count > 0)
			{
				friendUserIds = this.GetSelectFriendIdList();
				onSuccess = new Action(this.CorrespondAcceptEnd);
			}
			else
			{
				friendUserIds = new string[]
				{
					this.selectFriendData.userData.userId
				};
				onSuccess = new Action(this.EndFriendApprove);
			}
			APIRequestTask apirequestTask = APIUtil.Instance().RequestFriendApplicationDecision(friendUserIds, GameWebAPI.FR_Req_FriendDecision.DecisionType.APPROVE, false);
			apirequestTask.SetAfterBehavior(TaskBase.AfterAlertClosed.RETURN);
			AppCoroutine.Start(apirequestTask.Run(onSuccess, new Action<Exception>(this.OnCommonFailedRecieve), new Func<Exception, IEnumerator>(this.OnCommonAlertedRecieve)), false);
		}
	}

	private void OnCloseFriendRejectConfirm(int selectButtonIndex)
	{
		if (selectButtonIndex == 0)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
			string[] friendUserIds;
			Action onSuccess;
			if (this.selectFriendDict != null && this.selectFriendDict.Count > 0)
			{
				friendUserIds = this.GetSelectFriendIdList();
				onSuccess = new Action(this.CorrespondRejectEnd);
			}
			else
			{
				friendUserIds = new string[]
				{
					this.selectFriendData.userData.userId
				};
				onSuccess = new Action(this.EndFriendReject);
			}
			APIRequestTask apirequestTask = APIUtil.Instance().RequestFriendApplicationDecision(friendUserIds, GameWebAPI.FR_Req_FriendDecision.DecisionType.REFUSAL, true);
			apirequestTask.SetAfterBehavior(TaskBase.AfterAlertClosed.RETURN);
			AppCoroutine.Start(apirequestTask.Run(onSuccess, new Action<Exception>(this.OnCommonFailedRecieve), new Func<Exception, IEnumerator>(this.OnCommonAlertedRecieve)), false);
		}
	}

	private void EndFriendApprove()
	{
		this.OnCommonRecieve();
	}

	private void EndFriendReject()
	{
		this.OnCommonRecieve();
	}

	public void OpenFriendProfile()
	{
		Action<int> action = null;
		if (FarmRoot.Instance.IsVisitFriendFarm)
		{
			action = delegate(int j)
			{
				base.PartsTitle.SetReturnAct(delegate(int i)
				{
					this.ShowFriendFarm(FarmRoot.Instance.visitFriendData, null);
				});
			};
		}
		CMD_ProfileFriend cmd_ProfileFriend = GUIMain.ShowCommonDialog(action, "CMD_ProfileFriend") as CMD_ProfileFriend;
		cmd_ProfileFriend.SetLastLoginTime(this.selectUserlastLoginTime);
	}

	public void CloseFriendProfile(int idx)
	{
		switch (this.ProfOpeType)
		{
		case CMD_ProfileFriend.OperationType.Break:
			this.EndFriendBreak();
			break;
		case CMD_ProfileFriend.OperationType.Request:
			this.EndFriendRequest();
			break;
		case CMD_ProfileFriend.OperationType.Cancel:
			this.EndFriendReqCancel();
			break;
		case CMD_ProfileFriend.OperationType.Approve:
			this.EndFriendApprove();
			break;
		default:
			global::Debug.LogWarning("フレンド処理で失敗");
			break;
		}
	}

	public void OnBlockSet(string userId)
	{
		bool flag = this.friendList.Any((GameWebAPI.FriendList x) => x.userData.userId == userId);
		if (flag)
		{
			this.EndFriendBreak();
		}
	}

	public void ShowFriendFarm(GameWebAPI.FriendList friendData, Action callback = null)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
		FarmRoot.Instance.ShowFriendFarm(friendData, new Action(this.ShowFriendProfile), new Action(this.ShowFriendList), new Action(this.HideFriendFarm), delegate
		{
			RestrictionInput.EndLoad();
			this.HideDLG();
			if (callback != null)
			{
				callback();
			}
		});
	}

	private void ShowFriendProfile()
	{
		FarmRoot.Instance.ChangeFriendFarmMode(false, delegate
		{
			base.ShowDLG();
		});
		CMD_ProfileFriend.friendData = FarmRoot.Instance.visitFriendData;
		this.OpenFriendProfile();
	}

	private void ShowFriendList()
	{
		FarmRoot.Instance.ChangeFriendFarmMode(false, delegate
		{
			base.ShowDLG();
		});
		base.PartsTitle.SetReturnAct(delegate(int i)
		{
			this.ShowFriendFarm(FarmRoot.Instance.visitFriendData, null);
		});
		base.MultiTab.OnTabTouchEnded(1, true);
	}

	private void HideFriendFarm()
	{
		this.ClosePanel(true);
	}

	public void ChangeSelectMode()
	{
		if (this.currentListNum == 0)
		{
			return;
		}
		CMD_FriendTop.SELECT_MODE select_MODE = this.currentSelectMode;
		if (select_MODE != CMD_FriendTop.SELECT_MODE.NONE)
		{
			this.ReturnFromSelectMode();
		}
		else
		{
			this.GotoSelectMode();
		}
	}

	private void GotoSelectMode()
	{
		int curentTabIdx = base.MultiTab.GetCurentTabIdx();
		this.ngSelectModeButton.text = StringMaster.GetString("SystemButtonReturn");
		this.coSelectButton1.onTouchEnded -= this.FriendReleaseTouchEnded;
		this.coSelectButton1.onTouchEnded -= this.ApplyReleaseTouchEnded;
		this.coSelectButton1.onTouchEnded -= this.AcceptTouchEnded;
		this.coSelectButton1.onTouchEnded -= this.RejectTouchEnded;
		this.spSelectModeButton.spriteName = "Common02_Btn_BaseOFF";
		switch (curentTabIdx)
		{
		case 1:
			this.currentSelectMode = CMD_FriendTop.SELECT_MODE.FRIEND_RELEASE;
			this.coSelectButton1.gameObject.SetActive(true);
			this.coSelectButton1.GetComponent<Collider>().enabled = false;
			this.coSelectButton1.onTouchEnded += this.FriendReleaseTouchEnded;
			this.ngSelectButton1.text = StringMaster.GetString("Friend-06");
			break;
		case 2:
			this.currentSelectMode = CMD_FriendTop.SELECT_MODE.REQ_RELEASE;
			this.coSelectButton1.gameObject.SetActive(true);
			this.coSelectButton1.GetComponent<Collider>().enabled = false;
			this.coSelectButton1.onTouchEnded += this.ApplyReleaseTouchEnded;
			this.ngSelectButton1.text = StringMaster.GetString("FriendApply-05");
			break;
		case 3:
			this.currentSelectMode = CMD_FriendTop.SELECT_MODE.ACCEPT_REJECT;
			this.coSelectButton1.gameObject.SetActive(true);
			this.coSelectButton2.gameObject.SetActive(true);
			this.coSelectButton1.GetComponent<Collider>().enabled = false;
			this.coSelectButton2.GetComponent<Collider>().enabled = false;
			this.coSelectButton1.onTouchEnded += this.AcceptTouchEnded;
			this.coSelectButton2.onTouchEnded += this.RejectTouchEnded;
			this.ngSelectButton1.text = StringMaster.GetString("FriendApproval-04");
			this.ngSelectButton2.text = StringMaster.GetString("FriendApproval-03");
			break;
		}
	}

	private void ReturnFromSelectMode()
	{
		if (this.currentSelectMode == CMD_FriendTop.SELECT_MODE.NONE)
		{
			return;
		}
		this.currentSelectMode = CMD_FriendTop.SELECT_MODE.NONE;
		this.ngSelectModeButton.text = StringMaster.GetString("Friend-05");
		this.spSelectButton1.spriteName = "Common02_Btn_BaseG";
		this.spSelectButton2.spriteName = "Common02_Btn_BaseG";
		this.coSelectButton1.gameObject.SetActive(false);
		this.coSelectButton2.gameObject.SetActive(false);
		this.spSelectModeButton.spriteName = "Common02_Btn_SupportRed";
		List<GUIListPartBS> partObjs = this.csSelectPanel.partObjs;
		foreach (GUIListPartBS guilistPartBS in partObjs)
		{
			GUIListPartsFriend guilistPartsFriend = guilistPartBS as GUIListPartsFriend;
			guilistPartsFriend.ChangeUnselectItem(false);
		}
		this.selectFriendDict.Clear();
	}

	private void FriendReleaseTouchEnded(Touch touch, Vector2 pos, bool flag)
	{
		CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseFriendBreakConfirm), "CMD_Confirm") as CMD_Confirm;
		cmd_Confirm.Title = StringMaster.GetString("Friend-07");
		cmd_Confirm.Info = string.Format(StringMaster.GetString("Friend-08"), this.selectFriendDict.Count);
	}

	private void ApplyReleaseTouchEnded(Touch touch, Vector2 pos, bool flag)
	{
		CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseFriendReqCancelConfirm), "CMD_Confirm") as CMD_Confirm;
		cmd_Confirm.Title = StringMaster.GetString("FriendApply-03");
		cmd_Confirm.Info = string.Format(StringMaster.GetString("FriendApply-04"), this.selectFriendDict.Count);
	}

	private void AcceptTouchEnded(Touch touch, Vector2 pos, bool flag)
	{
		CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseFriendApproveConfirm), "CMD_Confirm") as CMD_Confirm;
		cmd_Confirm.Title = StringMaster.GetString("FriendApproval-10");
		cmd_Confirm.Info = string.Format(StringMaster.GetString("FriendApproval-11"), this.selectFriendDict.Count);
	}

	private void RejectTouchEnded(Touch touch, Vector2 pos, bool flag)
	{
		CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseFriendRejectConfirm), "CMD_Confirm") as CMD_Confirm;
		cmd_Confirm.Title = StringMaster.GetString("FriendApproval-05");
		cmd_Confirm.Info = string.Format(StringMaster.GetString("FriendApproval-06"), this.selectFriendDict.Count);
	}

	private void CorrespondFriendBreakEnd()
	{
		GUIMain.BarrierON(null);
		RestrictionInput.EndLoad();
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int nop)
		{
			this.OnCorrespondCommonRecieve();
		}, "CMD_ModalMessage") as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("Friend-09");
		cmd_ModalMessage.Info = string.Format(StringMaster.GetString("Friend-10"), this.selectFriendDict.Count);
	}

	private void CorrespondApplyReleaseEnd()
	{
		GUIMain.BarrierON(null);
		RestrictionInput.EndLoad();
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int nop)
		{
			this.OnCorrespondCommonRecieve();
		}, "CMD_ModalMessage") as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("FriendApply-05");
		cmd_ModalMessage.Info = string.Format(StringMaster.GetString("FriendApply-06"), this.selectFriendDict.Count);
	}

	private void CorrespondAcceptEnd()
	{
		GUIMain.BarrierON(null);
		RestrictionInput.EndLoad();
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int nop)
		{
			this.OnCorrespondCommonRecieve();
		}, "CMD_ModalMessage") as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("FriendApproval-12");
		cmd_ModalMessage.Info = string.Format(StringMaster.GetString("FriendApproval-13"), this.selectFriendDict.Count);
	}

	private void CorrespondRejectEnd()
	{
		GUIMain.BarrierON(null);
		RestrictionInput.EndLoad();
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int nop)
		{
			this.OnCorrespondCommonRecieve();
		}, "CMD_ModalMessage") as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("FriendApproval-07");
		cmd_ModalMessage.Info = string.Format(StringMaster.GetString("FriendApproval-08"), this.selectFriendDict.Count);
	}

	private string[] GetSelectFriendIdList()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<GameWebAPI.FriendList, GUIListPartsFriend> keyValuePair in this.selectFriendDict)
		{
			if (keyValuePair.Key != null && keyValuePair.Key.userData != null && !string.IsNullOrEmpty(keyValuePair.Key.userData.userId))
			{
				list.Add(keyValuePair.Key.userData.userId);
			}
		}
		return list.ToArray();
	}

	private void ResetBadgeInfo(Action onCompleted)
	{
		APIRequestTask task = DataMng.Instance().RequestMyPageData(true);
		AppCoroutine.Start(task.Run(onCompleted, null, null), false);
	}

	private void OnCommonRecieve()
	{
		this.ResetBadgeInfo(delegate
		{
			GUIMain.BarrierON(null);
			RestrictionInput.EndLoad();
			this.ReturnFromSelectMode();
			AppCoroutine.Start(this.ReloadFriendUI(), false);
		});
	}

	private void OnCorrespondCommonRecieve()
	{
		this.ResetBadgeInfo(delegate
		{
			this.ReturnFromSelectMode();
			AppCoroutine.Start(this.ReloadFriendUI(), false);
		});
	}

	private void OnCommonFailedRecieve(Exception e)
	{
		this.OnCommonRecieve();
	}

	private IEnumerator OnCommonAlertedRecieve(Exception e)
	{
		this.OnCommonRecieve();
		yield return null;
		yield break;
	}

	private enum SELECT_MODE
	{
		NONE,
		FRIEND_RELEASE,
		REQ_RELEASE,
		ACCEPT_REJECT
	}
}
