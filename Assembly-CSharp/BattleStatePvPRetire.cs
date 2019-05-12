using System;
using System.Collections;

public class BattleStatePvPRetire : BattleStateBase
{
	public BattleStatePvPRetire(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override IEnumerator MainRoutine()
	{
		IEnumerator startMsg = base.stateManager.pvpFunction.SendRetire();
		while (startMsg.MoveNext())
		{
			object obj = startMsg.Current;
			yield return obj;
		}
		yield break;
	}
}
