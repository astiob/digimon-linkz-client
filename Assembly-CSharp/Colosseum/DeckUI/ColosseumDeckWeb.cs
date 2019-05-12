using Monster;
using System;
using System.Collections.Generic;

namespace Colosseum.DeckUI
{
	public static class ColosseumDeckWeb
	{
		private static void OnResponseDeck(GameWebAPI.RespData_ColosseumDeckInfoLogic response)
		{
			if (response.partyMonsters != null)
			{
				List<string> list = new List<string>();
				for (int i = 0; i < response.partyMonsters.Length; i++)
				{
					list.Add(response.partyMonsters[i].userMonsterId);
				}
				ClassSingleton<MonsterUserDataMng>.Instance.SetColosseumDeckUserMonster(list.ToArray());
			}
		}

		public static APIRequestTask RequestDeck()
		{
			GameWebAPI.ColosseumDeckInfoLogic colosseumDeckInfoLogic = new GameWebAPI.ColosseumDeckInfoLogic();
			colosseumDeckInfoLogic.SetSendData = delegate(GameWebAPI.ReqData_ColosseumDeckInfoLogic param)
			{
				param.target = "me";
			};
			colosseumDeckInfoLogic.OnReceived = new Action<GameWebAPI.RespData_ColosseumDeckInfoLogic>(ColosseumDeckWeb.OnResponseDeck);
			GameWebAPI.ColosseumDeckInfoLogic request = colosseumDeckInfoLogic;
			return new APIRequestTask(request, false);
		}

		public static GameWebAPI.ColosseumDeckInfoLogic Request()
		{
			GameWebAPI.ColosseumDeckInfoLogic colosseumDeckInfoLogic = new GameWebAPI.ColosseumDeckInfoLogic();
			colosseumDeckInfoLogic.SetSendData = delegate(GameWebAPI.ReqData_ColosseumDeckInfoLogic param)
			{
				param.target = "me";
			};
			colosseumDeckInfoLogic.OnReceived = new Action<GameWebAPI.RespData_ColosseumDeckInfoLogic>(ColosseumDeckWeb.OnResponseDeck);
			return colosseumDeckInfoLogic;
		}

		public static APIRequestTask RequestSave(string[] idList)
		{
			GameWebAPI.ColosseumDeckEditLogic request = new GameWebAPI.ColosseumDeckEditLogic
			{
				SetSendData = delegate(GameWebAPI.ReqData_ColosseumDeckEditLogic param)
				{
					param.userMonsterIdList = idList;
				},
				OnReceived = delegate(GameWebAPI.RespData_ColosseumDeckEditLogic response)
				{
					ClassSingleton<MonsterUserDataMng>.Instance.SetColosseumDeckUserMonster(idList);
				}
			};
			return new APIRequestTask(request, true);
		}
	}
}
