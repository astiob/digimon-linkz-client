using Master;
using Monster;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_DeckList : CMD
{
	[SerializeField]
	[Header("ベース用チップ装備")]
	private ChipBaseSelect baseChipBaseSelect;

	[SerializeField]
	[Header("パートナー用チップ装備")]
	private ChipBaseSelect partnerChipBaseSelect;

	[SerializeField]
	private List<GameObject> goMN_ICON_LIST;

	[SerializeField]
	private GameObject goMN_ICON_NOW;

	[SerializeField]
	private GameObject goMN_ICON_CHG;

	[SerializeField]
	private MonsterBasicInfo nowMonsterBasicInfo;

	[SerializeField]
	private MonsterResistanceList nowMonsterResistanceList;

	[SerializeField]
	private MonsterGimmickEffectStatusList nowMonsterStatusList;

	[SerializeField]
	private MonsterMedalList nowMonsterMedalList;

	[SerializeField]
	private MonsterLeaderSkill nowMonsterLeaderSkill;

	[SerializeField]
	private MonsterLearnSkill nowMonsterUniqueSkill;

	[SerializeField]
	private MonsterLearnSkill nowMonsterSuccessionSkill;

	[SerializeField]
	private MonsterLearnSkill nowMonsterSuccessionSkill2;

	[SerializeField]
	private GameObject nowMonsterSuccessionSkillAvailable;

	[SerializeField]
	private GameObject nowMonsterSuccessionSkillGrayReady;

	[SerializeField]
	private GameObject nowMonsterSuccessionSkillGrayNA;

	[SerializeField]
	private MonsterBasicInfo changeMonsterBasicInfo;

	[SerializeField]
	private MonsterResistanceList changeMonsterResistanceList;

	[SerializeField]
	private MonsterGimmickEffectStatusList changeMonsterStatusList;

	[SerializeField]
	private MonsterMedalList changeMonsterMedalList;

	[SerializeField]
	private MonsterLeaderSkill changeMonsterLeaderSkill;

	[SerializeField]
	private MonsterLearnSkill changeMonsterUniqueSkill;

	[SerializeField]
	private MonsterLearnSkill changeMonsterSuccessionSkill;

	[SerializeField]
	private MonsterLearnSkill changeMonsterSuccessionSkill2;

	[SerializeField]
	private GameObject changeMonsterSuccessionSkillAvailable;

	[SerializeField]
	private GameObject changeMonsterSuccessionSkillGrayReady;

	[SerializeField]
	private GameObject changeMonsterSuccessionSkillGrayNA;

	[SerializeField]
	private MonsterStatusChangeValueList monsterStatusChangeValueList;

	[SerializeField]
	private GameObject goSimpleSkillPanel;

	[SerializeField]
	private GameObject goDetailedSkillPanel;

	[SerializeField]
	private MonsterLearnSkill detailedNowMonsterUniqueSkill;

	[SerializeField]
	private MonsterLearnSkill detailedNowMonsterSuccessionSkill;

	[SerializeField]
	private MonsterLearnSkill detailedNowMonsterSuccessionSkill2;

	[SerializeField]
	private GameObject detailedNowMonsterSuccessionSkillAvailable;

	[SerializeField]
	private GameObject detailedNowMonsterSuccessionSkillGrayReady;

	[SerializeField]
	private GameObject detailedNowMonsterSuccessionSkillGrayNA;

	[SerializeField]
	private MonsterLearnSkill detailedChangeMonsterUniqueSkill;

	[SerializeField]
	private MonsterLearnSkill detailedChangeMonsterSuccessionSkill;

	[SerializeField]
	private MonsterLearnSkill detailedChangeMonsterSuccessionSkill2;

	[SerializeField]
	private GameObject detailedChangeMonsterSuccessionSkillAvailable;

	[SerializeField]
	private GameObject detailedChangeMonsterSuccessionSkillGrayReady;

	[SerializeField]
	private GameObject detailedChangeMonsterSuccessionSkillGrayNA;

	[SerializeField]
	private GameObject switchSkillPanelBtn;

	[SerializeField]
	private UISprite selectButton;

	private int statusPage = 1;

	[SerializeField]
	private List<GameObject> goStatusPanelPage;

	private BtnSort sortButton;

	private GameObject goSelectPanelMonsterIcon;

	private GUISelectPanelMonsterIcon csSelectPanelMonsterIcon;

	private GameObject goMN_ICON_NOW_2;

	private GameObject goMN_ICON_CHG_2;

	private GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM[] effectArray;

	private static CMD_DeckList instance;

	[Header("キャラクターのステータスPanel")]
	[SerializeField]
	private StatusPanel statusPanel;

	private List<GameWebAPI.RespDataMA_WorldDungeonSortieLimit.WorldDungeonSortieLimit> sortieLimitList;

	private List<MonsterData> targetMonsterList;

	public static MonsterData SelectMonsterData { get; set; }

	private MonsterData DataChg { get; set; }

	public PartsPartyMonsInfo PPMI_Instance { get; set; }

	public List<PartsPartyMonsInfo> ppmiList { get; set; }

	public static CMD_DeckList Instance
	{
		get
		{
			return CMD_DeckList.instance;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		CMD_DeckList.instance = this;
		this.nowMonsterMedalList.SetActive(false);
		this.changeMonsterMedalList.SetActive(false);
		for (int i = 0; i < this.goMN_ICON_LIST.Count; i++)
		{
			this.goMN_ICON_LIST[i].SetActive(false);
		}
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.PartsTitle.SetTitle(StringMaster.GetString("PartyTitleSelect"));
		this.statusPanel.SetEnable(true);
		this.SetGimmickInfo();
		this.SetSelectedChar();
		this.SetCommonUI();
		this.InitMonsterList();
		this.ShowNowInfo();
		this.ShowChgInfo();
		this.ShowEtcInfo();
		this.SelectButtonActive(false);
		this.StatusPageChange(false);
		base.SetOpendAction(new Action<int>(this.OpendAction));
		base.Show(f, sizeX, sizeY, aT);
		RestrictionInput.EndLoad();
	}

	private void OpendAction(int i)
	{
		if (null != CMD_PartyEdit.instance)
		{
			CMD_PartyEdit.instance.ReloadAllCharacters(false);
		}
		if (null != CMD_MultiRecruitPartyWait.Instance)
		{
			CMD_MultiRecruitPartyWait.Instance.ReloadAllCharacters(false);
		}
	}

	protected override void OnDestroy()
	{
		if (null != CMD_PartyEdit.instance && null == CMD_MultiRecruitPartyWait.Instance && !GUIManager.IsCloseAllMode())
		{
			CMD_PartyEdit.instance.ReloadAllCharacters(true);
		}
		if (null != CMD_MultiRecruitPartyWait.Instance)
		{
			CMD_MultiRecruitPartyWait.Instance.ReloadAllCharacters(true);
		}
		base.OnDestroy();
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
	}

	public override void ClosePanel(bool animation = true)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	protected override void WindowClosed()
	{
		CMD_DeckList.SelectMonsterData = null;
		ClassSingleton<GUIMonsterIconList>.Instance.PushBackAllMonsterPrefab();
		base.WindowClosed();
	}

	private void SetGimmickInfo()
	{
		if (CMD_MultiRecruitPartyWait.StageDataBk != null)
		{
			this.effectArray = ExtraEffectUtil.GetExtraEffectArray(CMD_MultiRecruitPartyWait.StageDataBk.worldDungeonId);
		}
		else if (null != CMD_QuestTOP.instance && CMD_QuestTOP.instance.StageDataBk != null)
		{
			this.effectArray = ExtraEffectUtil.GetExtraEffectArray(CMD_QuestTOP.instance.StageDataBk.worldDungeonM.worldDungeonId);
		}
	}

	private void ShowNowInfo()
	{
		if (CMD_DeckList.SelectMonsterData != null)
		{
			this.baseChipBaseSelect.SetSelectedCharChg(CMD_DeckList.SelectMonsterData);
			this.nowMonsterBasicInfo.SetMonsterData(CMD_DeckList.SelectMonsterData);
			this.nowMonsterStatusList.SetValues(CMD_DeckList.SelectMonsterData, this.effectArray);
			this.nowMonsterMedalList.SetValues(CMD_DeckList.SelectMonsterData.userMonster);
			this.nowMonsterLeaderSkill.SetSkill(CMD_DeckList.SelectMonsterData);
			this.nowMonsterUniqueSkill.SetSkill(CMD_DeckList.SelectMonsterData);
			this.detailedNowMonsterUniqueSkill.SetSkill(CMD_DeckList.SelectMonsterData);
			this.nowMonsterSuccessionSkill.SetSkill(CMD_DeckList.SelectMonsterData);
			this.detailedNowMonsterSuccessionSkill.SetSkill(CMD_DeckList.SelectMonsterData);
			this.nowMonsterSuccessionSkillGrayReady.SetActive(false);
			this.nowMonsterSuccessionSkillAvailable.SetActive(false);
			this.nowMonsterSuccessionSkillGrayNA.SetActive(false);
			this.detailedNowMonsterSuccessionSkillGrayReady.SetActive(false);
			this.detailedNowMonsterSuccessionSkillAvailable.SetActive(false);
			this.detailedNowMonsterSuccessionSkillGrayNA.SetActive(false);
			this.nowMonsterSuccessionSkill2.SetSkill(CMD_DeckList.SelectMonsterData);
			this.detailedNowMonsterSuccessionSkill2.SetSkill(CMD_DeckList.SelectMonsterData);
			if (MonsterStatusData.IsVersionUp(CMD_DeckList.SelectMonsterData.GetMonsterMaster().Simple.rare))
			{
				if (CMD_DeckList.SelectMonsterData.GetExtraCommonSkill() == null)
				{
					this.nowMonsterSuccessionSkillGrayReady.SetActive(true);
					this.detailedNowMonsterSuccessionSkillGrayReady.SetActive(true);
				}
				else
				{
					this.nowMonsterSuccessionSkillAvailable.SetActive(true);
					this.detailedNowMonsterSuccessionSkillAvailable.SetActive(true);
				}
			}
			else
			{
				this.nowMonsterSuccessionSkillGrayNA.SetActive(true);
				this.detailedNowMonsterSuccessionSkillGrayNA.SetActive(true);
			}
			this.nowMonsterResistanceList.SetValues(CMD_DeckList.SelectMonsterData);
		}
		else
		{
			this.baseChipBaseSelect.ClearChipIcons();
			this.nowMonsterBasicInfo.ClearMonsterData();
			this.nowMonsterStatusList.ClearValues();
		}
	}

	private void ShowChgInfo()
	{
		if (this.DataChg != null)
		{
			this.partnerChipBaseSelect.SetSelectedCharChg(this.DataChg);
			this.changeMonsterBasicInfo.SetMonsterData(this.DataChg);
			this.changeMonsterStatusList.SetValues(this.DataChg, this.effectArray);
			this.changeMonsterMedalList.SetValues(this.DataChg.userMonster);
			this.changeMonsterLeaderSkill.SetSkill(this.DataChg);
			this.changeMonsterUniqueSkill.SetSkill(this.DataChg);
			this.detailedChangeMonsterUniqueSkill.SetSkill(this.DataChg);
			this.changeMonsterSuccessionSkill.SetSkill(this.DataChg);
			this.detailedChangeMonsterSuccessionSkill.SetSkill(this.DataChg);
			this.changeMonsterSuccessionSkillGrayReady.SetActive(false);
			this.changeMonsterSuccessionSkillAvailable.SetActive(false);
			this.changeMonsterSuccessionSkillGrayNA.SetActive(false);
			this.detailedChangeMonsterSuccessionSkillGrayReady.SetActive(false);
			this.detailedChangeMonsterSuccessionSkillAvailable.SetActive(false);
			this.detailedChangeMonsterSuccessionSkillGrayNA.SetActive(false);
			this.changeMonsterSuccessionSkill2.SetSkill(this.DataChg);
			this.detailedChangeMonsterSuccessionSkill2.SetSkill(this.DataChg);
			if (MonsterStatusData.IsVersionUp(this.DataChg.GetMonsterMaster().Simple.rare))
			{
				if (this.DataChg.GetExtraCommonSkill() == null)
				{
					this.changeMonsterSuccessionSkillGrayReady.SetActive(true);
					this.detailedChangeMonsterSuccessionSkillGrayReady.SetActive(true);
				}
				else
				{
					this.changeMonsterSuccessionSkillAvailable.SetActive(true);
					this.detailedChangeMonsterSuccessionSkillAvailable.SetActive(true);
				}
			}
			else
			{
				this.changeMonsterSuccessionSkillGrayNA.SetActive(true);
				this.detailedChangeMonsterSuccessionSkillGrayNA.SetActive(true);
			}
			this.changeMonsterResistanceList.SetValues(this.DataChg);
		}
		else
		{
			this.partnerChipBaseSelect.ClearChipIcons();
			this.changeMonsterBasicInfo.ClearMonsterData();
			this.changeMonsterStatusList.ClearValues();
			this.changeMonsterLeaderSkill.ClearSkill();
			this.changeMonsterUniqueSkill.ClearSkill();
			this.detailedChangeMonsterUniqueSkill.ClearSkill();
			this.changeMonsterSuccessionSkill.ClearSkill();
			this.changeMonsterSuccessionSkill2.ClearSkill();
			this.detailedChangeMonsterSuccessionSkill.ClearSkill();
			this.detailedChangeMonsterSuccessionSkill2.ClearSkill();
			this.changeMonsterResistanceList.ClearValues();
		}
		StatusValue values = this.nowMonsterStatusList.GetValues();
		StatusValue values2 = this.changeMonsterStatusList.GetValues();
		this.monsterStatusChangeValueList.SetValues(values, values2);
	}

	private void ShowEtcInfo()
	{
	}

	private void SetSelectedChar()
	{
		if (CMD_DeckList.SelectMonsterData != null)
		{
			Transform transform = this.goMN_ICON_NOW.transform;
			GUIMonsterIcon guimonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(CMD_DeckList.SelectMonsterData, transform.localScale, transform.localPosition, transform.parent, true, false);
			this.goMN_ICON_NOW_2 = guimonsterIcon.gameObject;
			this.goMN_ICON_NOW_2.SetActive(true);
			guimonsterIcon.Data = CMD_DeckList.SelectMonsterData;
			guimonsterIcon.SetTouchAct_L(new Action<MonsterData>(this.ActMIconLong));
			UIWidget component = this.goMN_ICON_NOW.GetComponent<UIWidget>();
			UIWidget component2 = guimonsterIcon.gameObject.GetComponent<UIWidget>();
			if (null != component && null != component2)
			{
				int add = component.depth - component2.depth;
				DepthController component3 = guimonsterIcon.gameObject.GetComponent<DepthController>();
				component3.AddWidgetDepth(guimonsterIcon.transform, add);
			}
			this.goMN_ICON_NOW.SetActive(false);
			guimonsterIcon.Gimmick = ExtraEffectUtil.IsExtraEffectMonster(CMD_DeckList.SelectMonsterData, this.effectArray);
		}
	}

	private void SetCommonUI()
	{
		this.goSelectPanelMonsterIcon = GUIManager.LoadCommonGUI("SelectListPanel/SelectListPanelMonsterIcon", base.gameObject);
		this.csSelectPanelMonsterIcon = this.goSelectPanelMonsterIcon.GetComponent<GUISelectPanelMonsterIcon>();
		if (null != this.goEFC_RIGHT)
		{
			this.goSelectPanelMonsterIcon.transform.parent = this.goEFC_RIGHT.transform;
		}
		Vector3 localPosition = this.goSelectPanelMonsterIcon.transform.localPosition;
		localPosition.x = 208f;
		GUICollider component = this.goSelectPanelMonsterIcon.GetComponent<GUICollider>();
		component.SetOriginalPos(localPosition);
		Rect listWindowViewRect = default(Rect);
		listWindowViewRect.xMin = -240f;
		listWindowViewRect.xMax = 240f;
		listWindowViewRect.yMin = -297f - GUIMain.VerticalSpaceSize;
		listWindowViewRect.yMax = 158f + GUIMain.VerticalSpaceSize;
		this.csSelectPanelMonsterIcon.ListWindowViewRect = listWindowViewRect;
	}

	private void InitMonsterList()
	{
		ClassSingleton<GUIMonsterIconList>.Instance.ResetIconState();
		List<MonsterData> list = MonsterDataMng.Instance().GetMonsterDataList();
		list = MonsterFilter.Filter(list, MonsterFilterType.ALL_OUT_GARDEN);
		MonsterDataMng.Instance().SortMDList(list);
		MonsterDataMng.Instance().SetSortLSMessage();
		this.csSelectPanelMonsterIcon.initLocation = true;
		Vector3 localScale = this.goMN_ICON_LIST[0].transform.localScale;
		if (null != CMD_MultiRecruitPartyWait.Instance)
		{
			this.SetDimmByMonsterDataList(list, CMD_DeckList.SelectMonsterData);
		}
		else
		{
			this.SetDimmByMonsterDataList(CMD_PartyEdit.instance.GetSelectedMD(), CMD_DeckList.SelectMonsterData);
		}
		this.csSelectPanelMonsterIcon.useLocationRecord = true;
		this.csSelectPanelMonsterIcon.SetCheckEnablePushAction(new Func<MonsterData, bool>(this.CheckEnablePush));
		this.targetMonsterList = list;
		list = MonsterDataMng.Instance().SelectionMDList(list);
		this.csSelectPanelMonsterIcon.AllBuild(list, localScale, new Action<MonsterData>(this.ActMIconLong), new Action<MonsterData>(this.ActMIconShort), false);
		if (this.sortieLimitList != null)
		{
			this.csSelectPanelMonsterIcon.SetIconSortieLimitParts(this.sortieLimitList);
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

	private void ActMIconShort(MonsterData tappedMonsterData)
	{
		if (this.DataChg != null)
		{
			GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(this.DataChg);
			icon.DimmMess = string.Empty;
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.ACTIVE);
			icon.SetTouchAct_S(new Action<MonsterData>(this.ActMIconShort));
		}
		this.DataChg = tappedMonsterData;
		if (this.DataChg != null)
		{
			GUIMonsterIcon icon2 = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(this.DataChg);
			icon2.DimmMess = StringMaster.GetString("SystemSelect");
			icon2.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.DISABLE);
			icon2.SetTouchAct_S(null);
			this.SetSelectedCharChg();
			this.SelectButtonActive(true);
		}
	}

	private void SetSelectedCharChg()
	{
		if (this.DataChg != null)
		{
			if (null != this.goMN_ICON_CHG_2)
			{
				UnityEngine.Object.DestroyImmediate(this.goMN_ICON_CHG_2);
			}
			Transform transform = this.goMN_ICON_CHG.transform;
			GUIMonsterIcon guimonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(this.DataChg, transform.localScale, transform.localPosition, transform.parent, true, false);
			this.goMN_ICON_CHG_2 = guimonsterIcon.gameObject;
			this.goMN_ICON_CHG_2.SetActive(true);
			guimonsterIcon.Data = this.DataChg;
			guimonsterIcon.SetTouchAct_S(new Action<MonsterData>(this.actRemoveChg));
			guimonsterIcon.SetTouchAct_L(new Action<MonsterData>(this.ActMIconLong));
			UIWidget component = this.goMN_ICON_CHG.GetComponent<UIWidget>();
			UIWidget component2 = guimonsterIcon.gameObject.GetComponent<UIWidget>();
			if (null != component && null != component2)
			{
				int add = component.depth - component2.depth;
				DepthController component3 = guimonsterIcon.gameObject.GetComponent<DepthController>();
				component3.AddWidgetDepth(guimonsterIcon.transform, add);
			}
			this.goMN_ICON_CHG.SetActive(false);
			guimonsterIcon.Gimmick = ExtraEffectUtil.IsExtraEffectMonster(this.DataChg, this.effectArray);
		}
		this.ShowChgInfo();
	}

	private void actRemoveChg(MonsterData md)
	{
		if (this.DataChg != null)
		{
			GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(this.DataChg);
			icon.DimmMess = string.Empty;
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.ACTIVE);
			icon.SetTouchAct_S(new Action<MonsterData>(this.ActMIconShort));
		}
		this.DataChg = null;
		if (null != this.goMN_ICON_CHG_2)
		{
			UnityEngine.Object.DestroyImmediate(this.goMN_ICON_CHG_2);
		}
		this.goMN_ICON_CHG.SetActive(true);
		this.ShowChgInfo();
		this.SelectButtonActive(false);
	}

	private void ActMIconLong(MonsterData md)
	{
		CMD_CharacterDetailed.DataChg = md;
		CMD_CharacterDetailed dl = GUIMain.ShowCommonDialog(delegate(int i)
		{
			GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(md);
			icon.Lock = md.userMonster.IsLocked;
		}, "CMD_CharacterDetailed", null) as CMD_CharacterDetailed;
		dl.DisableEvolutionButton();
		if (null != CMD_MultiRecruitPartyWait.Instance)
		{
			dl.PartsTitle.DisableCloseBtn(true);
		}
		CMD_BattleNextChoice battleNextChoice = UnityEngine.Object.FindObjectOfType<CMD_BattleNextChoice>();
		if (null != battleNextChoice)
		{
			dl.PartsTitle.SetCloseAct(delegate(int i)
			{
				battleNextChoice.ClosePanel(false);
				CMD_PartyEdit.instance.ClosePanel(false);
				CMD_DeckList.instance.ClosePanel(false);
				dl.SetCloseAction(delegate(int x)
				{
					CMD_BattleNextChoice.GoToFarm();
				});
				dl.ClosePanel(true);
			});
		}
	}

	private void OnTouchSort()
	{
	}

	private void OnTouchDecide()
	{
		for (int i = 0; i < this.ppmiList.Count; i++)
		{
			if (this.ppmiList[i].Data == this.DataChg)
			{
				MonsterData data = this.PPMI_Instance.Data;
				this.PPMI_Instance.Data = this.ppmiList[i].Data;
				this.ppmiList[i].Data = data;
				this.ClosePanel(true);
				return;
			}
		}
		this.PPMI_Instance.Data = this.DataChg;
		if (null != CMD_PartyEdit.instance)
		{
			List<string> pathL = CMD_PartyEdit.instance.MakeAllCharaPath();
			AssetDataCacheMng.Instance().RegisterCacheType(pathL, AssetDataCacheMng.CACHE_TYPE.CHARA_PARTY, false);
		}
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
		base.StartCoroutine(this.WaitSHow());
	}

	private IEnumerator WaitSHow()
	{
		while (!AssetDataCacheMng.Instance().IsCacheAllReadyType(AssetDataCacheMng.CACHE_TYPE.CHARA_PARTY))
		{
			yield return null;
		}
		RestrictionInput.EndLoad();
		this.ClosePanel(true);
		yield break;
	}

	private void SelectButtonActive(bool selected)
	{
		if (selected)
		{
			this.selectButton.spriteName = "Common02_Btn_Blue";
			this.selectButton.gameObject.GetComponent<GUICollider>().activeCollider = true;
		}
		else
		{
			this.selectButton.spriteName = "Common02_Btn_Gray";
			this.selectButton.gameObject.GetComponent<GUICollider>().activeCollider = false;
		}
	}

	public void StatusPageChangeTap()
	{
		this.switchDetailSkillPanel(false);
		this.StatusPageChange(true);
	}

	public void StatusPageChange(bool pageChange)
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
				this.switchSkillPanelBtn.SetActive(gameObject.name == "SkillChange");
			}
			else
			{
				gameObject.SetActive(false);
			}
			num++;
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (null != CMD_MultiRecruitPartyWait.Instance)
		{
			CMD_MultiRecruitPartyWait.Instance.OnApplicationPause(pauseStatus);
		}
	}

	public void SetSortieLimit(List<GameWebAPI.RespDataMA_WorldDungeonSortieLimit.WorldDungeonSortieLimit> limitList)
	{
		this.sortieLimitList = limitList;
		this.csSelectPanelMonsterIcon.SetIconSortieLimitParts(this.sortieLimitList);
	}

	public void switchDetailSkillPanel(bool isOpen)
	{
		this.goDetailedSkillPanel.SetActive(isOpen);
		this.goSimpleSkillPanel.SetActive(!isOpen);
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

	private void SetDimmByMonsterDataList(List<MonsterData> mdlist, MonsterData selectMonster)
	{
		for (int i = 0; i < mdlist.Count; i++)
		{
			GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(mdlist[i]);
			if (null != icon && selectMonster.userMonster.userMonsterId == mdlist[i].userMonster.userMonsterId)
			{
				icon.DimmMess = StringMaster.GetString("CharaIcon-04");
				icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.DISABLE);
			}
		}
	}

	private bool CheckEnablePush(MonsterData monsterData)
	{
		bool result = false;
		if (CMD_DeckList.SelectMonsterData.userMonster.userMonsterId != monsterData.userMonster.userMonsterId)
		{
			result = true;
		}
		return result;
	}
}
