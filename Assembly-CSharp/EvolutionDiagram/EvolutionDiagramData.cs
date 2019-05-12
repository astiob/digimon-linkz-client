using Master;
using System;
using System.Collections.Generic;

namespace EvolutionDiagram
{
	public sealed class EvolutionDiagramData
	{
		private List<EvolutionDiagramData.IconMonster> monsterListChild;

		private List<EvolutionDiagramData.IconMonster> monsterListGrowing;

		private List<EvolutionDiagramData.IconMonster> monsterListPipe;

		private List<EvolutionDiagramData.IconMonster> monsterListPerfect;

		private List<EvolutionDiagramData.IconMonster> monsterListUltimate;

		private List<EvolutionDiagramData.IconMonster> monsterListArmor;

		private List<EvolutionDiagramData.IconMonster> monsterListAll;

		private List<EvolutionDiagramData.IconMonster> partsDataList;

		public EvolutionDiagramData()
		{
			this.monsterListChild = new List<EvolutionDiagramData.IconMonster>();
			this.monsterListGrowing = new List<EvolutionDiagramData.IconMonster>();
			this.monsterListPipe = new List<EvolutionDiagramData.IconMonster>();
			this.monsterListPerfect = new List<EvolutionDiagramData.IconMonster>();
			this.monsterListUltimate = new List<EvolutionDiagramData.IconMonster>();
			this.monsterListArmor = new List<EvolutionDiagramData.IconMonster>();
			EvolutionDiagramDataCreator.CreateMonsterDataList(this);
			this.monsterListChild.Sort(new Comparison<EvolutionDiagramData.IconMonster>(EvolutionDiagramDataCreator.CompareByCollectionId));
			this.monsterListGrowing.Sort(new Comparison<EvolutionDiagramData.IconMonster>(EvolutionDiagramDataCreator.CompareByCollectionId));
			this.monsterListPipe.Sort(new Comparison<EvolutionDiagramData.IconMonster>(EvolutionDiagramDataCreator.CompareByCollectionId));
			this.monsterListPerfect.Sort(new Comparison<EvolutionDiagramData.IconMonster>(EvolutionDiagramDataCreator.CompareByCollectionId));
			this.monsterListUltimate.Sort(new Comparison<EvolutionDiagramData.IconMonster>(EvolutionDiagramDataCreator.CompareByCollectionId));
			this.monsterListArmor.Sort(new Comparison<EvolutionDiagramData.IconMonster>(EvolutionDiagramDataCreator.CompareByCollectionId));
			this.monsterListAll = new List<EvolutionDiagramData.IconMonster>();
			this.monsterListAll.AddRange(this.monsterListChild);
			this.monsterListAll.AddRange(this.monsterListGrowing);
			this.monsterListAll.AddRange(this.monsterListPipe);
			this.monsterListAll.AddRange(this.monsterListPerfect);
			this.monsterListAll.AddRange(this.monsterListUltimate);
			this.monsterListAll.AddRange(this.monsterListArmor);
			this.monsterListAll.Sort(new Comparison<EvolutionDiagramData.IconMonster>(EvolutionDiagramDataCreator.CompareByCollectionId));
		}

		public void AddMonsterData(EvolutionDiagramData.IconMonster monsterData)
		{
			switch (MonsterData.ToGrowStepId(monsterData.groupData.growStep))
			{
			case GrowStep.CHILD_1:
			case GrowStep.CHILD_2:
				this.monsterListChild.Add(monsterData);
				break;
			case GrowStep.GROWING:
				this.monsterListGrowing.Add(monsterData);
				break;
			case GrowStep.RIPE:
				this.monsterListPipe.Add(monsterData);
				break;
			case GrowStep.PERFECT:
				this.monsterListPerfect.Add(monsterData);
				break;
			case GrowStep.ULTIMATE:
				this.monsterListUltimate.Add(monsterData);
				break;
			case GrowStep.ARMOR_1:
			case GrowStep.ARMOR_2:
				this.monsterListArmor.Add(monsterData);
				break;
			}
		}

		public List<EvolutionDiagramData.IconMonster> GetMonsterDataList(GrowStep growStepId)
		{
			List<EvolutionDiagramData.IconMonster> result;
			switch (growStepId)
			{
			case GrowStep.CHILD_1:
			case GrowStep.CHILD_2:
				result = this.monsterListChild;
				break;
			case GrowStep.GROWING:
				result = this.monsterListGrowing;
				break;
			case GrowStep.RIPE:
				result = this.monsterListPipe;
				break;
			case GrowStep.PERFECT:
				result = this.monsterListPerfect;
				break;
			case GrowStep.ULTIMATE:
				result = this.monsterListUltimate;
				break;
			case GrowStep.ARMOR_1:
			case GrowStep.ARMOR_2:
				result = this.monsterListArmor;
				break;
			default:
				result = this.monsterListAll;
				break;
			}
			return result;
		}

		public void SetViewData(GrowStep grow, List<EvolutionDiagramData.IconMonster> monsterList)
		{
			this.partsDataList = monsterList;
		}

		public List<EvolutionDiagramData.IconMonster> GetViewData()
		{
			return this.partsDataList;
		}

		public sealed class IconMonster
		{
			public int collectionId;

			public GameWebAPI.RespDataMA_GetMonsterMS.MonsterM singleData;

			public GameWebAPI.RespDataMA_GetMonsterMG.MonsterM groupData;
		}
	}
}
