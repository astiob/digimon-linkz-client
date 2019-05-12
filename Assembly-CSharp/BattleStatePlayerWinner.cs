using System;
using System.Collections;
using UnityEngine;

public class BattleStatePlayerWinner : BattleStateController
{
	public BattleStatePlayerWinner(Action OnExit) : base(null, OnExit)
	{
	}

	protected override void EnabledThisState()
	{
		base.battleStateData.isEnableBackKeySystem = false;
		base.stateManager.uiControl.ShowHidePlayerWinnerButton(false);
	}

	protected override IEnumerator MainRoutine()
	{
		base.stateManager.SetBattleScreen(BattleScreen.PlayerWinner);
		base.stateManager.time.SetPlaySpeed(false, false);
		IEnumerator wait = this.PlayerWinnerAction();
		while (wait.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator PlayerWinnerAction()
	{
		if (base.battleMode != BattleMode.PvP && base.battleStateData.totalRoundNumber <= base.hierarchyData.speedClearRound && base.hierarchyData.speedClearRound > 0)
		{
			base.stateManager.battleUiComponents.playerWinnerUi.SpeedClearObjActive(true);
		}
		this.SaveBattleMenuSettings();
		base.battleStateData.isSkipWinnerAction = false;
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
		Action showNextButton = delegate()
		{
			base.stateManager.uiControl.ShowHidePlayerWinnerButton(true);
		};
		IEnumerator waitNextButton = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.uiProperty.winActionShowNextButtonWaitSecond, null, showNextButton);
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
		while (waitNextButton.MoveNext())
		{
			WinMotionWaitAction.MoveNext();
			yield return null;
		}
		while (!base.battleStateData.isSkipWinnerAction)
		{
			WinMotionWaitAction.MoveNext();
			yield return null;
		}
		SoundPlayer.StopBattleWinBGM();
		yield break;
	}

	private void SaveBattleMenuSettings()
	{
		if (base.hierarchyData.on2xSpeedPlay)
		{
			PlayerPrefs.SetInt("Battle2xSpeedPlay", 1);
		}
		else
		{
			PlayerPrefs.SetInt("Battle2xSpeedPlay", 0);
		}
		if (base.hierarchyData.onAutoPlay != 0)
		{
			PlayerPrefs.SetInt("BattleAutoPlay", base.hierarchyData.onAutoPlay);
		}
		else
		{
			PlayerPrefs.SetInt("BattleAutoPlay", 0);
		}
		PlayerPrefs.Save();
	}
}
