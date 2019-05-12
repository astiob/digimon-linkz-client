using BattleStateMachineInternal;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInitialize : BattleFunctionBase
{
	public const string characterRootName = "CharacterRoot";

	public const string skillEffectRootName = "SkillEffectRoot";

	public const string stageRootName = "StageRoot";

	public const string spawnPointRootName = "SpawnPointRoot";

	public const string cameraMotionRootName = "CameraMotionRoot";

	public const string hitEffectRootName = "HitEffectRoot";

	public const string alwaysEffectRootName = "AlwaysEffectRoot";

	public const string BattleInternalResourcesRootName = "BattleInternalResourcesRoot";

	public void InitializeRoots()
	{
		base.battleStateData.characterRoot = new GameObject("CharacterRoot").transform;
		base.battleStateData.skillEffectRoot = new GameObject("SkillEffectRoot").transform;
		base.battleStateData.alwaysEffectRoot = new GameObject("AlwaysEffectRoot").transform;
		base.battleStateData.stageRoot = new GameObject("StageRoot").transform;
		base.battleStateData.spawnPointRoot = new GameObject("SpawnPointRoot").transform;
		base.battleStateData.cameraMotionRoot = new GameObject("CameraMotionRoot").transform;
		base.battleStateData.hitEffectRoot = new GameObject("HitEffectRoot").transform;
		base.battleStateData.BattleInternalResourcesRoot = new GameObject("BattleInternalResourcesRoot").transform;
	}

	public CharacterStateControl LoadCharacterStateControl(CharacterStatus status, Tolerance tolerance, CharacterDatas datas, SkillStatus[] skillStatuses, LeaderSkillStatus leaderCharacterLeaderSkill, CharacterDatas leaderCharacterData, LeaderSkillStatus myLeaderSkill = null, bool isEnemy = false)
	{
		CharacterStateControl characterStateControl = new CharacterStateControl(status, tolerance, datas, skillStatuses, leaderCharacterLeaderSkill, leaderCharacterData, myLeaderSkill, isEnemy);
		characterStateControl.InitializeAp();
		return characterStateControl;
	}

	public IEnumerator LoadCharacterParam(string characterId, int index, Action<CharacterParams, int, string> result)
	{
		BattleDebug.Log(string.Concat(new object[]
		{
			"--- モンスター単体ロード characterId[",
			characterId,
			"] index[",
			index,
			"] : 開始"
		}));
		GameObject prefab = base.stateManager.serverControl.GetCharacterPrefab(characterId);
		GameObject character = base.Instantiate<GameObject>(prefab);
		yield return null;
		character.name = characterId;
		character.transform.SetParent(base.battleStateData.characterRoot);
		character.transform.position = Vector3.zero;
		character.transform.rotation = Quaternion.identity;
		CharacterParams characterParams = character.GetComponent<CharacterParams>();
		CapsuleCollider col = characterParams.GetComponent<CapsuleCollider>();
		col.radius = 1.03f;
		if (base.battleStateData.useCharacterShadow)
		{
			characterParams.SetShadowObject();
			yield return null;
		}
		characterParams.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
		yield return null;
		characterParams.Initialize(base.hierarchyData.cameraObject.camera3D);
		GameObject hasColliderObject = characterParams.collider.gameObject;
		BattleStateData battleStateData = base.battleStateData;
		battleStateData.characterColliderLayerMask |= 1 << hasColliderObject.layer;
		character.gameObject.SetActive(false);
		result(characterParams, index, characterId);
		BattleDebug.Log(string.Concat(new object[]
		{
			"--- モンスター単体ロード characterId[",
			characterId,
			"] index[",
			index,
			"] : 完了"
		}));
		yield break;
	}

	public List<IEnumerator> LoadHitEffectsByFlags(SkillStatus[] skillStatusList)
	{
		List<IEnumerator> result = new List<IEnumerator>();
		this.SetHitEffectPool("EFF_COM_HIT_WEAK");
		if (!base.stateManager.onEnableTutorial)
		{
			this.SetHitEffectPool("EFF_COM_SPECIALATTACK_U", AffectEffect.gimmickSpecialAttackUp.ToString());
			this.SetHitEffectPool("EFF_COM_SPECIALATTACK_D", AffectEffect.gimmickSpecialAttackDown.ToString());
		}
		List<AffectEffect> list = new List<AffectEffect>();
		bool flag = false;
		foreach (SkillStatus skillStatus in skillStatusList)
		{
			foreach (AffectEffectProperty affectEffectProperty in skillStatus.affectEffect)
			{
				if (!list.Contains(affectEffectProperty.type))
				{
					list.Add(affectEffectProperty.type);
				}
				if (affectEffectProperty.useDrain)
				{
					flag = true;
				}
			}
		}
		foreach (AffectEffect affectEffect in list)
		{
			if (affectEffect == AffectEffect.Damage)
			{
				this.SetHitEffectPool("EFF_COM_HIT_NORMAL");
				this.SetHitEffectPool("EFF_COM_HIT_WEAK");
				this.SetHitEffectPool("EFF_COM_HIT_CRITICAL");
				if (base.battleMode != BattleMode.Tutorial)
				{
					this.SetHitEffectPool("EFF_COM_S_HEAL");
				}
				if (flag)
				{
					this.SetHitEffectPool("EFF_COM_M_HEAL", AffectEffect.HpRevival.ToString());
				}
			}
			else if (affectEffect == AffectEffect.AttackUp)
			{
				this.SetHitEffectPool("EFF_COM_UP", AffectEffect.AttackUp.ToString());
			}
			else if (affectEffect == AffectEffect.AttackDown)
			{
				this.SetHitEffectPool("EFF_COM_DOWN", AffectEffect.AttackDown.ToString());
			}
			else if (affectEffect == AffectEffect.SpAttackUp)
			{
				this.SetHitEffectPool("EFF_COM_UP", AffectEffect.SpAttackUp.ToString());
			}
			else if (affectEffect == AffectEffect.SpAttackDown)
			{
				this.SetHitEffectPool("EFF_COM_DOWN", AffectEffect.SpAttackDown.ToString());
			}
			else if (affectEffect == AffectEffect.DefenceUp)
			{
				this.SetHitEffectPool("EFF_COM_UP", AffectEffect.DefenceUp.ToString());
			}
			else if (affectEffect == AffectEffect.DefenceDown)
			{
				this.SetHitEffectPool("EFF_COM_DOWN", AffectEffect.DefenceDown.ToString());
			}
			else if (affectEffect == AffectEffect.SpDefenceUp)
			{
				this.SetHitEffectPool("EFF_COM_UP", AffectEffect.SpDefenceUp.ToString());
			}
			else if (affectEffect == AffectEffect.SpDefenceDown)
			{
				this.SetHitEffectPool("EFF_COM_DOWN", AffectEffect.SpDefenceDown.ToString());
			}
			else if (affectEffect == AffectEffect.PowerCharge)
			{
				this.SetHitEffectPool("EFF_COM_SPIRIT", AffectEffect.PowerCharge.ToString());
			}
			else if (affectEffect == AffectEffect.Protect)
			{
				this.SetHitEffectPool("EFF_COM_GUARD", AffectEffect.Protect.ToString());
			}
			else if (affectEffect == AffectEffect.Destruct)
			{
				this.SetHitEffectPool("EFF_COM_DEATH", AffectEffect.Destruct.ToString());
			}
			else if (affectEffect == AffectEffect.Poison)
			{
				this.SetHitEffectPool("EFF_COM_POISONATTACK", AffectEffect.Poison.ToString());
			}
			else if (affectEffect == AffectEffect.Counter)
			{
				this.SetHitEffectPool("EFF_COM_COUNTER_P", AffectEffect.Counter.ToString());
			}
			else if (affectEffect == AffectEffect.Reflection)
			{
				this.SetHitEffectPool("EFF_COM_COUNTER_M", AffectEffect.Reflection.ToString());
			}
			else if (affectEffect == AffectEffect.Confusion)
			{
				this.SetHitEffectPool("EFF_COM_CONFUSIONATTACK", AffectEffect.Confusion.ToString());
			}
			else if (affectEffect == AffectEffect.CorrectionUpReset)
			{
				this.SetHitEffectPool("EFF_COM_DOWN", AffectEffect.CorrectionUpReset.ToString());
			}
			else if (affectEffect == AffectEffect.CorrectionDownReset)
			{
				this.SetHitEffectPool("EFF_COM_UP", AffectEffect.CorrectionDownReset.ToString());
			}
			else if (affectEffect == AffectEffect.HateUp)
			{
				this.SetHitEffectPool("EFF_COM_DOWN", AffectEffect.HateUp.ToString());
			}
			else if (affectEffect == AffectEffect.HateDown)
			{
				this.SetHitEffectPool("EFF_COM_UP", AffectEffect.HateDown.ToString());
			}
			else if (affectEffect == AffectEffect.HitRateUp)
			{
				this.SetHitEffectPool("EFF_COM_UP", AffectEffect.HitRateUp.ToString());
			}
			else if (affectEffect == AffectEffect.HitRateDown)
			{
				this.SetHitEffectPool("EFF_COM_DOWN", AffectEffect.HitRateDown.ToString());
			}
			else if (affectEffect == AffectEffect.HpRevival || affectEffect == AffectEffect.HpSettingFixable || affectEffect == AffectEffect.HpSettingPercentage)
			{
				this.SetHitEffectPool("EFF_COM_M_HEAL", AffectEffect.HpRevival.ToString());
			}
			else if (affectEffect == AffectEffect.InstantDeath)
			{
				this.SetHitEffectPool("EFF_COM_DEATHATTACK", AffectEffect.InstantDeath.ToString());
			}
			else if (affectEffect == AffectEffect.Paralysis)
			{
				this.SetHitEffectPool("EFF_COM_PARALYSISATTACK", AffectEffect.Paralysis.ToString());
			}
			else if (affectEffect == AffectEffect.Stun)
			{
				this.SetHitEffectPool("EFF_COM_BUGATTACK", AffectEffect.Stun.ToString());
			}
			else if (affectEffect == AffectEffect.SufferStatusClear)
			{
				this.SetHitEffectPool("EFF_COM_UP", AffectEffect.SufferStatusClear.ToString());
			}
			else if (affectEffect == AffectEffect.SpeedDown)
			{
				this.SetHitEffectPool("EFF_COM_DOWN", AffectEffect.SpeedDown.ToString());
			}
			else if (affectEffect == AffectEffect.SpeedUp)
			{
				this.SetHitEffectPool("EFF_COM_UP", AffectEffect.SpeedUp.ToString());
			}
			else if (affectEffect == AffectEffect.Sleep)
			{
				this.SetHitEffectPool("EFF_COM_SLEEPATTACK", AffectEffect.Sleep.ToString());
			}
			else if (affectEffect == AffectEffect.SkillLock)
			{
				this.SetHitEffectPool("EFF_COM_CRYSTALATTACK", AffectEffect.SkillLock.ToString());
			}
			else if (affectEffect == AffectEffect.SatisfactionRateDown)
			{
				this.SetHitEffectPool("EFF_COM_DOWN", AffectEffect.SatisfactionRateDown.ToString());
			}
			else if (affectEffect == AffectEffect.SatisfactionRateUp)
			{
				this.SetHitEffectPool("EFF_COM_UP", AffectEffect.SatisfactionRateUp.ToString());
			}
			else if (affectEffect == AffectEffect.ApRevival)
			{
				this.SetHitEffectPool("EFF_COM_M_HEAL", AffectEffect.ApRevival.ToString());
			}
			else if (affectEffect == AffectEffect.ApUp)
			{
				this.SetHitEffectPool("EFF_COM_UP", AffectEffect.ApUp.ToString());
			}
			else if (affectEffect == AffectEffect.ApDown)
			{
				this.SetHitEffectPool("EFF_COM_DOWN", AffectEffect.ApDown.ToString());
			}
			else if (affectEffect == AffectEffect.ApConsumptionUp)
			{
				this.SetHitEffectPool("EFF_COM_DOWN", AffectEffect.ApConsumptionUp.ToString());
			}
			else if (affectEffect == AffectEffect.ApConsumptionDown)
			{
				this.SetHitEffectPool("EFF_COM_UP", AffectEffect.ApConsumptionDown.ToString());
			}
			else if (affectEffect == AffectEffect.TurnBarrier)
			{
				this.SetHitEffectPool("EFF_COM_GUARD", AffectEffect.TurnBarrier.ToString());
			}
			else if (affectEffect == AffectEffect.CountBarrier)
			{
				this.SetHitEffectPool("EFF_COM_GUARD", AffectEffect.CountBarrier.ToString());
			}
			else if (affectEffect == AffectEffect.CountGuard)
			{
				this.SetHitEffectPool("EFF_COM_GUARD", AffectEffect.CountGuard.ToString());
			}
			else if (affectEffect == AffectEffect.Recommand)
			{
				this.SetHitEffectPool("EFF_COM_M_HEAL", AffectEffect.Recommand.ToString());
			}
			else if (affectEffect == AffectEffect.DamageRateUp)
			{
				this.SetHitEffectPool("EFF_COM_UP", AffectEffect.DamageRateUp.ToString());
			}
			else if (affectEffect == AffectEffect.DamageRateDown)
			{
				this.SetHitEffectPool("EFF_COM_DOWN", AffectEffect.DamageRateDown.ToString());
			}
			else if (affectEffect == AffectEffect.Regenerate)
			{
				this.SetHitEffectPool("EFF_COM_M_HEAL", AffectEffect.Regenerate.ToString());
			}
			else if (affectEffect == AffectEffect.TurnEvasion)
			{
				this.SetHitEffectPool("EFF_COM_UP", AffectEffect.TurnEvasion.ToString());
			}
			else if (affectEffect == AffectEffect.CountEvasion)
			{
				this.SetHitEffectPool("EFF_COM_UP", AffectEffect.CountEvasion.ToString());
			}
			else if (affectEffect == AffectEffect.ReferenceTargetHpRate)
			{
				this.SetHitEffectPool("EFF_COM_HIT_RATIO", AffectEffect.ReferenceTargetHpRate.ToString());
			}
			else if (affectEffect == AffectEffect.ApDrain)
			{
				this.SetHitEffectPool("EFF_COM_S_APDRAIN", AffectEffect.ApDown.ToString());
				this.SetHitEffectPool("EFF_COM_DOWN", AffectEffect.ApDown.ToString());
			}
			else if (affectEffect == AffectEffect.Escape)
			{
				this.SetHitEffectPool("EFF_COM_UP", AffectEffect.Escape.ToString());
			}
			else if (affectEffect == AffectEffect.RefHpRateNonAttribute)
			{
				this.SetHitEffectPool("EFF_COM_HIT_RATIO", AffectEffect.RefHpRateNonAttribute.ToString());
			}
		}
		return result;
	}

	private void SetEffectPool(string key, Transform parent, GameObject prefab)
	{
		BattleEffectManager.Instance.SetPool(key, parent, prefab);
	}

	public void SetHitEffectPool(string key)
	{
		this.SetHitEffectPool(key, key);
	}

	public void SetHitEffectPool(string prefab_key, string key)
	{
		GameObject hitEffectPrefab = base.stateManager.serverControl.GetHitEffectPrefab(prefab_key);
		hitEffectPrefab.name = prefab_key;
		HitEffectParams component = hitEffectPrefab.GetComponent<HitEffectParams>();
		if (component != null)
		{
			base.stateManager.soundPlayer.AddEffectSe(component.seId);
		}
		BattleEffectManager.Instance.SetCamera(base.hierarchyData.cameraObject.camera3D);
		BattleEffectManager.Instance.SetPool(key, base.battleStateData.hitEffectRoot.transform, hitEffectPrefab);
	}

	public IEnumerator LoadAlwaysEffect(string effectId, int index, Action<AlwaysEffectParams, int> result)
	{
		BattleDebug.Log(string.Concat(new object[]
		{
			"--- 常設エフェクト単体ロード effectId[",
			effectId,
			"] index[",
			index,
			"] : 開始"
		}));
		AlwaysEffectParams alwaysParam = null;
		GameObject effect = null;
		if (!BattleObjectPooler.TryGetAlwaysEffectParams(effectId, index, out alwaysParam))
		{
			GameObject prefab = base.stateManager.serverControl.GetAlwaysEffectPrefab(effectId);
			effect = base.Instantiate<GameObject>(prefab);
			yield return null;
			effect.name = effectId;
			alwaysParam = effect.GetComponent<AlwaysEffectParams>();
			BattleObjectPooler.AddAlwaysEffectParams(effectId, alwaysParam);
		}
		else
		{
			effect = alwaysParam.gameObject;
		}
		effect.transform.SetParent(base.battleStateData.alwaysEffectRoot);
		effect.transform.position = Vector3.zero;
		effect.transform.rotation = Quaternion.identity;
		IEnumerator initialize = alwaysParam.Initialize(base.hierarchyData.cameraObject.camera3D);
		while (initialize.MoveNext())
		{
			yield return null;
		}
		base.stateManager.threeDAction.StopAlwaysEffectAction(new AlwaysEffectParams[]
		{
			alwaysParam
		});
		base.stateManager.soundPlayer.AddEffectSe(alwaysParam.inSeId);
		base.stateManager.soundPlayer.AddEffectSe(alwaysParam.alwaysSeId);
		base.stateManager.soundPlayer.AddEffectSe(alwaysParam.outSeId);
		result(alwaysParam, index);
		BattleDebug.Log(string.Concat(new object[]
		{
			"--- 常設エフェクト単体ロード effectId[",
			effectId,
			"] index[",
			index,
			"] : 完了"
		}));
		yield break;
	}

	public IEnumerator LoadHitEffect(string effectId, int index, Action<HitEffectParams, int> result)
	{
		BattleDebug.Log(string.Concat(new object[]
		{
			"-- ヒットエフェクト単体ロード effectId[",
			effectId,
			"] index[",
			index,
			"] : 開始"
		}));
		HitEffectParams hitEffectParams = null;
		GameObject hitEffect = null;
		if (!BattleObjectPooler.TryGetHitEffectParams(effectId, index, out hitEffectParams))
		{
			GameObject prefab = base.stateManager.serverControl.GetHitEffectPrefab(effectId);
			hitEffect = base.Instantiate<GameObject>(prefab);
			yield return null;
			hitEffect.name = effectId;
			hitEffectParams = hitEffect.GetComponent<HitEffectParams>();
			BattleObjectPooler.AddHitEffectParams(effectId, hitEffectParams);
		}
		else
		{
			hitEffect = hitEffectParams.gameObject;
		}
		hitEffect.transform.SetParent(base.battleStateData.hitEffectRoot);
		hitEffect.SetActive(false);
		IEnumerator initialize = hitEffectParams.Initialize(base.hierarchyData.cameraObject.camera3D);
		while (initialize.MoveNext())
		{
			yield return null;
		}
		base.stateManager.soundPlayer.AddEffectSe(hitEffectParams.seId);
		result(hitEffectParams, index);
		BattleDebug.Log(string.Concat(new object[]
		{
			"-- ヒットエフェクト単体ロード effectId[",
			effectId,
			"] index[",
			index,
			"] : 完了"
		}));
		yield break;
	}

	public IEnumerator LoadPassiveEffect(string skillPassive, Action<PassiveEffectParams[], string> result)
	{
		BattleDebug.Log("--- 受動スキルエフェクト単体ロード [" + skillPassive + "] : 開始");
		List<PassiveEffectParams> passiveList = new List<PassiveEffectParams>();
		for (int i = 0; i < base.battleStateData.maxCharacterLength; i++)
		{
			GameObject prefab = base.stateManager.serverControl.GetPassiveEffectPrefab(skillPassive);
			GameObject skillObject = base.Instantiate<GameObject>(prefab);
			yield return null;
			skillObject.name = skillPassive;
			skillObject.transform.SetParent(base.battleStateData.skillEffectRoot);
			PassiveEffectParams passiveEffectParams = skillObject.GetComponent<PassiveEffectParams>();
			IEnumerator skillInit = passiveEffectParams.Initialize(base.hierarchyData.cameraObject.camera3D);
			while (skillInit.MoveNext())
			{
				yield return null;
			}
			skillObject.SetActive(false);
			passiveList.Add(passiveEffectParams);
		}
		result(passiveList.ToArray(), skillPassive);
		BattleDebug.Log("--- 受動スキルエフェクト単体ロード [" + skillPassive + "] : 完了");
		yield break;
	}

	public IEnumerator LoadInvocationEffect(string skillInvocation, Action<InvocationEffectParams, string> result)
	{
		BattleDebug.Log("--- 発動スキルエフェクト単体ロード [" + skillInvocation + "] : 開始");
		GameObject skillObject = base.Instantiate<GameObject>(base.stateManager.serverControl.GetInvocationEffectPrefab(skillInvocation));
		yield return null;
		skillObject.name = skillInvocation;
		skillObject.transform.SetParent(base.battleStateData.skillEffectRoot);
		InvocationEffectParams invocationEffectParams = skillObject.GetComponent<InvocationEffectParams>();
		IEnumerator skillInit = invocationEffectParams.SkillInitialize(base.hierarchyData.cameraObject.camera3D, base.battleStateData.stageObject, base.hierarchyData.cameraObject.sunLightColorChanger);
		while (skillInit.MoveNext())
		{
			yield return null;
		}
		skillObject.SetActive(false);
		result(invocationEffectParams, skillInvocation);
		BattleDebug.Log("--- 発動スキルエフェクト単体ロード [" + skillInvocation + "] : 完了");
		yield break;
	}

	public IEnumerator LoadStage()
	{
		BattleDebug.Log("--- ステージ単体ロード: 開始");
		GameObject stage = base.Instantiate<GameObject>(base.stateManager.serverControl.GetStagePrefab(base.hierarchyData.useStageId));
		yield return null;
		stage.name = base.hierarchyData.useStageId;
		stage.transform.SetParent(base.battleStateData.stageRoot);
		base.hierarchyData.stageParams = stage.GetComponent<StageParams>();
		base.hierarchyData.stageParams.SetHierarchyEnviroments(base.hierarchyData.cameraObject);
		base.battleStateData.stageObject = stage;
		stage.transform.localPosition = Vector3.zero;
		stage.transform.localRotation = Quaternion.identity;
		base.battleStateData.useCharacterShadow = base.hierarchyData.stageParams.UseCharacterShadow;
		BattleDebug.Log("--- ステージ単体ロード: 完了");
		yield break;
	}

	public IEnumerator LoadSpawnPoint(string spawnPointId)
	{
		BattleDebug.Log("--- 出現ポイント単体ロード spawnPointId[" + spawnPointId + "] : 開始");
		GameObject spawnPoint = base.Instantiate<GameObject>(base.stateManager.serverControl.GetSpawnPointPrefab(spawnPointId));
		yield return null;
		spawnPoint.name = spawnPointId;
		spawnPoint.transform.SetParent(base.battleStateData.spawnPointRoot);
		spawnPoint.transform.Clear();
		base.battleStateData.preloadSpawnPoints.Add(spawnPointId, spawnPoint.GetComponent<SpawnPointParams>());
		BattleDebug.Log("--- 出現ポイント単体ロード spawnPointId[" + spawnPointId + "] : 完了");
		yield break;
	}

	public IEnumerator LoadBattleInternalResources(string id, Action<GameObject> result)
	{
		BattleDebug.Log("--- バトル内部リソース単体ロード id[" + id + "] : 開始");
		GameObject BattleInternalResources = base.Instantiate<GameObject>(ResourcesPath.GetBattleInternalResourcesPrefab(id));
		yield return null;
		BattleInternalResources.name = id;
		BattleInternalResources.transform.SetParent(base.battleStateData.BattleInternalResourcesRoot);
		BattleInternalResources.transform.Clear();
		result(BattleInternalResources);
		BattleDebug.Log("--- バトル内部リソース単体ロード id[" + id + "] : 完了");
		yield break;
	}

	public IEnumerator LoadCoroutine(IEnumerator[] allIEnumerators)
	{
		foreach (IEnumerator allIEnumerator in allIEnumerators)
		{
			while (allIEnumerator.MoveNext())
			{
				yield return null;
			}
			if (base.stateManager.system.IfFullMemoryCallGC())
			{
				break;
			}
		}
		yield break;
	}
}
