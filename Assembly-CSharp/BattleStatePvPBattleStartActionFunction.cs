using System;
using System.Collections;

public class BattleStatePvPBattleStartActionFunction : BattleStateController
{
	public BattleStatePvPBattleStartActionFunction(Action OnRoundStart) : base(null, OnRoundStart)
	{
	}

	protected override void AwakeThisState()
	{
		base.AddState(new BattleStateBattleStartAction(null, null));
	}

	protected override IEnumerator MainRoutine()
	{
		base.SetState(typeof(BattleStateBattleStartAction));
		while (base.isWaitState)
		{
			yield return null;
		}
		yield break;
	}
}
