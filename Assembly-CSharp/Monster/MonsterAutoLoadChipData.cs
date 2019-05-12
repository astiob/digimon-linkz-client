using System;
using System.Collections.Generic;

namespace Monster
{
	public static class MonsterAutoLoadChipData
	{
		public static List<int> GetChipList(string targetMonsterId)
		{
			List<int> list = new List<int>();
			int num;
			if (int.TryParse(targetMonsterId, out num))
			{
				GameWebAPI.ResponseMonsterAutoLoadChipMaster.AutoLoadChip[] monsterAutoLoadChipM = MasterDataMng.Instance().MonsterAutoLoadChipMaster.monsterAutoLoadChipM;
				for (int i = 0; i < monsterAutoLoadChipM.Length; i++)
				{
					if (monsterAutoLoadChipM[i].monsterId == num && !list.Contains(monsterAutoLoadChipM[i].attachChipId))
					{
						list.Add(monsterAutoLoadChipM[i].attachChipId);
					}
				}
			}
			return list;
		}
	}
}
