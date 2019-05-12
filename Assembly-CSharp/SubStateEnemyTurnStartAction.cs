using System;
using System.Collections;

public class SubStateEnemyTurnStartAction : BattleStateController
{
	public SubStateEnemyTurnStartAction(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override void EnabledThisState()
	{
		CharacterStateControl[] totalCharacters = base.stateManager.battleStateData.GetTotalCharacters();
		base.stateManager.threeDAction.PlayIdleAnimationUndeadCharactersAction(totalCharacters);
	}

	protected override IEnumerator MainRoutine()
	{
		CharacterStateControl character = base.battleStateData.currentSelectCharacterState;
		if (!character.isEnemy)
		{
			yield break;
		}
		if (base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1)
		{
			base.stateManager.cameraControl.PlayCameraMotionAction("BigBoss/0002_enemy", base.battleStateData.stageSpawnPoint, true);
		}
		else
		{
			base.stateManager.cameraControl.PlayCameraMotionAction("0002_command", base.battleStateData.stageSpawnPoint, true);
		}
		base.stateManager.SetBattleScreen(BattleScreen.EnemyTurnAction);
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.enemyTurnStartActionWaitSecond, null, null);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		yield break;
	}

	protected override void DisabledThisState()
	{
		base.stateManager.cameraControl.StopCameraMotionAction("0002_command");
		base.stateManager.cameraControl.StopCameraMotionAction("BigBoss/0002_enemy");
	}

	protected override void GetEventThisState(EventState eventState)
	{
	}
}
