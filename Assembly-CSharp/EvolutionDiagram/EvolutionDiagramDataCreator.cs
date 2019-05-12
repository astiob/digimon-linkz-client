using System;
using System.Collections.Generic;

namespace EvolutionDiagram
{
	public static class EvolutionDiagramDataCreator
	{
		public static void CreateMonsterDataList(EvolutionDiagramData diagramData)
		{
			GameWebAPI.RespDataMA_GetMonsterMS.MonsterM[] monsterM = MasterDataMng.Instance().RespDataMA_MonsterMS.monsterM;
			List<string> list = new List<string>();
			foreach (GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monsterM2 in monsterM)
			{
				if (monsterM2.GetArousal() == 0)
				{
					GameWebAPI.RespDataMA_GetMonsterMG.MonsterM monsterGroupMasterByMonsterGroupId = MonsterDataMng.Instance().GetMonsterGroupMasterByMonsterGroupId(monsterM2.monsterGroupId);
					if ("0" != monsterGroupMasterByMonsterGroupId.monsterCollectionId && !list.Contains(monsterGroupMasterByMonsterGroupId.monsterCollectionId))
					{
						list.Add(monsterGroupMasterByMonsterGroupId.monsterCollectionId);
						EvolutionDiagramData.IconMonster monsterData = new EvolutionDiagramData.IconMonster
						{
							collectionId = monsterGroupMasterByMonsterGroupId.monsterCollectionId.ToInt32(),
							singleData = monsterM2,
							groupData = monsterGroupMasterByMonsterGroupId
						};
						diagramData.AddMonsterData(monsterData);
					}
				}
			}
		}

		public static int CompareByCollectionId(EvolutionDiagramData.IconMonster dataA, EvolutionDiagramData.IconMonster dataB)
		{
			return dataA.collectionId - dataB.collectionId;
		}
	}
}
