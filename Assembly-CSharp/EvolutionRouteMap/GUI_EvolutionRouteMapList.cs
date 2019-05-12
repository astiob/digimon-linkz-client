using EvolutionDiagram;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvolutionRouteMap
{
	public sealed class GUI_EvolutionRouteMapList : CMDRecycleViewUDWrapper
	{
		[SerializeField]
		private CMD_EvolutionRouteMap dialogRoot;

		private List<EvolutionDiagramData.IconMonster> routeDataList;

		public void OnChangeRouteMap(List<EvolutionDiagramData.IconMonster> route)
		{
			this.routeDataList = route;
			base.CreateList(route.Count);
		}

		public void OnChangeSelectMonster(EvolutionDiagramData.IconMonster monsterData)
		{
			EvolutionRouteMapData routeMapData = this.dialogRoot.GetRouteMapData();
			EvolutionDiagramData.IconMonster selectMonster = routeMapData.GetSelectMonster();
			if (selectMonster.singleData.monsterId != monsterData.singleData.monsterId)
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
