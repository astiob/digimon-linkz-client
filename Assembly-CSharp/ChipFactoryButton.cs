using System;

public class ChipFactoryButton : FacilityButtonSet
{
	private void OnPushedChipAttachment()
	{
		CMD_BaseSelect.CreateChipChipInstalling(null);
	}

	private void OnPushedChipReinforcement()
	{
		CMD_ChipReinforcement.Create(null);
	}

	private void OnPushedChipAdministration()
	{
		CMD_ChipAdministration.Create(null);
	}
}
