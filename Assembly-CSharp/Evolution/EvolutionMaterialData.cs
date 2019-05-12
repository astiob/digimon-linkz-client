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
	}
}
