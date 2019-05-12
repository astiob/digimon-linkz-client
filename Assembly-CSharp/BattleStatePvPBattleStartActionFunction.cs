using System;
using System.Collections;

public class BattleStatePvPBattleStartActionFunction : BattleStateController
{
	public BattleStatePvPBattleStartActionFunction(Action OnRoundStart, Action<EventState> OnExitGotEvent) : base(null, OnRoundStart, OnExitGotEvent)
	{
	}

	protected override void AwakeThisState()
	{
		base.AddState(new BattleStateBattleStartAction(null, new Action<EventState>(base.SendEventState)));
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
