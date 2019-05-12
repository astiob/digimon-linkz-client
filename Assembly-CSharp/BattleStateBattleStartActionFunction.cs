using System;
using System.Collections;

public class BattleStateBattleStartActionFunction : BattleStateController
{
	private Action onPlayerWinner;

	private Action onRoundStart;

	private bool isPlayerWinnerFlag;

	public BattleStateBattleStartActionFunction(Action OnRoundStart, Action OnPlayerWinner) : base(null, null)
	{
		this.onPlayerWinner = OnPlayerWinner;
		this.onRoundStart = OnRoundStart;
	}

	protected override void AwakeThisState()
	{
		base.AddState(new BattleStateBattleStartAction(null, null));
		base.AddState(new BattleStateWaveController(null, null));
	}

	protected override void EnabledThisState()
	{
		this.isPlayerWinnerFlag = false;
	}

	protected override IEnumerator MainRoutine()
	{
		bool isAfterPlayerWinner = false;
		bool useRoundStartAction = true;
		if (base.stateManager.recover.IsMustLoad(out isAfterPlayerWinner))
		{
			Debug.Log("stateManager.recover.IsMustLoad is true.");
			useRoundStartAction = false;
			this.isPlayerWinnerFlag = isAfterPlayerWinner;
			if (!this.isPlayerWinnerFlag)
			{
				int count = 0;
				for (int i = 0; i < base.battleStateData.enemies.Length; i++)
				{
					if (base.battleStateData.enemies[i].isDied)
					{
						count++;
					}
				}
				if (base.battleStateData.enemies.Length == count)
				{
					base.SetState(typeof(BattleStateWaveController));
					while (base.isWaitState)
					{
						yield return null;
					}
					useRoundStartAction = true;
				}
			}
			base.hierarchyData.stageParams.TransformStage(base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType);
			CharacterStateControl[] totalCharacters = base.battleStateData.GetTotalCharacters();
			base.stateManager.threeDAction.ShowAliveCharactersAction(totalCharacters);
			base.stateManager.threeDAction.PlayIdleAnimationActiveCharacterAction(totalCharacters);
		}
		if (useRoundStartAction)
		{
			base.SetState(typeof(BattleStateBattleStartAction));
			while (base.isWaitState)
			{
				yield return null;
			}
		}
		yield break;
	}

	protected override void DisabledThisState()
	{
		if (this.isPlayerWinnerFlag && this.onPlayerWinner != null)
		{
			this.onPlayerWinner();
		}
		else if (!this.isPlayerWinnerFlag && this.onRoundStart != null)
		{
			this.onRoundStart();
		}
	}
}
