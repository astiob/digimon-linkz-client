using System;
using System.Collections.Generic;

namespace Evolution
{
	public static class EvolutionMaterialData
	{
		private static Dictionary<int, GameWebAPI.MonsterEvolutionMaterialMaster.Material> materialList = new Dictionary<int, GameWebAPI.MonsterEvolutionMaterialMaster.Material>();

		public static void ClearCache()
		{
			EvolutionMaterialData.materialList.Clear();
		}

		public static GameWebAPI.MonsterEvolutionMaterialMaster.Material GetEvolutionMaterial(string monsterEvolutionMaterialId)
		{
			return EvolutionMaterialData.GetEvolutionMaterial(int.Parse(monsterEvolutionMaterialId));
		}

		public static GameWebAPI.MonsterEvolutionMaterialMaster.Material GetEvolutionMaterial(int monsterEvolutionMaterialId)
		{
			GameWebAPI.MonsterEvolutionMaterialMaster.Material result;
			if (!EvolutionMaterialData.materialList.TryGetValue(monsterEvolutionMaterialId, out result))
			{
				GameWebAPI.MonsterEvolutionMaterialMaster monsterEvolutionMaterialMaster = MasterDataMng.Instance().MonsterEvolutionMaterialMaster;
				int count = EvolutionMaterialData.materialList.Count;
				for (int i = count; i < monsterEvolutionMaterialMaster.Materials.Length; i++)
				{
					EvolutionMaterialData.materialList.Add(monsterEvolutionMaterialMaster.Materials[i].monsterEvolutionMaterialId, monsterEvolutionMaterialMaster.Materials[i]);
					if (monsterEvolutionMaterialMaster.Materials[i].monsterEvolutionMaterialId == monsterEvolutionMaterialId)
					{
						result = monsterEvolutionMaterialMaster.Materials[i];
						break;
					}
				}
			}
			return result;
		}

		public static GameWebAPI.UserSoulData GetUserEvolutionMaterial(string materialId)
		{
			GameWebAPI.UserSoulData[] userSoulData = DataMng.Instance().RespDataUS_SoulInfo.userSoulData;
			return Algorithm.BinarySearch<GameWebAPI.UserSoulData>(userSoulData, materialId, 0, userSoulData.Length - 1, "soulId", 8);
		}

		public static int GetUserEvolutionMaterialNum(string materialId)
		{
			int num = 0;
			if (DataMng.Instance().RespDataUS_SoulInfo != null && DataMng.Instance().RespDataUS_SoulInfo.userSoulData != null)
			{
				for (int i = 0; i < DataMng.Instance().RespDataUS_SoulInfo.userSoulData.Length; i++)
				{
					GameWebAPI.UserSoulData userSoulData = DataMng.Instance().RespDataUS_SoulInfo.userSoulData[i];
					if (userSoulData.soulId == materialId)
					{
						num++;
					}
				}
			}
			return num;
		}
	}
}
