using BattleStateMachineInternal;
using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExtension;

public class BattleRoundFunction : BattleFunctionBase
{
	private bool onStun;

	private bool onSleep;

	private bool onParalysis;

	private bool onPowerCharge;

	public BattleRoundFunction()
	{
		this.result = new bool[2];
	}

	public bool isExit { get; private set; }

	public bool[] result { get; private set; }

	public bool onSkipCharacterSelect { get; private set; }

	public bool onFreeze { get; private set; }

	public bool onConfusion { get; private set; }

	public IEnumerator RunRoundStartMessage()
	{
		base.stateManager.SetBattleScreen(BattleScreen.RoundStart);
		base.stateManager.uiControl.SetMenuAuto2xButtonEnabled(false);
		CharacterStateControl[] totalCharacters = base.battleStateData.GetTotalCharacters();
		base.stateManager.threeDAction.ShowAliveCharactersAction(totalCharacters);
		base.stateManager.threeDAction.PlayIdleAnimationActiveCharacterAction(totalCharacters);
		if (base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1)
		{
			base.stateManager.cameraControl.PlayCameraMotionAction("BigBoss/0002_command", base.battleStateData.stageSpawnPoint, true);
		}
		else
		{
			base.stateManager.cameraControl.PlayCameraMotionAction("0002_command", base.battleStateData.stageSpawnPoint, true);
		}
		if (base.battleStateData.fraudDataLogs.Count > 0)
		{
			IEnumerator fraudCheckCallApi = base.stateManager.fraudCheck.FraudCheckCallApi(base.battleStateData.fraudDataLogs.ToArray());
			while (fraudCheckCallApi.MoveNext())
			{
				object obj = fraudCheckCallApi.Current;
				yield return obj;
			}
			if (!(bool)fraudCheckCallApi.Current)
			{
				global::Debug.LogError("不正チェック通信失敗.");
			}
			base.battleStateData.fraudDataLogs.Clear();
		}
		if (base.battleStateData.isRunnedRevivalFunction)
		{
			IEnumerator characterRevivalBeforeFunction = this.CharacterRevivalBeforeFunction(base.battleStateData.revivaledCharactersIndex);
			while (characterRevivalBeforeFunction.MoveNext())
			{
				object obj2 = characterRevivalBeforeFunction.Current;
				yield return obj2;
			}
			if (!(bool)characterRevivalBeforeFunction.Current)
			{
				this.SetResult(false, false, false);
				yield break;
			}
			base.battleStateData.isRunnedRevivalFunction = false;
			if (base.stateManager.battleMode == BattleMode.Single)
			{
				base.stateManager.recover.Save();
			}
		}
		base.stateManager.uiControl.SetMenuAuto2xButtonEnabled(true);
		yield break;
	}

	private IEnumerator CharacterRevivalBeforeFunction(params int[] playerIndex)
	{
		if (base.stateManager.onEnableTutorial)
		{
			yield return true;
			yield break;
		}
		if (base.onServerConnect)
		{
			int[] continueCharacters = new int[playerIndex.Length];
			for (int i = 0; i < continueCharacters.Length; i++)
			{
				string userMonsterId = base.hierarchyData.usePlayerCharacters[playerIndex[i]].userMonsterId;
				continueCharacters[i] = int.Parse(userMonsterId);
			}
			bool getResult = false;
			bool outResult = false;
			if (base.stateManager.battleMode == BattleMode.Multi)
			{
				yield return ClassSingleton<QuestData>.Instance.DungeonContinueMulti(int.Parse(base.hierarchyData.startId), base.battleStateData.currentWaveNumberGUI, base.battleStateData.currentRoundNumber, continueCharacters, delegate(bool result)
				{
					getResult = true;
					outResult = result;
				});
			}
			else
			{
				yield return ClassSingleton<QuestData>.Instance.DungeonContinue(int.Parse(base.hierarchyData.startId), base.battleStateData.currentWaveNumberGUI, base.battleStateData.currentRoundNumber, continueCharacters, delegate(bool result)
				{
					getResult = true;
					outResult = result;
				});
			}
			while (!getResult)
			{
				yield return null;
			}
			if (outResult)
			{
				int minusCalcedStone = playerIndex.Length + ((playerIndex.Length != base.battleStateData.playerCharacters.Length) ? 0 : 2);
				DataMng.Instance().AddStone(-minusCalcedStone);
				base.battleStateData.beforeConfirmDigiStoneNumber = DataMng.Instance().GetStone();
				yield return true;
			}
			else
			{
				global::Debug.LogError("復活に失敗しました.");
				yield return false;
			}
		}
		else
		{
			int minus = playerIndex.Length + ((playerIndex.Length != base.battleStateData.playerCharacters.Length) ? 0 : 2);
			base.battleStateData.beforeConfirmDigiStoneNumber -= minus;
			yield return true;
		}
		yield break;
	}

	public void SetResult(bool IsExit, bool IsWin, bool IsNext)
	{
		this.isExit = IsExit;
		if (!this.isExit)
		{
			this.result[0] = IsWin;
			this.result[1] = IsNext;
		}
	}

	public void OnHitDamageBgmChangeFunction()
	{
		if (base.hierarchyData.batteWaves.Length <= base.battleStateData.currentWaveNumber || base.battleStateData.currentWaveNumber < 0)
		{
			return;
		}
		BattleWave battleWave = base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber];
		if (BoolExtension.AllMachValue(false, battleWave.enemiesBossFlag))
		{
			return;
		}
		if (battleWave.enemiesBossFlag.Length != base.battleStateData.enemies.Length)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < base.battleStateData.enemies.Length; i++)
		{
			if (battleWave.enemiesBossFlag[i])
			{
				num2++;
				if ((float)base.battleStateData.enemies[i].hp / (float)base.battleStateData.enemies[i].extraMaxHp < battleWave.bgmChangeHpPercentage)
				{
					num++;
				}
			}
		}
		if (num2 > num)
		{
			return;
		}
		base.stateManager.soundPlayer.TryPlayBGM(battleWave.changedBgmId, TimeExtension.GetTimeScaleDivided(base.stateManager.stateProperty.bgmCrossfadeSecond));
	}

	public void ResetPlayers()
	{
		for (int i = 0; i < base.battleStateData.isRoundStartApRevival.Length; i++)
		{
			base.battleStateData.isRoundStartApRevival[i] = false;
		}
		for (int j = 0; j < base.battleStateData.isRoundStartHpRevival.Length; j++)
		{
			base.battleStateData.isRoundStartHpRevival[j] = false;
		}
		for (int k = 0; k < base.battleStateData.playerCharacters.Length; k++)
		{
			base.battleStateData.playerCharacters[k].targetCharacter = null;
			base.battleStateData.playerCharacters[k].isSelectSkill = -1;
			base.stateManager.fraudCheck.FraudCheckOverflowMaxStatus(base.battleStateData.playerCharacters[k]);
		}
		for (int l = 0; l < base.battleStateData.enemies.Length; l++)
		{
			base.battleStateData.enemies[l].targetCharacter = null;
			base.battleStateData.enemies[l].isSelectSkill = -1;
			base.stateManager.fraudCheck.FraudCheckOverflowMaxStatus(base.battleStateData.enemies[l]);
		}
	}

	public void InitRunSufferFlags()
	{
		this.onSkipCharacterSelect = false;
		this.onFreeze = false;
		this.onStun = false;
		this.onSleep = false;
		this.onParalysis = false;
		this.onPowerCharge = false;
		this.onConfusion = false;
	}

	public void RunSufferBeforeCommand(List<CharacterStateControl> sortedCharacters, CharacterStateControl sortedCharacter)
	{
		if (sortedCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Stun))
		{
			this.onSkipCharacterSelect = true;
			this.onStun = true;
			this.onFreeze = true;
			sortedCharacter.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.Stun);
		}
		if (sortedCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Sleep))
		{
			this.onSkipCharacterSelect = true;
			this.onSleep = true;
			this.onFreeze = true;
		}
		SufferStateProperty sufferStateProperty = sortedCharacter.currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.PowerCharge);
		if (sufferStateProperty.isActive)
		{
			this.onPowerCharge = true;
			if (!sufferStateProperty.OnInvocationPowerChargeAttack)
			{
				this.onSkipCharacterSelect = true;
				this.onFreeze = true;
			}
		}
	}

	public void RunSufferAfterCommand(List<CharacterStateControl> sortedCharacters, CharacterStateControl sortedCharacter)
	{
		SufferStateProperty sufferStateProperty = sortedCharacter.currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.Paralysis);
		if (sufferStateProperty.isActive && sufferStateProperty.GetOccurrenceFreeze())
		{
			this.onFreeze = true;
			this.onParalysis = true;
		}
		SufferStateProperty sufferStateProperty2 = sortedCharacter.currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.Confusion);
		if (sufferStateProperty2.isActive && sufferStateProperty2.GetOccurrenceFreeze())
		{
			sortedCharacter.isSelectSkill = sortedCharacter.SkillIdToIndexOf(base.stateManager.publicAttackSkillId);
			this.onConfusion = true;
			CharacterStateControl characterStateControl;
			for (;;)
			{
				characterStateControl = sortedCharacters[UnityEngine.Random.Range(0, sortedCharacters.Count)];
				if (!characterStateControl.isDied)
				{
					if (sortedCharacter.isEnemy)
					{
						if (characterStateControl.isEnemy)
						{
							break;
						}
					}
					else if (!characterStateControl.isEnemy)
					{
						goto Block_8;
					}
				}
			}
			sortedCharacter.targetCharacter = characterStateControl;
			return;
			Block_8:
			sortedCharacter.targetCharacter = characterStateControl;
		}
	}

	public IEnumerator RunOnFreezAction(CharacterStateControl sortedCharacter)
	{
		if (this.onParalysis)
		{
			base.stateManager.uiControl.ApplyWarning(SufferStateProperty.SufferType.Paralysis, sortedCharacter);
		}
		else if (this.onSleep)
		{
			base.stateManager.uiControl.ApplyWarning(SufferStateProperty.SufferType.Sleep, sortedCharacter);
		}
		else if (this.onStun)
		{
			base.stateManager.uiControl.ApplyWarning(SufferStateProperty.SufferType.Stun, sortedCharacter);
		}
		else if (this.onPowerCharge)
		{
			base.stateManager.uiControl.ApplyWarning(SufferStateProperty.SufferType.PowerCharge, sortedCharacter);
		}
		base.stateManager.SetBattleScreen(BattleScreen.IsWarning);
		string cameraKey = "0006_behIncap";
		if (base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1 && sortedCharacter.isEnemy)
		{
			cameraKey = "BigBoss/skillF";
		}
		base.stateManager.threeDAction.PlayIdleAnimationCharactersAction(new CharacterStateControl[]
		{
			sortedCharacter
		});
		base.stateManager.cameraControl.PlayCameraMotionActionCharacter(cameraKey, sortedCharacter);
		IEnumerator wait2 = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.freezeActionWaitSecond, null, null);
		while (wait2.MoveNext())
		{
			yield return null;
		}
		base.stateManager.cameraControl.StopCameraMotionAction(cameraKey);
		yield break;
	}
}
