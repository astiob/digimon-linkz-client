using System;
using System.Collections;

public class BattleStateMultiPlayerFailed : BattleStateController
{
	public BattleStateMultiPlayerFailed(Action OnExit) : base(null, OnExit)
	{
	}

	protected override void AwakeThisState()
	{
		base.AddState(new SubStateInitialIntroducionFunction(null));
	}

	protected override void EnabledThisState()
	{
		base.stateManager.SetBattleScreen(BattleScreen.PlayerFailed);
		ClassSingleton<BattleDataStore>.Instance.DeleteForSystem();
	}

	protected override IEnumerator MainRoutine()
	{
		base.stateManager.uiControlMulti.HideAllDIalog();
		bool isRevivalFail = true;
		for (int i = 0; i < base.battleStateData.playerCharacters.Length; i++)
		{
			if (base.battleStateData.isRevivalReservedCharacter[i])
			{
				base.stateManager.threeDAction.PlayAlwaysEffectAction(base.battleStateData.revivalReservedEffect[i], AlwaysEffectState.Out);
				if (isRevivalFail)
				{
					base.stateManager.soundPlayer.TryPlaySE(base.battleStateData.revivalReservedEffect[i], AlwaysEffectState.Out);
				}
				isRevivalFail = false;
			}
		}
		IEnumerator wait = this.PlayerFailedAction();
		while (wait.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator PlayerFailedAction()
	{
		base.stateManager.soundPlayer.TryStopBGM();
		base.stateManager.time.SetPlaySpeed(false, false);
		CharacterStateControl[] totalCharacters = base.battleStateData.GetTotalCharacters();
		base.stateManager.threeDAction.ShowAliveCharactersAction(totalCharacters);
		base.stateManager.threeDAction.PlayIdleAnimationActiveCharacterAction(totalCharacters);
		base.stateManager.uiControl.HideCharacterHUDFunction();
		base.stateManager.cameraControl.PlayCameraMotionAction("0002_command", base.battleStateData.stageSpawnPoint, false);
		SoundPlayer.PlayBattleFailBGM();
		while (base.stateManager.cameraControl.IsPlaying("0002_command"))
		{
			yield return null;
		}
		base.SetState(typeof(SubStateInitialIntroducionFunction));
		while (base.isWaitState)
		{
			yield return null;
		}
		SoundPlayer.StopBattleFailBGM();
		yield break;
	}
}
