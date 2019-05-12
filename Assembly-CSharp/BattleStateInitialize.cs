using BattleStateMachineInternal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
		if (isBigBoss)
		{
			base.battleStateData.commandSelectTweenTargetCamera = base.battleStateData.bigBossCommandSelectTweenTargetCamera;
		}
		else
		{
			base.battleStateData.commandSelectTweenTargetCamera = base.battleStateData.normalCommandSelectTweenTargetCamera;
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
			int num = 0;
			foreach (bool flag in base.hierarchyData.batteWaves[i].enemiesBossFlag)
			{
				if (flag)
				{
					num++;
				}
			}
			maxBossLength = Mathf.Max(maxBossLength, num);
		}
		int playerLength = base.hierarchyData.usePlayerCharacters.Length;
		int maxDigimonLength = playerLength + maxEnemiesLength;
		List<IEnumerator> list = new List<IEnumerator>();
		base.battleStateData.isRoundStartApRevival = new bool[maxDigimonLength];
		base.stateManager.initialize.SetHitEffectPool("EFF_COM_APUP", AffectEffect.ApUp.ToString());
		base.battleStateData.stageGimmickUpEffect = new AlwaysEffectParams[maxDigimonLength];
		base.battleStateData.stageGimmickDownEffect = new AlwaysEffectParams[maxDigimonLength];
		if (!base.stateManager.onEnableTutorial)
		{
			for (int k = 0; k < maxDigimonLength; k++)
			{
				string effectId = "EFF_COM_GimmickUP";
				Action<AlwaysEffectParams, int> result = delegate(AlwaysEffectParams res, int index)
				{
					base.battleStateData.stageGimmickUpEffect[index] = res;
				};
				IEnumerator item = base.stateManager.initialize.LoadAlwaysEffect(effectId, k, result);
				list.Add(item);
				string effectId2 = "EFF_COM_GimmickDOWN";
				Action<AlwaysEffectParams, int> result2 = delegate(AlwaysEffectParams res, int index)
				{
					base.battleStateData.stageGimmickDownEffect[index] = res;
				};
				IEnumerator item2 = base.stateManager.initialize.LoadAlwaysEffect(effectId2, k, result2);
				list.Add(item2);
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

	protected virtual IEnumerator LoadPlayer()
	{
		int playerLength = base.hierarchyData.usePlayerCharacters.Length;
		List<IEnumerator> list = new List<IEnumerator>();
		base.battleStateData.playerCharacters = new CharacterStateControl[playerLength];
		base.battleStateData.revivalReservedEffect = new AlwaysEffectParams[playerLength];
		base.battleStateData.isRevivalReservedCharacter = new bool[playerLength];
		base.battleStateData.isRoundStartHpRevival = new bool[playerLength];
		LeaderSkillStatus leaderCharacterSkillStatus = null;
		PlayerStatus getStatusLeader = base.hierarchyData.usePlayerCharacters[base.hierarchyData.leaderCharacter];
		if (getStatusLeader.isHavingLeaderSkill)
		{
			leaderCharacterSkillStatus = base.hierarchyData.GetLeaderSkillStatus(getStatusLeader.leaderSkillId);
		}
		CharacterDatas leaderCharacterData = base.hierarchyData.GetCharacterDatas(getStatusLeader.groupId);
		for (int i = 0; i < playerLength; i++)
		{
			CharacterStatus characterStatus = base.hierarchyData.usePlayerCharacters[i];
			Tolerance tolerance = characterStatus.tolerance;
			CharacterDatas characterDatas = base.hierarchyData.GetCharacterDatas(characterStatus.groupId);
			SkillStatus[] skillStatuses = characterStatus.skillIds.Select((string item) => base.hierarchyData.GetSkillStatus(item)).ToArray<SkillStatus>();
			LeaderSkillStatus leaderSkillStatus = base.hierarchyData.GetLeaderSkillStatus(characterStatus.leaderSkillId);
			bool isEnemy = false;
			base.battleStateData.playerCharacters[i] = base.stateManager.initialize.LoadCharacterStateControl(characterStatus, tolerance, characterDatas, skillStatuses, leaderCharacterSkillStatus, leaderCharacterData, leaderSkillStatus, isEnemy);
			base.battleStateData.playerCharacters[i].myIndex = i;
			base.battleStateData.isRevivalReservedCharacter[i] = false;
			Action<CharacterParams, int, string> result = delegate(CharacterParams c, int index, string id)
			{
				base.battleStateData.playerCharacters[index].CharacterParams = c;
			};
			IEnumerator item4 = base.stateManager.initialize.LoadCharacterParam(characterStatus.prefabId, i, result);
			list.Add(item4);
			string effectId = "revivalReservationEffect";
			Action<AlwaysEffectParams, int> result2 = delegate(AlwaysEffectParams a, int id)
			{
				base.battleStateData.revivalReservedEffect[id] = a;
			};
			IEnumerator item2 = base.stateManager.initialize.LoadAlwaysEffect(effectId, i, result2);
			list.Add(item2);
			base.stateManager.initialize.SetHitEffectPool("EFF_COM_DEATH");
			base.stateManager.initialize.SetHitEffectPool("EFF_COM_L_HEAL");
		}
		base.battleStateData.leaderCharacter = base.battleStateData.playerCharacters[base.hierarchyData.leaderCharacter];
		base.battleStateData.leaderCharacter.isLeader = true;
		base.battleStateData.insertCharacterEffect = new AlwaysEffectParams[playerLength];
		for (int j = 0; j < playerLength; j++)
		{
			string insertPlayerPath = this.insertPlayerPath;
			Action<AlwaysEffectParams, int> result3 = delegate(AlwaysEffectParams res, int index)
			{
				base.battleStateData.insertCharacterEffect[index] = res;
			};
			IEnumerator item3 = base.stateManager.initialize.LoadAlwaysEffect(insertPlayerPath, j, result3);
			list.Add(item3);
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

	protected virtual IEnumerator LoadEnemy()
	{
		int maxEnemiesLength = 0;
		int maxBossLength = 0;
		for (int i = 0; i < base.hierarchyData.batteWaves.Length; i++)
		{
			maxEnemiesLength = Mathf.Max(maxEnemiesLength, base.hierarchyData.batteWaves[i].useEnemiesId.Length);
			int num = 0;
			foreach (bool flag in base.hierarchyData.batteWaves[i].enemiesBossFlag)
			{
				if (flag)
				{
					num++;
				}
			}
			maxBossLength = Mathf.Max(maxBossLength, num);
		}
		List<IEnumerator> list = new List<IEnumerator>();
		LeaderSkillStatus leaderCharacterLeaderSkill = null;
		CharacterStatus getStatusEnemyLeader = base.hierarchyData.batteWaves[0].enemyStatuses[base.hierarchyData.leaderCharacter];
		if (getStatusEnemyLeader.isHavingLeaderSkill)
		{
			leaderCharacterLeaderSkill = base.hierarchyData.GetLeaderSkillStatus(getStatusEnemyLeader.leaderSkillId);
		}
		CharacterDatas enemyLeaderCharacterData = base.hierarchyData.GetCharacterDatas(getStatusEnemyLeader.groupId);
		Dictionary<string, int> characterParamsIdListBattleWave = new Dictionary<string, int>();
		base.battleStateData.preloadEnemies = new CharacterStateControl[base.hierarchyData.batteWaves.Length][];
		for (int k = 0; k < base.battleStateData.preloadEnemies.Length; k++)
		{
			base.battleStateData.preloadEnemies[k] = new CharacterStateControl[base.hierarchyData.batteWaves[k].useEnemiesId.Length];
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			for (int l = 0; l < base.battleStateData.preloadEnemies[k].Length; l++)
			{
				CharacterStatus characterStatus = base.hierarchyData.batteWaves[k].enemyStatuses[l];
				Tolerance tolerance = characterStatus.tolerance;
				CharacterDatas characterDatas = base.hierarchyData.GetCharacterDatas(characterStatus.groupId);
				SkillStatus[] skillStatuses = characterStatus.skillIds.Select((string item) => base.hierarchyData.GetSkillStatus(item)).ToArray<SkillStatus>();
				LeaderSkillStatus leaderSkillStatus = base.hierarchyData.GetLeaderSkillStatus(characterStatus.leaderSkillId);
				bool isEnemy = true;
				base.battleStateData.preloadEnemies[k][l] = base.stateManager.initialize.LoadCharacterStateControl(characterStatus, tolerance, characterDatas, skillStatuses, leaderCharacterLeaderSkill, enemyLeaderCharacterData, leaderSkillStatus, isEnemy);
				base.battleStateData.preloadEnemies[k][l].myIndex = l;
				if (l == base.hierarchyData.leaderCharacter)
				{
					base.battleStateData.preloadEnemies[k][l].isLeader = true;
				}
				string prefabId = characterStatus.prefabId;
				if (!dictionary.ContainsKey(prefabId))
				{
					dictionary.Add(prefabId, 1);
				}
				else
				{
					Dictionary<string, int> dictionary2;
					string key;
					dictionary[prefabId] = ((dictionary2 = dictionary)[key = prefabId] = dictionary2[key] + 1);
				}
			}
			foreach (KeyValuePair<string, int> keyValuePair in dictionary)
			{
				if (characterParamsIdListBattleWave.ContainsKey(keyValuePair.Key))
				{
					if (characterParamsIdListBattleWave[keyValuePair.Key] < keyValuePair.Value)
					{
						characterParamsIdListBattleWave[keyValuePair.Key] = keyValuePair.Value;
					}
				}
				else
				{
					characterParamsIdListBattleWave.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}
		foreach (KeyValuePair<string, int> keyValuePair2 in characterParamsIdListBattleWave)
		{
			for (int m = 0; m < keyValuePair2.Value; m++)
			{
				IEnumerator item6 = base.stateManager.initialize.LoadCharacterParam(keyValuePair2.Key, m, delegate(CharacterParams c, int index, string id)
				{
					base.battleStateData.preloadEnemiesParams.Add(id, index.ToString(), c);
				});
				list.Add(item6);
			}
		}
		base.battleStateData.droppingItemNormalEffect = new AlwaysEffectParams[base.battleStateData.maxDropNum];
		base.battleStateData.droppingItemRareEffect = new AlwaysEffectParams[base.battleStateData.maxDropNum];
		for (int n = 0; n < maxEnemiesLength; n++)
		{
			base.stateManager.initialize.SetHitEffectPool("EFF_COM_DEATH");
			base.stateManager.initialize.SetHitEffectPool("EFF_COM_BOSSDEATH");
		}
		for (int num2 = 0; num2 < base.battleStateData.maxDropNum; num2++)
		{
			string effectId = "droppingItemEffectNormal";
			Action<AlwaysEffectParams, int> result = delegate(AlwaysEffectParams a, int index)
			{
				base.battleStateData.droppingItemNormalEffect[index] = a;
			};
			IEnumerator item2 = base.stateManager.initialize.LoadAlwaysEffect(effectId, num2, result);
			list.Add(item2);
			string effectId2 = "droppingItemEffectRare";
			Action<AlwaysEffectParams, int> result2 = delegate(AlwaysEffectParams a, int index)
			{
				base.battleStateData.droppingItemRareEffect[index] = a;
			};
			IEnumerator item3 = base.stateManager.initialize.LoadAlwaysEffect(effectId2, num2, result2);
			list.Add(item3);
		}
		base.battleStateData.leaderEnemyCharacter = base.battleStateData.enemies[base.hierarchyData.leaderCharacter];
		base.battleStateData.insertEnemyEffect = new AlwaysEffectParams[maxEnemiesLength];
		for (int num3 = 0; num3 < maxEnemiesLength; num3++)
		{
			string insertEnemyPath = this.insertEnemyPath;
			Action<AlwaysEffectParams, int> result3 = delegate(AlwaysEffectParams res, int index)
			{
				base.battleStateData.insertEnemyEffect[index] = res;
			};
			IEnumerator item4 = base.stateManager.initialize.LoadAlwaysEffect(insertEnemyPath, num3, result3);
			list.Add(item4);
		}
		base.battleStateData.insertBossCharacterEffect = new AlwaysEffectParams[maxBossLength];
		for (int num4 = 0; num4 < maxBossLength; num4++)
		{
			string effectId3 = "insertBossCharacterEffect";
			Action<AlwaysEffectParams, int> result4 = delegate(AlwaysEffectParams res, int index)
			{
				base.battleStateData.insertBossCharacterEffect[index] = res;
			};
			IEnumerator item5 = base.stateManager.initialize.LoadAlwaysEffect(effectId3, num4, result4);
			list.Add(item5);
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

	private IEnumerator LoadCharacterAfter()
	{
		CharacterStateControl[] playerCharacters = base.stateManager.battleStateData.playerCharacters;
		foreach (CharacterStateControl characterStateControl in playerCharacters)
		{
			characterStateControl.InitializeSpecialCorrectionStatus();
		}
		for (int j = 0; j < base.stateManager.battleStateData.preloadEnemies.Length; j++)
		{
			for (int k = 0; k < base.stateManager.battleStateData.preloadEnemies[j].Length; k++)
			{
				base.stateManager.battleStateData.preloadEnemies[j][k].InitializeSpecialCorrectionStatus();
			}
		}
		yield break;
	}

	private IEnumerator LoadSkill()
	{
		List<IEnumerator> effectStore = new List<IEnumerator>();
		Dictionary<string, InvocationEffectParams> invocationEffectDictionary = new Dictionary<string, InvocationEffectParams>();
		Dictionary<string, PassiveEffectParams[]> passiveEffectDictionary = new Dictionary<string, PassiveEffectParams[]>();
		List<string> skillInvocation = new List<string>();
		List<string> skillPassive = new List<string>();
		foreach (SkillStatus skillStatus in base.hierarchyData.GetAllSkillStatus())
		{
			string text = string.Empty;
			string text2 = string.Empty;
			SkillType skillType = skillStatus.skillType;
			if (skillType != SkillType.Attack)
			{
				if (skillType != SkillType.Deathblow)
				{
					if (skillType == SkillType.InheritanceTechnique)
					{
						text = "EFF_COM_SKILL";
						text2 = skillStatus.prefabId;
					}
				}
				else
				{
					text = skillStatus.prefabId;
					text2 = "none";
				}
			}
			else
			{
				text = "none";
				text2 = "none";
			}
			if (!skillInvocation.Contains(text))
			{
				string skillInvocation2 = text;
				Action<InvocationEffectParams, string> result = delegate(InvocationEffectParams p, string id)
				{
					invocationEffectDictionary.Add(id, p);
				};
				IEnumerator item = base.stateManager.initialize.LoadInvocationEffect(skillInvocation2, result);
				effectStore.Add(item);
				skillInvocation.Add(text);
			}
			if (!skillPassive.Contains(text2))
			{
				string skillPassive2 = text2;
				Action<PassiveEffectParams[], string> result2 = delegate(PassiveEffectParams[] p, string id)
				{
					passiveEffectDictionary.Add(id, p);
				};
				IEnumerator item2 = base.stateManager.initialize.LoadPassiveEffect(skillPassive2, result2);
				effectStore.Add(item2);
				skillPassive.Add(text2);
			}
		}
		IEnumerator loadEffect = base.stateManager.initialize.LoadCoroutine(effectStore.ToArray());
		while (loadEffect.MoveNext())
		{
			yield return null;
		}
		foreach (SkillStatus skillStatus2 in base.hierarchyData.GetAllSkillStatus())
		{
			string key = string.Empty;
			string key2 = string.Empty;
			SkillType skillType2 = skillStatus2.skillType;
			if (skillType2 != SkillType.Attack)
			{
				if (skillType2 != SkillType.Deathblow)
				{
					if (skillType2 == SkillType.InheritanceTechnique)
					{
						key = "EFF_COM_SKILL";
						key2 = skillStatus2.prefabId;
					}
				}
				else
				{
					key = skillStatus2.prefabId;
					key2 = "none";
				}
			}
			else
			{
				key = "none";
				key2 = "none";
			}
			skillStatus2.invocationEffectParams = invocationEffectDictionary[key];
			skillStatus2.passiveEffectParams = passiveEffectDictionary[key2];
		}
		SkillStatus[] allSkillStatus = base.hierarchyData.GetAllSkillStatus();
		List<IEnumerator> hitEffectStore = base.stateManager.initialize.LoadHitEffectsByFlags(allSkillStatus);
		IEnumerator loadHitEffect = base.stateManager.initialize.LoadCoroutine(hitEffectStore.ToArray());
		while (loadHitEffect.MoveNext())
		{
			yield return null;
		}
		List<IEnumerator> cameraMotions = base.stateManager.cameraControl.LoadCameraMotions();
		IEnumerator loadCameraMotions = base.stateManager.initialize.LoadCoroutine(cameraMotions.ToArray());
		while (loadCameraMotions.MoveNext())
		{
			yield return null;
		}
		List<IEnumerator> invocationEffectCameraMotionList = new List<IEnumerator>();
		List<string> invocationEffectKeys = new List<string>();
		foreach (SkillStatus skillStatus3 in base.hierarchyData.GetAllSkillStatus())
		{
			if (!BattleFunctionUtility.IsEmptyPath(skillStatus3.invocationEffectParams.cameraMotionId) && !invocationEffectKeys.Contains(skillStatus3.invocationEffectParams.cameraMotionId))
			{
				string cameraMotionId = skillStatus3.invocationEffectParams.cameraMotionId;
				IEnumerator item3 = base.stateManager.cameraControl.LoadCameraMotion(cameraMotionId, null);
				invocationEffectCameraMotionList.Add(item3);
				invocationEffectKeys.Add(cameraMotionId);
			}
		}
		IEnumerator loadInvocationEffectCameraMotion = base.stateManager.initialize.LoadCoroutine(invocationEffectCameraMotionList.ToArray());
		while (loadInvocationEffectCameraMotion.MoveNext())
		{
			yield return null;
		}
		foreach (SkillStatus skillStatus4 in base.hierarchyData.GetAllSkillStatus())
		{
			base.stateManager.soundPlayer.AddEffectSe(skillStatus4.seId);
		}
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
		if (base.battleMode == BattleMode.Tutorial)
		{
			base.hierarchyData.on2xSpeedPlay = false;
			base.hierarchyData.onAutoPlay = 0;
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
		this.SetLoadingImage(true);
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
		functionList.Add(this.LoadCharacterAfter());
		functionList.Add(this.LoadCommonEffect());
		functionList.Add(this.LoadSkill());
		functionList.Add(this.LoadAfterInitializeUI());
		Action loading = base.stateManager.battleUiComponents.initializeUi.GetLoadingInvoke(functionList.Count);
		TimeProfiler.BeginTotalProfile();
		foreach (IEnumerator function in functionList)
		{
			TimeProfiler.BeginProfile();
			while (function.MoveNext())
			{
				yield return null;
			}
			loading();
			yield return new WaitForEndOfFrame();
			TimeProfiler.EndProfile();
		}
		TimeProfiler.EndTotalProfile();
		for (int i = 0; i < base.hierarchyData.usePlayerCharacters.Length; i++)
		{
			bool isLeader = i == base.hierarchyData.leaderCharacter;
			CharacterStateControl characterStateControl = base.battleStateData.playerCharacters[i];
			Sprite characterThumbnail = base.stateManager.serverControl.GetCharacterThumbnail(characterStateControl.playerStatus.thumbnailId);
			base.stateManager.uiControl.ApplyMonsterButtonIcon(i, characterThumbnail, characterStateControl, isLeader);
		}
		BattleDebug.Log("--- バトルGC : 開始");
		Resources.UnloadUnusedAssets();
		GC.Collect();
		BattleDebug.Log("--- バトルGC : 完了");
		this.SetActiveHierarcyRendering(true);
		this.SetLoadingImage(false);
		base.stateManager.uiControl.SetTouchEnable(true);
		base.stateManager.SetBattleScreen(BattleScreen.BattleStartAction);
		base.stateManager.battleUiComponents.InitSafeArea();
		yield break;
	}

	private void SetLoadingImage(bool isShow)
	{
		if (base.onServerConnect)
		{
			if (isShow)
			{
				if (!Loading.IsShow())
				{
					RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
					TipsLoading.Instance.StartTipsLoad(CMD_Tips.DISPLAY_PLACE.QuestToSoloBattle, false);
				}
			}
			else
			{
				TipsLoading.Instance.StopTipsLoad(false);
				RestrictionInput.DeleteDisplayObject();
				RestrictionInput.EndLoad();
			}
		}
	}

	protected override void DisabledThisState()
	{
		base.battleStateData.isEnableBackKeySystem = true;
	}
}
