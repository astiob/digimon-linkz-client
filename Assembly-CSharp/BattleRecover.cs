using System;
using System.Collections;

public class BattleRecover : BattleFunctionBase
{
	private bool isMustLoad;

	public void Save()
	{
		if (base.stateManager.onEnableTutorial)
		{
			return;
		}
		if (base.onServerConnect)
		{
			BattleDataStore instance = ClassSingleton<BattleDataStore>.Instance;
			instance.Save(base.battleStateData);
		}
	}

	private void Load()
	{
		Debug.Log("BattleRecover.Load().");
		if (base.onServerConnect)
		{
			Debug.LogFormat("onServerConnect: {0}.", new object[]
			{
				base.onServerConnect
			});
			BattleDataStore instance = ClassSingleton<BattleDataStore>.Instance;
			instance.Load(base.stateManager.battleStateData);
			base.battleStateData.beforeConfirmDigiStoneNumber = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point;
			base.battleStateData.ChangePlayerLeader(base.battleStateData.leaderCharacterIndex);
			base.battleStateData.ChangeEnemyLeader(base.battleStateData.leaderCharacterIndex);
		}
	}

	public IEnumerator CheckRecover()
	{
		Debug.Log("BattleRecover.CheckRecover.");
		if (base.stateManager.onEnableTutorial)
		{
			yield break;
		}
		Debug.LogFormat("onServerConnect: {0}.", new object[]
		{
			base.onServerConnect
		});
		if (base.onServerConnect)
		{
			BattleDataStore battleDataStore = ClassSingleton<BattleDataStore>.Instance;
			Debug.Log("battleDataStore.CheckRecoverForBattle.");
			IEnumerator wait = battleDataStore.CheckRecoverForBattle(delegate(bool result)
			{
				this.isMustLoad = result;
				Debug.LogFormat("CheckRecoverForBattleのcallback. result:{0}", new object[]
				{
					result
				});
			});
			while (wait.MoveNext())
			{
				yield return null;
			}
		}
		yield break;
	}

	public bool IsMustLoad(out bool isAfterPlayerWinner)
	{
		Debug.Log("BattleRecover.IsMustLoad.");
		isAfterPlayerWinner = false;
		Debug.LogFormat("stateManager.onEnableTutorial: {0}.", new object[]
		{
			base.stateManager.onEnableTutorial
		});
		if (base.stateManager.onEnableTutorial)
		{
			return false;
		}
		if (this.isMustLoad)
		{
			Debug.Log("isMustLoad.");
			base.hierarchyData.onEnableRandomValue = false;
			this.isMustLoad = false;
			this.Load();
			base.stateManager.uiControl.ApplyWaveAndRound(base.battleStateData.currentWaveNumber, base.battleStateData.currentRoundNumber);
			base.stateManager.uiControl.ApplyDroppedItemNumber(base.battleStateData.currentDroppedNormalItem, base.battleStateData.currentDroppedRareItem);
			base.stateManager.soundPlayer.TryPlayBGM(base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].bgmId, 0f);
			base.stateManager.waveControl.CharacterWaveReset(base.battleStateData.currentWaveNumber, true);
			base.stateManager.uiControl.CharacterHudResetAndUpdate(true);
			if (base.battleStateData.GetCharactersDeath(true) && base.stateManager.isLastBattle)
			{
				base.stateManager.log.GetBattleFinishedLogData(DataMng.ClearFlag.Win, base.stateManager.isLastBattle, base.battleStateData.isBattleRetired);
				isAfterPlayerWinner = true;
			}
			return true;
		}
		Debug.Log("isMustLoad is false.");
		return false;
	}
}
