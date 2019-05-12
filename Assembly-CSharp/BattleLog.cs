using System;

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
			base.stateManager.serverControl.SetBattleResult(onWin, array, isRetire, array2);
		}
	}
}
