using MonsterIcon;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvolutionDiagram
{
	public sealed class GUI_EvolutionDiagramIconList : CMDRecycleViewUDWrapper
	{
		[SerializeField]
		private CMD_EvolutionDiagram dialogRoot;

		private const int RECYCLE_SECTOR_NUM = 2;

		private List<EvolutionDiagramData.IconMonster> evolutionMonsterList;

		private MonsterIcon monsterIconSource;

		public void Initialize()
		{
			this.monsterIconSource = MonsterIconFactory.CreateIcon(1);
			base.InitializeView(2);
		}

		public MonsterIcon GetMonsterIconObject()
		{
			return MonsterIconFactory.Copy(this.monsterIconSource);
		}

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
