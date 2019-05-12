using Cutscene;
using Evolution;
using Master;
using Monster;
using MonsterList.TranceResistance;
using ResistanceTrance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class CMD_ArousalTOP : CMD
{
	[SerializeField]
	private GameObject _goUI_LEFT_ITEM_LIST_DEFAULT;

	[SerializeField]
	private GameObject _goUI_LEFT_ITEM_LIST_MAX_RANK;

	[SerializeField]
	private List<GameObject> _goMN_ICON_MAT_LIST;

	[SerializeField]
	private List<GameObject> _goMN_ICON_LIST;

	[SerializeField]
	private GameObject _goMN_ICON_CHG;

	[SerializeField]
	private GameObject _goUI_TEXT;

	[SerializeField]
	private List<GameObject> _goITEM_ICON_LIST;

	[SerializeField]
	private List<GameObject> _goITEM_ICON_LIST_MAX_RANK;

	[SerializeField]
	private GameObject _goITEM_ICON_MAX_RANK_SP;

	[SerializeField]
	private GameObject _goITEM_LABEL_MAX_RANK_SP;

	[SerializeField]
	private List<GameObject> _goITEM_NUM_TEXT_LIST;

	[SerializeField]
	private List<GameObject> _goITEM_NUM_TEXT_LIST_MAX_RANK;

	[SerializeField]
	private MonsterBasicInfo monsterBasicInfo;

	[SerializeField]
	private MonsterResistanceList monsterResistanceList;

	[SerializeField]
	private UISprite _ngBTN_DECIDE;

	[SerializeField]
	private GUICollider _clBTN_DECIDE;

	[SerializeField]
	private UILabel _ngTX_DECIDE;

	[Header("チップ装備の処理は分離")]
	[SerializeField]
	private ChipBaseSelect chipBaseSelect;

	[Header("パートナーデジモンのラベル")]
	[SerializeField]
	private UILabel partnerTitleLabel;

	[Header("決定ボタンのラベル")]
	[SerializeField]
	private UILabel buttonSubmitLabel;

	[SerializeField]
	private UILabel buttonSortLabel;

	[SerializeField]
	private UILabel buttonEvolutionMaterialListLabel;

	[SerializeField]
	private GUICollider ResistanceBtn;

	[SerializeField]
	private GUICollider AttributeBtn;

	[SerializeField]
	private UILabel ResistanceLabel;

	[SerializeField]
	private UILabel AttributeLabel;

	[SerializeField]
	private BtnSort sortButton;

	private GUIMonsterIcon leftLargeMonsterIcon;

	private GameObject _goSelectPanelMonsterIcon;

	private GUISelectPanelMonsterIcon _csSelectPanelMonsterIcon;

	private List<MonsterData> deckMDList;

	private string oldResistanceIds;

	private readonly Color COLOR_NON = new Color(0.274509817f, 0.274509817f, 0.274509817f, 1f);

	private readonly Color COLOR_ACT = new Color(1f, 1f, 1f, 1f);

	private bool _itemSufficient;

	private List<MonsterData> targetMonsterList;

	private CMD_CharacterDetailed detailWindow;

	private string resistanceTabOff = "Common02_Btn_tab_2";

	private string resistanceTabOn = "Common02_Btn_tab_1";

	private readonly Color TAB_COLOR_OFF = new Color(0.5137255f, 0.5137255f, 0.5137255f, 1f);

	private readonly Color TAB_COLOR_ON = new Color(1f, 1f, 1f, 1f);

	private bool userVarUpPossession;

	private CMD_ArousalTOP.ArousalSelectState arousalSelectState = CMD_ArousalTOP.ArousalSelectState.ATTRIBUTE;

	private List<MonsterData> partnerMonsterList = new List<MonsterData>();

	private GUIMonsterIcon partnerMonsterIcon;

	private TranceResistanceIconGrayOut iconGrayOut;

	private TranceResistanceMonsterList monsterList;

	[CompilerGenerated]
	private static Action <>f__mg$cache0;

	[CompilerGenerated]
	private static Action <>f__mg$cache1;

	private MonsterData baseDigimon { get; set; }

	protected override void Awake()
	{
		base.Awake();
		this.deckMDList = ClassSingleton<MonsterUserDataMng>.Instance.GetDeckUserMonsterList();
		this.iconGrayOut = new TranceResistanceIconGrayOut();
		this.iconGrayOut.SetNormalAction(new Action<MonsterData>(this.ActMIconShort), new Action<MonsterData>(this.ActMIconLong));
		this.iconGrayOut.SetSelectedAction(new Action<MonsterData>(this.ActMIconS_Remove), new Action<MonsterData>(this.ActMIconLong));
		this.iconGrayOut.SetBlockAction(null, new Action<MonsterData>(this.ActMIconLong));
		this.monsterList = new TranceResistanceMonsterList();
		this.monsterList.Initialize(this.deckMDList, ClassSingleton<MonsterUserDataMng>.Instance.GetColosseumDeckUserMonsterList(), this.iconGrayOut);
		this.chipBaseSelect.ClearChipIcons();
		for (int i = 0; i < this._goMN_ICON_LIST.Count; i++)
		{
			this._goMN_ICON_LIST[i].SetActive(false);
		}
		this.partnerTitleLabel.text = StringMaster.GetString("ArousalPartner");
		this.buttonSubmitLabel.text = StringMaster.GetString("SystemButtonDecision");
		this.buttonSortLabel.text = StringMaster.GetString("SystemSortButton");
		this.buttonEvolutionMaterialListLabel.text = StringMaster.GetString("EvolutionMaterialList");
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		GUICollider.DisableAllCollider("CMD_ArousalTOP");
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		base.PartsTitle.SetTitle(StringMaster.GetString("ArousalTitle"));
		this.SetCommonUI();
		this.InitMonsterList(true, false);
		this.ShowChgInfo();
		this.SetInactiveAllItemIcon(this._goITEM_ICON_LIST, this._goITEM_NUM_TEXT_LIST);
		this.SetInactiveAllItemIcon(this._goITEM_ICON_LIST_MAX_RANK, this._goITEM_NUM_TEXT_LIST_MAX_RANK);
		APIRequestTask task = Singleton<UserDataMng>.Instance.RequestUserSoulData(false);
		base.StartCoroutine(task.Run(delegate
		{
			RestrictionInput.EndLoad();
			this.ShowDLG();
			this.SetTutorialAnyTime("anytime_second_tutorial_arousal");
			this.<Show>__BaseCallProxy0(f, sizeX, sizeY, aT);
		}, delegate(Exception nop)
		{
			RestrictionInput.EndLoad();
			GUICollider.EnableAllCollider("CMD_ArousalTOP");
			this.ClosePanel(true);
		}, null));
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
			string tutorialName = "second_tutorial_arousal";
			if (CMD_ArousalTOP.<>f__mg$cache0 == null)
			{
				CMD_ArousalTOP.<>f__mg$cache0 = new Action(GUIMain.BarrierOFF);
			}
			tutorialObserver2.StartSecondTutorial(tutorialName, CMD_ArousalTOP.<>f__mg$cache0, delegate
			{
				GUICollider.EnableAllCollider("CMD_ArousalTOP");
			});
		}
		else
		{
			GUICollider.EnableAllCollider("CMD_ArousalTOP");
		}
	}

	public override void ClosePanel(bool animation = true)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	private void SetCommonUI()
	{
		this._goSelectPanelMonsterIcon = GUIManager.LoadCommonGUI("SelectListPanel/SelectListPanelMonsterIcon", base.gameObject);
		this._csSelectPanelMonsterIcon = this._goSelectPanelMonsterIcon.GetComponent<GUISelectPanelMonsterIcon>();
		if (null != this.goEFC_RIGHT)
		{
			this._goSelectPanelMonsterIcon.transform.parent = this.goEFC_RIGHT.transform;
		}
		Vector3 localPosition = this._goSelectPanelMonsterIcon.transform.localPosition;
		localPosition.x = 208f;
		GUICollider component = this._goSelectPanelMonsterIcon.GetComponent<GUICollider>();
		component.SetOriginalPos(localPosition);
		this._csSelectPanelMonsterIcon.ListWindowViewRect = ConstValue.GetRectWindow2();
		this.sortButton.OnChangeSortType = new Action(this.OnChangeSortSetting);
	}

	public void InitMonsterList(bool initLoc = true, bool var2 = false)
	{
		ClassSingleton<GUIMonsterIconList>.Instance.ResetIconState();
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		List<MonsterData> list = monsterDataMng.GetMonsterDataList();
		list = MonsterFilter.Filter(list, MonsterFilterType.ALL_OUT_GARDEN);
		this.userVarUpPossession = false;
		for (int i = 0; i < list.Count; i++)
		{
			if (MonsterStatusData.IsVersionUp(list[i].GetMonsterMaster().Simple.rare))
			{
				this.userVarUpPossession = true;
			}
		}
		if (var2)
		{
			list = MonsterFilter.Filter(list, MonsterFilterType.ALL_VERSION_UP);
		}
		else
		{
			list = MonsterFilter.Filter(list, MonsterFilterType.ALL_OUT_GARDEN);
		}
		monsterDataMng.SortMDList(list);
		monsterDataMng.SetSortLSMessage();
		ClassSingleton<GUIMonsterIconList>.Instance.SetLockIcon();
		this._csSelectPanelMonsterIcon.SetCheckEnablePushAction(null);
		this._csSelectPanelMonsterIcon.useLocationRecord = true;
		this.targetMonsterList = list;
		list = MonsterDataMng.Instance().SelectionMDList(list);
		this._csSelectPanelMonsterIcon.initLocation = initLoc;
		Vector3 localScale = this._goMN_ICON_LIST[0].transform.localScale;
		this._csSelectPanelMonsterIcon.AllBuild(list, localScale, new Action<MonsterData>(this.ActMIconLong), new Action<MonsterData>(this.ActMIconShort), false);
		this._csSelectPanelMonsterIcon.ClearIconDungeonBonus();
		this.sortButton.SortTargetMonsterList = this.targetMonsterList;
	}

	private void OnChangeSortSetting()
	{
		MonsterDataMng.Instance().SortMDList(this.targetMonsterList);
		MonsterDataMng.Instance().SetSortLSMessage();
		List<MonsterData> dts = MonsterDataMng.Instance().SelectionMDList(this.targetMonsterList);
		this._csSelectPanelMonsterIcon.ReAllBuild(dts);
		if (this.baseDigimon != null || 0 < this.partnerMonsterList.Count)
		{
			this.monsterList.SetGrayOutIconPartyUsedMonster(this.baseDigimon);
			this.monsterList.SetIconGrayOutPartnerMonster(this.baseDigimon, this.partnerMonsterList);
		}
	}

	private void ActMIconLong(MonsterData monsterData)
	{
		this.ShowDetail(monsterData);
	}

	private CMD_CharacterDetailed ShowDetail(MonsterData tappedMonsterData)
	{
		CMD_CharacterDetailed.DataChg = tappedMonsterData;
		bool flag = this.CheckPartnerMonster(tappedMonsterData);
		CMD_CharacterDetailed cmd_CharacterDetailed = GUIMain.ShowCommonDialog(delegate(int i)
		{
			if (tappedMonsterData == this.baseDigimon)
			{
				this.leftLargeMonsterIcon.Lock = tappedMonsterData.userMonster.IsLocked;
			}
			GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(tappedMonsterData);
			if (null != icon)
			{
				icon.Lock = tappedMonsterData.userMonster.IsLocked;
				if (this.GetTargetMonsterGroupId(tappedMonsterData.GetMonsterMaster().Group.monsterGroupId) && !MonsterGrowStepData.IsUltimateScope(tappedMonsterData.GetMonsterMaster().Group.growStep) && !MonsterStatusData.IsSpecialTrainingType(tappedMonsterData.GetMonsterMaster().Group.monsterType) && this.IsPartnerCandidateMonster(tappedMonsterData))
				{
					this.iconGrayOut.LockIconReturnDetailed(icon, tappedMonsterData.userMonster.IsLocked);
				}
			}
		}, "CMD_CharacterDetailed", null) as CMD_CharacterDetailed;
		if (flag)
		{
			cmd_CharacterDetailed.Mode = CMD_CharacterDetailed.LockMode.Arousal;
		}
		return cmd_CharacterDetailed;
	}

	private bool IsDeckMonster(string userMonsterId)
	{
		bool flag = false;
		for (int i = 0; i < this.deckMDList.Count; i++)
		{
			if (userMonsterId == this.deckMDList[i].userMonster.userMonsterId)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			flag = ClassSingleton<MonsterUserDataMng>.Instance.FindMonsterColosseumDeck(userMonsterId);
		}
		return flag;
	}

	private bool GetTargetMonsterGroupId(string monsterGroupId)
	{
		string a = string.Empty;
		if (this.baseDigimon != null)
		{
			a = this.baseDigimon.monsterMG.monsterGroupId;
		}
		else if (0 < this.partnerMonsterList.Count)
		{
			a = this.partnerMonsterList[0].monsterMG.monsterGroupId;
		}
		return a == monsterGroupId;
	}

	private bool CheckPartnerMonster(MonsterData targetMonster)
	{
		bool result = false;
		for (int i = 0; i < this.partnerMonsterList.Count; i++)
		{
			if (this.partnerMonsterList[i] == targetMonster)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	private bool IsPartnerCandidateMonster(MonsterData targetMonster)
	{
		bool result = false;
		if (this.baseDigimon != null && this.baseDigimon != targetMonster && !this.CheckPartnerMonster(targetMonster) && !this.IsDeckMonster(targetMonster.userMonster.userMonsterId))
		{
			result = true;
		}
		return result;
	}

	private void ActMIconShort(MonsterData tapMonster)
	{
		GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(tapMonster);
		this.iconGrayOut.SetSelect(icon);
		if (this.baseDigimon == null)
		{
			this.baseDigimon = tapMonster;
			this.chipBaseSelect.SetSelectedCharChg(tapMonster);
			if (this.arousalSelectState == CMD_ArousalTOP.ArousalSelectState.ATTRIBUTE)
			{
				this.SetAttributeResistanceMaterial(tapMonster.userMonster.monsterId, tapMonster.monsterMG.tribe, tapMonster.monsterMG.growStep);
			}
			else if (this.arousalSelectState == CMD_ArousalTOP.ArousalSelectState.RESISTANCE)
			{
				if (this.IsGrowStepMax(tapMonster))
				{
					this.ActiveMaxRankUI();
				}
				else
				{
					this.ActiveNomalRankUI();
				}
				if (MonsterStatusData.IsVersionUp(tapMonster.GetMonsterMaster().Simple.rare))
				{
					this.SetStatusAilmentItemIcon(tapMonster);
				}
			}
			this.leftLargeMonsterIcon = this.CreateIcon(this.baseDigimon, this._goMN_ICON_CHG);
			this.leftLargeMonsterIcon.SetTouchAct_S(new Action<MonsterData>(this.ActMIconS_Remove));
			this.ShowChgInfo();
			this.monsterList.SetGrayOutIconPartyUsedMonster(this.baseDigimon);
			this.monsterList.SetIconGrayOutPartnerMonster(this.baseDigimon, this.partnerMonsterList);
		}
		else if (1 > this.partnerMonsterList.Count)
		{
			this.partnerMonsterList.Add(tapMonster);
			this.partnerMonsterIcon = this.CreateIcon(tapMonster, this._goMN_ICON_MAT_LIST[0]);
			this.monsterList.SetIconGrayOutPartnerMonster(this.baseDigimon, this.partnerMonsterList);
		}
		this.BtnCont();
	}

	private void SetAttributeResistanceMaterial(string monsterId, string tribe, string growStep)
	{
		List<CMD_ArousalTOP.MaterialInfo> list = new List<CMD_ArousalTOP.MaterialInfo>();
		List<GameWebAPI.RespDataMA_GetMonsterTranceM.MonsterTranceM> materialList;
		bool resistanceTrance = AttributeResistance.GetResistanceTrance(monsterId, out materialList);
		if (resistanceTrance && AttributeResistance.IsSpecialTypeMonster(materialList))
		{
			this.ActiveMaxRankUI();
			if (this.GetMaterialInfo(materialList, ref list))
			{
				this.SetSpecialItemIcon(list);
				if (1 < list.Count)
				{
					this.SetItemIcon(list, 1);
				}
			}
		}
		else
		{
			if (resistanceTrance && AttributeResistance.IsNeedFragment(materialList))
			{
				this.ActiveMaxRankUI();
				if (this.GetMaterialInfo(materialList, ref list))
				{
					this.SetSpecialItemIcon(list);
				}
			}
			else
			{
				this.ActiveNomalRankUI();
			}
			int count = list.Count;
			List<GameWebAPI.RespDataMA_GetMonsterTribeTranceM.MonsterTribeTranceM> materialList2;
			if (AttributeResistance.GetResistanceTribeTrance(tribe, growStep, out materialList2) && this.GetTribeMaterialInfo(materialList2, ref list))
			{
				this.SetItemIcon(list, count);
			}
		}
		this._itemSufficient = true;
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].needNum > list[i].haveNum)
			{
				this._itemSufficient = false;
				break;
			}
		}
	}

	private void ActiveMaxRankUI()
	{
		this._goUI_LEFT_ITEM_LIST_MAX_RANK.SetActive(true);
		this._goUI_LEFT_ITEM_LIST_DEFAULT.SetActive(false);
	}

	private void ActiveNomalRankUI()
	{
		this._goUI_LEFT_ITEM_LIST_MAX_RANK.SetActive(false);
		this._goUI_LEFT_ITEM_LIST_DEFAULT.SetActive(true);
	}

	private bool GetMaterialInfo(List<GameWebAPI.RespDataMA_GetMonsterTranceM.MonsterTranceM> materialList, ref List<CMD_ArousalTOP.MaterialInfo> materialInfoList)
	{
		int count = materialList.Count;
		if (0 < count)
		{
			for (int i = 0; i < materialList.Count; i++)
			{
				CMD_ArousalTOP.MaterialInfo item = default(CMD_ArousalTOP.MaterialInfo);
				item.haveNum = this.UserMaterialNum(materialList[i].soulId);
				item.needNum = int.Parse(materialList[i].num);
				item.path = ClassSingleton<EvolutionData>.Instance.GetEvolveItemIconPathByID(materialList[i].soulId);
				materialInfoList.Add(item);
			}
		}
		return 0 < count;
	}

	private bool GetTribeMaterialInfo(List<GameWebAPI.RespDataMA_GetMonsterTribeTranceM.MonsterTribeTranceM> materialList, ref List<CMD_ArousalTOP.MaterialInfo> materialInfoList)
	{
		int count = materialList.Count;
		if (0 < count)
		{
			for (int i = 0; i < materialList.Count; i++)
			{
				CMD_ArousalTOP.MaterialInfo item = default(CMD_ArousalTOP.MaterialInfo);
				item.haveNum = this.UserMaterialNum(materialList[i].soulId);
				item.needNum = int.Parse(materialList[i].num);
				item.path = ClassSingleton<EvolutionData>.Instance.GetEvolveItemIconPathByID(materialList[i].soulId);
				materialInfoList.Add(item);
			}
		}
		return 0 < count;
	}

	private void SetMaterialIcon(CMD_ArousalTOP.MaterialInfo materialInfo, UITexture iconTexture, UILabel iconLabel)
	{
		iconTexture.gameObject.SetActive(true);
		iconLabel.gameObject.SetActive(true);
		NGUIUtil.ChangeUITextureFromFile(iconTexture, materialInfo.path, false);
		if (materialInfo.needNum <= materialInfo.haveNum)
		{
			iconTexture.color = this.COLOR_ACT;
		}
		else
		{
			iconTexture.color = this.COLOR_NON;
		}
		iconLabel.text = string.Format(StringMaster.GetString("SystemFractionWide"), materialInfo.haveNum, materialInfo.needNum);
	}

	private void SetSpecialItemIcon(List<CMD_ArousalTOP.MaterialInfo> materialList)
	{
		UITexture component = this._goITEM_ICON_MAX_RANK_SP.GetComponent<UITexture>();
		UILabel component2 = this._goITEM_LABEL_MAX_RANK_SP.GetComponent<UILabel>();
		if (null != component && null != component2)
		{
			this.SetMaterialIcon(materialList[0], component, component2);
		}
	}

	private void SetItemIcon(List<CMD_ArousalTOP.MaterialInfo> infoList, int mainMaterialNum)
	{
		List<GameObject> list;
		List<GameObject> list2;
		if (0 < mainMaterialNum)
		{
			list = this._goITEM_ICON_LIST_MAX_RANK;
			list2 = this._goITEM_NUM_TEXT_LIST_MAX_RANK;
		}
		else
		{
			list = this._goITEM_ICON_LIST;
			list2 = this._goITEM_NUM_TEXT_LIST;
		}
		int num = 0;
		for (int i = mainMaterialNum; i < infoList.Count; i++)
		{
			UITexture component = list[num].GetComponent<UITexture>();
			UILabel component2 = list2[num].GetComponent<UILabel>();
			if (null != component && null != component2)
			{
				this.SetMaterialIcon(infoList[i], component, component2);
			}
			num++;
		}
	}

	private void SetStatusAilmentItemIcon(MonsterData md)
	{
		GameWebAPI.RespDataMA_MonsterStatusAilmentMaster responseMonsterStatusAilmentMaster = MasterDataMng.Instance().ResponseMonsterStatusAilmentMaster;
		GameWebAPI.RespDataMA_MonsterStatusAilmentGroupMaster responseMonsterStatusAilmentGroupMaster = MasterDataMng.Instance().ResponseMonsterStatusAilmentGroupMaster;
		GameWebAPI.RespDataMA_MonsterStatusAilmentMaster.StatusAilment statusAilment = null;
		GameWebAPI.RespDataMA_MonsterStatusAilmentGroupMaster.StatusAilmentGroup selectAilmentGroupData = null;
		for (int i = 0; i < responseMonsterStatusAilmentMaster.monsterStatusAilmentM.Length; i++)
		{
			if (md.monsterM.monsterId == responseMonsterStatusAilmentMaster.monsterStatusAilmentM[i].monsterId)
			{
				statusAilment = responseMonsterStatusAilmentMaster.monsterStatusAilmentM[i];
			}
		}
		if (statusAilment == null)
		{
			return;
		}
		for (int j = 0; j < responseMonsterStatusAilmentGroupMaster.monsterStatusAilmentMaterialM.Length; j++)
		{
			if (statusAilment.monsterStatusAilmentMaterialId == responseMonsterStatusAilmentGroupMaster.monsterStatusAilmentMaterialM[j].monsterStatusAilmentMaterialId)
			{
				selectAilmentGroupData = responseMonsterStatusAilmentGroupMaster.monsterStatusAilmentMaterialM[j];
			}
		}
		if (selectAilmentGroupData == null)
		{
			return;
		}
		List<GameObject> goITEM_ICON_LIST_MAX_RANK = this._goITEM_ICON_LIST_MAX_RANK;
		List<GameObject> targetLabels = this._goITEM_NUM_TEXT_LIST_MAX_RANK;
		this._itemSufficient = true;
		goITEM_ICON_LIST_MAX_RANK.Select((GameObject value, int index) => new
		{
			index,
			value
		}).ToList().ForEach(delegate(a)
		{
			string text = string.Empty;
			text = selectAilmentGroupData.GetAssetValue(a.index + 1);
			string assetNum = selectAilmentGroupData.GetAssetNum(a.index + 1);
			if (string.IsNullOrEmpty(text) || "0" == text)
			{
				a.value.SetActive(false);
			}
			else
			{
				a.value.SetActive(true);
				this.SetTextureToIcon(a.value, text);
			}
			bool flag2 = this.SetItemNumToText(targetLabels[a.index], text, assetNum);
			if (flag2)
			{
				a.value.GetComponent<UITexture>().color = this.COLOR_ACT;
			}
			else
			{
				this._itemSufficient = false;
				a.value.GetComponent<UITexture>().color = this.COLOR_NON;
			}
		});
		this.SetTextureToIcon(this._goITEM_ICON_MAX_RANK_SP, statusAilment.assetValue);
		bool flag = this.SetItemNumToText(this._goITEM_LABEL_MAX_RANK_SP, statusAilment.assetValue, statusAilment.assetNum);
		if (flag)
		{
			this._goITEM_ICON_MAX_RANK_SP.GetComponent<UITexture>().color = this.COLOR_ACT;
		}
		else
		{
			this._itemSufficient = false;
			this._goITEM_ICON_MAX_RANK_SP.GetComponent<UITexture>().color = this.COLOR_NON;
		}
	}

	private void SetInactiveAllItemIcon(List<GameObject> icons, List<GameObject> labels)
	{
		icons.Select((GameObject value, int index) => new
		{
			index,
			value
		}).ToList().ForEach(delegate(a)
		{
			this.SetInactiveItemIcon(a.value, labels[a.index]);
		});
	}

	private void DisableMaterialIconRank(List<GameObject> iconList, List<GameObject> labelList)
	{
		for (int i = 0; i < iconList.Count; i++)
		{
			if (iconList[i].activeSelf)
			{
				iconList[i].SetActive(false);
			}
		}
		for (int j = 0; j < labelList.Count; j++)
		{
			if (labelList[j].activeSelf)
			{
				labelList[j].SetActive(false);
			}
		}
	}

	private void SetInactiveItemIcon(GameObject icon, GameObject label)
	{
		icon.SetActive(false);
		label.SetActive(false);
	}

	private void SetActiveAllItem(bool flg)
	{
		this._goITEM_ICON_LIST.ForEach(delegate(GameObject n)
		{
			n.SetActive(flg);
		});
		this._goITEM_NUM_TEXT_LIST.ForEach(delegate(GameObject n)
		{
			n.SetActive(flg);
		});
	}

	private void SetTextureToIcon(GameObject go, string soulId)
	{
		string evolveItemIconPathByID = ClassSingleton<EvolutionData>.Instance.GetEvolveItemIconPathByID(soulId);
		UITexture component = go.GetComponent<UITexture>();
		if (null != component)
		{
			go.SetActive(true);
			NGUIUtil.ChangeUITextureFromFile(component, evolveItemIconPathByID, false);
		}
	}

	private int UserMaterialNum(string materialId)
	{
		int result = 0;
		GameWebAPI.UserSoulData[] userSoulData = DataMng.Instance().RespDataUS_SoulInfo.userSoulData;
		for (int i = 0; i < userSoulData.Length; i++)
		{
			if (userSoulData[i].soulId == materialId)
			{
				result = int.Parse(userSoulData[i].num);
				break;
			}
		}
		return result;
	}

	private bool SetItemNumToText(GameObject go, string soulId, string needNum)
	{
		if ("0" == needNum)
		{
			go.SetActive(false);
			return true;
		}
		GameWebAPI.UserSoulData[] userSoulData = DataMng.Instance().RespDataUS_SoulInfo.userSoulData;
		string text = userSoulData.Where((GameWebAPI.UserSoulData n) => n.soulId == soulId).Select((GameWebAPI.UserSoulData n) => n.num).SingleOrDefault<string>();
		text = ((text != null) ? text : "0");
		go.SetActive(true);
		UILabel component = go.GetComponent<UILabel>();
		component.text = string.Format(StringMaster.GetString("SystemFractionWide"), text, needNum);
		return int.Parse(text) >= int.Parse(needNum);
	}

	protected override void WindowClosed()
	{
		ClassSingleton<GUIMonsterIconList>.Instance.PushBackAllMonsterPrefab();
		base.WindowClosed();
	}

	private GUIMonsterIcon CreateIcon(MonsterData md, GameObject goEmpty)
	{
		Transform transform = goEmpty.transform;
		GUIMonsterIcon guimonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(md, transform.localScale, transform.localPosition, transform.parent, true, false);
		this.iconGrayOut.SelectIcon(guimonsterIcon);
		guimonsterIcon.Lock = md.userMonster.IsLocked;
		UIWidget component = goEmpty.GetComponent<UIWidget>();
		UIWidget component2 = guimonsterIcon.gameObject.GetComponent<UIWidget>();
		if (null != component && null != component2)
		{
			int add = component.depth - component2.depth;
			DepthController component3 = guimonsterIcon.gameObject.GetComponent<DepthController>();
			component3.AddWidgetDepth(guimonsterIcon.transform, add);
		}
		goEmpty.SetActive(false);
		return guimonsterIcon;
	}

	private void ActMIconS_Remove(MonsterData md)
	{
		if (md == this.baseDigimon)
		{
			UnityEngine.Object.Destroy(this.leftLargeMonsterIcon.gameObject);
			this.leftLargeMonsterIcon = null;
			this._goMN_ICON_CHG.SetActive(true);
			GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(md);
			this.iconGrayOut.CancelSelect(icon);
			if (this._goUI_LEFT_ITEM_LIST_MAX_RANK.activeSelf)
			{
				this._goUI_LEFT_ITEM_LIST_MAX_RANK.SetActive(false);
				this.DisableMaterialIconRank(this._goITEM_ICON_LIST_MAX_RANK, this._goITEM_NUM_TEXT_LIST_MAX_RANK);
				this._goUI_LEFT_ITEM_LIST_DEFAULT.SetActive(true);
			}
			this.baseDigimon = null;
			if (0 >= this.partnerMonsterList.Count)
			{
				this.monsterList.ClearIconGrayOutPartnerMonster(this.baseDigimon, this.partnerMonsterList);
				this.monsterList.ClearGrayOutIconPartyUsedMonster();
				this.SetActiveAllItem(false);
			}
			else
			{
				this.monsterList.ClearIconGrayOutPartnerMonster(this.baseDigimon, this.partnerMonsterList);
				this.monsterList.SetGrayOutIconPartyUsedMonster(this.baseDigimon);
				this.monsterList.SetIconGrayOutPartnerMonster(this.baseDigimon, this.partnerMonsterList);
			}
			this.ShowChgInfo();
			this.chipBaseSelect.ClearChipIcons();
		}
		else
		{
			for (int i = 0; i < this.partnerMonsterList.Count; i++)
			{
				if (this.partnerMonsterList[i] == md)
				{
					UnityEngine.Object.Destroy(this.partnerMonsterIcon.gameObject);
					this.partnerMonsterIcon = null;
					this._goMN_ICON_MAT_LIST[i].SetActive(true);
					GUIMonsterIcon icon2 = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(this.partnerMonsterList[i]);
					this.iconGrayOut.CancelSelect(icon2);
					this.partnerMonsterList.RemoveAt(i);
					this.ShiftPartnerIcon(i);
					break;
				}
			}
			if (this.baseDigimon == null)
			{
				this.SetActiveAllItem(false);
				this.monsterList.ClearIconGrayOutPartnerMonster(this.baseDigimon, this.partnerMonsterList);
				this.monsterList.ClearGrayOutIconPartyUsedMonster();
			}
			else
			{
				this.monsterList.ClearIconGrayOutPartnerMonster(this.baseDigimon, this.baseDigimon.monsterMG.monsterGroupId);
				this.monsterList.SetGrayOutIconPartyUsedMonster(this.baseDigimon);
			}
		}
		this.BtnCont();
	}

	private void SetMonsterIconReset()
	{
		if (this.baseDigimon != null)
		{
			if (null != this.leftLargeMonsterIcon)
			{
				Action<MonsterData> touchAct_S = this.leftLargeMonsterIcon.GetTouchAct_S();
				if (touchAct_S != null)
				{
					touchAct_S(this.leftLargeMonsterIcon.Data);
				}
			}
			if (null != this.partnerMonsterIcon)
			{
				Action<MonsterData> touchAct_S2 = this.partnerMonsterIcon.GetTouchAct_S();
				if (touchAct_S2 != null)
				{
					touchAct_S2(this.partnerMonsterIcon.Data);
				}
			}
		}
		for (int i = 0; i < this.partnerMonsterList.Count; i++)
		{
			if (this.partnerMonsterList[i] != null)
			{
				this._goMN_ICON_MAT_LIST[i].SetActive(true);
				GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(this.partnerMonsterList[i]);
				this.iconGrayOut.CancelSelect(icon);
				this.partnerMonsterList.RemoveAt(i);
				this.ShiftPartnerIcon(i);
			}
		}
		if (this.baseDigimon == null)
		{
			this.SetActiveAllItem(false);
			if (0 >= this.partnerMonsterList.Count)
			{
				this.monsterList.ClearIconGrayOutPartnerMonster(this.baseDigimon, this.partnerMonsterList);
				this.monsterList.ClearGrayOutIconPartyUsedMonster();
			}
		}
		this.BtnCont();
	}

	private void ShiftPartnerIcon(int noop)
	{
		for (int i = 0; i < this.partnerMonsterList.Count; i++)
		{
			this.partnerMonsterIcon.gameObject.transform.localPosition = this._goMN_ICON_MAT_LIST[i].transform.localPosition;
			this._goMN_ICON_MAT_LIST[i].SetActive(false);
		}
		for (int j = this.partnerMonsterList.Count; j < this._goMN_ICON_MAT_LIST.Count; j++)
		{
			this._goMN_ICON_MAT_LIST[j].SetActive(true);
		}
	}

	private void BtnCont()
	{
		bool flag = false;
		if (this.baseDigimon != null && this._itemSufficient)
		{
			if (this.arousalSelectState == CMD_ArousalTOP.ArousalSelectState.ATTRIBUTE)
			{
				List<GameWebAPI.RespDataMA_GetMonsterTranceM.MonsterTranceM> list;
				flag = (AttributeResistance.GetResistanceTrance(this.baseDigimon.userMonster.monsterId, out list) || 0 < this.partnerMonsterList.Count);
			}
			else
			{
				flag = (0 < this.partnerMonsterList.Count || this.IsGrowStepMax(this.baseDigimon));
			}
		}
		if (flag)
		{
			this._ngBTN_DECIDE.spriteName = "Common02_Btn_Blue";
			this._ngTX_DECIDE.color = Color.white;
			this._clBTN_DECIDE.activeCollider = true;
		}
		else
		{
			this._ngBTN_DECIDE.spriteName = "Common02_Btn_Gray";
			this._ngTX_DECIDE.color = Color.gray;
			this._clBTN_DECIDE.activeCollider = false;
		}
	}

	private void ShowChgInfo()
	{
		if (this.baseDigimon != null)
		{
			this.monsterBasicInfo.SetMonsterData(this.baseDigimon);
			this.monsterResistanceList.SetValues(this.baseDigimon);
		}
		else
		{
			this.monsterBasicInfo.ClearMonsterData();
			this.monsterResistanceList.ClearValues();
		}
	}

	private void OnTouchDecide()
	{
		CMD_ArousalCheck cmd_ArousalCheck = GUIMain.ShowCommonDialog(null, "CMD_ArousalCheck", null) as CMD_ArousalCheck;
		cmd_ArousalCheck.SetActionYesButton(new Action<CMD>(this.OnPushConfirmYesButton));
		bool isGrowStepMax;
		if (this.arousalSelectState == CMD_ArousalTOP.ArousalSelectState.ATTRIBUTE)
		{
			List<GameWebAPI.RespDataMA_GetMonsterTranceM.MonsterTranceM> list;
			isGrowStepMax = AttributeResistance.GetResistanceTrance(this.baseDigimon.userMonster.monsterId, out list);
		}
		else
		{
			isGrowStepMax = this.IsGrowStepMax(this.baseDigimon);
		}
		cmd_ArousalCheck.SetParams(this.baseDigimon, this.partnerMonsterList, isGrowStepMax);
	}

	private void OnPushConfirmYesButton(CMD confirmPopup)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		this.oldResistanceIds = this.baseDigimon.userMonster.tranceResistance;
		string userMonsterId = this.baseDigimon.userMonster.userMonsterId;
		string materialUserMonsterId = string.Empty;
		if (this.arousalSelectState == CMD_ArousalTOP.ArousalSelectState.ATTRIBUTE)
		{
			List<GameWebAPI.RespDataMA_GetMonsterTranceM.MonsterTranceM> list;
			if (AttributeResistance.GetResistanceTrance(this.baseDigimon.userMonster.monsterId, out list))
			{
				materialUserMonsterId = userMonsterId;
			}
			else
			{
				materialUserMonsterId = this.partnerMonsterList.FirstOrDefault<MonsterData>().userMonster.userMonsterId;
			}
		}
		else
		{
			this.oldResistanceIds = this.baseDigimon.userMonster.tranceStatusAilment;
			if (this.IsGrowStepMax(this.baseDigimon))
			{
				materialUserMonsterId = userMonsterId;
			}
			else
			{
				materialUserMonsterId = this.partnerMonsterList.FirstOrDefault<MonsterData>().userMonster.userMonsterId;
			}
		}
		confirmPopup.SetCloseAction(delegate(int noop)
		{
			this.RequestResistance(materialUserMonsterId);
		});
	}

	private void RequestResistance(string materialUserMonsterId)
	{
		GameWebAPI.RequestMN_MonsterTrance requestMN_MonsterTrance = new GameWebAPI.RequestMN_MonsterTrance();
		requestMN_MonsterTrance.SetSendData = delegate(GameWebAPI.MN_Req_Trunce param)
		{
			param.baseUserMonsterId = int.Parse(this.baseDigimon.userMonster.userMonsterId);
			param.materialUserMonsterId = int.Parse(materialUserMonsterId);
			param.type = (int)this.arousalSelectState;
		};
		requestMN_MonsterTrance.OnReceived = delegate(GameWebAPI.RespDataMN_TrunceExec response)
		{
			ClassSingleton<MonsterUserDataMng>.Instance.UpdateUserMonsterData(response.userMonster);
		};
		GameWebAPI.RequestMN_MonsterTrance request = requestMN_MonsterTrance;
		base.StartCoroutine(request.Run(delegate()
		{
			this.EndTrunce(materialUserMonsterId);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private bool IsGrowStepMax(MonsterData monster)
	{
		return monster != null && MonsterGrowStepData.IsUltimateScope(monster.monsterMG.growStep);
	}

	private void EndTrunce(string materialUserMonsterId)
	{
		if (this.arousalSelectState == CMD_ArousalTOP.ArousalSelectState.ATTRIBUTE)
		{
			List<GameWebAPI.RespDataMA_GetMonsterTranceM.MonsterTranceM> list;
			if (!AttributeResistance.GetResistanceTrance(this.baseDigimon.userMonster.monsterId, out list))
			{
				this.DeleteMaterialMonster(materialUserMonsterId);
			}
		}
		else if (!this.IsGrowStepMax(this.baseDigimon))
		{
			this.DeleteMaterialMonster(materialUserMonsterId);
		}
		APIRequestTask task = Singleton<UserDataMng>.Instance.RequestUserSoulData(true);
		base.StartCoroutine(task.Run(new Action(this.StartCutScene), null, null));
	}

	private void DeleteMaterialMonster(string userMonsterId)
	{
		string[] userMonsterIdList = new string[]
		{
			userMonsterId
		};
		ClassSingleton<MonsterUserDataMng>.Instance.DeleteUserMonsterData(userMonsterIdList);
		ChipDataMng.GetUserChipSlotData().RemoveChipData(userMonsterId, true);
	}

	private void StartCutScene()
	{
		CutsceneDataAwakening cutsceneDataAwakening = new CutsceneDataAwakening();
		cutsceneDataAwakening.path = "Cutscenes/Awakening";
		cutsceneDataAwakening.modelId = this.baseDigimon.GetMonsterMaster().Group.modelId;
		CutsceneDataAwakening cutsceneDataAwakening2 = cutsceneDataAwakening;
		if (CMD_ArousalTOP.<>f__mg$cache1 == null)
		{
			CMD_ArousalTOP.<>f__mg$cache1 = new Action(CutSceneMain.FadeReqCutSceneEnd);
		}
		cutsceneDataAwakening2.endCallback = CMD_ArousalTOP.<>f__mg$cache1;
		CutsceneDataAwakening cutsceneData = cutsceneDataAwakening;
		Loading.Invisible();
		CutSceneMain.FadeReqCutScene(cutsceneData, new Action(this.StartCutSceneCallBack), delegate()
		{
			this.detailWindow.StartAnimation();
			this.baseDigimon = null;
			this.BtnCont();
			RestrictionInput.EndLoad();
		}, 0.5f, 0.5f);
	}

	private void StartCutSceneCallBack()
	{
		UnityEngine.Object.Destroy(this.leftLargeMonsterIcon.gameObject);
		this.leftLargeMonsterIcon = null;
		this._goMN_ICON_CHG.SetActive(true);
		GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(this.baseDigimon);
		this.iconGrayOut.CancelSelect(icon);
		for (int i = 0; i < this.partnerMonsterList.Count; i++)
		{
			if (this.baseDigimon == null)
			{
				this.SetActiveAllItem(false);
			}
			this._goMN_ICON_MAT_LIST[i].SetActive(true);
			icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(this.partnerMonsterList[i]);
			this.iconGrayOut.CancelSelect(icon);
			this.partnerMonsterList.RemoveAt(i);
			this.ShiftPartnerIcon(i);
		}
		this.partnerMonsterList.Clear();
		if (null != this.partnerMonsterIcon)
		{
			UnityEngine.Object.Destroy(this.partnerMonsterIcon.gameObject);
			this.partnerMonsterIcon = null;
		}
		ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(MonsterDataMng.Instance().GetMonsterDataList());
		if (this.arousalSelectState == CMD_ArousalTOP.ArousalSelectState.ATTRIBUTE)
		{
			this.InitMonsterList(false, false);
		}
		else if (this.arousalSelectState == CMD_ArousalTOP.ArousalSelectState.RESISTANCE)
		{
			this.InitMonsterList(false, true);
		}
		string newResistanceIds = this.baseDigimon.userMonster.tranceResistance;
		if (this.arousalSelectState == CMD_ArousalTOP.ArousalSelectState.RESISTANCE)
		{
			newResistanceIds = this.baseDigimon.userMonster.tranceStatusAilment;
		}
		MonsterData monster = this.baseDigimon;
		this.detailWindow = CMD_CharacterDetailed.CreateWindow(this.baseDigimon, delegate()
		{
			icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monster);
			icon.Lock = monster.userMonster.IsLocked;
		}, this.baseDigimon.monsterM.resistanceId, this.oldResistanceIds, newResistanceIds);
		this.ShowChgInfo();
		this.monsterList.ClearGrayOutIconPartyUsedMonster();
		this.monsterList.ClearIconGrayOutPartnerMonster(this.baseDigimon, this.partnerMonsterList);
		this.monsterResistanceList.ClearValues();
		this.SetInactiveAllItemIcon(this._goITEM_ICON_LIST, this._goITEM_NUM_TEXT_LIST);
		this.SetInactiveAllItemIcon(this._goITEM_ICON_LIST_MAX_RANK, this._goITEM_NUM_TEXT_LIST_MAX_RANK);
		this.SetInactiveItemIcon(this._goITEM_ICON_MAX_RANK_SP, this._goITEM_LABEL_MAX_RANK_SP);
		this.ActiveNomalRankUI();
	}

	public void OnTouchedEvoltionItemListBtn()
	{
		GUIMain.ShowCommonDialog(null, "CMD_EvolutionItemList", null);
	}

	public void OnTouchedResistanceBtn()
	{
		if (!this.userVarUpPossession)
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("ArousalNonVersionUpTitle");
			cmd_ModalMessage.Info = StringMaster.GetString("ArousalNonVersionUp");
		}
		else if (this.arousalSelectState != CMD_ArousalTOP.ArousalSelectState.RESISTANCE)
		{
			this.arousalSelectState = CMD_ArousalTOP.ArousalSelectState.RESISTANCE;
			UISprite component = this.ResistanceBtn.gameObject.GetComponent<UISprite>();
			if (null != component)
			{
				component.spriteName = this.resistanceTabOn;
				this.ResistanceLabel.color = this.TAB_COLOR_ON;
			}
			UISprite component2 = this.AttributeBtn.gameObject.GetComponent<UISprite>();
			if (null != component2)
			{
				component2.spriteName = this.resistanceTabOff;
				this.AttributeLabel.color = this.TAB_COLOR_OFF;
			}
			this.SetMonsterIconReset();
			this.InitMonsterList(true, true);
			this._goUI_TEXT.SetActive(false);
		}
	}

	public void OnTouchedAttributeBtn()
	{
		if (this.arousalSelectState != CMD_ArousalTOP.ArousalSelectState.ATTRIBUTE)
		{
			this.arousalSelectState = CMD_ArousalTOP.ArousalSelectState.ATTRIBUTE;
			UISprite component = this.ResistanceBtn.gameObject.GetComponent<UISprite>();
			if (null != component)
			{
				component.spriteName = this.resistanceTabOff;
				this.ResistanceLabel.color = this.TAB_COLOR_OFF;
			}
			UISprite component2 = this.AttributeBtn.gameObject.GetComponent<UISprite>();
			if (null != component2)
			{
				component2.spriteName = this.resistanceTabOn;
				this.AttributeLabel.color = this.TAB_COLOR_ON;
			}
			this.SetMonsterIconReset();
			this.InitMonsterList(true, false);
			this._goUI_TEXT.SetActive(true);
		}
	}

	private struct MaterialInfo
	{
		public string path;

		public int haveNum;

		public int needNum;
	}

	public enum ArousalSelectState
	{
		RESISTANCE = 2,
		ATTRIBUTE = 1
	}
}
