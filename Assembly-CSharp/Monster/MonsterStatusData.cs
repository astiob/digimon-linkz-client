using System;

namespace Monster
{
	public static class MonsterStatusData
	{
		private static int CalcLevelStatusUpValue(string valueMin, string valueMax, string levelNow, string levelMax)
		{
			float num = float.Parse(valueMin);
			float num2 = float.Parse(valueMax);
			float num3 = float.Parse(levelNow);
			float num4 = float.Parse(levelMax);
			float num5 = (num2 - num) / (num4 - 1f);
			num5 *= num3 - 1f;
			num5 += num;
			return (int)Math.Round((double)num5, MidpointRounding.AwayFromZero);
		}

		public static StatusValue GetStatusValue(string monsterId, string level)
		{
			StatusValue result = default(StatusValue);
			MonsterClientMaster monsterMasterByMonsterId = MonsterMaster.GetMonsterMasterByMonsterId(monsterId);
			result.hp = MonsterStatusData.CalcLevelStatusUpValue(monsterMasterByMonsterId.Simple.defaultHp, monsterMasterByMonsterId.Simple.maxHp, level, monsterMasterByMonsterId.Simple.maxLevel);
			result.attack = MonsterStatusData.CalcLevelStatusUpValue(monsterMasterByMonsterId.Simple.defaultAttack, monsterMasterByMonsterId.Simple.maxAttack, level, monsterMasterByMonsterId.Simple.maxLevel);
			result.defense = MonsterStatusData.CalcLevelStatusUpValue(monsterMasterByMonsterId.Simple.defaultDefense, monsterMasterByMonsterId.Simple.maxDefense, level, monsterMasterByMonsterId.Simple.maxLevel);
			result.magicAttack = MonsterStatusData.CalcLevelStatusUpValue(monsterMasterByMonsterId.Simple.defaultSpAttack, monsterMasterByMonsterId.Simple.maxSpAttack, level, monsterMasterByMonsterId.Simple.maxLevel);
			result.magicDefense = MonsterStatusData.CalcLevelStatusUpValue(monsterMasterByMonsterId.Simple.defaultSpDefense, monsterMasterByMonsterId.Simple.maxSpDefense, level, monsterMasterByMonsterId.Simple.maxLevel);
			result.speed = int.Parse(monsterMasterByMonsterId.Simple.speed);
			return result;
		}

		public static bool IsLevelMax(string monsterId, string level)
		{
			int level2 = int.Parse(level);
			return MonsterStatusData.IsLevelMax(monsterId, level2);
		}

		public static bool IsLevelMax(string monsterId, int level)
		{
			MonsterClientMaster monsterMasterByMonsterId = MonsterMaster.GetMonsterMasterByMonsterId(monsterId);
			int num = int.Parse(monsterMasterByMonsterId.Simple.maxLevel);
			return num <= level;
		}

		public static bool IsArousal(string arousal)
		{
			bool result = false;
			if (!string.IsNullOrEmpty(arousal))
			{
				result = ("1" != arousal);
			}
			return result;
		}

		public static bool IsVersionUp(string arousal)
		{
			bool result = false;
			if (!string.IsNullOrEmpty(arousal))
			{
				result = (6 <= int.Parse(arousal));
			}
			return result;
		}

		public static bool IsSpecialTrainingType(string monsterType)
		{
			bool result = false;
			if (!string.IsNullOrEmpty(monsterType))
			{
				result = ("1" == monsterType);
			}
			return result;
		}
	}
}
