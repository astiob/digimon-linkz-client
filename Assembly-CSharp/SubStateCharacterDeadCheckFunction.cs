using System;
using System.Collections;
using System.Collections.Generic;

public class SubStateCharacterDeadCheckFunction : BattleStateController
{
	private Action<bool> onEnemyDead;

	private Action<bool> onPlayerDead;

	private Action onExit;

	private bool isContinue;

	private bool isLastBattle;

	private bool? isPlayerWinner;

	public SubStateCharacterDeadCheckFunction(Action OnExit, Action<bool> OnEnemyDead, Action<bool> OnPlayerDead) : base(null, null)
	{
		this.onExit = OnExit;
		this.onEnemyDead = OnEnemyDead;
		this.onPlayerDead = OnPlayerDead;
	}

	protected override void AwakeThisState()
	{
		base.AddState(new SubStateCharacterRevivalFunction(null, new Action<EventState>(base.SendEventState)));
	}

	protected override void EnabledThisState()
	{
		this.isPlayerWinner = null;
		this.isContinue = false;
		this.isLastBattle = base.stateManager.isLastBattle;
	}

	protected override IEnumerator MainRoutine()
	{
		if (base.battleStateData.GetCharactersDeath(true))
		{
			this.isPlayerWinner = new bool?(true);
			if (!this.isLastBattle)
			{
				base.SetState(typeof(SubStateCharacterRevivalFunction));
				while (base.isWaitState)
				{
					yield return null;
				}
			}
			base.stateManager.log.GetBattleFinishedLogData(DataMng.ClearFlag.Win, this.isLastBattle, base.battleStateData.isBattleRetired);
			yield break;
		}
		if (base.battleStateData.GetCharactersDeath(false))
		{
			this.isPlayerWinner = new bool?(false);
			IEnumerator wait = this.AllPlayerCharacterDiedContinueFunction();
			while (wait.MoveNext())
			{
				object obj = wait.Current;
				yield return obj;
			}
			base.stateManager.log.GetBattleFinishedLogData(DataMng.ClearFlag.Defeat, false, base.battleStateData.isBattleRetired);
			yield break;
		}
		yield break;
	}

	protected override void DisabledThisState()
	{
		if (this.isPlayerWinner == null)
		{
			if (this.onExit != null)
			{
				this.onExit();
			}
			return;
		}
		if (this.isPlayerWinner.Value)
		{
			if (this.onEnemyDead != null)
			{
				this.onEnemyDead(!this.isLastBattle);
			}
			return;
		}
		if (this.onPlayerDead != null)
		{
			this.onPlayerDead(this.isContinue);
		}
	}

	protected override void GetEventThisState(EventState eventState)
	{
	}

	protected virtual IEnumerator AllPlayerCharacterDiedContinueFunction()
	{
		if (base.hierarchyData.isPossibleContinue)
		{
			base.stateManager.uiControl.SetMenuAuto2xButtonEnabled(false);
			base.stateManager.SetBattleScreen(BattleScreen.Continue);
			SoundPlayer.PlayMenuOpen();
			base.stateManager.threeDAction.PlayIdleAnimationUndeadCharactersAction(base.battleStateData.enemies);
			base.battleStateData.isContinueFlag = false;
			List<CharacterParams> characters = new List<CharacterParams>();
			int counted = 0;
			for (int i = 0; i < base.battleStateData.playerCharacters.Length; i++)
			{
				if (base.battleStateData.isRevivalReservedCharacter[i])
				{
					counted++;
				}
				else
				{
					characters.Add(base.battleStateData.playerCharacters[i].CharacterParams);
					base.battleStateData.isRevivalReservedCharacter[i] = true;
					base.stateManager.threeDAction.PlayAlwaysEffectAction(base.battleStateData.revivalReservedEffect[i], AlwaysEffectState.In);
				}
			}
			base.stateManager.uiControl.ApplyDigiStoneNumber(base.battleStateData.beforeConfirmDigiStoneNumber - counted);
			base.battleStateData.isShowContinueWindow = true;
			base.battleStateData.isShowRetireWindow = false;
			base.stateManager.uiControl.ApplyContinueNeedDigiStone(characters.Count + 2, base.battleStateData.beforeConfirmDigiStoneNumber - counted, false);
			string cameraKey = "0002_command";
			base.stateManager.cameraControl.PlayCameraMotionAction(cameraKey, base.battleStateData.stageSpawnPoint, true);
			base.stateManager.uiControl.ShowCharacterHUDFunction(base.battleStateData.enemies);
			while (!base.battleStateData.isContinueFlag)
			{
				yield return null;
			}
			while (base.onServerConnect && base.stateManager.values.IsNotEnoughDigistoneForContinue())
			{
				while (!base.battleStateData.isContinueFlag)
				{
					yield return null;
				}
				IEnumerator wait = base.stateManager.serverControl.ContinueBuyDigistoneFunction(true);
				while (wait.MoveNext())
				{
					object obj = wait.Current;
					yield return obj;
				}
				base.stateManager.uiControl.ApplyDigiStoneNumber(base.battleStateData.beforeConfirmDigiStoneNumber - counted);
				if (base.stateManager.values.IsNotEnoughDigistoneForContinue())
				{
					base.stateManager.uiControl.ApplyContinueNeedDigiStone(characters.Count + 2, base.battleStateData.beforeConfirmDigiStoneNumber - counted, false);
					base.battleStateData.isContinueFlag = false;
				}
			}
			base.battleStateData.turnUseDigiStoneCount = 5;
			base.stateManager.cameraControl.StopCameraMotionAction(cameraKey);
			base.stateManager.uiControl.HideCharacterHUDFunction();
			IEnumerator waitDialogClose = base.stateManager.uiControl.HideContinueDialog();
			while (waitDialogClose.MoveNext())
			{
				object obj2 = waitDialogClose.Current;
				yield return obj2;
			}
			base.battleStateData.isShowContinueWindow = false;
			base.SetState(typeof(SubStateCharacterRevivalFunction));
			while (base.isWaitState)
			{
				yield return null;
			}
			this.isContinue = true;
			yield break;
		}
		base.battleStateData.unFightLoss = true;
		for (int j = base.battleStateData.currentWaveNumber + 1; j < base.hierarchyData.batteWaves.Length; j++)
		{
			base.battleStateData.currentWaveNumber++;
			base.stateManager.uiControl.ApplyWaveAndRound(base.battleStateData.currentWaveNumber, base.battleStateData.currentRoundNumber);
			base.stateManager.log.GetBattleFinishedLogData(DataMng.ClearFlag.Defeat, true, base.battleStateData.isBattleRetired);
		}
		this.isContinue = false;
		yield break;
	}
}
