using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleWaveControl : BattleFunctionBase
{
	public void CharacterWaveReset(int waveNumber, bool isRecover = false)
	{
		this.BattleWaveChangeResourcesCleaning();
		base.battleStateData.enemies = base.battleStateData.preloadEnemies[waveNumber];
		base.battleStateData.leaderEnemyCharacter = base.battleStateData.enemies[base.hierarchyData.leaderCharacter];
		int cameraType = base.hierarchyData.batteWaves[waveNumber].cameraType;
		string spawnPointId = ResourcesPath.ReservedID.SpawnPoints.GetSpawnPointId(base.battleStateData.enemies.Length, base.battleMode, cameraType);
		SpawnPointParams @object = base.battleStateData.preloadSpawnPoints.GetObject(spawnPointId);
		base.battleStateData.ApplySpawnPoint(@object);
		foreach (CharacterParams characterParams in base.battleStateData.preloadEnemiesParams.GetAllObject())
		{
			characterParams.gameObject.SetActive(false);
		}
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		for (int j = 0; j < base.battleStateData.enemies.Length; j++)
		{
			string prefabId = base.battleStateData.enemies[j].characterStatus.prefabId;
			int num = 0;
			if (dictionary.ContainsKey(prefabId))
			{
				Dictionary<string, int> dictionary2;
				string key;
				(dictionary2 = dictionary)[key = prefabId] = dictionary2[key] + 1;
				num = dictionary[prefabId];
			}
			else
			{
				dictionary.Add(prefabId, 0);
			}
			this.WaveResetEnemyResetupFunction(prefabId, num, j, isRecover);
			int index = base.battleStateData.playerCharacters.Length + j;
			base.stateManager.uiControl.ApplyCharacterHudBoss(index, base.hierarchyData.batteWaves[waveNumber].enemiesBossFlag[j]);
			base.stateManager.uiControl.ApplyCharacterHudContent(index, base.battleStateData.enemies[j]);
			base.stateManager.uiControl.ApplyCharacterHudReset(index);
		}
		base.stateManager.uiControl.ApplyBigBossCharacterHudBoss(false);
		base.stateManager.uiControl.ApplyBigBossCharacterHudContent(base.battleStateData.enemies[0]);
		base.stateManager.uiControl.ApplyBigBossCharacterHudReset();
		for (int k = 0; k < base.battleStateData.playerCharacters.Length; k++)
		{
			this.WaveResetPlayerResetupFunction(k);
			CharacterStateControl characterStateControl = base.battleStateData.playerCharacters[k];
			if (!characterStateControl.isDied)
			{
				if (!isRecover)
				{
					if (characterStateControl.WaveCountInitialize(base.hierarchyData.batteWaves[waveNumber].hpRevivalPercentage))
					{
						base.battleStateData.isRoundStartHpRevival[k] = true;
					}
				}
				if (waveNumber == 0)
				{
					base.stateManager.uiControl.ApplyCharacterHudBoss(k, false);
					base.stateManager.uiControl.ApplyCharacterHudContent(k, characterStateControl);
					base.stateManager.uiControl.ApplyCharacterHudReset(k);
				}
			}
		}
	}

	private void BattleWaveChangeResourcesCleaning()
	{
		Resources.UnloadUnusedAssets();
		GC.Collect();
	}

	private void WaveResetEnemyResetupFunction(string prefabId, int num, int i, bool isRecover = false)
	{
		CharacterParams @object = base.battleStateData.preloadEnemiesParams.GetObject(prefabId, num.ToString());
		@object.gameObject.SetActive(true);
		@object.Initialize(base.hierarchyData.cameraObject.camera3D);
		@object.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
		@object.transform.position = base.battleStateData.enemiesSpawnPoint[i].position;
		@object.transform.rotation = base.battleStateData.enemiesSpawnPoint[i].rotation;
		base.battleStateData.enemies[i].CharacterParams = @object;
		if (!isRecover)
		{
			base.battleStateData.enemies[i].InitializeAp();
		}
		ThreeDHoldPressButton threeDHoldPressButton = @object.gameObject.GetComponent<ThreeDHoldPressButton>();
		if (threeDHoldPressButton == null)
		{
			threeDHoldPressButton = @object.gameObject.AddComponent<ThreeDHoldPressButton>();
			threeDHoldPressButton.camera3D = base.hierarchyData.cameraObject.camera3D;
		}
		else
		{
			threeDHoldPressButton.onHoldWaitPress.Clear();
			threeDHoldPressButton.onDisengagePress.Clear();
		}
		threeDHoldPressButton.waitPressCall = 0.2f;
		BattleInputUtility.AddEvent(threeDHoldPressButton.onHoldWaitPress, new Action<int>(base.stateManager.input.OnShowEnemyDescription3D), i);
		BattleInputUtility.AddEvent(threeDHoldPressButton.onDisengagePress, new Action(base.stateManager.input.OnHideEnemyDescriotion3D));
		@object.gameObject.SetActive(false);
	}

	private void WaveResetPlayerResetupFunction(int i)
	{
		Transform transform = base.battleStateData.playerCharactersSpawnPoint[i];
		CharacterParams characterParams = base.battleStateData.playerCharacters[i].CharacterParams;
		AlwaysEffectParams alwaysEffectParams = base.battleStateData.revivalReservedEffect[i];
		characterParams.transform.position = transform.position;
		characterParams.transform.rotation = transform.rotation;
		characterParams.gameObject.SetActive(false);
		alwaysEffectParams.transform.position = transform.position;
		alwaysEffectParams.transform.rotation = transform.rotation;
	}

	public void RoundCountingFunction(CharacterStateControl[] totalCharacters, int[] apRevivals)
	{
		foreach (CharacterStateControl characterStateControl in this.GetTotalCharacters())
		{
			if (!characterStateControl.isDied)
			{
				SufferStateProperty sufferStateProperty = characterStateControl.currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.Sleep);
				if (sufferStateProperty.isActive && sufferStateProperty.GetSleepGetupOccurrence())
				{
					characterStateControl.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.Sleep, false);
				}
			}
		}
		for (int j = 0; j < totalCharacters.Length; j++)
		{
			CharacterStateControl characterStateControl2 = totalCharacters[j];
			if (!characterStateControl2.isDied)
			{
				bool flag = false;
				if (characterStateControl2.currentSufferState.FindSufferState(SufferStateProperty.SufferType.ApRevival))
				{
					int revivalAp = characterStateControl2.currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.ApRevival).GetRevivalAp(characterStateControl2.maxAp);
					characterStateControl2.ap += revivalAp;
					characterStateControl2.upAp += revivalAp;
					flag = true;
				}
				if (apRevivals != null)
				{
					int num = apRevivals[j];
					if (num > 0)
					{
						flag = true;
						characterStateControl2.ap += num;
						characterStateControl2.upAp += num;
					}
				}
				if (characterStateControl2.StartMyRoundApUp() || flag)
				{
					int num2;
					if (characterStateControl2.isEnemy)
					{
						num2 = base.battleStateData.playerCharacters.Length + characterStateControl2.myIndex;
					}
					else
					{
						num2 = characterStateControl2.myIndex;
					}
					base.battleStateData.isRoundStartApRevival[num2] = true;
				}
			}
			else
			{
				characterStateControl2.ApZero();
			}
			characterStateControl2.HateReset();
		}
		base.battleStateData.totalRoundNumber++;
		if (!base.battleStateData.GetCharactersDeath(true))
		{
			base.battleStateData.currentRoundNumber++;
		}
		base.stateManager.uiControl.ApplyWaveAndRound(base.battleStateData.currentWaveNumber, base.battleStateData.currentRoundNumber);
		foreach (CharacterStateControl characterStateControl3 in this.GetTotalCharacters())
		{
			if (!characterStateControl3.isDied)
			{
				characterStateControl3.currentSufferState.RoundUpdate();
			}
		}
	}

	private CharacterStateControl[] GetTotalCharacters()
	{
		if (base.stateManager.battleMode != BattleMode.PvP)
		{
			return base.battleStateData.GetTotalCharacters();
		}
		if (base.stateManager.pvpFunction.IsOwner)
		{
			return base.battleStateData.GetTotalCharacters();
		}
		return base.battleStateData.GetTotalCharactersEnemyFirst();
	}
}
