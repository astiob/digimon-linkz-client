using Master;
using Quest;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace NextChoice
{
	public sealed class NextChoiceReplay
	{
		private GUIScreenResult screenResult;

		private CMD_BattleNextChoice nextChoiceWindow;

		private CMD_ChangePOP_STONE confirmDialogRecoverStamina;

		private int shopBeforeStoneNum;

		public NextChoiceReplay(CMD_BattleNextChoice cmdNextChoice)
		{
			this.nextChoiceWindow = cmdNextChoice;
			this.screenResult = cmdNextChoice.screenResult;
		}

		private bool CheckStamina()
		{
			GameWebAPI.RespDataWD_DungeonStart dungeonStartInfo = ClassSingleton<QuestData>.Instance.RespDataWD_DungeonStart;
			GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM worldDungeonM = MasterDataMng.Instance().RespDataMA_WorldDungeonM.worldDungeonM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM x) => x.worldDungeonId == dungeonStartInfo.worldDungeonId);
			int num = int.Parse(worldDungeonM.needStamina);
			GameWebAPI.RespDataCP_Campaign respDataCP_Campaign = DataMng.Instance().RespDataCP_Campaign;
			GameWebAPI.RespDataCP_Campaign.CampaignInfo campaign = respDataCP_Campaign.GetCampaign(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestStmDown, worldDungeonM.worldStageId);
			if (campaign != null)
			{
				float num2 = (float)num;
				num = Mathf.CeilToInt(num2 * float.Parse(campaign.rate));
			}
			return num <= DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.stamina;
		}

		private void OnClosedEventConfirm(int selectButtonIndex)
		{
			if (selectButtonIndex == 0)
			{
				QuestStart.StartEventStage(DataMng.Instance().GetResultUtilData().last_dng_req);
			}
		}

		private void StartReplayRequest()
		{
			GameWebAPI.WD_Req_DngStart last_dng_req = DataMng.Instance().GetResultUtilData().last_dng_req;
			if (!ClassSingleton<QuestData>.Instance.ExistEvent(last_dng_req.dungeonId))
			{
				RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
				this.nextChoiceWindow.RequestDungeonStart();
			}
			else
			{
				QuestStart.OpenConfirmReplayEvent(new Action<int>(this.OnClosedEventConfirm));
			}
		}

		private void FinishedRecoverStamina()
		{
			CMD_ModalMessage.Create(StringMaster.GetString("StaminaRecoveryTitle"), StringMaster.GetString("StaminaRecoveryCompleted"), delegate(int noop)
			{
				this.StartReplayRequest();
			});
		}

		private IEnumerator RequestRecoverLife(Action onSuccessed)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
			APIRequestTask task = Singleton<UserDataMng>.Instance.RequestRecoverStamina(false);
			return task.Run(delegate
			{
				RestrictionInput.EndLoad();
				if (onSuccessed != null)
				{
					onSuccessed();
				}
			}, delegate(Exception nop)
			{
				RestrictionInput.EndLoad();
			}, null);
		}

		private void OnSelectedRecover()
		{
			AppCoroutine.Start(this.RequestRecoverLife(delegate
			{
				GUIPlayerStatus.RefreshParams_S(true);
				if (null != this.confirmDialogRecoverStamina)
				{
					this.confirmDialogRecoverStamina.SetCloseAction(delegate(int noop)
					{
						this.FinishedRecoverStamina();
					});
					this.confirmDialogRecoverStamina.ClosePanel(true);
				}
				else
				{
					this.FinishedRecoverStamina();
				}
			}), false);
		}

		private void OnCloseConfirmShop(int selectButtonIndex)
		{
			if (selectButtonIndex == 0)
			{
				this.shopBeforeStoneNum = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point;
				GUIMain.ShowCommonDialog(new Action<int>(this.EndShop), "CMD_Shop");
			}
		}

		private void EndShop(int noop)
		{
			CMD_BattleNextChoice cmd_BattleNextChoice = GUIMain.ShowCommonDialog(null, "CMD_BattleNextChoice") as CMD_BattleNextChoice;
			cmd_BattleNextChoice.screenResult = this.screenResult;
			if (this.shopBeforeStoneNum < DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point)
			{
				this.Start();
			}
		}

		private void OpenConfirmDialogRecoverStamina()
		{
			int stamina = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.stamina;
			int num = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.staminaMax);
			int point = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point;
			if (ConstValue.RECOVER_STAMINA_DIGISTONE_NUM <= point)
			{
				this.confirmDialogRecoverStamina = (GUIMain.ShowCommonDialog(null, "CMD_ChangePOP_STONE") as CMD_ChangePOP_STONE);
				this.confirmDialogRecoverStamina.Title = StringMaster.GetString("StaminaShortageTitle");
				this.confirmDialogRecoverStamina.Info = string.Format(StringMaster.GetString("StaminaShortageInfo"), new object[]
				{
					ConstValue.RECOVER_STAMINA_DIGISTONE_NUM,
					stamina,
					stamina + num,
					point
				});
				this.confirmDialogRecoverStamina.SetDigistone(point, ConstValue.RECOVER_STAMINA_DIGISTONE_NUM);
				this.confirmDialogRecoverStamina.BtnTextYes = StringMaster.GetString("StaminaRecoveryExecution");
				this.confirmDialogRecoverStamina.BtnTextNo = StringMaster.GetString("SystemButtonClose");
				this.confirmDialogRecoverStamina.OnPushedYesAction = new Action(this.OnSelectedRecover);
			}
			else
			{
				CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseConfirmShop), "CMD_Confirm") as CMD_Confirm;
				cmd_Confirm.Title = StringMaster.GetString("StaminaShortageTitle");
				cmd_Confirm.Info = string.Format(StringMaster.GetString("StaminaShortageGoShop"), ConstValue.RECOVER_STAMINA_DIGISTONE_NUM);
				cmd_Confirm.BtnTextYes = StringMaster.GetString("SystemButtonGoShop");
				cmd_Confirm.BtnTextNo = StringMaster.GetString("SystemButtonClose");
			}
		}

		public void Start()
		{
			if (this.CheckStamina())
			{
				this.StartReplayRequest();
			}
			else
			{
				this.OpenConfirmDialogRecoverStamina();
			}
		}
	}
}
