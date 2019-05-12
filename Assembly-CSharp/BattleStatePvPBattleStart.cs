using System;
using System.Collections;

public class BattleStatePvPBattleStart : BattleStateBase
{
	public BattleStatePvPBattleStart(Action OnExit) : base(null, OnExit)
	{
	}

	protected override IEnumerator MainRoutine()
	{
		yield break;
	}
}
