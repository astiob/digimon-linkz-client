using Evolution;
using System;
using System.Collections.Generic;

namespace Monster
{
	public static class MonsterFilter
	{
		public static List<MonsterData> Filter(List<MonsterData> targetMonsterList, MonsterFilterType type)
		{
			List<MonsterData> list = new List<MonsterData>();
			switch (type)
			{
			case MonsterFilterType.GROWING_IN_GARDEN:
				for (int i = 0; i < targetMonsterList.Count; i++)
				{
					bool flag = MonsterGrowStepData.IsGardenDigimonScope(targetMonsterList[i].GetMonsterMaster().Group.growStep);
					if (flag && (targetMonsterList[i].GetMonster().IsEgg() || !string.IsNullOrEmpty(targetMonsterList[i].GetMonster().growEndDate)))
					{
						list.Add(targetMonsterList[i]);
					}
				}
				break;
			case MonsterFilterType.ALL_OUT_GARDEN:
				for (int j = 0; j < targetMonsterList.Count; j++)
				{
					if (!MonsterGrowStepData.IsGardenDigimonScope(targetMonsterList[j].GetMonsterMaster().Group.growStep))
					{
						list.Add(targetMonsterList[j]);
					}
				}
				break;
			case MonsterFilterType.RESEARCH_TARGET:
				for (int k = 0; k < targetMonsterList.Count; k++)
				{
					if (MonsterGrowStepData.IsUltimateScope(targetMonsterList[k].GetMonsterMaster().Group.growStep))
					{
						list.Add(targetMonsterList[k]);
					}
				}
				break;
			case MonsterFilterType.CAN_EVOLVE:
				for (int l = 0; l < targetMonsterList.Count; l++)
				{
					if (!MonsterGrowStepData.IsGardenDigimonScope(targetMonsterList[l].GetMonsterMaster().Group.growStep))
					{
						List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> evoList = ClassSingleton<EvolutionData>.Instance.GetEvoList(targetMonsterList[l].GetMonster().monsterId);
						if (0 < evoList.Count)
						{
							list.Add(targetMonsterList[l]);
						}
					}
				}
				break;
			case MonsterFilterType.ALL_IN_GARDEN:
				for (int m = 0; m < targetMonsterList.Count; m++)
				{
					if (MonsterGrowStepData.IsGardenDigimonScope(targetMonsterList[m].GetMonsterMaster().Group.growStep))
					{
						list.Add(targetMonsterList[m]);
					}
				}
				break;
			case MonsterFilterType.HAVE_MEDALS:
				list = MonsterFilter.SelectionMedal(targetMonsterList, true);
				break;
			case MonsterFilterType.CAN_VERSION_UP:
				for (int n = 0; n < targetMonsterList.Count; n++)
				{
					if (targetMonsterList[n].CanVersionUp())
					{
						List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> monsterVersionUpList = ClassSingleton<EvolutionData>.Instance.GetMonsterVersionUpList(targetMonsterList[n].GetMonsterMaster().Simple.monsterId);
						if (0 < monsterVersionUpList.Count)
						{
							list.Add(targetMonsterList[n]);
						}
					}
				}
				break;
			case MonsterFilterType.ALL_VERSION_UP:
				for (int num = 0; num < targetMonsterList.Count; num++)
				{
					if (MonsterStatusData.IsVersionUp(targetMonsterList[num].GetMonsterMaster().Simple.rare))
					{
						list.Add(targetMonsterList[num]);
					}
				}
				break;
			}
			return list;
		}

		public static List<MonsterData> DetailedFilter(List<MonsterData> targetMonsterList, MonsterDetailedFilterType filterType)
		{
			if (filterType == MonsterDetailedFilterType.NONE)
			{
				return targetMonsterList;
			}
			List<MonsterData> list = targetMonsterList;
			if ((filterType & MonsterDetailedFilterType.LEADER_SKILL) > MonsterDetailedFilterType.NONE)
			{
				list = MonsterFilter.SelectionLeaderSkill(list, true);
			}
			if ((filterType & MonsterDetailedFilterType.ACTIVE_SUCCESS) > MonsterDetailedFilterType.NONE)
			{
				list = MonsterFilter.SelectionActiveSuccess(list);
			}
			if ((filterType & MonsterDetailedFilterType.PASSIV_SUCCESS) > MonsterDetailedFilterType.NONE)
			{
				list = MonsterFilter.SelectionPassivSuccess(list);
			}
			if ((filterType & MonsterDetailedFilterType.MEDAL) > MonsterDetailedFilterType.NONE)
			{
				list = MonsterFilter.SelectionMedal(list, true);
			}
			if ((filterType & MonsterDetailedFilterType.NO_LEADER_SKILL) > MonsterDetailedFilterType.NONE)
			{
				list = MonsterFilter.SelectionLeaderSkill(list, false);
			}
			if ((filterType & MonsterDetailedFilterType.NO_MEDAL) > MonsterDetailedFilterType.NONE)
			{
				list = MonsterFilter.SelectionMedal(list, false);
			}
			return list;
		}

		private static List<MonsterData> SelectionLeaderSkill(List<MonsterData> mdList, bool isPossession)
		{
			List<MonsterData> list = new List<MonsterData>();
			for (int i = 0; i < mdList.Count; i++)
			{
				if (isPossession)
				{
					if (mdList[i].GetLeaderSkill() != null && !mdList[i].GetMonster().IsEgg())
					{
						list.Add(mdList[i]);
					}
				}
				else if (mdList[i].GetLeaderSkill() == null && !mdList[i].GetMonster().IsEgg())
				{
					list.Add(mdList[i]);
				}
			}
			return list;
		}

		private static List<MonsterData> SelectionActiveSuccess(List<MonsterData> mdList)
		{
			List<MonsterData> list = new List<MonsterData>();
			for (int i = 0; i < mdList.Count; i++)
			{
				if (mdList[i].GetCommonSkill() != null && !mdList[i].GetMonster().IsEgg())
				{
					for (int j = 0; j < ConstValue.SkillDetailM_effectType.Length; j++)
					{
						if (mdList[i].GetCommonSkillDetail().effectType == ConstValue.SkillDetailM_effectType[j])
						{
							list.Add(mdList[i]);
							break;
						}
					}
				}
			}
			return list;
		}

		private static List<MonsterData> SelectionPassivSuccess(List<MonsterData> mdList)
		{
			List<MonsterData> list = new List<MonsterData>();
			for (int i = 0; i < mdList.Count; i++)
			{
				if (mdList[i].GetCommonSkill() != null && !mdList[i].GetMonster().IsEgg())
				{
					int j;
					for (j = 0; j < ConstValue.SkillDetailM_effectType.Length; j++)
					{
						if (mdList[i].GetCommonSkillDetail().effectType == ConstValue.SkillDetailM_effectType[j])
						{
							break;
						}
					}
					if (j == ConstValue.SkillDetailM_effectType.Length)
					{
						list.Add(mdList[i]);
					}
				}
			}
			return list;
		}

		private static List<MonsterData> SelectionMedal(List<MonsterData> mdList, bool isPossession)
		{
			List<MonsterData> list = new List<MonsterData>();
			for (int i = 0; i < mdList.Count; i++)
			{
				if (mdList[i].ExistMedal() == isPossession)
				{
					list.Add(mdList[i]);
				}
			}
			return list;
		}
	}
}
