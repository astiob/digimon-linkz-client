using System;
using System.Collections;

public class SubStatePvPRoundStartAction : SubStateRoundStartAction
{
	public SubStatePvPRoundStartAction(Action OnExit, Action<EventState> OnExitGotEvent) : base(OnExit, OnExitGotEvent)
	{
	}

	protected override IEnumerator RoundStartCameraMotionFunction()
	{
		yield break;
	}
}
