﻿using System;
using System.Collections;

public class BattleStateMultiTimeOver : BattleStateController
{
	public BattleStateMultiTimeOver(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override void AwakeThisState()
	{
		base.AddState(new SubStateInitialIntroducionFunction(null));
	}

	protected override void EnabledThisState()
	{
		base.stateManager.SetBattleScreen(BattleScreen.TimeOver);
		ClassSingleton<BattleDataStore>.Instance.DeleteForSystem();
	}

	protected override IEnumerator MainRoutine()
	{
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
		if (BattleStateManager.current.onServerConnect)
		{
			DataMng.Instance().WD_ReqDngResult.clearRound = 0;
		}
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
		IEnumerator wait = base.stateManager.multiFunction.SendTimeOut();
		while (wait.MoveNext())
		{
			yield return null;
		}
		SoundPlayer.StopBattleFailBGM();
		yield break;
	}
}
