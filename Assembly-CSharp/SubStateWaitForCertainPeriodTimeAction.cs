using System;
using System.Collections;

public class SubStateWaitForCertainPeriodTimeAction : BattleStateController
{
	public SubStateWaitForCertainPeriodTimeAction(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override void EnabledThisState()
	{
	}

	protected override IEnumerator MainRoutine()
	{
		yield break;
	}
}
