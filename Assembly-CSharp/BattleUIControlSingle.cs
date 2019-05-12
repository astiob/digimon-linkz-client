using System;

public class BattleUIControlSingle : BattleUIControlBasic
{
	private new BattleUIComponentsSingle ui
	{
		get
		{
			return base.stateManager.battleUiComponents as BattleUIComponentsSingle;
		}
	}
}
