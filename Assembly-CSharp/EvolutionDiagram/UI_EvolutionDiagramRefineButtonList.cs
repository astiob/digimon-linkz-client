using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvolutionDiagram
{
	public sealed class UI_EvolutionDiagramRefineButtonList : MonoBehaviour
	{
		[SerializeField]
		private CMD_EvolutionDiagram dialogRoot;

		[SerializeField]
		private List<GUI_EvolutionDiagramRefineButton> buttonList;

		[SerializeField]
		private GUI_EvolutionDiagramRefineButton defaultSelectButton;

		public void Initialize()
		{
			for (int i = 0; i < this.buttonList.Count; i++)
			{
				if (null != this.buttonList[i])
				{
					this.buttonList[i].Initialize();
				}
			}
			this.SetButtonState(this.defaultSelectButton);
		}

		public GrowStep GetDefaultSelectGrowStep()
		{
			return this.defaultSelectButton.GetGrowStep();
		}

		public void SetSingleGrowData(GrowStep growStep)
		{
			EvolutionDiagramData evolutionData = this.dialogRoot.GetEvolutionData();
			List<EvolutionDiagramData.IconMonster> refineMonsterData = EvolutionDiagramRefine.GetRefineMonsterData(evolutionData, growStep);
			evolutionData.SetViewData(growStep, refineMonsterData);
			this.dialogRoot.UpdateViewList();
		}

		public void SetButtonState(GUI_EvolutionDiagramRefineButton selectButton)
		{
			selectButton.SetSelected();
			for (int i = 0; i < this.buttonList.Count; i++)
			{
				if (this.buttonList[i] != selectButton)
				{
					this.buttonList[i].SetGrayOut();
				}
			}
		}
	}
}
