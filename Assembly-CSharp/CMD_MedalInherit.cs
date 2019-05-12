using Ability;
using Cutscene;
using Master;
using Monster;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class CMD_MedalInherit : CMD_PairSelectBase
{
	[SerializeField]
	private AbilityUpgradeDetail abilityUpgradeDetail;

	private bool IsSuccsessAbilityUpgrade_bk;

	private GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList updatedUserMonster_bk;

	[CompilerGenerated]
	private static Action <>f__mg$cache0;

	[CompilerGenerated]
	private static Action <>f__mg$cache1;

	[CompilerGenerated]
	private static Action <>f__mg$cache2;

	[CompilerGenerated]
	private static Action <>f__mg$cache3;

	protected override void ShowSecondTutorial()
	{
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		if (tutorialObserver != null)
		{
			GUIMain.BarrierON(null);
			TutorialObserver tutorialObserver2 = tutorialObserver;
			string tutorialName = "second_tutorial_ability_upgrade";
			if (CMD_MedalInherit.<>f__mg$cache0 == null)
			{
				CMD_MedalInherit.<>f__mg$cache0 = new Action(GUIMain.BarrierOFF);
			}
			tutorialObserver2.StartSecondTutorial(tutorialName, CMD_MedalInherit.<>f__mg$cache0, delegate
			{
				GUICollider.EnableAllCollider("CMD_MedalInherit");
			});
		}
		else
		{
			GUICollider.EnableAllCollider("CMD_MedalInherit");
		}
		base.SetTutorialAnyTime("anytime_second_tutorial_ability_upgrade");
	}

	private void OnPushDecide()
	{
		CMD_ResearchModalAlert popup = null;
		if (MonsterStatusData.IsVersionUp(this.partnerDigimon.GetMonsterMaster().Simple.rare))
		{
			popup = this.OpenAlertPartnerMonster(this.partnerDigimon, StringMaster.GetString("MedalInheritAlertInfo2"));
		}
		else if (MonsterStatusData.IsArousal(this.partnerDigimon.monsterM.rare))
		{
			popup = this.OpenAlertPartnerMonster(this.partnerDigimon, StringMaster.GetString("MedalInheritAlertInfo"));
		}
		if (null != popup)
		{
			popup.SetActionYesButton(delegate
			{
				popup.SetCloseAction(delegate(int noop)
				{
					this.OpenConfirmMedalInheritance();
				});
			});
		}
		else
		{
			this.OpenConfirmMedalInheritance();
		}
	}

	private CMD_ResearchModalAlert OpenAlertPartnerMonster(MonsterData monsterData, string description)
	{
		CMD_ResearchModalAlert cmd_ResearchModalAlert = GUIMain.ShowCommonDialog(null, "CMD_ResearchModalAlert", null) as CMD_ResearchModalAlert;
		cmd_ResearchModalAlert.SetTitle(StringMaster.GetString("MedalInheritAlertTitle"));
		cmd_ResearchModalAlert.SetExp(description);
		cmd_ResearchModalAlert.SetBtnText_YES(StringMaster.GetString("SystemButtonYes"));
		cmd_ResearchModalAlert.SetBtnText_NO(StringMaster.GetString("SystemButtonNo"));
		cmd_ResearchModalAlert.SetDigimonIcon(monsterData);
		cmd_ResearchModalAlert.AdjustSize();
		return cmd_ResearchModalAlert;
	}

	private void OpenConfirmMedalInheritance()
	{
		MonsterAbilityStatusInfo monsterAbilityStatusInfo = ClassSingleton<AbilityData>.Instance.CreateAbilityStatus(this.baseDigimon, this.partnerDigimon);
		MonsterClientMaster monsterMasterByMonsterId = MonsterMaster.GetMonsterMasterByMonsterId(this.baseDigimon.userMonster.monsterId);
		int goldMedalMaxNum = MonsterArousalData.GetGoldMedalMaxNum(monsterMasterByMonsterId.Simple.rare);
		List<AbilityData.GoldMedalInheritanceState> goldMedalInheritanceList = ClassSingleton<AbilityData>.Instance.GetGoldMedalInheritanceList(this.baseDigimon.userMonster, this.partnerDigimon.userMonster);
		ClassSingleton<AbilityData>.Instance.AdjustMedalInheritanceRate(monsterAbilityStatusInfo, goldMedalInheritanceList, goldMedalMaxNum);
		int countInheritanceGoldMedal = ClassSingleton<AbilityData>.Instance.GetCountInheritanceGoldMedal(goldMedalInheritanceList);
		bool hasGoldOver = goldMedalMaxNum < countInheritanceGoldMedal;
		CMD_AbilityModal cmd_AbilityModal = GUIMain.ShowCommonDialog(null, "CMD_AbilityModal", null) as CMD_AbilityModal;
		cmd_AbilityModal.SetRemovePartnerEquipChip(this.partnerDigimon);
		cmd_AbilityModal.SetAnyNotUpdate(monsterAbilityStatusInfo);
		cmd_AbilityModal.SetHasGoldOver(hasGoldOver, goldMedalMaxNum);
		cmd_AbilityModal.SetStatus(monsterAbilityStatusInfo);
		cmd_AbilityModal.SetMonsterIcon(this.baseDigimon);
		cmd_AbilityModal.SetActionYesButton(delegate
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			DataMng.Instance().CheckCampaign(new Action<int>(this.DoExec), new GameWebAPI.RespDataCP_Campaign.CampaignType[]
			{
				GameWebAPI.RespDataCP_Campaign.CampaignType.MedalTakeOverUp
			});
		});
	}

	private void DoExec(int campaignStatus)
	{
		if (campaignStatus == -1)
		{
			return;
		}
		if (0 < campaignStatus)
		{
			RestrictionInput.EndLoad();
			DataMng.Instance().CampaignErrorCloseAllCommonDialog(campaignStatus == 1, delegate
			{
				RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
				DataMng dataMng = DataMng.Instance();
				if (CMD_MedalInherit.<>f__mg$cache1 == null)
				{
					CMD_MedalInherit.<>f__mg$cache1 = new Action(RestrictionInput.EndLoad);
				}
				dataMng.ReloadCampaign(CMD_MedalInherit.<>f__mg$cache1);
			});
			return;
		}
		this.useClusterBK = this.CalcCluster();
		GameWebAPI.RequestMN_MedalInherit request = new GameWebAPI.RequestMN_MedalInherit
		{
			SetSendData = delegate(GameWebAPI.MN_Req_MedalInherit param)
			{
				param.baseUserMonsterId = this.baseDigimon.userMonster.userMonsterId;
				param.materialUserMonsterId = this.partnerDigimon.userMonster.userMonsterId;
			},
			OnReceived = delegate(GameWebAPI.RespDataMN_MedalInherit response)
			{
				DataMng.Instance().RespDataMN_MedalInherit = response;
				if (response.userMonster != null)
				{
					this.IsSuccsessAbilityUpgrade_bk = ClassSingleton<AbilityData>.Instance.IsSuccsessAbilityUpgrade(this.baseDigimon.userMonster, response.userMonster);
					this.updatedUserMonster_bk = response.userMonster;
					ClassSingleton<MonsterUserDataMng>.Instance.UpdateUserMonsterData(response.userMonster);
				}
			}
		};
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
		bool isSuccess = this.IsSuccsessAbilityUpgrade_bk;
		bool isEquipChip = this.partnerDigimon.GetChipEquip().IsAttachedChip();
		ChipDataMng.GetUserChipSlotData().RemoveChipData(this.partnerDigimon.userMonster.userMonsterId, true);
		ClassSingleton<MonsterUserDataMng>.Instance.DeleteUserMonsterData(this.partnerDigimon.userMonster.userMonsterId);
		ChipDataMng.GetUserChipSlotData().DeleteMonsterSlot(this.partnerDigimon.userMonster.userMonsterId);
		GooglePlayGamesTool.Instance.Laboratory();
		ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(MonsterDataMng.Instance().GetMonsterDataList());
		CutsceneDataMedalInherit cutsceneDataMedalInherit = new CutsceneDataMedalInherit();
		cutsceneDataMedalInherit.path = "Cutscenes/MedalInherit";
		cutsceneDataMedalInherit.baseModelId = this.baseDigimon.GetMonsterMaster().Group.modelId;
		cutsceneDataMedalInherit.materialModelId = this.partnerDigimon.GetMonsterMaster().Group.modelId;
		CutsceneDataMedalInherit cutsceneDataMedalInherit2 = cutsceneDataMedalInherit;
		if (CMD_MedalInherit.<>f__mg$cache2 == null)
		{
			CMD_MedalInherit.<>f__mg$cache2 = new Action(CutSceneMain.FadeReqCutSceneEnd);
		}
		cutsceneDataMedalInherit2.endCallback = CMD_MedalInherit.<>f__mg$cache2;
		CutsceneDataMedalInherit cutsceneData = cutsceneDataMedalInherit;
		Loading.Invisible();
		CutSceneMain.FadeReqCutScene(cutsceneData, delegate()
		{
			this.OnStartCutScene(isSuccess, isEquipChip);
		}, delegate()
		{
			this.characterDetailed.StartAnimation();
			if (!isEquipChip)
			{
				RestrictionInput.EndLoad();
			}
		}, 0.5f, 0.5f);
	}

	private void OnStartCutScene(bool isSuccessInheritance, bool isResetEquipChip)
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
			if (CMD_MedalInherit.<>f__mg$cache3 == null)
			{
				CMD_MedalInherit.<>f__mg$cache3 = new Action(RestrictionInput.EndLoad);
			}
			endCutin = CMD_MedalInherit.<>f__mg$cache3;
		}
		this.characterDetailed = CMD_CharacterDetailed.CreateWindow(monsterDataByUserMonsterID, endCutin, isSuccessInheritance, isResetEquipChip);
	}

	protected override string GetTitle()
	{
		return StringMaster.GetString("MedalInheritTitle");
	}

	protected override void ClearTargetStatus()
	{
		this.abilityUpgradeDetail.ClearStatus();
	}

	protected override GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList GetUserMonsterData()
	{
		return this.updatedUserMonster_bk;
	}

	protected override int CalcCluster()
	{
		int result = 0;
		if (this.baseDigimon != null && this.partnerDigimon != null)
		{
			float maxAbility = ClassSingleton<AbilityData>.Instance.GetMaxAbility(this.baseDigimon.userMonster);
			float num = Mathf.Pow(1.15f, maxAbility);
			float num2 = Mathf.Floor(num * 10f) / 10f * 500f;
			int totalAbilityCount = ClassSingleton<AbilityData>.Instance.GetTotalAbilityCount(this.partnerDigimon.userMonster);
			float num3 = 1f + 0.1f * (float)(totalAbilityCount - 1);
			result = (int)Mathf.Floor(num2 * num3);
		}
		return result;
	}

	protected override void SetTargetStatus()
	{
		MonsterAbilityStatusInfo monsterAbilityStatusInfo = ClassSingleton<AbilityData>.Instance.CreateAbilityStatus(this.baseDigimon, this.partnerDigimon);
		MonsterClientMaster monsterMasterByMonsterId = MonsterMaster.GetMonsterMasterByMonsterId(this.baseDigimon.userMonster.monsterId);
		int goldMedalMaxNum = MonsterArousalData.GetGoldMedalMaxNum(monsterMasterByMonsterId.Simple.rare);
		List<AbilityData.GoldMedalInheritanceState> goldMedalInheritanceList = ClassSingleton<AbilityData>.Instance.GetGoldMedalInheritanceList(this.baseDigimon.userMonster, this.partnerDigimon.userMonster);
		ClassSingleton<AbilityData>.Instance.AdjustMedalInheritanceRate(monsterAbilityStatusInfo, goldMedalInheritanceList, goldMedalMaxNum);
		this.abilityUpgradeDetail.SetStatus(monsterAbilityStatusInfo);
	}

	protected override bool CanEnter()
	{
		List<MonsterData> list = MonsterDataMng.Instance().GetMonsterDataList();
		list = MonsterFilter.Filter(list, MonsterFilterType.ALL_OUT_GARDEN);
		list = MonsterFilter.Filter(list, MonsterFilterType.HAVE_MEDALS);
		return list.Count > 0;
	}

	protected override string GetInfoCannotEnter()
	{
		return StringMaster.GetString("MedalInheritEnterAlertInfo");
	}

	protected override bool CanSelectMonster(int idx)
	{
		List<MonsterData> list = MonsterDataMng.Instance().GetMonsterDataList();
		list = MonsterFilter.Filter(list, MonsterFilterType.ALL_OUT_GARDEN);
		list = MonsterFilter.Filter(list, MonsterFilterType.HAVE_MEDALS);
		if (idx == 0)
		{
			if (list.Count > 0)
			{
				return true;
			}
		}
		else if (list.Count > 0)
		{
			if (this.baseDigimon == null)
			{
				return true;
			}
			if (list.Count != 1 || list[0].userMonster.userMonsterId != this.baseDigimon.userMonster.userMonsterId)
			{
				return true;
			}
		}
		return false;
	}

	protected override void OpenCanNotSelectMonsterPop()
	{
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("MedalInheritAlertTitle");
		cmd_ModalMessage.Info = StringMaster.GetString("MedalInheritEnterAlertInfo");
	}

	protected override void SetBaseTouchAct_L(GUIMonsterIcon cs)
	{
		cs.SetTouchAct_L(new Action<MonsterData>(base.ActMIconLongFree));
	}

	protected override void SetPartnerTouchAct_L(GUIMonsterIcon cs)
	{
		cs.SetTouchAct_L(new Action<MonsterData>(base.ActMIconLong));
	}

	protected override void SetBaseSelectType()
	{
		CMD_BaseSelect.BaseType = CMD_BaseSelect.BASE_TYPE.MEDAL_INHERIT;
	}

	protected override void OnBaseSelected()
	{
		this.abilityUpgradeDetail.ShowIcon(this.baseDigimon, true);
	}

	protected override void OpenBaseDigimonNonePop()
	{
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("LaboratoryNotSelectedTitle");
		cmd_ModalMessage.Info = StringMaster.GetString("LaboratoryNotSelectedInfo");
	}
}
