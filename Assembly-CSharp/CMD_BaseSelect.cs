using Ability;
using Evolution;
using FarmData;
using Master;
using Monster;
using MonsterList.BaseSelect;
using Picturebook;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_BaseSelect : CMD
{
	private const string CMD_NAME = "CMD_BaseSelect";

	public static CMD_BaseSelect instance;

	private static MonsterSortType iconSortType = MonsterSortType.LEVEL;

	[SerializeField]
	private ChipBaseSelect chipBaseSelect;

	[SerializeField]
	private BtnSort btnSort;

	[SerializeField]
	private UILabel btnSortText;

	[SerializeField]
	private GameObject goBTN_EVITEM_LIST;

	private GameObject goSelectPanelMonsterIcon;

	private GUISelectPanelMonsterIcon csSelectPanelMonsterIcon;

	private GUIMonsterIcon leftLargeMonsterIcon;

	[SerializeField]
	[Header("キャンペーンラベル")]
	private CampaignLabel campaignLabel;

	[SerializeField]
	private List<GameObject> goMN_ICON_LIST;

	[SerializeField]
	private GameObject goMN_ICON_CHG;

	[SerializeField]
	private MonsterBasicInfo monsterBasicInfo;

	[SerializeField]
	private MonsterResistanceList monsterResistanceList;

	[SerializeField]
	private MonsterStatusList monsterStatusList;

	[SerializeField]
	private MonsterMedalList monsterMedalList;

	[SerializeField]
	private MonsterLeaderSkill monsterLeaderSkill;

	[SerializeField]
	private MonsterLearnSkill monsterUniqueSkill;

	[SerializeField]
	private MonsterLearnSkill monsterSuccessionSkill;

	[SerializeField]
	private MonsterLearnSkill monsterSuccessionSkill2;

	[SerializeField]
	private GameObject monsterSuccessionSkillAvailable;

	[SerializeField]
	private GameObject monsterSuccessionSkillGrayReady;

	[SerializeField]
	private GameObject monsterSuccessionSkillGrayNA;

	[SerializeField]
	private GameObject goDetailedSkillPanel;

	[SerializeField]
	private MonsterResistanceList detailedMonsterResistanceList;

	[SerializeField]
	private MonsterLeaderSkill detailedMonsterLeaderSkill;

	[SerializeField]
	private MonsterLearnSkill detailedMonsterUniqueSkill;

	[SerializeField]
	private MonsterLearnSkill detailedMonsterSuccessionSkill;

	[SerializeField]
	private MonsterLearnSkill detailedMonsterSuccessionSkill2;

	[SerializeField]
	private GameObject detailedMonsterSuccessionSkillAvailable;

	[SerializeField]
	private GameObject detailedMonsterSuccessionSkillGrayReady;

	[SerializeField]
	private GameObject detailedMonsterSuccessionSkillGrayNA;

	[SerializeField]
	private GameObject switchSkillPanelBtn;

	[SerializeField]
	private UILabel ngTX_MN_HAVE;

	[SerializeField]
	private UILabel ngTX_SORT_DISP;

	[SerializeField]
	[Header("キャラクターのステータスPanel")]
	private StatusPanel statusPanel;

	[SerializeField]
	private UISprite ngBTN_DECIDE;

	[SerializeField]
	private UILabel ngTX_BTN_DECIDE;

	[SerializeField]
	private GUICollider clBTN_DECIDE;

	[SerializeField]
	private UILabel ngTX_DECIDE;

	[SerializeField]
	private UISprite ngSPR_CHIP;

	[SerializeField]
	private UILabel ngTXT_CHIP;

	[SerializeField]
	private UILabel ngTXT_MEAT;

	[SerializeField]
	private UILabel ngTXT_HQ_MEAT;

	[SerializeField]
	private GameObject goPlateClustar;

	[SerializeField]
	private GameObject goPlateMeat;

	private BtnSort sortButton;

	private GameObject goLeftLargeMonsterIcon;

	private int statusPage = 1;

	[SerializeField]
	private List<GameObject> goStatusPanelPage;

	[SerializeField]
	private UILabel switchingBtnText;

	private List<MonsterData> targetMonsterList;

	private BaseSelectIconGrayOut iconGrayOut;

	private BaseSelectMonsterList monsterList;

	public static MonsterSortType IconSortType
	{
		get
		{
			return CMD_BaseSelect.iconSortType;
		}
		set
		{
			CMD_BaseSelect.iconSortType = value;
		}
	}

	public static MonsterSortOrder IconSortOrder { get; set; }

	public static MonsterDetailedFilterType IconFilterType { get; set; }

	public static CMD_BaseSelect.BASE_TYPE BaseType { get; set; }

	public static CMD_BaseSelect.ELEMENT_TYPE ElementType { get; set; }

	public static MonsterData DataChg { get; set; }

	public static CMD_BaseSelect CreateChipChipInstalling(Action<int> callback = null)
	{
		CMD_BaseSelect.BaseType = CMD_BaseSelect.BASE_TYPE.CHIP;
		CMD_BaseSelect.ElementType = CMD_BaseSelect.ELEMENT_TYPE.BASE;
		return GUIMain.ShowCommonDialog(callback, "CMD_BaseSelect") as CMD_BaseSelect;
	}

	protected override void Awake()
	{
		this.iconGrayOut = new BaseSelectIconGrayOut();
		this.iconGrayOut.SetNormalAction(new Action<MonsterData>(this.ActMIconShort), new Action<MonsterData>(this.ActMIconLong));
		this.iconGrayOut.SetSelectedAction(new Action<MonsterData>(this.RemoveIcon), new Action<MonsterData>(this.ActMIconLong));
		this.iconGrayOut.SetBlockAction(null, new Action<MonsterData>(this.ActMIconLong));
		this.monsterList = new BaseSelectMonsterList();
		this.monsterList.Initialize(ClassSingleton<MonsterUserDataMng>.Instance.GetDeckUserMonsterList(), ClassSingleton<MonsterUserDataMng>.Instance.GetColosseumDeckUserMonsterList(), this.iconGrayOut);
		CMD_BaseSelect.DataChg = null;
		this.chipBaseSelect.ClearChipIcons();
		base.Awake();
		this.monsterMedalList.SetActive(false);
		for (int i = 0; i < this.goMN_ICON_LIST.Count; i++)
		{
			this.goMN_ICON_LIST[i].SetActive(false);
		}
		this.clBTN_DECIDE.activeCollider = false;
		this.ngTX_BTN_DECIDE.text = StringMaster.GetString("SystemButtonDecision");
		this.btnSortText.text = StringMaster.GetString("SystemSortButton");
		this.switchingBtnText.text = StringMaster.GetString("PartyStatusChange2");
		CMD_BaseSelect.instance = this;
	}

	protected override void WindowClosed()
	{
		CMD_BaseSelect.instance = null;
		ClassSingleton<GUIMonsterIconList>.Instance.PushBackAllMonsterPrefab();
		base.WindowClosed();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		GUICollider.DisableAllCollider("CMD_BaseSelect : " + CMD_BaseSelect.BaseType);
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		this.SetCommonUI();
		base.StartCoroutine(this.InitBaseSelect(f, sizeX, sizeY, aT));
	}

	private IEnumerator InitBaseSelect(Action<int> f, float sizeX, float sizeY, float aT)
	{
		bool success = false;
		this.chipBaseSelect.ClearChipIcons();
		switch (CMD_BaseSelect.BaseType)
		{
		case CMD_BaseSelect.BASE_TYPE.EVOLVE:
		{
			APIRequestTask task = Singleton<UserDataMng>.Instance.RequestUserSoulData(false);
			if (!MonsterPicturebookData.IsReady())
			{
				task.Add(MonsterPicturebookData.RequestUserPicturebook());
			}
			yield return AppCoroutine.Start(task.Run(delegate
			{
				success = true;
			}, delegate(Exception nop)
			{
				success = false;
			}, null), false);
			if (success)
			{
				this.goPlateMeat.SetActive(false);
				this.goPlateClustar.SetActive(true);
				this.campaignLabel.refCampaignType = new GameWebAPI.RespDataCP_Campaign.CampaignType[0];
				this.campaignLabel.Refresh();
				base.PartsTitle.SetTitle(StringMaster.GetString("BaseSelect-04"));
				NGUIUtil.ChangeUISpriteWithSize(this.ngSPR_CHIP, "Common02_Icon_Chip");
			}
			this.goBTN_EVITEM_LIST.SetActive(true);
			break;
		}
		case CMD_BaseSelect.BASE_TYPE.MEAL:
		{
			GameWebAPI.RespDataCP_Campaign.CampaignInfo meatExpUpData = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.MeatExpUp);
			if (meatExpUpData == null)
			{
				APIRequestTask task = DataMng.Instance().RequestCampaignAll(false);
				yield return AppCoroutine.Start(task.Run(delegate
				{
					meatExpUpData = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.MeatExpUp);
					success = true;
				}, delegate(Exception nop)
				{
					success = false;
				}, null), false);
			}
			else
			{
				success = true;
			}
			if (meatExpUpData != null)
			{
				this.campaignLabel.refCampaignType = new GameWebAPI.RespDataCP_Campaign.CampaignType[]
				{
					GameWebAPI.RespDataCP_Campaign.CampaignType.MeatExpUp
				};
			}
			else
			{
				this.campaignLabel.refCampaignType = new GameWebAPI.RespDataCP_Campaign.CampaignType[0];
				this.campaignLabel.Refresh();
			}
			this.goPlateMeat.SetActive(true);
			this.goPlateClustar.SetActive(false);
			base.PartsTitle.SetTitle(StringMaster.GetString("BaseSelect-03"));
			NGUIUtil.ChangeUISpriteWithSize(this.ngSPR_CHIP, "Common02_Icon_Meat");
			break;
		}
		case CMD_BaseSelect.BASE_TYPE.LABO:
		{
			success = true;
			this.campaignLabel.refCampaignType = new GameWebAPI.RespDataCP_Campaign.CampaignType[0];
			this.campaignLabel.Refresh();
			string elementTypeName = string.Empty;
			if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.BASE)
			{
				elementTypeName = StringMaster.GetString("SystemBase");
			}
			else if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.PARTNER)
			{
				elementTypeName = StringMaster.GetString("SystemPartner");
			}
			base.PartsTitle.SetTitle(string.Format(StringMaster.GetString("BaseSelect-08"), elementTypeName));
			NGUIUtil.ChangeUISpriteWithSize(this.ngSPR_CHIP, "Common02_Icon_Chip");
			break;
		}
		case CMD_BaseSelect.BASE_TYPE.CHIP:
			success = true;
			this.campaignLabel.refCampaignType = new GameWebAPI.RespDataCP_Campaign.CampaignType[0];
			this.campaignLabel.Refresh();
			base.PartsTitle.SetTitle(StringMaster.GetString("ChipInstalling-02"));
			this.chipBaseSelect.InitBaseSelect();
			break;
		case CMD_BaseSelect.BASE_TYPE.MEDAL_INHERIT:
		{
			success = true;
			this.campaignLabel.refCampaignType = new GameWebAPI.RespDataCP_Campaign.CampaignType[0];
			this.campaignLabel.Refresh();
			string _elementTypeName = string.Empty;
			if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.BASE)
			{
				_elementTypeName = StringMaster.GetString("SystemBase");
			}
			else if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.PARTNER)
			{
				_elementTypeName = StringMaster.GetString("SystemPartner");
			}
			base.PartsTitle.SetTitle(string.Format(StringMaster.GetString("BaseSelect-09"), _elementTypeName));
			break;
		}
		case CMD_BaseSelect.BASE_TYPE.VERSION_UP:
		{
			APIRequestTask task = Singleton<UserDataMng>.Instance.RequestUserSoulData(false);
			yield return AppCoroutine.Start(task.Run(delegate
			{
				success = true;
			}, delegate(Exception nop)
			{
				success = false;
			}, null), false);
			if (success)
			{
				this.campaignLabel.refCampaignType = new GameWebAPI.RespDataCP_Campaign.CampaignType[0];
				this.campaignLabel.Refresh();
				string _elementTypeName = StringMaster.GetString("SystemBase");
				base.PartsTitle.SetTitle(string.Format(StringMaster.GetString("BaseSelect-10"), _elementTypeName));
				this.goBTN_EVITEM_LIST.SetActive(true);
			}
			break;
		}
		default:
			success = true;
			this.campaignLabel.refCampaignType = new GameWebAPI.RespDataCP_Campaign.CampaignType[0];
			global::Debug.LogError("=========================================== CMD_BaseSelect TYPE設定がない!");
			break;
		}
		if (success)
		{
			this.ChipNumUpdate();
			this.InitMonsterList(true);
			this.ShowChgInfo();
			base.ShowDLG();
			switch (CMD_BaseSelect.BaseType)
			{
			case CMD_BaseSelect.BASE_TYPE.EVOLVE:
				base.SetTutorialAnyTime("anytime_second_tutorial_evolution");
				break;
			case CMD_BaseSelect.BASE_TYPE.MEAL:
				base.SetTutorialAnyTime("anytime_second_tutorial_meal");
				break;
			case CMD_BaseSelect.BASE_TYPE.CHIP:
				base.SetTutorialAnyTime("anytime_second_tutorial_chip_equipment");
				break;
			}
			base.Show(f, sizeX, sizeY, aT);
		}
		else
		{
			GUICollider.EnableAllCollider("CMD_BaseSelect : " + CMD_BaseSelect.BaseType);
			base.ClosePanel(false);
		}
		RestrictionInput.EndLoad();
		yield break;
	}

	public void ChipNumUpdate()
	{
		GameWebAPI.RespDataUS_GetPlayerInfo.PlayerInfo playerInfo = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo;
		switch (CMD_BaseSelect.BaseType)
		{
		case CMD_BaseSelect.BASE_TYPE.EVOLVE:
			this.ngTXT_CHIP.text = StringFormat.Cluster(playerInfo.gamemoney);
			break;
		case CMD_BaseSelect.BASE_TYPE.MEAL:
			this.ngTXT_MEAT.text = playerInfo.meatNum;
			this.ngTXT_HQ_MEAT.text = Singleton<UserDataMng>.Instance.GetUserItemNumByItemId(50001).ToString();
			this.ngTXT_MEAT.color = ((int.Parse(playerInfo.meatNum) > 0) ? Color.white : Color.red);
			break;
		case CMD_BaseSelect.BASE_TYPE.LABO:
			this.ngTXT_CHIP.text = StringFormat.Cluster(playerInfo.gamemoney);
			break;
		case CMD_BaseSelect.BASE_TYPE.MEDAL_INHERIT:
			this.ngTXT_CHIP.text = StringFormat.Cluster(playerInfo.gamemoney);
			break;
		case CMD_BaseSelect.BASE_TYPE.VERSION_UP:
			this.ngTXT_CHIP.text = StringFormat.Cluster(playerInfo.gamemoney);
			break;
		}
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		if (tutorialObserver != null)
		{
			GUIMain.BarrierON(null);
			switch (CMD_BaseSelect.BaseType)
			{
			case CMD_BaseSelect.BASE_TYPE.EVOLVE:
				tutorialObserver.StartSecondTutorial("second_tutorial_evolution", new Action(GUIMain.BarrierOFF), delegate
				{
					GUICollider.EnableAllCollider("CMD_BaseSelect : " + CMD_BaseSelect.BaseType);
				});
				goto IL_DB;
			case CMD_BaseSelect.BASE_TYPE.CHIP:
				tutorialObserver.StartSecondTutorial("second_tutorial_chip_equipment", new Action(GUIMain.BarrierOFF), delegate
				{
					GUICollider.EnableAllCollider("CMD_BaseSelect : " + CMD_BaseSelect.BaseType);
				});
				goto IL_DB;
			}
			GUIMain.BarrierOFF();
			GUICollider.EnableAllCollider("CMD_BaseSelect : " + CMD_BaseSelect.BaseType);
			IL_DB:;
		}
		else
		{
			GUICollider.EnableAllCollider("CMD_BaseSelect : " + CMD_BaseSelect.BaseType);
		}
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void ClosePanel(bool animation = true)
	{
		FarmRoot farmRoot = FarmRoot.Instance;
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
		if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.EVOLVE && farmRoot != null && farmRoot.DigimonManager != null)
		{
			farmRoot.DigimonManager.RefreshDigimonGameObject(true, delegate
			{
				this.CloseAndFarmCamOn(animation);
			});
		}
		else
		{
			this.CloseAndFarmCamOn(animation);
		}
	}

	private void CloseAndFarmCamOn(bool animation)
	{
		if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.LABO || CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.MEDAL_INHERIT || CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.VERSION_UP)
		{
			FarmCameraControlForCMD.On();
			base.ClosePanel(animation);
			RestrictionInput.EndLoad();
		}
		else
		{
			if (base.gameObject == null || !base.gameObject.activeSelf)
			{
				FarmCameraControlForCMD.On();
				base.ClosePanel(animation);
				RestrictionInput.EndLoad();
				return;
			}
			APIRequestTask task = DataMng.Instance().RequestMyPageData(false);
			AppCoroutine.Start(task.Run(delegate
			{
				ClassSingleton<FaceMissionAccessor>.Instance.faceMission.SetBadge(true);
				FarmCameraControlForCMD.On();
				this.ClosePanel(animation);
				RestrictionInput.EndLoad();
			}, delegate(Exception nop)
			{
				FarmCameraControlForCMD.On();
				this.ClosePanel(animation);
				RestrictionInput.EndLoad();
			}, null), false);
		}
	}

	private void ShowChgInfo()
	{
		this.statusPanel.SetEnable(CMD_BaseSelect.DataChg != null);
		if (CMD_BaseSelect.DataChg != null)
		{
			this.monsterBasicInfo.SetMonsterData(CMD_BaseSelect.DataChg);
			this.monsterStatusList.SetValues(CMD_BaseSelect.DataChg, false);
			this.monsterLeaderSkill.SetSkill(CMD_BaseSelect.DataChg);
			this.detailedMonsterLeaderSkill.SetSkill(CMD_BaseSelect.DataChg);
			this.monsterUniqueSkill.SetSkill(CMD_BaseSelect.DataChg);
			this.detailedMonsterUniqueSkill.SetSkill(CMD_BaseSelect.DataChg);
			this.monsterSuccessionSkill.SetSkill(CMD_BaseSelect.DataChg);
			this.detailedMonsterSuccessionSkill.SetSkill(CMD_BaseSelect.DataChg);
			this.monsterSuccessionSkillGrayReady.SetActive(false);
			this.monsterSuccessionSkillAvailable.SetActive(false);
			this.monsterSuccessionSkillGrayNA.SetActive(false);
			this.monsterSuccessionSkill2.SetSkill(CMD_BaseSelect.DataChg);
			this.detailedMonsterSuccessionSkillGrayReady.SetActive(false);
			this.detailedMonsterSuccessionSkillAvailable.SetActive(false);
			this.detailedMonsterSuccessionSkillGrayNA.SetActive(false);
			this.detailedMonsterSuccessionSkill2.SetSkill(CMD_BaseSelect.DataChg);
			if (MonsterStatusData.IsVersionUp(CMD_BaseSelect.DataChg.GetMonsterMaster().Simple.rare))
			{
				if (CMD_BaseSelect.DataChg.GetExtraCommonSkill() == null)
				{
					this.monsterSuccessionSkillGrayReady.SetActive(true);
					this.detailedMonsterSuccessionSkillGrayReady.SetActive(true);
				}
				else
				{
					this.monsterSuccessionSkillAvailable.SetActive(true);
					this.detailedMonsterSuccessionSkillAvailable.SetActive(true);
				}
			}
			else
			{
				this.monsterSuccessionSkillGrayNA.SetActive(true);
				this.detailedMonsterSuccessionSkillGrayNA.SetActive(true);
			}
			this.monsterResistanceList.SetValues(CMD_BaseSelect.DataChg);
			this.detailedMonsterResistanceList.SetValues(CMD_BaseSelect.DataChg);
			this.monsterMedalList.SetValues(CMD_BaseSelect.DataChg.userMonster);
			this.StatusPageChange(false);
		}
		else
		{
			this.chipBaseSelect.ClearChipIcons();
			this.monsterBasicInfo.ClearMonsterData();
			this.monsterStatusList.ClearValues();
			this.monsterMedalList.SetActive(false);
			this.switchDetailSkillPanel(false);
			this.RequestStatusPage(1);
		}
	}

	private void OnTouchDecide()
	{
		switch (CMD_BaseSelect.BaseType)
		{
		case CMD_BaseSelect.BASE_TYPE.EVOLVE:
			GUIMain.ShowCommonDialog(delegate(int i)
			{
				if (this.leftLargeMonsterIcon != null && CMD_BaseSelect.DataChg != null)
				{
					this.leftLargeMonsterIcon.Lock = CMD_BaseSelect.DataChg.userMonster.IsLocked;
				}
			}, "CMD_Evolution");
			break;
		case CMD_BaseSelect.BASE_TYPE.MEAL:
		{
			List<UserFacility> userFacilityList = Singleton<UserDataMng>.Instance.GetUserFacilityList();
			if (!userFacilityList.Exists((UserFacility x) => x.facilityId == 1))
			{
				CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.LeadBuildMeat), "CMD_Confirm") as CMD_Confirm;
				cmd_Confirm.Title = StringMaster.GetString("BaseSelect-05");
				cmd_Confirm.Info = StringMaster.GetString("BaseSelect-06");
				cmd_Confirm.BtnTextYes = StringMaster.GetString("BaseSelect-07");
				cmd_Confirm.BtnTextNo = StringMaster.GetString("SystemButtonClose");
			}
			else
			{
				CMD_MealExecution.DataChg = CMD_BaseSelect.DataChg;
				GUIMain.ShowCommonDialog(null, "CMD_MealExecution");
				CMD_MealExecution.UpdateParamCallback = delegate()
				{
					ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(MonsterDataMng.Instance().GetMonsterDataList());
					this.InitMonsterList(true);
					this.ShowChgInfo();
					this.CheckChip();
				};
			}
			break;
		}
		case CMD_BaseSelect.BASE_TYPE.LABO:
		case CMD_BaseSelect.BASE_TYPE.MEDAL_INHERIT:
		{
			MonsterData partnerDigimon = CMD_PairSelectBase.instance.partnerDigimon;
			if (CMD_BaseSelect.DataChg == null)
			{
				if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.BASE)
				{
					if (partnerDigimon != null)
					{
						CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
						if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.LABO)
						{
							cmd_ModalMessage.Title = StringMaster.GetString("LaboratoryTitle");
						}
						else if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.MEDAL_INHERIT)
						{
							cmd_ModalMessage.Title = StringMaster.GetString("MedalInheritTitle");
						}
						cmd_ModalMessage.Info = StringMaster.GetString("Laboratory-06");
						return;
					}
					CMD_PairSelectBase.instance.RemoveBaseDigimon();
				}
				else if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.PARTNER)
				{
					CMD_PairSelectBase.instance.RemovePartnerDigimon();
				}
			}
			base.SetForceReturnValue(1);
			this.ClosePanel(true);
			break;
		}
		case CMD_BaseSelect.BASE_TYPE.CHIP:
			CMD_ChipSphere.DataChg = CMD_BaseSelect.DataChg;
			this.chipBaseSelect.OnTouchDecide(delegate
			{
				this.SetSelectedCharChg();
			});
			break;
		case CMD_BaseSelect.BASE_TYPE.VERSION_UP:
			if (CMD_BaseSelect.DataChg == null && CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.BASE)
			{
				CMD_PairSelectBase.instance.RemoveBaseDigimon();
			}
			base.SetForceReturnValue(1);
			this.ClosePanel(true);
			break;
		default:
			global::Debug.LogError("=========================================== CMD_BaseSelect TYPE設定がない!");
			break;
		}
	}

	private void SetCommonUI()
	{
		this.goSelectPanelMonsterIcon = GUIManager.LoadCommonGUI("SelectListPanel/SelectListPanelMonsterIcon", base.gameObject);
		this.csSelectPanelMonsterIcon = this.goSelectPanelMonsterIcon.GetComponent<GUISelectPanelMonsterIcon>();
		if (this.goEFC_RIGHT != null)
		{
			this.goSelectPanelMonsterIcon.transform.SetParent(this.goEFC_RIGHT.transform);
		}
		Vector3 localPosition = this.goSelectPanelMonsterIcon.transform.localPosition;
		localPosition.x = 208f;
		GUICollider component = this.goSelectPanelMonsterIcon.GetComponent<GUICollider>();
		component.SetOriginalPos(localPosition);
		this.csSelectPanelMonsterIcon.ListWindowViewRect = ConstValue.GetRectWindow2();
	}

	public void InitMonsterList(bool initLoc = true)
	{
		ClassSingleton<GUIMonsterIconList>.Instance.ResetIconState();
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		if (this.targetMonsterList != null)
		{
			this.targetMonsterList.Clear();
		}
		List<MonsterData> list = monsterDataMng.GetMonsterDataList();
		switch (CMD_BaseSelect.BaseType)
		{
		case CMD_BaseSelect.BASE_TYPE.EVOLVE:
			list = MonsterFilter.Filter(list, MonsterFilterType.CAN_EVOLVE);
			this.csSelectPanelMonsterIcon.BaseType = CMD_BaseSelect.BASE_TYPE.EVOLVE;
			goto IL_10E;
		case CMD_BaseSelect.BASE_TYPE.MEAL:
			list = MonsterFilter.Filter(list, MonsterFilterType.ALL_OUT_GARDEN);
			this.csSelectPanelMonsterIcon.BaseType = CMD_BaseSelect.BASE_TYPE.MEAL;
			goto IL_10E;
		case CMD_BaseSelect.BASE_TYPE.LABO:
			list = MonsterFilter.Filter(list, MonsterFilterType.RESEARCH_TARGET);
			this.csSelectPanelMonsterIcon.BaseType = CMD_BaseSelect.BASE_TYPE.LABO;
			goto IL_10E;
		case CMD_BaseSelect.BASE_TYPE.MEDAL_INHERIT:
			if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.BASE)
			{
				list = MonsterFilter.Filter(list, MonsterFilterType.ALL_OUT_GARDEN);
			}
			else if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.PARTNER)
			{
				list = MonsterFilter.Filter(list, MonsterFilterType.ALL_OUT_GARDEN);
				list = MonsterFilter.Filter(list, MonsterFilterType.HAVE_MEDALS);
			}
			this.csSelectPanelMonsterIcon.BaseType = CMD_BaseSelect.BASE_TYPE.MEDAL_INHERIT;
			goto IL_10E;
		case CMD_BaseSelect.BASE_TYPE.VERSION_UP:
			list = MonsterFilter.Filter(list, MonsterFilterType.CAN_VERSION_UP);
			this.csSelectPanelMonsterIcon.BaseType = CMD_BaseSelect.BASE_TYPE.VERSION_UP;
			goto IL_10E;
		}
		list = MonsterFilter.Filter(list, MonsterFilterType.ALL_OUT_GARDEN);
		IL_10E:
		monsterDataMng.SortMDList(list);
		if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.EVOLVE)
		{
			this.SetEvolutionMonsterIconList(list);
		}
		else if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.VERSION_UP)
		{
			this.SetVersionUPMonsterIconList(list);
		}
		else
		{
			monsterDataMng.SetSortLSMessage();
		}
		this.csSelectPanelMonsterIcon.initLocation = initLoc;
		Vector3 localScale = this.goMN_ICON_LIST[0].transform.localScale;
		ClassSingleton<GUIMonsterIconList>.Instance.SetLockIcon();
		List<MonsterData> deckUserMonsterList = ClassSingleton<MonsterUserDataMng>.Instance.GetDeckUserMonsterList();
		if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.LABO || CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.MEDAL_INHERIT || CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.VERSION_UP)
		{
			MonsterData baseDigimon = CMD_PairSelectBase.instance.baseDigimon;
			MonsterData partnerDigimon = CMD_PairSelectBase.instance.partnerDigimon;
			bool flag = false;
			if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.MEDAL_INHERIT && CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.BASE)
			{
				flag = true;
			}
			if (!flag)
			{
				this.monsterList.GrayOutPartyUsed();
			}
			if (baseDigimon != null)
			{
				GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(baseDigimon);
				this.iconGrayOut.SetSelectText(icon, StringMaster.GetString("SystemBase"));
			}
			if (partnerDigimon != null)
			{
				GUIMonsterIcon icon2 = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(partnerDigimon);
				this.iconGrayOut.SetSelectText(icon2, "1");
			}
			if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.BASE)
			{
				CMD_BaseSelect.DataChg = baseDigimon;
			}
			else if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.PARTNER)
			{
				CMD_BaseSelect.DataChg = partnerDigimon;
			}
			this.SetSelectedCharChg();
		}
		else if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.MEAL)
		{
			for (int i = 0; i < list.Count; i++)
			{
				int num = int.Parse(list[i].userMonster.level);
				int num2 = int.Parse(list[i].monsterM.maxLevel);
				if (num >= num2)
				{
					GUIMonsterIcon icon3 = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(list[i]);
					this.iconGrayOut.BlockLevelMax(icon3);
				}
			}
		}
		this.csSelectPanelMonsterIcon.SetCheckEnablePushAction(new Func<MonsterData, bool>(this.CheckEnablePush));
		this.csSelectPanelMonsterIcon.useLocationRecord = true;
		this.targetMonsterList = list;
		list = MonsterDataMng.Instance().SelectionMDList(list);
		this.csSelectPanelMonsterIcon.AllBuild(list, localScale, new Action<MonsterData>(this.ActMIconLong), new Action<MonsterData>(this.ActMIconShort), false);
		if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.LABO || CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.MEDAL_INHERIT || CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.VERSION_UP)
		{
			bool flag2 = false;
			if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.MEDAL_INHERIT && CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.BASE)
			{
				flag2 = true;
			}
			if (!flag2)
			{
				if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.LABO)
				{
					this.monsterList.GrayOutPartyUsed();
				}
				foreach (MonsterData monsterData in this.targetMonsterList)
				{
					if (monsterData.userMonster.IsLocked)
					{
						GUIMonsterIcon icon4 = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterData);
						this.iconGrayOut.BlockLockMonster(icon4);
					}
					if (MonsterStatusData.IsSpecialTrainingType(monsterData.GetMonsterMaster().Group.monsterType) && CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.PARTNER)
					{
						GUIMonsterIcon icon5 = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterData);
						this.iconGrayOut.BlockSpecialTypeMonster(icon5);
					}
					if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.MEDAL_INHERIT && CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.PARTNER)
					{
						MonsterData baseDigimon2 = CMD_PairSelectBase.instance.baseDigimon;
						if (baseDigimon2 != null && baseDigimon2 != monsterData)
						{
							bool flag3 = false;
							for (int j = 0; j < deckUserMonsterList.Count; j++)
							{
								if (deckUserMonsterList[j] == monsterData)
								{
									flag3 = true;
									break;
								}
							}
							if (!flag3)
							{
								bool flag4 = false;
								MonsterAbilityStatusInfo mas = ClassSingleton<AbilityData>.Instance.CreateAbilityStatus(baseDigimon2, monsterData, ref flag4);
								float totalAbilityRate = ClassSingleton<AbilityData>.Instance.GetTotalAbilityRate(mas);
								if (totalAbilityRate <= 0f)
								{
									GUIMonsterIcon icon6 = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterData);
									this.iconGrayOut.BlockMedalNoPossibility(icon6);
								}
							}
						}
					}
				}
			}
			if (CMD_BaseSelect.DataChg != null)
			{
				if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.BASE)
				{
					if (CMD_BaseSelect.DataChg != null)
					{
						GUIMonsterIcon icon7 = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(CMD_BaseSelect.DataChg);
						this.iconGrayOut.SetSelectText(icon7, StringMaster.GetString("SystemBase"));
					}
				}
				else if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.PARTNER)
				{
					GUIMonsterIcon icon8 = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(CMD_BaseSelect.DataChg);
					this.iconGrayOut.SetSelectText(icon8, "1");
				}
			}
		}
		else if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.MEAL && CMD_BaseSelect.DataChg != null)
		{
			if (CMD_BaseSelect.DataChg.userMonster.level.ToInt32() >= CMD_BaseSelect.DataChg.monsterM.maxLevel.ToInt32())
			{
				this.SetEmpty();
			}
			else
			{
				this.SetSelectedCharChg();
			}
		}
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
	}

	private void RemoveIcon(MonsterData md)
	{
		if (CMD_BaseSelect.DataChg != null)
		{
			this.ChangeSelectMonsterIcon(CMD_BaseSelect.DataChg);
			this.SetEmpty();
			this.CheckChip();
		}
	}

	private void ActMIconShort(MonsterData tappedMonsterData)
	{
		if (CMD_BaseSelect.DataChg != null)
		{
			this.ChangeSelectMonsterIcon(CMD_BaseSelect.DataChg);
		}
		CMD_BaseSelect.DataChg = tappedMonsterData;
		this.SetSelectedCharChg();
	}

	private void ChangeSelectMonsterIcon(MonsterData selectMonster)
	{
		GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(selectMonster);
		this.iconGrayOut.CancelSelect(icon);
		if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.EVOLVE)
		{
			this.SetEvolutionMonsterIcon(selectMonster);
		}
		else if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.VERSION_UP)
		{
			List<HaveSoulData> versionUpAlMightyMaterial = VersionUpMaterialData.GetVersionUpAlMightyMaterial();
			this.SetVersionUPMonsterIcon(selectMonster, versionUpAlMightyMaterial);
		}
	}

	private void ActMIconLong(MonsterData tappedMonsterData)
	{
		CMD_CharacterDetailed.DataChg = tappedMonsterData;
		if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.LABO || CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.MEDAL_INHERIT || CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.VERSION_UP)
		{
			bool flag = false;
			bool isCheckDim = true;
			if (tappedMonsterData == CMD_BaseSelect.DataChg || tappedMonsterData == CMD_PairSelectBase.instance.baseDigimon || tappedMonsterData == CMD_PairSelectBase.instance.partnerDigimon)
			{
				isCheckDim = false;
			}
			if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.LABO || CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.VERSION_UP)
			{
				if (tappedMonsterData == CMD_BaseSelect.DataChg || tappedMonsterData == CMD_PairSelectBase.instance.baseDigimon || tappedMonsterData == CMD_PairSelectBase.instance.partnerDigimon)
				{
					flag = true;
				}
			}
			else if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.MEDAL_INHERIT)
			{
				if (tappedMonsterData == CMD_PairSelectBase.instance.partnerDigimon)
				{
					flag = true;
				}
				if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.PARTNER)
				{
					if (tappedMonsterData == CMD_BaseSelect.DataChg)
					{
						flag = true;
					}
					MonsterData baseDigimon = CMD_PairSelectBase.instance.baseDigimon;
					if (baseDigimon != null && baseDigimon != tappedMonsterData)
					{
						bool flag2 = false;
						MonsterAbilityStatusInfo mas = ClassSingleton<AbilityData>.Instance.CreateAbilityStatus(baseDigimon, tappedMonsterData, ref flag2);
						float totalAbilityRate = ClassSingleton<AbilityData>.Instance.GetTotalAbilityRate(mas);
						if (totalAbilityRate <= 0f)
						{
							isCheckDim = false;
						}
					}
				}
				else if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.BASE && isCheckDim)
				{
					isCheckDim = false;
				}
			}
			if (ClassSingleton<MonsterUserDataMng>.Instance.FindMonsterColosseumDeck(tappedMonsterData.GetMonster().userMonsterId))
			{
				isCheckDim = false;
			}
			else
			{
				List<MonsterData> deckUserMonsterList = ClassSingleton<MonsterUserDataMng>.Instance.GetDeckUserMonsterList();
				foreach (MonsterData monsterData in deckUserMonsterList)
				{
					if (monsterData == tappedMonsterData)
					{
						isCheckDim = false;
					}
				}
			}
			CMD_CharacterDetailed cmd_CharacterDetailed = GUIMain.ShowCommonDialog(delegate(int i)
			{
				GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(tappedMonsterData);
				icon.Lock = tappedMonsterData.userMonster.IsLocked;
				if (isCheckDim)
				{
					if (tappedMonsterData.userMonster.IsLocked)
					{
						this.iconGrayOut.BlockLockMonsterReturnDetailed(icon);
					}
					else if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.PARTNER && MonsterStatusData.IsSpecialTrainingType(tappedMonsterData.GetMonsterMaster().Group.monsterType))
					{
						this.iconGrayOut.BlockLockMonsterReturnDetailed(icon);
					}
					else if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.VERSION_UP)
					{
						List<HaveSoulData> versionUpAlMightyMaterial = VersionUpMaterialData.GetVersionUpAlMightyMaterial();
						this.SetVersionUPMonsterIcon(tappedMonsterData, versionUpAlMightyMaterial);
					}
					else
					{
						this.iconGrayOut.CancelLockMonsterReturnDetailed(icon);
					}
				}
			}, "CMD_CharacterDetailed") as CMD_CharacterDetailed;
			if (flag)
			{
				cmd_CharacterDetailed.Mode = CMD_CharacterDetailed.LockMode.Laboratory;
			}
		}
		else
		{
			CMD_CharacterDetailed.DataChg = tappedMonsterData;
			CMD_CharacterDetailed cmd_CharacterDetailed2 = GUIMain.ShowCommonDialog(delegate(int i)
			{
				GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(tappedMonsterData);
				icon.Lock = tappedMonsterData.userMonster.IsLocked;
				if (this.leftLargeMonsterIcon != null && tappedMonsterData == CMD_CharacterDetailed.DataChg)
				{
					this.leftLargeMonsterIcon.Lock = tappedMonsterData.userMonster.IsLocked;
				}
			}, "CMD_CharacterDetailed") as CMD_CharacterDetailed;
			if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.EVOLVE)
			{
				cmd_CharacterDetailed2.Mode = CMD_CharacterDetailed.LockMode.Evolution;
			}
		}
	}

	public void SetEmpty()
	{
		CMD_BaseSelect.DataChg = null;
		this.chipBaseSelect.ClearChipIcons();
		this.ShowChgInfo();
		if (this.goLeftLargeMonsterIcon != null)
		{
			UnityEngine.Object.Destroy(this.goLeftLargeMonsterIcon);
		}
		this.goMN_ICON_CHG.SetActive(true);
	}

	private void SetSelectedCharChg()
	{
		if (CMD_BaseSelect.DataChg != null)
		{
			if (this.goLeftLargeMonsterIcon != null)
			{
				UnityEngine.Object.Destroy(this.goLeftLargeMonsterIcon);
			}
			if (this.goMN_ICON_CHG == null)
			{
				return;
			}
			Transform transform = this.goMN_ICON_CHG.transform;
			this.leftLargeMonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(CMD_BaseSelect.DataChg, transform.localScale, transform.localPosition, transform.parent, true, false);
			this.goLeftLargeMonsterIcon = this.leftLargeMonsterIcon.gameObject;
			this.goLeftLargeMonsterIcon.SetActive(true);
			this.leftLargeMonsterIcon.Data = CMD_BaseSelect.DataChg;
			this.chipBaseSelect.SetSelectedCharChg(CMD_BaseSelect.DataChg);
			if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.LABO || CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.MEDAL_INHERIT || CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.VERSION_UP)
			{
				GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(CMD_BaseSelect.DataChg);
				if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.BASE)
				{
					this.iconGrayOut.SetSelectText(icon, StringMaster.GetString("SystemBase"));
					this.iconGrayOut.SelectIcon(this.leftLargeMonsterIcon);
				}
				else if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.PARTNER)
				{
					this.iconGrayOut.SetSelectText(icon, "1");
					this.iconGrayOut.SelectIcon(this.leftLargeMonsterIcon);
				}
			}
			else
			{
				GUIMonsterIcon icon2 = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(CMD_BaseSelect.DataChg);
				this.iconGrayOut.SetSelect(icon2);
				this.iconGrayOut.SelectIcon(this.leftLargeMonsterIcon);
			}
			this.leftLargeMonsterIcon.SetTouchAct_L(new Action<MonsterData>(this.ActMIconLong));
			this.leftLargeMonsterIcon.Lock = CMD_BaseSelect.DataChg.userMonster.IsLocked;
			UIWidget component = this.goMN_ICON_CHG.GetComponent<UIWidget>();
			UIWidget component2 = this.leftLargeMonsterIcon.gameObject.GetComponent<UIWidget>();
			if (component != null && component2 != null)
			{
				int add = component.depth - component2.depth;
				DepthController component3 = this.leftLargeMonsterIcon.gameObject.GetComponent<DepthController>();
				component3.AddWidgetDepth(this.leftLargeMonsterIcon.transform, add);
			}
			this.goMN_ICON_CHG.SetActive(false);
			this.ShowChgInfo();
			this.CheckChip();
		}
	}

	private void CheckChip()
	{
		bool decideButton = true;
		if (CMD_BaseSelect.DataChg == null)
		{
			decideButton = false;
		}
		else if (CMD_BaseSelect.BaseType == CMD_BaseSelect.BASE_TYPE.MEAL)
		{
			if (int.Parse(CMD_BaseSelect.DataChg.userMonster.level) >= int.Parse(CMD_BaseSelect.DataChg.monsterM.maxLevel))
			{
				decideButton = false;
			}
			List<UserFacility> userFacilityList = Singleton<UserDataMng>.Instance.GetUserFacilityList();
			if (!userFacilityList.Exists((UserFacility x) => x.facilityId == 1))
			{
				decideButton = true;
			}
		}
		this.SetDecideButton(decideButton);
	}

	public void SetDecideButton(bool isExecutable)
	{
		if (isExecutable)
		{
			this.ngBTN_DECIDE.spriteName = "Common02_Btn_Blue";
			this.ngTX_DECIDE.color = Color.white;
			this.clBTN_DECIDE.activeCollider = true;
		}
		else
		{
			this.ngBTN_DECIDE.spriteName = "Common02_Btn_Gray";
			this.ngTX_DECIDE.color = Color.gray;
			this.clBTN_DECIDE.activeCollider = false;
		}
	}

	private DataMng.ExperienceInfo GetExpInfo()
	{
		DataMng dataMng = DataMng.Instance();
		int meat_num = 1;
		int expFromMeat = dataMng.GetExpFromMeat(meat_num);
		int exp = int.Parse(CMD_BaseSelect.DataChg.userMonster.ex) + expFromMeat;
		return dataMng.GetExperienceInfo(exp);
	}

	public void StatusPageChangeTap()
	{
		this.switchDetailSkillPanel(false);
		this.StatusPageChange(true);
	}

	private void StatusPageChange(bool pageChange)
	{
		if (CMD_BaseSelect.DataChg != null)
		{
			if (pageChange)
			{
				if (this.statusPage < this.goStatusPanelPage.Count)
				{
					this.statusPage++;
				}
				else
				{
					this.statusPage = 1;
				}
			}
			int num = 1;
			foreach (GameObject gameObject in this.goStatusPanelPage)
			{
				if (num == this.statusPage)
				{
					gameObject.SetActive(true);
					this.switchSkillPanelBtn.SetActive(gameObject.name == "PartsStatusSkill");
				}
				else
				{
					gameObject.SetActive(false);
				}
				num++;
			}
		}
	}

	private void RequestStatusPage(int requestPage)
	{
		this.statusPage = requestPage;
		if (this.statusPage >= this.goStatusPanelPage.Count || this.statusPage < 1)
		{
			this.statusPage = 1;
		}
		int num = 1;
		foreach (GameObject gameObject in this.goStatusPanelPage)
		{
			if (num == this.statusPage)
			{
				gameObject.SetActive(true);
				this.switchSkillPanelBtn.SetActive(gameObject.name == "PartsStatusSkill");
			}
			else
			{
				gameObject.SetActive(false);
			}
			num++;
		}
	}

	private void LeadBuildMeat(int index)
	{
		if (index == 0)
		{
			base.SetCloseAction(new Action<int>(this.CloseAllCommonDialog));
			this.ClosePanel(true);
		}
	}

	private void CloseAllCommonDialog(int noop)
	{
		GUIManager.CloseAllCommonDialog(delegate
		{
			GUIFace.ForceHideDigiviceBtn_S();
			GUIFace.ForceHideFacilityBtn_S();
			GUIMain.ShowCommonDialog(null, "CMD_FacilityShop");
		});
	}

	public void OnTouchedEvoltionItemListBtn()
	{
		GUIMain.ShowCommonDialog(null, "CMD_EvolutionItemList");
	}

	public void switchDetailSkillPanel(bool isOpen)
	{
		this.goDetailedSkillPanel.SetActive(isOpen);
		UISprite component = this.switchSkillPanelBtn.GetComponent<UISprite>();
		if (isOpen)
		{
			component.flip = UIBasicSprite.Flip.Vertically;
		}
		else
		{
			component.flip = UIBasicSprite.Flip.Nothing;
		}
	}

	public void OnSwitchSkillPanelBtn()
	{
		this.switchDetailSkillPanel(!this.goDetailedSkillPanel.activeSelf);
	}

	private bool CheckEnablePush(MonsterData monsterData)
	{
		bool result = true;
		switch (CMD_BaseSelect.BaseType)
		{
		case CMD_BaseSelect.BASE_TYPE.MEAL:
			if (MonsterStatusData.IsLevelMax(monsterData.userMonster.monsterId, monsterData.userMonster.level))
			{
				result = false;
			}
			break;
		case CMD_BaseSelect.BASE_TYPE.LABO:
			if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.BASE && monsterData == CMD_PairSelectBase.instance.partnerDigimon)
			{
				result = false;
			}
			else if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.PARTNER && monsterData == CMD_PairSelectBase.instance.baseDigimon)
			{
				result = false;
			}
			else if (monsterData.userMonster.IsLocked)
			{
				result = false;
			}
			else
			{
				List<string> deckUserMonsterIdList = ClassSingleton<MonsterUserDataMng>.Instance.GetDeckUserMonsterIdList();
				for (int i = 0; i < deckUserMonsterIdList.Count; i++)
				{
					if (deckUserMonsterIdList[i] == monsterData.userMonster.userMonsterId)
					{
						result = false;
						break;
					}
				}
				List<MonsterData> colosseumDeckUserMonsterList = ClassSingleton<MonsterUserDataMng>.Instance.GetColosseumDeckUserMonsterList();
				for (int j = 0; j < colosseumDeckUserMonsterList.Count; j++)
				{
					if (colosseumDeckUserMonsterList[j] == monsterData)
					{
						result = false;
						break;
					}
				}
				if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.PARTNER && MonsterStatusData.IsSpecialTrainingType(monsterData.GetMonsterMaster().Group.monsterType))
				{
					result = false;
				}
			}
			break;
		case CMD_BaseSelect.BASE_TYPE.MEDAL_INHERIT:
			if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.BASE && monsterData == CMD_PairSelectBase.instance.partnerDigimon)
			{
				result = false;
			}
			else if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.PARTNER)
			{
				if (monsterData == CMD_PairSelectBase.instance.baseDigimon)
				{
					result = false;
				}
				else if (monsterData.userMonster.IsLocked)
				{
					result = false;
				}
				List<string> deckUserMonsterIdList2 = ClassSingleton<MonsterUserDataMng>.Instance.GetDeckUserMonsterIdList();
				for (int k = 0; k < deckUserMonsterIdList2.Count; k++)
				{
					if (deckUserMonsterIdList2[k] == monsterData.userMonster.userMonsterId)
					{
						result = false;
						break;
					}
				}
				List<MonsterData> colosseumDeckUserMonsterList2 = ClassSingleton<MonsterUserDataMng>.Instance.GetColosseumDeckUserMonsterList();
				for (int l = 0; l < colosseumDeckUserMonsterList2.Count; l++)
				{
					if (colosseumDeckUserMonsterList2[l] == monsterData)
					{
						result = false;
						break;
					}
				}
				if (MonsterStatusData.IsSpecialTrainingType(monsterData.GetMonsterMaster().Group.monsterType))
				{
					result = false;
				}
			}
			break;
		case CMD_BaseSelect.BASE_TYPE.VERSION_UP:
			if (monsterData.userMonster.IsLocked)
			{
				result = false;
			}
			else
			{
				List<string> deckUserMonsterIdList3 = ClassSingleton<MonsterUserDataMng>.Instance.GetDeckUserMonsterIdList();
				for (int m = 0; m < deckUserMonsterIdList3.Count; m++)
				{
					if (deckUserMonsterIdList3[m] == monsterData.userMonster.userMonsterId)
					{
						result = false;
						break;
					}
				}
				List<MonsterData> colosseumDeckUserMonsterList3 = ClassSingleton<MonsterUserDataMng>.Instance.GetColosseumDeckUserMonsterList();
				for (int n = 0; n < colosseumDeckUserMonsterList3.Count; n++)
				{
					if (colosseumDeckUserMonsterList3[n] == monsterData)
					{
						result = false;
						break;
					}
				}
			}
			break;
		}
		return result;
	}

	private void SetEvolutionMonsterIconList(List<MonsterData> monsterDataList)
	{
		for (int i = 0; i < monsterDataList.Count; i++)
		{
			this.SetEvolutionMonsterIcon(monsterDataList[i]);
		}
	}

	private void SetEvolutionMonsterIcon(MonsterData monster)
	{
		bool canEvolve = ClassSingleton<EvolutionData>.Instance.CanEvolution(monster.GetMonster().monsterId, monster.IsMaxLevel());
		GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monster);
		this.iconGrayOut.SetEvolutionIcon(icon, canEvolve, GUIMonsterIcon.GetIconGrayOutType(CMD_BaseSelect.IconSortType));
	}

	private void SetVersionUPMonsterIconList(List<MonsterData> monsterDataList)
	{
		List<HaveSoulData> versionUpAlMightyMaterial = VersionUpMaterialData.GetVersionUpAlMightyMaterial();
		for (int i = 0; i < monsterDataList.Count; i++)
		{
			this.SetVersionUPMonsterIcon(monsterDataList[i], versionUpAlMightyMaterial);
		}
	}

	private void SetVersionUPMonsterIcon(MonsterData monster, List<HaveSoulData> almightyMaterialList)
	{
		bool canVersionUp = VersionUpMaterialData.CanVersionUp(monster.GetMonsterMaster().Simple, monster.GetMonster(), almightyMaterialList);
		GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monster);
		this.iconGrayOut.SetVersionUpIcon(icon, canVersionUp, GUIMonsterIcon.GetIconGrayOutType(CMD_BaseSelect.IconSortType));
	}

	public enum BASE_TYPE
	{
		None,
		EVOLVE,
		MEAL,
		LABO,
		CHIP,
		MEDAL_INHERIT,
		VERSION_UP
	}

	public enum ELEMENT_TYPE
	{
		BASE,
		PARTNER
	}
}
