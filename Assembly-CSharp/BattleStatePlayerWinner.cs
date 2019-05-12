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
		IEnumerator motionResetAliveCharacterAction = base.stateManager.threeDAction.MotionResetAliveCharacterAction(base.battleStateData.playerCharacters);
		while (motionResetAliveCharacterAction.MoveNext())
		{
			yield return null;
		}
		Action playWinMotion = delegate()
		{
			base.stateManager.threeDAction.PlayAnimationCharacterAction(CharacterAnimationType.win, new CharacterStateControl[]
			{
				base.battleStateData.playerCharacters[base.battleStateData.lastAttackPlayerCharacterIndex]
			});
		};
		IEnumerator WinMotionWaitAction = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.winActionStartMotionWaitSecond, null, playWinMotion);
		Action showNextButton = delegate()
		{
			base.stateManager.uiControl.ShowHidePlayerWinnerButton(true);
		};
		float waitTime = base.stateManager.uiProperty.winActionShowNextButtonWaitSecond;
		IEnumerator waitNextButton = base.stateManager.time.WaitForCertainPeriodTimeAction(waitTime, null, showNextButton);
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
		if (base.stateManager.battleMode != BattleMode.Single)
		{
			return;
		}
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
