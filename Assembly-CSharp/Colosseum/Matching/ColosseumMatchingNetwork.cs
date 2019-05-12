using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebAPIRequest;

namespace Colosseum.Matching
{
	public sealed class ColosseumMatchingNetwork
	{
		private IColosseumMatchingInfo matchingInfo;

		private ColosseumMatchingEventListener eventListener;

		private ColosseumMatchingNetworkHTTP httpAPI;

		private ColosseumMatchingNetworkSocket socketAPI;

		private List<int> addressUserIdList;

		public ColosseumMatchingNetwork(IColosseumMatchingInfo info, ColosseumMatchingEventListener listener)
		{
			this.matchingInfo = info;
			this.eventListener = listener;
			this.httpAPI = new ColosseumMatchingNetworkHTTP();
			this.socketAPI = new ColosseumMatchingNetworkSocket(listener);
			this.addressUserIdList = new List<int>();
		}

		public IEnumerator ReadyMatching()
		{
			bool result = false;
			GameWebAPI.ColosseumUserStatus userStatus = null;
			GameWebAPI.ColosseumUserStatusLogic colosseumUserStatusRequest = this.matchingInfo.GetColosseumUserStatusRequest();
			TaskBase task = this.httpAPI.RequestColosseumUserStatus(colosseumUserStatusRequest, delegate(GameWebAPI.ColosseumUserStatus r)
			{
				userStatus = r;
			}).Add(this.httpAPI.RequestEntryColosseumDeck(delegate(bool r)
			{
				result = r;
			}));
			return task.Run(delegate
			{
				this.eventListener.OnFinishReadyMatching(result, userStatus);
			}, delegate(Exception nop)
			{
				this.eventListener.OnFinishReadyMatching(false, null);
			}, null);
		}

		public void StartSocketNetwork()
		{
			Singleton<TCPUtil>.Instance.PrepareTCPServer(delegate(string noop)
			{
				Singleton<TCPUtil>.Instance.MakeTCPClient();
				Singleton<TCPUtil>.Instance.SetAfterConnectTCPMethod(new Action<bool>(this.eventListener.OnConnected));
				Singleton<TCPUtil>.Instance.SetOnExitCallBackMethod(new Action(this.eventListener.OnConnetionClosed));
				Singleton<TCPUtil>.Instance.SetExceptionMethod(new Action<short, string>(this.eventListener.OnExceptionSocket));
				Singleton<TCPUtil>.Instance.SetRequestExceptionMethod(new Action<string, string>(this.eventListener.OnExceptionSocketRequest));
				Singleton<TCPUtil>.Instance.SetTCPCallBackMethod(new Action<Dictionary<string, object>>(this.socketAPI.GetResponseData));
				Singleton<TCPUtil>.Instance.ConnectTCPServerAsync(DataMng.Instance().UserId);
			}, string.Empty);
		}

		public void CloseSocketNetwork()
		{
			Singleton<TCPUtil>.Instance.TCPDisConnect(false);
		}

		public void UpdateOnlineStatus()
		{
			this.socketAPI.UpdateOnlineStatus();
		}

		public void StartMatching()
		{
			this.socketAPI.StartMatching(this.matchingInfo);
		}

		public void StopMatching()
		{
			this.socketAPI.StopMatching(this.matchingInfo);
		}

		public void ResumeMatching()
		{
			this.socketAPI.ResumeMatching(this.matchingInfo);
		}

		public IEnumerator SendLoseBattle()
		{
			GameWebAPI.ColosseumBattleEndLogic loseBattleRequest = this.matchingInfo.GetLoseBattleRequest();
			return this.httpAPI.RequestEndBattle(loseBattleRequest, new Action<GameWebAPI.RespData_ColosseumBattleEndLogic>(this.eventListener.OnReceiveLoseBattleRequest));
		}

		public IEnumerator GetOpponentColosseumUserStatus()
		{
			GameWebAPI.ColosseumUserStatusLogic opponentColosseumUserStatusRequest = this.matchingInfo.GetOpponentColosseumUserStatusRequest();
			return this.httpAPI.RequestOpponentColosseumUserStatus(opponentColosseumUserStatusRequest, new Action<GameWebAPI.RespData_ColosseumUserStatusLogic>(this.eventListener.OnReceivedOpponentColosseumUserStatus));
		}

		public IEnumerator GetOpponentColosseumDeck()
		{
			GameWebAPI.ColosseumDeckInfoLogic opponentColosseumDeckRequest = this.matchingInfo.GetOpponentColosseumDeckRequest();
			return this.httpAPI.RequestOpponentColosseumDeck(opponentColosseumDeckRequest, new Action<GameWebAPI.RespData_ColosseumDeckInfoLogic>(this.eventListener.OnReceivedOpponentColosseumDeck));
		}

		public IEnumerator GetOpponentMonsterChipSlot(int[] userMonsterIdList)
		{
			GameWebAPI.MonsterSlotInfoListLogic request = new GameWebAPI.MonsterSlotInfoListLogic
			{
				SetSendData = delegate(GameWebAPI.ReqDataCS_MonsterSlotInfoListLogic param)
				{
					param.userMonsterId = userMonsterIdList;
				}
			};
			return this.httpAPI.RequestOpponentMonsterChipSlot(request, new Action<GameWebAPI.RespDataCS_MonsterSlotInfoListLogic>(this.eventListener.OnReceivedOpponentMonsterChipSlot));
		}

		public IEnumerator GetOpponentUserChipInfo(int[] userChipIdList)
		{
			GameWebAPI.ReqDataCS_ChipListLogic request = new GameWebAPI.ReqDataCS_ChipListLogic
			{
				SetSendData = delegate(GameWebAPI.SendDataCS_ChipListLogic param)
				{
					param.userChipId = userChipIdList;
				}
			};
			return this.httpAPI.RequestOpponentUserChipInfo(request, new Action<GameWebAPI.RespDataCS_ChipListLogic.UserChipList[]>(this.eventListener.OnReceivedOpponentUserChipInfo));
		}

		public void StartBattle()
		{
			this.socketAPI.StartBattle(this.matchingInfo);
		}

		public ColosseumMatchingNetworkSynchronize GetSelectMonsterSync(int opponentUserId, int[] selectMonsterIndexList)
		{
			this.addressUserIdList.Clear();
			this.addressUserIdList.Add(opponentUserId);
			return this.socketAPI.CreateSelectMonsterMessage(this.addressUserIdList, selectMonsterIndexList);
		}

		public void SendSelectMonster(int opponentUserId, int[] selectMonsterIndexList)
		{
			this.addressUserIdList.Clear();
			this.addressUserIdList.Add(opponentUserId);
			this.socketAPI.SendSelectMonsterIndexList(this.addressUserIdList, selectMonsterIndexList);
		}

		public void SendAckReceivedSelectMonsterIndex(int opponentUserId, int battleRandomSeed)
		{
			this.addressUserIdList.Clear();
			this.addressUserIdList.Add(opponentUserId);
			this.socketAPI.SendAckReceivedSelectMonsterIndex(this.addressUserIdList, battleRandomSeed);
		}

		public void SaveMatchingFinish(int[] mySelectMonster, int[] opponentSelectMonster)
		{
			this.socketAPI.SendMatchingFinish(mySelectMonster, opponentSelectMonster);
		}

		public ColosseumMatchingNetworkSynchronize GetOpponentReadyStateSync(Func<RequestBase, Coroutine> requestRoutine)
		{
			GameWebAPI.ColosseumPrepareStatusLogic opponentReadyCheckRequest = this.matchingInfo.GetOpponentReadyCheckRequest();
			ColosseumMatchingNetworkSynchronize colosseumMatchingNetworkSynchronize = new ColosseumMatchingNetworkSynchronize();
			colosseumMatchingNetworkSynchronize.SetIntervalAndTrialTime(5f, 15f);
			colosseumMatchingNetworkSynchronize.SetFailedAction(new Action(this.eventListener.OnFailedSyncMatchingFinish));
			colosseumMatchingNetworkSynchronize.SetRequestHTTP(opponentReadyCheckRequest, requestRoutine);
			return colosseumMatchingNetworkSynchronize;
		}

		public IEnumerator RequestOpponentReadyCheck(RequestBase request)
		{
			return this.httpAPI.RequestOpponentReadyState(request as GameWebAPI.ColosseumPrepareStatusLogic, delegate(GameWebAPI.RespData_ColosseumPrepareStatusLogic response)
			{
				switch (response.GetResultCodeEnum)
				{
				case GameWebAPI.RespData_ColosseumPrepareStatusLogic.PrepareStatusCode.READY:
				case GameWebAPI.RespData_ColosseumPrepareStatusLogic.PrepareStatusCode.CANT_START:
					this.eventListener.OnSyncMatchingFinish();
					break;
				}
			});
		}

		public void CheckOpponentOnlineStatus()
		{
			this.socketAPI.CheckOpponentOnlineStatus();
		}
	}
}
