using MultiBattle.Tools;
using System;
using System.Collections.Generic;

namespace Colosseum.Matching
{
	public sealed class MatchingResult
	{
		public string dungeonId;

		public MultiBattleData.PvPUserData myData;

		public int[] mySelectMonsterIndexList;

		public MultiBattleData.PvPUserData opponentData;

		public int[] opponentSelectMonsterIndexList;

		public List<MonsterData> opponentMonsterIconDataList;

		public bool isMySelectedMonster;

		public bool isOpponentSelectedMonster;

		public bool isOpponentLeaveRoom;

		public int randomSeed;

		public int OpponentUserId
		{
			get
			{
				return int.Parse(this.opponentData.userStatus.userId);
			}
		}
	}
}
