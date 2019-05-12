using BattleStateMachineInternal;
using System;
using System.Collections;
using System.Collections.Generic;
using TextureTimeScrollInternal;
using UnityEngine;

public class BattleStateInitialize : BattleStateController
{
	public BattleStateInitialize(Action OnExit, Action<EventState> OnExitGotEvent = null) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected virtual string enemySpawnPoint1
	{
		get
		{
			return "0001_enemiesOne_small";
		}
	}

	protected virtual string enemySpawnPoint2
	{
		get
		{
			return "0002_enemiesTwo_small";
		}
	}

	protected virtual string enemySpawnPoint3
	{
		get
		{
			return "0003_enemiesThree_small";
		}
	}

	protected virtual string insertPlayerPath
	{
		get
		{
			return "insertCharacterEffect";
		}
	}

	protected virtual string insertEnemyPath
	{
		get
		{
			return "insertCharacterEffect";
		}
	}

	protected override void AwakeThisState()
	{
		base.battleStateData.isEnableBackKeySystem = false;
	}

	private void SetActiveHierarcyRendering(bool enable)
	{
		base.hierarchyData.cameraObject.sunLightColorChanger.enabled = enable;
		base.hierarchyData.cameraObject.sunLight.enabled = enable;
		base.hierarchyData.cameraObject.camera3D.enabled = enable;
	}

	private IEnumerator LoadBeforeInitializeUI()
	{
		BattleDebug.Log("ロード開始前UI初期化: 開始");
		IEnumerator initializeUI = base.stateManager.uiControl.BeforeInitializeUI();
		while (initializeUI.MoveNext())
		{
			yield return null;
		}
		BattleDebug.Log("ロード開始前UI初期化: 完了");
		yield break;
	}

	protected virtual IEnumerator CheckRecover()
	{
		IEnumerator wait = base.stateManager.recover.CheckRecover();
		while (wait.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator LoadResources()
	{
		List<IEnumerator> LoadObjectStore = new List<IEnumerator>();
		LoadObjectStore.Add(base.stateManager.initialize.LoadBattleInternalResources("WinCameraMotion", delegate(GameObject g)
		{
			base.battleStateData.winCameraMotionInternalResources = g.GetComponent<BattleCameraSwitcher>();
			base.battleStateData.winCameraMotionInternalResources.Initialize(base.hierarchyData.cameraObject.camera3D);
		}));
		LoadObjectStore.Add(base.stateManager.initialize.LoadBattleInternalResources("CommandSelectCameraMotion", delegate(GameObject g)
		{
			base.battleStateData.normalCommandSelectTweenTargetCamera = g.GetComponent<TweenCameraTargetFunction>();
			base.battleStateData.normalCommandSelectTweenTargetCamera.isFoolowUp = false;
			base.battleStateData.normalCommandSelectTweenTargetCamera.Initialize(base.hierarchyData.cameraObject.camera3D);
			base.battleStateData.commandSelectTweenTargetCamera = base.battleStateData.normalCommandSelectTweenTargetCamera;
		}));
		bool isBigBoss = false;
		foreach (BattleWave battleWave in base.hierarchyData.batteWaves)
		{
			if (battleWave.cameraType == 1)
			{
				isBigBoss = true;
				break;
			}
		}
		if (isBigBoss)
		{
			LoadObjectStore.Add(base.stateManager.initialize.LoadBattleInternalResources("BigBossCommandSelectCameraMotion", delegate(GameObject g)
			{
				base.battleStateData.bigBossCommandSelectTweenTargetCamera = g.GetComponent<TweenCameraTargetFunction>();
				base.battleStateData.bigBossCommandSelectTweenTargetCamera.isFoolowUp = false;
				base.battleStateData.bigBossCommandSelectTweenTargetCamera.Initialize(base.hierarchyData.cameraObject.camera3D);
			}));
		}
		if (base.hierarchyData.onInstanceStage)
		{
			LoadObjectStore.Add(base.stateManager.initialize.LoadStage());
		}
		LoadObjectStore.Add(base.stateManager.initialize.LoadSpawnPoint(this.enemySpawnPoint1));
		LoadObjectStore.Add(base.stateManager.initialize.LoadSpawnPoint(this.enemySpawnPoint2));
		LoadObjectStore.Add(base.stateManager.initialize.LoadSpawnPoint(this.enemySpawnPoint3));
		if (isBigBoss)
		{
			LoadObjectStore.Add(base.stateManager.initialize.LoadSpawnPoint("0001_enemiesOne_small_bigboss"));
		}
		BattleDebug.Log("ステージ / 内部リソースのロード: 開始");
		IEnumerator LoadDataStore = base.stateManager.initialize.LoadCoroutine(LoadObjectStore.ToArray());
		while (LoadDataStore.MoveNext())
		{
			yield return null;
		}
		BattleDebug.Log("ステージ / 内部リソースのロード: 完了");
		yield break;
	}

	private IEnumerator LoadCommonEffect()
	{
		int maxEnemiesLength = 0;
		int maxBossLength = 0;
		for (int i = 0; i < base.hierarchyData.batteWaves.Length; i++)
		{
			maxEnemiesLength = Mathf.Max(maxEnemiesLength, base.hierarchyData.batteWaves[i].useEnemiesId.Length);
			int currentBossLength = 0;
			foreach (bool boss in base.hierarchyData.batteWaves[i].enemiesBossFlag)
			{
				if (boss)
				{
					currentBossLength++;
				}
			}
			maxBossLength = Mathf.Max(maxBossLength, currentBossLength);
		}
		int playerLength = base.hierarchyData.usePlayerCharactersId.Length;
		int maxDigimonLength = playerLength + maxEnemiesLength;
		List<IEnumerator> list = new List<IEnumerator>();
		base.battleStateData.roundChangeApRevivalEffect = new HitEffectParams[maxDigimonLength];
		base.battleStateData.isRoundStartApRevival = new bool[maxDigimonLength];
		for (int j = 0; j < maxDigimonLength; j++)
		{
			string key = "EFF_COM_APUP";
			Action<HitEffectParams, int> callback = delegate(HitEffectParams h, int index)
			{
				base.battleStateData.roundChangeApRevivalEffect[index] = h;
			};
			IEnumerator hitEffect = base.stateManager.initialize.LoadHitEffect(key, j, callback);
			list.Add(hitEffect);
		}
		base.battleStateData.stageGimmickUpEffect = new AlwaysEffectParams[maxDigimonLength];
		base.battleStateData.stageGimmickDownEffect = new AlwaysEffectParams[maxDigimonLength];
		if (!base.stateManager.onEnableTutorial)
		{
			for (int k = 0; k < maxDigimonLength; k++)
			{
				string upKey = "EFF_COM_GimmickUP";
				Action<AlwaysEffectParams, int> upCallback = delegate(AlwaysEffectParams res, int index)
				{
					base.battleStateData.stageGimmickUpEffect[index] = res;
				};
				IEnumerator upAlwaysEffect = base.stateManager.initialize.LoadAlwaysEffect(upKey, k, upCallback);
				list.Add(upAlwaysEffect);
				string downKey = "EFF_COM_GimmickDOWN";
				Action<AlwaysEffectParams, int> downCallback = delegate(AlwaysEffectParams res, int index)
				{
					base.battleStateData.stageGimmickDownEffect[index] = res;
				};
				IEnumerator downAlwaysEffect = base.stateManager.initialize.LoadAlwaysEffect(downKey, k, downCallback);
				list.Add(downAlwaysEffect);
			}
			for (int l = 0; l < base.hierarchyData.extraEffectsId.Length; l++)
			{
				ExtraEffectStatus extraEffectStatus = base.stateManager.serverControl.GetExtraEffectStatus(base.hierarchyData.extraEffectsId[l]);
				base.battleStateData.extraEffectStatus.Add(extraEffectStatus);
			}
		}
		BattleDebug.Log("共通エフェクトのロード: 開始");
		IEnumerator LoadDataStore = base.stateManager.initialize.LoadCoroutine(list.ToArray());
		while (LoadDataStore.MoveNext())
		{
			yield return null;
		}
		BattleDebug.Log("共通エフェクトのロード: 完了");
		yield break;
	}

	private IEnumerator LoadPlayer()
	{
		int playerLength = base.hierarchyData.usePlayerCharactersId.Length;
		List<IEnumerator> list = new List<IEnumerator>();
		base.battleStateData.playerCharacters = new CharacterStateControl[playerLength];
		base.battleStateData.revivalReservedEffect = new AlwaysEffectParams[playerLength];
		base.battleStateData.isRevivalReservedCharacter = new bool[playerLength];
		base.battleStateData.revivalEffect = new HitEffectParams[playerLength];
		base.battleStateData.waveChangeHpRevivalEffect = new HitEffectParams[playerLength];
		base.battleStateData.isRoundStartHpRevival = new bool[playerLength];
		base.battleStateData.playersDeathEffect = new HitEffectParams[playerLength];
		LeaderSkillStatus leaderSkillStatus = null;
		string leaderCharacter = base.hierarchyData.usePlayerCharactersId[base.hierarchyData.leaderCharacter];
		CharacterStatus getStatusLeader = this.GetPlayerStatus(leaderCharacter);
		if (getStatusLeader.isHavingLeaderSkill)
		{
			leaderSkillStatus = base.stateManager.serverControl.GetLeaderSkillStatus(getStatusLeader.leaderSkillId);
		}
		CharacterDatas leaderCharacterData = base.stateManager.serverControl.GetCharacterData(getStatusLeader.prefabId);
		for (int i = 0; i < playerLength; i++)
		{
			CharacterStatus getStatus = this.GetPlayerStatus(base.hierarchyData.usePlayerCharactersId[i]);
			base.battleStateData.playerCharacters[i] = base.stateManager.initialize.LoadCharacterStateControl(getStatus, leaderSkillStatus, leaderCharacterData, false);
			base.battleStateData.playerCharacters[i].myIndex = i;
			base.battleStateData.isRevivalReservedCharacter[i] = false;
			Action<CharacterParams, int, string> idCallback = delegate(CharacterParams c, int index, string id)
			{
				base.battleStateData.playerCharacters[index].CharacterParams = c;
			};
			IEnumerator idCharacterParam = base.stateManager.initialize.LoadCharacterParam(getStatus.prefabId, i, idCallback);
			list.Add(idCharacterParam);
			string revivalReservedKey = "revivalReservationEffect";
			Action<AlwaysEffectParams, int> revivalReservedCallback = delegate(AlwaysEffectParams a, int id)
			{
				base.battleStateData.revivalReservedEffect[id] = a;
			};
			IEnumerator revivalReservedAlwaysEffect = base.stateManager.initialize.LoadAlwaysEffect(revivalReservedKey, i, revivalReservedCallback);
			list.Add(revivalReservedAlwaysEffect);
			string death = "EFF_COM_DEATH";
			Action<HitEffectParams, int> deathCallback = delegate(HitEffectParams h, int index)
			{
				base.battleStateData.playersDeathEffect[index] = h;
			};
			IEnumerator deathHitEffect = base.stateManager.initialize.LoadHitEffect(death, i, deathCallback);
			list.Add(deathHitEffect);
			string revival = "EFF_COM_L_HEAL";
			Action<HitEffectParams, int> revivalCallback = delegate(HitEffectParams h, int index)
			{
				base.battleStateData.revivalEffect[index] = h;
			};
			IEnumerator revivalHitEffect = base.stateManager.initialize.LoadHitEffect(revival, i, revivalCallback);
			list.Add(revivalHitEffect);
			string waveHpRevival = "EFF_COM_L_HEAL";
			Action<HitEffectParams, int> waveHpRevivalCallback = delegate(HitEffectParams h, int index)
			{
				base.battleStateData.waveChangeHpRevivalEffect[index] = h;
			};
			IEnumerator waveHpRevivalHitEffect = base.stateManager.initialize.LoadHitEffect(waveHpRevival, i, waveHpRevivalCallback);
			list.Add(waveHpRevivalHitEffect);
		}
		base.battleStateData.leaderCharacter = base.battleStateData.playerCharacters[base.hierarchyData.leaderCharacter];
		base.battleStateData.leaderCharacter.isLeader = true;
		base.battleStateData.leaderCharacter.LeaderInitialize();
		base.battleStateData.insertCharacterEffect = new AlwaysEffectParams[playerLength];
		for (int j = 0; j < playerLength; j++)
		{
			string key = this.insertPlayerPath;
			Action<AlwaysEffectParams, int> callback = delegate(AlwaysEffectParams res, int index)
			{
				base.battleStateData.insertCharacterEffect[index] = res;
			};
			IEnumerator alwaysEffect = base.stateManager.initialize.LoadAlwaysEffect(key, j, callback);
			list.Add(alwaysEffect);
		}
		BattleDebug.Log("プレイヤーキャラクタ / 関連エフェクトのロード: 開始");
		IEnumerator LoadDataStore = base.stateManager.initialize.LoadCoroutine(list.ToArray());
		while (LoadDataStore.MoveNext())
		{
			yield return null;
		}
		base.stateManager.system.IfFullMemoryCallGC();
		BattleDebug.Log("プレイヤーキャラクタ / 関連エフェクトのロード: 完了");
		yield break;
	}

	private IEnumerator LoadEnemy()
	{
		int maxEnemiesLength = 0;
		int maxBossLength = 0;
		for (int i = 0; i < base.hierarchyData.batteWaves.Length; i++)
		{
			maxEnemiesLength = Mathf.Max(maxEnemiesLength, base.hierarchyData.batteWaves[i].useEnemiesId.Length);
			int currentBossLength = 0;
			foreach (bool boss in base.hierarchyData.batteWaves[i].enemiesBossFlag)
			{
				if (boss)
				{
					currentBossLength++;
				}
			}
			maxBossLength = Mathf.Max(maxBossLength, currentBossLength);
		}
		List<IEnumerator> list = new List<IEnumerator>();
		LeaderSkillStatus enemyLeaderSkillStatus = null;
		CharacterStatus getStatusEnemyLeader = this.GetEnemyStatus(base.hierarchyData.batteWaves[0].useEnemiesId[base.hierarchyData.leaderCharacter]);
		if (getStatusEnemyLeader.isHavingLeaderSkill)
		{
			enemyLeaderSkillStatus = base.stateManager.serverControl.GetLeaderSkillStatus(getStatusEnemyLeader.leaderSkillId);
		}
		CharacterDatas enemyLeaderCharacterData = base.stateManager.serverControl.GetCharacterData(getStatusEnemyLeader.prefabId);
		Dictionary<string, int> characterParamsIdListBattleWave = new Dictionary<string, int>();
		base.battleStateData.preloadEnemies = new CharacterStateControl[base.hierarchyData.batteWaves.Length][];
		for (int i2 = 0; i2 < base.battleStateData.preloadEnemies.Length; i2++)
		{
			base.battleStateData.preloadEnemies[i2] = new CharacterStateControl[base.hierarchyData.batteWaves[i2].useEnemiesId.Length];
			Dictionary<string, int> characterParamsIdListBattleWaveCurrent = new Dictionary<string, int>();
			for (int j = 0; j < base.battleStateData.preloadEnemies[i2].Length; j++)
			{
				CharacterStatus getStatus = this.GetEnemyStatus(base.hierarchyData.batteWaves[i2].useEnemiesId[j]);
				base.battleStateData.preloadEnemies[i2][j] = base.stateManager.initialize.LoadCharacterStateControl(getStatus, enemyLeaderSkillStatus, enemyLeaderCharacterData, true);
				base.battleStateData.preloadEnemies[i2][j].myIndex = j;
				if (j == base.hierarchyData.leaderCharacter)
				{
					base.battleStateData.preloadEnemies[i2][j].isLeader = true;
				}
				string prefabId = getStatus.prefabId;
				if (!characterParamsIdListBattleWaveCurrent.ContainsKey(prefabId))
				{
					characterParamsIdListBattleWaveCurrent.Add(prefabId, 1);
				}
				else
				{
					Dictionary<string, int> dictionary2;
					Dictionary<string, int> dictionary = dictionary2 = characterParamsIdListBattleWaveCurrent;
					string key4;
					string key3 = key4 = prefabId;
					int num2 = dictionary2[key4];
					dictionary[key3] = num2 + 1;
				}
			}
			foreach (KeyValuePair<string, int> d in characterParamsIdListBattleWaveCurrent)
			{
				if (!characterParamsIdListBattleWave.ContainsKey(d.Key))
				{
					characterParamsIdListBattleWave.Add(d.Key, d.Value);
				}
				if (characterParamsIdListBattleWave[d.Key] < d.Value)
				{
					characterParamsIdListBattleWave[d.Key] = d.Value;
				}
			}
		}
		foreach (KeyValuePair<string, int> d2 in characterParamsIdListBattleWave)
		{
			for (int k = 0; k < d2.Value; k++)
			{
				Action<CharacterParams, int, string> callback = delegate(CharacterParams c, int index, string id)
				{
					base.battleStateData.preloadEnemiesParams.Add(id, index.ToString(), c);
				};
				IEnumerator characterParam = base.stateManager.initialize.LoadCharacterParam(d2.Key, k, callback);
				list.Add(characterParam);
			}
		}
		base.battleStateData.enemiesDeathEffect = new HitEffectParams[maxEnemiesLength];
		base.battleStateData.enemiesLastDeadEffect = new HitEffectParams[maxEnemiesLength];
		base.battleStateData.droppingItemNormalEffect = new AlwaysEffectParams[base.battleStateData.maxDropNum];
		base.battleStateData.droppingItemRareEffect = new AlwaysEffectParams[base.battleStateData.maxDropNum];
		for (int l = 0; l < maxEnemiesLength; l++)
		{
			string deathKey = "EFF_COM_DEATH";
			Action<HitEffectParams, int> deathCallback = delegate(HitEffectParams h, int index)
			{
				base.battleStateData.enemiesDeathEffect[index] = h;
			};
			IEnumerator deathHitEffect = base.stateManager.initialize.LoadHitEffect(deathKey, l, deathCallback);
			list.Add(deathHitEffect);
			string deathLastKey = "EFF_COM_BOSSDEATH";
			Action<HitEffectParams, int> deathLastCallback = delegate(HitEffectParams h, int index)
			{
				base.battleStateData.enemiesLastDeadEffect[index] = h;
			};
			IEnumerator deathLastHitEffect = base.stateManager.initialize.LoadHitEffect(deathLastKey, l, deathLastCallback);
			list.Add(deathLastHitEffect);
		}
		for (int m = 0; m < base.battleStateData.maxDropNum; m++)
		{
			string droppingItemNormalKey = "droppingItemEffectNormal";
			Action<AlwaysEffectParams, int> droppingItemNormalCallback = delegate(AlwaysEffectParams a, int index)
			{
				base.battleStateData.droppingItemNormalEffect[index] = a;
			};
			IEnumerator droppingItemNormalAlwaysEffect = base.stateManager.initialize.LoadAlwaysEffect(droppingItemNormalKey, m, droppingItemNormalCallback);
			list.Add(droppingItemNormalAlwaysEffect);
			string droppingItemRareKey = "droppingItemEffectRare";
			Action<AlwaysEffectParams, int> droppingItemRareCallback = delegate(AlwaysEffectParams a, int index)
			{
				base.battleStateData.droppingItemRareEffect[index] = a;
			};
			IEnumerator droppingItemRareAlwaysEffect = base.stateManager.initialize.LoadAlwaysEffect(droppingItemRareKey, m, droppingItemRareCallback);
			list.Add(droppingItemRareAlwaysEffect);
		}
		base.battleStateData.leaderEnemyCharacter = base.battleStateData.enemies[base.hierarchyData.leaderCharacter];
		base.battleStateData.insertEnemyEffect = new AlwaysEffectParams[maxEnemiesLength];
		for (int n = 0; n < maxEnemiesLength; n++)
		{
			string key = this.insertEnemyPath;
			Action<AlwaysEffectParams, int> callback2 = delegate(AlwaysEffectParams res, int index)
			{
				base.battleStateData.insertEnemyEffect[index] = res;
			};
			IEnumerator alwaysEffect = base.stateManager.initialize.LoadAlwaysEffect(key, n, callback2);
			list.Add(alwaysEffect);
		}
		base.battleStateData.insertBossCharacterEffect = new AlwaysEffectParams[maxBossLength];
		for (int i3 = 0; i3 < maxBossLength; i3++)
		{
			string key2 = "insertBossCharacterEffect";
			Action<AlwaysEffectParams, int> callback3 = delegate(AlwaysEffectParams res, int index)
			{
				base.battleStateData.insertBossCharacterEffect[index] = res;
			};
			IEnumerator alwaysEffect2 = base.stateManager.initialize.LoadAlwaysEffect(key2, i3, callback3);
			list.Add(alwaysEffect2);
		}
		BattleDebug.Log("敵キャラクタ / 関連エフェクトのロード: 開始");
		IEnumerator LoadDataStore = base.stateManager.initialize.LoadCoroutine(list.ToArray());
		while (LoadDataStore.MoveNext())
		{
			yield return null;
		}
		base.stateManager.system.IfFullMemoryCallGC();
		BattleDebug.Log("敵キャラクタ / 関連エフェクトのロード: 完了");
		yield break;
	}

	protected virtual CharacterStatus GetPlayerStatus(string id)
	{
		return base.stateManager.serverControl.GetPlayerStatus(id);
	}

	protected virtual CharacterStatus GetEnemyStatus(string id)
	{
		return base.stateManager.serverControl.GetEnemyStatus(id);
	}

	private IEnumerator LoadSkill()
	{
		List<CharacterStateControl> digimonList = new List<CharacterStateControl>();
		for (int i = 0; i < base.battleStateData.playerCharacters.Length; i++)
		{
			digimonList.Add(base.battleStateData.playerCharacters[i]);
		}
		for (int j = 0; j < base.battleStateData.preloadEnemies.Length; j++)
		{
			for (int i2 = 0; i2 < base.battleStateData.preloadEnemies[j].Length; i2++)
			{
				digimonList.Add(base.battleStateData.preloadEnemies[j][i2]);
			}
		}
		List<string> skillList = new List<string>();
		foreach (CharacterStateControl digimon in digimonList)
		{
			for (int k = 0; k < digimon.GetSkillLength(); k++)
			{
				if (!skillList.Contains(digimon.skillIds[k]))
				{
					skillList.Add(digimon.skillIds[k]);
				}
			}
			foreach (int chipId in digimon.chipIds)
			{
				GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects = ChipDataMng.GetChipEffectData(chipId.ToString());
				if (chipEffects != null)
				{
					foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in chipEffects)
					{
						int effectType = chipEffect.effectType.ToInt32();
						string effectValue = chipEffect.effectValue;
						if ((effectType == 60 || (effectType == 56 && effectType > 0)) && !skillList.Contains(effectValue))
						{
							skillList.Add(effectValue);
							if (!base.stateManager.serverControl._cachedSkillStatus.ContainsKey(effectValue))
							{
								SkillStatus skillStatus = base.stateManager.serverControl.SkillMToSkillStatus(effectValue);
								base.stateManager.serverControl._cachedSkillStatus.Add(effectValue, skillStatus);
							}
						}
					}
				}
			}
		}
		if (base.battleStateData.extraEffectStatus != null)
		{
			foreach (ExtraEffectStatus extraEffectStatus in base.battleStateData.extraEffectStatus)
			{
				int effectType2 = extraEffectStatus.EffectType;
				string effectValue2 = extraEffectStatus.EffectValue.ToString();
				if (effectType2 == 60 && !skillList.Contains(effectValue2))
				{
					skillList.Add(effectValue2);
					if (!base.stateManager.serverControl._cachedSkillStatus.ContainsKey(effectValue2))
					{
						SkillStatus skillStatus2 = base.stateManager.serverControl.SkillMToSkillStatus(effectValue2);
						base.stateManager.serverControl._cachedSkillStatus.Add(effectValue2, skillStatus2);
					}
				}
			}
		}
		List<SkillStatus> skillStatusTemp = new List<SkillStatus>();
		List<string> skillInvocation = new List<string>();
		List<string> skillPassive = new List<string>();
		for (int l = 0; l < skillList.Count; l++)
		{
			SkillStatus skillStatus3 = base.stateManager.serverControl.GetSkillStatus(skillList[l]);
			if (skillStatus3 != null)
			{
				skillStatusTemp.Add(skillStatus3);
				string invocationId = "none";
				string passiveId = "none";
				SkillType skillType = skillStatus3.skillType;
				if (skillType != SkillType.Deathblow)
				{
					if (skillType == SkillType.InheritanceTechnique)
					{
						invocationId = "EFF_COM_SKILL";
						passiveId = skillStatus3.prefabId;
					}
				}
				else
				{
					invocationId = skillStatus3.prefabId;
					passiveId = "none";
				}
				base.battleStateData.skillStatus.Add(skillList[l], skillStatus3);
				if (!skillInvocation.Contains(invocationId))
				{
					skillInvocation.Add(invocationId);
				}
				if (!skillPassive.Contains(passiveId))
				{
					skillPassive.Add(passiveId);
				}
				base.stateManager.soundPlayer.AddEffectSe(skillStatus3.seId);
			}
		}
		List<IEnumerator> cameraMotions = base.stateManager.cameraControl.LoadCameraMotions();
		BattleDebug.Log("カメラ演出のロード: 開始");
		IEnumerator LoadDataStore = base.stateManager.initialize.LoadCoroutine(cameraMotions.ToArray());
		while (LoadDataStore.MoveNext())
		{
			yield return null;
		}
		BattleDebug.Log("カメラ演出のロード: 完了");
		List<IEnumerator> effectStore = new List<IEnumerator>();
		UnityObjectPooler<InvocationEffectParams> invocationEffectParamsTemp = new UnityObjectPooler<InvocationEffectParams>();
		for (int m = 0; m < skillInvocation.Count; m++)
		{
			string key = skillInvocation[m];
			Action<InvocationEffectParams, string> callback = delegate(InvocationEffectParams p, string id)
			{
				invocationEffectParamsTemp.Add(id, p);
			};
			IEnumerator invocationEffect = base.stateManager.initialize.LoadInvocationEffect(key, callback);
			effectStore.Add(invocationEffect);
		}
		UnityObjectPooler<PassiveEffectParams[]> passiveEffectParamsTemp = new UnityObjectPooler<PassiveEffectParams[]>();
		for (int n = 0; n < skillPassive.Count; n++)
		{
			string key2 = skillPassive[n];
			Action<PassiveEffectParams[], string> callback2 = delegate(PassiveEffectParams[] p, string id)
			{
				passiveEffectParamsTemp.Add(id, p);
			};
			IEnumerator passiveEffect = base.stateManager.initialize.LoadPassiveEffect(key2, callback2);
			effectStore.Add(passiveEffect);
		}
		BattleDebug.Log("スキルエフェクト演出のロード: 開始");
		LoadDataStore = base.stateManager.initialize.LoadCoroutine(effectStore.ToArray());
		while (LoadDataStore.MoveNext())
		{
			yield return null;
		}
		base.stateManager.system.IfFullMemoryCallGC();
		BattleDebug.Log("スキルエフェクト演出のロード: 完了");
		for (int i3 = 0; i3 < skillStatusTemp.Count; i3++)
		{
			string invocationId2 = "none";
			string passiveId2 = "none";
			SkillType skillType = skillStatusTemp[i3].skillType;
			if (skillType != SkillType.Deathblow)
			{
				if (skillType == SkillType.InheritanceTechnique)
				{
					invocationId2 = "EFF_COM_SKILL";
					passiveId2 = skillStatusTemp[i3].prefabId;
				}
			}
			else
			{
				invocationId2 = skillStatusTemp[i3].prefabId;
				passiveId2 = "none";
			}
			skillStatusTemp[i3].invocationEffectParams = invocationEffectParamsTemp.GetObject(invocationId2);
			skillStatusTemp[i3].passiveEffectParams = passiveEffectParamsTemp.GetObject(passiveId2);
		}
		foreach (CharacterStateControl digimon2 in digimonList)
		{
			for (int i4 = 0; i4 < digimon2.skillIds.Length; i4++)
			{
				string skillId = digimon2.skillIds[i4];
				digimon2.skillStatus[i4] = base.battleStateData.skillStatus.GetObject(skillId);
			}
			digimon2.InitializeSkillExtraStatus();
		}
		List<IEnumerator> hitEffectStore = base.stateManager.initialize.LoadHitEffectsByFlags(skillStatusTemp.ToArray());
		List<string> invocationEffectKeys = new List<string>();
		InvocationEffectParams[] invocationEffectParams = invocationEffectParamsTemp.GetAllObject();
		foreach (InvocationEffectParams invocationEffectParam in invocationEffectParams)
		{
			if (!BattleFunctionUtility.IsEmptyPath(invocationEffectParam.cameraMotionId) && !invocationEffectKeys.Contains(invocationEffectParam.cameraMotionId))
			{
				string key3 = invocationEffectParam.cameraMotionId;
				IEnumerator cameraMotion = base.stateManager.cameraControl.LoadCameraMotion(key3, null);
				hitEffectStore.Add(cameraMotion);
				invocationEffectKeys.Add(key3);
			}
		}
		BattleDebug.Log("スキルエフェクトカメラのロード: 開始");
		LoadDataStore = base.stateManager.initialize.LoadCoroutine(hitEffectStore.ToArray());
		while (LoadDataStore.MoveNext())
		{
			yield return null;
		}
		BattleDebug.Log("スキルエフェクトカメラのロード: 完了");
		base.stateManager.soundPlayer.LoadSound();
		yield break;
	}

	protected virtual IEnumerator LoadAfterInitializeUI()
	{
		base.stateManager.uiControl.ApplyChipNumber(base.battleStateData.currentGettedChip);
		base.stateManager.uiControl.ApplyExpNumber(base.battleStateData.currentGettedExp);
		BattleDebug.Log("ロード完了後UI初期化: 開始");
		IEnumerator afterInitializeUI = base.stateManager.uiControl.AfterInitializeUI();
		while (afterInitializeUI.MoveNext())
		{
			yield return null;
		}
		BattleDebug.Log("ロード完了後UI初期化: 完了");
		if (PlayerPrefs.GetInt("Battle2xSpeedPlay", 0) == 1)
		{
			base.hierarchyData.on2xSpeedPlay = true;
		}
		int autoFlag = PlayerPrefs.GetInt("BattleAutoPlay", 0);
		if (0 < autoFlag)
		{
			if (autoFlag == 1)
			{
				base.hierarchyData.onAutoPlay = 1;
			}
			else if (autoFlag == 2)
			{
				base.hierarchyData.onAutoPlay = 2;
			}
		}
		base.stateManager.uiControl.ApplyHideHitIcon();
		base.stateManager.uiControl.ApplyDroppedItemHide();
		base.stateManager.uiControl.ApplyAutoPlay(base.hierarchyData.onAutoPlay);
		base.stateManager.uiControl.Apply2xPlay(base.hierarchyData.on2xSpeedPlay);
		base.stateManager.uiControl.ApplyAreaName(base.hierarchyData.areaName);
		bool sleep = base.hierarchyData.onAutoPlay > 0;
		base.stateManager.sleep.SetSleepOff(sleep);
		yield break;
	}

	private IEnumerator InitSound()
	{
		bool isPlayingBGM = base.stateManager.soundManager.IsPlayingBGM;
		Action<int> stopBGMEnd = delegate(int i)
		{
			isPlayingBGM = false;
		};
		if (base.stateManager.soundManager.IsPlayingBGM)
		{
			base.stateManager.soundManager.StopBGM(1.5f, stopBGMEnd);
		}
		while (isPlayingBGM)
		{
			yield return null;
		}
		base.stateManager.soundManager.ReleaseAudio();
		yield break;
	}

	protected override IEnumerator MainRoutine()
	{
		BattleObjectPooler.CallInitialize();
		BattleObjectPooler.isCheckEnable = false;
		TextureTimeScrollRealTime.TimeReset();
		base.stateManager.serverControl.SetLoadingImage(true);
		this.SetActiveHierarcyRendering(false);
		base.stateManager.initialize.InitializeRoots();
		base.battleStateData.beforeConfirmDigiStoneNumber = base.hierarchyData.digiStoneNumber;
		List<IEnumerator> functionList = new List<IEnumerator>();
		functionList.Add(this.LoadBeforeInitializeUI());
		functionList.Add(this.InitSound());
		functionList.Add(this.CheckRecover());
		functionList.Add(this.LoadResources());
		functionList.Add(this.LoadPlayer());
		functionList.Add(this.LoadEnemy());
		functionList.Add(this.LoadCommonEffect());
		functionList.Add(this.LoadSkill());
		functionList.Add(this.LoadAfterInitializeUI());
		Action loading = base.stateManager.battleUiComponents.initializeUi.GetLoadingInvoke(functionList.Count);
		foreach (IEnumerator function in functionList)
		{
			while (function.MoveNext())
			{
				yield return null;
			}
			loading();
			yield return new WaitForEndOfFrame();
		}
		for (int i = 0; i < base.hierarchyData.usePlayerCharactersId.Length; i++)
		{
			bool isLeader = i == base.hierarchyData.leaderCharacter;
			CharacterStateControl playerCharacter = base.battleStateData.playerCharacters[i];
			Sprite characterThumbnail = base.stateManager.serverControl.GetCharacterThumbnail(playerCharacter.playerStatus.thumbnailId);
			base.stateManager.uiControl.ApplyMonsterButtonIcon(i, characterThumbnail, playerCharacter, isLeader);
		}
		if (base.onServerConnect)
		{
			base.stateManager.serverControl._cachedPlayerStatus.Clear();
			base.stateManager.serverControl._cachedEnemyStatus.Clear();
			base.stateManager.serverControl._cachedCharacterDatas.Clear();
			base.stateManager.serverControl._cachedLeaderSkillStatus.Clear();
			base.stateManager.serverControl._cachedTolerance.Clear();
			base.stateManager.serverControl._cachedLeaderSkillM.Clear();
			base.stateManager.serverControl._cachedExtraEffectStatus.Clear();
		}
		BattleDebug.Log("--- バトルGC : 開始");
		Resources.UnloadUnusedAssets();
		GC.Collect();
		BattleDebug.Log("--- バトルGC : 完了");
		this.SetActiveHierarcyRendering(true);
		base.stateManager.serverControl.SetLoadingImage(false);
		base.stateManager.uiControl.SetTouchEnable(true);
		base.stateManager.SetBattleScreen(BattleScreen.BattleStartAction);
		yield break;
	}

	protected override void DisabledThisState()
	{
		base.battleStateData.isEnableBackKeySystem = true;
	}
}
