using System;

public class BattleValues : BattleFunctionBase
{
	public bool IsNotEnoughDigistoneForContinue()
	{
		return base.battleStateData.beforeConfirmDigiStoneNumber < base.battleStateData.playerCharacters.Length + 2;
	}
}
