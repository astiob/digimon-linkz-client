using System;
using System.Collections;

public class SubStateRoundStartAction : BattleStateController
{
	public SubStateRoundStartAction(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override void AwakeThisState()
	{
		base.AddState(new SubStatePlayChipEffect(null, new Action<EventState>(base.SendEventState), null));
	}

	protected override void EnabledThisState()
	{
	}

	protected override IEnumerator MainRoutine()
	{
		if (!base.stateManager.onEnableTutorial)
		{
			if (base.battleStateData.currentRoundNumber <= 1)
			{
				IEnumerator gimmickWait = base.stateManager.threeDAction.PlayGimmickAnimation(base.battleStateData.playerCharacters, base.battleStateData.enemies);
				while (gimmickWait.MoveNext())
				{
					object obj = gimmickWait.Current;
					yield return obj;
				}
			}
			if (base.battleStateData.currentRoundNumber <= 1)
			{
				foreach (CharacterStateControl characterState in base.battleStateData.playerCharacters)
				{
					characterState.OnChipTrigger(ChipEffectStatus.EffectTriggerType.WaveStarted, false);
				}
				foreach (CharacterStateControl characterState2 in base.battleStateData.enemies)
				{
					characterState2.OnChipTrigger(ChipEffectStatus.EffectTriggerType.WaveStarted, false);
				}
			}
			foreach (CharacterStateControl characterState3 in base.battleStateData.playerCharacters)
			{
				characterState3.OnChipTrigger(ChipEffectStatus.EffectTriggerType.RoundStarted, false);
			}
			foreach (CharacterStateControl characterState4 in base.battleStateData.enemies)
			{
				characterState4.OnChipTrigger(ChipEffectStatus.EffectTriggerType.RoundStarted, false);
			}
			base.SetState(typeof(SubStatePlayChipEffect));
			while (base.isWaitState)
			{
				yield return null;
			}
		}
		bool playedHpRevivalSE = false;
		for (int i = 0; i < base.battleStateData.playerCharacters.Length; i++)
		{
			if (base.battleStateData.isRoundStartHpRevival[i] && !base.battleStateData.playerCharacters[i].isDied)
			{
				HitEffectParams hitEffectParams = base.battleStateData.waveChangeHpRevivalEffect[i];
				base.stateManager.threeDAction.PlayHitEffectAction(hitEffectParams, base.battleStateData.playerCharacters[i]);
				if (!playedHpRevivalSE)
				{
					playedHpRevivalSE = true;
					base.stateManager.soundPlayer.TryPlaySE(hitEffectParams);
				}
			}
		}
		bool playedApRevivalSE = false;
		CharacterStateControl[] totalCharacters = base.battleStateData.GetTotalCharacters();
		for (int j = 0; j < base.battleStateData.totalCharacterLength; j++)
		{
			if (!totalCharacters[j].isDied && base.battleStateData.isRoundStartApRevival[j])
			{
				HitEffectParams hitEffectParams2 = base.battleStateData.roundChangeApRevivalEffect[j];
				base.stateManager.threeDAction.PlayHitEffectAction(hitEffectParams2, totalCharacters[j]);
				if (!playedApRevivalSE)
				{
					playedApRevivalSE = true;
					base.stateManager.soundPlayer.TryPlaySE(hitEffectParams2);
				}
			}
		}
		base.stateManager.SetBattleScreen(BattleScreen.RoundStartActions);
		base.stateManager.threeDAction.ShowAliveCharactersAction(base.battleStateData.GetTotalCharacters());
		if (!base.battleStateData.calledBattleStartAction)
		{
			base.stateManager.threeDAction.PlayIdleAnimationActiveCharacterAction(base.battleStateData.GetTotalCharacters());
		}
		IEnumerator roundStartCameraMotionFunction = this.RoundStartCameraMotionFunction();
		while (roundStartCameraMotionFunction.MoveNext())
		{
			yield return null;
		}
		base.stateManager.soundPlayer.TryStopSE(base.battleStateData.waveChangeHpRevivalEffect);
		base.stateManager.soundPlayer.TryStopSE(base.battleStateData.roundChangeApRevivalEffect);
		for (int k = 0; k < base.battleStateData.playerCharacters.Length; k++)
		{
			base.stateManager.threeDAction.StopHitEffectAction(new HitEffectParams[]
			{
				base.battleStateData.waveChangeHpRevivalEffect[k]
			});
		}
		for (int l = 0; l < base.battleStateData.totalCharacterLength; l++)
		{
			base.stateManager.threeDAction.StopHitEffectAction(new HitEffectParams[]
			{
				base.battleStateData.roundChangeApRevivalEffect[l]
			});
		}
		if (base.battleStateData.calledBattleStartAction)
		{
			base.stateManager.threeDAction.PlayIdleAnimationActiveCharacterAction(base.battleStateData.GetTotalCharacters());
		}
		yield break;
	}

	protected virtual IEnumerator RoundStartCameraMotionFunction()
	{
		if (base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1)
		{
			base.stateManager.cameraControl.PlayCameraMotionAction("BigBoss/0002_command", base.battleStateData.stageSpawnPoint, true);
		}
		else
		{
			base.stateManager.cameraControl.PlayCameraMotionAction("0002_command", base.battleStateData.stageSpawnPoint, true);
		}
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.RoundStartActionWaitSecond, null, null);
		while (wait.MoveNext())
		{
			yield return null;
		}
		base.stateManager.cameraControl.StopCameraMotionAction("0002_command");
		base.stateManager.cameraControl.StopCameraMotionAction("BigBoss/0002_command");
		yield break;
	}
}
