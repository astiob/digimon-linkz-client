using Monster;
using MultiBattle.Tools;
using PvP;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WebAPIRequest;

namespace Colosseum.Matching
{
	public sealed class ColosseumMatchingEventListener : MonoBehaviour
	{
		private CMD_ColosseumMatching uiRoot;

		private ColosseumMatchingNetwork network;

		private ColosseumMatchingAnimation anime;

		private Coroutine animeRoutine;

		private MatchingConfig matchingConfig;

		private MatchingResult matchingResult;

		private ColosseumMatchingNetworkSynchronize syncData;

		private Coroutine httpRequestRoutine;

		private bool isResume;

		private GameWebAPI.Common_MonsterData[] GetMyMonsterNetworkData()
		{
			global::Debug.Log("GetMyMonsterNetworkData");
			List<MonsterData> colosseumDeckUserMonsterList = ClassSingleton<MonsterUserDataMng>.Instance.GetColosseumDeckUserMonsterList();
			GameWebAPI.Common_MonsterData[] array = new GameWebAPI.Common_MonsterData[colosseumDeckUserMonsterList.Count];
			for (int i = 0; i < colosseumDeckUserMonsterList.Count; i++)
			{
				array[i] = new GameWebAPI.Common_MonsterData(colosseumDeckUserMonsterList[i]);
			}
			return array;
		}

		public void SetInstance(CMD_ColosseumMatching parent, MatchingConfig config, IColosseumMatchingInfo info, string dungeonId, ColosseumMatchingAnimation modelAnimation)
		{
			this.uiRoot = parent;
			this.matchingConfig = config;
			this.network = new ColosseumMatchingNetwork(info, this);
			this.anime = modelAnimation;
			this.matchingResult = new MatchingResult
			{
				dungeonId = dungeonId
			};
		}

		public void InitializeMatching()
		{
			global::Debug.Log("InitializeMatching");
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
			this.matchingResult.myData = new MultiBattleData.PvPUserData();
			this.httpRequestRoutine = base.StartCoroutine(this.network.ReadyMatching());
		}

		public void OnFinishReadyMatching(bool isSuccess, GameWebAPI.ColosseumUserStatus userStatus)
		{
			global::Debug.Log("OnFinishReadyMatching");
			if (!isSuccess)
			{
				RestrictionInput.EndLoad();
				this.uiRoot.SetErrorData("AlertNetworkErrorTitle", "AlertNetworkErrorInfo");
				this.uiRoot.ClosePanel(true);
			}
			else
			{
				if (userStatus != null)
				{
					this.matchingResult.myData.userStatus = userStatus;
				}
				else if (this.matchingResult.myData.userStatus == null)
				{
					RestrictionInput.EndLoad();
					this.uiRoot.SetErrorData("AlertNetworkErrorTitle", "AlertNetworkErrorInfo");
					this.uiRoot.ClosePanel(true);
				}
				this.network.StartSocketNetwork();
			}
		}

		public void OnConnected(bool isSuccess)
		{
			global::Debug.Log("OnConnected");
			if (!isSuccess)
			{
				RestrictionInput.EndLoad();
				this.uiRoot.SetErrorData("AlertNetworkErrorTitle", "AlertNetworkErrorInfo");
				this.uiRoot.ClosePanel(true);
			}
			else
			{
				if (this.anime.GetShowMonsterCount() == 0)
				{
					List<MonsterData> colosseumDeckUserMonsterList = ClassSingleton<MonsterUserDataMng>.Instance.GetColosseumDeckUserMonsterList();
					MonsterData[] array = new MonsterData[3];
					for (int i = 0; i < array.Length; i++)
					{
						int index = UnityEngine.Random.Range(0, colosseumDeckUserMonsterList.Count);
						MonsterData monsterData = colosseumDeckUserMonsterList[index];
						colosseumDeckUserMonsterList.Remove(monsterData);
						array[i] = monsterData;
					}
					this.anime.ShowMonster(array);
				}
				this.network.UpdateOnlineStatus();
			}
		}

		public void OnConnetionClosed()
		{
			global::Debug.Log("OnConnetionClosed");
			RestrictionInput.EndLoad();
			if (this.httpRequestRoutine != null)
			{
				base.StopCoroutine(this.httpRequestRoutine);
				this.httpRequestRoutine = null;
			}
			if (this.animeRoutine != null)
			{
				base.StopCoroutine(this.animeRoutine);
				this.animeRoutine = null;
			}
			this.uiRoot.ClosePanel(true);
		}

		public void OnExceptionSocket(short errorCode, string errorMessage)
		{
			global::Debug.Log("OnExceptionSocket");
			RestrictionInput.EndLoad();
			if (this.syncData != null)
			{
				this.syncData.StopSync();
				this.syncData = null;
			}
			if (this.httpRequestRoutine != null)
			{
				base.StopCoroutine(this.httpRequestRoutine);
				this.httpRequestRoutine = null;
			}
			if (this.animeRoutine != null)
			{
				base.StopCoroutine(this.animeRoutine);
				this.animeRoutine = null;
			}
			if (errorCode != 741)
			{
				this.uiRoot.SetErrorData("MultiRecruit-13", "ColosseumNetworkError");
				this.uiRoot.ClosePanel(true);
			}
		}

		public void OnExceptionSocketRequest(string errorCode, string errorMessage)
		{
			global::Debug.Log("OnExceptionSocketRequest");
			RestrictionInput.EndLoad();
			if (this.syncData != null)
			{
				this.syncData.StopSync();
				this.syncData = null;
			}
			if (this.httpRequestRoutine != null)
			{
				base.StopCoroutine(this.httpRequestRoutine);
				this.httpRequestRoutine = null;
			}
			if (this.animeRoutine != null)
			{
				base.StopCoroutine(this.animeRoutine);
				this.animeRoutine = null;
			}
			this.uiRoot.SetErrorData("MultiRecruit-13", "ColosseumNetworkError");
			this.network.CloseSocketNetwork();
		}

		public void OnResumeApplication()
		{
			global::Debug.Log("OnResumeApplication");
			this.isResume = true;
			if (!Singleton<TCPUtil>.Instance.CheckTCPConnection())
			{
				RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
				this.httpRequestRoutine = base.StartCoroutine(this.network.ReadyMatching());
			}
			else
			{
				RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
				this.network.UpdateOnlineStatus();
			}
		}

		public void OnReceivedUpdateOnlineStatusResponse()
		{
			global::Debug.Log("OnReceivedUpdateOnlineStatusResponse");
			if (!this.isResume)
			{
				this.network.StartMatching();
			}
			else
			{
				this.network.ResumeMatching();
				this.isResume = false;
			}
		}

		public void OnBecameOwner()
		{
			global::Debug.Log("OnBecameOwner");
			this.matchingResult.myData.isOwner = true;
			RestrictionInput.EndLoad();
			this.uiRoot.EnableCancelButton(true);
		}

		public void OnCancelMatching()
		{
			global::Debug.Log("OnCancelMatching");
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
			if (this.syncData != null)
			{
				this.syncData.StopSync();
				this.syncData = null;
			}
			this.network.StopMatching();
		}

		public void OnStopMatching()
		{
			global::Debug.Log("OnStopMatching");
			this.network.CloseSocketNetwork();
		}

		public void OnDecidedOpponent()
		{
			global::Debug.Log("OnDecidedOpponent");
			this.uiRoot.EnableCancelButton(false);
			this.httpRequestRoutine = base.StartCoroutine(this.network.GetOpponentColosseumUserStatus());
		}

		public void OnDeleteMathing()
		{
			global::Debug.Log("OnDeleteMathing");
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
			if (this.syncData != null)
			{
				this.syncData.StopSync();
				this.syncData = null;
			}
			if (this.httpRequestRoutine != null)
			{
				base.StopCoroutine(this.httpRequestRoutine);
				this.httpRequestRoutine = null;
			}
			if (this.animeRoutine != null)
			{
				base.StopCoroutine(this.animeRoutine);
				this.animeRoutine = null;
			}
			base.StartCoroutine(this.network.SendLoseBattle());
		}

		public void OnReceiveLoseBattleRequest(GameWebAPI.RespData_ColosseumBattleEndLogic response)
		{
			global::Debug.Log("OnReceiveLoseBattleRequest");
			this.uiRoot.SetErrorData("AlertNetworkErrorTitle", "ColosseumNetworkError");
			this.network.CloseSocketNetwork();
		}

		public void OnErrorMatchingResponse(string title, string info)
		{
			global::Debug.Log("OnErrorMatchingResponse");
			this.uiRoot.SetErrorData(title, info);
			this.network.CloseSocketNetwork();
		}

		public void OnExceptionMatchingResponse(string errorCode)
		{
			global::Debug.Log("OnExceptionMatchingResponse");
			if ("E-PV99" == errorCode)
			{
				base.StartCoroutine(this.network.SendLoseBattle());
			}
			else
			{
				this.uiRoot.SetAlertData(errorCode);
				this.network.CloseSocketNetwork();
			}
		}

		public void OnResumeMatchingSuccess()
		{
			global::Debug.Log("OnResumeMatchingSuccess");
			RestrictionInput.EndLoad();
			this.uiRoot.EnableCancelButton(true);
		}

		public void OnResumeSelectMonsterSuccess()
		{
			global::Debug.Log("OnResumeSelectMonsterSuccess");
			RestrictionInput.EndLoad();
		}

		public void OnReceivedOpponentColosseumUserStatus(GameWebAPI.RespData_ColosseumUserStatusLogic opponentStatus)
		{
			global::Debug.Log("OnReceivedOpponentColosseumUserStatus");
			PvPUtility.SetPvPTopNoticeCode(opponentStatus);
			if (opponentStatus.GetResultCodeEnum != GameWebAPI.RespData_ColosseumUserStatusLogic.ResultCode.SUCCESS)
			{
				this.network.StopMatching();
			}
			else
			{
				this.matchingResult.opponentData = new MultiBattleData.PvPUserData
				{
					monsterData = new GameWebAPI.Common_MonsterData[6],
					userStatus = opponentStatus.userStatus
				};
				this.httpRequestRoutine = base.StartCoroutine(this.network.GetOpponentColosseumDeck());
			}
		}

		public void OnReceivedOpponentColosseumDeck(GameWebAPI.RespData_ColosseumDeckInfoLogic opponentDeck)
		{
			global::Debug.Log("OnReceivedOpponentColosseumDeck");
			if (opponentDeck.GetResultCodeEnum != GameWebAPI.RespData_ColosseumDeckInfoLogic.ResultCode.SUCCESS)
			{
				this.network.StopMatching();
			}
			else
			{
				this.matchingResult.opponentData.monsterData = opponentDeck.partyMonsters;
				this.matchingResult.opponentMonsterIconDataList = new List<MonsterData>();
				int[] array = new int[opponentDeck.partyMonsters.Length];
				for (int i = 0; i < opponentDeck.partyMonsters.Length; i++)
				{
					int num = int.Parse(opponentDeck.partyMonsters[i].userMonsterId);
					array[i] = num;
					MonsterData item = MonsterDataMng.Instance().CreateMonsterDataByMID(opponentDeck.partyMonsters[i].monsterId);
					this.matchingResult.opponentMonsterIconDataList.Add(item);
				}
				this.httpRequestRoutine = base.StartCoroutine(this.network.GetOpponentMonsterChipSlot(array));
			}
		}

		public void OnReceivedOpponentMonsterChipSlot(GameWebAPI.RespDataCS_MonsterSlotInfoListLogic slotInfoList)
		{
			global::Debug.Log("OnReceivedOpponentMonsterChipSlot");
			List<GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip> list = null;
			if (slotInfoList != null && slotInfoList.slotInfo != null)
			{
				list = new List<GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip>();
				for (int i = 0; i < slotInfoList.slotInfo.Length; i++)
				{
					if (slotInfoList.slotInfo[i] != null && slotInfoList.slotInfo[i].equip != null)
					{
						list.AddRange(slotInfoList.slotInfo[i].equip);
					}
				}
			}
			if (list != null && 0 < list.Count)
			{
				int[] array = new int[list.Count];
				for (int j = 0; j < list.Count; j++)
				{
					array[j] = list[j].userChipId;
				}
				this.httpRequestRoutine = base.StartCoroutine(this.network.GetOpponentUserChipInfo(array));
			}
			else
			{
				this.matchingResult.myData.monsterData = this.GetMyMonsterNetworkData();
				this.network.StartBattle();
			}
		}

		public void OnReceivedOpponentUserChipInfo(GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] userChipList)
		{
			global::Debug.Log("OnReceivedOpponentUserChipInfo");
			for (int i = 0; i < this.matchingResult.opponentData.monsterData.Length; i++)
			{
				if (userChipList != null && 0 < userChipList.Length)
				{
					int id = int.Parse(this.matchingResult.opponentData.monsterData[i].userMonsterId);
					int[] array = userChipList.Where((GameWebAPI.RespDataCS_ChipListLogic.UserChipList x) => x.userMonsterId == id).Select((GameWebAPI.RespDataCS_ChipListLogic.UserChipList x) => x.chipId).ToArray<int>();
					if (0 < array.Length)
					{
						this.matchingResult.opponentData.monsterData[i].chipIdList = array;
					}
				}
			}
			this.matchingResult.myData.monsterData = this.GetMyMonsterNetworkData();
			this.network.StartBattle();
		}

		public void OnReceivedMyStartBattleResponse()
		{
			global::Debug.Log("OnReceivedMyStartBattleResponse");
			RestrictionInput.EndLoad();
			Singleton<UserDataMng>.Instance.ConsumeUserStamina(this.matchingConfig.staminaCost);
			this.animeRoutine = base.StartCoroutine(this.anime.StartEffect());
		}

		public void OnReceivedOpponentStartBattleResponse()
		{
			global::Debug.Log("OnReceivedOpponentStartBattleResponse");
		}

		public void OnExceptionStartBattleResponse(int errorCode)
		{
			global::Debug.Log("OnExceptionStartBattleResponse");
			if (this.syncData != null)
			{
				this.syncData.StopSync();
				this.syncData = null;
			}
			this.uiRoot.SetAlertData(errorCode.ToString());
			this.network.CloseSocketNetwork();
		}

		public void OnCompletedMatchingAnimation()
		{
			global::Debug.Log("OnCompletedMatchingAnimation");
			this.animeRoutine = null;
			PvPVersusInfo6Icon versusUI = this.uiRoot.CreateVersusInfo();
			versusUI.SetTitle(this.matchingConfig.isMockBattle);
			versusUI.SetUserInfo(this.matchingResult.myData, this.matchingResult.opponentData);
			versusUI.SetActionAnimationEnd(delegate
			{
				this.anime.DeleteTransferEffect();
				versusUI.gameObject.SetActive(false);
				this.uiRoot.HideCancelButton();
				PVPPartySelect3 monsterSelectUI = this.uiRoot.GetMonsterSelectUI();
				monsterSelectUI.Init();
				monsterSelectUI.SetData(this);
				monsterSelectUI.SetMonsterData(ClassSingleton<MonsterUserDataMng>.Instance.GetColosseumDeckUserMonsterList(), this.matchingResult.opponentMonsterIconDataList);
				monsterSelectUI.gameObject.SetActive(true);
			});
		}

		public void OnOpponentLeave()
		{
			global::Debug.Log("OnOpponentLeave");
			this.matchingResult.isOpponentLeaveRoom = true;
			GameWebAPI.Common_MonsterData[] array = new GameWebAPI.Common_MonsterData[3];
			int[] array2 = new int[3];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.matchingResult.opponentData.monsterData[i];
				array2[i] = i;
			}
			this.matchingResult.opponentData.monsterData = array;
			this.matchingResult.opponentSelectMonsterIndexList = array2;
			this.network.SaveMatchingFinish(this.matchingResult.mySelectMonsterIndexList, this.matchingResult.opponentSelectMonsterIndexList);
		}

		public void OnOpponentOnline()
		{
			global::Debug.Log("OnOpponentOnline");
			if (this.syncData != null)
			{
				this.syncData.SynchronizeRoutine = base.StartCoroutine(this.syncData.Synchronize());
			}
		}

		public void OnSelectedMonster(int[] indexList)
		{
			global::Debug.Log("OnSelectedMonster");
			List<MonsterData> colosseumDeckUserMonsterList = ClassSingleton<MonsterUserDataMng>.Instance.GetColosseumDeckUserMonsterList();
			ColosseumData.LastUseMonsterList = new MonsterData[indexList.Length];
			GameWebAPI.Common_MonsterData[] array = new GameWebAPI.Common_MonsterData[indexList.Length];
			for (int i = 0; i < indexList.Length; i++)
			{
				MonsterData monsterData = colosseumDeckUserMonsterList[indexList[i]];
				ColosseumData.LastUseMonsterList[i] = monsterData;
				array[i] = new GameWebAPI.Common_MonsterData(monsterData);
			}
			this.matchingResult.myData.monsterData = array;
			this.matchingResult.mySelectMonsterIndexList = indexList;
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
			this.syncData = this.network.GetSelectMonsterSync(this.matchingResult.OpponentUserId, indexList);
			this.syncData.SynchronizeRoutine = base.StartCoroutine(this.syncData.Synchronize());
		}

		public void OnFailedSelectMonsterSend()
		{
			global::Debug.Log("OnFailedSelectMonsterSend");
			this.network.CheckOpponentOnlineStatus();
		}

		public void OnResumeOpponent()
		{
			global::Debug.Log("OnResumeOpponent");
			if (this.matchingResult.mySelectMonsterIndexList != null)
			{
				this.network.SendSelectMonster(this.matchingResult.OpponentUserId, this.matchingResult.mySelectMonsterIndexList);
			}
		}

		public void OnReceivedSelectMonster(int[] opponentSelectMonsterIndexList)
		{
			global::Debug.Log("OnReceivedSelectMonster");
			if (this.matchingResult.opponentData == null)
			{
				return;
			}
			if (!this.matchingResult.isOpponentSelectedMonster)
			{
				this.matchingResult.isOpponentSelectedMonster = true;
				GameWebAPI.Common_MonsterData[] array = new GameWebAPI.Common_MonsterData[opponentSelectMonsterIndexList.Length];
				for (int i = 0; i < opponentSelectMonsterIndexList.Length; i++)
				{
					int num = opponentSelectMonsterIndexList[i];
					array[i] = this.matchingResult.opponentData.monsterData[num];
				}
				this.matchingResult.opponentData.monsterData = array;
				this.matchingResult.opponentSelectMonsterIndexList = opponentSelectMonsterIndexList;
				if (this.matchingResult.myData.isOwner)
				{
					this.matchingResult.randomSeed = ((int)DateTime.Now.Ticks & 65535);
				}
			}
			this.network.SendAckReceivedSelectMonsterIndex(this.matchingResult.OpponentUserId, this.matchingResult.randomSeed);
			if (this.matchingResult.isMySelectedMonster)
			{
				this.network.SaveMatchingFinish(this.matchingResult.mySelectMonsterIndexList, this.matchingResult.opponentSelectMonsterIndexList);
			}
		}

		public void OnAckReceivedSelectMonster(int randomSeed)
		{
			global::Debug.Log("OnAckReceivedSelectMonster");
			if (this.matchingResult.opponentData == null)
			{
				return;
			}
			this.syncData.StopSync();
			this.syncData = null;
			if (!this.matchingResult.isMySelectedMonster)
			{
				this.matchingResult.isMySelectedMonster = true;
				if (!this.matchingResult.myData.isOwner)
				{
					this.matchingResult.randomSeed = randomSeed;
				}
				if (this.matchingResult.isOpponentSelectedMonster)
				{
					this.network.SaveMatchingFinish(this.matchingResult.mySelectMonsterIndexList, this.matchingResult.opponentSelectMonsterIndexList);
				}
			}
		}

		public void OnMonsterSelectTimeOver()
		{
			global::Debug.Log("OnMonsterSelectTimeOver");
			if (this.syncData == null)
			{
				this.syncData = new ColosseumMatchingNetworkSynchronize();
				this.syncData.SetFailedAction(new Action(this.OnFailedSelectMonsterSend));
				this.syncData.SetWaitReceive(8f);
				this.syncData.SynchronizeRoutine = base.StartCoroutine(this.syncData.Synchronize());
			}
		}

		public void OnReadyBattleCompleted()
		{
			global::Debug.Log("OnReadyBattleCompleted");
			if (this.syncData != null)
			{
				this.syncData.StopSync();
				this.syncData = null;
			}
			if (!this.matchingResult.isOpponentLeaveRoom)
			{
				this.syncData = this.network.GetOpponentReadyStateSync(delegate(RequestBase request)
				{
					this.httpRequestRoutine = base.StartCoroutine(this.network.RequestOpponentReadyCheck(request));
					return this.httpRequestRoutine;
				});
				this.syncData.SynchronizeRoutine = base.StartCoroutine(this.syncData.Synchronize());
			}
			else
			{
				this.OnSyncMatchingFinish();
			}
		}

		public void OnFailedSyncMatchingFinish()
		{
			global::Debug.Log("OnFailedSyncMatchingFinish");
			this.network.CheckOpponentOnlineStatus();
		}

		public void OnSyncMatchingFinish()
		{
			global::Debug.Log("OnSyncMatchingFinish");
			Singleton<TCPUtil>.Instance.ResetAllCallBackMethod();
			if (this.syncData != null)
			{
				this.syncData.StopSync();
				this.syncData = null;
			}
			ClassSingleton<MultiBattleData>.Instance.MaxAttackTime = ConstValue.PVP_MAX_ATTACK_TIME;
			ClassSingleton<MultiBattleData>.Instance.HurryUpAttackTime = ConstValue.PVP_HURRYUP_ATTACK_TIME;
			ClassSingleton<MultiBattleData>.Instance.MaxRoundNum = ConstValue.PVP_MAX_ROUND_NUM;
			ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId = DataMng.Instance().UserId;
			ClassSingleton<MultiBattleData>.Instance.PvPUserDatas = new MultiBattleData.PvPUserData[]
			{
				this.matchingResult.myData,
				this.matchingResult.opponentData
			};
			ClassSingleton<MultiBattleData>.Instance.PvPField = new MultiBattleData.PvPFieldData
			{
				worldDungeonId = this.matchingResult.dungeonId
			};
			ClassSingleton<MultiBattleData>.Instance.RandomSeed = this.matchingResult.randomSeed;
			this.uiRoot.ChangeBattleScene();
		}
	}
}
