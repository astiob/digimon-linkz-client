using FarmData;
using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CMD_GashaTOP : CMD
{
	public static CMD_GashaTOP instance;

	public static GameWebAPI.RespDataGA_GetGachaInfo gachaInfo;

	[SerializeField]
	private GameObject goBTN_SINGLE;

	[SerializeField]
	private GameObject goBTN_TEN;

	[SerializeField]
	private GameObject rareGashaBG;

	[SerializeField]
	private GameObject linkGashaBG;

	[SerializeField]
	private UILabel ngTX_LINK_POINT;

	[SerializeField]
	private UILabel ngTX_STONE_NUM;

	private UISprite ngBTN_SINGLE;

	private UISprite ngBTN_TEN;

	private GUICollider colBTN_SINGLE;

	private GUICollider colBTN_TEN;

	[SerializeField]
	private GameObject goCAMPAIGN_1;

	[SerializeField]
	private GameObject goCAMPAIGN_10;

	[SerializeField]
	private UILabel lbCAMPAIGN_1;

	[SerializeField]
	private UILabel lbCAMPAIGN_10;

	[SerializeField]
	private UILabel ngTX_EXP_SINGLE;

	[SerializeField]
	private UILabel ngTX_EXP_TEN;

	[SerializeField]
	private UITexture ngTEX_BANNER_B;

	[SerializeField]
	private UITexture ngTEX_BANNER_B_Link;

	[SerializeField]
	private GameObject goCAD_ROOT;

	[SerializeField]
	private UILabel ngTX_ADVENT_CAUTION;

	private GashaTutorialMode gashaTutorialMode = new GashaTutorialMode();

	[SerializeField]
	private GUISelectPanelGashaEdit csSelectPanelGasha;

	[SerializeField]
	private GameObject rareParts;

	[SerializeField]
	private GameObject linkParts;

	[SerializeField]
	private GameObject goSelectPanelGashaMain;

	[SerializeField]
	private GameObject goListPartsGashaMain_0;

	public GUISelectPanelGashaMain csSelectPanelGashaMain;

	private Action finishedActionCutScene;

	private Action finishedActionCutScene_2;

	private bool isExecGasha;

	private List<GameWebAPI.RespDataGA_GetGachaInfo.Result> gashaList;

	private int activeListPartsIDX;

	private GameWebAPI.GA_Req_ExecGacha req_exec_bk;

	private GameWebAPI.RespDataGA_ExecChip chipResult;

	private GameWebAPI.RespDataGA_ExecGacha gachaResult;

	private GameWebAPI.RespDataGA_ExecTicket ticketResult;

	public string SingleNeedCount { get; private set; }

	public string TenNeedCount { get; private set; }

	protected override void Awake()
	{
		CMD_GashaTOP.instance = this;
		base.Awake();
		this.ngBTN_SINGLE = this.goBTN_SINGLE.GetComponent<UISprite>();
		this.ngBTN_TEN = this.goBTN_TEN.GetComponent<UISprite>();
		this.colBTN_SINGLE = this.goBTN_SINGLE.GetComponent<GUICollider>();
		this.colBTN_TEN = this.goBTN_TEN.GetComponent<GUICollider>();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		base.StartCoroutine(this.InitDLG(f, sizeX, sizeY, aT));
	}

	private IEnumerator InitDLG(Action<int> f, float sizeX, float sizeY, float aT)
	{
		GameWebAPI.GA_Req_GashaInfo.Type type = GameWebAPI.GA_Req_GashaInfo.Type.NORMAL;
		if (GashaTutorialMode.TutoExec)
		{
			type = GameWebAPI.GA_Req_GashaInfo.Type.TUTORIAL;
		}
		APIRequestTask task = APIUtil.Instance().RequestGashaInfo(type, false);
		yield return base.StartCoroutine(task.Run(delegate
		{
			this.OnSuccessRequestGashaInfo(f, sizeX, sizeY, aT);
		}, new Action<Exception>(this.OnFailedRequestGashaInfo), null));
		yield break;
	}

	private void OnSuccessRequestGashaInfo(Action<int> closeEvent, float sizeX, float sizeY, float showTime)
	{
		Singleton<UserDataMng>.Instance.RequestUserStockFacilityDataAPI(delegate(bool flg)
		{
			if (flg)
			{
				this.InitGashaInfo();
				if (this.IsValidGashaInfo())
				{
					DownloadGashaTopTex.Instance.DownloadTexture(this.gashaList, delegate
					{
						this.ShowDLG();
						SoundMng.Instance().PlayGameBGM("bgm_202");
						RestrictionInput.EndLoad();
						this.activeListPartsIDX = 0;
						this.BuildMainList();
						this.ChangeSelection(this.activeListPartsIDX);
						this.Show(closeEvent, sizeX, sizeY, showTime);
					});
				}
				else
				{
					Loading.Invisible();
					AlertManager.ShowAlertDialog(delegate(int idx)
					{
						this.ClosePanel(false);
						RestrictionInput.EndLoad();
					}, "C-GA01");
				}
			}
			else
			{
				this.ClosePanel(false);
				RestrictionInput.EndLoad();
			}
		});
	}

	private void OnFailedRequestGashaInfo(Exception noop)
	{
		RestrictionInput.EndLoad();
		if (GashaTutorialMode.TutoExec)
		{
			base.SetCloseAction(delegate(int nop)
			{
				GUIMain.BackToTOP("UIStartupCaution", 0.8f, 0.8f);
			});
		}
		base.ClosePanel(false);
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
	}

	private void CloseAndFarmCamOn(bool animation)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	public override void ClosePanel(bool animation = true)
	{
		if (this.isExecGasha && base.gameObject.activeSelf)
		{
			base.StartCoroutine(this.RequestMyPageData(animation));
		}
		else
		{
			this.Finalize(animation);
		}
	}

	private IEnumerator RequestMyPageData(bool animation)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		APIRequestTask task = DataMng.Instance().RequestMyPageData(false);
		return task.Run(delegate
		{
			ClassSingleton<FaceMissionAccessor>.Instance.faceMission.SetBadge();
			this.Finalize(animation);
			RestrictionInput.EndLoad();
		}, delegate(Exception noop)
		{
			this.Finalize(animation);
			RestrictionInput.EndLoad();
		}, null);
	}

	private void Finalize(bool animation)
	{
		if (CMD_10gashaResult.instance != null)
		{
			CMD_10gashaResult.instance.ClosePanel(true);
		}
		SoundMng.Instance().PlayGameBGM("bgm_201");
		this.csSelectPanelGashaMain.FadeOutAllListParts(null, false);
		this.CloseAndFarmCamOn(animation);
	}

	protected override void OnDestroy()
	{
		this.ReleaseMainTex();
		base.OnDestroy();
		CMD_GashaTOP.instance = null;
	}

	private void OnClickedShop()
	{
		GUIMain.ShowCommonDialog(delegate(int i)
		{
			this.ShowPointData();
			this.ShowAllData(this.activeListPartsIDX);
			this.csSelectPanelGashaMain.RefreshAbleCount();
		}, "CMD_Shop");
	}

	private void OnClickedAppear()
	{
		GameWebAPI.RespDataGA_GetGachaInfo.Result result = this.gashaList[this.activeListPartsIDX];
		CMDWebWindow cmdwebWindow = GUIMain.ShowCommonDialog(null, "CMDWebWindow") as CMDWebWindow;
		cmdwebWindow.TitleText = StringMaster.GetString("SystemCaution");
		if (result.IsLinkChip() || result.IsRareChip())
		{
			cmdwebWindow.Url = WebAddress.EXT_ADR_CHIP_GASHA_NOTICE + result.gachaId;
		}
		else if (result.IsLink() || result.IsRare())
		{
			cmdwebWindow.Url = WebAddress.EXT_ADR_GASHA_NOTICE + result.gachaId;
		}
		else if (result.IsLinkTicket() || result.IsRareTicket())
		{
			cmdwebWindow.Url = WebAddress.EXT_ADR_TICKET_GASHA_NOTICE + result.gachaId;
		}
		else
		{
			global::Debug.LogError("存在しないガシャ");
		}
	}

	private void OnClickedPickUp()
	{
		GameWebAPI.RespDataGA_GetGachaInfo.Result result = this.gashaList[this.activeListPartsIDX];
		CommonDialog commonDialog = GUIMain.ShowCommonDialog(null, "CMDWebWindow");
		if (result.IsLinkChip() || result.IsRareChip())
		{
			((CMDWebWindow)commonDialog).TitleText = Regex.Replace(StringMaster.GetString("ChipGashaLineupCaution"), "\\t|\\n|\\r", string.Empty);
			((CMDWebWindow)commonDialog).Url = WebAddress.EXT_ADR_CHIP_GASHA_INCIDENCE + result.gachaId;
		}
		else if (result.IsLink() || result.IsRare())
		{
			((CMDWebWindow)commonDialog).TitleText = Regex.Replace(StringMaster.GetString("GashaLineupCaution"), "\\t|\\n|\\r", string.Empty);
			((CMDWebWindow)commonDialog).Url = WebAddress.EXT_ADR_GASHA_INCIDENCE + result.gachaId;
		}
		else if (result.IsLinkTicket() || result.IsRareTicket())
		{
			((CMDWebWindow)commonDialog).TitleText = Regex.Replace(StringMaster.GetString("TicketGashaLineupCaution"), "\\t|\\n|\\r", string.Empty);
			((CMDWebWindow)commonDialog).Url = WebAddress.EXT_ADR_TICKET_GASHA_INCIDENCE + result.gachaId;
		}
	}

	private void OnCrickedInfo()
	{
		GameWebAPI.RespDataGA_GetGachaInfo.Result result = this.gashaList[this.activeListPartsIDX];
		CommonDialog commonDialog = GUIMain.ShowCommonDialog(null, "CMDWebWindow");
		((CMDWebWindow)commonDialog).TitleText = result.gachaName;
		((CMDWebWindow)commonDialog).callbackAction = null;
		int num = int.Parse(result.gachaId);
		int num2 = num;
		((CMDWebWindow)commonDialog).Url = ConstValue.APP_WEB_DOMAIN + ConstValue.WEB_INFO_ADR + num2.ToString();
	}

	private GameWebAPI.RespDataGA_GetGachaInfo.Result GetGashaInfo(int index)
	{
		if (this.gashaList.Count > index)
		{
			return this.gashaList[index];
		}
		return null;
	}

	public GameWebAPI.RespDataGA_GetGachaInfo.Result GetGashaInfo()
	{
		if (this.gashaList.Count > this.activeListPartsIDX)
		{
			return this.gashaList[this.activeListPartsIDX];
		}
		return null;
	}

	private void InitGashaInfo()
	{
		GameWebAPI.RespDataGA_GetGachaInfo.Result[] result = CMD_GashaTOP.gachaInfo.result;
		this.gashaList = new List<GameWebAPI.RespDataGA_GetGachaInfo.Result>();
		for (int i = 0; i < result.Length; i++)
		{
			this.gashaList.Add(result[i]);
		}
		if (!GashaTutorialMode.TutoExec)
		{
			this.gashaList.Sort(new Comparison<GameWebAPI.RespDataGA_GetGachaInfo.Result>(this.Compare_SORT));
		}
	}

	private bool IsValidGashaInfo()
	{
		return this.gashaList.Count >= 2;
	}

	private int Compare_SORT(GameWebAPI.RespDataGA_GetGachaInfo.Result x, GameWebAPI.RespDataGA_GetGachaInfo.Result y)
	{
		int num = int.Parse(x.dispNum);
		int num2 = int.Parse(y.dispNum);
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		string startTime = x.startTime;
		string startTime2 = y.startTime;
		num = startTime.CompareTo(startTime2);
		num2 = startTime2.CompareTo(startTime);
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		num = int.Parse(x.gachaId);
		num2 = int.Parse(y.gachaId);
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

	private void BuildMainList()
	{
		this.csSelectPanelGashaMain = this.goSelectPanelGashaMain.GetComponent<GUISelectPanelGashaMain>();
		this.csSelectPanelGashaMain.selectParts = this.goListPartsGashaMain_0;
		Rect rectWindowArea = this.GetRectWindowArea(this.goSelectPanelGashaMain.transform.localPosition);
		this.csSelectPanelGashaMain.ListWindowViewRect = rectWindowArea;
		int facilityCount = this.GetFacilityCount(25);
		if (facilityCount <= 0)
		{
			List<GameWebAPI.RespDataGA_GetGachaInfo.Result> list = new List<GameWebAPI.RespDataGA_GetGachaInfo.Result>();
			for (int i = 0; i < this.gashaList.Count; i++)
			{
				if (!this.gashaList[i].IsRareChip() && !this.gashaList[i].IsLinkChip())
				{
					list.Add(this.gashaList[i]);
				}
			}
			this.gashaList = list;
		}
		this.csSelectPanelGashaMain.initLocation = true;
		this.csSelectPanelGashaMain.AllBuild(this.gashaList);
		this.goListPartsGashaMain_0.SetActive(false);
	}

	private int GetFacilityCount(int facilityID)
	{
		FarmRoot farmRoot = FarmRoot.Instance;
		if (null == farmRoot)
		{
			global::Debug.LogError("FarmRoot Not Found");
			return -1;
		}
		int facilityCount = farmRoot.Scenery.GetFacilityCount(facilityID);
		List<UserFacility> stockFacilityListByfacilityIdAndLevel = Singleton<UserDataMng>.Instance.GetStockFacilityListByfacilityIdAndLevel(facilityID, -1);
		int count = stockFacilityListByfacilityIdAndLevel.Count;
		return facilityCount + count;
	}

	private Rect GetRectWindowArea(Vector3 v3)
	{
		return new Rect
		{
			xMin = -280f + v3.x,
			xMax = 280f + v3.x,
			yMin = -240f - GUIMain.VerticalSpaceSize,
			yMax = 250f + GUIMain.VerticalSpaceSize
		};
	}

	private void ReleaseMainTex()
	{
		if (this.gashaList != null)
		{
			for (int i = 0; i < this.gashaList.Count; i++)
			{
				this.gashaList[i].tex = null;
			}
		}
	}

	public void ChangeSelection(int idx)
	{
		this.activeListPartsIDX = idx;
		GameWebAPI.RespDataGA_GetGachaInfo.Result gashaInfo = this.GetGashaInfo(this.activeListPartsIDX);
		if (gashaInfo.IsRare() || gashaInfo.IsLink() || gashaInfo.IsRareChip() || gashaInfo.IsLinkChip() || gashaInfo.IsRareTicket() || gashaInfo.IsLinkTicket())
		{
			this.InitPanel();
			this.csSelectPanelGasha.ResetAutoScrollTime();
			this.csSelectPanelGasha.initLocation = true;
			this.csSelectPanelGasha.AllBuild(gashaInfo.subImagePath);
			this.csSelectPanelGasha.selectParts.SetActive(false);
			if (gashaInfo.IsRare())
			{
				base.PartsTitle.SetTitle(StringMaster.GetString("GashaTitleRare"));
			}
			else if (gashaInfo.IsLink())
			{
				base.PartsTitle.SetTitle(StringMaster.GetString("GashaTitleLink"));
			}
			else if (gashaInfo.IsRareChip())
			{
				base.PartsTitle.SetTitle(StringMaster.GetString("GashaTitleRare"));
			}
			else if (gashaInfo.IsLinkChip())
			{
				base.PartsTitle.SetTitle(StringMaster.GetString("GashaTitleLink"));
			}
			else if (gashaInfo.IsRareTicket())
			{
				base.PartsTitle.SetTitle(StringMaster.GetString("GashaTitleRare"));
			}
			else if (gashaInfo.IsLinkTicket())
			{
				base.PartsTitle.SetTitle(StringMaster.GetString("GashaTitleLink"));
			}
			this.ShowAllData(this.activeListPartsIDX);
			this.ShowCampaign(this.activeListPartsIDX);
			this.rareParts.SetActive(true);
			if (gashaInfo.IsRare() || gashaInfo.IsLink())
			{
				this.ngTX_ADVENT_CAUTION.text = StringMaster.GetString("GashaLineupCaution");
			}
			else if (gashaInfo.IsRareChip() || gashaInfo.IsLinkChip())
			{
				this.ngTX_ADVENT_CAUTION.text = StringMaster.GetString("ChipGashaLineupCaution");
			}
			else if (gashaInfo.IsRareTicket() || gashaInfo.IsLinkTicket())
			{
				this.ngTX_ADVENT_CAUTION.text = StringMaster.GetString("TicketGashaLineupCaution");
			}
			if (gashaInfo.IsLink() || gashaInfo.IsLinkChip() || gashaInfo.IsLinkTicket())
			{
				this.goCAD_ROOT.SetActive(false);
			}
			else
			{
				this.goCAD_ROOT.SetActive(true);
			}
		}
		else
		{
			global::Debug.LogError("存在しないガシャ");
		}
		List<string> list = new List<string>();
		foreach (GameWebAPI.RespDataGA_GetGachaInfo.Result result in this.gashaList)
		{
			list.Add(result.endTime);
		}
		LeadCapture.Instance.SaveCaptureUpdate(list);
		this.ShowPointData();
	}

	private void InitPanel()
	{
		Rect listWindowViewRect = default(Rect);
		listWindowViewRect.xMin = 117f;
		listWindowViewRect.xMax = 660f;
		listWindowViewRect.yMin = -240f;
		listWindowViewRect.yMax = 240f;
		this.csSelectPanelGasha.ListWindowViewRect = listWindowViewRect;
	}

	private void ShowAllData(int index)
	{
		GameWebAPI.RespDataGA_GetGachaInfo.Result result = this.gashaList[index];
		DataMng dataMng = DataMng.Instance();
		int point = dataMng.RespDataUS_PlayerInfo.playerInfo.point;
		int num = int.Parse(dataMng.RespDataUS_PlayerInfo.playerInfo.friendPoint);
		int num2 = 0;
		if (result.isFirstGacha1.total == 1)
		{
			num2 = int.Parse(result.priceFirst1);
		}
		else if (result.isFirstGacha1.today == 1)
		{
			string dailyresetFirst = result.dailyresetFirst1;
			if (dailyresetFirst == "1")
			{
				num2 = int.Parse(result.priceFirst1);
			}
			else if (dailyresetFirst == "0")
			{
				num2 = int.Parse(result.price);
			}
			else
			{
				global::Debug.LogError("dailyresetFirst1がおかしいです。");
			}
		}
		else
		{
			num2 = int.Parse(result.price);
		}
		int num3 = 0;
		if (result.isFirstGacha10.total == 1)
		{
			num3 = int.Parse(result.priceFirst10);
		}
		else if (result.isFirstGacha10.today == 1)
		{
			string dailyresetFirst2 = result.dailyresetFirst10;
			if (dailyresetFirst2 == "1")
			{
				num3 = int.Parse(result.priceFirst10);
			}
			else if (dailyresetFirst2 == "0")
			{
				num3 = int.Parse(result.priceDiscount10);
			}
			else
			{
				global::Debug.LogError("dailyresetFirst10がおかしいです。");
			}
		}
		else
		{
			num3 = int.Parse(result.priceDiscount10);
		}
		string str = string.Empty;
		if (result.IsRare() || result.IsRareChip() || result.IsRareTicket())
		{
			str = StringMaster.GetString("GashaDigistoneCost");
		}
		else if (result.IsLink() || result.IsLinkChip() || result.IsLinkTicket())
		{
			str = StringMaster.GetString("GashaLinkpointCost");
		}
		this.SingleNeedCount = num2.ToString();
		this.ngTX_EXP_SINGLE.text = str + this.SingleNeedCount;
		if (result.IsRare() || result.IsRareChip() || result.IsRareTicket())
		{
			if (point < num2)
			{
				this.ngBTN_SINGLE.spriteName = "Common02_Btn_Blue";
				this.colBTN_SINGLE.activeCollider = true;
			}
			else
			{
				this.ngBTN_SINGLE.spriteName = "Common02_Btn_Blue";
				this.colBTN_SINGLE.activeCollider = true;
			}
		}
		else if (result.IsLink() || result.IsLinkChip() || result.IsLinkTicket())
		{
			if (num < num2)
			{
				this.ngBTN_SINGLE.spriteName = "Common02_Btn_Gray";
				this.colBTN_SINGLE.activeCollider = false;
			}
			else
			{
				this.ngBTN_SINGLE.spriteName = "Common02_Btn_Blue";
				this.colBTN_SINGLE.activeCollider = true;
			}
		}
		this.TenNeedCount = num3.ToString();
		this.ngTX_EXP_TEN.text = str + this.TenNeedCount;
		if (result.IsRare() || result.IsRareChip() || result.IsRareTicket())
		{
			if (point < num3)
			{
				this.ngBTN_TEN.spriteName = "Common02_Btn_Red";
				this.colBTN_TEN.activeCollider = true;
			}
			else
			{
				this.ngBTN_TEN.spriteName = "Common02_Btn_Red";
				this.colBTN_TEN.activeCollider = true;
			}
		}
		else if (result.IsLink() || result.IsLinkChip() || result.IsLinkTicket())
		{
			if (num < num3)
			{
				this.ngBTN_TEN.spriteName = "Common02_Btn_Gray";
				this.colBTN_TEN.activeCollider = false;
			}
			else
			{
				this.ngBTN_TEN.spriteName = "Common02_Btn_Red";
				this.colBTN_TEN.activeCollider = true;
			}
		}
	}

	private void ShowCampaign(int idx)
	{
		if (GashaTutorialMode.TutoExec)
		{
			this.goCAMPAIGN_1.SetActive(false);
			this.goCAMPAIGN_10.SetActive(false);
			return;
		}
		bool flag = false;
		bool flag2 = false;
		GameWebAPI.RespDataGA_GetGachaInfo.Result result = this.gashaList[idx];
		if (result.isFirstGacha1.total == 0 && result.isFirstGacha1.today == 0)
		{
			flag = true;
		}
		if (result.isFirstGacha10.total == 0 && result.isFirstGacha10.today == 0)
		{
			flag2 = true;
		}
		if (flag)
		{
			string text = result.appealTextDisplayType1;
			switch (text)
			{
			case "0":
				result.appealText1 = string.Empty;
				break;
			case "2":
			case "3":
				result.appealText1 = string.Empty;
				break;
			}
		}
		else if (result.appealTextDisplayType1 == "0")
		{
			result.appealText1 = string.Empty;
		}
		if (flag2)
		{
			string text = result.appealTextDisplayType10;
			switch (text)
			{
			case "0":
				result.appealText10 = string.Empty;
				break;
			case "2":
			case "3":
				result.appealText10 = string.Empty;
				break;
			}
		}
		else if (result.appealTextDisplayType10 == "0")
		{
			result.appealText10 = string.Empty;
		}
		if (string.IsNullOrEmpty(result.appealText1) || result.appealText1 == "null")
		{
			this.goCAMPAIGN_1.SetActive(false);
		}
		else
		{
			this.goCAMPAIGN_1.SetActive(true);
			this.lbCAMPAIGN_1.text = result.appealText1;
		}
		if (string.IsNullOrEmpty(result.appealText10) || result.appealText10 == "null")
		{
			this.goCAMPAIGN_10.SetActive(false);
		}
		else
		{
			this.goCAMPAIGN_10.SetActive(true);
			this.lbCAMPAIGN_10.text = result.appealText10;
		}
	}

	private void ShowPointData()
	{
		GameWebAPI.RespDataUS_GetPlayerInfo.PlayerInfo playerInfo = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo;
		this.ngTX_LINK_POINT.text = playerInfo.friendPoint;
		this.ngTX_STONE_NUM.text = playerInfo.point.ToString();
		GUIPlayerStatus.RefreshParams_S(false);
	}

	private void ResetStatus(int ct)
	{
		GameWebAPI.RespDataGA_GetGachaInfo.Result result = this.gashaList[this.activeListPartsIDX];
		int num = 0;
		if (ct == 1)
		{
			if (result.isFirstGacha1.total == 1)
			{
				num = int.Parse(result.priceFirst1);
				result.isFirstGacha1.total = 0;
				result.isFirstGacha1.today = 0;
			}
			else if (result.isFirstGacha1.today == 1)
			{
				string dailyresetFirst = result.dailyresetFirst1;
				if (dailyresetFirst == "1")
				{
					num = int.Parse(result.priceFirst1);
				}
				else if (dailyresetFirst == "0")
				{
					num = int.Parse(result.price);
				}
				else
				{
					global::Debug.LogError("dailyresetFirst1がおかしいです。");
				}
				result.isFirstGacha1.today = 0;
			}
			else
			{
				num = int.Parse(result.price);
			}
		}
		else if (ct == 10)
		{
			if (result.isFirstGacha10.total == 1)
			{
				num = int.Parse(result.priceFirst10);
				result.isFirstGacha10.total = 0;
				result.isFirstGacha10.today = 0;
			}
			else if (result.isFirstGacha10.today == 1)
			{
				string dailyresetFirst2 = result.dailyresetFirst10;
				if (dailyresetFirst2 == "1")
				{
					num = int.Parse(result.priceFirst10);
				}
				else if (dailyresetFirst2 == "0")
				{
					num = int.Parse(result.priceDiscount10);
				}
				else
				{
					global::Debug.LogError("dailyresetFirst10がおかしいです。");
				}
				result.isFirstGacha10.today = 0;
			}
			else
			{
				num = int.Parse(result.priceDiscount10);
			}
		}
		else
		{
			global::Debug.LogError("単発でも10連でもないエラーです。");
		}
		if (result.IsRare() || result.IsRareChip() || result.IsRareTicket())
		{
			int num2 = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point;
			num2 -= num;
			DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point = num2;
		}
		else if (result.IsLink() || result.IsLinkChip() || result.IsLinkTicket())
		{
			int num3 = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.friendPoint);
			num3 -= num;
			DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.friendPoint = num3.ToString();
		}
		this.ShowAllData(this.activeListPartsIDX);
		if (ct == 1)
		{
			this.ShowCampaign(this.activeListPartsIDX);
		}
		else if (ct == 10)
		{
			this.ShowCampaign(this.activeListPartsIDX);
		}
		this.ShowPointData();
	}

	public void OnClickedSingle()
	{
		GameWebAPI.RespDataGA_GetGachaInfo.Result result = this.gashaList[this.activeListPartsIDX];
		bool flag = true;
		bool flag2 = true;
		if (result.IsRare() || result.IsLink())
		{
			flag = Singleton<UserDataMng>.Instance.IsOverUnitLimit(ConstValue.ENABLE_MONSTER_SPACE_TOEXEC_GASHA_1);
			flag2 = false;
		}
		else if (result.IsRareChip() || result.IsLinkChip())
		{
			flag = Singleton<UserDataMng>.Instance.IsOverChipLimit(ConstValue.ENABLE_CHIP_SPACE_TOEXEC_GASHA_1);
			flag2 = true;
		}
		else if (result.IsRareTicket() || result.IsLinkTicket())
		{
			flag = false;
		}
		if (!flag)
		{
			this.req_exec_bk = new GameWebAPI.GA_Req_ExecGacha();
			this.req_exec_bk.gachaId = int.Parse(result.gachaId);
			this.req_exec_bk.playCount = 1;
			this.req_exec_bk.itemCount = this.GetUserPoint(result.IsRare() || result.IsRareChip() || result.IsRareTicket());
			this.ShowGashaAlert(result.IsRare() || result.IsRareChip() || result.IsRareTicket(), result.gachaName, this.SingleNeedCount, "1");
		}
		else if (!flag2)
		{
			CMD_UpperLimit cmd_UpperLimit = GUIMain.ShowCommonDialog(null, "CMD_Upperlimit") as CMD_UpperLimit;
			cmd_UpperLimit.SetType(CMD_UpperLimit.MessageType.GASHA);
		}
		else
		{
			CMD_UpperlimitChip cmd_UpperlimitChip = GUIMain.ShowCommonDialog(null, "CMD_UpperlimitChip") as CMD_UpperlimitChip;
			cmd_UpperlimitChip.SetType(CMD_UpperlimitChip.MessageType.GASHA);
		}
	}

	public void OnClicked10()
	{
		GameWebAPI.RespDataGA_GetGachaInfo.Result result = this.gashaList[this.activeListPartsIDX];
		bool flag = true;
		bool flag2 = true;
		if (result.IsRare() || result.IsLink())
		{
			flag = Singleton<UserDataMng>.Instance.IsOverUnitLimit(ConstValue.ENABLE_MONSTER_SPACE_TOEXEC_GASHA_10);
			flag2 = false;
		}
		else if (result.IsRareChip() || result.IsLinkChip())
		{
			flag = Singleton<UserDataMng>.Instance.IsOverChipLimit(ConstValue.ENABLE_CHIP_SPACE_TOEXEC_GASHA_10);
			flag2 = true;
		}
		else if (result.IsRareTicket() || result.IsLinkTicket())
		{
			flag = false;
		}
		int playCount = 10;
		if (!flag)
		{
			this.req_exec_bk = new GameWebAPI.GA_Req_ExecGacha();
			this.req_exec_bk.gachaId = int.Parse(result.gachaId);
			this.req_exec_bk.playCount = playCount;
			this.req_exec_bk.itemCount = this.GetUserPoint(result.IsRare() || result.IsRareChip() || result.IsRareTicket());
			this.ShowGashaAlert(result.IsRare() || result.IsRareChip() || result.IsRareTicket(), result.gachaName, this.TenNeedCount, playCount.ToString());
		}
		else if (!flag2)
		{
			CMD_UpperLimit cmd_UpperLimit = GUIMain.ShowCommonDialog(null, "CMD_Upperlimit") as CMD_UpperLimit;
			cmd_UpperLimit.SetType(CMD_UpperLimit.MessageType.GASHA);
		}
		else
		{
			CMD_UpperlimitChip cmd_UpperlimitChip = GUIMain.ShowCommonDialog(null, "CMD_UpperlimitChip") as CMD_UpperlimitChip;
			cmd_UpperlimitChip.SetType(CMD_UpperlimitChip.MessageType.GASHA);
		}
	}

	private int GetUserPoint(bool isRare)
	{
		int result = 0;
		if (isRare)
		{
			result = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point;
		}
		else
		{
			int.TryParse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.friendPoint, out result);
		}
		return result;
	}

	private void ShowGashaAlert(bool isRare, string gashaName, string price, string gashaCount)
	{
		DataMng dataMng = DataMng.Instance();
		int point = dataMng.RespDataUS_PlayerInfo.playerInfo.point;
		int num = int.Parse(price);
		bool flag = true;
		if (point < num && isRare)
		{
			flag = false;
		}
		if (flag)
		{
			string arg = string.Empty;
			string s = string.Empty;
			if (isRare)
			{
				CMD_ChangePOP_STONE cmd_ChangePOP_STONE = GUIMain.ShowCommonDialog(null, "CMD_ChangePOP_STONE") as CMD_ChangePOP_STONE;
				cmd_ChangePOP_STONE.Title = gashaName;
				arg = MasterDataMng.Instance().RespDataMA_AssetCategoryM.GetAssetCategory("2").assetTitle;
				s = this.ngTX_STONE_NUM.text;
				cmd_ChangePOP_STONE.OnPushedYesAction = new Action(this.OnPushedGashaConfirmYesButton);
				if (GashaTutorialMode.TutoExec)
				{
					this.gashaTutorialMode.UpdateFakeExec(isRare);
					if (this.gashaTutorialMode.FakeExec)
					{
						s = price;
					}
				}
				cmd_ChangePOP_STONE.Info = string.Format(StringMaster.GetString("GashaDigistone"), arg, price, gashaCount);
				cmd_ChangePOP_STONE.SetDigistone(int.Parse(s), num);
			}
			else
			{
				CMD_ChangePOP cmd_ChangePOP = GUIMain.ShowCommonDialog(null, "CMD_ChangePOP") as CMD_ChangePOP;
				cmd_ChangePOP.Title = gashaName;
				arg = MasterDataMng.Instance().RespDataMA_AssetCategoryM.GetAssetCategory("3").assetTitle;
				s = this.ngTX_LINK_POINT.text;
				cmd_ChangePOP.OnPushedYesAction = new Action(this.OnPushedGashaConfirmYesButton);
				if (GashaTutorialMode.TutoExec)
				{
					this.gashaTutorialMode.UpdateFakeExec(isRare);
					if (this.gashaTutorialMode.FakeExec)
					{
						s = price;
					}
				}
				cmd_ChangePOP.Info = string.Format(StringMaster.GetString("GashaLinkpoint"), arg, price, gashaCount);
				cmd_ChangePOP.SetPoint(int.Parse(s), num);
			}
		}
		else
		{
			CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseConfirmShop), "CMD_Confirm") as CMD_Confirm;
			cmd_Confirm.Title = gashaName;
			cmd_Confirm.Info = StringMaster.GetString("GashaShortage");
			cmd_Confirm.BtnTextYes = StringMaster.GetString("SystemButtonGoShop");
			cmd_Confirm.BtnTextNo = StringMaster.GetString("SystemButtonClose");
		}
	}

	private void OnPushedGashaConfirmYesButton()
	{
		this.CheckExecGasha();
		CMD_ChangePOP cmd_ChangePOP = UnityEngine.Object.FindObjectOfType<CMD_ChangePOP>();
		if (null != cmd_ChangePOP)
		{
			cmd_ChangePOP.ClosePanel(true);
		}
		else
		{
			CMD_ChangePOP_STONE cmd_ChangePOP_STONE = UnityEngine.Object.FindObjectOfType<CMD_ChangePOP_STONE>();
			if (null != cmd_ChangePOP_STONE)
			{
				cmd_ChangePOP_STONE.ClosePanel(true);
			}
		}
	}

	private void CheckExecGasha()
	{
		GameWebAPI.RespDataGA_GetGachaInfo.Result result = this.gashaList[this.activeListPartsIDX];
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		if (this.gashaTutorialMode.FakeExec)
		{
			this.gachaResult = this.gashaTutorialMode.GetFakeGashaResult();
			this.EndExecGashaSuccess();
		}
		else if (result.IsRare() || result.IsLink())
		{
			AppCoroutine.Start(this.ExecGashaAPI(), false);
		}
		else if (result.IsRareChip() || result.IsLinkChip())
		{
			AppCoroutine.Start(this.ExecChipAPI(), false);
		}
		else if (result.IsRareTicket() || result.IsLinkTicket())
		{
			AppCoroutine.Start(this.ExecTicketAPI(), false);
		}
	}

	private void OnCloseConfirmShop(int idx)
	{
		if (idx == 0)
		{
			GUIMain.ShowCommonDialog(delegate(int i)
			{
				this.ShowPointData();
				if (CMD_10gashaResult.instance != null)
				{
					CMD_10gashaResult.instance.ShowPointData();
				}
				if (CMD_ChipGashaResult.instance != null)
				{
					CMD_ChipGashaResult.instance.ShowPointData();
				}
				this.csSelectPanelGashaMain.RefreshAbleCount();
			}, "CMD_Shop");
		}
	}

	public string LinkPointString
	{
		get
		{
			return this.ngTX_LINK_POINT.text;
		}
	}

	public string StoneNumString
	{
		get
		{
			return this.ngTX_STONE_NUM.text;
		}
	}

	public string NeedSingleNumString
	{
		get
		{
			return this.ngTX_EXP_SINGLE.text;
		}
	}

	public string NeedTenNumString
	{
		get
		{
			return this.ngTX_EXP_TEN.text;
		}
	}

	private void ShowUI(bool value)
	{
		base.gameObject.transform.FindChild("TitleRoot").gameObject.SetActive(value);
		base.gameObject.transform.FindChild("EFC_LEFT").gameObject.SetActive(value);
		base.gameObject.transform.FindChild("EFC_RIGHT").gameObject.SetActive(value);
		base.gameObject.transform.FindChild("EFC_BG").gameObject.SetActive(value);
		PartsMenu.instance.gameObject.SetActive(value);
		GUIFace.instance.gameObject.SetActive(value);
		GUIFaceIndicator.instance.gameObject.SetActive(value);
	}

	private IEnumerator ExecChipAPI()
	{
		GameWebAPI.RequestGA_ChipExec request = new GameWebAPI.RequestGA_ChipExec
		{
			SetSendData = delegate(GameWebAPI.GA_Req_ExecChip param)
			{
				param.gachaId = this.req_exec_bk.gachaId;
				param.playCount = this.req_exec_bk.playCount;
			},
			OnReceived = delegate(GameWebAPI.RespDataGA_ExecChip response)
			{
				this.chipResult = response;
				List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList> userChipList = this.GetUserChipList(this.chipResult.userAssetList);
				ChipDataMng.AddUserChipDataList(userChipList);
			}
		};
		yield return AppCoroutine.Start(request.RunOneTime(new Action(this.EndExecChipSuccess), delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
			this.ClosePanel(true);
		}, null), false);
		yield break;
	}

	private List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList> GetUserChipList(GameWebAPI.RespDataGA_ExecChip.UserAssetList[] userAssetS)
	{
		List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList> list = new List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>();
		for (int i = 0; i < userAssetS.Length; i++)
		{
			list.Add(new GameWebAPI.RespDataCS_ChipListLogic.UserChipList
			{
				chipId = int.Parse(userAssetS[i].assetValue),
				userChipId = int.Parse(userAssetS[i].userAssetId),
				userMonsterId = 0
			});
		}
		return list;
	}

	private void EndExecChipSuccess()
	{
		RestrictionInput.EndLoad();
		GUICollider.DisableAllCollider("=================================== ExecChipGasha::EndExecChipSuccess");
		if (!GashaTutorialMode.TutoExec)
		{
			this.isExecGasha = true;
		}
		if (this.req_exec_bk.playCount < 2)
		{
			this.ResetStatus(1);
		}
		else
		{
			this.ResetStatus(10);
		}
		CutSceneMain.UserAssetList = this.chipResult.userAssetList;
		CutSceneMain.FadeReqCutScene("Cutscenes/chip_gacha", new Action<int>(this.StartChipCutSceneCallBack), delegate(int index)
		{
			this.ShowUI(true);
			CutSceneMain.FadeReqCutSceneEnd();
			SoundMng.Instance().PlayGameBGM("bgm_202");
		}, delegate(int index)
		{
			RestrictionInput.EndLoad();
			if (this.finishedActionCutScene != null)
			{
				this.finishedActionCutScene();
				this.finishedActionCutScene = null;
			}
			if (this.finishedActionCutScene_2 != null)
			{
				this.finishedActionCutScene_2();
				this.finishedActionCutScene_2 = null;
			}
		}, null, null, 2, 1, 0.5f, 0.5f);
	}

	private void StartChipCutSceneCallBack(int i)
	{
		if (this.req_exec_bk.playCount < 2)
		{
			SoundMng.Instance().PlayGameBGM("bgm_204");
		}
		else
		{
			SoundMng.Instance().PlayGameBGM("bgm_205");
		}
		this.csSelectPanelGashaMain.RefreshAbleCount();
		GameWebAPI.RespDataGA_GetGachaInfo.Result result = this.gashaList[this.activeListPartsIDX];
		List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList> userChipList = this.GetUserChipList(this.chipResult.userAssetList);
		CMD_ChipGashaResult.UserAssetList = this.chipResult.userAssetList;
		CMD_ChipGashaResult.DataList = userChipList;
		if (result.IsRareChip())
		{
			CMD_ChipGashaResult.GashaType = ConstValue.RARE_GASHA_TYPE;
		}
		else if (result.IsLinkChip())
		{
			CMD_ChipGashaResult.GashaType = ConstValue.LINK_GASHA_TYPE;
		}
		if (CMD_ChipGashaResult.instance != null)
		{
			CMD_ChipGashaResult.instance.ReShow();
		}
		else
		{
			GUIMain.ShowCommonDialog(null, "CMD_ChipGashaResult");
		}
		GUICollider.EnableAllCollider("=================================== CMD_ExecChipGasha::StartChipCutSceneCallBack");
		CutSceneMain.SetChipGashaTex(CMD_ChipGashaResult.instance.txBG);
		this.ShowUI(false);
	}

	private IEnumerator ExecGashaAPI()
	{
		GameWebAPI.RequestGA_GashaExec request = new GameWebAPI.RequestGA_GashaExec
		{
			SetSendData = delegate(GameWebAPI.GA_Req_ExecGacha param)
			{
				param.gachaId = this.req_exec_bk.gachaId;
				param.playCount = this.req_exec_bk.playCount;
				param.itemCount = this.req_exec_bk.itemCount;
			},
			OnReceived = delegate(GameWebAPI.RespDataGA_ExecGacha response)
			{
				this.gachaResult = response;
				DataMng.Instance().AddUserMonsterList(response.userMonsterList);
			}
		};
		yield return AppCoroutine.Start(request.RunOneTime(delegate()
		{
			AppCoroutine.Start(this.GetChipSlotInfo(), false);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
			this.ClosePanel(true);
		}, null), false);
		yield break;
	}

	private IEnumerator GetChipSlotInfo()
	{
		GameWebAPI.MonsterSlotInfoListLogic request = ChipDataMng.RequestAPIMonsterSlotInfo(this.gachaResult.userMonsterList, null);
		yield return AppCoroutine.Start(request.Run(new Action(this.EndExecGashaSuccess), delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
			this.ClosePanel(true);
		}, null), false);
		yield break;
	}

	private void EndExecGashaSuccess()
	{
		if (!GashaTutorialMode.TutoExec)
		{
			this.isExecGasha = true;
		}
		GameWebAPI.RespDataGA_ExecGacha.GachaResultMonster[] userMonsterList = this.gachaResult.userMonsterList;
		if (this.req_exec_bk.playCount < 2)
		{
			this.ResetStatus(1);
		}
		else
		{
			this.ResetStatus(10);
		}
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		monsterDataMng.RefreshMonsterDataList();
		for (int i = 0; i < userMonsterList.Length; i++)
		{
			MonsterData monsterDataByUserMonsterID = monsterDataMng.GetMonsterDataByUserMonsterID(userMonsterList[i].userMonsterId, false);
			monsterDataByUserMonsterID.New = Convert.ToBoolean(userMonsterList[i].isNew);
		}
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		for (int i = 0; i < userMonsterList.Length; i++)
		{
			MonsterData monsterDataByUserMonsterID2 = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(userMonsterList[i].userMonsterId, false);
			int item = int.Parse(monsterDataByUserMonsterID2.monsterM.monsterGroupId);
			list.Add(item);
			list2.Add(int.Parse(monsterDataByUserMonsterID2.monsterMG.growStep));
		}
		Loading.Invisible();
		CutSceneMain.FadeReqCutScene("Cutscenes/Gasha", new Action<int>(this.StartCutSceneCallBack), delegate(int index)
		{
			this.ShowUI(true);
			CutSceneMain.FadeReqCutSceneEnd();
		}, delegate(int index)
		{
			RestrictionInput.EndLoad();
			if (this.finishedActionCutScene != null)
			{
				this.finishedActionCutScene();
				this.finishedActionCutScene = null;
			}
			if (this.finishedActionCutScene_2 != null)
			{
				this.finishedActionCutScene_2();
				this.finishedActionCutScene_2 = null;
			}
		}, list, list2, 2, 1, 0.5f, 0.5f);
	}

	public void SetFinishedActionCutScene(Action action)
	{
		this.finishedActionCutScene = action;
	}

	public void SetFinishedActionCutScene_2(Action action)
	{
		this.finishedActionCutScene_2 = action;
	}

	private void StartCutSceneCallBack(int i)
	{
		this.csSelectPanelGashaMain.RefreshAbleCount();
		GameWebAPI.RespDataGA_GetGachaInfo.Result result = this.gashaList[this.activeListPartsIDX];
		GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList[] userMonsterList = this.gachaResult.userMonsterList;
		CMD_10gashaResult.RewardsData = this.gachaResult.rewards;
		if (this.req_exec_bk.playCount < 1)
		{
			MonsterData monsterDataByUserMonsterID = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(userMonsterList[0].userMonsterId, false);
			CMD_CharacterDetailed.DataChg = monsterDataByUserMonsterID;
			GUIMain.ShowCommonDialog(null, "CMD_CharacterDetailed");
			if (CMD_10gashaResult.instance != null)
			{
				CMD_10gashaResult.instance.ClosePanel(true);
			}
			if (result.IsRare())
			{
				LeadReview leadReview = new LeadReview();
				leadReview.DisplayDialog(monsterDataByUserMonsterID);
			}
		}
		else
		{
			List<string> list = new List<string>();
			for (int j = 0; j < userMonsterList.Length; j++)
			{
				list.Add(userMonsterList[j].userMonsterId);
			}
			List<MonsterData> monsterDataListByUserMonsterIDList = MonsterDataMng.Instance().GetMonsterDataListByUserMonsterIDList(list);
			CMD_10gashaResult.DataList = monsterDataListByUserMonsterIDList;
			if (result.IsRare())
			{
				CMD_10gashaResult.GashaType = ConstValue.RARE_GASHA_TYPE;
			}
			else if (result.IsLink())
			{
				CMD_10gashaResult.GashaType = ConstValue.LINK_GASHA_TYPE;
			}
			if (CMD_10gashaResult.instance != null)
			{
				CMD_10gashaResult.instance.ReShow();
			}
			else
			{
				GUIMain.ShowCommonDialog(null, "CMD_10gashaResult");
			}
			if (result.IsRare())
			{
				LeadReview leadReview2 = new LeadReview();
				leadReview2.DisplayDialog(monsterDataListByUserMonsterIDList);
			}
		}
		this.ShowUI(false);
	}

	private IEnumerator ExecTicketAPI()
	{
		GameWebAPI.RequestGA_TicketExec request = new GameWebAPI.RequestGA_TicketExec
		{
			SetSendData = delegate(GameWebAPI.GA_Req_ExecTicket param)
			{
				param.gachaId = this.req_exec_bk.gachaId;
				param.playCount = this.req_exec_bk.playCount;
			},
			OnReceived = delegate(GameWebAPI.RespDataGA_ExecTicket response)
			{
				this.ticketResult = response;
			}
		};
		yield return AppCoroutine.Start(request.RunOneTime(new Action(this.EndExecTicketSuccess), delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
			this.ClosePanel(true);
		}, null), false);
		yield break;
	}

	private void EndExecTicketSuccess()
	{
		RestrictionInput.EndLoad();
		GUICollider.DisableAllCollider("=================================== ExecTicketGasha::EndExecTicketSuccess");
		if (!GashaTutorialMode.TutoExec)
		{
			this.isExecGasha = true;
		}
		if (this.req_exec_bk.playCount < 2)
		{
			this.ResetStatus(1);
		}
		else
		{
			this.ResetStatus(10);
		}
		CutSceneMain.UserTicketList = this.ticketResult.userDungeonTicketList;
		CutSceneMain.FadeReqCutScene("Cutscenes/ticketGacha", new Action<int>(this.StartTicketCutSceneCallBack), delegate(int index)
		{
			this.ShowUI(true);
			CutSceneMain.FadeReqCutSceneEnd();
			SoundMng.Instance().PlayGameBGM("bgm_202");
		}, delegate(int index)
		{
			RestrictionInput.EndLoad();
			if (this.finishedActionCutScene != null)
			{
				this.finishedActionCutScene();
				this.finishedActionCutScene = null;
			}
			if (this.finishedActionCutScene_2 != null)
			{
				this.finishedActionCutScene_2();
				this.finishedActionCutScene_2 = null;
			}
		}, null, null, 2, 1, 0.5f, 0.5f);
	}

	private void StartTicketCutSceneCallBack(int i)
	{
		if (this.req_exec_bk.playCount < 2)
		{
			SoundMng.Instance().PlayGameBGM("bgm_204");
		}
		else
		{
			SoundMng.Instance().PlayGameBGM("bgm_205");
		}
		this.csSelectPanelGashaMain.RefreshAbleCount();
		GameWebAPI.RespDataGA_GetGachaInfo.Result result = this.gashaList[this.activeListPartsIDX];
		CMD_TicketGashaResult.UserDungeonTicketList = this.ticketResult.userDungeonTicketList;
		if (result.IsRareTicket())
		{
			CMD_TicketGashaResult.GashaType = ConstValue.RARE_GASHA_TYPE;
		}
		else if (result.IsLinkTicket())
		{
			CMD_TicketGashaResult.GashaType = ConstValue.LINK_GASHA_TYPE;
		}
		if (CMD_TicketGashaResult.instance != null)
		{
			CMD_TicketGashaResult.instance.ReShow();
		}
		else
		{
			GUIMain.ShowCommonDialog(null, "CMD_TicketGashaResult");
		}
		GUICollider.EnableAllCollider("=================================== CMD_ExecTicketGasha::StartTicketCutSceneCallBack");
		CutSceneMain.SetTicketGashaTex(CMD_TicketGashaResult.instance.txBG);
		this.ShowUI(false);
	}

	public enum MODE
	{
		RARE = 1,
		LINK
	}
}
