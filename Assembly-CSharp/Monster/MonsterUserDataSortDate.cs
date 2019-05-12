using System;

namespace Monster
{
	public sealed class MonsterUserDataSortDate : MonsterUserDataSort
	{
		public override int Compare(MonsterUserData dataA, MonsterUserData dataB)
		{
			int num = int.Parse(dataA.GetMonster().userMonsterId);
			int num2 = int.Parse(dataB.GetMonster().userMonsterId);
			if (this.sortOrder == MonsterSortOrder.DESC)
			{
				if (num > num2)
				{
					return -1;
				}
				if (num < num2)
				{
					return 1;
				}
			}
			else
			{
				if (num < num2)
				{
					return -1;
				}
				if (num > num2)
				{
					return 1;
				}
			}
			return 0;
		}
	}
}
