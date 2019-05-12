using EvolutionDiagram;
using MonsterIcon;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvolutionRouteMap
{
	public sealed class GUI_EvolutionRouteMapList : CMDRecycleViewUDWrapper
	{
		private const int RECYCLE_SECTOR_NUM = 1;

		[SerializeField]
		private CMD_EvolutionRouteMap dialogRoot;

		private List<EvolutionDiagramData.IconMonster> routeDataList;

		public void Initialize()
		{
			base.InitializeView(1);
		}

		public MonsterIcon GetMonsterIconObject()
		{
			return this.dialogRoot.GetMonsterIconObject();
		}

		public void OnChangeRouteMap(List<EvolutionDiagramData.IconMonster> route)
		{
			this.routeDataList = route;
			base.CreateList(route.Count);
		}

		public void OnChangeSelectMonster(EvolutionDiagramData.IconMonster monsterData)
		{
			EvolutionRouteMapData routeMapData = this.dialogRoot.GetRouteMapData();
			EvolutionDiagramData.IconMonster selectMonster = routeMapData.GetSelectMonster();
			if (selectMonster.master.Simple.monsterId != monsterData.master.Simple.monsterId)
			{
				routeMapData.SetSelectMonster(monsterData);
				this.dialogRoot.UpdateSelectMonster();
				this.dialogRoot.UpdateViewList();
			}
		}

		public List<EvolutionDiagramData.IconMonster> GetRouteMapData()
		{
			return this.routeDataList;
		}
	}
}
