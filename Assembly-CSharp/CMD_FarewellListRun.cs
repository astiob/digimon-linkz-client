using Master;
using Monster;
using MonsterList.HouseGarden;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class CMD_FarewellListRun : CMD
{
	private const int FAREWELL_LIMIT = 10;

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

	[SerializeField]
	[Header("お別れの実行のボタンのラベル")]
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

	[SerializeField]
	[Header("一覧ボタンのラベル")]
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

	private BtnSort sortButton;

	private bool isOfflineModeFlag;

	private GameObject goSelectPanelMonsterIcon;

	private GUISelectPanelMonsterIcon csSelectPanelMonsterIcon;

	private int chip_bak;

	private List<MonsterData> sellMonsterList;

	private List<MonsterData> targetMonsterList;

	private HouseGardenIconGrayOut iconGrayOut;

	private HouseGardenMonsterList monsterList;

	private bool isReceived;

	public static CMD_FarewellListRun.MODE Mode { private get; set; }

	protected override void Awake()
	{
		base.Awake();
		this.sellMonsterList = new List<MonsterData>();
		this.isReceived = false;
		this.iconGrayOut = new HouseGardenIconGrayOut();
		this.iconGrayOut.SetNormalAction(new Action<MonsterData>(this.ActMIconShort), new Action<MonsterData>(this.ActMIconLong));
		this.iconGrayOut.SetSelectedAction(new Action<MonsterData>(this.ActMIconS_Remove), new Action<MonsterData>(this.ActMIconLong));
		this.iconGrayOut.SetBlockAction(null, new Action<MonsterData>(this.ActMIconLong));
		this.monsterList = new HouseGardenMonsterList();
		this.monsterList.Initialize(ClassSingleton<MonsterUserDataMng>.Instance.GetDeckUserMonsterList(), ClassSingleton<MonsterUserDataMng>.Instance.GetColosseumDeckUserMonsterList(), this.sellMonsterList, this.iconGrayOut);
		for (int i = 0; i < this.goMN_ICON_LIST.Count; i++)
		{
			this.goMN_ICON_LIST[i].SetActive(false);
		}
		this.ngTX_MN_HAVE = this.goTX_MN_HAVE.GetComponent<UILabel>();
		this.ngTX_CHIP_GET = this.goTX_CHIP_GET.GetComponent<UILabel>();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		this.SetCommonUI();
		this.InitMonsterList(true);
		this.ChangeMode();
		this.ShowHaveMonster();
		base.SetTutorialAnyTime("anytime_second_tutorial_house");
		base.Show(f, sizeX, sizeY, aT);
		RestrictionInput.EndLoad();
		GUICollider.DisableAllCollider("CMD_FarewellListRun");
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.SHOW)
		{
			TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
			if (tutorialObserver != null)
			{
				GUIMain.BarrierON(null);
				tutorialObserver.StartSecondTutorial("second_tutorial_house", new Action(GUIMain.BarrierOFF), delegate
				{
					GUICollider.EnableAllCollider("CMD_FarewellListRun");
				});
			}
			else
			{
				GUICollider.EnableAllCollider("CMD_FarewellListRun");
			}
		}
		else
		{
			GUICollider.EnableAllCollider("CMD_FarewellListRun");
		}
	}

	protected override void Update()
	{
		base.Update();
		this.UpdateChipGet();
	}

	protected override void WindowClosed()
	{
		this.csSelectPanelMonsterIcon.PARTS_CT_MN = 4;
		ClassSingleton<GUIMonsterIconList>.Instance.PushBackAllMonsterPrefab();
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
		cmd_SaleCheck.SetParams(this.sellMonsterList, this.ngTX_CHIP_GET.text);
	}

	private void OnCloseSale(int idx)
	{
		if (idx == 0)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
			GameWebAPI.MN_Req_Sale req = new GameWebAPI.MN_Req_Sale();
			req.saleMonsterDataList = new GameWebAPI.MN_Req_Sale.SaleMonsterDataList[this.sellMonsterList.Count];
			for (int i = 0; i < this.sellMonsterList.Count; i++)
			{
				GameWebAPI.MN_Req_Sale.SaleMonsterDataList saleMonsterDataList = new GameWebAPI.MN_Req_Sale.SaleMonsterDataList();
				saleMonsterDataList.userMonsterId = this.sellMonsterList[i].userMonster.userMonsterId;
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
				if (response.itemRecovered == 1)
				{
					this.isReceived = true;
					string @string = StringMaster.GetString("SellRecoverItem");
					CMD_ModalMessageNoBtn cmd_ModalMessageNoBtn = GUIMain.ShowCommonDialog(null, "CMD_ModalMessageNoBtn") as CMD_ModalMessageNoBtn;
					cmd_ModalMessageNoBtn.SetParam(@string);
					cmd_ModalMessageNoBtn.AdjustSize();
				}
				this.EndSale(response);
			}, delegate(Exception noop)
			{
				RestrictionInput.EndLoad();
			}, null));
		}
	}

	private void EndSale(GameWebAPI.RespDataMN_SaleExec response)
	{
		string[] userMonsterIdList = this.sellMonsterList.Select((MonsterData x) => x.userMonster.userMonsterId).ToArray<string>();
		ClassSingleton<MonsterUserDataMng>.Instance.DeleteUserMonsterData(userMonsterIdList);
		ChipDataMng.DeleteEquipChip(userMonsterIdList);
		ChipDataMng.GetUserChipSlotData().DeleteMonsterSlotList(userMonsterIdList);
		ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(MonsterDataMng.Instance().GetMonsterDataList());
		this.InitMonsterList(false);
		this.sellMonsterList.Clear();
		this.monsterList.SetGrayOutBlockMonster();
		if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.SELL)
		{
			this.monsterList.SetGrayOutPartyUsed();
		}
		else if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN_SELL)
		{
			this.monsterList.SetGrayOutGrowing(this.targetMonsterList);
		}
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
			this.sellMonsterList.Clear();
			this.monsterList.ClearGrayOutBlockMonster();
			if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.SHOW)
			{
				this.monsterList.ClearGrayOutPartyUsed();
			}
			else if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN)
			{
				this.monsterList.SetGrayOutGrowing(this.targetMonsterList);
			}
		}
		else if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.SELL || CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN_SELL)
		{
			this.showBtnSprite.spriteName = "Common02_Btn_BaseON";
			this.showBtnCollider.activeCollider = true;
			this.showBtnLabel.color = Color.white;
			this.farewellBtnSprite.spriteName = "Common02_Btn_BaseG";
			this.farewellBtnCollider.activeCollider = false;
			this.farewellBtnLabel.color = Util.convertColor(180f, 180f, 180f, 255f);
			base.PartsTitle.SetTitle(StringMaster.GetString("SaleTitle"));
			this.goBT_SELL.SetActive(true);
			this.goBT_SELL_CHIP.SetActive(true);
			this.monsterList.SetGrayOutBlockMonster();
			if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.SELL)
			{
				this.monsterList.SetGrayOutPartyUsed();
			}
			else if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN_SELL)
			{
				this.monsterList.SetGrayOutGrowing(this.targetMonsterList);
			}
			this.BTSeleOn();
		}
	}

	private void BTSeleOn()
	{
		UISprite component = this.goBT_SELL.GetComponent<UISprite>();
		if (this.sellMonsterList.Count > 0)
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
		ClassSingleton<GUIMonsterIconList>.Instance.ResetIconState();
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		List<MonsterData> list = monsterDataMng.GetMonsterDataList();
		this.ngTX_MN_HAVE_ADULT.text = this.GetDigiHouseMonsterNum(list).ToString();
		int gerdenMonsterNum = this.GetGerdenMonsterNum(list);
		this.ngTX_MN_HAVE_CHILD.text = gerdenMonsterNum.ToString();
		this.growingNum = this.GetGardenGrowingMonsterNum(list);
		if ((CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN || CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN_SELL) && gerdenMonsterNum == 0)
		{
			this.cautionLabel.gameObject.SetActive(true);
			this.cautionLabel.text = StringMaster.GetString("Garden-02");
		}
		else if (this.cautionLabel.gameObject.activeSelf)
		{
			this.cautionLabel.gameObject.SetActive(false);
		}
		if (CMD_FarewellListRun.Mode != CMD_FarewellListRun.MODE.GARDEN && CMD_FarewellListRun.Mode != CMD_FarewellListRun.MODE.GARDEN_SELL)
		{
			list = MonsterFilter.Filter(list, MonsterFilterType.ALL_OUT_GARDEN);
		}
		else
		{
			list = MonsterFilter.Filter(list, MonsterFilterType.ALL_IN_GARDEN);
		}
		monsterDataMng.SortMDList(list);
		monsterDataMng.SetSortLSMessage();
		this.csSelectPanelMonsterIcon.initLocation = initLoc;
		Vector3 localScale = this.goMN_ICON_LIST[0].transform.localScale;
		ClassSingleton<GUIMonsterIconList>.Instance.SetLockIcon();
		this.csSelectPanelMonsterIcon.useLocationRecord = true;
		this.csSelectPanelMonsterIcon.SetCheckEnablePushAction(null);
		this.targetMonsterList = list;
		list = MonsterDataMng.Instance().SelectionMDList(list);
		this.csSelectPanelMonsterIcon.AllBuild(list, localScale, new Action<MonsterData>(this.ActMIconLong), new Action<MonsterData>(this.ActMIconShort), false);
		this.csSelectPanelMonsterIcon.ScrollBarPosX = 458f;
		this.csSelectPanelMonsterIcon.ScrollBarBGPosX = 458f;
		BtnSort[] componentsInChildren = base.GetComponentsInChildren<BtnSort>(true);
		this.sortButton = componentsInChildren[0];
		this.sortButton.OnChangeSortType = new Action(this.OnChangeSortSetting);
		this.sortButton.SortTargetMonsterList = this.targetMonsterList;
	}

	private void OnChangeSortSetting()
	{
		MonsterDataMng.Instance().SortMDList(this.targetMonsterList);
		MonsterDataMng.Instance().SetSortLSMessage();
		List<MonsterData> dts = MonsterDataMng.Instance().SelectionMDList(this.targetMonsterList);
		this.csSelectPanelMonsterIcon.ReAllBuild(dts);
		if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.SELL)
		{
			this.monsterList.SetGrayOutBlockMonster();
			this.monsterList.SetGrayOutPartyUsed();
			this.monsterList.SetSellMonsterList();
		}
		else if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN_SELL)
		{
			this.monsterList.SetGrayOutBlockMonster();
			this.monsterList.SetGrayOutGrowing(this.targetMonsterList);
			this.monsterList.SetSellMonsterList();
		}
	}

	private void ActMIconShort(MonsterData tappedMonsterData)
	{
		if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.SHOW)
		{
			this.ShowMonsterDetailForList(tappedMonsterData);
		}
		else if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.SELL || CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN_SELL)
		{
			if (this.sellMonsterList.Count < 10)
			{
				this.sellMonsterList.Add(tappedMonsterData);
				GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(tappedMonsterData);
				this.iconGrayOut.SetSellMonster(icon, this.sellMonsterList.Count);
				if (this.sellMonsterList.Count == 10)
				{
					this.SetGrayOutNotSelectedIcon();
				}
				this.BTSeleOn();
			}
		}
		else if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN)
		{
			CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(delegate(int selectButtonIndex)
			{
				this.CallbackConfirmMove(selectButtonIndex, tappedMonsterData);
			}, "CMD_Confirm") as CMD_Confirm;
			cmd_Confirm.Title = StringMaster.GetString("Garden-03");
			cmd_Confirm.Info = StringMaster.GetString("Garden-04");
			cmd_Confirm.BtnTextYes = StringMaster.GetString("SystemButtonYes");
			cmd_Confirm.BtnTextNo = StringMaster.GetString("SystemButtonNo");
		}
	}

	private void CallbackConfirmMove(int selectButtonIndex, MonsterData monster)
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
					base.StartCoroutine(this.MoveDigiGarden(monster));
				}
			}
			else
			{
				CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
				cmd_ModalMessage.Title = StringMaster.GetString("Garden-03");
				cmd_ModalMessage.Info = StringMaster.GetString("GardenGrowMax");
			}
		}
	}

	private IEnumerator MoveDigiGarden(MonsterData monster)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
		APIRequestTask task = this.RequestMoveDigiGarden(monster);
		yield return base.StartCoroutine(task.Run(delegate
		{
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
					ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(MonsterDataMng.Instance().GetMonsterDataList());
					CMD_DigiGarden.instance.InitMonsterList(true);
				}
			});
			RestrictionInput.EndLoad();
			this.ClosePanel(true);
		}, delegate(Exception nop)
		{
			if (null != CMD_DigiGarden.instance && CMD_DigiGarden.instance.GetActionStatus() == CommonDialog.ACT_STATUS.OPEN)
			{
				base.SetCloseAction(delegate(int noop)
				{
					CMD_DigiGarden.instance.ClosePanel(true);
				});
			}
			RestrictionInput.EndLoad();
			this.ClosePanel(true);
		}, null));
		yield break;
	}

	private APIRequestTask RequestMoveDigiGarden(MonsterData monster)
	{
		GameWebAPI.RequestMN_MoveDigiGarden requestMN_MoveDigiGarden = new GameWebAPI.RequestMN_MoveDigiGarden();
		requestMN_MoveDigiGarden.SetSendData = delegate(GameWebAPI.MN_Req_MoveDigiGarden param)
		{
			param.userMonsterId = monster.userMonster.userMonsterId;
		};
		requestMN_MoveDigiGarden.OnReceived = delegate(GameWebAPI.RespDataMN_MoveDigiGarden response)
		{
			ClassSingleton<MonsterUserDataMng>.Instance.UpdateUserMonsterData(response.userMonster);
		};
		GameWebAPI.RequestMN_MoveDigiGarden request = requestMN_MoveDigiGarden;
		return new APIRequestTask(request, false);
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
		for (int i = 0; i < this.targetMonsterList.Count; i++)
		{
			this.targetMonsterList[i].userMonster.growEndDate = ServerDateTime.Now.ToString();
		}
		CMD_DigiGarden.instance.InitMonsterList(true);
		this.ClosePanel(true);
	}

	private void ActMIconS_Remove(MonsterData tappedMonster)
	{
		if (this.sellMonsterList.Contains(tappedMonster))
		{
			this.sellMonsterList.Remove(tappedMonster);
		}
		this.ClearGrayOutNotSelectedIcon();
		GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(tappedMonster);
		this.iconGrayOut.CancelSell(icon);
		for (int i = 0; i < this.sellMonsterList.Count; i++)
		{
			icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(this.sellMonsterList[i]);
			icon.SelectNum = i + 1;
		}
		this.BTSeleOn();
	}

	private void ShowMonsterDetailForList(MonsterData monsterData)
	{
		CMD_CharacterDetailed.DataChg = monsterData;
		GUIMain.ShowCommonDialog(delegate(int i)
		{
			GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterData);
			icon.Lock = monsterData.userMonster.IsLocked;
		}, "CMD_CharacterDetailed");
	}

	private void ActMIconLong(MonsterData tappedMonsterData)
	{
		if (this.isOfflineModeFlag)
		{
			return;
		}
		if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.SHOW || CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN)
		{
			this.ShowMonsterDetailForList(tappedMonsterData);
		}
		else
		{
			CMD_CharacterDetailed.DataChg = tappedMonsterData;
			CMD_CharacterDetailed cmd_CharacterDetailed = GUIMain.ShowCommonDialog(delegate(int i)
			{
				GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(tappedMonsterData);
				this.monsterList.SetGrayOutReturnDetailed(icon, tappedMonsterData, 10 <= this.sellMonsterList.Count);
			}, "CMD_CharacterDetailed") as CMD_CharacterDetailed;
			if (this.sellMonsterList.Contains(tappedMonsterData))
			{
				cmd_CharacterDetailed.Mode = CMD_CharacterDetailed.LockMode.Farewell;
			}
		}
	}

	private void UpdateChipGet()
	{
		int value = this.CalcChipGet();
		this.ngTX_CHIP_GET.text = StringFormat.Cluster(value);
	}

	private int CalcChipGet()
	{
		int num = 0;
		for (int i = 0; i < this.sellMonsterList.Count; i++)
		{
			num += this.sellMonsterList[i].GetPrice();
		}
		for (int j = 0; j < this.sellMonsterList.Count; j++)
		{
			foreach (GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip equip in this.sellMonsterList[j].GetSlotEquip())
			{
				GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChip = ChipDataMng.GetUserChip(equip.userChipId);
				GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(userChip.chipId.ToString());
				num += chipMainData.GetSellPrice();
			}
		}
		return num;
	}

	private void ShowHaveMonster()
	{
		int num = this.targetMonsterList.Count;
		int num2 = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.unitLimitMax);
		this.growingNum = this.GetGardenGrowingMonsterNum(this.targetMonsterList);
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

	private void SetGrayOutNotSelectedIcon()
	{
		if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.SELL)
		{
			this.monsterList.SetGrayOutNotSelectedIconHouse();
		}
		else if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN_SELL)
		{
			this.monsterList.SetGrayOutNotSelectedIconGarden();
		}
	}

	private void ClearGrayOutNotSelectedIcon()
	{
		if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.SELL)
		{
			this.monsterList.ClearGrayOutNotSelectedIconHouse();
		}
		else if (CMD_FarewellListRun.Mode == CMD_FarewellListRun.MODE.GARDEN_SELL)
		{
			this.monsterList.ClearGrayOutNotSelectedIconGarden();
		}
	}

	public void SetOfflineMode(bool isOfflineMode)
	{
		this.isOfflineModeFlag = isOfflineMode;
	}

	private int GetDigiHouseMonsterNum(List<MonsterData> monsterDataList)
	{
		int num = 0;
		for (int i = 0; i < monsterDataList.Count; i++)
		{
			int growStep = (int)MonsterGrowStepData.ToGrowStep(monsterDataList[i].monsterMG.growStep);
			if (!MonsterGrowStepData.IsGardenDigimonScope(growStep))
			{
				num++;
			}
		}
		return num;
	}

	private int GetGerdenMonsterNum(List<MonsterData> monsterDataList)
	{
		int num = 0;
		for (int i = 0; i < monsterDataList.Count; i++)
		{
			int growStep = (int)MonsterGrowStepData.ToGrowStep(monsterDataList[i].monsterMG.growStep);
			if (MonsterGrowStepData.IsGardenDigimonScope(growStep))
			{
				num++;
			}
		}
		return num;
	}

	private int GetGardenGrowingMonsterNum(List<MonsterData> monsterDataList)
	{
		int num = 0;
		for (int i = 0; i < monsterDataList.Count; i++)
		{
			int growStep = (int)MonsterGrowStepData.ToGrowStep(monsterDataList[i].monsterMG.growStep);
			if (MonsterGrowStepData.IsGardenDigimonScope(growStep) && (monsterDataList[i].userMonster.IsEgg() || monsterDataList[i].userMonster.IsGrowing()))
			{
				num++;
			}
		}
		return num;
	}

	public override void ClosePanel(bool animation = true)
	{
		if (this.isReceived)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			APIRequestTask task = DataMng.Instance().RequestMyPageData(false);
			base.StartCoroutine(task.Run(delegate
			{
				ClassSingleton<FacePresentAccessor>.Instance.facePresent.SetBadgeOnly();
				RestrictionInput.EndLoad();
				this.ClosePanel(animation);
			}, delegate(Exception nop)
			{
				RestrictionInput.EndLoad();
				this.ClosePanel(animation);
			}, null));
		}
		else
		{
			base.ClosePanel(animation);
		}
	}

	public enum MODE
	{
		SHOW,
		SELL,
		GARDEN,
		GARDEN_SELL
	}
}
