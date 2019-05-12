using System;
using System.Collections;
using UnityEngine;

public class BattleStateMultiPlayerWinner : BattleStateController
{
	public BattleStateMultiPlayerWinner(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override IEnumerator MainRoutine()
	{
		IEnumerator[] functions = new IEnumerator[]
		{
			this.PlayAdventureScene(BattleAdventureSceneManager.TriggerType.WinStart),
			this.PlayerWinnerAction(),
			this.PlayAdventureScene(BattleAdventureSceneManager.TriggerType.WinEnd)
		};
		foreach (IEnumerator function in functions)
		{
			while (function.MoveNext())
			{
				yield return null;
			}
		}
		yield break;
	}

	private IEnumerator PlayAdventureScene(BattleAdventureSceneManager.TriggerType triggerType)
	{
		base.stateManager.battleAdventureSceneManager.OnTrigger(triggerType);
		IEnumerator update = base.stateManager.battleAdventureSceneManager.Update();
		while (update.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator PlayerWinnerAction()
	{
		base.stateManager.SetBattleScreen(BattleScreen.PlayerWinner);
		base.stateManager.time.SetPlaySpeed(false, false);
		base.stateManager.uiControlMulti.HideAllDIalog();
		base.stateManager.uiControlMulti.ShowWinnerUI();
		base.stateManager.uiControl.ShowHidePlayerWinnerButton(false);
		base.battleStateData.isSkipWinnerAction = false;
		if (base.battleStateData.totalRoundNumber <= base.hierarchyData.speedClearRound && base.hierarchyData.speedClearRound > 0)
		{
			base.stateManager.battleUiComponents.playerWinnerUi.SpeedClearObjActive(true);
		}
		float startTime = Time.time;
		base.stateManager.soundPlayer.TryStopBGM();
		foreach (AlwaysEffectParams a in base.battleStateData.revivalReservedEffect)
		{
			a.gameObject.SetActive(false);
		}
		base.stateManager.threeDAction.ShowAliveCharactersAction(base.battleStateData.GetTotalCharacters());
		base.stateManager.threeDAction.PlayIdleAnimationActiveCharacterAction(base.battleStateData.playerCharacters);
		Action playWinMotion = delegate()
		{
			base.stateManager.threeDAction.PlaySmoothAnimationCharacterAction(CharacterAnimationType.win, new CharacterStateControl[]
			{
				base.battleStateData.playerCharacters[base.battleStateData.lastAttackPlayerCharacterIndex]
			});
		};
		IEnumerator WinMotionWaitAction = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.winActionStartMotionWaitSecond, null, playWinMotion);
		IEnumerator waitNextButton = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.uiProperty.winActionShowNextButtonWaitSecond, null, null);
		IEnumerator wait = base.stateManager.threeDAction.MotionResetAliveCharacterAction(new CharacterStateControl[]
		{
			base.battleStateData.playerCharacters[base.battleStateData.lastAttackPlayerCharacterIndex]
		});
		while (wait.MoveNext())
		{
			yield return null;
		}
		base.stateManager.cameraControl.StopCameraMotionAction("0007_commandCharaView");
		base.stateManager.cameraControl.StopCameraMotionAction("BigBoss/0007_commandCharaView");
		base.stateManager.cameraControl.StopCameraMotionAction("0002_command");
		CharacterStateControl cameraTargetCharacter = base.battleStateData.playerCharacters[base.battleStateData.lastAttackPlayerCharacterIndex];
		if (cameraTargetCharacter.isDied)
		{
			for (int i = 0; i < base.battleStateData.playerCharacters.Length; i++)
			{
				if (!base.battleStateData.playerCharacters[i].isDied)
				{
					cameraTargetCharacter = base.battleStateData.playerCharacters[i];
				}
			}
		}
		base.battleStateData.winCameraMotionInternalResources.StartAutoChange(cameraTargetCharacter.CharacterParams);
		SoundPlayer.PlayBattleWinBGM();
		if (base.stateManager.onServerConnect)
		{
			IEnumerator sendClearResult = base.stateManager.multiFunction.SendClearResult(null);
			while (waitNextButton.MoveNext() || sendClearResult.MoveNext())
			{
				WinMotionWaitAction.MoveNext();
				yield return null;
			}
		}
		else
		{
			while (waitNextButton.MoveNext())
			{
				WinMotionWaitAction.MoveNext();
				yield return null;
			}
		}
		base.stateManager.uiControl.ShowHidePlayerWinnerButton(true);
		while (!base.battleStateData.isSkipWinnerAction)
		{
			WinMotionWaitAction.MoveNext();
			yield return null;
		}
		float endTime = Time.time;
		while (endTime - startTime < 3f)
		{
			endTime = Time.time;
			yield return null;
		}
		SoundPlayer.StopBattleWinBGM();
		yield break;
	}
}
