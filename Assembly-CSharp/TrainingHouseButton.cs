using System;

public sealed class TrainingHouseButton : FacilityButtonSet
{
	private void OnPushedTransitionButton()
	{
		GUIMain.ShowCommonDialog(null, "CMD_Succession");
	}

	private void OnPushedArousalButton()
	{
		GUIMain.ShowCommonDialog(null, "CMD_ArousalTOP");
	}
}
