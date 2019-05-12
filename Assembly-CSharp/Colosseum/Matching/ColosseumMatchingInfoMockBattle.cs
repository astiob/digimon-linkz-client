using System;

namespace Colosseum.Matching
{
	public sealed class ColosseumMatchingInfoMockBattle : IColosseumMatchingInfo
	{
		private MatchingConfig matchingConfig;

		public ColosseumMatchingInfoMockBattle(MatchingConfig config)
		{
			this.matchingConfig = config;
		}

		public string GetDungeonId()
		{
			return "10001";
		}

		public GameWebAPI.ColosseumUserStatusLogic GetColosseumUserStatusRequest()
		{
			GameWebAPI.ColosseumUserStatusLogic colosseumUserStatusLogic = new GameWebAPI.ColosseumUserStatusLogic();
			colosseumUserStatusLogic.SetSendData = delegate(GameWebAPI.ReqData_ColosseumUserStatusLogic param)
			{
				param.target = GameWebAPI.ReqData_ColosseumUserStatusLogic.GetTargetMyFlag();
				param.isMockBattle = 1;
			};
			return colosseumUserStatusLogic;
		}

		public PvPMatching GetStartMatchingRequest()
		{
			return new PvPMatching
			{
				act = 1,
				targetUserCode = this.matchingConfig.mockBattleOppoentUserCode,
				isMockBattle = this.matchingConfig.isMockBattle,
				uniqueRequestId = Singleton<TCPUtil>.Instance.GetUniqueRequestId()
			};
		}

		public PvPMatching GetStopMatchingRequest()
		{
			return new PvPMatching
			{
				act = 0,
				targetUserCode = this.matchingConfig.mockBattleOppoentUserCode,
				isMockBattle = this.matchingConfig.isMockBattle,
				uniqueRequestId = Singleton<TCPUtil>.Instance.GetUniqueRequestId()
			};
		}

		public PvPBattleRecover GetResumeMatchingRequest()
		{
			return new PvPBattleRecover
			{
				isMockBattle = 2,
				uniqueRequestId = Singleton<TCPUtil>.Instance.GetUniqueRequestId()
			};
		}

		public GameWebAPI.ColosseumBattleEndLogic GetLoseBattleRequest()
		{
			GameWebAPI.ColosseumBattleEndLogic colosseumBattleEndLogic = new GameWebAPI.ColosseumBattleEndLogic();
			colosseumBattleEndLogic.SetSendData = delegate(GameWebAPI.ReqData_ColosseumBattleEndLogic param)
			{
				param.battleResult = 2;
				param.isMockBattle = 1;
			};
			return colosseumBattleEndLogic;
		}

		public GameWebAPI.ColosseumUserStatusLogic GetOpponentColosseumUserStatusRequest()
		{
			GameWebAPI.ColosseumUserStatusLogic colosseumUserStatusLogic = new GameWebAPI.ColosseumUserStatusLogic();
			colosseumUserStatusLogic.SetSendData = delegate(GameWebAPI.ReqData_ColosseumUserStatusLogic param)
			{
				param.target = "battle";
				param.isMockBattle = 1;
			};
			return colosseumUserStatusLogic;
		}

		public GameWebAPI.ColosseumDeckInfoLogic GetOpponentColosseumDeckRequest()
		{
			GameWebAPI.ColosseumDeckInfoLogic colosseumDeckInfoLogic = new GameWebAPI.ColosseumDeckInfoLogic();
			colosseumDeckInfoLogic.SetSendData = delegate(GameWebAPI.ReqData_ColosseumDeckInfoLogic param)
			{
				param.isMockBattle = 1;
			};
			return colosseumDeckInfoLogic;
		}

		public PvPBattleStart GetStartBattleRequest()
		{
			return new PvPBattleStart
			{
				isMockBattle = 1,
				entranceType = 1,
				uniqueRequestId = Singleton<TCPUtil>.Instance.GetUniqueRequestId()
			};
		}

		public GameWebAPI.ColosseumPrepareStatusLogic GetOpponentReadyCheckRequest()
		{
			GameWebAPI.ColosseumPrepareStatusLogic colosseumPrepareStatusLogic = new GameWebAPI.ColosseumPrepareStatusLogic();
			colosseumPrepareStatusLogic.SetSendData = delegate(GameWebAPI.ReqData_ColosseumPrepareStatusLogic param)
			{
				param.isMockBattle = 1;
			};
			return colosseumPrepareStatusLogic;
		}
	}
}
