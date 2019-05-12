using Master;
using ResistanceTrance;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class CMD_ArousalTOP : CMD
{
	private List<MonsterData> _selectedMonsterDataList;

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

	[SerializeField]
	private UILabel _ngTX_MN_HAVE;

	[Header("チップ装備の処理は分離")]
	[SerializeField]
	private ChipBaseSelect chipBaseSelect;

	[SerializeField]
	[Header("パートナーデジモンのラベル")]
	private UILabel partnerTitleLabel;

	[SerializeField]
	[Header("決定ボタンのラベル")]
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

	private GUIMonsterIcon _leftLargeMonsterIcon;

	private GameObject _goSelectPanelMonsterIcon;

	private GUISelectPanelMonsterIcon _csSelectPanelMonsterIcon;

	private List<MonsterData> _deckMDList;

	private MonsterData _oldMonsterData;

	private readonly Color COLOR_NON = new Color(0.274509817f, 0.274509817f, 0.274509817f, 1f);

	private readonly Color COLOR_ACT = new Color(1f, 1f, 1f, 1f);

	private bool _itemSufficient;

	private CMD_CharacterDetailed detailWindow;

	private string resistanceTabOff = "Common02_Btn_tab_2";

	private string resistanceTabOn = "Common02_Btn_tab_1";

	private readonly Color TAB_COLOR_OFF = new Color(0.5137255f, 0.5137255f, 0.5137255f, 1f);

	private readonly Color TAB_COLOR_ON = new Color(1f, 1f, 1f, 1f);

	private bool userVarUpPossession;

	private CMD_ArousalTOP.ArousalSelectState arousalSelectState = CMD_ArousalTOP.ArousalSelectState.ATTRIBUTE;

	private MonsterData _baseDigimon;

	private List<MonsterData> _partnerDigimons = new List<MonsterData>();

	private int mUserMonsterId;

	protected override void Awake()
	{
		base.Awake();
		this.chipBaseSelect.ClearChipIcons();
		for (int i = 0; i < this._goMN_ICON_LIST.Count; i++)
		{
			this._goMN_ICON_LIST[i].SetActive(false);
		}
		PartyUtil.ActMIconShort = new Action<MonsterData>(this.ActMIconShort);
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
			this.Show(f, sizeX, sizeY, aT);
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
		if (tutorialObserver != null)
		{
			GUIMain.BarrierON(null);
			tutorialObserver.StartSecondTutorial("second_tutorial_arousal", new Action(GUIMain.BarrierOFF), delegate
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
		if (this.goEFC_RIGHT != null)
		{
			this._goSelectPanelMonsterIcon.transform.parent = this.goEFC_RIGHT.transform;
		}
		Vector3 localPosition = this._goSelectPanelMonsterIcon.transform.localPosition;
		localPosition.x = 208f;
		GUICollider component = this._goSelectPanelMonsterIcon.GetComponent<GUICollider>();
		component.SetOriginalPos(localPosition);
		this._csSelectPanelMonsterIcon.ListWindowViewRect = ConstValue.GetRectWindow2();
	}

	public void InitMonsterList(bool initLoc = true, bool var2 = false)
	{
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		monsterDataMng.ClearSortMessAll();
		monsterDataMng.ClearLevelMessAll();
		List<MonsterData> list = monsterDataMng.GetMonsterDataList(false);
		this.userVarUpPossession = false;
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].IsVersionUp())
			{
				this.userVarUpPossession = true;
			}
		}
		if (var2)
		{
			list = monsterDataMng.SelectMonsterDataList(list, MonsterDataMng.SELECT_TYPE.ALL_VAR2);
		}
		else
		{
			list = monsterDataMng.SelectMonsterDataList(list, MonsterDataMng.SELECT_TYPE.ALL_OUT_GARDEN);
		}
		list = monsterDataMng.SortMDList(list, false);
		this._csSelectPanelMonsterIcon.initLocation = initLoc;
		Vector3 localScale = this._goMN_ICON_LIST[0].transform.localScale;
		monsterDataMng.SetDimmAll(GUIMonsterIcon.DIMM_LEVEL.ACTIVE);
		monsterDataMng.SetSelectOffAll();
		monsterDataMng.ClearDimmMessAll();
		this._csSelectPanelMonsterIcon.useLocationRecord = true;
		this._csSelectPanelMonsterIcon.AllBuild(list, localScale, new Action<MonsterData>(this.ActMIconLong), new Action<MonsterData>(this.ActMIconShort), false);
		this._deckMDList = MonsterDataMng.Instance().GetDeckMonsterDataList(false);
	}

	private void ActMIconLong(MonsterData monsterData)
	{
		this.ShowDetail(monsterData);
	}

	private CMD_CharacterDetailed ShowDetail(MonsterData tappedMonsterData)
	{
		CMD_CharacterDetailed.DataChg = tappedMonsterData;
		bool flag = false;
		bool isCheckDim = true;
		if (this._selectedMonsterDataList == null || this._selectedMonsterDataList.Count == 0)
		{
			isCheckDim = false;
		}
		else
		{
			foreach (MonsterData monsterData in this._selectedMonsterDataList)
			{
				if (monsterData == tappedMonsterData && monsterData != this._baseDigimon)
				{
					flag = true;
				}
				if (monsterData == tappedMonsterData)
				{
					isCheckDim = false;
				}
			}
		}
		foreach (MonsterData monsterData2 in this._deckMDList)
		{
			if (monsterData2 == tappedMonsterData)
			{
				isCheckDim = false;
			}
		}
		if (this.IsGrowStepMax(tappedMonsterData.monsterMG.growStep))
		{
			isCheckDim = false;
		}
		CMD_CharacterDetailed cmd_CharacterDetailed = GUIMain.ShowCommonDialog(delegate(int i)
		{
			PartyUtil.SetLock(tappedMonsterData, isCheckDim);
			MonsterData monsterData3 = null;
			foreach (MonsterData monsterData4 in this._partnerDigimons)
			{
				if (monsterData4.userMonster.monsterId == tappedMonsterData.userMonster.monsterId)
				{
					monsterData3 = monsterData4;
					break;
				}
			}
			bool flag2 = this._baseDigimon == null && monsterData3 != null;
			bool flag3 = this._baseDigimon != null || monsterData3 != null;
			if (flag3)
			{
				flag3 = !flag2;
			}
			if (flag3 && !this.IsGrowStepMax(tappedMonsterData.monsterMG.growStep))
			{
				string b = (this._baseDigimon != null) ? this._baseDigimon.monsterMG.monsterGroupId : string.Empty;
				if (tappedMonsterData.monsterMG.monsterGroupId != b)
				{
					PartyUtil.SetDimIcon(true, tappedMonsterData, string.Empty, false);
				}
				if (tappedMonsterData.userMonster.IsLocked)
				{
					PartyUtil.SetDimIcon(true, tappedMonsterData, string.Empty, true);
				}
			}
			if (tappedMonsterData == this._baseDigimon)
			{
				this._leftLargeMonsterIcon.Lock = tappedMonsterData.userMonster.IsLocked;
			}
		}, "CMD_CharacterDetailed") as CMD_CharacterDetailed;
		if (flag)
		{
			cmd_CharacterDetailed.Mode = CMD_CharacterDetailed.LockMode.Arousal;
		}
		return cmd_CharacterDetailed;
	}

	private void ActMIconShort(MonsterData md)
	{
		if (this._baseDigimon == null)
		{
			this._baseDigimon = md;
			this.chipBaseSelect.SetSelectedCharChg(md);
			if (this.arousalSelectState == CMD_ArousalTOP.ArousalSelectState.ATTRIBUTE)
			{
				this.SetAttributeResistanceMaterial(md.userMonster.monsterId, md.monsterMG.tribe, md.monsterMG.growStep);
			}
			else if (this.arousalSelectState == CMD_ArousalTOP.ArousalSelectState.RESISTANCE)
			{
				if (this.IsGrowStepMax(md))
				{
					this.ActiveMaxRankUI();
				}
				else
				{
					this.ActiveNomalRankUI();
				}
				if (md.IsVersionUp())
				{
					this.SetStatusAilmentItemIcon(md);
				}
			}
			this._leftLargeMonsterIcon = this.CreateIcon(this._baseDigimon, this._goMN_ICON_CHG);
			this._baseDigimon.csMIcon = this._leftLargeMonsterIcon;
			this._leftLargeMonsterIcon.SetTouchAct_S(new Action<MonsterData>(this.ActMIconS_Remove));
			this.ShowChgInfo();
			this.SetDimParty(true);
			this.CheckDimRightPartners(true);
		}
		else if (this._partnerDigimons.Count < 1)
		{
			this._partnerDigimons.Add(md);
			int index = this._partnerDigimons.Count - 1;
			GUIMonsterIcon guimonsterIcon = this.CreateIcon(this._partnerDigimons[index], this._goMN_ICON_MAT_LIST[index]);
			this._partnerDigimons[index].csMIcon = guimonsterIcon;
			guimonsterIcon.SetTouchAct_S(new Action<MonsterData>(this.ActMIconS_Remove));
		}
		this.RefreshSelectedInMonsterList();
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
				item.path = MonsterDataMng.Instance().GetEvolveItemIconPathByID(materialList[i].soulId);
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
				item.path = MonsterDataMng.Instance().GetEvolveItemIconPathByID(materialList[i].soulId);
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
			if (string.IsNullOrEmpty(text) || int.Parse(text) == 0)
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
		string evolveItemIconPathByID = MonsterDataMng.Instance().GetEvolveItemIconPathByID(soulId);
		UITexture component = go.GetComponent<UITexture>();
		if (component != null)
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
		if (needNum == "0")
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
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		if (monsterDataMng != null)
		{
			monsterDataMng.PushBackAllMonsterPrefab();
		}
		base.WindowClosed();
	}

	private GUIMonsterIcon CreateIcon(MonsterData md, GameObject goEmpty)
	{
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		Transform transform = goEmpty.transform;
		GUIMonsterIcon guimonsterIcon = monsterDataMng.MakePrefabByMonsterData(md, transform.localScale, transform.localPosition, transform.parent, true, false);
		guimonsterIcon.SetTouchAct_L(new Action<MonsterData>(this.ActMIconLong));
		md.dimmLevel = GUIMonsterIcon.DIMM_LEVEL.DISABLE;
		md.selectNum = 0;
		GUIMonsterIcon monsterCS_ByMonsterData = monsterDataMng.GetMonsterCS_ByMonsterData(md);
		monsterCS_ByMonsterData.DimmLevel = md.dimmLevel;
		monsterCS_ByMonsterData.SelectNum = md.selectNum;
		monsterCS_ByMonsterData.SetTouchAct_S(new Action<MonsterData>(this.ActMIconS_Remove));
		guimonsterIcon.Lock = md.userMonster.IsLocked;
		UIWidget component = goEmpty.GetComponent<UIWidget>();
		UIWidget component2 = guimonsterIcon.gameObject.GetComponent<UIWidget>();
		if (component != null && component2 != null)
		{
			int add = component.depth - component2.depth;
			DepthController component3 = guimonsterIcon.gameObject.GetComponent<DepthController>();
			component3.AddWidgetDepth(guimonsterIcon.transform, add);
		}
		goEmpty.SetActive(false);
		return guimonsterIcon;
	}

	private void DeleteIcon(MonsterData md, GameObject goEmpty)
	{
		UnityEngine.Object.DestroyImmediate(md.csMIcon.gameObject);
		goEmpty.SetActive(true);
		md.dimmLevel = GUIMonsterIcon.DIMM_LEVEL.ACTIVE;
		md.selectNum = -1;
		GUIMonsterIcon monsterCS_ByMonsterData = MonsterDataMng.Instance().GetMonsterCS_ByMonsterData(md);
		monsterCS_ByMonsterData.DimmLevel = md.dimmLevel;
		monsterCS_ByMonsterData.SelectNum = md.selectNum;
		monsterCS_ByMonsterData.SetTouchAct_S(new Action<MonsterData>(this.ActMIconShort));
	}

	private void ActMIconS_Remove(MonsterData md)
	{
		if (md == this._baseDigimon)
		{
			this.DeleteIcon(this._baseDigimon, this._goMN_ICON_CHG);
			if (this._goUI_LEFT_ITEM_LIST_MAX_RANK.activeSelf)
			{
				this._goUI_LEFT_ITEM_LIST_MAX_RANK.SetActive(false);
				this.DisableMaterialIconRank(this._goITEM_ICON_LIST_MAX_RANK, this._goITEM_NUM_TEXT_LIST_MAX_RANK);
				this._goUI_LEFT_ITEM_LIST_DEFAULT.SetActive(true);
			}
			this._baseDigimon = null;
			if (this._partnerDigimons.Count <= 0)
			{
				this.CheckDimRightPartners(false);
				this.SetDimParty(false);
				this.SetActiveAllItem(false);
			}
			this.ShowChgInfo();
			this.chipBaseSelect.ClearChipIcons();
		}
		else
		{
			for (int i = 0; i < this._partnerDigimons.Count; i++)
			{
				if (this._partnerDigimons[i] == md)
				{
					this.DeleteIcon(this._partnerDigimons[i], this._goMN_ICON_MAT_LIST[i]);
					this._partnerDigimons.RemoveAt(i);
					this.ShiftPartnerIcon(i);
				}
			}
			if (this._baseDigimon == null)
			{
				this.SetActiveAllItem(false);
				if (this._partnerDigimons.Count <= 0)
				{
					this.CheckDimRightPartners(false);
					this.SetDimParty(false);
				}
			}
		}
		this.RefreshSelectedInMonsterList();
		this.BtnCont();
	}

	private void SetMonsterIconReset()
	{
		if (this._baseDigimon != null)
		{
			this.DeleteIcon(this._baseDigimon, this._goMN_ICON_CHG);
			if (this._goUI_LEFT_ITEM_LIST_MAX_RANK.activeSelf)
			{
				this._goUI_LEFT_ITEM_LIST_MAX_RANK.SetActive(false);
				this.DisableMaterialIconRank(this._goITEM_ICON_LIST_MAX_RANK, this._goITEM_NUM_TEXT_LIST_MAX_RANK);
				this._goUI_LEFT_ITEM_LIST_DEFAULT.SetActive(true);
			}
			this._baseDigimon = null;
			if (this._partnerDigimons.Count <= 0)
			{
				this.CheckDimRightPartners(false);
				this.SetDimParty(false);
				this.SetActiveAllItem(false);
			}
			this.ShowChgInfo();
			this.chipBaseSelect.ClearChipIcons();
		}
		for (int i = 0; i < this._partnerDigimons.Count; i++)
		{
			if (this._partnerDigimons[i] != null)
			{
				this.DeleteIcon(this._partnerDigimons[i], this._goMN_ICON_MAT_LIST[i]);
				this._partnerDigimons.RemoveAt(i);
				this.ShiftPartnerIcon(i);
			}
		}
		if (this._baseDigimon == null)
		{
			this.SetActiveAllItem(false);
			if (this._partnerDigimons.Count <= 0)
			{
				this.CheckDimRightPartners(false);
				this.SetDimParty(false);
			}
		}
		this.RefreshSelectedInMonsterList();
		this.BtnCont();
	}

	private void ShiftPartnerIcon(int idx)
	{
		int i;
		for (i = 0; i < this._partnerDigimons.Count; i++)
		{
			this._partnerDigimons[i].csMIcon.gameObject.transform.localPosition = this._goMN_ICON_MAT_LIST[i].transform.localPosition;
			this._goMN_ICON_MAT_LIST[i].SetActive(false);
		}
		while (i < this._goMN_ICON_MAT_LIST.Count)
		{
			this._goMN_ICON_MAT_LIST[i].SetActive(true);
			i++;
		}
	}

	private void RefreshSelectedInMonsterList()
	{
		this._selectedMonsterDataList = new List<MonsterData>();
		int snum;
		if (this._baseDigimon != null)
		{
			this._selectedMonsterDataList.Add(this._baseDigimon);
			snum = 0;
		}
		else
		{
			snum = 1;
		}
		for (int i = 0; i < this._partnerDigimons.Count; i++)
		{
			this._selectedMonsterDataList.Add(this._partnerDigimons[i]);
		}
		MonsterDataMng.Instance().SetSelectByMonsterDataList(this._selectedMonsterDataList, snum, true);
	}

	private void BtnCont()
	{
		bool flag = false;
		if (this._baseDigimon != null && this._itemSufficient)
		{
			if (this.arousalSelectState == CMD_ArousalTOP.ArousalSelectState.ATTRIBUTE)
			{
				List<GameWebAPI.RespDataMA_GetMonsterTranceM.MonsterTranceM> list;
				flag = (AttributeResistance.GetResistanceTrance(this._baseDigimon.userMonster.monsterId, out list) || 0 < this._partnerDigimons.Count);
			}
			else
			{
				flag = (0 < this._partnerDigimons.Count || this.IsGrowStepMax(this._baseDigimon));
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
		if (this._baseDigimon != null)
		{
			this.monsterBasicInfo.SetMonsterData(this._baseDigimon);
			this.monsterResistanceList.SetValues(this._baseDigimon);
		}
		else
		{
			this.monsterBasicInfo.ClearMonsterData();
			this.monsterResistanceList.ClearValues();
		}
	}

	private void SetDimParty(bool isDim)
	{
		for (int i = 0; i < this._deckMDList.Count; i++)
		{
			MonsterData monsterData = this._deckMDList[i];
			if (isDim)
			{
				if (monsterData != this._baseDigimon)
				{
					PartyUtil.SetDimIcon(true, monsterData, StringMaster.GetString("CharaIcon-04"), false);
				}
			}
			else
			{
				PartyUtil.SetDimIcon(false, monsterData, string.Empty, false);
			}
		}
	}

	private void CheckDimRightPartners(bool isDim)
	{
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		if (isDim)
		{
			List<MonsterData> monsterDataList = monsterDataMng.GetMonsterDataList(false);
			foreach (MonsterData monsterData in monsterDataList)
			{
				if (monsterData == this._baseDigimon)
				{
					PartyUtil.SetLock(monsterData, false);
				}
				else
				{
					string b = (this._baseDigimon != null) ? this._baseDigimon.monsterMG.monsterGroupId : string.Empty;
					List<GameWebAPI.RespDataMA_GetMonsterTranceM.MonsterTranceM> materialList;
					bool resistanceTrance = AttributeResistance.GetResistanceTrance(monsterData.monsterM.monsterId, out materialList);
					if (monsterData.monsterMG.monsterGroupId != b || this.IsGrowStepMax(monsterData) || (resistanceTrance && AttributeResistance.IsSpecialTypeMonster(materialList)))
					{
						PartyUtil.SetDimIcon(true, monsterData, string.Empty, false);
					}
					if (monsterData.userMonster.IsLocked)
					{
						PartyUtil.SetDimIcon(true, monsterData, string.Empty, true);
					}
				}
			}
		}
		else
		{
			List<MonsterData> monsterDataList2 = monsterDataMng.GetMonsterDataList(false);
			foreach (MonsterData monsterData2 in monsterDataList2)
			{
				if (monsterData2 != this._baseDigimon && !this._partnerDigimons.Contains(monsterData2))
				{
					PartyUtil.SetDimIcon(false, monsterData2, string.Empty, false);
				}
			}
		}
	}

	private void OnTouchDecide()
	{
		CMD_ArousalCheck cmd_ArousalCheck = GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseArousal), "CMD_ArousalCheck") as CMD_ArousalCheck;
		bool isGrowStepMax;
		if (this.arousalSelectState == CMD_ArousalTOP.ArousalSelectState.ATTRIBUTE)
		{
			List<GameWebAPI.RespDataMA_GetMonsterTranceM.MonsterTranceM> list;
			isGrowStepMax = AttributeResistance.GetResistanceTrance(this._baseDigimon.userMonster.monsterId, out list);
		}
		else
		{
			isGrowStepMax = this.IsGrowStepMax(this._baseDigimon);
		}
		cmd_ArousalCheck.SetParams(this._baseDigimon, this._partnerDigimons, isGrowStepMax);
	}

	private void OnCloseArousal(int idx)
	{
		if (idx == 0)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			int baseUserMonsterId = int.Parse(this._baseDigimon.userMonster.userMonsterId);
			if (this.arousalSelectState == CMD_ArousalTOP.ArousalSelectState.ATTRIBUTE)
			{
				List<GameWebAPI.RespDataMA_GetMonsterTranceM.MonsterTranceM> list;
				if (AttributeResistance.GetResistanceTrance(this._baseDigimon.userMonster.monsterId, out list))
				{
					this.mUserMonsterId = baseUserMonsterId;
				}
				else
				{
					this.mUserMonsterId = int.Parse(this._partnerDigimons.FirstOrDefault<MonsterData>().userMonster.userMonsterId);
				}
			}
			else if (this.IsGrowStepMax(this._baseDigimon))
			{
				this.mUserMonsterId = baseUserMonsterId;
			}
			else
			{
				this.mUserMonsterId = int.Parse(this._partnerDigimons.FirstOrDefault<MonsterData>().userMonster.userMonsterId);
			}
			GameWebAPI.RequestMN_MonsterTrance requestMN_MonsterTrance = new GameWebAPI.RequestMN_MonsterTrance();
			requestMN_MonsterTrance.SetSendData = delegate(GameWebAPI.MN_Req_Trunce param)
			{
				param.baseUserMonsterId = baseUserMonsterId;
				param.materialUserMonsterId = this.mUserMonsterId;
				param.type = (int)this.arousalSelectState;
			};
			requestMN_MonsterTrance.OnReceived = delegate(GameWebAPI.RespDataMN_TrunceExec response)
			{
				DataMng.Instance().SetUserMonster(response.userMonster);
			};
			GameWebAPI.RequestMN_MonsterTrance request = requestMN_MonsterTrance;
			base.StartCoroutine(request.Run(new Action(this.EndTrunce), delegate(Exception noop)
			{
				RestrictionInput.EndLoad();
			}, null));
		}
	}

	private bool IsGrowStepMax(MonsterData mData)
	{
		if (mData == null)
		{
			return false;
		}
		GrowStep growStep = (GrowStep)mData.monsterMG.growStep.ToInt32();
		return growStep == GrowStep.ULTIMATE || GrowStep.ARMOR_2 == growStep;
	}

	private bool IsGrowStepMax(string growStep)
	{
		GrowStep growStep2 = (GrowStep)int.Parse(growStep);
		return growStep2 == GrowStep.ULTIMATE || GrowStep.ARMOR_2 == growStep2;
	}

	private void EndTrunce()
	{
		if (this.arousalSelectState == CMD_ArousalTOP.ArousalSelectState.ATTRIBUTE)
		{
			List<GameWebAPI.RespDataMA_GetMonsterTranceM.MonsterTranceM> list;
			if (!AttributeResistance.GetResistanceTrance(this._baseDigimon.userMonster.monsterId, out list))
			{
				DataMng.Instance().DeleteUserMonsterList(new int[]
				{
					this.mUserMonsterId
				});
			}
		}
		else if (!this.IsGrowStepMax(this._baseDigimon))
		{
			DataMng.Instance().DeleteUserMonsterList(new int[]
			{
				this.mUserMonsterId
			});
		}
		base.StartCoroutine(this.GetSoulDataAPI());
	}

	private IEnumerator GetSoulDataAPI()
	{
		APIRequestTask task = Singleton<UserDataMng>.Instance.RequestUserSoulData(true);
		yield return base.StartCoroutine(task.Run(null, null, null));
		this.StartCutScene();
		yield break;
	}

	private void StartCutScene()
	{
		List<int> list = new List<int>();
		list.Add(int.Parse(this._baseDigimon.monsterM.monsterGroupId));
		this._oldMonsterData = new MonsterData();
		this._oldMonsterData.userMonster = this._baseDigimon.GetDuplicateUserMonster(this._baseDigimon.userMonster);
		this._oldMonsterData.monsterM = this._baseDigimon.monsterM;
		this._oldMonsterData.InitResistanceInfo();
		Loading.Invisible();
		CutSceneMain.FadeReqCutScene("Cutscenes/Awakening", new Action<int>(this.StartCutSceneCallBack), delegate(int index)
		{
			CutSceneMain.FadeReqCutSceneEnd();
		}, delegate(int index)
		{
			this.detailWindow.ShowByArousal(this._oldMonsterData, this._baseDigimon);
			this._baseDigimon = null;
			this.BtnCont();
			RestrictionInput.EndLoad();
		}, list, null, 2, 1, 0.5f, 0.5f);
	}

	private void StartCutSceneCallBack(int i)
	{
		this.DeleteIcon(this._baseDigimon, this._goMN_ICON_CHG);
		for (int j = 0; j < this._partnerDigimons.Count; j++)
		{
			if (this._baseDigimon == null)
			{
				this.SetActiveAllItem(false);
			}
			this.DeleteIcon(this._partnerDigimons[j], this._goMN_ICON_MAT_LIST[j]);
			this._partnerDigimons.RemoveAt(j);
			this.ShiftPartnerIcon(j);
		}
		this._partnerDigimons.Clear();
		MonsterDataMng.Instance().RefreshMonsterDataList();
		if (this.arousalSelectState == CMD_ArousalTOP.ArousalSelectState.ATTRIBUTE)
		{
			this.InitMonsterList(false, false);
		}
		else if (this.arousalSelectState == CMD_ArousalTOP.ArousalSelectState.RESISTANCE)
		{
			this.InitMonsterList(false, true);
		}
		this.detailWindow = this.ShowDetail(this._baseDigimon);
		this.ShowChgInfo();
		this.SetDimParty(false);
		this.CheckDimRightPartners(false);
		this.monsterResistanceList.ClearValues();
		this.SetInactiveAllItemIcon(this._goITEM_ICON_LIST, this._goITEM_NUM_TEXT_LIST);
		this.SetInactiveAllItemIcon(this._goITEM_ICON_LIST_MAX_RANK, this._goITEM_NUM_TEXT_LIST_MAX_RANK);
		this.SetInactiveItemIcon(this._goITEM_ICON_MAX_RANK_SP, this._goITEM_LABEL_MAX_RANK_SP);
		this.ActiveNomalRankUI();
	}

	public void OnTouchedEvoltionItemListBtn()
	{
		GUIMain.ShowCommonDialog(null, "CMD_EvolutionItemList");
	}

	public void OnTouchedResistanceBtn()
	{
		if (!this.userVarUpPossession)
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("ArousalNonVersionUpTitle");
			cmd_ModalMessage.Info = StringMaster.GetString("ArousalNonVersionUp");
			return;
		}
		if (this.arousalSelectState != CMD_ArousalTOP.ArousalSelectState.RESISTANCE)
		{
			this.arousalSelectState = CMD_ArousalTOP.ArousalSelectState.RESISTANCE;
			UISprite component = this.ResistanceBtn.gameObject.GetComponent<UISprite>();
			if (component != null)
			{
				component.spriteName = this.resistanceTabOn;
				this.ResistanceLabel.color = this.TAB_COLOR_ON;
			}
			UISprite component2 = this.AttributeBtn.gameObject.GetComponent<UISprite>();
			if (component2 != null)
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
			if (component != null)
			{
				component.spriteName = this.resistanceTabOff;
				this.ResistanceLabel.color = this.TAB_COLOR_OFF;
			}
			UISprite component2 = this.AttributeBtn.gameObject.GetComponent<UISprite>();
			if (component2 != null)
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
