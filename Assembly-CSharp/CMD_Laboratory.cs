using Cutscene;
using Evolution;
using Master;
using Monster;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class CMD_Laboratory : CMD_PairSelectBase
{
	[SerializeField]
	private LaboratoryPartsStatusDetail digitamaDetail;

	[CompilerGenerated]
	private static Action <>f__mg$cache0;

	[CompilerGenerated]
	private static Action <>f__mg$cache1;

	[CompilerGenerated]
	private static Action <>f__mg$cache2;

	protected override void ShowSecondTutorial()
	{
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		if (tutorialObserver != null)
		{
			GUIMain.BarrierON(null);
			TutorialObserver tutorialObserver2 = tutorialObserver;
			string tutorialName = "second_tutorial_laboratory";
			if (CMD_Laboratory.<>f__mg$cache0 == null)
			{
				CMD_Laboratory.<>f__mg$cache0 = new Action(GUIMain.BarrierOFF);
			}
			tutorialObserver2.StartSecondTutorial(tutorialName, CMD_Laboratory.<>f__mg$cache0, delegate
			{
				GUICollider.EnableAllCollider("CMD_Laboratory");
			});
		}
		else
		{
			GUICollider.EnableAllCollider("CMD_Laboratory");
		}
		base.SetTutorialAnyTime("anytime_second_tutorial_laboratory");
	}

	private void OnPushDecide()
	{
		CMD_ResearchModalAlert popup = null;
		if (MonsterStatusData.IsVersionUp(this.baseDigimon.GetMonsterMaster().Simple.rare))
		{
			popup = this.OpenAlertTargetMonster(this.baseDigimon, StringMaster.GetString("LaboratoryResearchAlertInfo3"));
		}
		if (null != popup)
		{
			popup.SetActionYesButton(delegate
			{
				popup.SetCloseAction(delegate(int noop)
				{
					this.CheckPartnerMonster();
				});
			});
		}
		else
		{
			this.CheckPartnerMonster();
		}
	}

	private void CheckPartnerMonster()
	{
		CMD_ResearchModalAlert popup = null;
		if (MonsterStatusData.IsVersionUp(this.partnerDigimon.GetMonsterMaster().Simple.rare))
		{
			popup = this.OpenAlertTargetMonster(this.partnerDigimon, StringMaster.GetString("LaboratoryResearchAlertInfo2"));
		}
		else if (MonsterStatusData.IsArousal(this.partnerDigimon.monsterM.rare))
		{
			popup = this.OpenAlertTargetMonster(this.partnerDigimon, StringMaster.GetString("LaboratoryResearchAlertInfo"));
		}
		if (null != popup)
		{
			popup.SetActionYesButton(delegate
			{
				popup.SetCloseAction(delegate(int noop)
				{
					this.OpenConfirmResearch();
				});
			});
		}
		else
		{
			this.OpenConfirmResearch();
		}
	}

	private CMD_ResearchModalAlert OpenAlertTargetMonster(MonsterData monsterData, string description)
	{
		CMD_ResearchModalAlert cmd_ResearchModalAlert = GUIMain.ShowCommonDialog(null, "CMD_ResearchModalAlert", null) as CMD_ResearchModalAlert;
		cmd_ResearchModalAlert.SetTitle(StringMaster.GetString("LaboratoryResearchAlertTitle"));
		cmd_ResearchModalAlert.SetExp(description);
		cmd_ResearchModalAlert.SetBtnText_YES(StringMaster.GetString("SystemButtonYes"));
		cmd_ResearchModalAlert.SetBtnText_NO(StringMaster.GetString("SystemButtonNo"));
		cmd_ResearchModalAlert.SetDigimonIcon(monsterData);
		cmd_ResearchModalAlert.AdjustSize();
		return cmd_ResearchModalAlert;
	}

	private void OpenConfirmResearch()
	{
		MonsterEggStatusInfo digitamaStatus = this.CreateDigitamaStatus(this.baseDigimon);
		CMD_ResearchModal cmd_ResearchModal = GUIMain.ShowCommonDialog(null, "CMD_ResearchModal", null) as CMD_ResearchModal;
		cmd_ResearchModal.SetAlertEquipChip(this.baseDigimon, this.partnerDigimon);
		cmd_ResearchModal.SetDigitamaStatus(digitamaStatus);
		cmd_ResearchModal.SetActionYesButton(delegate
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			this.DoExec();
		});
	}

	private void DoExec()
	{
		this.useClusterBK = this.CalcCluster();
		GameWebAPI.RequestMN_MonsterCombination requestMN_MonsterCombination = new GameWebAPI.RequestMN_MonsterCombination();
		requestMN_MonsterCombination.SetSendData = delegate(GameWebAPI.MN_Req_Labo param)
		{
			param.baseUserMonsterId = this.baseDigimon.userMonster.userMonsterId;
			param.materialUserMonsterId = this.partnerDigimon.userMonster.userMonsterId;
		};
		requestMN_MonsterCombination.OnReceived = delegate(GameWebAPI.RespDataMN_LaboExec response)
		{
			DataMng.Instance().RespDataMN_LaboExec = response;
			if (response.userMonster != null)
			{
				ClassSingleton<MonsterUserDataMng>.Instance.AddUserMonsterData(response.userMonster);
			}
		};
		GameWebAPI.RequestMN_MonsterCombination request = requestMN_MonsterCombination;
		AppCoroutine.Start(request.Run(delegate()
		{
			AppCoroutine.Start(base.GetChipSlotInfo(), false);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
			this.ClosePanel(true);
		}, null), false);
	}

	protected override void EndSuccess()
	{
		int num = int.Parse(this.baseDigimon.userMonster.friendship);
		int friendshipMaxValue = MonsterFriendshipData.GetFriendshipMaxValue(this.baseDigimon.monsterMG.growStep);
		bool isArousal = num >= friendshipMaxValue;
		bool isResetEquipChip = false;
		if (this.baseDigimon.GetChipEquip().IsAttachedChip())
		{
			isResetEquipChip = true;
			base.RemoveEquipChip(false, this.baseDigimon.userMonster.userMonsterId);
		}
		if (this.partnerDigimon.GetChipEquip().IsAttachedChip())
		{
			isResetEquipChip = true;
			base.RemoveEquipChip(false, this.partnerDigimon.userMonster.userMonsterId);
		}
		string[] userMonsterIdList = new string[]
		{
			this.baseDigimon.userMonster.userMonsterId,
			this.partnerDigimon.userMonster.userMonsterId
		};
		ClassSingleton<MonsterUserDataMng>.Instance.DeleteUserMonsterData(userMonsterIdList);
		GooglePlayGamesTool.Instance.Laboratory();
		ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(MonsterDataMng.Instance().GetMonsterDataList());
		GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonsterData = this.GetUserMonsterData();
		MonsterData userMonster = ClassSingleton<MonsterUserDataMng>.Instance.GetUserMonster(userMonsterData.userMonsterId);
		CutsceneDataFusion cutsceneDataFusion = new CutsceneDataFusion();
		cutsceneDataFusion.path = "Cutscenes/Fusion";
		cutsceneDataFusion.baseModelId = this.baseDigimon.GetMonsterMaster().Group.modelId;
		cutsceneDataFusion.materialModelId = this.partnerDigimon.GetMonsterMaster().Group.modelId;
		cutsceneDataFusion.eggModelId = ClassSingleton<EvolutionData>.Instance.GetEggType(userMonster.userMonster.monsterEvolutionRouteId);
		cutsceneDataFusion.upArousal = isArousal;
		CutsceneDataFusion cutsceneDataFusion2 = cutsceneDataFusion;
		if (CMD_Laboratory.<>f__mg$cache1 == null)
		{
			CMD_Laboratory.<>f__mg$cache1 = new Action(CutSceneMain.FadeReqCutSceneEnd);
		}
		cutsceneDataFusion2.endCallback = CMD_Laboratory.<>f__mg$cache1;
		CutsceneDataFusion cutsceneData = cutsceneDataFusion;
		Loading.Invisible();
		CutSceneMain.FadeReqCutScene(cutsceneData, delegate()
		{
			this.OnStartCutScene(isArousal, isResetEquipChip);
		}, delegate()
		{
			this.characterDetailed.StartAnimation();
			if (!isResetEquipChip)
			{
				RestrictionInput.EndLoad();
			}
		}, 0.5f, 0.5f);
	}

	private void OnStartCutScene(bool isArousal, bool isResetEquipChip)
	{
		base.RemoveBaseDigimon();
		base.RemovePartnerDigimon();
		this.ClearTargetStatus();
		DataMng.Instance().US_PlayerInfoSubChipNum(this.useClusterBK);
		base.UpdateClusterNum();
		GUIPlayerStatus.RefreshParams_S(false);
		string userMonsterId = this.GetUserMonsterData().userMonsterId;
		MonsterData monsterDataByUserMonsterID = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(userMonsterId, false);
		Action endCutin = null;
		if (isResetEquipChip)
		{
			if (CMD_Laboratory.<>f__mg$cache2 == null)
			{
				CMD_Laboratory.<>f__mg$cache2 = new Action(RestrictionInput.EndLoad);
			}
			endCutin = CMD_Laboratory.<>f__mg$cache2;
		}
		this.characterDetailed = CMD_CharacterDetailed.CreateWindow(monsterDataByUserMonsterID, isArousal, isResetEquipChip, endCutin);
	}

	protected override void ClearTargetStatus()
	{
		this.digitamaDetail.ClearDigitamaStatus();
	}

	protected override GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList GetUserMonsterData()
	{
		return DataMng.Instance().RespDataMN_LaboExec.userMonster;
	}

	protected override int CalcCluster()
	{
		int num = 0;
		if (this.baseDigimon != null)
		{
			int num2 = this.baseDigimon.monsterM.GetArousal() + ConstValue.LABORATORY_BASE_PLUS_COEFFICIENT;
			num += num2 * ConstValue.LABORATORY_BASE_COEFFICIENT;
		}
		if (this.partnerDigimon != null)
		{
			int num3 = this.partnerDigimon.monsterM.GetArousal() + ConstValue.LABORATORY_PARTNER_PLUS_COEFFICIENT;
			num += num3 * ConstValue.LABORATORY_PARTNER_COEFFICIENT;
		}
		return num;
	}

	protected override void SetTargetStatus()
	{
		MonsterEggStatusInfo digitamaStatus = this.CreateDigitamaStatus(this.baseDigimon);
		this.digitamaDetail.SetDigitamaStatus(digitamaStatus);
	}

	protected override bool CanEnter()
	{
		List<MonsterData> list = MonsterDataMng.Instance().GetMonsterDataList();
		list = MonsterFilter.Filter(list, MonsterFilterType.GROWING_IN_GARDEN);
		return list.Count < ConstValue.MAX_CHILD_MONSTER;
	}

	protected override string GetInfoCannotEnter()
	{
		return StringMaster.GetString("LaboratoryMaxGarden");
	}

	protected override bool CanSelectMonster(int idx)
	{
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		List<MonsterData> list = monsterDataMng.GetMonsterDataList();
		list = MonsterFilter.Filter(list, MonsterFilterType.RESEARCH_TARGET);
		monsterDataMng.SortMDList(list);
		return list.Count > 1;
	}

	protected override void OpenCanNotSelectMonsterPop()
	{
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("LaboratoryNoUltimateTitle");
		cmd_ModalMessage.Info = StringMaster.GetString("LaboratoryNoUltimateInfo");
	}

	protected override void SetBaseTouchAct_L(GUIMonsterIcon cs)
	{
		cs.SetTouchAct_L(new Action<MonsterData>(base.ActMIconLong));
	}

	protected override void SetPartnerTouchAct_L(GUIMonsterIcon cs)
	{
		cs.SetTouchAct_L(new Action<MonsterData>(base.ActMIconLong));
	}

	protected override void SetBaseSelectType()
	{
		CMD_BaseSelect.BaseType = CMD_BaseSelect.BASE_TYPE.LABO;
	}

	protected override void OpenBaseDigimonNonePop()
	{
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("LaboratoryNotSelectedTitle");
		cmd_ModalMessage.Info = StringMaster.GetString("LaboratoryNotSelectedInfo");
	}

	private MonsterEggStatusInfo CreateDigitamaStatus(MonsterData baseData)
	{
		MonsterEggStatusInfo monsterEggStatusInfo = new MonsterEggStatusInfo();
		monsterEggStatusInfo.rare = baseData.monsterM.rare;
		int num = int.Parse(baseData.userMonster.friendship);
		int friendshipMaxValue = MonsterFriendshipData.GetFriendshipMaxValue(baseData.monsterMG.growStep);
		int num2 = monsterEggStatusInfo.rare.ToInt32();
		monsterEggStatusInfo.isArousal = false;
		monsterEggStatusInfo.isReturn = false;
		if (num2 >= 6)
		{
			monsterEggStatusInfo.isReturn = true;
		}
		else if (num == friendshipMaxValue && num2 < 5)
		{
			monsterEggStatusInfo.isArousal = true;
		}
		monsterEggStatusInfo.hpAbilityFlg = this.GetCandidateMedal(baseData.userMonster.hpAbilityFlg);
		monsterEggStatusInfo.attackAbilityFlg = this.GetCandidateMedal(baseData.userMonster.attackAbilityFlg);
		monsterEggStatusInfo.defenseAbilityFlg = this.GetCandidateMedal(baseData.userMonster.defenseAbilityFlg);
		monsterEggStatusInfo.spAttackAbilityFlg = this.GetCandidateMedal(baseData.userMonster.spAttackAbilityFlg);
		monsterEggStatusInfo.spDefenseAbilityFlg = this.GetCandidateMedal(baseData.userMonster.spDefenseAbilityFlg);
		monsterEggStatusInfo.speedAbilityFlg = this.GetCandidateMedal(baseData.userMonster.speedAbilityFlg);
		monsterEggStatusInfo.luck = baseData.userMonster.luck;
		return monsterEggStatusInfo;
	}

	private ConstValue.CandidateMedal GetCandidateMedal(string baseMedalType)
	{
		int num = int.Parse(baseMedalType);
		ConstValue.CandidateMedal result = ConstValue.CandidateMedal.NONE;
		if (num != 1)
		{
			if (num == 2)
			{
				result = ConstValue.CandidateMedal.SILVER;
			}
		}
		else
		{
			result = ConstValue.CandidateMedal.GOLD;
		}
		return result;
	}

	protected override string GetTitle()
	{
		return StringMaster.GetString("LaboratoryTitle");
	}
}
