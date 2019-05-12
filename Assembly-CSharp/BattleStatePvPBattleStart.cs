using System;
using System.Collections;

public class BattleStatePvPBattleStart : BattleStateBase
{
	public BattleStatePvPBattleStart(Action OnExit) : base(null, OnExit)
	{
	}

	protected override IEnumerator MainRoutine()
	{
		if (base.onServerConnect)
		{
			IEnumerator startMsg = base.stateManager.pvpFunction.BattleStartActionFunction();
			while (startMsg.MoveNext())
			{
				yield return null;
			}
			yield break;
		}
		yield break;
	}
}
