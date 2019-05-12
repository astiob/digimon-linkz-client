using System;
using System.Collections.Generic;

namespace ResistanceTrance
{
	public static class AttributeResistance
	{
		public static bool GetResistanceTrance(string monsterId, out List<GameWebAPI.RespDataMA_GetMonsterTranceM.MonsterTranceM> materialList)
		{
			materialList = null;
			GameWebAPI.RespDataMA_GetMonsterTranceM.MonsterTranceM[] monsterTranceM = MasterDataMng.Instance().RespDataMA_MonsterTranceM.monsterTranceM;
			for (int i = 0; i < monsterTranceM.Length; i++)
			{
				if (monsterTranceM[i].monsterId == monsterId)
				{
					if (materialList == null)
					{
						materialList = new List<GameWebAPI.RespDataMA_GetMonsterTranceM.MonsterTranceM>();
					}
					materialList.Add(monsterTranceM[i]);
				}
			}
			return null != materialList;
		}

		public static bool IsSpecialTypeMonster(List<GameWebAPI.RespDataMA_GetMonsterTranceM.MonsterTranceM> materialList)
		{
			return 1 < materialList.Count;
		}

		public static bool IsNeedFragment(List<GameWebAPI.RespDataMA_GetMonsterTranceM.MonsterTranceM> materialList)
		{
			return 0 < materialList.Count;
		}

		public static bool GetResistanceTribeTrance(string tribeId, string growStep, out List<GameWebAPI.RespDataMA_GetMonsterTribeTranceM.MonsterTribeTranceM> materialList)
		{
			materialList = null;
			GameWebAPI.RespDataMA_GetMonsterTribeTranceM.MonsterTribeTranceM[] monsterTribeTranceM = MasterDataMng.Instance().RespDataMA_MonsterTribeTranceM.monsterTribeTranceM;
			for (int i = 0; i < monsterTribeTranceM.Length; i++)
			{
				if (monsterTribeTranceM[i].tribe == tribeId && monsterTribeTranceM[i].growStep == growStep)
				{
					if (materialList == null)
					{
						materialList = new List<GameWebAPI.RespDataMA_GetMonsterTribeTranceM.MonsterTribeTranceM>();
					}
					materialList.Add(monsterTribeTranceM[i]);
				}
			}
			return null != materialList;
		}
	}
}
