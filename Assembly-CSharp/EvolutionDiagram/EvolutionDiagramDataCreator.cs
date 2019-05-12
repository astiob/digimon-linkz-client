using Monster;
using System;
using System.Collections.Generic;

namespace EvolutionDiagram
{
	public static class EvolutionDiagramDataCreator
	{
		public static void CreateMonsterDataList(EvolutionDiagramData diagramData)
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, Dictionary<string, MonsterClientMaster>> keyValuePair in MonsterMaster.GetGroupMasterList())
			{
				MonsterClientMaster monsterClientMaster;
				if (keyValuePair.Value.TryGetValue("1", out monsterClientMaster) && "0" != monsterClientMaster.Group.monsterCollectionId && !list.Contains(monsterClientMaster.Group.monsterCollectionId))
				{
					list.Add(monsterClientMaster.Group.monsterCollectionId);
					EvolutionDiagramData.IconMonster monsterData = new EvolutionDiagramData.IconMonster
					{
						collectionId = monsterClientMaster.Group.monsterCollectionId.ToInt32(),
						master = monsterClientMaster
					};
					diagramData.AddMonsterData(monsterData);
				}
			}
		}

		public static int CompareByCollectionId(EvolutionDiagramData.IconMonster dataA, EvolutionDiagramData.IconMonster dataB)
		{
			return dataA.collectionId - dataB.collectionId;
		}
	}
}
