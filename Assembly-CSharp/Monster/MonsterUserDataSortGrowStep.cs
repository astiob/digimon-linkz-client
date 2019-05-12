using System;

namespace Monster
{
	public sealed class MonsterUserDataSortGrowStep : MonsterUserDataSort
	{
		public override int Compare(MonsterUserData dataA, MonsterUserData dataB)
		{
			int num = base.GetMonsterParameter(dataA.GetMonster(), dataA.GetMonsterMaster().Group.growStep);
			int num2 = base.GetMonsterParameter(dataB.GetMonster(), dataB.GetMonsterMaster().Group.growStep);
			num = MonsterGrowStepData.GetGrowStepSortValue(num);
			num2 = MonsterGrowStepData.GetGrowStepSortValue(num2);
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
