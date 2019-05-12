using System;

namespace Colosseum.Matching
{
	public sealed class MatchingConfig
	{
		public string mockBattleOppoentUserCode;

		public int isMockBattle;

		public bool isHighCostMainBattle;

		public int staminaCost;

		public bool IsMockBattle()
		{
			return 1 == this.isMockBattle;
		}
	}
}
