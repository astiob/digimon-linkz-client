using Master;
using Monster;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterList.ChangeMonster
{
	public sealed class CMD_ChangeMonster : CMD
	{
		[Header("ベース用チップ装備")]
		[SerializeField]
		private ChipBaseSelect baseChipBaseSelect;

		[Header("パートナー用チップ装備")]
		[SerializeField]
		private ChipBaseSelect partnerChipBaseSelect;

		[SerializeField]
		private List<GameObject> goMN_ICON_LIST;

		[SerializeField]
		private GameObject goMN_ICON_NOW;

		[SerializeField]
		private GameObject goMN_ICON_CHG;

		private GameObject goMN_ICON_NOW_2;

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

		private GameObject goMN_ICON_CHG_2;

		private GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM[] effectArray;

		[Header("キャラクターのステータスPanel")]
		[SerializeField]
		private StatusPanel statusPanel;

		private MonsterData changeMonsterData;

		private List<MonsterData> targetMonsterList;

		private Action<MonsterUserData> OnChanged;

		private ChangeMonsterIconGrayOut iconGrayOut;

		private ChangeMonsterMonsterList monsterList;

		public static MonsterData SelectMonsterData { get; set; }

		protected override void Awake()
		{
			base.Awake();
			this.iconGrayOut = new ChangeMonsterIconGrayOut();
			this.iconGrayOut.SetNormalAction(new Action<MonsterData>(this.ActMIconShort), new Action<MonsterData>(this.ActMIconLong));
			this.iconGrayOut.SetSelectedAction(new Action<MonsterData>(this.actRemoveChg), new Action<MonsterData>(this.ActMIconLong));
			this.iconGrayOut.SetBlockAction(null, new Action<MonsterData>(this.ActMIconLong));
			this.monsterList = new ChangeMonsterMonsterList();
			this.monsterList.Initialize(this.iconGrayOut);
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
			this.SetSelectedChar();
			this.SetCommonUI();
			this.InitMonsterList();
			this.ShowNowInfo();
			this.ClearChangeMonsterData();
			this.ShowEtcInfo();
			this.SelectButtonActive();
			this.StatusPageChange(false);
			base.Show(f, sizeX, sizeY, aT);
			RestrictionInput.EndLoad();
		}

		protected override void WindowClosed()
		{
			ClassSingleton<GUIMonsterIconList>.Instance.PushBackAllMonsterPrefab();
			base.WindowClosed();
		}

		private void ShowNowInfo()
		{
			if (CMD_ChangeMonster.SelectMonsterData != null)
			{
				this.baseChipBaseSelect.SetSelectedCharChg(CMD_ChangeMonster.SelectMonsterData);
				this.nowMonsterBasicInfo.SetMonsterData(CMD_ChangeMonster.SelectMonsterData);
				this.nowMonsterStatusList.SetValues(CMD_ChangeMonster.SelectMonsterData, this.effectArray);
				this.nowMonsterMedalList.SetValues(CMD_ChangeMonster.SelectMonsterData.userMonster);
				this.nowMonsterLeaderSkill.SetSkill(CMD_ChangeMonster.SelectMonsterData);
				this.nowMonsterUniqueSkill.SetSkill(CMD_ChangeMonster.SelectMonsterData);
				this.detailedNowMonsterUniqueSkill.SetSkill(CMD_ChangeMonster.SelectMonsterData);
				this.nowMonsterSuccessionSkill.SetSkill(CMD_ChangeMonster.SelectMonsterData);
				this.detailedNowMonsterSuccessionSkill.SetSkill(CMD_ChangeMonster.SelectMonsterData);
				this.nowMonsterSuccessionSkillGrayReady.SetActive(false);
				this.nowMonsterSuccessionSkillAvailable.SetActive(false);
				this.nowMonsterSuccessionSkillGrayNA.SetActive(false);
				this.detailedNowMonsterSuccessionSkillGrayReady.SetActive(false);
				this.detailedNowMonsterSuccessionSkillAvailable.SetActive(false);
				this.detailedNowMonsterSuccessionSkillGrayNA.SetActive(false);
				this.nowMonsterSuccessionSkill2.SetSkill(CMD_ChangeMonster.SelectMonsterData);
				this.detailedNowMonsterSuccessionSkill2.SetSkill(CMD_ChangeMonster.SelectMonsterData);
				if (MonsterStatusData.IsVersionUp(CMD_ChangeMonster.SelectMonsterData.GetMonsterMaster().Simple.rare))
				{
					if (CMD_ChangeMonster.SelectMonsterData.GetExtraCommonSkill() == null)
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
				this.nowMonsterResistanceList.SetValues(CMD_ChangeMonster.SelectMonsterData);
			}
			else
			{
				this.baseChipBaseSelect.ClearChipIcons();
				this.nowMonsterBasicInfo.ClearMonsterData();
				this.nowMonsterStatusList.ClearValues();
				this.nowMonsterMedalList.SetActive(false);
				this.nowMonsterLeaderSkill.ClearSkill();
				this.nowMonsterUniqueSkill.ClearSkill();
				this.detailedNowMonsterUniqueSkill.ClearSkill();
				this.nowMonsterSuccessionSkill.ClearSkill();
				this.nowMonsterSuccessionSkill2.ClearSkill();
				this.detailedNowMonsterSuccessionSkill.ClearSkill();
				this.detailedNowMonsterSuccessionSkill2.ClearSkill();
				this.nowMonsterResistanceList.ClearValues();
			}
		}

		private void SetChangeMonsterData()
		{
			this.partnerChipBaseSelect.SetSelectedCharChg(this.changeMonsterData);
			this.changeMonsterBasicInfo.SetMonsterData(this.changeMonsterData);
			this.changeMonsterStatusList.SetValues(this.changeMonsterData, this.effectArray);
			this.changeMonsterMedalList.SetValues(this.changeMonsterData.userMonster);
			this.changeMonsterLeaderSkill.SetSkill(this.changeMonsterData);
			this.changeMonsterUniqueSkill.SetSkill(this.changeMonsterData);
			this.detailedChangeMonsterUniqueSkill.SetSkill(this.changeMonsterData);
			this.changeMonsterSuccessionSkill.SetSkill(this.changeMonsterData);
			this.detailedChangeMonsterSuccessionSkill.SetSkill(this.changeMonsterData);
			this.changeMonsterSuccessionSkillGrayReady.SetActive(false);
			this.changeMonsterSuccessionSkillAvailable.SetActive(false);
			this.changeMonsterSuccessionSkillGrayNA.SetActive(false);
			this.detailedChangeMonsterSuccessionSkillGrayReady.SetActive(false);
			this.detailedChangeMonsterSuccessionSkillAvailable.SetActive(false);
			this.detailedChangeMonsterSuccessionSkillGrayNA.SetActive(false);
			this.changeMonsterSuccessionSkill2.SetSkill(this.changeMonsterData);
			this.detailedChangeMonsterSuccessionSkill2.SetSkill(this.changeMonsterData);
			if (MonsterStatusData.IsVersionUp(this.changeMonsterData.GetMonsterMaster().Simple.rare))
			{
				if (this.changeMonsterData.GetExtraCommonSkill() == null)
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
			this.changeMonsterResistanceList.SetValues(this.changeMonsterData);
			this.SetChangeStatusValue();
		}

		private void ClearChangeMonsterData()
		{
			this.partnerChipBaseSelect.ClearChipIcons();
			this.changeMonsterBasicInfo.ClearMonsterData();
			this.changeMonsterStatusList.ClearValues();
			this.changeMonsterMedalList.SetActive(false);
			this.changeMonsterLeaderSkill.ClearSkill();
			this.changeMonsterUniqueSkill.ClearSkill();
			this.detailedChangeMonsterUniqueSkill.ClearSkill();
			this.changeMonsterSuccessionSkill.ClearSkill();
			this.changeMonsterSuccessionSkill2.ClearSkill();
			this.detailedChangeMonsterSuccessionSkill.ClearSkill();
			this.detailedChangeMonsterSuccessionSkill2.ClearSkill();
			this.changeMonsterResistanceList.ClearValues();
			this.SetChangeStatusValue();
		}

		private void SetChangeStatusValue()
		{
			StatusValue values = this.nowMonsterStatusList.GetValues();
			StatusValue values2 = this.changeMonsterStatusList.GetValues();
			this.monsterStatusChangeValueList.SetValues(values, values2);
		}

		private void ShowEtcInfo()
		{
		}

		private void SetSelectedChar()
		{
			if (CMD_ChangeMonster.SelectMonsterData != null)
			{
				Transform transform = this.goMN_ICON_NOW.transform;
				GUIMonsterIcon guimonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(CMD_ChangeMonster.SelectMonsterData, transform.localScale, transform.localPosition, transform.parent, true, false);
				this.goMN_ICON_NOW_2 = guimonsterIcon.gameObject;
				this.goMN_ICON_NOW_2.SetActive(true);
				guimonsterIcon.Data = CMD_ChangeMonster.SelectMonsterData;
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
				guimonsterIcon.Gimmick = ExtraEffectUtil.IsExtraEffectMonster(CMD_ChangeMonster.SelectMonsterData, this.effectArray);
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
			this.csSelectPanelMonsterIcon.useLocationRecord = true;
			this.csSelectPanelMonsterIcon.SetCheckEnablePushAction(new Func<MonsterData, bool>(this.CheckEnablePush));
			this.targetMonsterList = list;
			list = MonsterDataMng.Instance().SelectionMDList(list);
			this.csSelectPanelMonsterIcon.AllBuild(list, localScale, new Action<MonsterData>(this.ActMIconLong), new Action<MonsterData>(this.ActMIconShort), false);
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
			if (this.changeMonsterData != null)
			{
				this.monsterList.CancelSelectedIcon(this.changeMonsterData);
				this.changeMonsterData = null;
			}
			if (tappedMonsterData != null)
			{
				GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(tappedMonsterData);
				this.iconGrayOut.SetSelect(icon);
				this.SetSelectedCharChg(tappedMonsterData);
				this.SelectButtonActive();
			}
		}

		private void SetSelectedCharChg(MonsterData monster)
		{
			this.changeMonsterData = monster;
			if (null != this.goMN_ICON_CHG_2)
			{
				UnityEngine.Object.Destroy(this.goMN_ICON_CHG_2);
			}
			Transform transform = this.goMN_ICON_CHG.transform;
			GUIMonsterIcon guimonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(this.changeMonsterData, transform.localScale, transform.localPosition, transform.parent, true, false);
			this.goMN_ICON_CHG_2 = guimonsterIcon.gameObject;
			this.goMN_ICON_CHG_2.SetActive(true);
			guimonsterIcon.Data = this.changeMonsterData;
			guimonsterIcon.SetTouchAct_S(new Action<MonsterData>(this.actRemoveChg));
			guimonsterIcon.SetTouchAct_L(new Action<MonsterData>(this.ActMIconLong));
			UIWidget component = this.goMN_ICON_CHG.GetComponent<UIWidget>();
			UIWidget component2 = guimonsterIcon.gameObject.GetComponent<UIWidget>();
			if (component != null && component2 != null)
			{
				int add = component.depth - component2.depth;
				DepthController component3 = guimonsterIcon.gameObject.GetComponent<DepthController>();
				component3.AddWidgetDepth(guimonsterIcon.transform, add);
			}
			this.goMN_ICON_CHG.SetActive(false);
			guimonsterIcon.Gimmick = ExtraEffectUtil.IsExtraEffectMonster(this.changeMonsterData, this.effectArray);
			this.SetChangeMonsterData();
		}

		private void actRemoveChg(MonsterData md)
		{
			if (this.changeMonsterData != null)
			{
				this.monsterList.CancelSelectedIcon(this.changeMonsterData);
				this.changeMonsterData = null;
			}
			if (null != this.goMN_ICON_CHG_2)
			{
				UnityEngine.Object.Destroy(this.goMN_ICON_CHG_2);
			}
			this.goMN_ICON_CHG.SetActive(true);
			this.ClearChangeMonsterData();
			this.SelectButtonActive();
		}

		private void ActMIconLong(MonsterData md)
		{
			CMD_CharacterDetailed.DataChg = md;
			CMD_CharacterDetailed cmd_CharacterDetailed = GUIMain.ShowCommonDialog(delegate(int i)
			{
				GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(md);
				icon.Lock = md.userMonster.IsLocked;
			}, "CMD_CharacterDetailed", null) as CMD_CharacterDetailed;
			cmd_CharacterDetailed.DisableEvolutionButton();
		}

		private void OnTouchDecide()
		{
			if (this.OnChanged != null)
			{
				this.OnChanged(this.changeMonsterData);
			}
			this.ClosePanel(true);
		}

		private void SelectButtonActive()
		{
			if (this.changeMonsterData != null)
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

		private bool CheckEnablePush(MonsterData monsterData)
		{
			bool result = false;
			if (CMD_ChangeMonster.SelectMonsterData == null)
			{
				result = true;
			}
			else if (CMD_ChangeMonster.SelectMonsterData.userMonster.userMonsterId != monsterData.userMonster.userMonsterId)
			{
				result = true;
			}
			return result;
		}

		public void SetChangedAction(Action<MonsterUserData> action)
		{
			this.OnChanged = action;
		}

		public void SetIconColosseumDeck(MonsterUserData targetMonster, MonsterUserData[] deck, List<GameWebAPI.RespDataMA_WorldDungeonSortieLimit.WorldDungeonSortieLimit> sortieLimit)
		{
			this.monsterList.SetIconColosseumDeck(targetMonster, deck);
			if (sortieLimit != null)
			{
				this.csSelectPanelMonsterIcon.SetIconSortieLimitParts(sortieLimit);
			}
		}
	}
}
