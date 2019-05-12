using System;

public sealed class MeatFarmButton : FacilityButtonSet
{
	private void OnPushedMealButton()
	{
		CMD_BaseSelect.BaseType = CMD_BaseSelect.BASE_TYPE.MEAL;
		GUIMain.ShowCommonDialog(null, "CMD_BaseSelect");
	}
}
