using System;

public class BattleStateWaveController : BattleStateBase
{
	public BattleStateWaveController(Action OnExit, Action<EventState> OnExitGotEvent = null) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected virtual bool isPose
	{
		get
		{
			return base.battleStateData.isShowMenuWindow;
		}
	}

	protected override void EnabledThisState()
	{
		this.SetNextWave();
		base.stateManager.uiControl.ApplyDroppedItemNumber(base.battleStateData.currentDroppedNormalItem, base.battleStateData.currentDroppedRareItem);
		base.stateManager.time.SetPlaySpeed(base.hierarchyData.on2xSpeedPlay, this.isPose);
		base.stateManager.uiControl.HideCharacterHUDFunction();
		base.stateManager.threeDAction.PlayIdleAnimationUndeadCharactersAction(base.battleStateData.playerCharacters);
		base.stateManager.threeDAction.PlayIdleAnimationUndeadCharactersAction(base.battleStateData.enemies);
	}

	private void SetNextWave()
	{
		if (base.battleStateData.enemies != null)
		{
			foreach (CharacterStateControl characterStateControl in base.battleStateData.enemies)
			{
				if (characterStateControl.CharacterParams != null)
				{
					characterStateControl.CharacterParams.gameObject.SetActive(false);
				}
			}
			base.stateManager.threeDAction.StopHitEffectAction(base.battleStateData.enemiesDeathEffect);
			base.stateManager.threeDAction.StopHitEffectAction(base.battleStateData.enemiesLastDeadEffect);
		}
		base.battleStateData.currentRoundNumber = 1;
		base.battleStateData.currentWaveNumber++;
		base.stateManager.uiControl.ApplyWaveAndRound(base.battleStateData.currentWaveNumber, base.battleStateData.currentRoundNumber);
		base.stateManager.waveControl.CharacterWaveReset(base.battleStateData.currentWaveNumber, false);
	}
}
