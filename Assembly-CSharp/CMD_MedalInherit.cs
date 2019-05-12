using Ability;
using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CMD_MedalInherit : CMD_PairSelectBase
{
	[SerializeField]
	private AbilityUpgradeDetail abilityUpgradeDetail;

	private bool IsSuccsessAbilityUpgrade_bk;

	private GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList updatedUserMonster_bk;

	protected override void ShowSecondTutorial()
	{
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		if (tutorialObserver != null)
		{
			GUIMain.BarrierON(null);
			tutorialObserver.StartSecondTutorial("second_tutorial_ability_upgrade", new Action(GUIMain.BarrierOFF), delegate
			{
				GUICollider.EnableAllCollider("CMD_MedalInherit");
			});
		}
		else
		{
			GUICollider.EnableAllCollider("CMD_MedalInherit");
		}
	}

	protected override void SetTextConfirmPartnerArousal(CMD_ResearchModalAlert cd)
	{
		cd.SetTitle(StringMaster.GetString("MedalInheritAlertTitle"));
		cd.SetExp(StringMaster.GetString("MedalInheritAlertInfo"));
		cd.SetBtnText_YES(StringMaster.GetString("SystemButtonYes"));
		cd.SetBtnText_NO(StringMaster.GetString("SystemButtonNo"));
	}

	protected override void OpenConfirmTargetParameter(int selectButtonIndex)
	{
		if (selectButtonIndex == 1)
		{
			bool hasGoldOver = false;
			MonsterAbilityStatusInfo monsterAbilityStatusInfo = ClassSingleton<AbilityData>.Instance.CreateAbilityStatus(this.baseDigimon, this.partnerDigimon, ref hasGoldOver);
			CMD_AbilityModal cmd_AbilityModal = GUIMain.ShowCommonDialog(new Action<int>(base.OnCloseConfirm), "CMD_AbilityModal") as CMD_AbilityModal;
			cmd_AbilityModal.SetChipParams(this.partnerDigimon);
			cmd_AbilityModal.SetAnyNotUpdate(monsterAbilityStatusInfo);
			cmd_AbilityModal.SetHasGoldOver(hasGoldOver);
			cmd_AbilityModal.SetStatus(monsterAbilityStatusInfo);
			cmd_AbilityModal.ShowIcon(this.baseDigimon, true);
		}
	}

	protected override void DoExec(int result)
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
		this.useClusterBK = this.CalcCluster();
		GameWebAPI.RequestMN_MedalInherit request = new GameWebAPI.RequestMN_MedalInherit
		{
			SetSendData = delegate(GameWebAPI.MN_Req_MedalInherit param)
			{
				param.baseUserMonsterId = int.Parse(this.baseDigimon.userMonster.userMonsterId);
				param.materialUserMonsterId = int.Parse(this.partnerDigimon.userMonster.userMonsterId);
			},
			OnReceived = delegate(GameWebAPI.RespDataMN_MedalInherit response)
			{
				DataMng.Instance().RespDataMN_MedalInherit = response;
				if (response.userMonster != null)
				{
					this.IsSuccsessAbilityUpgrade_bk = ClassSingleton<AbilityData>.Instance.IsSuccsessAbilityUpgrade(this.baseDigimon.userMonster, response.userMonster);
					this.updatedUserMonster_bk = response.userMonster;
					DataMng.Instance().SetUserMonster(response.userMonster);
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
		bool hasChip = this.ResetChipAfterExec();
		int[] umidL = new int[]
		{
			int.Parse(this.partnerDigimon.userMonster.userMonsterId)
		};
		DataMng.Instance().DeleteUserMonsterList(umidL);
		GooglePlayGamesTool.Instance.Laboratory();
		int item = int.Parse(this.baseDigimon.monsterM.monsterGroupId);
		int item2 = int.Parse(this.partnerDigimon.monsterM.monsterGroupId);
		List<int> umidList = new List<int>
		{
			item
		};
		List<int> umidList2 = new List<int>
		{
			item2
		};
		MonsterDataMng.Instance().RefreshMonsterDataList();
		Loading.Invisible();
		CutSceneMain.FadeReqCutScene("Cutscenes/MedalInherit", new Action<int>(base.StartCutSceneCallBack), delegate(int index)
		{
			CutSceneMain.FadeReqCutSceneEnd();
			if (null != this.characterDetailed)
			{
				this.DisableCutinButton(this.characterDetailed.transform);
			}
			PartsMenu partsMenu = UnityEngine.Object.FindObjectOfType<PartsMenu>();
			if (null != partsMenu)
			{
				partsMenu.SetEnableMenuButton(false);
			}
		}, delegate(int index)
		{
			if (PartsUpperCutinController.Instance != null)
			{
				if (isSuccess)
				{
					PartsUpperCutinController.Instance.PlayAnimator(PartsUpperCutinController.AnimeType.TalentBlooms, delegate
					{
						this.ShowStoreChipDialog(hasChip);
					});
				}
				else
				{
					PartsUpperCutinController.Instance.PlayAnimator(PartsUpperCutinController.AnimeType.BloomsFailed, delegate
					{
						this.ShowStoreChipDialog(hasChip);
					});
				}
			}
			if (!hasChip)
			{
				RestrictionInput.EndLoad();
				this.EnableCutinButton();
				PartsMenu partsMenu = UnityEngine.Object.FindObjectOfType<PartsMenu>();
				if (null != partsMenu)
				{
					partsMenu.SetEnableMenuButton(true);
				}
			}
		}, umidList, umidList2, 3, 0, 0.5f, 0.5f);
	}

	private bool ResetChipAfterExec()
	{
		bool result = this.partnerDigimon.IsAttachedChip();
		GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] monsterChipList = ChipDataMng.GetMonsterChipList(this.partnerDigimon.userMonster.userMonsterId);
		if (monsterChipList != null)
		{
			foreach (GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChipList in monsterChipList)
			{
				if (userChipList.userMonsterId == int.Parse(this.partnerDigimon.userMonster.userMonsterId))
				{
					ChipDataMng.DeleteUserChipData(userChipList.userChipId);
				}
			}
		}
		return result;
	}

	protected override string GetTitle()
	{
		return StringMaster.GetString("MedalInheritTitle");
	}

	protected override string GetStoreChipInfo()
	{
		return StringMaster.GetString("MedalInheritCautionChip");
	}

	protected override void ClearTargetStatus()
	{
		this.abilityUpgradeDetail.ClearStatus();
	}

	protected override GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList GetUserMonsterData()
	{
		return this.updatedUserMonster_bk;
	}

	protected override void AddButton()
	{
	}

	protected override int CalcCluster()
	{
		int result;
		if (this.baseDigimon != null && this.partnerDigimon != null)
		{
			result = CalculatorUtil.CalcClusterForAbilityUpgrade(this.baseDigimon.userMonster, this.partnerDigimon.userMonster);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	protected override void SetTargetStatus()
	{
		bool flag = false;
		MonsterAbilityStatusInfo status = ClassSingleton<AbilityData>.Instance.CreateAbilityStatus(this.baseDigimon, this.partnerDigimon, ref flag);
		this.abilityUpgradeDetail.SetStatus(status);
	}

	protected override bool CanEnter()
	{
		List<MonsterData> list = MonsterDataMng.Instance().GetMonsterDataList(false);
		list = MonsterDataMng.Instance().SelectMonsterDataList(list, MonsterDataMng.SELECT_TYPE.ALL_OUT_GARDEN);
		list = MonsterDataMng.Instance().SelectMonsterDataList(list, MonsterDataMng.SELECT_TYPE.HAVE_MEDALS);
		return list.Count > 0;
	}

	protected override string GetInfoCannotEnter()
	{
		return StringMaster.GetString("MedalInheritEnterAlertInfo");
	}

	protected override bool CanSelectMonster(int idx)
	{
		List<MonsterData> list = MonsterDataMng.Instance().GetMonsterDataList(false);
		list = MonsterDataMng.Instance().SelectMonsterDataList(list, MonsterDataMng.SELECT_TYPE.ALL_OUT_GARDEN);
		list = MonsterDataMng.Instance().SelectMonsterDataList(list, MonsterDataMng.SELECT_TYPE.HAVE_MEDALS);
		if (idx == 0)
		{
			return list.Count > 0;
		}
		return idx != 1 || (list.Count > 0 && (this.baseDigimon == null || list.Count != 1 || !(list[0].userMonster.userMonsterId == this.baseDigimon.userMonster.userMonsterId)));
	}

	protected override void OpenCanNotSelectMonsterPop()
	{
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
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
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("LaboratoryNotSelectedTitle");
		cmd_ModalMessage.Info = StringMaster.GetString("LaboratoryNotSelectedInfo");
	}
}
