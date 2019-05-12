using Master;
using System;
using System.Collections.Generic;

namespace EvolutionDiagram
{
	public static class EvolutionDiagramRefine
	{
		public static List<EvolutionDiagramData.IconMonster> GetRefineMonsterData(EvolutionDiagramData evolutionData, GrowStep growStep)
		{
			return evolutionData.GetMonsterDataList(growStep);
		}

		public static List<EvolutionDiagramData.IconMonster> GetRefineMonsterData(EvolutionDiagramData evolutionData, GrowStep[] growStepList)
		{
			List<EvolutionDiagramData.IconMonster> list = new List<EvolutionDiagramData.IconMonster>();
			for (int i = 0; i < growStepList.Length; i++)
			{
				list.AddRange(evolutionData.GetMonsterDataList(growStepList[i]));
			}
			return list;
		}

		public static List<EvolutionDiagramData.IconMonster> GetRefineMonsterData(EvolutionDiagramData evolutionData)
		{
			return evolutionData.GetMonsterDataList(GrowStep.NONE);
		}
	}
}
