using EvolutionDiagram;
using System;

public sealed class DigiHouseButton : FacilityButtonSet
{
	private void OnPushedTransitionButton()
	{
		CMD_FarewellListRun.Mode = CMD_FarewellListRun.MODE.SHOW;
		GUIMain.ShowCommonDialog(null, "CMD_FarewellListRun");
	}

	private void OnPushedEvolutionDiagramButton()
	{
		CMD_EvolutionDiagram.CreateDialog(null);
	}
}
