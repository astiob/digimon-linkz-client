using System;

namespace Monster
{
	public sealed class MonsterUserDataSortDefense : MonsterUserDataSort
	{
		public override int Compare(MonsterUserData dataA, MonsterUserData dataB)
		{
			int monsterParameter = base.GetMonsterParameter(dataA.GetMonster(), dataA.GetMonster().defense);
			int monsterParameter2 = base.GetMonsterParameter(dataB.GetMonster(), dataB.GetMonster().defense);
			if (this.sortOrder == MonsterSortOrder.DESC)
			{
				if (monsterParameter > monsterParameter2)
				{
					return -1;
				}
				if (monsterParameter < monsterParameter2)
				{
					return 1;
				}
			}
			else
			{
				if (monsterParameter < monsterParameter2)
				{
					return -1;
				}
				if (monsterParameter > monsterParameter2)
				{
					return 1;
				}
			}
			return base.CompareMonsterIdAndLevel(dataA.GetMonster(), dataB.GetMonster());
		}
	}
}
