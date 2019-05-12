using System;

public sealed class LaboratoryButton : FacilityButtonSet
{
	private void OnPushedTransitionButtonOfLaboratory()
	{
		GUIMain.ShowCommonDialog(null, "CMD_Laboratory", null);
	}

	private void OnPushedTransitionButtonOfDigiGarden()
	{
		GUIMain.ShowCommonDialog(null, "CMD_DigiGarden", null);
	}

	private void OnPushedTransitionButtonOfMedalInherit()
	{
		GUIMain.ShowCommonDialog(null, "CMD_MedalInherit", null);
	}

	private void OnPushedTransitionButtonOfVersionUP()
	{
		GUIMain.ShowCommonDialog(null, "CMD_VersionUP", null);
	}
}
