using FarmData;
using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CMD_FacilityStock : CMD
{
	[SerializeField]
	private SelectPanelFacilityStock csSelectPanel;

	[SerializeField]
	private ListPartsFacilityStock csListParts;

	[SerializeField]
	private UILabel lbTX_EMPTY;

	public static CMD_FacilityStock instance;

	private Action<int> f_bk;

	private float sizeX_bk;

	private float sizeY_bk;

	private float aT_bk;

	protected override void Awake()
	{
		base.Awake();
		this.lbTX_EMPTY.gameObject.SetActive(false);
		CMD_FacilityStock.instance = this;
	}

	protected override void WindowClosed()
	{
		base.WindowClosed();
		CMD_FacilityStock.instance = null;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		this.f_bk = f;
		this.sizeX_bk = sizeX;
		this.sizeY_bk = sizeY;
		this.aT_bk = aT;
		Singleton<UserDataMng>.Instance.RequestUserStockFacilityDataAPI(new Action<bool>(this.EndReceiveData));
	}

	private void EndReceiveData(bool isLoadedData)
	{
		RestrictionInput.EndLoad();
		if (!isLoadedData)
		{
			base.ClosePanel(false);
		}
		else
		{
			base.ShowDLG();
			this.SetCommonUI();
			this.InitFacilityStockList();
			GUIFace.SetFacilityStockIcon(false);
			base.Show(this.f_bk, this.sizeX_bk, this.sizeY_bk, this.aT_bk);
		}
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		this.lbTX_EMPTY.gameObject.SetActive(true);
		FarmCameraControlForCMD.Off();
	}

	private void CloseAndFarmCamOn(bool animation)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	public override void ClosePanel(bool animation = true)
	{
		GUIFace.instance.ShowGUI();
		GUIFaceIndicator.instance.ShowLocator();
		GUIFace.ShowLocator();
		this.CloseAndFarmCamOn(animation);
		this.csSelectPanel.FadeOutAllListParts(null, false);
		this.csSelectPanel.SetHideScrollBarAllWays(true);
		this.lbTX_EMPTY.gameObject.SetActive(false);
	}

	private void SetCommonUI()
	{
		base.PartsTitle.SetTitle(StringMaster.GetString("FacilityStockTitle"));
	}

	private void InitFacilityStockList()
	{
		List<ListPartsFacilityStock.FacilityStockListData> list = this.MakeFacilityStockDataList();
		this.csSelectPanel.selectParts = this.csListParts.gameObject;
		this.csSelectPanel.initLocation = true;
		if (list.Count > 0)
		{
			this.csSelectPanel.AllBuild(list, new Action<int>(this.OnClickedPlace));
		}
		else
		{
			this.lbTX_EMPTY.text = StringMaster.GetString("FacilityStockNone");
		}
		this.csListParts.gameObject.SetActive(false);
	}

	private List<ListPartsFacilityStock.FacilityStockListData> MakeFacilityStockDataList()
	{
		List<ListPartsFacilityStock.FacilityStockListData> list = new List<ListPartsFacilityStock.FacilityStockListData>();
		List<UserFacility> userStockFacilityList = Singleton<UserDataMng>.Instance.GetUserStockFacilityList();
		userStockFacilityList.Sort(new Comparison<UserFacility>(this.CompareUserStockFacility));
		int num = -1;
		ListPartsFacilityStock.FacilityStockListData facilityStockListData = null;
		for (int i = 0; i < userStockFacilityList.Count; i++)
		{
			if (userStockFacilityList[i].facilityId != num)
			{
				facilityStockListData = new ListPartsFacilityStock.FacilityStockListData();
				list.Add(facilityStockListData);
				facilityStockListData.facilityId = userStockFacilityList[i].facilityId;
				num = userStockFacilityList[i].facilityId;
				facilityStockListData.userFacilityIdList = new List<int>();
				facilityStockListData.userFacilityIdList.Add(userStockFacilityList[i].userFacilityId);
			}
			else
			{
				facilityStockListData.userFacilityIdList.Add(userStockFacilityList[i].userFacilityId);
			}
		}
		return list;
	}

	private int CompareUserStockFacility(UserFacility x, UserFacility y)
	{
		int num = x.facilityId;
		int num2 = y.facilityId;
		if (num < num2)
		{
			return -1;
		}
		if (num > num2)
		{
			return 1;
		}
		num = x.level;
		num2 = y.level;
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		return 0;
	}

	public void RebuildFacilityStockList()
	{
		List<ListPartsFacilityStock.FacilityStockListData> list = this.MakeFacilityStockDataList();
		this.csListParts.gameObject.SetActive(true);
		if (list.Count > 0)
		{
			this.csSelectPanel.AllBuild(list, new Action<int>(this.OnClickedPlace));
		}
		else
		{
			this.lbTX_EMPTY.text = StringMaster.GetString("FacilityStockNone");
			this.csSelectPanel.ReleaseBuild();
		}
		this.csListParts.gameObject.SetActive(false);
	}

	private void OnClickedPlace(int facilityID)
	{
		this.StartFarmObjectPutMode(facilityID);
	}

	private void StartFarmObjectPutMode(int facilityID)
	{
		FarmRoot farmRoot = FarmRoot.Instance;
		if (null == farmRoot)
		{
			global::Debug.LogError("FarmRoot Not Found");
			this.ClosePanel(true);
			return;
		}
		FarmObjectSetting settingObject = farmRoot.SettingObject;
		FarmObjectSelect selectObject = farmRoot.SelectObject;
		FarmScenery scenery = farmRoot.Scenery;
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
				StockFacilityConfirmation componentInChildren = Singleton<GUIMain>.Instance.GetComponentInChildren<StockFacilityConfirmation>();
				componentInChildren.DeleteObject();
			}
		}
		if (!flag && scenery.BuildFarmObject(facilityID))
		{
			farmRoot.farmUI.CreateStockFacilityConfirmation();
			farmRoot.SetActiveNotTouchObject(false);
		}
		this.ClosePanel(true);
		GUIFace.instance.HideGUI();
		GUIFace.instance.HideHeaderBtns();
		GUIFace.HideLocator();
		GUIFaceIndicator.instance.HideLocator(false);
	}
}
