using Cutscene;
using Master;
using Monster;
using MonsterList.StrengthenChara;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class CMD_ReinforcementTOP : CMD
{
	[SerializeField]
	private GameObject goMN_ICON_CHG;

	[SerializeField]
	private List<GameObject> goMN_ICON_MAT_LIST;

	[SerializeField]
	private List<GameObject> goMN_ICON_LIST;

	[SerializeField]
	private MonsterBasicInfoExpGauge monsterBasicInfo;

	[SerializeField]
	private UILabel ngTX_LEV_S_CHG;

	[SerializeField]
	private UILabel ngTX_LEV_S_AFTER;

	[SerializeField]
	private UILabel ngTX_LEV_S_PLUS;

	[SerializeField]
	[Header("チップ部分")]
	private ChipBaseSelect chipBaseSelect;

	[SerializeField]
	[Header("所持クラスタ")]
	private UILabel possessionClusterLabel;

	[Header("消費クラスタ")]
	[SerializeField]
	private UILabel useClusterLabel;

	[SerializeField]
	private UISprite ngBTN_DECIDE;

	[SerializeField]
	private GUICollider clBTN_DECIDE;

	[Header("決定ラベル")]
	[SerializeField]
	private UILabelEx decideLabel;

	[SerializeField]
	private UILabel ngTX_MN_HAVE;

	[SerializeField]
	private UILabel luckUpChance;

	private BtnSort sortButton;

	private GUIMonsterIcon leftLargeMonsterIcon;

	private GameObject goSelectPanelMonsterIcon;

	private GUISelectPanelMonsterIcon csSelectPanelMonsterIcon;

	private int useClusterBK;

	private CMD_CharacterDetailed charaDetail;

	private List<MonsterData> targetMonsterList;

	private List<MonsterData> partnerMonsterList;

	private List<GUIMonsterIcon> partnerIconList;

	private StrengthenCharaIconGrayOut iconGrayOut;

	private StrengthenCharaMonsterList monsterList;

	private MonsterData baseDigimon { get; set; }

	private bool IsMaxPartner
	{
		get
		{
			return this.partnerMonsterList.Count >= 5;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.partnerIconList = new List<GUIMonsterIcon>();
		this.partnerMonsterList = new List<MonsterData>();
		this.iconGrayOut = new StrengthenCharaIconGrayOut();
		this.iconGrayOut.SetNormalAction(new Action<MonsterData>(this.ActMIconShort), new Action<MonsterData>(this.ActMIconLong));
		this.iconGrayOut.SetSelectedAction(new Action<MonsterData>(this.ActMIconS_Remove), new Action<MonsterData>(this.ActMIconLong));
		this.iconGrayOut.SetBlockAction(null, new Action<MonsterData>(this.ActMIconLong));
		this.monsterList = new StrengthenCharaMonsterList();
		this.monsterList.Initialize(ClassSingleton<MonsterUserDataMng>.Instance.GetDeckUserMonsterList(), ClassSingleton<MonsterUserDataMng>.Instance.GetColosseumDeckUserMonsterList(), this.partnerMonsterList, this.iconGrayOut);
		for (int i = 0; i < this.goMN_ICON_LIST.Count; i++)
		{
			this.goMN_ICON_LIST[i].SetActive(false);
		}
		this.chipBaseSelect.ClearChipIcons();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		GUICollider.DisableAllCollider("CMD_ReinforcementTOP");
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		this.SetCommonUI();
		base.StartCoroutine(this.InitReinforceTOP(f, sizeX, sizeY, aT));
	}

	private IEnumerator InitReinforceTOP(Action<int> f, float sizeX, float sizeY, float aT)
	{
		bool success = false;
		GameWebAPI.RespDataCP_Campaign.CampaignInfo trainExpUpData = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.TrainExpUp);
		GameWebAPI.RespDataCP_Campaign.CampaignInfo trainCostDownData = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.TrainCostDown);
		if (trainExpUpData == null && trainCostDownData == null)
		{
			APIRequestTask task = DataMng.Instance().RequestCampaignAll(false);
			yield return base.StartCoroutine(task.Run(delegate
			{
				success = true;
			}, delegate(Exception nop)
			{
				success = false;
			}, null));
		}
		else
		{
			success = true;
		}
		if (success)
		{
			base.PartsTitle.SetTitle(StringMaster.GetString("ReinforcementTitle"));
			this.InitMonsterList(true);
			this.ShowChgInfo();
			this.CalcAndShowLevelChange();
			this.UpdateClusterNum();
			base.ShowDLG();
			base.SetTutorialAnyTime("anytime_second_tutorial_reinforcement");
			base.Show(f, sizeX, sizeY, aT);
		}
		else
		{
			GUICollider.EnableAllCollider("CMD_ReinforcementTOP");
			base.ClosePanel(true);
		}
		RestrictionInput.EndLoad();
		yield break;
	}

	public override void ClosePanel(bool animation = true)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
		this.CloseAndFarmCamOn(animation);
	}

	private void CloseAndFarmCamOn(bool animation)
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
			ClassSingleton<FaceMissionAccessor>.Instance.faceMission.SetBadge();
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

	protected override void WindowClosed()
	{
		ClassSingleton<GUIMonsterIconList>.Instance.PushBackAllMonsterPrefab();
		base.WindowClosed();
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		if (tutorialObserver != null)
		{
			GUIMain.BarrierON(null);
			tutorialObserver.StartSecondTutorial("second_tutorial_reinforcement", new Action(GUIMain.BarrierOFF), delegate
			{
				GUICollider.EnableAllCollider("CMD_ReinforcementTOP");
			});
		}
		else
		{
			GUICollider.EnableAllCollider("CMD_ReinforcementTOP");
		}
	}

	private void OnTouchSort()
	{
	}

	private void OnTouchDecide()
	{
		CMD_StrengthenCheck cmd_StrengthenCheck = GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseReinforce), "CMD_StrengthenCheck") as CMD_StrengthenCheck;
		string text = this.useClusterLabel.text;
		string text2 = this.ngTX_LEV_S_CHG.text;
		string text3 = this.ngTX_LEV_S_AFTER.text;
		string text4 = this.ngTX_LEV_S_PLUS.text;
		bool isLevelMax = MonsterStatusData.IsLevelMax(this.baseDigimon.userMonster.monsterId, this.baseDigimon.userMonster.level);
		cmd_StrengthenCheck.SetParams(this.partnerMonsterList, text, text2, text3, text4, isLevelMax);
	}

	private void OnCloseReinforce(int idx)
	{
		if (idx == 0)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
			DataMng.Instance().CheckCampaign(new Action<int>(this.FusionExec), new GameWebAPI.RespDataCP_Campaign.CampaignType[]
			{
				GameWebAPI.RespDataCP_Campaign.CampaignType.TrainExpUp,
				GameWebAPI.RespDataCP_Campaign.CampaignType.TrainCostDown,
				GameWebAPI.RespDataCP_Campaign.CampaignType.TrainLuckUp
			});
		}
	}

	private void FusionExec(int result)
	{
		if (result == -1)
		{
			return;
		}
		if (result > 0)
		{
			RestrictionInput.EndLoad();
			DataMng.Instance().CampaignErrorCloseAllCommonDialog(result == 1, delegate
			{
				RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
				DataMng.Instance().ReloadCampaign(delegate
				{
					RestrictionInput.EndLoad();
				});
			});
			RestrictionInput.EndLoad();
			return;
		}
		string[] mat = new string[this.partnerMonsterList.Count];
		for (int i = 0; i < this.partnerMonsterList.Count; i++)
		{
			mat[i] = this.partnerMonsterList[i].userMonster.userMonsterId;
		}
		this.useClusterBK = this.CalcClusterForReinforcement(this.GetReinforcementCost(this.partnerMonsterList));
		GameWebAPI.RequestMN_MonsterFusion requestMN_MonsterFusion = new GameWebAPI.RequestMN_MonsterFusion();
		requestMN_MonsterFusion.SetSendData = delegate(GameWebAPI.MN_Req_Fusion param)
		{
			param.baseMonster = this.baseDigimon.userMonster.userMonsterId;
			param.materialMonster = mat;
		};
		requestMN_MonsterFusion.OnReceived = delegate(GameWebAPI.RespDataMN_FusionExec response)
		{
			ClassSingleton<MonsterUserDataMng>.Instance.UpdateUserMonsterData(response.userMonster);
		};
		GameWebAPI.RequestMN_MonsterFusion request = requestMN_MonsterFusion;
		GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList oldUserMonster = new GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList(this.baseDigimon.userMonster);
		base.StartCoroutine(request.Run(delegate()
		{
			this.EndReinforce(oldUserMonster);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void EndReinforce(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList oldUserMonster)
	{
		string[] userMonsterIdList = this.partnerMonsterList.Select((MonsterData x) => x.userMonster.userMonsterId).ToArray<string>();
		ClassSingleton<MonsterUserDataMng>.Instance.DeleteUserMonsterData(userMonsterIdList);
		ChipDataMng.DeleteEquipChip(userMonsterIdList);
		ChipDataMng.GetUserChipSlotData().DeleteMonsterSlotList(userMonsterIdList);
		GooglePlayGamesTool.Instance.Reinforce();
		ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(MonsterDataMng.Instance().GetMonsterDataList());
		this.InitMonsterList(false);
		int num = int.Parse(this.baseDigimon.userMonster.luck);
		int luckDiff = num - int.Parse(oldUserMonster.luck);
		CutsceneDataTraining cutsceneData = new CutsceneDataTraining
		{
			path = "Cutscenes/Training",
			baseModelId = this.baseDigimon.GetMonsterMaster().Group.modelId,
			materialNum = this.partnerMonsterList.Count,
			endCallback = new Action(CutSceneMain.FadeReqCutSceneEnd)
		};
		Loading.Invisible();
		CutSceneMain.FadeReqCutScene(cutsceneData, new Action(this.StartCutSceneCallBack), null, delegate(int index)
		{
			AppCoroutine.Start(this.charaDetail.StartReinforcementEffect(this.baseDigimon.userMonster.ex, oldUserMonster, luckDiff), false);
			RestrictionInput.EndLoad();
		}, 0.5f, 0.5f);
	}

	private void StartCutSceneCallBack()
	{
		this.leftLargeMonsterIcon.Data = this.baseDigimon;
		GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(this.baseDigimon);
		this.iconGrayOut.SetSelect(icon);
		for (int i = 0; i < this.partnerMonsterList.Count; i++)
		{
			UnityEngine.Object.Destroy(this.partnerIconList[i].gameObject);
			this.goMN_ICON_MAT_LIST[i].SetActive(true);
		}
		this.partnerMonsterList.Clear();
		this.partnerIconList.Clear();
		DataMng.Instance().US_PlayerInfoSubChipNum(this.useClusterBK);
		this.UpdateClusterNum();
		GUIPlayerStatus.RefreshParams_S(false);
		this.ShowChgInfo();
		this.monsterList.SetIconGrayOutUserMonsterList(this.baseDigimon);
		this.monsterList.SetGrayOutIconPartyUsed(this.baseDigimon);
		this.SetLuckChanceDescript();
		this.CalcAndShowLevelChange();
		this.BtnCont();
		this.charaDetail = this.ShowDetail(this.baseDigimon);
	}

	private void ShowChgInfo()
	{
		if (this.baseDigimon != null)
		{
			this.chipBaseSelect.SetSelectedCharChg(this.baseDigimon);
			this.monsterBasicInfo.SetMonsterData(this.baseDigimon);
		}
		else
		{
			this.chipBaseSelect.ClearChipIcons();
			this.monsterBasicInfo.ClearMonsterData();
			this.luckUpChance.text = string.Empty;
		}
	}

	private void CalcAndShowLevelChange()
	{
		int num = 0;
		string text = string.Empty;
		string text2 = string.Empty;
		if (this.baseDigimon != null && MonsterStatusData.IsLevelMax(this.baseDigimon.userMonster.monsterId, this.baseDigimon.userMonster.level))
		{
			text = this.baseDigimon.userMonster.level;
			text2 = this.baseDigimon.userMonster.level;
			int exp = int.Parse(this.baseDigimon.userMonster.ex);
			DataMng.ExperienceInfo experienceInfo = DataMng.Instance().GetExperienceInfo(exp);
			this.monsterBasicInfo.UpdateExpGauge(this.baseDigimon, experienceInfo);
		}
		else
		{
			int num2 = 0;
			if (this.baseDigimon != null)
			{
				num2 = int.Parse(this.baseDigimon.userMonster.ex);
			}
			GameWebAPI.RespDataCP_Campaign respDataCP_Campaign = DataMng.Instance().RespDataCP_Campaign;
			GameWebAPI.RespDataCP_Campaign.CampaignInfo campaign = respDataCP_Campaign.GetCampaign(GameWebAPI.RespDataCP_Campaign.CampaignType.TrainExpUp, false);
			float num3 = 1f;
			if (campaign != null)
			{
				num3 = campaign.rate.ToFloat();
			}
			float num4 = (float)this.GetAddExpFromMonsterDataList(this.partnerMonsterList) * num3;
			DataMng dataMng = DataMng.Instance();
			DataMng.ExperienceInfo experienceInfo2 = dataMng.GetExperienceInfo(num2);
			DataMng.ExperienceInfo experienceInfo3 = dataMng.GetExperienceInfo(num2 + (int)num4);
			if (this.baseDigimon != null)
			{
				int num5 = Mathf.Clamp(experienceInfo3.lev, 1, this.baseDigimon.monsterM.maxLevel.ToInt32());
				text2 = num5.ToString();
				this.monsterBasicInfo.UpdateExpGauge(this.baseDigimon, experienceInfo3);
				num = num5 - experienceInfo2.lev;
				if (num > 0)
				{
					this.ngTX_LEV_S_PLUS.color = ConstValue.PLUS_COLOR;
				}
				else if (num < 0)
				{
					this.ngTX_LEV_S_PLUS.color = ConstValue.MINUS_COLOR;
				}
				else
				{
					this.ngTX_LEV_S_PLUS.color = ConstValue.DEFAULT_COLOR;
				}
				text = experienceInfo2.lev.ToString();
			}
			else
			{
				this.ngTX_LEV_S_PLUS.color = ConstValue.DEFAULT_COLOR;
			}
		}
		this.ngTX_LEV_S_CHG.text = text;
		this.ngTX_LEV_S_AFTER.text = text2;
		this.ngTX_LEV_S_PLUS.text = string.Format(StringMaster.GetString("SystemPlusCount"), num.ToString());
		string gamemoney = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney;
		int num6 = this.CalcClusterForReinforcement(this.GetReinforcementCost(this.partnerMonsterList));
		this.useClusterLabel.text = StringFormat.Cluster(num6);
		if (num6 > gamemoney.ToInt32OrDefault(0))
		{
			this.useClusterLabel.color = Color.red;
		}
		else
		{
			this.useClusterLabel.color = Color.white;
		}
	}

	private void SetCommonUI()
	{
		this.goSelectPanelMonsterIcon = GUIManager.LoadCommonGUI("SelectListPanel/SelectListPanelMonsterIcon", base.gameObject);
		this.csSelectPanelMonsterIcon = this.goSelectPanelMonsterIcon.GetComponent<GUISelectPanelMonsterIcon>();
		if (this.goEFC_RIGHT != null)
		{
			this.goSelectPanelMonsterIcon.transform.parent = this.goEFC_RIGHT.transform;
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
		List<MonsterData> list = monsterDataMng.GetMonsterDataList();
		list = MonsterFilter.Filter(list, MonsterFilterType.ALL_OUT_GARDEN);
		monsterDataMng.SortMDList(list);
		monsterDataMng.SetSortLSMessage();
		ClassSingleton<GUIMonsterIconList>.Instance.SetLockIcon();
		this.monsterList.SetIconGrayOutUserMonsterList(this.baseDigimon);
		this.SetLuckChanceDescript();
		this.csSelectPanelMonsterIcon.SetCheckEnablePushAction(null);
		this.csSelectPanelMonsterIcon.useLocationRecord = true;
		this.targetMonsterList = list;
		list = MonsterDataMng.Instance().SelectionMDList(list);
		this.csSelectPanelMonsterIcon.initLocation = initLoc;
		Vector3 localScale = this.goMN_ICON_LIST[0].transform.localScale;
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
		if (this.baseDigimon != null)
		{
			this.monsterList.SetIconGrayOutUserMonsterList(this.baseDigimon);
			this.monsterList.SetGrayOutIconPartyUsed(this.baseDigimon);
		}
	}

	private void ActMIconLong(MonsterData monsterData)
	{
		this.ShowDetail(monsterData);
	}

	private CMD_CharacterDetailed ShowDetail(MonsterData tappedMonsterData)
	{
		CMD_CharacterDetailed.DataChg = tappedMonsterData;
		CMD_CharacterDetailed cmd_CharacterDetailed = GUIMain.ShowCommonDialog(delegate(int noop)
		{
			if (this.baseDigimon != null)
			{
				GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(tappedMonsterData);
				this.monsterList.LockIconReturnDetailed(icon, tappedMonsterData, this.baseDigimon);
				if (tappedMonsterData == this.baseDigimon)
				{
					this.leftLargeMonsterIcon.Lock = tappedMonsterData.userMonster.IsLocked;
				}
			}
		}, "CMD_CharacterDetailed") as CMD_CharacterDetailed;
		if (this.partnerMonsterList.Contains(tappedMonsterData))
		{
			cmd_CharacterDetailed.Mode = CMD_CharacterDetailed.LockMode.Reinforcement;
		}
		return cmd_CharacterDetailed;
	}

	private void ActMIconShort(MonsterData md)
	{
		if (this.baseDigimon == null)
		{
			GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(md);
			this.iconGrayOut.SetSelect(icon);
			this.baseDigimon = md;
			this.leftLargeMonsterIcon = this.CreateIcon(this.baseDigimon, this.goMN_ICON_CHG);
			this.ShowChgInfo();
			this.monsterList.SetIconGrayOutUserMonsterList(this.baseDigimon);
			this.monsterList.SetGrayOutIconPartyUsed(this.baseDigimon);
			this.SetLuckChanceDescript();
		}
		else if (!this.IsMaxPartner)
		{
			GameObject goEmpty = this.goMN_ICON_MAT_LIST[this.partnerIconList.Count];
			this.partnerIconList.Add(this.CreateIcon(md, goEmpty));
			this.partnerMonsterList.Add(md);
			GUIMonsterIcon icon2 = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(md);
			this.iconGrayOut.SetSelectPartner(icon2, this.partnerMonsterList.Count);
		}
		if (this.IsMaxPartner)
		{
			this.monsterList.BlockNotSelectIcon(this.baseDigimon);
		}
		this.CalcAndShowLevelChange();
		this.BtnCont();
		this.SetLuckChanceDescript();
	}

	private void ActMIconS_Remove(MonsterData md)
	{
		if (md == this.baseDigimon)
		{
			this.baseDigimon = null;
			UnityEngine.Object.Destroy(this.leftLargeMonsterIcon.gameObject);
			this.leftLargeMonsterIcon = null;
			this.goMN_ICON_CHG.SetActive(true);
			GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(md);
			this.iconGrayOut.CancelSelect(icon);
			this.ShowChgInfo();
			this.monsterList.ClearGrayOutIconPartyUsed();
		}
		else
		{
			for (int i = 0; i < this.partnerMonsterList.Count; i++)
			{
				if (this.partnerMonsterList[i] == md)
				{
					UnityEngine.Object.Destroy(this.partnerIconList[i].gameObject);
					this.partnerIconList.RemoveAt(i);
					this.goMN_ICON_MAT_LIST[i].SetActive(true);
					GUIMonsterIcon icon2 = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(md);
					this.iconGrayOut.CancelSelect(icon2);
					this.partnerMonsterList.RemoveAt(i);
					this.ShiftPartnerIcon(i);
					break;
				}
			}
			for (int j = 0; j < this.partnerMonsterList.Count; j++)
			{
				GUIMonsterIcon icon3 = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(this.partnerMonsterList[j]);
				icon3.SelectNum = j + 1;
			}
		}
		this.monsterList.SetIconGrayOutUserMonsterList(this.baseDigimon);
		this.SetLuckChanceDescript();
		this.CalcAndShowLevelChange();
		this.BtnCont();
	}

	private GUIMonsterIcon CreateIcon(MonsterData md, GameObject goEmpty)
	{
		Transform transform = goEmpty.transform;
		GUIMonsterIcon guimonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(md, transform.localScale, transform.localPosition, transform.parent, true, false);
		this.iconGrayOut.SelectIcon(guimonsterIcon);
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

	private void ShiftPartnerIcon(int index)
	{
		int i;
		for (i = 0; i < this.partnerMonsterList.Count; i++)
		{
			this.partnerIconList[i].gameObject.transform.localPosition = this.goMN_ICON_MAT_LIST[i].transform.localPosition;
			this.goMN_ICON_MAT_LIST[i].SetActive(false);
		}
		while (i < this.goMN_ICON_MAT_LIST.Count)
		{
			this.goMN_ICON_MAT_LIST[i].SetActive(true);
			i++;
		}
	}

	public void UpdateClusterNum()
	{
		this.possessionClusterLabel.text = StringFormat.Cluster(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney);
	}

	private void BtnCont()
	{
		bool flag = false;
		if (this.baseDigimon != null && this.partnerMonsterList.Count > 0)
		{
			int num = this.CalcClusterForReinforcement(this.GetReinforcementCost(this.partnerMonsterList));
			int num2 = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney);
			if (num <= num2)
			{
				flag = true;
			}
		}
		if (flag)
		{
			this.ngBTN_DECIDE.spriteName = "Common02_Btn_Blue";
			this.decideLabel.color = Color.white;
			this.clBTN_DECIDE.activeCollider = true;
		}
		else
		{
			this.ngBTN_DECIDE.spriteName = "Common02_Btn_Gray";
			this.decideLabel.color = Color.gray;
			this.clBTN_DECIDE.activeCollider = false;
		}
	}

	private int GetAddExpFromMonsterDataList(List<MonsterData> mdL)
	{
		int num = 0;
		for (int i = 0; i < mdL.Count; i++)
		{
			int num2 = int.Parse(mdL[i].userMonster.level) - 1;
			int num3 = int.Parse(mdL[i].monsterM.fusionExp);
			int num4 = int.Parse(mdL[i].monsterM.fusionExpRise);
			num += num3 + num4 * num2;
		}
		return num;
	}

	private int CalcClusterForReinforcement(float cost)
	{
		GameWebAPI.RespDataCP_Campaign respDataCP_Campaign = DataMng.Instance().RespDataCP_Campaign;
		GameWebAPI.RespDataCP_Campaign.CampaignInfo campaign = respDataCP_Campaign.GetCampaign(GameWebAPI.RespDataCP_Campaign.CampaignType.TrainCostDown, false);
		float num = 1f;
		if (campaign != null)
		{
			num = campaign.rate.ToFloat();
		}
		return Mathf.FloorToInt(cost * num);
	}

	private float GetReinforcementCost(List<MonsterData> mdL)
	{
		float num = 0f;
		for (int i = 0; i < mdL.Count; i++)
		{
			int num2 = int.Parse(mdL[i].userMonster.level) - 1;
			int num3 = int.Parse(mdL[i].monsterM.fusionExp);
			int num4 = int.Parse(mdL[i].monsterM.fusionExpRise);
			num += ((float)(num3 + num4 * num2) + 70f) * ConstValue.REINFORCEMENT_COEFFICIENT;
		}
		return num;
	}

	private void SetLuckChanceDescript()
	{
		this.luckUpChance.text = string.Empty;
		if (this.baseDigimon != null)
		{
			if (this.IsLuckMax(this.baseDigimon.userMonster.monsterId, this.baseDigimon.userMonster.luck))
			{
				this.luckUpChance.text = StringMaster.GetString("ReinforcementLuckMax");
			}
			else if (0 < this.partnerMonsterList.Count)
			{
				string[] array = new string[this.partnerMonsterList.Count];
				for (int i = 0; i < this.partnerMonsterList.Count; i++)
				{
					array[i] = this.partnerMonsterList[i].monsterMG.tribe;
				}
				if (this.CheckLuckUpChance(this.baseDigimon.monsterMG.tribe, array))
				{
					this.luckUpChance.text = StringMaster.GetString("ReinforcementLuckChance");
				}
			}
		}
	}

	private bool IsLuckMax(string monsterId, string luck)
	{
		GameWebAPI.RespDataMA_GetMonsterMS.MonsterM simple = MonsterMaster.GetMonsterMasterByMonsterId(monsterId).Simple;
		int num = int.Parse(luck);
		int num2 = int.Parse(simple.maxLuck);
		return num2 <= num;
	}

	private bool CheckLuckUpChance(string baseTribe, string[] partnerTribeList)
	{
		bool result = false;
		for (int i = 0; i < partnerTribeList.Length; i++)
		{
			if (partnerTribeList[i] == baseTribe)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	private int GetLuckUpChanceRateMax(string baseTribe, string[] partnerTribeList, string[] partnerGrowStepList)
	{
		int num = 0;
		for (int i = 0; i < partnerTribeList.Length; i++)
		{
			if (partnerTribeList[i] == baseTribe)
			{
				int growStep = int.Parse(partnerGrowStepList[i]);
				if (MonsterGrowStepData.IsGrowingScope(growStep))
				{
					num = Mathf.Max(num, 7);
				}
				else if (MonsterGrowStepData.IsRipeScope(growStep))
				{
					num = Mathf.Max(num, 20);
				}
				else if (MonsterGrowStepData.IsPerfectScope(growStep))
				{
					num = Mathf.Max(num, 50);
				}
				else if (MonsterGrowStepData.IsUltimateScope(growStep))
				{
					num = Mathf.Max(num, 100);
				}
			}
		}
		return num;
	}
}
