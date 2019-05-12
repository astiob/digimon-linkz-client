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

	private Dictionary<string, List<HitEffectParams>> hitEffectObject = new Dictionary<string, List<HitEffectParams>>();

	public CharacterStateControl LoadCharacterStateControl(CharacterStatus getStatus, LeaderSkillStatus leaderSkill = null, CharacterDatas leaderCharacter = null, bool isEnemy = false)
	{
		CharacterDatas characterData = base.stateManager.serverControl.GetCharacterData(getStatus.prefabId);
		Tolerance tolerance = null;
		if (getStatus.GetType() == typeof(PlayerStatus))
		{
			tolerance = ((PlayerStatus)getStatus).tolerance;
		}
		if (tolerance == null)
		{
			tolerance = base.stateManager.serverControl.GetToleranceStatus(getStatus.toleranceId);
		}
		LeaderSkillStatus myLeaderSkill = null;
		if (getStatus.GetType() == typeof(PlayerStatus) && ((PlayerStatus)getStatus).isHavingLeaderSkill)
		{
			myLeaderSkill = base.stateManager.serverControl.GetLeaderSkillStatus(((PlayerStatus)getStatus).leaderSkillId);
		}
		CharacterStateControl characterStateControl = new CharacterStateControl(getStatus, tolerance, characterData, leaderSkill, base.stateManager.publicAttackSkillId, leaderCharacter, myLeaderSkill, isEnemy);
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

	public List<IEnumerator> LoadHitEffectsByFlags(SkillStatus[] skillStatusList)
	{
		List<IEnumerator> list = new List<IEnumerator>();
		if (!base.stateManager.onEnableTutorial)
		{
			string hitEffectId = "EFF_COM_SPECIALATTACK_U";
			IEnumerator item = this.LoadHitEffects(hitEffectId, AffectEffect.gimmickSpecialAttackUp, false);
			list.Add(item);
			string hitEffectId2 = "EFF_COM_SPECIALATTACK_D";
			IEnumerator item2 = this.LoadHitEffects(hitEffectId2, AffectEffect.gimmickSpecialAttackDown, false);
			list.Add(item2);
		}
		List<AffectEffect> list2 = new List<AffectEffect>();
		bool flag = false;
		foreach (SkillStatus skillStatus in skillStatusList)
		{
			foreach (AffectEffectProperty affectEffectProperty in skillStatus.affectEffect)
			{
				if (!list2.Contains(affectEffectProperty.type))
				{
					list2.Add(affectEffectProperty.type);
				}
				if (affectEffectProperty.useDrain)
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			string effectId = "EFF_COM_S_HEAL";
			Action<HitEffectParams, int> result = delegate(HitEffectParams h, int index)
			{
				base.battleStateData.drainEffect = h;
			};
			IEnumerator item3 = base.stateManager.initialize.LoadHitEffect(effectId, 0, result);
			list.Add(item3);
		}
		foreach (AffectEffect affectEffect in list2)
		{
			if (affectEffect == AffectEffect.Damage)
			{
				string hitEffectId3 = "EFF_COM_HIT_NORMAL";
				IEnumerator item4 = this.LoadHitEffects(hitEffectId3, AffectEffect.Damage, Strength.None, true);
				list.Add(item4);
				string hitEffectId4 = "EFF_COM_HIT_WEAK";
				IEnumerator item5 = this.LoadHitEffects(hitEffectId4, AffectEffect.Damage, Strength.Strong, true);
				list.Add(item5);
				string hitEffectId5 = "EFF_COM_HIT_CRITICAL";
				IEnumerator item6 = this.LoadHitEffects(hitEffectId5, AffectEffect.Damage, Strength.Weak, true);
				list.Add(item6);
				string hitEffectId6 = "EFF_COM_HIT_WEAK";
				IEnumerator item7 = this.LoadHitEffects(hitEffectId6, AffectEffect.Damage, Strength.Invalid, true);
				list.Add(item7);
				if (base.battleMode != BattleMode.Tutorial)
				{
					string hitEffectId7 = "EFF_COM_S_HEAL";
					IEnumerator item8 = this.LoadHitEffects(hitEffectId7, AffectEffect.Damage, Strength.Drain, true);
					list.Add(item8);
				}
			}
			else if (affectEffect == AffectEffect.AttackUp)
			{
				string hitEffectId8 = "EFF_COM_UP";
				IEnumerator item9 = this.LoadHitEffects(hitEffectId8, AffectEffect.AttackUp, true);
				list.Add(item9);
			}
			else if (affectEffect == AffectEffect.AttackDown)
			{
				string hitEffectId9 = "EFF_COM_DOWN";
				IEnumerator item10 = this.LoadHitEffects(hitEffectId9, AffectEffect.AttackDown, true);
				list.Add(item10);
			}
			else if (affectEffect == AffectEffect.SpAttackUp)
			{
				string hitEffectId10 = "EFF_COM_UP";
				IEnumerator item11 = this.LoadHitEffects(hitEffectId10, AffectEffect.SpAttackUp, true);
				list.Add(item11);
			}
			else if (affectEffect == AffectEffect.SpAttackDown)
			{
				string hitEffectId11 = "EFF_COM_DOWN";
				IEnumerator item12 = this.LoadHitEffects(hitEffectId11, AffectEffect.SpAttackDown, true);
				list.Add(item12);
			}
			else if (affectEffect == AffectEffect.DefenceUp)
			{
				string hitEffectId12 = "EFF_COM_UP";
				IEnumerator item13 = this.LoadHitEffects(hitEffectId12, AffectEffect.DefenceUp, true);
				list.Add(item13);
			}
			else if (affectEffect == AffectEffect.DefenceDown)
			{
				string hitEffectId13 = "EFF_COM_DOWN";
				IEnumerator item14 = this.LoadHitEffects(hitEffectId13, AffectEffect.DefenceDown, true);
				list.Add(item14);
			}
			else if (affectEffect == AffectEffect.SpDefenceUp)
			{
				string hitEffectId14 = "EFF_COM_UP";
				IEnumerator item15 = this.LoadHitEffects(hitEffectId14, AffectEffect.SpDefenceUp, true);
				list.Add(item15);
			}
			else if (affectEffect == AffectEffect.SpDefenceDown)
			{
				string hitEffectId15 = "EFF_COM_DOWN";
				IEnumerator item16 = this.LoadHitEffects(hitEffectId15, AffectEffect.SpDefenceDown, true);
				list.Add(item16);
			}
			else if (affectEffect == AffectEffect.PowerCharge)
			{
				string hitEffectId16 = "EFF_COM_SPIRIT";
				IEnumerator item17 = this.LoadHitEffects(hitEffectId16, AffectEffect.PowerCharge, false);
				list.Add(item17);
			}
			else if (affectEffect == AffectEffect.Protect)
			{
				string hitEffectId17 = "EFF_COM_GUARD";
				IEnumerator item18 = this.LoadHitEffects(hitEffectId17, AffectEffect.Protect, false);
				list.Add(item18);
			}
			else if (affectEffect == AffectEffect.Destruct)
			{
				string hitEffectId18 = "EFF_COM_DEATH";
				IEnumerator item19 = this.LoadHitEffects(hitEffectId18, AffectEffect.Destruct, false);
				list.Add(item19);
			}
			else if (affectEffect == AffectEffect.Poison)
			{
				string hitEffectId19 = "EFF_COM_POISONATTACK";
				IEnumerator item20 = this.LoadHitEffects(hitEffectId19, AffectEffect.Poison, true);
				list.Add(item20);
			}
			else if (affectEffect == AffectEffect.Counter)
			{
				string hitEffectId20 = "EFF_COM_COUNTER_P";
				IEnumerator item21 = this.LoadHitEffects(hitEffectId20, AffectEffect.Counter, false);
				list.Add(item21);
			}
			else if (affectEffect == AffectEffect.Reflection)
			{
				string hitEffectId21 = "EFF_COM_COUNTER_M";
				IEnumerator item22 = this.LoadHitEffects(hitEffectId21, AffectEffect.Reflection, false);
				list.Add(item22);
			}
			else if (affectEffect == AffectEffect.Confusion)
			{
				string hitEffectId22 = "EFF_COM_CONFUSIONATTACK";
				IEnumerator item23 = this.LoadHitEffects(hitEffectId22, AffectEffect.Confusion, true);
				list.Add(item23);
			}
			else if (affectEffect == AffectEffect.CorrectionUpReset)
			{
				string hitEffectId23 = "EFF_COM_DOWN";
				IEnumerator item24 = this.LoadHitEffects(hitEffectId23, AffectEffect.CorrectionUpReset, true);
				list.Add(item24);
			}
			else if (affectEffect == AffectEffect.CorrectionDownReset)
			{
				string hitEffectId24 = "EFF_COM_UP";
				IEnumerator item25 = this.LoadHitEffects(hitEffectId24, AffectEffect.CorrectionDownReset, true);
				list.Add(item25);
			}
			else if (affectEffect == AffectEffect.HateUp)
			{
				string hitEffectId25 = "EFF_COM_DOWN";
				IEnumerator item26 = this.LoadHitEffects(hitEffectId25, AffectEffect.HateUp, false);
				list.Add(item26);
			}
			else if (affectEffect == AffectEffect.HateDown)
			{
				string hitEffectId26 = "EFF_COM_UP";
				IEnumerator item27 = this.LoadHitEffects(hitEffectId26, AffectEffect.HateDown, false);
				list.Add(item27);
			}
			else if (affectEffect == AffectEffect.HitRateUp)
			{
				string hitEffectId27 = "EFF_COM_UP";
				IEnumerator item28 = this.LoadHitEffects(hitEffectId27, AffectEffect.HitRateUp, true);
				list.Add(item28);
			}
			else if (affectEffect == AffectEffect.HitRateDown)
			{
				string hitEffectId28 = "EFF_COM_DOWN";
				IEnumerator item29 = this.LoadHitEffects(hitEffectId28, AffectEffect.HitRateDown, true);
				list.Add(item29);
			}
			else if (affectEffect == AffectEffect.HpRevival)
			{
				string hitEffectId29 = "EFF_COM_M_HEAL";
				IEnumerator item30 = this.LoadHitEffects(hitEffectId29, AffectEffect.HpRevival, true);
				list.Add(item30);
			}
			else if (affectEffect == AffectEffect.InstantDeath)
			{
				string hitEffectId30 = "EFF_COM_DEATHATTACK";
				IEnumerator item31 = this.LoadHitEffects(hitEffectId30, AffectEffect.InstantDeath, true);
				list.Add(item31);
			}
			else if (affectEffect == AffectEffect.Paralysis)
			{
				string hitEffectId31 = "EFF_COM_PARALYSISATTACK";
				IEnumerator item32 = this.LoadHitEffects(hitEffectId31, AffectEffect.Paralysis, true);
				list.Add(item32);
			}
			else if (affectEffect == AffectEffect.Stun)
			{
				string hitEffectId32 = "EFF_COM_BUGATTACK";
				IEnumerator item33 = this.LoadHitEffects(hitEffectId32, AffectEffect.Stun, true);
				list.Add(item33);
			}
			else if (affectEffect == AffectEffect.SufferStatusClear)
			{
				string hitEffectId33 = "EFF_COM_UP";
				IEnumerator item34 = this.LoadHitEffects(hitEffectId33, AffectEffect.SufferStatusClear, true);
				list.Add(item34);
			}
			else if (affectEffect == AffectEffect.SpeedDown)
			{
				string hitEffectId34 = "EFF_COM_DOWN";
				IEnumerator item35 = this.LoadHitEffects(hitEffectId34, AffectEffect.SpeedDown, true);
				list.Add(item35);
			}
			else if (affectEffect == AffectEffect.SpeedUp)
			{
				string hitEffectId35 = "EFF_COM_UP";
				IEnumerator item36 = this.LoadHitEffects(hitEffectId35, AffectEffect.SpeedUp, true);
				list.Add(item36);
			}
			else if (affectEffect == AffectEffect.Sleep)
			{
				string hitEffectId36 = "EFF_COM_SLEEPATTACK";
				IEnumerator item37 = this.LoadHitEffects(hitEffectId36, AffectEffect.Sleep, true);
				list.Add(item37);
			}
			else if (affectEffect == AffectEffect.SkillLock)
			{
				string hitEffectId37 = "EFF_COM_CRYSTALATTACK";
				IEnumerator item38 = this.LoadHitEffects(hitEffectId37, AffectEffect.SkillLock, true);
				list.Add(item38);
			}
			else if (affectEffect == AffectEffect.SatisfactionRateDown)
			{
				string hitEffectId38 = "EFF_COM_DOWN";
				IEnumerator item39 = this.LoadHitEffects(hitEffectId38, AffectEffect.SatisfactionRateDown, true);
				list.Add(item39);
			}
			else if (affectEffect == AffectEffect.SatisfactionRateUp)
			{
				string hitEffectId39 = "EFF_COM_UP";
				IEnumerator item40 = this.LoadHitEffects(hitEffectId39, AffectEffect.SatisfactionRateUp, true);
				list.Add(item40);
			}
			else if (affectEffect == AffectEffect.ApRevival)
			{
				string hitEffectId40 = "EFF_COM_M_HEAL";
				IEnumerator item41 = this.LoadHitEffects(hitEffectId40, AffectEffect.ApRevival, false);
				list.Add(item41);
			}
			else if (affectEffect == AffectEffect.ApUp)
			{
				string hitEffectId41 = "EFF_COM_UP";
				IEnumerator item42 = this.LoadHitEffects(hitEffectId41, AffectEffect.ApUp, false);
				list.Add(item42);
			}
			else if (affectEffect == AffectEffect.ApDown)
			{
				string hitEffectId42 = "EFF_COM_DOWN";
				IEnumerator item43 = this.LoadHitEffects(hitEffectId42, AffectEffect.ApDown, false);
				list.Add(item43);
			}
			else if (affectEffect == AffectEffect.ApConsumptionUp)
			{
				string hitEffectId43 = "EFF_COM_DOWN";
				IEnumerator item44 = this.LoadHitEffects(hitEffectId43, AffectEffect.ApConsumptionUp, false);
				list.Add(item44);
			}
			else if (affectEffect == AffectEffect.ApConsumptionDown)
			{
				string hitEffectId44 = "EFF_COM_UP";
				IEnumerator item45 = this.LoadHitEffects(hitEffectId44, AffectEffect.ApConsumptionDown, false);
				list.Add(item45);
			}
			else if (affectEffect == AffectEffect.TurnBarrier)
			{
				string hitEffectId45 = "EFF_COM_GUARD";
				IEnumerator item46 = this.LoadHitEffects(hitEffectId45, AffectEffect.TurnBarrier, true);
				list.Add(item46);
			}
			else if (affectEffect == AffectEffect.CountBarrier)
			{
				string hitEffectId46 = "EFF_COM_GUARD";
				IEnumerator item47 = this.LoadHitEffects(hitEffectId46, AffectEffect.CountBarrier, true);
				list.Add(item47);
			}
			else if (affectEffect == AffectEffect.CountGuard)
			{
				string hitEffectId47 = "EFF_COM_GUARD";
				IEnumerator item48 = this.LoadHitEffects(hitEffectId47, AffectEffect.CountGuard, true);
				list.Add(item48);
			}
			else if (affectEffect == AffectEffect.Recommand)
			{
				string hitEffectId48 = "EFF_COM_M_HEAL";
				IEnumerator item49 = this.LoadHitEffects(hitEffectId48, AffectEffect.Recommand, false);
				list.Add(item49);
			}
			else if (affectEffect == AffectEffect.DamageRateUp)
			{
				string hitEffectId49 = "EFF_COM_UP";
				IEnumerator item50 = this.LoadHitEffects(hitEffectId49, AffectEffect.DamageRateUp, true);
				list.Add(item50);
			}
			else if (affectEffect == AffectEffect.DamageRateDown)
			{
				string hitEffectId50 = "EFF_COM_DOWN";
				IEnumerator item51 = this.LoadHitEffects(hitEffectId50, AffectEffect.DamageRateDown, true);
				list.Add(item51);
			}
			else if (affectEffect == AffectEffect.Regenerate)
			{
				string hitEffectId51 = "EFF_COM_M_HEAL";
				IEnumerator item52 = this.LoadHitEffects(hitEffectId51, AffectEffect.Regenerate, true);
				list.Add(item52);
			}
			else if (affectEffect == AffectEffect.TurnEvasion)
			{
				string hitEffectId52 = "EFF_COM_UP";
				IEnumerator item53 = this.LoadHitEffects(hitEffectId52, AffectEffect.TurnEvasion, true);
				list.Add(item53);
			}
			else if (affectEffect == AffectEffect.CountEvasion)
			{
				string hitEffectId53 = "EFF_COM_UP";
				IEnumerator item54 = this.LoadHitEffects(hitEffectId53, AffectEffect.CountEvasion, true);
				list.Add(item54);
			}
			else if (affectEffect == AffectEffect.ReferenceTargetHpRate)
			{
				string hitEffectId54 = "EFF_COM_HIT_RATIO";
				IEnumerator item55 = this.LoadHitEffects(hitEffectId54, AffectEffect.ReferenceTargetHpRate, true);
				list.Add(item55);
			}
			else if (affectEffect == AffectEffect.ApDrain)
			{
				string hitEffectId55 = "EFF_COM_S_APDRAIN";
				IEnumerator item56 = this.LoadHitEffects(hitEffectId55, AffectEffect.ApDrain, false);
				list.Add(item56);
				string hitEffectId56 = "EFF_COM_DOWN";
				IEnumerator item57 = this.LoadHitEffects(hitEffectId56, AffectEffect.ApDown, false);
				list.Add(item57);
			}
		}
		return list;
	}

	private IEnumerator LoadHitEffects(string hitEffectId, AffectEffect affectEffect, bool isTotal = false)
	{
		BattleDebug.Log(string.Concat(new string[]
		{
			"-- ヒットエフェクト複数ロード hitEffectId[",
			hitEffectId,
			"] affectEffect[",
			affectEffect.ToString(),
			"] : 開始"
		}));
		IEnumerator<HitEffectParams[]> loadEffect = this.LoadHitEffects(hitEffectId, isTotal);
		while (loadEffect.MoveNext())
		{
			yield return null;
		}
		base.battleStateData.hitEffects.Add(affectEffect.ToString(), loadEffect.Current);
		BattleDebug.Log(string.Concat(new string[]
		{
			"-- ヒットエフェクト複数ロード hitEffectId[",
			hitEffectId,
			"] affectEffect[",
			affectEffect.ToString(),
			"] : 完了"
		}));
		yield break;
	}

	private IEnumerator LoadHitEffects(string hitEffectId, AffectEffect affectEffect, Strength strength, bool isTotal = false)
	{
		BattleDebug.Log(string.Concat(new string[]
		{
			"-- ヒットエフェクト複数ロード hitEffectId[",
			hitEffectId,
			"] affectEffect[",
			affectEffect.ToString(),
			"] strength[",
			strength.ToString(),
			"] : 開始"
		}));
		IEnumerator<HitEffectParams[]> loadEffect = this.LoadHitEffects(hitEffectId, isTotal);
		while (loadEffect.MoveNext())
		{
			yield return null;
		}
		base.battleStateData.hitEffects.Add(affectEffect.ToString(), strength.ToString(), loadEffect.Current);
		BattleDebug.Log(string.Concat(new string[]
		{
			"-- ヒットエフェクト複数ロード hitEffectId[",
			hitEffectId,
			"] affectEffect[",
			affectEffect.ToString(),
			"] strength[",
			strength.ToString(),
			"] : 完了"
		}));
		yield break;
	}

	private IEnumerator<HitEffectParams[]> LoadHitEffects(string hitEffectId, bool isTotal = false)
	{
		List<HitEffectParams> hitEffectLoadedObjects = null;
		this.hitEffectObject.TryGetValue(hitEffectId, out hitEffectLoadedObjects);
		int createCount = base.battleStateData.maxCharacterLength;
		if (isTotal)
		{
			createCount = base.battleStateData.totalPreloadCharacterLength;
		}
		int hitEffectLoadedObjectsLength = 0;
		if (hitEffectLoadedObjects != null)
		{
			hitEffectLoadedObjectsLength = hitEffectLoadedObjects.Count;
		}
		createCount = Math.Max(createCount - hitEffectLoadedObjectsLength, 0);
		Transform thisHitEffectRoot = new GameObject(hitEffectId).transform;
		thisHitEffectRoot.SetParent(base.battleStateData.hitEffectRoot);
		HitEffectParams[] hitEffectParams = new HitEffectParams[createCount];
		for (int i = 0; i < createCount; i++)
		{
			IEnumerator loadHitEffect = this.LoadHitEffect(hitEffectId, i, delegate(HitEffectParams h, int index)
			{
				hitEffectParams[index] = h;
				h.transform.SetParent(thisHitEffectRoot);
			});
			while (loadHitEffect.MoveNext())
			{
				yield return null;
			}
		}
		if (!this.hitEffectObject.ContainsKey(hitEffectId))
		{
			this.hitEffectObject[hitEffectId] = new List<HitEffectParams>();
		}
		this.hitEffectObject[hitEffectId].AddRange(hitEffectParams);
		yield return this.hitEffectObject[hitEffectId].ToArray();
		yield break;
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
