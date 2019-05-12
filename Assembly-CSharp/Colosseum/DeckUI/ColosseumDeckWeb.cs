using Monster;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using WebAPIRequest;

namespace Colosseum.DeckUI
{
	public static class ColosseumDeckWeb
	{
		[CompilerGenerated]
		private static Action<GameWebAPI.RespData_ColosseumDeckInfoLogic> <>f__mg$cache0;

		[CompilerGenerated]
		private static Action<GameWebAPI.RespData_ColosseumDeckInfoLogic> <>f__mg$cache1;

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
			RequestTypeBase<GameWebAPI.ReqData_ColosseumDeckInfoLogic, GameWebAPI.RespData_ColosseumDeckInfoLogic> requestTypeBase = colosseumDeckInfoLogic;
			if (ColosseumDeckWeb.<>f__mg$cache0 == null)
			{
				ColosseumDeckWeb.<>f__mg$cache0 = new Action<GameWebAPI.RespData_ColosseumDeckInfoLogic>(ColosseumDeckWeb.OnResponseDeck);
			}
			requestTypeBase.OnReceived = ColosseumDeckWeb.<>f__mg$cache0;
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
			RequestTypeBase<GameWebAPI.ReqData_ColosseumDeckInfoLogic, GameWebAPI.RespData_ColosseumDeckInfoLogic> requestTypeBase = colosseumDeckInfoLogic;
			if (ColosseumDeckWeb.<>f__mg$cache1 == null)
			{
				ColosseumDeckWeb.<>f__mg$cache1 = new Action<GameWebAPI.RespData_ColosseumDeckInfoLogic>(ColosseumDeckWeb.OnResponseDeck);
			}
			requestTypeBase.OnReceived = ColosseumDeckWeb.<>f__mg$cache1;
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
