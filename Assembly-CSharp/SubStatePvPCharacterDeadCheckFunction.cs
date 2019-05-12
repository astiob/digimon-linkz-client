using System;
using System.Collections;

public class SubStatePvPCharacterDeadCheckFunction : SubStateCharacterDeadCheckFunction
{
	public SubStatePvPCharacterDeadCheckFunction(Action OnExit, Action<bool> OnEnemyDead, Action<bool> OnPlayerDead, Action<EventState> OnGotEvent) : base(OnExit, OnEnemyDead, OnPlayerDead, OnGotEvent)
	{
	}

	protected override IEnumerator MainRoutine()
	{
		CharacterStateControl actor = base.battleStateData.currentSelectCharacterState;
		if (actor != null && actor.isEnemy)
		{
			yield return base.CheckPlayerDeath();
			yield return base.CheckEnemyDeath();
		}
		else
		{
			yield return base.CheckEnemyDeath();
			yield return base.CheckPlayerDeath();
		}
		yield break;
	}

	protected override IEnumerator AllPlayerCharacterDiedContinueFunction()
	{
		yield break;
	}
}
