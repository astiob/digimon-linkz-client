using System;

public sealed class LaboratoryButton : FacilityButtonSet
{
	private void OnPushedTransitionButtonOfLaboratory()
	{
		GUIMain.ShowCommonDialog(null, "CMD_Laboratory");
	}

	private void OnPushedTransitionButtonOfDigiGarden()
	{
		GUIMain.ShowCommonDialog(null, "CMD_DigiGarden");
	}

	private void OnPushedTransitionButtonOfMedalInherit()
	{
		GUIMain.ShowCommonDialog(null, "CMD_MedalInherit");
	}
}
