using System;

namespace EvolutionDiagram
{
	public static class EvolutionDiagramDataCreator
	{
		public static void CreateMonsterDataList(EvolutionDiagramData diagramData)
		{
			foreach (GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monsterM2 in MasterDataMng.Instance().RespDataMA_MonsterMS.monsterM)
			{
				if (monsterM2.GetArousal() == 0)
				{
					GameWebAPI.RespDataMA_GetMonsterMG.MonsterM monsterGroupMasterByMonsterGroupId = MonsterDataMng.Instance().GetMonsterGroupMasterByMonsterGroupId(monsterM2.monsterGroupId);
					if ("0" != monsterGroupMasterByMonsterGroupId.monsterCollectionId)
					{
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
