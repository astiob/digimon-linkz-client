using System;
using System.Collections;

public class BattleDeadOrAlive : BattleFunctionBase
{
	public void AfterEnemyDeadFunction(params CharacterStateControl[] enemies)
	{
		foreach (CharacterStateControl characterStateControl in enemies)
		{
			if (characterStateControl.isEnemy)
			{
				base.battleStateData.currentGettedNormalChip += characterStateControl.enemyStatus.getChip;
				base.battleStateData.currentGettedNormalExp += characterStateControl.enemyStatus.getExp;
				BattleStateManager.current.uiControl.ApplyChipNumber(base.battleStateData.currentGettedChip);
				BattleStateManager.current.uiControl.ApplyExpNumber(base.battleStateData.currentGettedExp);
			}
		}
	}

	public IEnumerator OnDecisionCharacterRevivalFunction(int revivalCharacterIndex)
	{
		if (base.battleStateData.currentDigiStoneNumber < 1)
		{
			IEnumerator buyDigistoneFunction = base.stateManager.serverControl.ContinueBuyDigistoneFunction(false);
			while (buyDigistoneFunction.MoveNext())
			{
				yield return null;
			}
		}
		if (base.battleStateData.currentDigiStoneNumber >= 1)
		{
			base.battleStateData.isShowRevivalWindow = false;
			BattleStateManager.current.uiControl.ApplyEnableCharacterRevivalWindow(false, false, null);
			base.battleStateData.isRevivalReservedCharacter[revivalCharacterIndex] = true;
			AlwaysEffectParams alwaysEffect = base.battleStateData.revivalReservedEffect[revivalCharacterIndex];
			base.stateManager.threeDAction.PlayAlwaysEffectAction(alwaysEffect, AlwaysEffectState.In);
			base.stateManager.soundPlayer.TryPlaySE(alwaysEffect, AlwaysEffectState.In);
			SoundPlayer.PlayButtonEnter();
			base.battleStateData.turnUseDigiStoneCount++;
		}
		yield break;
	}
}
