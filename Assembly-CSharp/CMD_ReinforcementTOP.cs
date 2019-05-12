using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_ReinforcementTOP : CMD
{
	private List<MonsterData> selectedMonsterDataList;

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

	[SerializeField]
	[Header("消費クラスタ")]
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

	private GUIMonsterIcon leftLargeMonsterIcon;

	private GameObject goSelectPanelMonsterIcon;

	private GUISelectPanelMonsterIcon csSelectPanelMonsterIcon;

	private int useClusterBK;

	private List<MonsterData> deckMDList;

	private CMD_CharacterDetailed charaDetail;

	private MonsterData oldMonsterData;

	private int oldLevel;

	private int upLuck;

	private MonsterData baseDigimon;

	private List<MonsterData> partnerDigimons = new List<MonsterData>();

	protected override void Awake()
	{
		base.Awake();
		for (int i = 0; i < this.goMN_ICON_LIST.Count; i++)
		{
			this.goMN_ICON_LIST[i].SetActive(false);
		}
		PartyUtil.ActMIconShort = new Action<MonsterData>(this.ActMIconShort);
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
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		if (monsterDataMng != null)
		{
			monsterDataMng.PushBackAllMonsterPrefab();
		}
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
		bool isLevelMax = this.baseDigimon.IsLevelMax();
		cmd_StrengthenCheck.SetParams(this.partnerDigimons, text, text2, text3, text4, isLevelMax);
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
		int[] mat = new int[this.partnerDigimons.Count];
		for (int i = 0; i < this.partnerDigimons.Count; i++)
		{
			mat[i] = int.Parse(this.partnerDigimons[i].userMonster.userMonsterId);
		}
		this.useClusterBK = CalculatorUtil.CalcClusterForReinforcement(this.partnerDigimons);
		GameWebAPI.RequestMN_MonsterFusion requestMN_MonsterFusion = new GameWebAPI.RequestMN_MonsterFusion();
		requestMN_MonsterFusion.SetSendData = delegate(GameWebAPI.MN_Req_Fusion param)
		{
			param.baseMonster = int.Parse(this.baseDigimon.userMonster.userMonsterId);
			param.materialMonster = mat;
		};
		requestMN_MonsterFusion.OnReceived = delegate(GameWebAPI.RespDataMN_FusionExec response)
		{
			DataMng.Instance().SetUserMonster(response.userMonster);
		};
		GameWebAPI.RequestMN_MonsterFusion request = requestMN_MonsterFusion;
		base.StartCoroutine(request.Run(new Action(this.EndReinforce), delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void EndReinforce()
	{
		int[] array = new int[this.partnerDigimons.Count];
		for (int i = 0; i < this.partnerDigimons.Count; i++)
		{
			array[i] = int.Parse(this.partnerDigimons[i].userMonster.userMonsterId);
		}
		DataMng.Instance().DeleteUserMonsterList(array);
		GooglePlayGamesTool.Instance.Reinforce();
		List<int> list = new List<int>();
		list.Add(int.Parse(this.baseDigimon.monsterM.monsterGroupId));
		List<int> list2 = new List<int>();
		for (int i = 0; i < this.partnerDigimons.Count; i++)
		{
			list2.Add(int.Parse(this.partnerDigimons[i].monsterM.monsterGroupId));
		}
		this.oldMonsterData = new MonsterData();
		this.oldMonsterData.userMonster = this.baseDigimon.GetDuplicateUserMonster(this.baseDigimon.userMonster);
		this.oldMonsterData.monsterM = this.baseDigimon.monsterM;
		this.oldLevel = int.Parse(this.baseDigimon.userMonster.level);
		CutSceneMain.Mons_BeforeLevel = this.oldLevel;
		int num = int.Parse(this.baseDigimon.userMonster.luck);
		int num2 = int.Parse(this.baseDigimon.userMonster.ex);
		bool flag = this.baseDigimon.IsLevelMax();
		MonsterDataMng.Instance().RefreshMonsterDataList();
		this.InitMonsterList(false);
		int num3 = int.Parse(this.baseDigimon.userMonster.luck);
		this.upLuck = num3 - num;
		int num4;
		if (flag)
		{
			num4 = num2;
		}
		else
		{
			num4 = int.Parse(this.baseDigimon.userMonster.ex);
		}
		GameWebAPI.RespDataMA_GetMonsterExperienceM.MonsterExperienceM[] monsterExperienceM = MasterDataMng.Instance().RespDataMA_MonsterExperienceM.monsterExperienceM;
		GameWebAPI.RespDataMA_GetMonsterExperienceM.MonsterExperienceM monsterExperienceM2 = null;
		List<int> list3 = new List<int>();
		for (int i = 0; i < monsterExperienceM.Length; i++)
		{
			int num5 = int.Parse(monsterExperienceM[i].level);
			if (num5 > CutSceneMain.Mons_BeforeLevel)
			{
				int num6 = int.Parse(monsterExperienceM[i].experienceNum);
				if (num6 > num4)
				{
					list3.Add(num6);
					break;
				}
				list3.Add(num6);
			}
			else
			{
				monsterExperienceM2 = monsterExperienceM[i];
			}
		}
		int[] array2 = new int[list3.Count + 1];
		array2[0] = int.Parse(monsterExperienceM2.experienceNum);
		for (int i = 0; i < list3.Count; i++)
		{
			array2[i + 1] = list3[i];
		}
		global::Debug.Log("================================================= EX_OFS_000 = " + (num2 - int.Parse(monsterExperienceM2.experienceNum)).ToString());
		global::Debug.Log("================================================= EX_BEFORE_000 = " + num2.ToString());
		global::Debug.Log("================================================= EX_AFTER_000 = " + num4.ToString());
		Loading.Invisible();
		CutSceneMain.FadeReqCutScene("Cutscenes/Training", new Action<int>(this.StartCutSceneCallBack), delegate(int index)
		{
			CutSceneMain.FadeReqCutSceneEnd();
		}, delegate(int index)
		{
			this.charaDetail.ShowByReinforcement(this.baseDigimon.userMonster.ex, this.oldMonsterData, this.oldLevel, this.upLuck);
			RestrictionInput.EndLoad();
		}, list, list2, 2, 1, 0.5f, 0.5f);
	}

	private void StartCutSceneCallBack(int i)
	{
		this.baseDigimon.csMIcon.Data = this.baseDigimon;
		this.baseDigimon.csMIcon.DimmLevel = GUIMonsterIcon.DIMM_LEVEL.ACTIVE;
		this.baseDigimon.csMIcon.SelectNum = -1;
		GUIMonsterIcon monsterCS_ByMonsterData = MonsterDataMng.Instance().GetMonsterCS_ByMonsterData(this.baseDigimon);
		monsterCS_ByMonsterData.SetTouchAct_S(new Action<MonsterData>(this.ActMIconS_Remove));
		this.baseDigimon.dimmLevel = GUIMonsterIcon.DIMM_LEVEL.DISABLE;
		monsterCS_ByMonsterData.DimmLevel = this.baseDigimon.dimmLevel;
		this.baseDigimon.selectNum = 0;
		monsterCS_ByMonsterData.SelectNum = this.baseDigimon.selectNum;
		for (int j = 0; j < this.partnerDigimons.Count; j++)
		{
			UnityEngine.Object.DestroyImmediate(this.partnerDigimons[j].csMIcon.gameObject);
			this.goMN_ICON_MAT_LIST[j].SetActive(true);
		}
		this.partnerDigimons = new List<MonsterData>();
		DataMng.Instance().US_PlayerInfoSubChipNum(this.useClusterBK);
		this.UpdateClusterNum();
		GUIPlayerStatus.RefreshParams_S(false);
		this.ShowChgInfo();
		this.SetDimParty(true);
		this.CalcAndShowLevelChange();
		this.BtnCont();
		this.SetLockIcon();
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
		if (this.baseDigimon != null && this.baseDigimon.IsLevelMax())
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
			float num4 = (float)MonsterDataMng.Instance().GetAddExpFromMonsterDataList(this.partnerDigimons) * num3;
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
		int num6 = CalculatorUtil.CalcClusterForReinforcement(this.partnerDigimons);
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
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		monsterDataMng.ClearSortMessAll();
		monsterDataMng.ClearLevelMessAll();
		List<MonsterData> list = monsterDataMng.GetMonsterDataList(false);
		list = monsterDataMng.SelectMonsterDataList(list, MonsterDataMng.SELECT_TYPE.ALL_OUT_GARDEN);
		list = monsterDataMng.SortMDList(list, false);
		this.csSelectPanelMonsterIcon.initLocation = initLoc;
		Vector3 localScale = this.goMN_ICON_LIST[0].transform.localScale;
		monsterDataMng.SetDimmAll(GUIMonsterIcon.DIMM_LEVEL.ACTIVE);
		monsterDataMng.SetSelectOffAll();
		monsterDataMng.ClearDimmMessAll();
		this.csSelectPanelMonsterIcon.useLocationRecord = true;
		this.csSelectPanelMonsterIcon.AllBuild(list, localScale, new Action<MonsterData>(this.ActMIconLong), new Action<MonsterData>(this.ActMIconShort), false);
		this.deckMDList = MonsterDataMng.Instance().GetDeckMonsterDataList(false);
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
		if (this.selectedMonsterDataList == null || this.selectedMonsterDataList.Count == 0)
		{
			isCheckDim = false;
		}
		else
		{
			foreach (MonsterData monsterData in this.selectedMonsterDataList)
			{
				if (monsterData == tappedMonsterData && monsterData != this.baseDigimon)
				{
					flag = true;
				}
				if (monsterData == tappedMonsterData)
				{
					isCheckDim = false;
				}
			}
		}
		foreach (MonsterData monsterData2 in this.deckMDList)
		{
			if (monsterData2 == tappedMonsterData)
			{
				isCheckDim = false;
			}
		}
		CMD_CharacterDetailed cmd_CharacterDetailed = GUIMain.ShowCommonDialog(delegate(int i)
		{
			PartyUtil.SetLock(tappedMonsterData, isCheckDim);
			if (tappedMonsterData == this.baseDigimon)
			{
				this.leftLargeMonsterIcon.Lock = tappedMonsterData.userMonster.IsLocked;
			}
			if (this.baseDigimon != null && this.IsMaxPartner)
			{
				this.SetOtherDimIcon(true);
			}
			if (this.baseDigimon == null)
			{
				MonsterDataMng.Instance().SetIconGrayOut("1", GUIMonsterIcon.DIMM_LEVEL.ACTIVE, new Action<MonsterData>(this.ActMIconShort));
			}
			else
			{
				MonsterDataMng.Instance().SetIconGrayOut("1", GUIMonsterIcon.DIMM_LEVEL.DISABLE, null);
				if ("1" == this.baseDigimon.monsterMG.monsterType)
				{
					GUIMonsterIcon monsterCS_ByMonsterData = MonsterDataMng.Instance().GetMonsterCS_ByMonsterData(this.baseDigimon);
					monsterCS_ByMonsterData.SetTouchAct_S(new Action<MonsterData>(this.ActMIconS_Remove));
				}
			}
		}, "CMD_CharacterDetailed") as CMD_CharacterDetailed;
		if (flag)
		{
			cmd_CharacterDetailed.Mode = CMD_CharacterDetailed.LockMode.Reinforcement;
		}
		return cmd_CharacterDetailed;
	}

	private bool IsMaxPartner
	{
		get
		{
			return this.partnerDigimons.Count >= 5;
		}
	}

	private void ActMIconShort(MonsterData md)
	{
		if (this.baseDigimon == null)
		{
			this.baseDigimon = md;
			this.leftLargeMonsterIcon = this.CreateIcon(this.baseDigimon, this.goMN_ICON_CHG);
			this.baseDigimon.csMIcon = this.leftLargeMonsterIcon;
			this.leftLargeMonsterIcon.SetTouchAct_S(new Action<MonsterData>(this.ActMIconS_Remove));
			this.ShowChgInfo();
			this.SetDimParty(true);
		}
		else if (!this.IsMaxPartner)
		{
			this.partnerDigimons.Add(md);
			int index = this.partnerDigimons.Count - 1;
			GUIMonsterIcon guimonsterIcon = this.CreateIcon(this.partnerDigimons[index], this.goMN_ICON_MAT_LIST[index]);
			this.partnerDigimons[index].csMIcon = guimonsterIcon;
			guimonsterIcon.SetTouchAct_S(new Action<MonsterData>(this.ActMIconS_Remove));
		}
		if (this.IsMaxPartner)
		{
			this.SetOtherDimIcon(true);
		}
		this.SetLockIcon();
		if (this.baseDigimon == null)
		{
			MonsterDataMng.Instance().SetIconGrayOut("1", GUIMonsterIcon.DIMM_LEVEL.ACTIVE, new Action<MonsterData>(this.ActMIconShort));
		}
		else
		{
			MonsterDataMng.Instance().SetIconGrayOut("1", GUIMonsterIcon.DIMM_LEVEL.DISABLE, null);
			if ("1" == this.baseDigimon.monsterMG.monsterType)
			{
				GUIMonsterIcon monsterCS_ByMonsterData = MonsterDataMng.Instance().GetMonsterCS_ByMonsterData(this.baseDigimon);
				monsterCS_ByMonsterData.SetTouchAct_S(new Action<MonsterData>(this.ActMIconS_Remove));
			}
		}
		this.CalcAndShowLevelChange();
		this.RefreshSelectedInMonsterList();
		this.BtnCont();
		this.SetLuckChanceDescript();
	}

	private void ActMIconS_Remove(MonsterData md)
	{
		if (md == this.baseDigimon)
		{
			this.DeleteIcon(this.baseDigimon, this.goMN_ICON_CHG);
			this.baseDigimon = null;
			this.ShowChgInfo();
			this.SetDimParty(false);
		}
		else
		{
			for (int i = 0; i < this.partnerDigimons.Count; i++)
			{
				if (this.partnerDigimons[i] == md)
				{
					this.DeleteIcon(this.partnerDigimons[i], this.goMN_ICON_MAT_LIST[i]);
					this.partnerDigimons.RemoveAt(i);
					this.ShiftPartnerIcon(i);
				}
			}
		}
		this.SetOtherDimIcon(false);
		this.SetLockIcon();
		if (this.baseDigimon == null)
		{
			MonsterDataMng.Instance().SetIconGrayOut("1", GUIMonsterIcon.DIMM_LEVEL.ACTIVE, new Action<MonsterData>(this.ActMIconShort));
		}
		else
		{
			MonsterDataMng.Instance().SetIconGrayOut("1", GUIMonsterIcon.DIMM_LEVEL.DISABLE, null);
			if ("1" == this.baseDigimon.monsterMG.monsterType)
			{
				GUIMonsterIcon monsterCS_ByMonsterData = MonsterDataMng.Instance().GetMonsterCS_ByMonsterData(this.baseDigimon);
				monsterCS_ByMonsterData.SetTouchAct_S(new Action<MonsterData>(this.ActMIconS_Remove));
			}
		}
		this.CalcAndShowLevelChange();
		this.RefreshSelectedInMonsterList();
		this.BtnCont();
		this.SetLuckChanceDescript();
	}

	private void SetDimParty(bool isDim)
	{
		for (int i = 0; i < this.deckMDList.Count; i++)
		{
			MonsterData monsterData = this.deckMDList[i];
			if (isDim)
			{
				if (monsterData != this.baseDigimon)
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

	private void SetOtherDimIcon(bool isDim)
	{
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		List<MonsterData> monsterDataList = monsterDataMng.GetMonsterDataList(false);
		foreach (MonsterData monsterData in monsterDataList)
		{
			if (monsterData != this.baseDigimon && !this.partnerDigimons.Contains(monsterData) && !this.deckMDList.Contains(monsterData))
			{
				PartyUtil.SetDimIcon(isDim, monsterData, string.Empty, monsterData.Lock);
			}
		}
	}

	private void SetLockIcon()
	{
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		List<MonsterData> monsterDataList = monsterDataMng.GetMonsterDataList(false);
		foreach (MonsterData monsterData in monsterDataList)
		{
			if (monsterData != this.baseDigimon && !this.partnerDigimons.Contains(monsterData) && !this.deckMDList.Contains(monsterData))
			{
				if (this.baseDigimon != null && monsterData.userMonster.IsLocked)
				{
					PartyUtil.SetDimIcon(true, monsterData, string.Empty, monsterData.Lock);
				}
			}
		}
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

	private void ShiftPartnerIcon(int idx)
	{
		int i;
		for (i = 0; i < this.partnerDigimons.Count; i++)
		{
			this.partnerDigimons[i].csMIcon.gameObject.transform.localPosition = this.goMN_ICON_MAT_LIST[i].transform.localPosition;
			this.goMN_ICON_MAT_LIST[i].SetActive(false);
		}
		while (i < this.goMN_ICON_MAT_LIST.Count)
		{
			this.goMN_ICON_MAT_LIST[i].SetActive(true);
			i++;
		}
	}

	private void RefreshSelectedInMonsterList()
	{
		this.selectedMonsterDataList = new List<MonsterData>();
		int snum;
		if (this.baseDigimon != null)
		{
			this.selectedMonsterDataList.Add(this.baseDigimon);
			snum = 0;
		}
		else
		{
			snum = 1;
		}
		for (int i = 0; i < this.partnerDigimons.Count; i++)
		{
			this.selectedMonsterDataList.Add(this.partnerDigimons[i]);
		}
		MonsterDataMng.Instance().SetSelectByMonsterDataList(this.selectedMonsterDataList, snum, true);
	}

	public void UpdateClusterNum()
	{
		this.possessionClusterLabel.text = StringFormat.Cluster(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney);
	}

	private void BtnCont()
	{
		bool flag = false;
		if (this.baseDigimon != null && this.partnerDigimons.Count > 0)
		{
			int num = CalculatorUtil.CalcClusterForReinforcement(this.partnerDigimons);
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

	private void SetLuckChanceDescript()
	{
		this.luckUpChance.text = string.Empty;
		if (this.baseDigimon != null)
		{
			if (this.IsLuckMax(this.baseDigimon.userMonster.monsterId, this.baseDigimon.userMonster.luck))
			{
				this.luckUpChance.text = StringMaster.GetString("ReinforcementLuckMax");
			}
			else if (0 < this.partnerDigimons.Count)
			{
				string[] array = new string[this.partnerDigimons.Count];
				for (int i = 0; i < this.partnerDigimons.Count; i++)
				{
					array[i] = this.partnerDigimons[i].monsterMG.tribe;
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
		GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monsterMasterByMonsterId = MonsterDataMng.Instance().GetMonsterMasterByMonsterId(monsterId);
		int num = int.Parse(luck);
		int num2 = int.Parse(monsterMasterByMonsterId.maxLuck);
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
				switch (int.Parse(partnerGrowStepList[i]))
				{
				case 4:
					num = Mathf.Max(num, 7);
					break;
				case 5:
				case 8:
					num = Mathf.Max(num, 20);
					break;
				case 6:
					num = Mathf.Max(num, 50);
					break;
				case 7:
				case 9:
					num = Mathf.Max(num, 100);
					break;
				}
			}
		}
		return num;
	}
}
