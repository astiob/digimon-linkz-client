using System;

namespace Monster
{
	public abstract class MonsterUserDataSort
	{
		protected MonsterSortOrder sortOrder;

		protected int CompareMonsterIdAndLevel(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList dataA, GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList dataB)
		{
			int num = int.Parse(dataA.monsterId);
			int num2 = int.Parse(dataB.monsterId);
			if (num > num2)
			{
				return -1;
			}
			if (num < num2)
			{
				return 1;
			}
			num = int.Parse(dataA.level);
			num2 = int.Parse(dataB.level);
			if (num > num2)
			{
				return -1;
			}
			if (num < num2)
			{
				return 1;
			}
			num = int.Parse(dataA.userMonsterId);
			num2 = int.Parse(dataB.userMonsterId);
			if (num > num2)
			{
				return -1;
			}
			if (num < num2)
			{
				return 1;
			}
			return 0;
		}

		protected int CompareMonsterIdAndArousal(MonsterUserData dataA, MonsterUserData dataB)
		{
			int num = int.Parse(dataA.GetMonster().monsterId);
			int num2 = int.Parse(dataB.GetMonster().monsterId);
			if (num > num2)
			{
				return -1;
			}
			if (num < num2)
			{
				return 1;
			}
			num = int.Parse(dataA.GetMonsterMaster().Simple.rare);
			num2 = int.Parse(dataB.GetMonsterMaster().Simple.rare);
			if (num > num2)
			{
				return -1;
			}
			if (num < num2)
			{
				return 1;
			}
			num = int.Parse(dataA.GetMonster().userMonsterId);
			num2 = int.Parse(dataB.GetMonster().userMonsterId);
			if (num > num2)
			{
				return -1;
			}
			if (num < num2)
			{
				return 1;
			}
			return 0;
		}

		protected int GetMonsterParameter(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster, string value)
		{
			int result = -1;
			if (!userMonster.IsEgg())
			{
				result = int.Parse(value);
			}
			return result;
		}

		public abstract int Compare(MonsterUserData dataA, MonsterUserData dataB);

		public void SetSortOrder(MonsterSortOrder order)
		{
			this.sortOrder = order;
		}
	}
}
