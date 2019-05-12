using System;
using System.Collections;

public class SubStatePvPCharacterDeadCheckFunction : SubStateCharacterDeadCheckFunction
{
	public SubStatePvPCharacterDeadCheckFunction(Action OnExit, Action<bool> OnEnemyDead, Action<bool> OnPlayerDead, Action<EventState> OnGotEvent) : base(OnExit, OnEnemyDead, OnPlayerDead, OnGotEvent)
	{
	}

	protected override IEnumerator AllPlayerCharacterDiedContinueFunction()
	{
		yield break;
	}
}
