using System;

namespace Monster
{
	public sealed class MonsterUserDataSortArousal : MonsterUserDataSort
	{
		public override int Compare(MonsterUserData dataA, MonsterUserData dataB)
		{
			int num = int.Parse(dataA.GetMonsterMaster().Simple.rare);
			int num2 = int.Parse(dataB.GetMonsterMaster().Simple.rare);
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
			return base.CompareMonsterIdAndLevel(dataA.GetMonster(), dataB.GetMonster());
		}
	}
}
