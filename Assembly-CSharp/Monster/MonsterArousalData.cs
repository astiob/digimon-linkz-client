using System;

namespace Monster
{
	public static class MonsterArousalData
	{
		public static string GetTitle(string arousal)
		{
			string result = string.Empty;
			GameWebAPI.RespDataMA_MonsterArousalMaster.ArousalData[] monsterRareM = MasterDataMng.Instance().ResponseMonsterArousalMaster.monsterRareM;
			for (int i = 0; i < monsterRareM.Length; i++)
			{
				if (arousal == monsterRareM[i].monsterRareId)
				{
					result = monsterRareM[i].title;
					break;
				}
			}
			return result;
		}

		public static string GetSpriteName(string arousal)
		{
			string result = string.Empty;
			GameWebAPI.RespDataMA_MonsterArousalMaster.ArousalData[] monsterRareM = MasterDataMng.Instance().ResponseMonsterArousalMaster.monsterRareM;
			for (int i = 0; i < monsterRareM.Length; i++)
			{
				if (arousal == monsterRareM[i].monsterRareId)
				{
					result = monsterRareM[i].name;
					break;
				}
			}
			return result;
		}

		public static int GetGoldMedalMaxNum(string arousal)
		{
			int result = 0;
			GameWebAPI.RespDataMA_MonsterArousalMaster.ArousalData[] monsterRareM = MasterDataMng.Instance().ResponseMonsterArousalMaster.monsterRareM;
			for (int i = 0; i < monsterRareM.Length; i++)
			{
				if (arousal == monsterRareM[i].monsterRareId)
				{
					result = int.Parse(monsterRareM[i].goldMedalMaxNum);
					break;
				}
			}
			return result;
		}

		public static bool IsVersionUp(string arousal)
		{
			int num = int.Parse(arousal);
			return 6 <= num;
		}
	}
}
