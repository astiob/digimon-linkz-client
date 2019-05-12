using Monster;
using System;
using System.Collections;

namespace Colosseum.Matching
{
	public sealed class ColosseumMatchingNetworkHTTP
	{
		public APIRequestTask RequestColosseumUserStatus(GameWebAPI.ColosseumUserStatusLogic request, Action<GameWebAPI.ColosseumUserStatus> onReceived)
		{
			request.OnReceived = delegate(GameWebAPI.RespData_ColosseumUserStatusLogic response)
			{
				onReceived(response.userStatus);
			};
			return new APIRequestTask(request, true);
		}

		public APIRequestTask RequestEntryColosseumDeck(Action<bool> onReceived)
		{
			GameWebAPI.ColosseumDeckEditLogic colosseumDeckEditLogic = new GameWebAPI.ColosseumDeckEditLogic();
			colosseumDeckEditLogic.SetSendData = delegate(GameWebAPI.ReqData_ColosseumDeckEditLogic param)
			{
				param.userMonsterIdList = ClassSingleton<MonsterUserDataMng>.Instance.GetColosseumDeckUserMonsterIdList();
			};
			colosseumDeckEditLogic.OnReceived = delegate(GameWebAPI.RespData_ColosseumDeckEditLogic response)
			{
				bool obj = 1 == response.resultCode;
				onReceived(obj);
			};
			GameWebAPI.ColosseumDeckEditLogic request = colosseumDeckEditLogic;
			return new APIRequestTask(request, false);
		}

		public IEnumerator RequestEndBattle(GameWebAPI.ColosseumBattleEndLogic request, Action<GameWebAPI.RespData_ColosseumBattleEndLogic> onCompleted)
		{
			GameWebAPI.RespData_ColosseumBattleEndLogic endBattleResponse = null;
			request.OnReceived = delegate(GameWebAPI.RespData_ColosseumBattleEndLogic response)
			{
				endBattleResponse = response;
			};
			return request.Run(delegate()
			{
				onCompleted(endBattleResponse);
			}, null, null);
		}

		public IEnumerator RequestOpponentColosseumUserStatus(GameWebAPI.ColosseumUserStatusLogic request, Action<GameWebAPI.RespData_ColosseumUserStatusLogic> onCompleted)
		{
			GameWebAPI.RespData_ColosseumUserStatusLogic opponentUserStatus = null;
			request.OnReceived = delegate(GameWebAPI.RespData_ColosseumUserStatusLogic response)
			{
				opponentUserStatus = response;
			};
			return request.Run(delegate()
			{
				onCompleted(opponentUserStatus);
			}, null, null);
		}

		public IEnumerator RequestOpponentColosseumDeck(GameWebAPI.ColosseumDeckInfoLogic request, Action<GameWebAPI.RespData_ColosseumDeckInfoLogic> onCompleted)
		{
			GameWebAPI.RespData_ColosseumDeckInfoLogic opponentDeck = null;
			request.OnReceived = delegate(GameWebAPI.RespData_ColosseumDeckInfoLogic response)
			{
				opponentDeck = response;
			};
			return request.Run(delegate()
			{
				onCompleted(opponentDeck);
			}, null, null);
		}

		public IEnumerator RequestOpponentMonsterChipSlot(GameWebAPI.MonsterSlotInfoListLogic request, Action<GameWebAPI.RespDataCS_MonsterSlotInfoListLogic> onCompleted)
		{
			GameWebAPI.RespDataCS_MonsterSlotInfoListLogic monsterSlotInfoList = null;
			request.OnReceived = delegate(GameWebAPI.RespDataCS_MonsterSlotInfoListLogic response)
			{
				monsterSlotInfoList = response;
			};
			return request.Run(delegate()
			{
				onCompleted(monsterSlotInfoList);
			}, null, null);
		}

		public IEnumerator RequestOpponentUserChipInfo(GameWebAPI.ReqDataCS_ChipListLogic request, Action<GameWebAPI.RespDataCS_ChipListLogic.UserChipList[]> onCompleted)
		{
			GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] chipList = null;
			request.OnReceived = delegate(GameWebAPI.RespDataCS_ChipListLogic response)
			{
				chipList = response.userChipList;
			};
			return request.Run(delegate()
			{
				onCompleted(chipList);
			}, null, null);
		}

		public IEnumerator RequestOpponentReadyState(GameWebAPI.ColosseumPrepareStatusLogic request, Action<GameWebAPI.RespData_ColosseumPrepareStatusLogic> onCompleted)
		{
			GameWebAPI.RespData_ColosseumPrepareStatusLogic opponentReadyState = null;
			request.OnReceived = delegate(GameWebAPI.RespData_ColosseumPrepareStatusLogic response)
			{
				opponentReadyState = response;
			};
			return request.Run(delegate()
			{
				onCompleted(opponentReadyState);
			}, null, null);
		}
	}
}
