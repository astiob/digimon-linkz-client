using System;
using System.Linq;

namespace Colosseum.Matching
{
	public sealed class ColosseumMatchingInfoMainBattle : IColosseumMatchingInfo
	{
		private MatchingConfig matchingConfig;

		public ColosseumMatchingInfoMainBattle(MatchingConfig config)
		{
			this.matchingConfig = config;
		}

		public string GetDungeonId()
		{
			string result = string.Empty;
			GameWebAPI.RespDataMA_ColosseumM.Colosseum[] colosseumM = MasterDataMng.Instance().RespDataMA_ColosseumMaster.colosseumM;
			GameWebAPI.RespDataMA_ColosseumM.Colosseum colosseum = colosseumM.SingleOrDefault(delegate(GameWebAPI.RespDataMA_ColosseumM.Colosseum x)
			{
				DateTime t = DateTime.Parse(x.openTime);
				DateTime t2 = DateTime.Parse(x.closeTime);
				return t < ServerDateTime.Now && ServerDateTime.Now < t2;
			});
			if (colosseum != null)
			{
				result = colosseum.worldDungeonId;
			}
			return result;
		}

		public GameWebAPI.ColosseumUserStatusLogic GetColosseumUserStatusRequest()
		{
			GameWebAPI.ColosseumUserStatusLogic colosseumUserStatusLogic = new GameWebAPI.ColosseumUserStatusLogic();
			colosseumUserStatusLogic.SetSendData = delegate(GameWebAPI.ReqData_ColosseumUserStatusLogic param)
			{
				param.target = GameWebAPI.ReqData_ColosseumUserStatusLogic.GetTargetMyFlag();
				param.isMockBattle = 0;
			};
			return colosseumUserStatusLogic;
		}

		public PvPMatching GetStartMatchingRequest()
		{
			return new PvPMatching
			{
				act = 1,
				uniqueRequestId = Singleton<TCPUtil>.Instance.GetUniqueRequestId()
			};
		}

		public PvPMatching GetStopMatchingRequest()
		{
			return new PvPMatching
			{
				act = 0,
				uniqueRequestId = Singleton<TCPUtil>.Instance.GetUniqueRequestId()
			};
		}

		public PvPBattleRecover GetResumeMatchingRequest()
		{
			return new PvPBattleRecover
			{
				uniqueRequestId = Singleton<TCPUtil>.Instance.GetUniqueRequestId()
			};
		}

		public GameWebAPI.ColosseumBattleEndLogic GetLoseBattleRequest()
		{
			GameWebAPI.ColosseumBattleEndLogic colosseumBattleEndLogic = new GameWebAPI.ColosseumBattleEndLogic();
			colosseumBattleEndLogic.SetSendData = delegate(GameWebAPI.ReqData_ColosseumBattleEndLogic param)
			{
				param.battleResult = 2;
			};
			return colosseumBattleEndLogic;
		}

		public GameWebAPI.ColosseumUserStatusLogic GetOpponentColosseumUserStatusRequest()
		{
			GameWebAPI.ColosseumUserStatusLogic colosseumUserStatusLogic = new GameWebAPI.ColosseumUserStatusLogic();
			colosseumUserStatusLogic.SetSendData = delegate(GameWebAPI.ReqData_ColosseumUserStatusLogic param)
			{
				param.target = "battle";
			};
			return colosseumUserStatusLogic;
		}

		public GameWebAPI.ColosseumDeckInfoLogic GetOpponentColosseumDeckRequest()
		{
			return new GameWebAPI.ColosseumDeckInfoLogic();
		}

		public PvPBattleStart GetStartBattleRequest()
		{
			int entranceType = (!this.matchingConfig.isHighCostMainBattle) ? 1 : 2;
			return new PvPBattleStart
			{
				entranceType = entranceType,
				uniqueRequestId = Singleton<TCPUtil>.Instance.GetUniqueRequestId()
			};
		}

		public GameWebAPI.ColosseumPrepareStatusLogic GetOpponentReadyCheckRequest()
		{
			return new GameWebAPI.ColosseumPrepareStatusLogic();
		}
	}
}
