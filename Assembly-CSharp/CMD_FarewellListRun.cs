using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_FarewellListRun : CMD
{
	private const int FAREWELL_LIMIT = 10;

	private readonly string eggFlagString = "1";

	private List<MonsterData> selectedMonsterDataList;

	[SerializeField]
	private List<GameObject> goMN_ICON_LIST;

	[SerializeField]
	private GameObject goTX_MN_HAVE;

	[SerializeField]
	private GameObject goTX_SORT_DISP;

	[SerializeField]
	private UILabel cointCount;

	private UILabel ngTX_MN_HAVE;

	[SerializeField]
	private UILabel ngTX_MN_HAVE_CHILD;

	[SerializeField]
	private UILabel ngTX_MN_HAVE_ADULT;

	private int growingNum;

	[SerializeField]
	private GameObject goBT_MODE_CHG;

	[SerializeField]
	private GameObject goTX_MODE_CHG;

	[SerializeField]
	private GameObject goBT_SELL;

	[Header("お別れの実行のボタンのラベル")]
	[SerializeField]
	private UILabelEx cellBtnLabel;

	[SerializeField]
	private GameObject goBT_SELL_CHIP;

	[SerializeField]
	private GameObject goTX_CHIP_GET;

	private UILabel ngTX_CHIP_GET;

	[SerializeField]
	private UISprite showBtnSprite;

	[SerializeField]
	private GUICollider showBtnCollider;

	[Header("一覧ボタンのラベル")]
	[SerializeField]
	private UILabelEx showBtnLabel;

	[SerializeField]
	private UISprite farewellBtnSprite;

	[SerializeField]
	private GUICollider farewellBtnCollider;

	[Header("お別れボタンのラベル")]
	[SerializeField]
	private UILabelEx farewellBtnLabel;

	[SerializeField]
	private UILabel cautionLabel;

	private MonsterData selectedMonsterData;

	private bool isOfflineModeFlag;

	private List<MonsterData> deckMDList;

	private GameObject goSelectPanelMonsterIcon;

	private GUISelectPanelMonsterIcon csSelectPanelMonsterIcon;

	private int chip_bak;

	private List<MonsterData> data_matL = new List<MonsterData>();

	public static CMD_FarewellListRun.MODE Mode { private get; set; }

	protected override void Awake()
	{
		base.Awake();
		for (int i = 0; i < this.goMN_ICON_LIST.Count; i++)
		{
			this.goMN_ICON_LIST[i].SetActive(false);
		}
		this.ngTX_MN_HAVE = this.goTX_MN_HAVE.GetComponent<UILabel>();
		this.ngTX_CHIP_GET = this.goTX_CHIP_GET.GetComponent<UILabel>();
		PartyUtil.ActMIconShort = new Action<MonsterData>(this.ActMIconShort);
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		this.SetCommonUI();
		this.InitMonsterList(true);
		this.ChangeMode();
		this.ShowHaveMonster();
		base.Show(f, sizeX, sizeY, aT);
		RestrictionInput.EndLoad();
	}

	protected override void Update()
	{
		base.Update();
		this.UpdateChipGet();
	}

	public override void ClosePanel(bool animation = true)
	{
		base.ClosePanel(animation);
	}

	protected override void WindowClosed()
	{
		this.csSelectPanelMonsterIcon.PARTS_CT_MN = 4;
		if (MonsterDataMng.Instance() != null)
		{
			MonsterDataMng.Instance().PushBackAllMonsterPrefab();
		}
		base.WindowClosed();
	}

	private void OnTouchSort()
	{
	}

	private void OnTouchMode()
	{
		if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.SHOW)
		{
			CMD_FarewellListRun.Mode = CMD_FarewellListRun.MODE.SELL;
		}
		else if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.SELL)
		{
			CMD_FarewellListRun.Mode = CMD_FarewellListRun.MODE.SHOW;
		}
		else if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN)
		{
			CMD_FarewellListRun.Mode = CMD_FarewellListRun.MODE.GARDEN_SELL;
		}
		else if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN_SELL)
		{
			CMD_FarewellListRun.Mode = CMD_FarewellListRun.MODE.GARDEN;
		}
		this.ChangeMode();
	}

	private void OnTouchSell()
	{
		CMD_SaleCheck cmd_SaleCheck = GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseSale), "CMD_SaleCheck") as CMD_SaleCheck;
		cmd_SaleCheck.SetParams(this.data_matL, this.ngTX_CHIP_GET.text);
	}

	private void OnCloseSale(int idx)
	{
		if (idx == 0)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
			GameWebAPI.MN_Req_Sale req = new GameWebAPI.MN_Req_Sale();
			req.saleMonsterDataList = new GameWebAPI.MN_Req_Sale.SaleMonsterDataList[this.data_matL.Count];
			for (int i = 0; i < this.data_matL.Count; i++)
			{
				int userMonsterId = int.Parse(this.data_matL[i].userMonster.userMonsterId);
				GameWebAPI.MN_Req_Sale.SaleMonsterDataList saleMonsterDataList = new GameWebAPI.MN_Req_Sale.SaleMonsterDataList();
				saleMonsterDataList.userMonsterId = userMonsterId;
				req.saleMonsterDataList[i] = saleMonsterDataList;
			}
			this.chip_bak = this.CalcChipGet();
			GameWebAPI.RespDataMN_SaleExec response = null;
			GameWebAPI.RequestMN_MonsterSale request = new GameWebAPI.RequestMN_MonsterSale
			{
				SetSendData = delegate(GameWebAPI.MN_Req_Sale param)
				{
					param.saleMonsterDataList = req.saleMonsterDataList;
				},
				OnReceived = delegate(GameWebAPI.RespDataMN_SaleExec res)
				{
					response = res;
				}
			};
			base.StartCoroutine(request.Run(delegate()
			{
				RestrictionInput.EndLoad();
				this.EndSale(response);
			}, delegate(Exception noop)
			{
				RestrictionInput.EndLoad();
			}, null));
		}
	}

	private void EndSale(GameWebAPI.RespDataMN_SaleExec response)
	{
		int[] array = new int[this.data_matL.Count];
		for (int i = 0; i < this.data_matL.Count; i++)
		{
			array[i] = int.Parse(this.data_matL[i].userMonster.userMonsterId);
		}
		DataMng.Instance().DeleteUserMonsterList(array);
		MonsterDataMng.Instance().RefreshMonsterDataList();
		this.InitMonsterList(false);
		if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.SELL)
		{
			this.SetDimParty(true);
		}
		else if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN_SELL)
		{
			this.SetDimEgg(true);
			this.SetDimGrowing(true);
		}
		this.CheckDimPartners(true);
		MonsterDataMng.Instance().SetIconGrayOut("1", GUIMonsterIcon.DIMM_LEVEL.DISABLE, null);
		this.data_matL = new List<MonsterData>();
		this.ShowHaveMonster();
		int num = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney);
		int num2 = num + this.chip_bak;
		DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney = num2.ToString();
		this.BTSeleOn();
		this.UpdateDigicoin();
	}

	private void ChangeMode()
	{
		if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.SHOW || CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN)
		{
			this.showBtnSprite.spriteName = "Common02_Btn_BaseG";
			this.showBtnCollider.activeCollider = false;
			this.showBtnLabel.color = Util.convertColor(180f, 180f, 180f, 255f);
			this.farewellBtnSprite.spriteName = "Common02_Btn_BaseON";
			this.farewellBtnCollider.activeCollider = true;
			this.farewellBtnLabel.color = Color.white;
			base.PartsTitle.SetTitle(StringMaster.GetString("SystemList"));
			this.goBT_SELL.SetActive(false);
			this.goBT_SELL_CHIP.SetActive(false);
			this.CheckDimPartners(false);
			if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.SHOW)
			{
				this.SetDimParty(false);
			}
			else if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN)
			{
				this.SetDimEgg(true);
				this.SetDimGrowing(true);
			}
			this.ReleaseAllMat();
			MonsterDataMng.Instance().SetIconGrayOut("1", GUIMonsterIcon.DIMM_LEVEL.ACTIVE, new Action<MonsterData>(this.ActMIconShort));
		}
		else if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.SELL || CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN_SELL)
		{
			this.ReleaseSelectedMonsterData();
			this.showBtnSprite.spriteName = "Common02_Btn_BaseON";
			this.showBtnCollider.activeCollider = true;
			this.showBtnLabel.color = Color.white;
			this.farewellBtnSprite.spriteName = "Common02_Btn_BaseG";
			this.farewellBtnCollider.activeCollider = false;
			this.farewellBtnLabel.color = Util.convertColor(180f, 180f, 180f, 255f);
			base.PartsTitle.SetTitle(StringMaster.GetString("SaleTitle"));
			this.goBT_SELL.SetActive(true);
			this.goBT_SELL_CHIP.SetActive(true);
			this.CheckDimPartners(true);
			if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.SELL)
			{
				this.SetDimParty(true);
			}
			else if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN_SELL)
			{
				this.SetDimEgg(true);
				this.SetDimGrowing(true);
			}
			this.ReleaseAllMat();
			this.BTSeleOn();
			MonsterDataMng.Instance().SetIconGrayOut("1", GUIMonsterIcon.DIMM_LEVEL.DISABLE, null);
		}
	}

	private void BTSeleOn()
	{
		UISprite component = this.goBT_SELL.GetComponent<UISprite>();
		if (this.data_matL.Count > 0)
		{
			this.goBT_SELL.GetComponent<GUICollider>().activeCollider = true;
			component.spriteName = "Common02_Btn_Red";
			this.cellBtnLabel.color = Color.white;
		}
		else
		{
			this.goBT_SELL.GetComponent<GUICollider>().activeCollider = false;
			component.spriteName = "Common02_Btn_Gray";
			this.cellBtnLabel.color = ConstValue.DEACTIVE_BUTTON_LABEL;
		}
	}

	private void SetCommonUI()
	{
		this.UpdateDigicoin();
		this.goSelectPanelMonsterIcon = GUIManager.LoadCommonGUI("SelectListPanel/SelectListPanelMonsterIconL", base.gameObject);
		this.csSelectPanelMonsterIcon = this.goSelectPanelMonsterIcon.GetComponent<GUISelectPanelMonsterIcon>();
		if (this.goEFC_FOOTER != null)
		{
			this.goSelectPanelMonsterIcon.transform.parent = this.goEFC_FOOTER.transform;
		}
		Vector3 localPosition = this.goSelectPanelMonsterIcon.transform.localPosition;
		localPosition.x = -108.5f;
		GUICollider component = this.goSelectPanelMonsterIcon.GetComponent<GUICollider>();
		component.SetOriginalPos(localPosition);
		Rect listWindowViewRect = default(Rect);
		listWindowViewRect.xMin = -480f;
		listWindowViewRect.xMax = 480f;
		listWindowViewRect.yMin = -297f - GUIMain.VerticalSpaceSize;
		listWindowViewRect.yMax = 158f + GUIMain.VerticalSpaceSize;
		this.csSelectPanelMonsterIcon.ListWindowViewRect = listWindowViewRect;
		this.csSelectPanelMonsterIcon.PARTS_CT_MN = 7;
	}

	private void UpdateDigicoin()
	{
		GameWebAPI.RespDataUS_GetPlayerInfo.PlayerInfo playerInfo = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo;
		this.cointCount.text = StringFormat.Cluster(playerInfo.gamemoney);
	}

	private void InitMonsterList(bool initLoc = true)
	{
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		monsterDataMng.ClearSortMessAll();
		monsterDataMng.ClearLevelMessAll();
		List<MonsterData> list = monsterDataMng.GetMonsterDataList(false);
		this.ngTX_MN_HAVE_ADULT.text = MonsterDataMng.Instance().SelectMonsterDataListCount(list, MonsterDataMng.SELECT_TYPE.ALL_OUT_GARDEN).ToString();
		int num = MonsterDataMng.Instance().SelectMonsterDataListCount(list, MonsterDataMng.SELECT_TYPE.ALL_IN_GARDEN);
		this.ngTX_MN_HAVE_CHILD.text = num.ToString();
		this.growingNum = MonsterDataMng.Instance().SelectMonsterDataListCount(list, MonsterDataMng.SELECT_TYPE.GROWING_IN_GARDEN);
		if ((CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN || CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN_SELL) && num == 0)
		{
			this.cautionLabel.gameObject.SetActive(true);
			this.cautionLabel.text = StringMaster.GetString("Garden-02");
		}
		else if (this.cautionLabel.gameObject.activeSelf)
		{
			this.cautionLabel.gameObject.SetActive(false);
		}
		list = monsterDataMng.SelectMonsterDataList(list, (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN || CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN_SELL) ? MonsterDataMng.SELECT_TYPE.ALL_IN_GARDEN : MonsterDataMng.SELECT_TYPE.ALL_OUT_GARDEN);
		list = monsterDataMng.SortMDList(list, false);
		this.csSelectPanelMonsterIcon.initLocation = initLoc;
		Vector3 localScale = this.goMN_ICON_LIST[0].transform.localScale;
		monsterDataMng.SetDimmAll(GUIMonsterIcon.DIMM_LEVEL.ACTIVE);
		monsterDataMng.SetSelectOffAll();
		monsterDataMng.ClearDimmMessAll();
		this.csSelectPanelMonsterIcon.useLocationRecord = true;
		this.csSelectPanelMonsterIcon.AllBuild(list, localScale, new Action<MonsterData>(this.ActMIconLong), new Action<MonsterData>(this.ActMIconShort), false);
		this.csSelectPanelMonsterIcon.ScrollBarPosX = 458f;
		this.csSelectPanelMonsterIcon.ScrollBarBGPosX = 458f;
	}

	private void ActMIconShort(MonsterData tappedMonsterData)
	{
		if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.SHOW)
		{
			this.ReleaseSelectedMonsterData();
			this.selectedMonsterData = tappedMonsterData;
			CMD_CharacterDetailed.DataChg = tappedMonsterData;
			this.ShowMonsterDetailForList(tappedMonsterData);
		}
		else if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.SELL || CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN_SELL)
		{
			if (this.data_matL.Count < 10)
			{
				this.data_matL.Add(tappedMonsterData);
				tappedMonsterData.dimmLevel = GUIMonsterIcon.DIMM_LEVEL.NOTACTIVE;
				tappedMonsterData.selectNum = 0;
				GUIMonsterIcon monsterCS_ByMonsterData = MonsterDataMng.Instance().GetMonsterCS_ByMonsterData(tappedMonsterData);
				monsterCS_ByMonsterData.DimmLevel = tappedMonsterData.dimmLevel;
				monsterCS_ByMonsterData.SelectNum = tappedMonsterData.selectNum;
				monsterCS_ByMonsterData.SetTouchAct_S(new Action<MonsterData>(this.ActMIconS_Remove));
				this.RefreshSelectedInMonsterList();
				this.BTSeleOn();
				if (this.data_matL.Count == 10)
				{
					this.SetOtherDimIcon(true);
				}
			}
			MonsterDataMng.Instance().SetIconGrayOut("1", GUIMonsterIcon.DIMM_LEVEL.DISABLE, null);
		}
		else if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN)
		{
			this.selectedMonsterData = tappedMonsterData;
			CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.CallbackConfirmMove), "CMD_Confirm") as CMD_Confirm;
			cmd_Confirm.Title = StringMaster.GetString("Garden-03");
			cmd_Confirm.Info = StringMaster.GetString("Garden-04");
			cmd_Confirm.BtnTextYes = StringMaster.GetString("SystemButtonYes");
			cmd_Confirm.BtnTextNo = StringMaster.GetString("SystemButtonNo");
		}
	}

	private void CallbackConfirmMove(int selectButtonIndex)
	{
		if (selectButtonIndex == 0)
		{
			if (this.growingNum < 3)
			{
				if (CMD_DigiGarden.instance != null && this.isOfflineModeFlag)
				{
					this.OfflineMoveDigiGarden();
				}
				else
				{
					base.StartCoroutine(this.MoveDigiGarden());
				}
			}
			else
			{
				this.selectedMonsterData = null;
				CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
				cmd_ModalMessage.Title = StringMaster.GetString("Garden-03");
				cmd_ModalMessage.Info = StringMaster.GetString("GardenGrowMax");
			}
		}
		else
		{
			this.selectedMonsterData = null;
		}
	}

	private IEnumerator MoveDigiGarden()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
		yield return base.StartCoroutine(this.RequestMoveDigiGarden(false).Run(delegate
		{
			this.selectedMonsterData = null;
			base.SetCloseAction(delegate(int index)
			{
				if (CMD_DigiGarden.instance == null)
				{
					if (CMD_GashaTOP.instance != null)
					{
						CMD_GashaTOP.instance.ClosePanel(true);
						AppCoroutine.Start(this.OpenDigiGarden(CMD_GashaTOP.instance), false);
					}
					else if (CMD_QuestTOP.instance != null)
					{
						CMD_QuestTOP.instance.ClosePanel(true);
						AppCoroutine.Start(this.OpenDigiGarden(CMD_QuestTOP.instance), false);
					}
				}
				else
				{
					CMD_DigiGarden.instance.DestroyRender3DRT();
					MonsterDataMng.Instance().RefreshMonsterDataList();
					CMD_DigiGarden.instance.InitMonsterList(true);
				}
			});
		}, delegate(Exception nop)
		{
			if (null != CMD_DigiGarden.instance && CMD_DigiGarden.instance.GetActionStatus() == CommonDialog.ACT_STATUS.OPEN)
			{
				base.SetCloseAction(delegate(int index)
				{
					CMD_DigiGarden.instance.ClosePanel(true);
				});
			}
		}, null));
		RestrictionInput.EndLoad();
		this.ClosePanel(true);
		yield break;
	}

	private APIRequestTask RequestMoveDigiGarden(bool requestRetry = true)
	{
		GameWebAPI.RequestMN_MoveDigiGarden requestMN_MoveDigiGarden = new GameWebAPI.RequestMN_MoveDigiGarden();
		requestMN_MoveDigiGarden.SetSendData = delegate(GameWebAPI.MN_Req_MoveDigiGarden param)
		{
			param.userMonsterId = this.selectedMonsterData.userMonster.userMonsterId;
		};
		requestMN_MoveDigiGarden.OnReceived = delegate(GameWebAPI.RespDataMN_MoveDigiGarden response)
		{
			DataMng.Instance().SetUserMonster(response.userMonster);
		};
		GameWebAPI.RequestMN_MoveDigiGarden request = requestMN_MoveDigiGarden;
		return new APIRequestTask(request, requestRetry);
	}

	private IEnumerator OpenDigiGarden(CMD waitCloseWindow)
	{
		while (null != waitCloseWindow && waitCloseWindow.GetActionStatus() != CommonDialog.ACT_STATUS.CLOSED)
		{
			yield return null;
		}
		GUIMain.ShowCommonDialog(null, "CMD_DigiGarden");
		yield break;
	}

	private void OfflineMoveDigiGarden()
	{
		List<MonsterData> selectMonsterDataList = MonsterDataMng.Instance().GetSelectMonsterDataList();
		foreach (MonsterData monsterData in selectMonsterDataList)
		{
			monsterData.userMonster.growEndDate = ServerDateTime.Now.ToString();
		}
		CMD_DigiGarden.instance.InitMonsterList(true);
		this.ClosePanel(true);
	}

	private void ActMIconS_Remove(MonsterData md)
	{
		for (int i = 0; i < this.data_matL.Count; i++)
		{
			if (this.data_matL[i] == md)
			{
				this.data_matL.RemoveAt(i);
				break;
			}
		}
		this.SetOtherDimIcon(false);
		md.dimmLevel = GUIMonsterIcon.DIMM_LEVEL.ACTIVE;
		md.selectNum = -1;
		GUIMonsterIcon monsterCS_ByMonsterData = MonsterDataMng.Instance().GetMonsterCS_ByMonsterData(md);
		monsterCS_ByMonsterData.DimmLevel = md.dimmLevel;
		monsterCS_ByMonsterData.SelectNum = md.selectNum;
		monsterCS_ByMonsterData.SetTouchAct_S(new Action<MonsterData>(this.ActMIconShort));
		this.RefreshSelectedInMonsterList();
		this.BTSeleOn();
		MonsterDataMng.Instance().SetIconGrayOut("1", GUIMonsterIcon.DIMM_LEVEL.DISABLE, null);
	}

	private void ShowMonsterDetailForList(MonsterData monsterData)
	{
		GUIMain.ShowCommonDialog(delegate(int i)
		{
			PartyUtil.SetLock(monsterData, false);
		}, "CMD_CharacterDetailed");
	}

	private void ActMIconLong(MonsterData tappedMonsterData)
	{
		if (this.isOfflineModeFlag)
		{
			return;
		}
		CMD_CharacterDetailed.DataChg = tappedMonsterData;
		if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.SHOW || CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN)
		{
			this.ShowMonsterDetailForList(tappedMonsterData);
			return;
		}
		bool flag = false;
		bool isCheckDim = true;
		if (this.selectedMonsterDataList != null && this.selectedMonsterDataList.Count != 0)
		{
			foreach (MonsterData monsterData in this.selectedMonsterDataList)
			{
				if (monsterData == tappedMonsterData)
				{
					flag = true;
					isCheckDim = false;
				}
			}
		}
		List<MonsterData> deckMonsterDataList = MonsterDataMng.Instance().GetDeckMonsterDataList(false);
		foreach (MonsterData monsterData2 in deckMonsterDataList)
		{
			if (monsterData2 == tappedMonsterData)
			{
				isCheckDim = false;
			}
		}
		CMD_CharacterDetailed cmd_CharacterDetailed = GUIMain.ShowCommonDialog(delegate(int i)
		{
			PartyUtil.SetLock(tappedMonsterData, isCheckDim);
			this.SetDimEgg(true);
			this.SetDimGrowing(true);
			if (this.data_matL.Count == 10)
			{
				this.SetOtherDimIcon(true);
			}
			if (CMD_FarewellListRun.Mode != CMD_FarewellListRun.MODE.SELL && CMD_FarewellListRun.Mode != CMD_FarewellListRun.MODE.GARDEN_SELL)
			{
				MonsterDataMng.Instance().SetIconGrayOut("1", GUIMonsterIcon.DIMM_LEVEL.ACTIVE, new Action<MonsterData>(this.ActMIconShort));
			}
			else
			{
				MonsterDataMng.Instance().SetIconGrayOut("1", GUIMonsterIcon.DIMM_LEVEL.DISABLE, null);
			}
		}, "CMD_CharacterDetailed") as CMD_CharacterDetailed;
		if (flag)
		{
			cmd_CharacterDetailed.Mode = CMD_CharacterDetailed.LockMode.Farewell;
		}
	}

	private void ReleaseSelectedMonsterData()
	{
		if (this.selectedMonsterData != null)
		{
			PartyUtil.SetDimIcon(false, this.selectedMonsterData, string.Empty, false);
		}
	}

	private void ReleaseAllMat()
	{
		for (int i = this.data_matL.Count - 1; i >= 0; i--)
		{
			MonsterData monsterData = this.data_matL[i];
			monsterData.dimmLevel = GUIMonsterIcon.DIMM_LEVEL.ACTIVE;
			monsterData.selectNum = -1;
			GUIMonsterIcon monsterCS_ByMonsterData = MonsterDataMng.Instance().GetMonsterCS_ByMonsterData(monsterData);
			monsterCS_ByMonsterData.DimmLevel = monsterData.dimmLevel;
			monsterCS_ByMonsterData.SelectNum = monsterData.selectNum;
			monsterCS_ByMonsterData.SetTouchAct_S(new Action<MonsterData>(this.ActMIconShort));
			this.data_matL.RemoveAt(i);
		}
	}

	private void SetDimParty(bool flg)
	{
		this.deckMDList = MonsterDataMng.Instance().GetDeckMonsterDataList(false);
		foreach (MonsterData deckMonsterData in this.deckMDList)
		{
			if (flg)
			{
				PartyUtil.SetDimIcon(true, deckMonsterData, StringMaster.GetString("CharaIcon-04"), false);
			}
			else
			{
				PartyUtil.SetDimIcon(false, deckMonsterData, string.Empty, false);
			}
		}
	}

	private void SetDimEgg(bool flg)
	{
		List<MonsterData> selectMonsterDataList = MonsterDataMng.Instance().GetSelectMonsterDataList();
		foreach (MonsterData monsterData in selectMonsterDataList)
		{
			if (monsterData.userMonster.eggFlg == this.eggFlagString)
			{
				PartyUtil.SetDimIcon(flg, monsterData, StringMaster.GetString("CharaIcon-03"), false);
			}
		}
	}

	private void SetDimGrowing(bool flg)
	{
		List<MonsterData> selectMonsterDataList = MonsterDataMng.Instance().GetSelectMonsterDataList();
		foreach (MonsterData monsterData in selectMonsterDataList)
		{
			if (!string.IsNullOrEmpty(monsterData.userMonster.growEndDate))
			{
				PartyUtil.SetDimIcon(flg, monsterData, StringMaster.GetString("CharaIcon-03"), false);
			}
		}
	}

	private void CheckDimPartners(bool isDim)
	{
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		if (isDim)
		{
			List<MonsterData> monsterDataList = monsterDataMng.GetMonsterDataList(false);
			foreach (MonsterData monsterData in monsterDataList)
			{
				if (monsterData.userMonster.IsLocked)
				{
					PartyUtil.SetDimIcon(true, monsterData, string.Empty, true);
				}
			}
		}
		else
		{
			List<MonsterData> monsterDataList2 = monsterDataMng.GetMonsterDataList(false);
			foreach (MonsterData deckMonsterData in monsterDataList2)
			{
				PartyUtil.SetDimIcon(false, deckMonsterData, string.Empty, false);
			}
		}
	}

	private void RefreshSelectedInMonsterList()
	{
		this.selectedMonsterDataList = new List<MonsterData>();
		int snum = 1;
		for (int i = 0; i < this.data_matL.Count; i++)
		{
			this.selectedMonsterDataList.Add(this.data_matL[i]);
		}
		MonsterDataMng.Instance().SetSelectByMonsterDataList(this.selectedMonsterDataList, snum, true);
	}

	private void UpdateChipGet()
	{
		int value = this.CalcChipGet();
		this.ngTX_CHIP_GET.text = StringFormat.Cluster(value);
	}

	private int CalcChipGet()
	{
		if (this.data_matL == null)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < this.data_matL.Count; i++)
		{
			num += this.data_matL[i].Now_Price(-1);
		}
		for (int j = 0; j < this.data_matL.Count; j++)
		{
			if (this.data_matL[j].userMonsterSlotInfo != null && this.data_matL[j].userMonsterSlotInfo.equip != null)
			{
				foreach (GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip equip2 in this.data_matL[j].userMonsterSlotInfo.equip)
				{
					GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChipDataByUserChipId = ChipDataMng.GetUserChipDataByUserChipId(equip2.userChipId);
					GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(userChipDataByUserChipId.chipId.ToString());
					num += chipMainData.GetSellPrice();
				}
			}
		}
		return num;
	}

	private void ShowHaveMonster()
	{
		List<MonsterData> selectMonsterDataList = MonsterDataMng.Instance().GetSelectMonsterDataList();
		int num = selectMonsterDataList.Count;
		int num2 = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.unitLimitMax);
		this.growingNum = MonsterDataMng.Instance().SelectMonsterDataListCount(selectMonsterDataList, MonsterDataMng.SELECT_TYPE.GROWING_IN_GARDEN);
		if (CMD_FarewellListRun.Mode != CMD_FarewellListRun.MODE.GARDEN && CMD_FarewellListRun.Mode != CMD_FarewellListRun.MODE.GARDEN_SELL)
		{
			this.ngTX_MN_HAVE_ADULT.text = num.ToString();
			num += int.Parse(this.ngTX_MN_HAVE_CHILD.text);
		}
		else
		{
			this.ngTX_MN_HAVE_CHILD.text = num.ToString();
			num += int.Parse(this.ngTX_MN_HAVE_ADULT.text);
		}
		this.ngTX_MN_HAVE.text = string.Format(StringMaster.GetString("SystemFraction"), num.ToString(), num2.ToString());
	}

	private void SetOtherDimIcon(bool isDim)
	{
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		List<MonsterData> monsterDataList = monsterDataMng.GetMonsterDataList(false);
		foreach (MonsterData monsterData in monsterDataList)
		{
			if (!this.data_matL.Contains(monsterData) && (CMD_FarewellListRun.Mode != CMD_FarewellListRun.MODE.SELL || !this.deckMDList.Contains(monsterData)) && (CMD_FarewellListRun.Mode != CMD_FarewellListRun.MODE.GARDEN_SELL || string.IsNullOrEmpty(monsterData.userMonster.growEndDate)) && (CMD_FarewellListRun.Mode != CMD_FarewellListRun.MODE.GARDEN_SELL || !(monsterData.userMonster.eggFlg == this.eggFlagString)))
			{
				if (monsterData.userMonster.IsLocked)
				{
					PartyUtil.SetDimIcon(true, monsterData, string.Empty, monsterData.Lock);
				}
				else
				{
					PartyUtil.SetDimIcon(isDim, monsterData, string.Empty, monsterData.Lock);
				}
			}
		}
	}

	public void SetOfflineMode(bool isOfflineMode)
	{
		this.isOfflineModeFlag = isOfflineMode;
	}

	public enum MODE
	{
		SHOW,
		SELL,
		GARDEN,
		GARDEN_SELL
	}
}
