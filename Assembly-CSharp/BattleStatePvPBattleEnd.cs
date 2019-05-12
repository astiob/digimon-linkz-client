using System;
using System.Collections;

public class BattleStatePvPBattleEnd : BattleStateBase
{
	public BattleStatePvPBattleEnd(Action OnExit) : base(null, OnExit)
	{
	}

	protected override IEnumerator MainRoutine()
	{
		base.stateManager.uiControl.SetTouchEnable(true);
		base.stateManager.uiControl.HideCharacterHUDFunction();
		if (base.onServerConnect)
		{
			IEnumerator startMsg = base.stateManager.pvpFunction.BattleEndActionFunction();
			while (startMsg.MoveNext())
			{
				object obj = startMsg.Current;
				yield return obj;
			}
			yield break;
		}
		yield break;
	}

	protected override void DisabledThisState()
	{
	}
}
