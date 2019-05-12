using System;
using System.Collections;

public class SubStatePvPCharacterDeadCheckFunction : SubStateCharacterDeadCheckFunction
{
	public SubStatePvPCharacterDeadCheckFunction(Action OnExit, Action<bool> OnEnemyDead, Action<bool> OnPlayerDead) : base(OnExit, OnEnemyDead, OnPlayerDead)
	{
	}

	protected override IEnumerator AllPlayerCharacterDiedContinueFunction()
	{
		yield break;
	}
}
