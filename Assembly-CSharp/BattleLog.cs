using System;
using UnityEngine;

public class BattleLog : BattleFunctionBase
{
	public override void BattleTriggerInitialize()
	{
	}

	public void GetBattleFinishedLogData(DataMng.ClearFlag onWin, bool onSlipOutBattle, bool isRetire)
	{
		this.WriteLogSingle(onWin, onSlipOutBattle, isRetire);
	}

	private void WriteLogSingle(DataMng.ClearFlag onWin, bool onSlipOutBattle, bool isRetire)
	{
		if (base.onServerConnect && onSlipOutBattle)
		{
			bool[] array = new bool[base.battleStateData.playerCharacters.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = !base.battleStateData.playerCharacters[i].isDied;
			}
			int[][] array2 = new int[base.battleStateData.preloadEnemies.Length][];
			for (int j = 0; j < base.battleStateData.preloadEnemies.Length; j++)
			{
				int[] array3 = new int[base.battleStateData.preloadEnemies[j].Length];
				for (int k = 0; k < base.battleStateData.preloadEnemies[j].Length; k++)
				{
					array3[k] = ((!base.battleStateData.preloadEnemies[j][k].isDied) ? 1 : 0);
				}
				array2[j] = array3;
			}
			this.SetBattleResult(onWin, array, isRetire, array2);
		}
	}

	private void SetBattleResult(DataMng.ClearFlag onClearBattle, bool[] characterAliveFlags, bool isRetire, int[][] enemyAliveList)
	{
		DataMng.Instance().SetClearFlag(onClearBattle);
		DataMng.Instance().SetAliveFlag(characterAliveFlags);
		DataMng.Instance().SetEnemyAliveFlag(enemyAliveList);
		DataMng.Instance().AddStone(base.battleStateData.beforeConfirmDigiStoneNumber - DataMng.Instance().GetStone());
		if (!isRetire && onClearBattle != DataMng.ClearFlag.Win)
		{
			if (!base.hierarchyData.useInitialIntroduction)
			{
				return;
			}
			PlayerPrefs.SetInt("BATTLE_LOSE_COUNT", Mathf.Clamp(base.hierarchyData.initialIntroductionIndex + 1, 0, base.hierarchyData.maxInitialIntroductionIndex + 1));
			PlayerPrefs.Save();
		}
	}
}
