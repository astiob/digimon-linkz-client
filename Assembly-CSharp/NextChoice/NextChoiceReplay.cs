using Master;
using Quest;
using System;
using System.Collections;
using UnityEngine;

namespace NextChoice
{
	public sealed class NextChoiceReplay
	{
		private CMD_BattleNextChoice nextChoiceWindow;

		private CMD_ChangePOP_STONE confirmDialogRecoverStamina;

		private int shopBeforeStoneNum;

		public NextChoiceReplay(CMD_BattleNextChoice cmdNextChoice)
		{
			this.nextChoiceWindow = cmdNextChoice;
		}

		private bool CheckStamina()
		{
			GameWebAPI.WD_Req_DngStart lastDngReq = DataMng.Instance().GetResultUtilData().GetLastDngReq();
			GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM worldDungeonMaster = ClassSingleton<QuestData>.Instance.GetWorldDungeonMaster(lastDngReq.dungeonId);
			int num = int.Parse(worldDungeonMaster.needStamina);
			GameWebAPI.RespDataCP_Campaign respDataCP_Campaign = DataMng.Instance().RespDataCP_Campaign;
			GameWebAPI.RespDataCP_Campaign.CampaignInfo campaign = respDataCP_Campaign.GetCampaign(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestStmDown, worldDungeonMaster.worldStageId);
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
				CMD_Shop cmd = GUIMain.ShowCommonDialog(null, "CMD_Shop") as CMD_Shop;
				cmd.PartsTitle.SetReturnAct(delegate(int _i_)
				{
					cmd.SetCloseAction(delegate(int x)
					{
						this.EndShop(0);
					});
					cmd.ClosePanel(true);
				});
				cmd.PartsTitle.DisableReturnBtn(false);
				cmd.PartsTitle.SetCloseAct(delegate(int _i_)
				{
					GameObject dialog = GUIManager.GetDialog("CMD_BattleNextChoice");
					if (dialog != null)
					{
						CMD_BattleNextChoice component = dialog.GetComponent<CMD_BattleNextChoice>();
						component.ClosePanel(false);
					}
					cmd.SetCloseAction(delegate(int x)
					{
						CMD_BattleNextChoice.GoToFarm();
					});
					cmd.ClosePanel(true);
				});
			}
		}

		private void EndShop(int noop)
		{
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
