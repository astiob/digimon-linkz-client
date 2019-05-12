using System;
using UnityEngine;
using UnityEngine.Csv;
using UnityEngine.Csv.IO;

public class BattleLog : BattleFunctionBase
{
	private const string exportDirectory = "/..";

	private int _battleRepeatLength = 100;

	private bool statusWrite;

	private int battleRepeat;

	private Csv _logCsvWinLost = new Csv(0, new string[]
	{
		"バトル数",
		"WAVE数",
		"勝敗",
		"不戦敗の有無",
		"獲得経験値",
		"獲得チップ"
	});

	private Csv _logCsvPlayerMonsterStatus = new Csv(0, new string[]
	{
		"名前",
		"レベル",
		"攻撃",
		"防御",
		"特殊攻撃",
		"特殊防御",
		"必殺技",
		"継承技"
	});

	private Csv _logCsvEnemyMonsterStatus = new Csv(0, new string[]
	{
		"WAVE",
		"名前",
		"レベル",
		"攻撃",
		"防御",
		"特殊攻撃",
		"特殊防御"
	});

	public bool battleStopFlag;

	public override void BattleTriggerInitialize()
	{
		if (base.isSkipAction)
		{
			global::Debug.Log("バトルシミュレートを開始しました。");
		}
	}

	public void GetBattleFinishedLogData(DataMng.ClearFlag onWin, bool onSlipOutBattle, bool isRetire)
	{
		BattleMode battleMode = base.battleMode;
		if (battleMode != BattleMode.SkipAction)
		{
			this.WriteLogSingle(onWin, onSlipOutBattle, isRetire);
		}
		else
		{
			this.WriteLogSkipAction(onWin, onSlipOutBattle, isRetire);
		}
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

	public Csv logCsvWinLost
	{
		get
		{
			return this._logCsvWinLost;
		}
		set
		{
			this._logCsvWinLost = value;
		}
	}

	public Csv logCsvPlayerMonsterStatus
	{
		get
		{
			return this._logCsvPlayerMonsterStatus;
		}
		set
		{
			this._logCsvPlayerMonsterStatus = value;
		}
	}

	public Csv logCsvEnemyMonsterStatus
	{
		get
		{
			return this._logCsvEnemyMonsterStatus;
		}
		set
		{
			this._logCsvEnemyMonsterStatus = value;
		}
	}

	public int battleRepeatLength
	{
		get
		{
			return this._battleRepeatLength;
		}
		set
		{
			this._battleRepeatLength = value;
		}
	}

	private void WriteLogSkipAction(DataMng.ClearFlag onWin, bool onSlipOutBattle, bool isRetire)
	{
		if (base.battleStateData.currentWaveNumber == 0)
		{
			this.battleRepeat++;
		}
		CsvRow csvRow;
		if (!this.statusWrite)
		{
			for (int i = 0; i < base.battleStateData.playerCharacters.Length; i++)
			{
				CharacterStateControl characterStateControl = base.battleStateData.playerCharacters[i];
				csvRow = new CsvRow(this.logCsvPlayerMonsterStatus);
				csvRow["名前"].stringValue = characterStateControl.name;
				csvRow["レベル"].intValue = characterStateControl.level;
				csvRow["攻撃"].intValue = characterStateControl.attackPower;
				csvRow["防御"].intValue = characterStateControl.defencePower;
				csvRow["特殊攻撃"].intValue = characterStateControl.specialAttackPower;
				csvRow["特殊防御"].intValue = characterStateControl.specialDefencePower;
				csvRow["必殺技"].stringValue = characterStateControl.skillIds[1];
				csvRow["継承技"].stringValue = characterStateControl.skillIds[2];
				this.logCsvPlayerMonsterStatus.AddRow(csvRow);
			}
			for (int j = 0; j < base.battleStateData.preloadEnemies.Length; j++)
			{
				for (int k = 0; k < base.battleStateData.preloadEnemies[j].Length; k++)
				{
					int num = 7;
					CharacterStateControl characterStateControl = base.battleStateData.preloadEnemies[j][k];
					if (num + characterStateControl.skillIds.Length > this.logCsvEnemyMonsterStatus.ColumnLength)
					{
						for (int l = 0; l < characterStateControl.skillIds.Length; l++)
						{
							this.logCsvEnemyMonsterStatus.AddColumn("スキル" + (this.logCsvEnemyMonsterStatus.ColumnLength - num + 1).ToString());
						}
					}
					csvRow = new CsvRow(this.logCsvEnemyMonsterStatus);
					csvRow["WAVE"].intValue = j;
					csvRow["名前"].stringValue = characterStateControl.name;
					csvRow["レベル"].intValue = characterStateControl.level;
					csvRow["攻撃"].intValue = characterStateControl.attackPower;
					csvRow["防御"].intValue = characterStateControl.defencePower;
					csvRow["特殊攻撃"].intValue = characterStateControl.specialAttackPower;
					csvRow["特殊防御"].intValue = characterStateControl.specialDefencePower;
					for (int m = 0; m < characterStateControl.skillIds.Length; m++)
					{
						csvRow["スキル" + (m + 1).ToString()].stringValue = characterStateControl.skillIds[m];
					}
					this.logCsvEnemyMonsterStatus.AddRow(csvRow);
				}
			}
			this.statusWrite = true;
		}
		int num2 = 6;
		if (base.battleStateData.itemDropResults.Count > (num2 - this.logCsvWinLost.ColumnLength) / 2)
		{
			for (int n = 0; n < base.battleStateData.itemDropResults.Count; n++)
			{
				int num3 = Mathf.RoundToInt((float)((this.logCsvWinLost.ColumnLength - num2) / 2)) + 1;
				this.logCsvWinLost.AddColumn("獲得アイテム" + num3.ToString());
				this.logCsvWinLost.AddColumn("個数" + num3.ToString());
			}
		}
		csvRow = new CsvRow(this.logCsvWinLost);
		csvRow["バトル数"].intValue = this.battleRepeat;
		csvRow["WAVE数"].intValue = base.battleStateData.currentWaveNumberGUI;
		if (onWin == DataMng.ClearFlag.Win)
		{
			csvRow["勝敗"].stringValue = "勝利";
		}
		else
		{
			csvRow["勝敗"].stringValue = "敗北";
		}
		if (base.battleStateData.unFightLoss)
		{
			csvRow["不戦敗の有無"].stringValue = "有";
		}
		else
		{
			csvRow["不戦敗の有無"].stringValue = "無";
			csvRow["獲得経験値"].intValue = base.battleStateData.currentGettedExp;
			csvRow["獲得チップ"].intValue = base.battleStateData.currentGettedChip;
			for (int num4 = 0; num4 < base.battleStateData.itemDropResults.Count; num4++)
			{
				csvRow["獲得アイテム" + (num4 + 1).ToString()].stringValue = base.battleStateData.itemDropResults[num4].dropAssetType.ToString();
				csvRow["個数" + (num4 + 1).ToString()].intValue = base.battleStateData.itemDropResults[num4].dropNumber;
			}
		}
		this.logCsvWinLost.AddRow(csvRow);
		if (this.battleRepeatLength <= this.battleRepeat && base.battleStateData.currentWaveNumber >= base.hierarchyData.batteWaves.Length - 1)
		{
			global::Debug.Log(this.logCsvPlayerMonsterStatus.ToString());
			global::Debug.Log(this.logCsvEnemyMonsterStatus.ToString());
			global::Debug.Log(this.logCsvWinLost.ToString());
			CsvFile.Export(Application.dataPath + "/..", "BattleLog_WinLost", this.logCsvWinLost, true);
			global::Debug.Log("[ " + Application.dataPath + "/../BattleLog_WinLost.csv ] にバトル勝敗記録を書き出しました。 (同じ場所にデータが既に存在する場合は上書きされます。)");
			CsvFile.Export(Application.dataPath + "/..", "BattleLog_PlayerStatus", this.logCsvPlayerMonsterStatus, true);
			global::Debug.Log("[ " + Application.dataPath + "/../BattleLog_PlayerStatus.csv ] に味方情報を書き出しました。 (同じ場所にデータが既に存在する場合は上書きされます。)");
			CsvFile.Export(Application.dataPath + "/..", "BattleLog_EnemyStatus", this.logCsvEnemyMonsterStatus, true);
			global::Debug.Log("[ " + Application.dataPath + "/../BattleLog_EnemyStatus.csv ] に敵情報を書き出しました。 (同じ場所にデータが既に存在する場合は上書きされます。)");
			global::Debug.Log("バトルシミュレートを終了しました。");
			this.battleStopFlag = true;
		}
	}
}
