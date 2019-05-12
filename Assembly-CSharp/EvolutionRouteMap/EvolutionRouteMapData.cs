using Evolution;
using EvolutionDiagram;
using System;
using System.Collections.Generic;

namespace EvolutionRouteMap
{
	public sealed class EvolutionRouteMapData
	{
		private EvolutionDiagramData.IconMonster selectMonster;

		private List<EvolutionDiagramData.IconMonster> beforeRouteList;

		private List<EvolutionDiagramData.IconMonster> afterRouteList;

		public EvolutionRouteMapData()
		{
			this.beforeRouteList = new List<EvolutionDiagramData.IconMonster>();
			this.afterRouteList = new List<EvolutionDiagramData.IconMonster>();
		}

		private void AddMonsterData(GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monsterMaster, List<EvolutionDiagramData.IconMonster> monsterList)
		{
			GameWebAPI.RespDataMA_GetMonsterMG.MonsterM monsterGroupMasterByMonsterGroupId = MonsterDataMng.Instance().GetMonsterGroupMasterByMonsterGroupId(monsterMaster.monsterGroupId);
			if (monsterGroupMasterByMonsterGroupId != null)
			{
				EvolutionDiagramData.IconMonster item = new EvolutionDiagramData.IconMonster
				{
					collectionId = monsterGroupMasterByMonsterGroupId.monsterCollectionId.ToInt32(),
					singleData = monsterMaster,
					groupData = monsterGroupMasterByMonsterGroupId
				};
				monsterList.Add(item);
			}
		}

		private void CreateRouteList(string monsterId, string growStep)
		{
			this.beforeRouteList.Clear();
			this.afterRouteList.Clear();
			List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> list = ClassSingleton<EvolutionData>.Instance.GetBeforeMonsterEvolutionList(monsterId, growStep);
			for (int i = 0; i < list.Count; i++)
			{
				GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monsterMasterByMonsterId = MonsterDataMng.Instance().GetMonsterMasterByMonsterId(list[i].baseMonsterId);
				if (monsterMasterByMonsterId != null)
				{
					this.AddMonsterData(monsterMasterByMonsterId, this.beforeRouteList);
				}
			}
			list = ClassSingleton<EvolutionData>.Instance.GetAfterMonsterEvolutionList(monsterId, growStep);
			for (int j = 0; j < list.Count; j++)
			{
				GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monsterMasterByMonsterId2 = MonsterDataMng.Instance().GetMonsterMasterByMonsterId(list[j].nextMonsterId);
				if (monsterMasterByMonsterId2 != null)
				{
					this.AddMonsterData(monsterMasterByMonsterId2, this.afterRouteList);
				}
			}
		}

		public void SetSelectMonster(EvolutionDiagramData.IconMonster monster)
		{
			if (this.selectMonster == null || this.selectMonster.singleData.monsterId != monster.singleData.monsterId)
			{
				this.selectMonster = monster;
				this.CreateRouteList(monster.singleData.monsterId, monster.groupData.growStep);
			}
		}

		public EvolutionDiagramData.IconMonster GetSelectMonster()
		{
			return this.selectMonster;
		}

		public List<EvolutionDiagramData.IconMonster> GetBeforeRouteList()
		{
			return this.beforeRouteList;
		}

		public List<EvolutionDiagramData.IconMonster> GetAfterRouteList()
		{
			return this.afterRouteList;
		}
	}
}
