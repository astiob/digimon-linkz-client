using FacilityShop;
using FarmData;
using Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class CMD_FacilityShop : CMD
{
	private static CMD_FacilityShop.FacilityShopTab lastFocusTab = CMD_FacilityShop.FacilityShopTab.FACILITY;

	[SerializeField]
	private GUISelectPanelFacility facilityList;

	[SerializeField]
	private GUISelectPanelFacility decorationList;

	[SerializeField]
	private GameObject facilityListOriginalItem;

	[SerializeField]
	private FacilityShopPlayerInfo playerInfo;

	private CMD_FacilityShop.ViewedList viewedList;

	private int[] newFacilityItemList;

	[CompilerGenerated]
	private static Action <>f__mg$cache0;

	[CompilerGenerated]
	private static Func<FacilityConditionM, bool> <>f__mg$cache1;

	protected override void Awake()
	{
		base.Awake();
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
	}

	public override void Show(Action<int> closeEvent, float sizeX, float sizeY, float showTime)
	{
		base.SetTutorialAnyTime("anytime_second_tutorial_facility_shop");
		GUICollider.DisableAllCollider("CMD_FacilityShop");
		if (this.ExistNewFacility() || this.ExistNewDecoration())
		{
			base.HideDLG();
			APIRequestTask apirequestTask = Singleton<UserDataMng>.Instance.RequestUserFacilityConditionData(true);
			APIRequestTask apirequestTask2 = this.ReqeustUserNewCount();
			if (apirequestTask2 != null)
			{
				apirequestTask.Add(apirequestTask2);
			}
			this.RequestUserFacilityShopInfo(apirequestTask, closeEvent, sizeX, sizeY, showTime);
		}
		else if (!Singleton<UserDataMng>.Instance.ExistUserFacilityCondition())
		{
			base.HideDLG();
			APIRequestTask task = Singleton<UserDataMng>.Instance.RequestUserFacilityConditionData(true);
			this.RequestUserFacilityShopInfo(task, closeEvent, sizeX, sizeY, showTime);
		}
		else
		{
			this.InitFacilityShop(closeEvent, sizeX, sizeY, showTime);
			RestrictionInput.EndLoad();
		}
	}

	private bool ExistNewFacility()
	{
		return DataMng.Instance().RespDataMP_MyPage.userNewsCountList.facilityNewCount > 0;
	}

	private bool ExistNewDecoration()
	{
		return DataMng.Instance().RespDataMP_MyPage.userNewsCountList.decorationNewCount > 0;
	}

	private APIRequestTask ReqeustUserNewCount()
	{
		if (this.ExistNewFacility() && CMD_FacilityShop.lastFocusTab == CMD_FacilityShop.FacilityShopTab.FACILITY)
		{
			DataMng.Instance().RespDataMP_MyPage.userNewsCountList.facilityNewCount = 0;
			return FarmDataManager.RequestUserNewFacility(FacilityType.FACILITY, new Action<UserNewFacilityResponse>(this.SetUserFacilityNewCount), true);
		}
		if (this.ExistNewDecoration() && CMD_FacilityShop.lastFocusTab == CMD_FacilityShop.FacilityShopTab.DECORATION)
		{
			DataMng.Instance().RespDataMP_MyPage.userNewsCountList.decorationNewCount = 0;
			return FarmDataManager.RequestUserNewFacility(FacilityType.DECORATION, new Action<UserNewFacilityResponse>(this.SetUserFacilityNewCount), true);
		}
		return null;
	}

	private void SetUserFacilityNewCount(UserNewFacilityResponse response)
	{
		this.newFacilityItemList = null;
		if (response != null && response.facilityIds != null)
		{
			this.newFacilityItemList = new int[response.facilityIds.Length];
			for (int i = 0; i < response.facilityIds.Length; i++)
			{
				this.newFacilityItemList[i] = response.facilityIds[i];
			}
		}
	}

	private void RequestUserFacilityShopInfo(APIRequestTask task, Action<int> closeEvent, float sizeX, float sizeY, float showTime)
	{
		base.StartCoroutine(task.Run(delegate
		{
			RestrictionInput.EndLoad();
			Singleton<UserDataMng>.Instance.RequestUserStockFacilityDataAPI(delegate(bool flg)
			{
				if (flg)
				{
					this.ShowDLG();
					this.InitFacilityShop(closeEvent, sizeX, sizeY, showTime);
				}
				else
				{
					RestrictionInput.EndLoad();
					GUICollider.EnableAllCollider("CMD_FacilityShop");
					this.ClosePanel(false);
				}
			});
		}, delegate(Exception nop)
		{
			RestrictionInput.EndLoad();
			GUICollider.EnableAllCollider("CMD_FacilityShop");
			this.ClosePanel(false);
		}, null));
	}

	private void InitFacilityShop(Action<int> closeEvent, float sizeX, float sizeY, float showTime)
	{
		base.PartsTitle.SetTitle(StringMaster.GetString("FacilityShopTitle"));
		base.MultiTab.InitMultiTab(new List<Action<int>>
		{
			new Action<int>(this.OnTouchedTabFacility),
			new Action<int>(this.OnTouchedTabDecoration)
		}, new List<string>
		{
			StringMaster.GetString("FacilityShopTitle"),
			StringMaster.GetString("FacilityShopDecoration")
		});
		base.MultiTab.SetOnOffColor(new Color(0.980392158f, 0.945098042f, 0f, 1f), new Color(1f, 1f, 1f, 1f));
		base.MultiTab.SetFocus((int)CMD_FacilityShop.lastFocusTab);
		base.MultiTab.SetActiveAlertIcon(new bool[]
		{
			this.ExistNewFacility(),
			this.ExistNewDecoration()
		});
		FacilityM[] array = FarmDataManager.GetFacilityShopGoods(FacilityType.FACILITY);
		FacilityM[] array2 = FarmDataManager.GetFacilityShopGoods(FacilityType.DECORATION);
		array = FacilityShopFilter.CheckFilter(array);
		array2 = FacilityShopFilter.CheckFilter(array2);
		array = this.SortFacilityShopItemData(this.newFacilityItemList, array);
		array2 = this.SortFacilityShopItemData(this.newFacilityItemList, array2);
		this.CreateFacilityList(this.facilityList, array.Length);
		this.SetFacilityDetail(this.facilityList, array);
		this.CreateFacilityList(this.decorationList, array2.Length);
		this.SetFacilityDetail(this.decorationList, array2);
		this.facilityListOriginalItem.SetActive(false);
		if (CMD_FacilityShop.lastFocusTab == CMD_FacilityShop.FacilityShopTab.FACILITY)
		{
			this.viewedList.facility = true;
			this.decorationList.gameObject.SetActive(false);
		}
		else
		{
			this.viewedList.decoration = true;
			this.facilityList.gameObject.SetActive(false);
		}
		base.Show(closeEvent, sizeX, sizeY, showTime);
		GUIFace.instance.HideGUI();
		GUIFaceIndicator.instance.HideLocator(true);
		this.playerInfo.SetPlayerInfo();
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		if (null != tutorialObserver)
		{
			GUIMain.BarrierON(null);
			TutorialObserver tutorialObserver2 = tutorialObserver;
			string tutorialName = "second_tutorial_facility_shop";
			if (CMD_FacilityShop.<>f__mg$cache0 == null)
			{
				CMD_FacilityShop.<>f__mg$cache0 = new Action(GUIMain.BarrierOFF);
			}
			tutorialObserver2.StartSecondTutorial(tutorialName, CMD_FacilityShop.<>f__mg$cache0, delegate
			{
				GUICollider.EnableAllCollider("CMD_FacilityShop");
			});
		}
		else
		{
			GUICollider.EnableAllCollider("CMD_FacilityShop");
		}
	}

	private void CreateFacilityList(GUISelectPanelFacility listUI, int listItemCount)
	{
		GUICollider component = listUI.GetComponent<GUICollider>();
		BoxCollider component2 = component.GetComponent<BoxCollider>();
		Vector3 localPosition = component.transform.localPosition;
		component.SetOriginalPos(this.facilityListOriginalItem.transform.localPosition);
		component.transform.localPosition = localPosition;
		Rect listWindowViewRect = new Rect
		{
			xMin = component2.size.x * -0.5f,
			xMax = component2.size.x * 0.5f,
			yMin = component2.size.y * -0.5f - 40f,
			yMax = component2.size.y * 0.5f
		};
		listWindowViewRect.yMin = listWindowViewRect.y - GUIMain.VerticalSpaceSize;
		listWindowViewRect.yMax = listWindowViewRect.y + listWindowViewRect.height + GUIMain.VerticalSpaceSize;
		listUI.ListWindowViewRect = listWindowViewRect;
		listUI.selectParts = this.facilityListOriginalItem;
		listUI.initLocation = true;
		listUI.AllBuild(listItemCount);
	}

	private void SetFacilityDetail(GUISelectPanelFacility listUI, FacilityM[] facilityData)
	{
		CMD_FacilityShop.<SetFacilityDetail>c__AnonStorey1 <SetFacilityDetail>c__AnonStorey = new CMD_FacilityShop.<SetFacilityDetail>c__AnonStorey1();
		<SetFacilityDetail>c__AnonStorey.items = listUI.GetComponentsInChildren<FacilityShopItem>();
		if (<SetFacilityDetail>c__AnonStorey.items == null)
		{
			return;
		}
		int i;
		for (i = 0; i < <SetFacilityDetail>c__AnonStorey.items.Length; i++)
		{
			FacilityConditionM[] facilityCondition = FarmDataManager.GetFacilityCondition(facilityData[i].releaseId);
			bool[] isClearConditions = null;
			if (facilityCondition != null)
			{
				IEnumerable<FacilityConditionM> source = facilityCondition;
				if (CMD_FacilityShop.<>f__mg$cache1 == null)
				{
					CMD_FacilityShop.<>f__mg$cache1 = new Func<FacilityConditionM, bool>(FacilityShopFilter.CheckFacilityCondition);
				}
				isClearConditions = source.Select(CMD_FacilityShop.<>f__mg$cache1).ToArray<bool>();
			}
			<SetFacilityDetail>c__AnonStorey.items[i].SetDetail(facilityData[i], facilityCondition, isClearConditions, new Action<FacilityShopItem>(this.OnPushedBuyButton));
			BoxCollider component = <SetFacilityDetail>c__AnonStorey.items[i].GetComponent<BoxCollider>();
			if (null != component)
			{
				component.enabled = false;
			}
			if (this.newFacilityItemList != null)
			{
				<SetFacilityDetail>c__AnonStorey.items[i].SetNewIcon(this.newFacilityItemList.Any((int id) => id == <SetFacilityDetail>c__AnonStorey.items[i].facilityID));
			}
			else
			{
				<SetFacilityDetail>c__AnonStorey.items[i].SetNewIcon(false);
			}
		}
	}

	private void SetNewIcon(GUISelectPanelFacility listUI)
	{
		CMD_FacilityShop.<SetNewIcon>c__AnonStorey3 <SetNewIcon>c__AnonStorey = new CMD_FacilityShop.<SetNewIcon>c__AnonStorey3();
		<SetNewIcon>c__AnonStorey.items = listUI.GetComponentsInChildren<FacilityShopItem>(true);
		if (<SetNewIcon>c__AnonStorey.items == null)
		{
			return;
		}
		int i;
		for (i = 0; i < <SetNewIcon>c__AnonStorey.items.Length; i++)
		{
			bool newIcon = false;
			if (this.newFacilityItemList != null)
			{
				newIcon = this.newFacilityItemList.Any((int id) => id == <SetNewIcon>c__AnonStorey.items[i].facilityID);
			}
			<SetNewIcon>c__AnonStorey.items[i].SetNewIcon(newIcon);
		}
		this.newFacilityItemList = null;
	}

	public void OnPushedBuyButton(FacilityShopItem facilityItem)
	{
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(facilityItem.facilityID);
		FacilityConditionM[] facilityCondition = FarmDataManager.GetFacilityCondition(facilityMaster.releaseId);
		if (facilityCondition != null)
		{
			if (!facilityCondition.Any((FacilityConditionM x) => false == FacilityShopFilter.CheckFacilityCondition(x)))
			{
				this.StartFarmObjectPutMode(facilityItem.facilityID);
			}
		}
		else
		{
			this.StartFarmObjectPutMode(facilityItem.facilityID);
		}
	}

	private void StartFarmObjectPutMode(int facilityID)
	{
		FarmRoot instance = FarmRoot.Instance;
		if (null == instance)
		{
			global::Debug.LogError("FarmRoot Not Found");
			this.ClosePanel(true);
			return;
		}
		FarmObjectSetting settingObject = instance.SettingObject;
		FarmObjectSelect selectObject = instance.SelectObject;
		FarmScenery scenery = instance.Scenery;
		selectObject.ClearSelectState();
		selectObject.EnabledTouchedEvent(false);
		bool flag = false;
		if (null != settingObject.farmObject && settingObject.settingMode == FarmObjectSetting.SettingMode.BUILD)
		{
			if (facilityID == settingObject.farmObject.facilityID)
			{
				flag = true;
			}
			else
			{
				scenery.CancelSetting();
				FacilityConfirmation componentInChildren = Singleton<GUIMain>.Instance.GetComponentInChildren<FacilityConfirmation>();
				componentInChildren.DeleteObject();
			}
		}
		if (!flag && scenery.BuildFarmObject(facilityID))
		{
			instance.farmUI.CreateFacilityConfirmation();
			instance.SetActiveNotTouchObject(false);
		}
		PartsMenu.instance.gameObject.SetActive(false);
		this.CloseAction(true);
	}

	private void OnTouchedTabFacility(int noop)
	{
		CMD_FacilityShop.lastFocusTab = CMD_FacilityShop.FacilityShopTab.FACILITY;
		APIRequestTask apirequestTask = this.ReqeustUserNewCount();
		if (apirequestTask != null)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			base.StartCoroutine(apirequestTask.Run(delegate
			{
				RestrictionInput.EndLoad();
				this.EnableFacilityList();
			}, null, null));
		}
		else
		{
			this.EnableFacilityList();
		}
	}

	private void EnableFacilityList()
	{
		this.facilityList.gameObject.SetActive(true);
		this.decorationList.gameObject.SetActive(false);
		this.decorationList.FadeOutAllListParts(null, true);
		this.SetNewIcon(this.facilityList);
		base.MultiTab.SetActiveAlertIcon(new bool[]
		{
			this.ExistNewFacility(),
			this.ExistNewDecoration()
		});
		if (this.viewedList.facility)
		{
			this.facilityList.FadeInAllListParts(null);
		}
		else
		{
			this.viewedList.facility = true;
		}
	}

	private void OnTouchedTabDecoration(int noop)
	{
		CMD_FacilityShop.lastFocusTab = CMD_FacilityShop.FacilityShopTab.DECORATION;
		APIRequestTask apirequestTask = this.ReqeustUserNewCount();
		if (apirequestTask != null)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			base.StartCoroutine(apirequestTask.Run(delegate
			{
				RestrictionInput.EndLoad();
				this.EnableDecorationList();
			}, null, null));
		}
		else
		{
			this.EnableDecorationList();
		}
	}

	private void EnableDecorationList()
	{
		this.facilityList.gameObject.SetActive(false);
		this.facilityList.FadeOutAllListParts(null, true);
		this.decorationList.gameObject.SetActive(true);
		this.SetNewIcon(this.decorationList);
		base.MultiTab.SetActiveAlertIcon(new bool[]
		{
			this.ExistNewFacility(),
			this.ExistNewDecoration()
		});
		if (this.viewedList.decoration)
		{
			this.decorationList.FadeInAllListParts(null);
		}
		else
		{
			this.viewedList.decoration = true;
		}
	}

	private void OnClicedStoneShop()
	{
		GUIMain.ShowCommonDialog(null, "CMD_Shop", null);
	}

	public override void ClosePanel(bool animation = true)
	{
		GUIFace.instance.ShowGUI();
		GUIFaceIndicator.instance.ShowLocator();
		GUIFace.SetFacilityShopButtonBadge();
		this.CloseAction(animation);
	}

	private void CloseAction(bool animation)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
		if (this.facilityList != null)
		{
			this.facilityList.FadeOutAllListParts(null, false);
			this.facilityList.SetHideScrollBarAllWays(true);
		}
		if (this.decorationList != null)
		{
			this.decorationList.FadeOutAllListParts(null, false);
			this.decorationList.SetHideScrollBarAllWays(true);
		}
	}

	private void FakeMethod()
	{
	}

	private FacilityM[] SortFacilityShopItemData(int[] newItemIdList, FacilityM[] facilityList)
	{
		List<FacilityM> list = new List<FacilityM>();
		List<FacilityM> list2 = new List<FacilityM>();
		List<FacilityM> list3 = new List<FacilityM>();
		List<FacilityM> list4 = new List<FacilityM>();
		if (null != FarmRoot.Instance)
		{
			for (int i = 0; i < facilityList.Length; i++)
			{
				int id = facilityList[i].facilityId.ToInt32();
				int num = FarmRoot.Instance.Scenery.GetFacilityCount(id);
				List<UserFacility> stockFacilityListByfacilityIdAndLevel = Singleton<UserDataMng>.Instance.GetStockFacilityListByfacilityIdAndLevel(id);
				int count = stockFacilityListByfacilityIdAndLevel.Count;
				num += count;
				FacilityConditionM[] facilityCondition = FarmDataManager.GetFacilityCondition(facilityList[i].releaseId);
				if (newItemIdList != null && newItemIdList.Any((int x) => x == id))
				{
					list.Add(facilityList[i]);
				}
				else
				{
					if (facilityCondition != null)
					{
						if (facilityCondition.Any((FacilityConditionM x) => !FacilityShopFilter.CheckFacilityCondition(x)))
						{
							list3.Add(facilityList[i]);
							goto IL_131;
						}
					}
					if (num < facilityList[i].maxNum.ToInt32())
					{
						list2.Add(facilityList[i]);
					}
					else
					{
						list4.Add(facilityList[i]);
					}
				}
				IL_131:;
			}
		}
		List<FacilityM> list5 = new List<FacilityM>(facilityList.Length);
		list5.AddRange(list);
		list5.AddRange(list2);
		list5.AddRange(list3);
		list5.AddRange(list4);
		return list5.ToArray();
	}

	public enum FacilityShopTab
	{
		FACILITY = 1,
		DECORATION
	}

	private struct ViewedList
	{
		public bool facility;

		public bool decoration;
	}
}
