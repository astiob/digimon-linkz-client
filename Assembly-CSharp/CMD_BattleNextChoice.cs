using Master;
using NextChoice;
using Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class CMD_BattleNextChoice : CMD
{
	[HideInInspector]
	public GUIScreenResult screenResult;

	[SerializeField]
	private GameObject[] singlePlayButtons;

	[SerializeField]
	private GameObject multiPlayButtons;

	[SerializeField]
	private UserStamina userStamina;

	[SerializeField]
	private UILabel staminaText;

	[SerializeField]
	private UILabel farmText;

	[SerializeField]
	private UILabel retryText;

	[SerializeField]
	private UILabel nextText;

	[SerializeField]
	private UILabel multiFarmText;

	[SerializeField]
	private UILabel multiNextText;

	[SerializeField]
	private UILabel battleOptionDescription;

	private GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList[] newMonsterParamList;

	[SerializeField]
	private BattleNextBattleOption battleOption;

	private static bool isGoToFarmCalled;

	private bool pushedBackKey;

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		CMD_BattleNextChoice.isGoToFarmCalled = false;
		if (ClassSingleton<QuestData>.Instance.RespData_WorldMultiResultInfoLogic != null)
		{
			foreach (GameObject gameObject in this.singlePlayButtons)
			{
				gameObject.SetActive(false);
			}
			this.multiPlayButtons.SetActive(true);
			aT = 0.1f;
			ClassSingleton<QuestData>.Instance.RespData_WorldMultiResultInfoLogic = null;
		}
		else if (!this.CanPushAgainButton())
		{
			this.singlePlayButtons[1].SetActive(false);
		}
		if (null != this.userStamina)
		{
			this.userStamina.RefreshParams();
		}
		if (this.staminaText != null)
		{
			this.staminaText.text = StringMaster.GetString("BattleResult-11");
		}
		if (this.farmText != null)
		{
			this.farmText.text = StringMaster.GetString("Mission-01");
		}
		if (this.retryText != null)
		{
			this.retryText.text = StringMaster.GetString("BattleResult-12");
		}
		if (this.nextText != null)
		{
			this.nextText.text = StringMaster.GetString("BattleResult-13");
		}
		if (this.multiFarmText != null)
		{
			this.multiFarmText.text = StringMaster.GetString("Mission-01");
		}
		if (this.multiNextText != null)
		{
			this.multiNextText.text = StringMaster.GetString("BattleResult-13");
		}
		if (this.battleOptionDescription != null)
		{
			this.battleOptionDescription.text = StringMaster.GetString("BattleResult-15");
		}
		base.Show(f, sizeX, sizeY, aT);
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
		return !(worldStageM2.worldAreaId == "8") || !string.IsNullOrEmpty(last_dng_req.userDungeonTicketId);
	}

	private void OnPushedAgainButton()
	{
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
					GUIScreenResult.instance.UsedCT_ForLimitQuest = 0;
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
		GameWebAPI.WD_Req_DngStart last_dng_req = DataMng.Instance().GetResultUtilData().last_dng_req;
		GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM[] worldDungeonM = MasterDataMng.Instance().RespDataMA_WorldDungeonM.worldDungeonM;
		int dungeonID = int.Parse(last_dng_req.dungeonId);
		GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM masterDungeon = worldDungeonM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM x) => x.worldDungeonId == dungeonID.ToString());
		GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM[] worldStageM = MasterDataMng.Instance().RespDataMA_WorldStageM.worldStageM;
		GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM worldStageM2 = worldStageM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM x) => x.worldStageId == masterDungeon.worldStageId);
		if (worldStageM2.worldAreaId == "8")
		{
			GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons dng = ClassSingleton<QuestData>.Instance.GetTicketQuestDungeonByTicketID(last_dng_req.userDungeonTicketId);
			int ticketNum = int.Parse(dng.dungeonTicketNum);
			ticketNum -= GUIScreenResult.instance.UsedCT_ForLimitQuest;
			if (ticketNum <= 0)
			{
				CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
				cmd_ModalMessage.Title = StringMaster.GetString("TicketQuestTitle");
				cmd_ModalMessage.Info = StringMaster.GetString("QuestPlayLimitZeroInfo");
			}
			else
			{
				CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(delegate(int idx)
				{
					if (idx == 0)
					{
						dng.dungeonTicketNum = ticketNum.ToString();
						NextChoiceReplay nextChoiceReplay2 = new NextChoiceReplay(this);
						nextChoiceReplay2.Start();
					}
				}, "CMD_Confirm") as CMD_Confirm;
				cmd_Confirm.Title = StringMaster.GetString("TicketQuestTitle");
				string info = string.Format(StringMaster.GetString("TicketQuestConfirmInfo"), worldStageM2.name, ticketNum, ticketNum - 1);
				cmd_Confirm.Info = info;
				cmd_Confirm.BtnTextYes = StringMaster.GetString("SystemButtonYes");
				cmd_Confirm.BtnTextNo = StringMaster.GetString("SystemButtonClose");
			}
		}
		else
		{
			List<QuestData.WorldDungeonData> wdd = ClassSingleton<QuestData>.Instance.GetWorldDungeonData_ByAreaIdStageId(worldStageM2.worldAreaId, masterDungeon.worldStageId, 0, false);
			int m;
			for (m = 0; m < wdd.Count; m++)
			{
				if (wdd[m].dungeon.worldDungeonId.ToString() == masterDungeon.worldDungeonId)
				{
					break;
				}
			}
			if (m < wdd.Count)
			{
				if (!ClassSingleton<PlayLimit>.Instance.PlayLimitCheck(wdd[m].dungeon, delegate(int idx)
				{
					if (idx == 0)
					{
						if (wdd[m].dungeon.playLimit.recoveryAssetCategoryId == 2)
						{
							CMD_Shop cmd = GUIMain.ShowCommonDialog(delegate(int _idx)
							{
							}, "CMD_Shop") as CMD_Shop;
							cmd.PartsTitle.SetReturnAct(delegate(int _i_)
							{
								cmd.ClosePanel(true);
							});
							cmd.PartsTitle.SetCloseAct(delegate(int _i_)
							{
								cmd.ClosePanel(true);
							});
						}
						else if (wdd[m].dungeon.playLimit.recoveryAssetCategoryId == 6)
						{
							GameWebAPI.RespDataMA_GetItemM.ItemM itemM = MasterDataMng.Instance().RespDataMA_ItemM.GetItemM(wdd[m].dungeon.playLimit.recoveryAssetValue.ToString());
							CMD_ModalMessage cmd_ModalMessage3 = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
							cmd_ModalMessage3.Title = string.Format(StringMaster.GetString("SystemShortage"), itemM.name);
							cmd_ModalMessage3.Info = string.Format(StringMaster.GetString("QuestPlayLimitItemShortInfo"), itemM.name);
						}
					}
				}, delegate(int _idx)
				{
					ClassSingleton<PlayLimit>.Instance.RecoverPlayLimit(wdd[m].dungeon, new Action<GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons>(this.OnSuccessedRecoverPlayLimit));
				}, GUIScreenResult.instance.UsedCT_ForLimitQuest))
				{
					return;
				}
				NextChoiceReplay nextChoiceReplay = new NextChoiceReplay(this);
				nextChoiceReplay.Start();
			}
			else
			{
				CMD_ModalMessage cmd_ModalMessage2 = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
				cmd_ModalMessage2.Title = StringMaster.GetString("QuestPlayLimitTitle");
				cmd_ModalMessage2.Info = StringMaster.GetString("QuestPlayLimitZeroInfo");
			}
		}
	}

	private void OnSuccessedRecoverPlayLimit(GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons dng)
	{
		GUIScreenResult.instance.UsedCT_ForLimitQuest = 0;
	}

	public void RequestDungeonStart()
	{
		int[] aliveMonsterList = this.GetAliveMonsterList();
		APIRequestTask apirequestTask = this.RequestUserMonsterData(aliveMonsterList, true);
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
			this.UpdateMonsterData();
			this.ReceivedDungeonStart();
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null), false);
	}

	private void ReceivedDungeonStart()
	{
		Loading.DisableMask();
		TipsLoading.Instance.StartTipsLoad(CMD_Tips.DISPLAY_PLACE.QuestToSoloBattle, true);
		CMD_BattleNextChoice component = GameObject.Find("CMD_BattleNextChoice").GetComponent<CMD_BattleNextChoice>();
		component.SetCloseAction(delegate(int noop)
		{
			DataMng.Instance().WD_ReqDngResult.clear = 0;
			BattleStateManager.StartSingle(0.5f, 0.5f, false, null);
		});
		component.ClosePanel(true);
	}

	private void OnPushedNextQuestButton()
	{
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
			this.UpdateMonsterData();
			this.RequestQuestData();
		}, null, null), false);
	}

	private void RequestQuestData()
	{
		MonsterDataMng.Instance().GetMonsterDataList(true);
		MonsterDataMng.Instance().InitMonsterGO();
		List<string> list = new List<string>();
		list.Add("1");
		list.Add("3");
		list.Add("8");
		ClassSingleton<QuestData>.Instance.GetWorldDungeonInfo(list, new Action<bool>(this.OpenQuestUI));
	}

	private void OpenMultiRecruitTop()
	{
		base.SetCloseAction(delegate(int x)
		{
			CommonDialog commonDialog = GUIMain.ShowCommonDialog(new Action<int>(CMD_BattleNextChoice.OnCloseQuestTOP), "CMD_MultiRecruitTop");
			commonDialog.SetForceReturnValue(1);
		});
		base.ClosePanel(true);
	}

	private void OpenQuestUI(bool isSuccess)
	{
		if (isSuccess)
		{
			GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM[] worldDungeonM = MasterDataMng.Instance().RespDataMA_WorldDungeonM.worldDungeonM;
			int nextDungeonID = int.Parse(this.screenResult.clearDungeonID);
			nextDungeonID++;
			GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM masterNextDangeon = worldDungeonM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM x) => x.worldDungeonId == nextDungeonID.ToString());
			if (masterNextDangeon == null)
			{
				nextDungeonID--;
				masterNextDangeon = worldDungeonM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM x) => x.worldDungeonId == nextDungeonID.ToString());
			}
			GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM[] worldStageM = MasterDataMng.Instance().RespDataMA_WorldStageM.worldStageM;
			GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM nextStage = worldStageM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM x) => x.worldStageId == masterNextDangeon.worldStageId);
			if (int.Parse(masterNextDangeon.worldDungeonId) > 3000)
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
				RestrictionInput.EndLoad();
				ClassSingleton<QuestTOPAccessor>.Instance.nextDungeon = masterNextDangeon;
				ClassSingleton<QuestTOPAccessor>.Instance.nextStage = nextStage;
				CMD cmd = GUIMain.ShowCommonDialog(new Action<int>(CMD_BattleNextChoice.OnCloseQuestTOP), "CMD_QuestTOP") as CMD;
				PartsTitleBase partsTitle = cmd.PartsTitle;
				if (partsTitle != null)
				{
					partsTitle.SetReturnAct(delegate(int idx)
					{
						cmd.SetCloseAction(delegate(int i)
						{
							CMD cmd = GUIMain.ShowCommonDialog(new Action<int>(CMD_BattleNextChoice.OnCloseQuestTOP), "CMD_QuestSelect") as CMD;
							cmd.SetForceReturnValue(1);
						});
						cmd.ClosePanel(true);
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
		if (CMD_BattleNextChoice.isGoToFarmCalled)
		{
			return;
		}
		CMD_BattleNextChoice.isGoToFarmCalled = true;
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
		BattleNextBattleOption.ClearBattleMenuSettings();
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

	private void UpdateMonsterData()
	{
		for (int i = 0; i < this.newMonsterParamList.Length; i++)
		{
			string userMonsterId = this.newMonsterParamList[i].userMonsterId;
			MonsterData monsterDataByUserMonsterID = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(userMonsterId, false);
			if (monsterDataByUserMonsterID != null)
			{
				monsterDataByUserMonsterID.DuplicateUserMonster(this.newMonsterParamList[i]);
			}
		}
	}

	private APIRequestTask RequestUserMonsterData(int[] userMonsterIds, bool requestRetry = true)
	{
		GameWebAPI.RequestMonsterList request = new GameWebAPI.RequestMonsterList
		{
			SetSendData = delegate(GameWebAPI.ReqDataUS_GetMonsterList param)
			{
				param.userMonsterIds = userMonsterIds;
			},
			OnReceived = delegate(GameWebAPI.RespDataUS_GetMonsterList response)
			{
				this.newMonsterParamList = response.userMonsterList;
				DataMng.Instance().SetUserMonsterList(response.userMonsterList);
			}
		};
		return new APIRequestTask(request, requestRetry);
	}

	private bool IsLockNextDungeon(int ClearDungeonID)
	{
		if (ClearDungeonID > 3000)
		{
			return true;
		}
		int num = ClearDungeonID + 1;
		GameWebAPI.RespDataWD_GetDungeonInfo dngeonInfoByWorldId = ClassSingleton<QuestData>.Instance.GetDngeonInfoByWorldId("1");
		int num2 = 1;
		for (int i = 0; i < dngeonInfoByWorldId.worldDungeonInfo.Length; i++)
		{
			if (num > num2 + dngeonInfoByWorldId.worldDungeonInfo[i].dungeons.Length)
			{
				num2 += dngeonInfoByWorldId.worldDungeonInfo[i].dungeons.Length;
			}
			else
			{
				for (int j = 0; j < dngeonInfoByWorldId.worldDungeonInfo[i].dungeons.Length; j++)
				{
					if (num == num2)
					{
						return dngeonInfoByWorldId.worldDungeonInfo[i].dungeons[j].status > 1;
					}
					num2++;
				}
			}
		}
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
			if (topDialog != null && topDialog.gameObject.name == base.gameObject.name && !this.permanentMode && base.GetActionStatus() == CommonDialog.ACT_STATUS.OPEN && !GUICollider.IsAllColliderDisable())
			{
				this.pushedBackKey = true;
				this.OnPushedFarmButton();
			}
		}
	}
}
