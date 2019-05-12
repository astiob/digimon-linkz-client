using System;
using System.Collections;

public class BattleStateTimeOver : BattleStateController
{
	public BattleStateTimeOver(Action OnExit) : base(null, OnExit)
	{
	}

	protected override void AwakeThisState()
	{
		base.AddState(new SubStateInitialIntroducionFunction(null));
	}

	protected override void EnabledThisState()
	{
	}

	protected override void DisabledThisState()
	{
		ClassSingleton<BattleDataStore>.Instance.DeleteForSystem();
	}

	protected override IEnumerator MainRoutine()
	{
		if (BattleStateManager.current.onServerConnect)
		{
			DataMng.Instance().WD_ReqDngResult.clearRound = 0;
		}
		base.stateManager.battleAdventureSceneManager.OnTrigger(BattleAdventureSceneManager.TriggerType.LoseStart);
		if (base.stateManager.battleAdventureSceneManager.isUpdate)
		{
			IEnumerator loseStart = base.stateManager.battleAdventureSceneManager.Update();
			while (loseStart.MoveNext())
			{
				yield return null;
			}
		}
		else
		{
			IEnumerator playerFailedAction = this.PlayerFailedAction();
			while (playerFailedAction.MoveNext())
			{
				yield return null;
			}
		}
		base.stateManager.battleAdventureSceneManager.OnTrigger(BattleAdventureSceneManager.TriggerType.LoseEnd);
		IEnumerator loseEnd = base.stateManager.battleAdventureSceneManager.Update();
		while (loseEnd.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator PlayerFailedAction()
	{
		base.stateManager.log.GetBattleFinishedLogData(DataMng.ClearFlag.Excess, true, base.battleStateData.isBattleRetired);
		base.stateManager.SetBattleScreen(BattleScreen.TimeOver);
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
		DataMng.Instance().WD_ReqDngResult.clear = 0;
		SoundPlayer.StopBattleFailBGM();
		yield break;
	}
}
