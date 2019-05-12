using Monster;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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

		private List<EvolutionDiagramData.IconMonster> monsterListHybrid;

		private List<EvolutionDiagramData.IconMonster> monsterListAll;

		private List<EvolutionDiagramData.IconMonster> partsDataList;

		[CompilerGenerated]
		private static Comparison<EvolutionDiagramData.IconMonster> <>f__mg$cache0;

		[CompilerGenerated]
		private static Comparison<EvolutionDiagramData.IconMonster> <>f__mg$cache1;

		[CompilerGenerated]
		private static Comparison<EvolutionDiagramData.IconMonster> <>f__mg$cache2;

		[CompilerGenerated]
		private static Comparison<EvolutionDiagramData.IconMonster> <>f__mg$cache3;

		[CompilerGenerated]
		private static Comparison<EvolutionDiagramData.IconMonster> <>f__mg$cache4;

		[CompilerGenerated]
		private static Comparison<EvolutionDiagramData.IconMonster> <>f__mg$cache5;

		[CompilerGenerated]
		private static Comparison<EvolutionDiagramData.IconMonster> <>f__mg$cache6;

		[CompilerGenerated]
		private static Comparison<EvolutionDiagramData.IconMonster> <>f__mg$cache7;

		public EvolutionDiagramData()
		{
			this.monsterListChild = new List<EvolutionDiagramData.IconMonster>();
			this.monsterListGrowing = new List<EvolutionDiagramData.IconMonster>();
			this.monsterListPipe = new List<EvolutionDiagramData.IconMonster>();
			this.monsterListPerfect = new List<EvolutionDiagramData.IconMonster>();
			this.monsterListUltimate = new List<EvolutionDiagramData.IconMonster>();
			this.monsterListArmor = new List<EvolutionDiagramData.IconMonster>();
			this.monsterListHybrid = new List<EvolutionDiagramData.IconMonster>();
			EvolutionDiagramDataCreator.CreateMonsterDataList(this);
			List<EvolutionDiagramData.IconMonster> list = this.monsterListChild;
			if (EvolutionDiagramData.<>f__mg$cache0 == null)
			{
				EvolutionDiagramData.<>f__mg$cache0 = new Comparison<EvolutionDiagramData.IconMonster>(EvolutionDiagramDataCreator.CompareByCollectionId);
			}
			list.Sort(EvolutionDiagramData.<>f__mg$cache0);
			List<EvolutionDiagramData.IconMonster> list2 = this.monsterListGrowing;
			if (EvolutionDiagramData.<>f__mg$cache1 == null)
			{
				EvolutionDiagramData.<>f__mg$cache1 = new Comparison<EvolutionDiagramData.IconMonster>(EvolutionDiagramDataCreator.CompareByCollectionId);
			}
			list2.Sort(EvolutionDiagramData.<>f__mg$cache1);
			List<EvolutionDiagramData.IconMonster> list3 = this.monsterListPipe;
			if (EvolutionDiagramData.<>f__mg$cache2 == null)
			{
				EvolutionDiagramData.<>f__mg$cache2 = new Comparison<EvolutionDiagramData.IconMonster>(EvolutionDiagramDataCreator.CompareByCollectionId);
			}
			list3.Sort(EvolutionDiagramData.<>f__mg$cache2);
			List<EvolutionDiagramData.IconMonster> list4 = this.monsterListPerfect;
			if (EvolutionDiagramData.<>f__mg$cache3 == null)
			{
				EvolutionDiagramData.<>f__mg$cache3 = new Comparison<EvolutionDiagramData.IconMonster>(EvolutionDiagramDataCreator.CompareByCollectionId);
			}
			list4.Sort(EvolutionDiagramData.<>f__mg$cache3);
			List<EvolutionDiagramData.IconMonster> list5 = this.monsterListUltimate;
			if (EvolutionDiagramData.<>f__mg$cache4 == null)
			{
				EvolutionDiagramData.<>f__mg$cache4 = new Comparison<EvolutionDiagramData.IconMonster>(EvolutionDiagramDataCreator.CompareByCollectionId);
			}
			list5.Sort(EvolutionDiagramData.<>f__mg$cache4);
			List<EvolutionDiagramData.IconMonster> list6 = this.monsterListArmor;
			if (EvolutionDiagramData.<>f__mg$cache5 == null)
			{
				EvolutionDiagramData.<>f__mg$cache5 = new Comparison<EvolutionDiagramData.IconMonster>(EvolutionDiagramDataCreator.CompareByCollectionId);
			}
			list6.Sort(EvolutionDiagramData.<>f__mg$cache5);
			List<EvolutionDiagramData.IconMonster> list7 = this.monsterListHybrid;
			if (EvolutionDiagramData.<>f__mg$cache6 == null)
			{
				EvolutionDiagramData.<>f__mg$cache6 = new Comparison<EvolutionDiagramData.IconMonster>(EvolutionDiagramDataCreator.CompareByCollectionId);
			}
			list7.Sort(EvolutionDiagramData.<>f__mg$cache6);
			this.monsterListAll = new List<EvolutionDiagramData.IconMonster>();
			this.monsterListAll.AddRange(this.monsterListChild);
			this.monsterListAll.AddRange(this.monsterListGrowing);
			this.monsterListAll.AddRange(this.monsterListPipe);
			this.monsterListAll.AddRange(this.monsterListPerfect);
			this.monsterListAll.AddRange(this.monsterListUltimate);
			this.monsterListAll.AddRange(this.monsterListArmor);
			this.monsterListAll.AddRange(this.monsterListHybrid);
			List<EvolutionDiagramData.IconMonster> list8 = this.monsterListAll;
			if (EvolutionDiagramData.<>f__mg$cache7 == null)
			{
				EvolutionDiagramData.<>f__mg$cache7 = new Comparison<EvolutionDiagramData.IconMonster>(EvolutionDiagramDataCreator.CompareByCollectionId);
			}
			list8.Sort(EvolutionDiagramData.<>f__mg$cache7);
		}

		public void AddMonsterData(EvolutionDiagramData.IconMonster monsterData)
		{
			switch (MonsterGrowStepData.ToGrowStep(monsterData.master.Group.growStep))
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
			case GrowStep.HYBRID_GROWING:
			case GrowStep.HYBRID_RIPE:
			case GrowStep.HYBRID_PERFECT:
			case GrowStep.HYBRID_ULTIMATE:
				this.monsterListHybrid.Add(monsterData);
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
			case GrowStep.HYBRID_GROWING:
			case GrowStep.HYBRID_RIPE:
			case GrowStep.HYBRID_PERFECT:
			case GrowStep.HYBRID_ULTIMATE:
				result = this.monsterListHybrid;
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

			public MonsterClientMaster master;
		}
	}
}
