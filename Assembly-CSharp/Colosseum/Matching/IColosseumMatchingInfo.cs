using System;

namespace Colosseum.Matching
{
	public interface IColosseumMatchingInfo
	{
		string GetDungeonId();

		GameWebAPI.ColosseumUserStatusLogic GetColosseumUserStatusRequest();

		PvPMatching GetStartMatchingRequest();

		PvPMatching GetStopMatchingRequest();

		PvPBattleRecover GetResumeMatchingRequest();

		GameWebAPI.ColosseumBattleEndLogic GetLoseBattleRequest();

		GameWebAPI.ColosseumUserStatusLogic GetOpponentColosseumUserStatusRequest();

		GameWebAPI.ColosseumDeckInfoLogic GetOpponentColosseumDeckRequest();

		PvPBattleStart GetStartBattleRequest();

		GameWebAPI.ColosseumPrepareStatusLogic GetOpponentReadyCheckRequest();
	}
}
