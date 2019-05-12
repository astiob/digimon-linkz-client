using Master;
using System;
using System.Collections.Generic;

namespace Monster
{
	public static class MonsterTribeData
	{
		private static Dictionary<string, GameWebAPI.RespDataMA_GetMonsterTribeM.MonsterTribeM> tribeTable;

		public static void Initialize()
		{
			if (MonsterTribeData.tribeTable == null)
			{
				MonsterTribeData.tribeTable = new Dictionary<string, GameWebAPI.RespDataMA_GetMonsterTribeM.MonsterTribeM>();
			}
			else
			{
				MonsterTribeData.tribeTable.Clear();
			}
		}

		public static string GetTribeName(string tribe)
		{
			string result = StringMaster.GetString("CharaStatus-03");
			GameWebAPI.RespDataMA_GetMonsterTribeM.MonsterTribeM tribeMaster = MonsterTribeData.GetTribeMaster(tribe);
			if (tribeMaster != null)
			{
				result = tribeMaster.monsterTribeName;
			}
			return result;
		}

		public static GameWebAPI.RespDataMA_GetMonsterTribeM.MonsterTribeM GetTribeMaster(string tribe)
		{
			GameWebAPI.RespDataMA_GetMonsterTribeM.MonsterTribeM result = null;
			if (!MonsterTribeData.tribeTable.TryGetValue(tribe, out result))
			{
				GameWebAPI.RespDataMA_GetMonsterTribeM.MonsterTribeM[] monsterTribeM = MasterDataMng.Instance().RespDataMA_MonsterTribeM.monsterTribeM;
				for (int i = 0; i < monsterTribeM.Length; i++)
				{
					if (monsterTribeM[i].monsterTribeId == tribe)
					{
						result = monsterTribeM[i];
						MonsterTribeData.tribeTable.Add(tribe, monsterTribeM[i]);
						break;
					}
				}
			}
			return result;
		}
	}
}
