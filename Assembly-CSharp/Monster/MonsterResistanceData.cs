using System;
using System.Collections.Generic;

namespace Monster
{
	public static class MonsterResistanceData
	{
		private static GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM AddResistanceData(GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM addResistance, ref GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM destResistance)
		{
			if (addResistance.none != "0")
			{
				destResistance.none = addResistance.none;
			}
			if (addResistance.fire != "0")
			{
				destResistance.fire = addResistance.fire;
			}
			if (addResistance.water != "0")
			{
				destResistance.water = addResistance.water;
			}
			if (addResistance.nature != "0")
			{
				destResistance.nature = addResistance.nature;
			}
			if (addResistance.thunder != "0")
			{
				destResistance.thunder = addResistance.thunder;
			}
			if (addResistance.dark != "0")
			{
				destResistance.dark = addResistance.dark;
			}
			if (addResistance.light != "0")
			{
				destResistance.light = addResistance.light;
			}
			if (addResistance.stun != "0")
			{
				destResistance.stun = addResistance.stun;
			}
			if (addResistance.skillLock != "0")
			{
				destResistance.skillLock = addResistance.skillLock;
			}
			if (addResistance.sleep != "0")
			{
				destResistance.sleep = addResistance.sleep;
			}
			if (addResistance.paralysis != "0")
			{
				destResistance.paralysis = addResistance.paralysis;
			}
			if (addResistance.confusion != "0")
			{
				destResistance.confusion = addResistance.confusion;
			}
			if (addResistance.poison != "0")
			{
				destResistance.poison = addResistance.poison;
			}
			if (addResistance.death != "0")
			{
				destResistance.death = addResistance.death;
			}
			return destResistance;
		}

		public static GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM GetResistanceMaster(string resistanceId)
		{
			GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM monsterResistanceM = null;
			GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM[] monsterResistanceM2 = MasterDataMng.Instance().RespDataMA_MonsterResistanceM.monsterResistanceM;
			for (int i = 0; i < monsterResistanceM2.Length; i++)
			{
				if (monsterResistanceM2[i].monsterResistanceId == resistanceId)
				{
					monsterResistanceM = monsterResistanceM2[i];
					break;
				}
			}
			Debug.Assert(null != monsterResistanceM, "耐性マスターデータが見つからない id (" + resistanceId + ")");
			return monsterResistanceM;
		}

		public static List<GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM> GetUniqueResistanceListByJson(string resistIds)
		{
			List<GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM> list = new List<GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM>();
			if (!string.IsNullOrEmpty(resistIds))
			{
				string[] array = resistIds.Split(new char[]
				{
					','
				});
				for (int i = 0; i < array.Length; i++)
				{
					GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM resistanceMaster = MonsterResistanceData.GetResistanceMaster(array[i]);
					list.Add(resistanceMaster);
				}
			}
			return list;
		}

		public static List<GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM> GetUniqueResistanceList(List<string> resistIdList)
		{
			List<GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM> list = new List<GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM>();
			for (int i = 0; i < resistIdList.Count; i++)
			{
				GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM resistanceMaster = MonsterResistanceData.GetResistanceMaster(resistIdList[i]);
				list.Add(resistanceMaster);
			}
			return list;
		}

		public static GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM AddResistanceFromMultipleTranceData(GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM resistanceMaster, List<GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM> uniqueResistanceList)
		{
			GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM monsterResistanceM = new GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM();
			monsterResistanceM.monsterResistanceId = resistanceMaster.monsterResistanceId;
			monsterResistanceM.description = resistanceMaster.description;
			monsterResistanceM.fire = resistanceMaster.fire;
			monsterResistanceM.water = resistanceMaster.water;
			monsterResistanceM.thunder = resistanceMaster.thunder;
			monsterResistanceM.nature = resistanceMaster.nature;
			monsterResistanceM.none = resistanceMaster.none;
			monsterResistanceM.light = resistanceMaster.light;
			monsterResistanceM.dark = resistanceMaster.dark;
			monsterResistanceM.poison = resistanceMaster.poison;
			monsterResistanceM.confusion = resistanceMaster.confusion;
			monsterResistanceM.paralysis = resistanceMaster.paralysis;
			monsterResistanceM.sleep = resistanceMaster.sleep;
			monsterResistanceM.stun = resistanceMaster.stun;
			monsterResistanceM.skillLock = resistanceMaster.skillLock;
			monsterResistanceM.death = resistanceMaster.death;
			for (int i = 0; i < uniqueResistanceList.Count; i++)
			{
				MonsterResistanceData.AddResistanceData(uniqueResistanceList[i], ref monsterResistanceM);
			}
			return monsterResistanceM;
		}
	}
}
