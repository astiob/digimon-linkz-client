using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvolutionDiagram
{
	public sealed class GUI_EvolutionDiagramIconList : CMDRecycleViewUDWrapper
	{
		[SerializeField]
		private CMD_EvolutionDiagram dialogRoot;

		private List<EvolutionDiagramData.IconMonster> evolutionMonsterList;

		public void OnChangeRefine(List<EvolutionDiagramData.IconMonster> monsterList)
		{
			this.evolutionMonsterList = monsterList;
			base.CreateList(monsterList.Count);
		}

		public List<EvolutionDiagramData.IconMonster> GetEvolutionData()
		{
			return this.evolutionMonsterList;
		}

		public CMD_EvolutionDiagram GetDialogRoot()
		{
			return this.dialogRoot;
		}
	}
}
