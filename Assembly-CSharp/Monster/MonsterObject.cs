using System;
using System.Text;

namespace Monster
{
	public static class MonsterObject
	{
		private static StringBuilder stringBuilder = new StringBuilder();

		public static string GetFilePath(string modelId)
		{
			MonsterObject.stringBuilder.Length = 0;
			MonsterObject.stringBuilder.Append("Characters/");
			MonsterObject.stringBuilder.Append(modelId);
			MonsterObject.stringBuilder.Append("/prefab");
			return MonsterObject.stringBuilder.ToString();
		}

		public static string GetEggModelId(string monsterEvolutionRouteId)
		{
			string result = string.Empty;
			foreach (GameWebAPI.RespDataMA_GetMonsterEvolutionRouteM.MonsterEvolutionRouteM monsterEvolutionRouteM2 in MasterDataMng.Instance().RespDataMA_MonsterEvolutionRouteM.monsterEvolutionRouteM)
			{
				if (monsterEvolutionRouteM2.monsterEvolutionRouteId == monsterEvolutionRouteId)
				{
					MonsterClientMaster monsterMasterByMonsterId = MonsterMaster.GetMonsterMasterByMonsterId(monsterEvolutionRouteM2.eggMonsterId);
					if (monsterMasterByMonsterId != null)
					{
						result = monsterMasterByMonsterId.Group.modelId;
					}
					break;
				}
			}
			return result;
		}
	}
}
