using System;
using System.Collections.Generic;

public static class MonsterEvolutionUtil
{
	public static Dictionary<int, GameWebAPI.MonsterEvolutionMaterialMaster.Material> materials = new Dictionary<int, GameWebAPI.MonsterEvolutionMaterialMaster.Material>();

	public static void ClearCache()
	{
		MonsterEvolutionUtil.materials.Clear();
	}

	public static GameWebAPI.MonsterEvolutionMaterialMaster.Material GetEvolutionMaterial(int monsterEvolutionMaterialId)
	{
		GameWebAPI.MonsterEvolutionMaterialMaster.Material result;
		if (!MonsterEvolutionUtil.materials.TryGetValue(monsterEvolutionMaterialId, out result))
		{
			GameWebAPI.MonsterEvolutionMaterialMaster monsterEvolutionMaterialMaster = MasterDataMng.Instance().MonsterEvolutionMaterialMaster;
			int count = MonsterEvolutionUtil.materials.Count;
			for (int i = count; i < monsterEvolutionMaterialMaster.Materials.Length; i++)
			{
				MonsterEvolutionUtil.materials.Add(monsterEvolutionMaterialMaster.Materials[i].monsterEvolutionMaterialId, monsterEvolutionMaterialMaster.Materials[i]);
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
