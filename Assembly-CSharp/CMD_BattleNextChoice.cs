using Master;
using Monster;
using NextChoice;
using Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class CMD_BattleNextChoice : CMD
{
	[HideInInspector]
	public GUIScreenResult screenResult;

	[SerializeField]
	private GameObject[] singlePlayButtons;

	[SerializeField]
	private UserStamina userStamina;

	[SerializeField]
	private BattleNextBattleOption battleOption;

	private bool isMulti;

	private static bool isGoToFarmCalled;

	private bool pushedBackKey;

	[CompilerGenerated]
	private static Action<int> <>f__mg$cache0;

	[CompilerGenerated]
	private static Action<int> <>f__mg$cache1;

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		CMD_BattleNextChoice.isGoToFarmCalled = false;
		ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(MonsterDataMng.Instance().GetMonsterDataList());
		if (ClassSingleton<QuestData>.Instance.RespData_WorldMultiResultInfoLogic != null)
		{
			aT = 0.1f;
			ClassSingleton<QuestData>.Instance.RespData_WorldMultiResultInfoLogic = null;
			if (!this.CanPushAgainButton())
			{
				this.singlePlayButtons[1].SetActive(false);
			}
			this.isMulti = true;
		}
		else
		{
			ClassSingleton<QuestData>.Instance.RespDataWD_DungeonResult = null;
			if (!this.CanPushAgainButton())
			{
				this.singlePlayButtons[1].SetActive(false);
			}
			this.isMulti = false;
		}
		if (null != this.userStamina)
		{
			this.userStamina.RefreshParams();
		}
		base.Show(f, sizeX, sizeY, aT);
	}

	private void OnPushedFarmButton()
	{
		base.SetCloseAction(delegate(int x)
		{
			CMD_BattleNextChoice.GoToFarm();
		});
		base.ClosePanel(true);
	}

	private bool CanPushAgainButton()
	{
		GameWebAPI.WD_Req_DngStart last_dng_req = DataMng.Instance().GetResultUtilData().last_dng_req;
		GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM[] worldDungeonM = MasterDataMng.Instance().RespDataMA_WorldDungeonM.worldDungeonM;
		int dungeonID = int.Parse(last_dng_req.dungeonId);
		GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM masterDungeon = worldDungeonM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM x) => x.worldDungeonId == dungeonID.ToString());
		GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM[] worldStageM = MasterDataMng.Instance().RespDataMA_WorldStageM.worldStageM;
		GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM worldStageM2 = worldStageM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM x) => x.worldStageId == masterDungeon.worldStageId);
		return true;
	}

	private void OnPushedAgainButton()
	{
		if (!this.CanPlayDungeonOver())
		{
			return;
		}
		if (!ClassSingleton<QuestData>.Instance.IsWD_DngInfoDataExist())
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			List<string> worldIdList = new List<string>
			{
				"1",
				"3",
				"8"
			};
			ClassSingleton<QuestData>.Instance.GetWorldDungeonInfo(worldIdList, delegate(bool flg)
			{
				RestrictionInput.EndLoad();
				if (flg)
				{
					this.OnPushedAgainCallBack();
				}
			});
		}
		else
		{
			this.OnPushedAgainCallBack();
		}
	}

	private void OnPushedAgainCallBack()
	{
		ClassSingleton<PlayLimit>.Instance.ClearTicketNumCont();
		ClassSingleton<PlayLimit>.Instance.ClearPlayLimitNumCont();
		GameWebAPI.WD_Req_DngStart last_dng_req = DataMng.Instance().GetResultUtilData().last_dng_req;
		GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM[] worldDungeonM = MasterDataMng.Instance().RespDataMA_WorldDungeonM.worldDungeonM;
		int dungeonID = int.Parse(last_dng_req.dungeonId);
		GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM masterDungeon = worldDungeonM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM x) => x.worldDungeonId == dungeonID.ToString());
		GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM[] worldStageM = MasterDataMng.Instance().RespDataMA_WorldStageM.worldStageM;
		GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM worldStageM2 = worldStageM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM x) => x.worldStageId == masterDungeon.worldStageId);
		if (QuestData.IsTicketQuest(worldStageM2.worldAreaId))
		{
			if (this.isMulti && "-1" == last_dng_req.userDungeonTicketId)
			{
				CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
				cmd_ModalMessage.Title = StringMaster.GetString("TicketQuestTitle");
				cmd_ModalMessage.Info = StringMaster.GetString("MultiParticipateAgainAlert");
				return;
			}
			GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons dng = ClassSingleton<QuestData>.Instance.GetTicketQuestDungeonByTicketID(last_dng_req.userDungeonTicketId);
			int num = int.Parse(dng.dungeonTicketNum);
			if (0 >= num)
			{
				CMD_ModalMessage cmd_ModalMessage2 = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
				cmd_ModalMessage2.Title = StringMaster.GetString("TicketQuestTitle");
				cmd_ModalMessage2.Info = StringMaster.GetString("QuestPlayLimitZeroInfo");
			}
			else
			{
				CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(delegate(int idx)
				{
					if (idx == 0)
					{
						ClassSingleton<PlayLimit>.Instance.SetTicketNumCont(dng, ConstValue.PLAYLIMIT_USE_COUNT);
						if (this.isMulti)
						{
							this.RequestDungeonStart();
						}
						else
						{
							NextChoiceReplay nextChoiceReplay2 = new NextChoiceReplay(new Action(this.RequestDungeonStart));
							nextChoiceReplay2.Start();
						}
					}
				}, "CMD_Confirm", null) as CMD_Confirm;
				cmd_Confirm.Title = StringMaster.GetString("TicketQuestTitle");
				cmd_Confirm.Info = string.Format(StringMaster.GetString("TicketQuestConfirmInfo"), worldStageM2.name, num, num - 1);
				cmd_Confirm.BtnTextYes = StringMaster.GetString("SystemButtonYes");
				cmd_Confirm.BtnTextNo = StringMaster.GetString("SystemButtonClose");
			}
		}
		else
		{
			GameWebAPI.RespDataWD_GetDungeonInfo dngeonInfoByWorldAreaId = ClassSingleton<QuestData>.Instance.GetDngeonInfoByWorldAreaId(worldStageM2.worldAreaId);
			List<QuestData.WorldDungeonData> wdd = ClassSingleton<QuestData>.Instance.GetWorldDungeonData_ByAreaIdStageId(worldStageM2.worldAreaId, masterDungeon.worldStageId, dngeonInfoByWorldAreaId, 0, false, true);
			if (wdd == null)
			{
				CMD_ModalMessage cmd_ModalMessage3 = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
				cmd_ModalMessage3.Title = StringMaster.GetString("MultiAgainAlertYetClearTitle");
				cmd_ModalMessage3.Info = StringMaster.GetString("MultiAgainAlertYetClearInfo");
				return;
			}
			int worldDungeonDataIndex = 0;
			for (int i = 0; i < wdd.Count; i++)
			{
				if (wdd[i].dungeon.worldDungeonId.ToString() == masterDungeon.worldDungeonId)
				{
					worldDungeonDataIndex = i;
					break;
				}
			}
			if (worldDungeonDataIndex < wdd.Count)
			{
				int usedCT = ConstValue.PLAYLIMIT_USE_COUNT;
				if (this.isMulti && "-1" == last_dng_req.userDungeonTicketId)
				{
					usedCT = 0;
				}
				bool flag = ClassSingleton<QuestData>.Instance.IsEmptyDng(wdd[worldDungeonDataIndex].dungeon, worldStageM2.worldAreaId);
				if (flag)
				{
					CMD_ModalMessage cmd_ModalMessage4 = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
					cmd_ModalMessage4.Title = StringMaster.GetString("QuestPlayLimitTitle");
					cmd_ModalMessage4.Info = StringMaster.GetString("QuestPlayLimitZeroInfo");
					return;
				}
				if (!ClassSingleton<PlayLimit>.Instance.PlayLimitCheck(wdd[worldDungeonDataIndex].dungeon, delegate(int idx)
				{
					if (idx == 0)
					{
						if (wdd[worldDungeonDataIndex].dungeon.playLimit.recoveryAssetCategoryId == 2)
						{
							CMD_Shop cmd = GUIMain.ShowCommonDialog(delegate(int _idx)
							{
							}, "CMD_Shop", null) as CMD_Shop;
							cmd.PartsTitle.SetReturnAct(delegate(int _i_)
							{
								cmd.ClosePanel(true);
							});
							cmd.PartsTitle.DisableReturnBtn(false);
							cmd.PartsTitle.SetCloseAct(delegate(int _i_)
							{
								this.<ClosePanel>__BaseCallProxy0(false);
								cmd.SetCloseAction(delegate(int x)
								{
									CMD_BattleNextChoice.GoToFarm();
								});
								cmd.ClosePanel(true);
							});
						}
						else if (wdd[worldDungeonDataIndex].dungeon.playLimit.recoveryAssetCategoryId == 6)
						{
							GameWebAPI.RespDataMA_GetItemM.ItemM itemM = MasterDataMng.Instance().RespDataMA_ItemM.GetItemM(wdd[worldDungeonDataIndex].dungeon.playLimit.recoveryAssetValue.ToString());
							CMD_ModalMessage cmd_ModalMessage6 = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
							cmd_ModalMessage6.Title = string.Format(StringMaster.GetString("SystemShortage"), itemM.name);
							cmd_ModalMessage6.Info = string.Format(StringMaster.GetString("QuestPlayLimitItemShortInfo"), itemM.name);
						}
					}
				}, delegate(int _idx)
				{
					ClassSingleton<PlayLimit>.Instance.RecoverPlayLimit(wdd[worldDungeonDataIndex].dungeon, new Action<GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons>(this.OnSuccessedRecoverPlayLimit));
				}, usedCT))
				{
					return;
				}
				if (this.isMulti)
				{
					this.RequestDungeonStart();
				}
				else
				{
					NextChoiceReplay nextChoiceReplay = new NextChoiceReplay(new Action(this.RequestDungeonStart));
					nextChoiceReplay.Start();
				}
			}
			else
			{
				CMD_ModalMessage cmd_ModalMessage5 = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
				cmd_ModalMessage5.Title = StringMaster.GetString("MultiAgainAlertYetClearTitle");
				cmd_ModalMessage5.Info = StringMaster.GetString("MultiAgainAlertYetClearInfo");
			}
		}
	}

	private void OnSuccessedRecoverPlayLimit(GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons dng)
	{
	}

	public void RequestDungeonStart()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		if (this.isMulti)
		{
			int[] aliveMonsterList = this.GetAliveMonsterList();
			GameWebAPI.WD_Req_DngStart sendData = DataMng.Instance().GetResultUtilData().last_dng_req;
			APIRequestTask task = this.RequestUserMonsterData(aliveMonsterList, true);
			AppCoroutine.Start(task.Run(delegate
			{
				if (null != DataMng.Instance() && DataMng.Instance().WD_ReqDngResult != null)
				{
					DataMng.Instance().WD_ReqDngResult.dungeonId = sendData.dungeonId;
				}
				RestrictionInput.EndLoad();
				GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseMultiBattleMenu), "CMD_MultiBattleParticipateMenu", null);
			}, delegate(Exception noop)
			{
				RestrictionInput.EndLoad();
			}, null), false);
		}
		else
		{
			int[] aliveMonsterList2 = this.GetAliveMonsterList();
			APIRequestTask apirequestTask = this.RequestUserMonsterData(aliveMonsterList2, true);
			GameWebAPI.WD_Req_DngStart sendData = DataMng.Instance().GetResultUtilData().last_dng_req;
			GameWebAPI.RequestWD_WorldStart requestWD_WorldStart = new GameWebAPI.RequestWD_WorldStart();
			requestWD_WorldStart.SetSendData = delegate(GameWebAPI.WD_Req_DngStart param)
			{
				param.dungeonId = sendData.dungeonId;
				param.deckNum = sendData.deckNum;
				param.userDungeonTicketId = sendData.userDungeonTicketId;
			};
			requestWD_WorldStart.OnReceived = delegate(GameWebAPI.RespDataWD_DungeonStart response)
			{
				ClassSingleton<QuestData>.Instance.RespDataWD_DungeonStart = response;
			};
			GameWebAPI.RequestWD_WorldStart request = requestWD_WorldStart;
			apirequestTask.Add(new APIRequestTask(request, false));
			AppCoroutine.Start(apirequestTask.Run(delegate
			{
				if (null != DataMng.Instance() && DataMng.Instance().WD_ReqDngResult != null)
				{
					DataMng.Instance().WD_ReqDngResult.dungeonId = sendData.dungeonId;
				}
				BattleNextBattleOption.SaveBattleMenuSettings(this.battleOption.GetBattleOptionSettings());
				this.ReceivedDungeonStart();
			}, delegate(Exception noop)
			{
				RestrictionInput.EndLoad();
			}, null), false);
		}
	}

	private void ReceivedDungeonStart()
	{
		Loading.DisableMask();
		TipsLoading.Instance.StartTipsLoad(CMD_Tips.DISPLAY_PLACE.QuestToSoloBattle, true);
		CMD_BattleNextChoice component = GameObject.Find("CMD_BattleNextChoice").GetComponent<CMD_BattleNextChoice>();
		component.SetCloseAction(delegate(int noop)
		{
			DataMng.Instance().WD_ReqDngResult.clear = 0;
			BattleStateManager.StartSingle(0.5f, 0.5f, true, null);
		});
		component.ClosePanel(true);
	}

	private void OnCloseMultiBattleMenu(int idx)
	{
		switch (idx)
		{
		case 1:
		{
			GameWebAPI.WD_Req_DngStart lastDngReq = DataMng.Instance().GetResultUtilData().GetLastDngReq();
			CMD_MultiRecruitTop cmd1 = CMD_MultiRecruitTop.CreateTargetStage(lastDngReq.dungeonId);
			cmd1.PartsTitle.SetReturnAct(delegate(int i)
			{
				cmd1.ClosePanel(true);
			});
			cmd1.PartsTitle.DisableReturnBtn(false);
			cmd1.PartsTitle.SetCloseAct(delegate(int i)
			{
				this.<ClosePanel>__BaseCallProxy0(false);
				cmd1.SetCloseAction(delegate(int x)
				{
					CMD_BattleNextChoice.GoToFarm();
				});
				cmd1.ClosePanel(true);
			});
			break;
		}
		case 2:
		{
			NextChoiceReplay nextChoiceReplay = new NextChoiceReplay(new Action(this.MultyParty));
			nextChoiceReplay.Start();
			break;
		}
		}
	}

	private void MultyParty()
	{
		GameWebAPI.WD_Req_DngStart last_dng_req = DataMng.Instance().GetResultUtilData().last_dng_req;
		GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM[] worldDungeonM = MasterDataMng.Instance().RespDataMA_WorldDungeonM.worldDungeonM;
		int dungeonID = int.Parse(last_dng_req.dungeonId);
		GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM masterDungeon = worldDungeonM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM x) => x.worldDungeonId == dungeonID.ToString());
		GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM[] worldStageM = MasterDataMng.Instance().RespDataMA_WorldStageM.worldStageM;
		GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM worldStageM2 = worldStageM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM x) => x.worldStageId == masterDungeon.worldStageId);
		CMD_PartyEdit.replayMultiStageId = worldStageM2.worldStageId;
		CMD_PartyEdit.replayMultiDungeonId = last_dng_req.dungeonId;
		CMD_PartyEdit.ModeType = CMD_PartyEdit.MODE_TYPE.MULTI;
		CMD_PartyEdit partyEdit = GUIMain.ShowCommonDialog(null, "CMD_PartyEdit", new Action<CommonDialog>(this.OnReadyPartyEdit)) as CMD_PartyEdit;
		partyEdit.PartsTitle.SetReturnAct(delegate(int i)
		{
			partyEdit.ClosePanel(true);
		});
		partyEdit.PartsTitle.DisableReturnBtn(false);
		partyEdit.PartsTitle.SetCloseAct(delegate(int i)
		{
			this.<ClosePanel>__BaseCallProxy0(false);
			partyEdit.SetCloseAction(delegate(int x)
			{
				CMD_BattleNextChoice.GoToFarm();
			});
			partyEdit.ClosePanel(true);
		});
	}

	private void OnReadyPartyEdit(CommonDialog dialog)
	{
		CMD_PartyEdit cmd_PartyEdit = dialog as CMD_PartyEdit;
		if (null != cmd_PartyEdit)
		{
			GameWebAPI.WD_Req_DngStart last_dng_req = DataMng.Instance().GetResultUtilData().last_dng_req;
			GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM[] worldDungeonM = MasterDataMng.Instance().RespDataMA_WorldDungeonM.worldDungeonM;
			int dungeonID = int.Parse(last_dng_req.dungeonId);
			GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM masterDungeon = worldDungeonM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM x) => x.worldDungeonId == dungeonID.ToString());
			GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM[] worldStageM = MasterDataMng.Instance().RespDataMA_WorldStageM.worldStageM;
			GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM worldStageM2 = worldStageM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM x) => x.worldStageId == masterDungeon.worldStageId);
			cmd_PartyEdit.SetQuestId(worldStageM2.worldAreaId, worldStageM2.worldStageId, last_dng_req.dungeonId);
		}
	}

	private void OnPushedNextQuestButton()
	{
		ClassSingleton<PlayLimit>.Instance.ClearTicketNumCont();
		ClassSingleton<PlayLimit>.Instance.ClearPlayLimitNumCont();
		if (!this.CanPlayDungeonOver())
		{
			return;
		}
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		int[] aliveMonsterList = this.GetAliveMonsterList();
		APIRequestTask apirequestTask = this.RequestUserMonsterData(aliveMonsterList, true);
		GameWebAPI.RespDataCP_Campaign.CampaignInfo campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestExpUp);
		if (campaignInfo == null)
		{
			campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestCipUp);
		}
		if (campaignInfo == null)
		{
			campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestMatUp);
		}
		if (campaignInfo == null)
		{
			campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestRrDrpUp);
		}
		if (campaignInfo == null)
		{
			campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestFriUp);
		}
		if (campaignInfo == null)
		{
			campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestStmDown);
		}
		if (campaignInfo == null)
		{
			campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestExpUpMul);
		}
		if (campaignInfo == null)
		{
			campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestCipUpMul);
		}
		if (campaignInfo == null)
		{
			campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestMatUpMul);
		}
		if (campaignInfo == null)
		{
			campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestRrDrpUpMul);
		}
		if (campaignInfo == null)
		{
			campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestFriUpMul);
		}
		if (campaignInfo == null)
		{
			campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestStmDownMul);
		}
		if (campaignInfo == null)
		{
			apirequestTask.Add(DataMng.Instance().RequestCampaignAll(true));
		}
		AppCoroutine.Start(apirequestTask.Run(delegate
		{
			this.RequestQuestData();
		}, null, null), false);
	}

	private void RequestQuestData()
	{
		List<string> list = new List<string>();
		list.Add("1");
		list.Add("3");
		list.Add("8");
		ClassSingleton<QuestData>.Instance.GetWorldDungeonInfo(list, new Action<bool>(this.OpenQuestUI));
	}

	private void OpenQuestUI(bool isSuccess)
	{
		if (isSuccess)
		{
			CMD_BattleNextChoice.<OpenQuestUI>c__AnonStoreyA <OpenQuestUI>c__AnonStoreyA = new CMD_BattleNextChoice.<OpenQuestUI>c__AnonStoreyA();
			GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM[] worldDungeonM = MasterDataMng.Instance().RespDataMA_WorldDungeonM.worldDungeonM;
			<OpenQuestUI>c__AnonStoreyA.nextDungeonID = int.Parse(this.screenResult.clearDungeonID);
			int nextDungeonID = <OpenQuestUI>c__AnonStoreyA.nextDungeonID;
			<OpenQuestUI>c__AnonStoreyA.nextDungeonID++;
			<OpenQuestUI>c__AnonStoreyA.masterNextDangeon = worldDungeonM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM x) => x.worldDungeonId == <OpenQuestUI>c__AnonStoreyA.nextDungeonID.ToString());
			if (<OpenQuestUI>c__AnonStoreyA.masterNextDangeon == null)
			{
				<OpenQuestUI>c__AnonStoreyA.nextDungeonID--;
				<OpenQuestUI>c__AnonStoreyA.masterNextDangeon = worldDungeonM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM x) => x.worldDungeonId == <OpenQuestUI>c__AnonStoreyA.nextDungeonID.ToString());
			}
			GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM[] worldStageM = MasterDataMng.Instance().RespDataMA_WorldStageM.worldStageM;
			<OpenQuestUI>c__AnonStoreyA.nextStage = worldStageM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM x) => x.worldStageId == <OpenQuestUI>c__AnonStoreyA.masterNextDangeon.worldStageId);
			if (<OpenQuestUI>c__AnonStoreyA.nextDungeonID > nextDungeonID && <OpenQuestUI>c__AnonStoreyA.nextStage == null)
			{
				<OpenQuestUI>c__AnonStoreyA.nextDungeonID--;
				<OpenQuestUI>c__AnonStoreyA.masterNextDangeon = worldDungeonM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM x) => x.worldDungeonId == <OpenQuestUI>c__AnonStoreyA.nextDungeonID.ToString());
				<OpenQuestUI>c__AnonStoreyA.nextStage = worldStageM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM x) => x.worldStageId == <OpenQuestUI>c__AnonStoreyA.masterNextDangeon.worldStageId);
			}
			if (3000 < int.Parse(<OpenQuestUI>c__AnonStoreyA.masterNextDangeon.worldDungeonId))
			{
				ClassSingleton<QuestTOPAccessor>.Instance.nextAreaFlg = false;
				ClassSingleton<QuestTOPAccessor>.Instance.nextAreaEvent = true;
			}
			else
			{
				ClassSingleton<QuestTOPAccessor>.Instance.nextAreaFlg = this.IsLockNextDungeon(int.Parse(this.screenResult.clearDungeonID));
				ClassSingleton<QuestTOPAccessor>.Instance.nextAreaEvent = false;
			}
			BattleNextBattleOption.ClearBattleMenuSettings();
			base.SetCloseAction(delegate(int x)
			{
				CMD_BattleNextChoice.<OpenQuestUI>c__AnonStoreyA.<OpenQuestUI>c__AnonStoreyB <OpenQuestUI>c__AnonStoreyB = new CMD_BattleNextChoice.<OpenQuestUI>c__AnonStoreyA.<OpenQuestUI>c__AnonStoreyB();
				<OpenQuestUI>c__AnonStoreyB.<>f__ref$10 = <OpenQuestUI>c__AnonStoreyA;
				RestrictionInput.EndLoad();
				ClassSingleton<QuestTOPAccessor>.Instance.nextDungeon = <OpenQuestUI>c__AnonStoreyA.masterNextDangeon;
				ClassSingleton<QuestTOPAccessor>.Instance.nextStage = <OpenQuestUI>c__AnonStoreyA.nextStage;
				CMD_BattleNextChoice.<OpenQuestUI>c__AnonStoreyA.<OpenQuestUI>c__AnonStoreyB <OpenQuestUI>c__AnonStoreyB2 = <OpenQuestUI>c__AnonStoreyB;
				if (CMD_BattleNextChoice.<>f__mg$cache0 == null)
				{
					CMD_BattleNextChoice.<>f__mg$cache0 = new Action<int>(CMD_BattleNextChoice.OnCloseQuestTOP);
				}
				<OpenQuestUI>c__AnonStoreyB2.cmd = (GUIMain.ShowCommonDialog(CMD_BattleNextChoice.<>f__mg$cache0, "CMD_QuestTOP", null) as CMD);
				PartsTitleBase partsTitle = <OpenQuestUI>c__AnonStoreyB.cmd.PartsTitle;
				if (null != partsTitle)
				{
					partsTitle.SetReturnAct(delegate(int idx)
					{
						<OpenQuestUI>c__AnonStoreyB.cmd.SetCloseAction(delegate(int i)
						{
							if (CMD_BattleNextChoice.<>f__mg$cache1 == null)
							{
								CMD_BattleNextChoice.<>f__mg$cache1 = new Action<int>(CMD_BattleNextChoice.OnCloseQuestTOP);
							}
							CMD cmd = GUIMain.ShowCommonDialog(CMD_BattleNextChoice.<>f__mg$cache1, "CMD_QuestSelect", null) as CMD;
							cmd.SetForceReturnValue(1);
						});
						<OpenQuestUI>c__AnonStoreyB.cmd.ClosePanel(true);
					});
				}
			});
			base.ClosePanel(true);
		}
		else
		{
			RestrictionInput.EndLoad();
		}
	}

	public static void OnCloseQuestTOP(int selectButtonIndex)
	{
		if (selectButtonIndex != 0)
		{
			CMD_BattleNextChoice.GoToFarm();
		}
	}

	public static void GoToFarm()
	{
		ClassSingleton<PlayLimit>.Instance.ClearTicketNumCont();
		ClassSingleton<PlayLimit>.Instance.ClearPlayLimitNumCont();
		if (CMD_BattleNextChoice.isGoToFarmCalled)
		{
			return;
		}
		CMD_BattleNextChoice.isGoToFarmCalled = true;
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
		BattleNextBattleOption.ClearBattleMenuSettings();
		DataMng.Instance().GetResultUtilData().ClearLastDngReq();
		ScreenController.ChangeHomeScreen(CMD_Tips.DISPLAY_PLACE.BattleToFarm);
	}

	private int[] GetAliveMonsterList()
	{
		int num = int.Parse(DataMng.Instance().RespDataMN_DeckList.selectDeckNum) - 1;
		GameWebAPI.RespDataMN_GetDeckList.DeckList deckList = DataMng.Instance().RespDataMN_DeckList.deckList[num];
		List<int> list = new List<int>();
		List<MonsterData> list2 = new List<MonsterData>();
		int[] aliveInfo = DataMng.Instance().WD_ReqDngResult.aliveInfo;
		for (int i = 0; i < aliveInfo.Length; i++)
		{
			if (aliveInfo[i] == 1 && deckList.monsterList.Length > i)
			{
				string userMonsterId = deckList.monsterList[i].userMonsterId;
				MonsterData monsterDataByUserMonsterID = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(userMonsterId, false);
				list2.Add(monsterDataByUserMonsterID);
				list.Add(int.Parse(userMonsterId));
			}
		}
		return list.ToArray();
	}

	private APIRequestTask RequestUserMonsterData(int[] userMonsterIds, bool requestRetry = true)
	{
		GameWebAPI.RequestMonsterList requestMonsterList = new GameWebAPI.RequestMonsterList();
		requestMonsterList.SetSendData = delegate(GameWebAPI.ReqDataUS_GetMonsterList param)
		{
			param.userMonsterIds = userMonsterIds;
		};
		requestMonsterList.OnReceived = delegate(GameWebAPI.RespDataUS_GetMonsterList response)
		{
			ClassSingleton<MonsterUserDataMng>.Instance.UpdateUserMonsterData(response.userMonsterList);
		};
		GameWebAPI.RequestMonsterList request = requestMonsterList;
		return new APIRequestTask(request, requestRetry);
	}

	private bool IsLockNextDungeon(int ClearDungeonID)
	{
		if (3000 < ClearDungeonID)
		{
			return true;
		}
		int num = ClearDungeonID + 1;
		GameWebAPI.RespDataWD_GetDungeonInfo dngeonInfoByWorldAreaId = ClassSingleton<QuestData>.Instance.GetDngeonInfoByWorldAreaId("1");
		int num2 = 1;
		for (int i = 0; i < dngeonInfoByWorldAreaId.worldDungeonInfo.Length; i++)
		{
			if (num > num2 + dngeonInfoByWorldAreaId.worldDungeonInfo[i].dungeons.Length)
			{
				num2 += dngeonInfoByWorldAreaId.worldDungeonInfo[i].dungeons.Length;
			}
			else
			{
				for (int j = 0; j < dngeonInfoByWorldAreaId.worldDungeonInfo[i].dungeons.Length; j++)
				{
					if (num == num2)
					{
						return dngeonInfoByWorldAreaId.worldDungeonInfo[i].dungeons[j].status > 1;
					}
					num2++;
				}
			}
		}
		return false;
	}

	private bool CanPlayDungeonOver()
	{
		bool flag = Singleton<UserDataMng>.Instance.IsOverUnitLimit(ClassSingleton<MonsterUserDataMng>.Instance.GetMonsterNum() + ConstValue.ENABLE_SPACE_TOEXEC_DUNGEON);
		bool flag2 = Singleton<UserDataMng>.Instance.IsOverChipLimit(ConstValue.ENABLE_SPACE_TOEXEC_DUNGEON);
		if (!flag && !flag2)
		{
			return true;
		}
		string info = string.Empty;
		if (flag && flag2)
		{
			info = StringMaster.GetString("ResultUnitAndChipLimitOver");
		}
		else if (flag)
		{
			info = StringMaster.GetString("ResultUnitLimitOver");
		}
		else if (flag2)
		{
			info = StringMaster.GetString("ResultChipLimitOver");
		}
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int result)
		{
			this.OnPushedFarmButton();
		}, "CMD_ModalMessage", null) as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("Present-08");
		cmd_ModalMessage.Info = info;
		cmd_ModalMessage.BtnText = StringMaster.GetString("GoToFarm");
		return false;
	}

	protected override void Update()
	{
		base.Update();
		this.UpdateAndroidBackKey();
	}

	private void UpdateAndroidBackKey()
	{
		if (GUIManager.ExtBackKeyReady && !this.pushedBackKey && Input.GetKeyDown(KeyCode.Escape))
		{
			CommonDialog topDialog = GUIManager.GetTopDialog(null, false);
			if (null != topDialog && topDialog.gameObject.name == base.gameObject.name && !this.permanentMode && base.GetActionStatus() == CommonDialog.ACT_STATUS.OPEN && !GUICollider.IsAllColliderDisable())
			{
				this.pushedBackKey = true;
				this.OnPushedFarmButton();
			}
		}
	}
}
