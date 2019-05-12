using System;

namespace MultiBattle.Tools
{
	public class MultiBattleData : ClassSingleton<MultiBattleData>
	{
		public string MyPlayerUserId { get; set; }

		public int MaxAttackTime { get; set; }

		public int HurryUpAttackTime { get; set; }

		public int MaxRoundNum { get; set; }

		public string MockBattleUserCode { get; set; }

		public bool IsSimulator { get; set; }

		public MultiUser[] MultiUsers { get; set; }

		public string[] PlayerUserMonsterIds { get; set; }

		public MultiBattleData.PvPUserData[] PvPUserDatas { get; set; }

		public MultiBattleData.PvPFieldData PvPField { get; set; }

		public int BattleResult { get; set; }

		public MultiBattleData.BattleEndResponseData BattleEndResponse { get; set; }

		[Serializable]
		public class PvPUserData
		{
			public bool isOwner;

			public GameWebAPI.ColosseumUserStatus userStatus;

			public GameWebAPI.Common_MonsterData[] monsterData;
		}

		public class PvPFieldData
		{
			public string worldDungeonId;

			public string startId;
		}

		public class BattleEndResponseData
		{
			public int resultCode;

			public MultiBattleData.BattleEndResponseData.Reward[] reward;

			public MultiBattleData.BattleEndResponseData.Reward[] firstRankUpReward;

			public int score;

			public int colosseumRankId;

			public int isFirstRankUp;

			public MultiBattleData.BattleEndResponseData.ColosseumBattleRecord battleRecord;

			public class Reward
			{
				public int assetCategoryId;

				public int assetValue;

				public int assetNum;
			}

			public class ColosseumBattleRecord
			{
				public int count;

				public int winPercent;
			}
		}
	}
}
