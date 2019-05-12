using System;

public class ExchangeButton : FacilityButtonSet
{
	private void OnPushedTransitionButtonOfExchange()
	{
		GUIMain.ShowCommonDialog(null, "CMD_ClearingHouseTOP");
	}
}
