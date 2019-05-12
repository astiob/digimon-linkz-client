using System;
using System.Collections.Generic;

namespace Monster
{
	public static class MonsterSort
	{
		public static void SortMonsterUserDataList(List<MonsterData> monsterDataList, MonsterSortType sortType, MonsterSortOrder sortOrder)
		{
			MonsterUserDataSort monsterUserDataSort;
			switch (sortType)
			{
			case MonsterSortType.DATE:
				monsterUserDataSort = new MonsterUserDataSortDate();
				break;
			case MonsterSortType.AROUSAL:
				monsterUserDataSort = new MonsterUserDataSortArousal();
				break;
			default:
				monsterUserDataSort = new MonsterUserDataSortLevel();
				break;
			case MonsterSortType.HP:
				monsterUserDataSort = new MonsterUserDataSortHP();
				break;
			case MonsterSortType.ATK:
				monsterUserDataSort = new MonsterUserDataSortAttack();
				break;
			case MonsterSortType.DEF:
				monsterUserDataSort = new MonsterUserDataSortDefense();
				break;
			case MonsterSortType.S_ATK:
				monsterUserDataSort = new MonsterUserDataSortMagicAttack();
				break;
			case MonsterSortType.S_DEF:
				monsterUserDataSort = new MonsterUserDataSortMagicDefense();
				break;
			case MonsterSortType.SPD:
				monsterUserDataSort = new MonsterUserDataSortSpeed();
				break;
			case MonsterSortType.LUCK:
				monsterUserDataSort = new MonsterUserDataSortLuck();
				break;
			case MonsterSortType.GROW_STEP:
				monsterUserDataSort = new MonsterUserDataSortGrowStep();
				break;
			case MonsterSortType.TRIBE:
				monsterUserDataSort = new MonsterUserDataSortTribe();
				break;
			case MonsterSortType.FRIENDSHIP:
				monsterUserDataSort = new MonsterUserDataSortFriendship();
				break;
			}
			monsterUserDataSort.SetSortOrder(sortOrder);
			monsterDataList.Sort(new Comparison<MonsterData>(monsterUserDataSort.Compare));
		}
	}
}
