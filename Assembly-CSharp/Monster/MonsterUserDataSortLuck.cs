using System;

namespace Monster
{
	public sealed class MonsterUserDataSortLuck : MonsterUserDataSort
	{
		public override int Compare(MonsterUserData dataA, MonsterUserData dataB)
		{
			int monsterParameter = base.GetMonsterParameter(dataA.GetMonster(), dataA.GetMonster().luck);
			int monsterParameter2 = base.GetMonsterParameter(dataB.GetMonster(), dataB.GetMonster().luck);
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
