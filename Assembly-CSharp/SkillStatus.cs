using System;
using System.Collections.Generic;
using UnityEngine;
using UnityExtension;

[Serializable]
public class SkillStatus
{
	public const int maxAp = 5;

	public static bool onHitRate100Percent;

	private static string COUNTER_ATTACK_SKILL_ID = "50001";

	[SerializeField]
	private string _name = string.Empty;

	[SerializeField]
	[Multiline(2)]
	private string _description = string.Empty;

	[SerializeField]
	private string _prefabId = string.Empty;

	[SerializeField]
	private string _seId = string.Empty;

	[SerializeField]
	private SkillType _skillType;

	[SerializeField]
	private EffectTarget _target;

	[SerializeField]
	private int _needAp;

	[SerializeField]
	private List<AffectEffectProperty> _affectEffect = new List<AffectEffectProperty>();

	private EffectNumbers _numbers;

	private string _useCountType = string.Empty;

	private int _useCountValue;

	private List<AffectEffectProperty> _addAffectEffect = new List<AffectEffectProperty>();

	private List<AffectEffectProperty> _returnAffectEffect = new List<AffectEffectProperty>();

	private InvocationEffectParams _instancedInvocationEffectParams;

	private PassiveEffectParams[] _instancedPassiveEffectParams = new PassiveEffectParams[0];

	public SkillStatus()
	{
		this._prefabId = string.Empty;
		this._seId = string.Empty;
		this._skillType = SkillType.Attack;
		this._name = string.Empty;
		this._description = string.Empty;
		this._target = EffectTarget.Enemy;
		this._numbers = EffectNumbers.Simple;
		this._needAp = 0;
		this._affectEffect = new List<AffectEffectProperty>();
		this._affectEffect.Add(new AffectEffectProperty());
		this._returnAffectEffect.AddRange(this._affectEffect);
	}

	public SkillStatus(string skillId, string prefabId, string seId, SkillType skillType, string name, string description, EffectTarget target, EffectNumbers numbers, int needAp, string useCountType, int useCountValue, params AffectEffectProperty[] affectEffect)
	{
		this.skillId = skillId;
		this._prefabId = prefabId;
		this._seId = seId;
		this._skillType = skillType;
		this._name = name;
		this._description = description;
		this._target = target;
		this._numbers = numbers;
		this._needAp = needAp;
		this._useCountType = useCountType;
		this._useCountValue = useCountValue;
		this._affectEffect = new List<AffectEffectProperty>(affectEffect);
		this._returnAffectEffect.AddRange(this._affectEffect);
	}

	public string skillId { get; private set; }

	public string useCountType
	{
		get
		{
			return this._useCountType;
		}
	}

	public int useCountValue
	{
		get
		{
			return this._useCountValue;
		}
	}

	public bool ThisSkillIsAttack
	{
		get
		{
			foreach (AffectEffectProperty affectEffectProperty in this.affectEffect)
			{
				if (affectEffectProperty.ThisSkillIsAttack)
				{
					return true;
				}
			}
			return false;
		}
	}

	public int AttributeMachLevel(global::Attribute attribute)
	{
		int num = 0;
		for (int i = 0; i < this.affectEffect.Count; i++)
		{
			if (this.affectEffect[i].ThisSkillIsAttack && this.affectEffect[i].attribute == attribute)
			{
				num++;
			}
		}
		return num;
	}

	public bool TryGetInvocationSEID(out string Id)
	{
		string text = string.Empty;
		SkillType skillType = this._skillType;
		if (skillType != SkillType.Deathblow)
		{
			if (skillType != SkillType.InheritanceTechnique)
			{
				text = "bt_207";
			}
			else
			{
				text = "bt_010";
			}
		}
		else
		{
			text = this.seId;
		}
		Id = text;
		return !string.IsNullOrEmpty(text);
	}

	public bool TryGetPassiveSEID(out string Id)
	{
		string text = string.Empty;
		SkillType skillType = this._skillType;
		if (skillType != SkillType.Deathblow)
		{
			if (skillType != SkillType.InheritanceTechnique)
			{
				text = string.Empty;
			}
			else
			{
				text = this.seId;
			}
		}
		else
		{
			text = string.Empty;
		}
		Id = text;
		return text != string.Empty;
	}

	public string prefabId
	{
		get
		{
			return this._prefabId;
		}
	}

	public string seId
	{
		get
		{
			return this._seId;
		}
	}

	public SkillType skillType
	{
		get
		{
			return this._skillType;
		}
	}

	public string name
	{
		get
		{
			return this._name;
		}
	}

	public string description
	{
		get
		{
			return this._description;
		}
	}

	public EffectTarget target
	{
		get
		{
			return this._target;
		}
	}

	public int needAp
	{
		get
		{
			return this._needAp;
		}
		private set
		{
			this._needAp = Mathf.Clamp(value, 0, 5);
		}
	}

	public List<AffectEffectProperty> affectEffect
	{
		get
		{
			return this._returnAffectEffect;
		}
		private set
		{
			this._affectEffect = value;
			this._returnAffectEffect.Clear();
			this._returnAffectEffect.AddRange(this._affectEffect);
			this._returnAffectEffect.AddRange(this._addAffectEffect);
		}
	}

	public void AddAffectEffect(AffectEffectProperty addAffectEffect)
	{
		this._addAffectEffect.Add(addAffectEffect);
		this._returnAffectEffect.Clear();
		this._returnAffectEffect.AddRange(this._affectEffect);
		this._returnAffectEffect.AddRange(this._addAffectEffect);
	}

	public void ClearAffectEffect()
	{
		this._addAffectEffect.Clear();
		this._returnAffectEffect.Clear();
		this._returnAffectEffect.AddRange(this._affectEffect);
	}

	public int GetPowerFirst(CharacterStateControl characterStateControl = null)
	{
		AffectEffectProperty affectEffectAttackFirst = this.GetAffectEffectAttackFirst();
		return (affectEffectAttackFirst == null) ? 0 : affectEffectAttackFirst.GetPower(characterStateControl);
	}

	public EffectNumbers numbers
	{
		get
		{
			return this._numbers;
		}
	}

	public global::Attribute attribute
	{
		get
		{
			AffectEffectProperty affectEffectAttackFirst = this.GetAffectEffectAttackFirst();
			return (affectEffectAttackFirst == null) ? global::Attribute.None : affectEffectAttackFirst.attribute;
		}
	}

	public float hitRate
	{
		get
		{
			return (this.affectEffect.Count <= 0) ? 0f : this.affectEffect[0].hitRate;
		}
	}

	public AffectEffectProperty GetAffectEffectFirst()
	{
		foreach (AffectEffectProperty affectEffectProperty in this.affectEffect)
		{
			if (affectEffectProperty != null)
			{
				return affectEffectProperty;
			}
		}
		return null;
	}

	private AffectEffectProperty GetAffectEffectAttackFirst()
	{
		foreach (AffectEffectProperty affectEffectProperty in this.affectEffect)
		{
			if (affectEffectProperty != null && affectEffectProperty.ThisSkillIsAttack)
			{
				return affectEffectProperty;
			}
		}
		return null;
	}

	public InvocationEffectParams invocationEffectParams
	{
		get
		{
			return this._instancedInvocationEffectParams;
		}
		set
		{
			this._instancedInvocationEffectParams = value;
		}
	}

	public PassiveEffectParams[] passiveEffectParams
	{
		get
		{
			return this._instancedPassiveEffectParams;
		}
		set
		{
			this._instancedPassiveEffectParams = value;
		}
	}

	public static SkillResults GetSkillResults(AffectEffectProperty affectEffectProperty, CharacterStateControl attackerCharacter, CharacterStateControl targetCharacter, bool onRandomAttack = true, int attackNum = 0)
	{
		SkillResults skillResults = new SkillResults();
		LeaderSkillResult leaderSkillResult = attackerCharacter.leaderSkillResult;
		HaveSufferState currentSufferState = attackerCharacter.currentSufferState;
		HaveSufferState currentSufferState2 = targetCharacter.currentSufferState;
		float[] array = new float[2];
		skillResults.useAffectEffectProperty = affectEffectProperty;
		skillResults.hitIconAffectEffect = AffectEffect.Damage;
		skillResults.attackCharacter = attackerCharacter;
		skillResults.targetCharacter = targetCharacter;
		if (targetCharacter.isDied)
		{
			return skillResults;
		}
		skillResults.onWeakHit = ((!affectEffectProperty.canUseAttribute) ? Strength.None : targetCharacter.tolerance.GetAttributeStrength(affectEffectProperty.attribute));
		if (attackerCharacter.hittingTheTargetType == BattleInvariant.Type.Non)
		{
			skillResults.onMissHit = !affectEffectProperty.OnHit(attackerCharacter, targetCharacter);
		}
		else if (attackerCharacter.hittingTheTargetType == BattleInvariant.Type.Up)
		{
			skillResults.onMissHit = false;
		}
		else if (attackerCharacter.hittingTheTargetType == BattleInvariant.Type.Down)
		{
			skillResults.onMissHit = true;
		}
		if (skillResults.onWeakHit == Strength.None || skillResults.onWeakHit == Strength.Weak)
		{
			if (attackerCharacter.criticalTheTargetType == BattleInvariant.Type.Non)
			{
				float num = affectEffectProperty.satisfactionRate;
				SufferStateProperty sufferStateProperty = currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.SatisfactionRateUp);
				if (sufferStateProperty.isActive)
				{
					num += sufferStateProperty.upPercent;
				}
				SufferStateProperty sufferStateProperty2 = currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.SatisfactionRateDown);
				if (sufferStateProperty2.isActive)
				{
					num -= sufferStateProperty2.downPercent;
				}
				if (attackerCharacter.isSelectSkill > 0)
				{
					num += leaderSkillResult.satisfactionRateUpPercent;
				}
				num += attackerCharacter.chipAddCritical;
				skillResults.onCriticalHit = RandomExtension.Switch(num);
			}
			else if (attackerCharacter.criticalTheTargetType == BattleInvariant.Type.Up)
			{
				skillResults.onCriticalHit = true;
			}
			else if (attackerCharacter.criticalTheTargetType == BattleInvariant.Type.Down)
			{
				skillResults.onCriticalHit = false;
			}
		}
		if (!skillResults.onMissHit)
		{
			if (affectEffectProperty.powerType == PowerType.Percentage && skillResults.onWeakHit == Strength.Invalid)
			{
				skillResults.hitIconAffectEffect = AffectEffect.Invalid;
				skillResults.onMissHit = false;
				skillResults.onWeakHit = Strength.Invalid;
				skillResults.onCriticalHit = false;
			}
			else if (skillResults.targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.TurnBarrier))
			{
				skillResults.hitIconAffectEffect = AffectEffect.TurnBarrier;
				skillResults.onMissHit = false;
				skillResults.onWeakHit = Strength.None;
				skillResults.onCriticalHit = false;
			}
			else if (skillResults.targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountBarrier))
			{
				skillResults.hitIconAffectEffect = AffectEffect.CountBarrier;
				skillResults.onMissHit = false;
				skillResults.onWeakHit = Strength.None;
				skillResults.onCriticalHit = false;
			}
			else if (skillResults.targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.TurnEvasion))
			{
				skillResults.hitIconAffectEffect = AffectEffect.TurnEvasion;
				skillResults.onMissHit = false;
				skillResults.onWeakHit = Strength.None;
				skillResults.onCriticalHit = false;
			}
			else if (skillResults.targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountEvasion))
			{
				skillResults.hitIconAffectEffect = AffectEffect.CountEvasion;
				skillResults.onMissHit = false;
				skillResults.onWeakHit = Strength.None;
				skillResults.onCriticalHit = false;
			}
			else if (affectEffectProperty.type == AffectEffect.ReferenceTargetHpRate)
			{
				SufferStateProperty.DamageRateResult reduceDamageRate = SkillStatus.GetReduceDamageRate(affectEffectProperty, attackerCharacter, targetCharacter);
				skillResults.damageRateResult = reduceDamageRate;
				float attributeDamegeResult = SkillStatus.GetAttributeDamegeResult(skillResults.onWeakHit);
				float num2 = 1f;
				if (skillResults.onCriticalHit)
				{
					num2 = 1.2f;
				}
				int num3 = (int)(affectEffectProperty.damagePercent * (float)targetCharacter.hp);
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = (float)num3 * attributeDamegeResult * num2 * reduceDamageRate.damageRate;
				}
			}
			else if (affectEffectProperty.type == AffectEffect.RefHpRateNonAttribute)
			{
				SufferStateProperty.DamageRateResult reduceDamageRate2 = SkillStatus.GetReduceDamageRate(affectEffectProperty, attackerCharacter, targetCharacter);
				skillResults.damageRateResult = reduceDamageRate2;
				float num4 = 1f;
				if (skillResults.onCriticalHit)
				{
					num4 = 1.2f;
				}
				int num5 = (int)(affectEffectProperty.damagePercent * (float)targetCharacter.hp);
				for (int j = 0; j < array.Length; j++)
				{
					array[j] = (float)num5 * num4 * reduceDamageRate2.damageRate;
				}
			}
			else if (affectEffectProperty.powerType == PowerType.Percentage)
			{
				bool flag = affectEffectProperty.techniqueType == TechniqueType.Physics;
				float attributeDamegeResult2 = SkillStatus.GetAttributeDamegeResult(skillResults.onWeakHit);
				float num6 = 1f;
				if (skillResults.onCriticalHit)
				{
					num6 = 1.2f;
				}
				float num7 = 1f;
				if (onRandomAttack)
				{
					num7 = UnityEngine.Random.Range(0.85f, 1f);
				}
				SufferStateProperty.DamageRateResult reduceDamageRate3 = SkillStatus.GetReduceDamageRate(affectEffectProperty, attackerCharacter, targetCharacter);
				skillResults.damageRateResult = reduceDamageRate3;
				float num8 = 1f;
				if (flag)
				{
					SufferStateProperty sufferStateProperty3 = targetCharacter.currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.Counter);
					if (sufferStateProperty3.isActive)
					{
						num8 *= sufferStateProperty3.recieveDamageRate;
					}
				}
				else
				{
					SufferStateProperty sufferStateProperty4 = targetCharacter.currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.Reflection);
					if (sufferStateProperty4.isActive)
					{
						num8 *= sufferStateProperty4.recieveDamageRate;
					}
				}
				float num9 = 1f;
				SufferStateProperty sufferStateProperty5 = currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.PowerCharge);
				if (sufferStateProperty5.isActive)
				{
					if (flag)
					{
						num9 *= sufferStateProperty5.physicUpPercent;
					}
					else
					{
						num9 *= sufferStateProperty5.specialUpPercent;
					}
				}
				float num10 = 1f;
				float num11 = 0f;
				float num12 = 0f;
				SufferStateProperty sufferStateProperty6 = currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.DamageRateUp);
				if (sufferStateProperty6.isActive)
				{
					SufferStateProperty.DamageRateResult caseDamageRate = sufferStateProperty6.GetCaseDamageRate(affectEffectProperty, targetCharacter);
					num11 += caseDamageRate.damageRate;
				}
				SufferStateProperty sufferStateProperty7 = currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.DamageRateDown);
				if (sufferStateProperty7.isActive)
				{
					SufferStateProperty.DamageRateResult caseDamageRate2 = sufferStateProperty7.GetCaseDamageRate(affectEffectProperty, targetCharacter);
					num12 += caseDamageRate2.damageRate;
				}
				num10 = Mathf.Max(0f, num10 + num11 - num12);
				for (int k = 0; k < array.Length; k++)
				{
					int num13 = 0;
					float num14 = 1f;
					foreach (int num15 in attackerCharacter.potencyChipIdList.Keys)
					{
						GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffectDataToId = ChipDataMng.GetChipEffectDataToId(num15.ToString());
						if (k != 0 || chipEffectDataToId.effectTrigger.ToInt32() != 11)
						{
							GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects = new GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[]
							{
								chipEffectDataToId
							};
							num13 += ChipEffectStatus.GetSkillPowerCorrectionValue(chipEffects, affectEffectProperty, attackerCharacter, attackNum);
							num14 += ChipEffectStatus.GetSkillDamageCorrectionValue(chipEffects, affectEffectProperty, attackerCharacter);
						}
					}
					float num16 = 1f;
					foreach (int num17 in targetCharacter.potencyChipIdList.Keys)
					{
						GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffectDataToId2 = ChipDataMng.GetChipEffectDataToId(num17.ToString());
						if (k != 0 || chipEffectDataToId2.effectTrigger.ToInt32() != 11)
						{
							GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects2 = new GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[]
							{
								chipEffectDataToId2
							};
							num16 += ChipEffectStatus.GetSkillDamageCorrectionValue(chipEffects2, affectEffectProperty, targetCharacter);
						}
					}
					float num18 = 0f;
					float num19 = 0f;
					SkillStatus.CalculationBasePower(ref num18, ref num19, flag, k == 0, attackerCharacter, targetCharacter);
					int num20;
					if (k == 0)
					{
						num20 = affectEffectProperty.GetPower(attackerCharacter);
					}
					else
					{
						List<ExtraEffectStatus> extraEffectStatus = BattleStateManager.current.battleStateData.extraEffectStatus;
						List<ExtraEffectStatus> invocationList = ExtraEffectStatus.GetInvocationList(extraEffectStatus, EffectStatusBase.EffectTriggerType.Usually);
						num20 = ExtraEffectStatus.GetSkillPowerCorrectionValue(invocationList, affectEffectProperty, attackerCharacter);
					}
					if (affectEffectProperty.type == AffectEffect.DefenseThroughDamage || affectEffectProperty.type == AffectEffect.SpDefenseThroughDamage)
					{
						num19 = (float)affectEffectProperty.DefenseThrough;
					}
					if (num18 <= 0f)
					{
						array[k] = 0f;
					}
					else
					{
						num18 = Mathf.Max(num18, 0f);
						num19 = Mathf.Max(num19, 1f);
						float num21 = (float)attackerCharacter.level * 0.01f + 1f;
						float num22 = (float)(num20 + num13) * (1f + leaderSkillResult.damageUpPercent);
						float num23 = num21 * num22 * num18 * num9 / num19 + 2f;
						float num24 = num14 * num16 * reduceDamageRate3.damageRate * num8 * num7 * num6 * attributeDamegeResult2 * num10;
						array[k] = num23 * num24;
					}
				}
			}
			else if (affectEffectProperty.powerType == PowerType.Fixable)
			{
				for (int l = 0; l < array.Length; l++)
				{
					array[l] = (float)affectEffectProperty.damagePower;
				}
				skillResults.onWeakHit = Strength.None;
			}
		}
		skillResults.originalAttackPower = Mathf.FloorToInt(array[0]);
		skillResults.attackPower = Mathf.FloorToInt(array[1]);
		if (skillResults.hitIconAffectEffect == AffectEffect.Damage && skillResults.attackPower <= 0)
		{
			skillResults.onMissHit = true;
			skillResults.onCriticalHit = false;
		}
		if (skillResults.onWeakHit != Strength.Drain)
		{
			targetCharacter.hp -= skillResults.attackPower;
		}
		else
		{
			targetCharacter.hp += skillResults.attackPower;
		}
		if (skillResults.attackPower < skillResults.originalAttackPower)
		{
			skillResults.extraEffectType = ExtraEffectType.Down;
		}
		else if (skillResults.attackPower > skillResults.originalAttackPower)
		{
			skillResults.extraEffectType = ExtraEffectType.Up;
		}
		else
		{
			skillResults.extraEffectType = ExtraEffectType.Non;
		}
		return skillResults;
	}

	private static void CalculationBasePower(ref float attackCalced, ref float defenceCalced, bool isPhysics, bool isExtra, CharacterStateControl attackerCharacter, CharacterStateControl targetCharacter)
	{
		HaveSufferState currentSufferState = attackerCharacter.currentSufferState;
		HaveSufferState currentSufferState2 = targetCharacter.currentSufferState;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		int num5;
		int num6;
		if (isExtra)
		{
			num5 = ((!isPhysics) ? attackerCharacter.specialAttackPower : attackerCharacter.attackPower);
			num6 = ((!isPhysics) ? targetCharacter.specialDefencePower : targetCharacter.defencePower);
		}
		else
		{
			num5 = ((!isPhysics) ? attackerCharacter.extraSpecialAttackPower : attackerCharacter.extraAttackPower);
			num6 = ((!isPhysics) ? targetCharacter.extraSpecialDefencePower : targetCharacter.extraDefencePower);
		}
		float num7 = (!isPhysics) ? attackerCharacter.leaderSkillResult.specialAttackUpPercent : attackerCharacter.leaderSkillResult.attackUpPercent;
		float num8 = (!isPhysics) ? attackerCharacter.leaderSkillResult.specialDefenceUpPercent : targetCharacter.leaderSkillResult.defenceUpPercent;
		float num9 = num7 * (float)num5;
		float num10 = num8 * (float)num6;
		global::Debug.Log(string.Format("onLeaderAttackUp {0} | onLeaderDefenceUp {1}", num9, num10));
		SufferStateProperty.SufferType sufferType = SufferStateProperty.SufferType.AttackUp;
		SufferStateProperty.SufferType sufferType2 = SufferStateProperty.SufferType.AttackDown;
		SufferStateProperty.SufferType sufferType3 = SufferStateProperty.SufferType.DefenceUp;
		SufferStateProperty.SufferType sufferType4 = SufferStateProperty.SufferType.DefenceDown;
		if (!isPhysics)
		{
			sufferType = SufferStateProperty.SufferType.SpAttackUp;
			sufferType2 = SufferStateProperty.SufferType.SpAttackDown;
			sufferType3 = SufferStateProperty.SufferType.SpDefenceUp;
			sufferType4 = SufferStateProperty.SufferType.SpDefenceDown;
		}
		SufferStateProperty sufferStateProperty = currentSufferState.GetSufferStateProperty(sufferType);
		if (sufferStateProperty.isActive)
		{
			num = sufferStateProperty.upPercent * (float)num5;
		}
		SufferStateProperty sufferStateProperty2 = currentSufferState.GetSufferStateProperty(sufferType2);
		if (sufferStateProperty2.isActive)
		{
			num2 = sufferStateProperty2.downPercent * (float)num5;
		}
		SufferStateProperty sufferStateProperty3 = currentSufferState2.GetSufferStateProperty(sufferType3);
		if (sufferStateProperty3.isActive)
		{
			num3 = sufferStateProperty3.upPercent * (float)num6;
		}
		SufferStateProperty sufferStateProperty4 = currentSufferState2.GetSufferStateProperty(sufferType4);
		if (sufferStateProperty4.isActive)
		{
			num4 = sufferStateProperty4.downPercent * (float)num6;
		}
		attackCalced = (float)num5 + num - num2 + num9;
		defenceCalced = (float)num6 + num3 - num4 + num10;
		attackCalced = Mathf.Min(attackCalced, (float)num5 * 2.2f);
		attackCalced = Mathf.Max(attackCalced, (float)num5 * 0.6f);
		defenceCalced = Mathf.Min(defenceCalced, (float)num6 * 2.2f);
		defenceCalced = Mathf.Max(defenceCalced, (float)num6 * 0.6f);
	}

	public static SkillResults GetStageDamageSkillResult(AffectEffectProperty affectEffectProperty, CharacterStateControl attackerCharacter, CharacterStateControl targetCharacter)
	{
		SkillResults skillResults = new SkillResults();
		skillResults.useAffectEffectProperty = affectEffectProperty;
		skillResults.hitIconAffectEffect = AffectEffect.Damage;
		skillResults.attackCharacter = null;
		skillResults.targetCharacter = targetCharacter;
		skillResults.onMissHit = !affectEffectProperty.OnHit(attackerCharacter, targetCharacter);
		skillResults.onWeakHit = targetCharacter.tolerance.GetAttributeStrength(affectEffectProperty.attribute);
		if (skillResults.onMissHit)
		{
			return skillResults;
		}
		if (affectEffectProperty.powerType == PowerType.Percentage && skillResults.onWeakHit == Strength.Invalid)
		{
			skillResults.hitIconAffectEffect = AffectEffect.Invalid;
		}
		else if (targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.TurnBarrier))
		{
			skillResults.hitIconAffectEffect = AffectEffect.TurnBarrier;
		}
		else if (targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountBarrier))
		{
			skillResults.hitIconAffectEffect = AffectEffect.CountBarrier;
		}
		else if (targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.TurnEvasion))
		{
			skillResults.hitIconAffectEffect = AffectEffect.TurnEvasion;
		}
		else if (targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountEvasion))
		{
			skillResults.hitIconAffectEffect = AffectEffect.CountEvasion;
		}
		else
		{
			int num;
			if (affectEffectProperty.powerType == PowerType.Fixable)
			{
				num = affectEffectProperty.damagePower;
				skillResults.onWeakHit = Strength.None;
			}
			else
			{
				SufferStateProperty.DamageRateResult reduceDamageRate = SkillStatus.GetReduceDamageRate(affectEffectProperty, attackerCharacter, targetCharacter);
				skillResults.damageRateResult = reduceDamageRate;
				float attributeDamegeResult = SkillStatus.GetAttributeDamegeResult(skillResults.onWeakHit);
				num = (int)(affectEffectProperty.damagePercent * (float)targetCharacter.hp * reduceDamageRate.damageRate * attributeDamegeResult);
			}
			if (skillResults.onWeakHit != Strength.Drain)
			{
				targetCharacter.hp -= num;
			}
			else
			{
				targetCharacter.hp += num;
			}
			skillResults.attackPower = num;
			skillResults.originalAttackPower = num;
		}
		return skillResults;
	}

	private static SufferStateProperty.DamageRateResult GetReduceDamageRate(AffectEffectProperty affectEffectProperty, CharacterStateControl attackerCharacter, CharacterStateControl targetCharacter)
	{
		HaveSufferState currentSufferState = targetCharacter.currentSufferState;
		SufferStateProperty.DamageRateResult damageRateResult = new SufferStateProperty.DamageRateResult();
		damageRateResult.damageRate = 1f;
		if (affectEffectProperty.powerType == PowerType.Percentage && currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountGuard))
		{
			SufferStateProperty sufferStateProperty = currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.CountGuard);
			SufferStateProperty.DamageRateResult caseDamageRate = sufferStateProperty.GetCaseDamageRate(affectEffectProperty, attackerCharacter);
			if (BattleStateManager.current.battleStateData.IsChipSkill())
			{
				CharacterStateControl autoCounterCharacter = BattleStateManager.current.battleStateData.GetAutoCounterCharacter();
				if (autoCounterCharacter.chipSkillId == SkillStatus.COUNTER_ATTACK_SKILL_ID)
				{
					caseDamageRate.damageRate = 0f;
				}
			}
			float damageRate = Mathf.Max(1f - caseDamageRate.damageRate, 0f);
			damageRateResult.damageRate = damageRate;
			damageRateResult.dataList.AddRange(caseDamageRate.dataList);
		}
		return damageRateResult;
	}

	private static float GetAttributeDamegeResult(Strength strength)
	{
		switch (strength)
		{
		case Strength.None:
			return 1f;
		case Strength.Strong:
			return 0.5f;
		case Strength.Weak:
			return 1.5f;
		case Strength.Drain:
			return 1f;
		case Strength.Invalid:
			return 0f;
		default:
			return 1f;
		}
	}

	public void OnAttackUseAttackerAp(CharacterStateControl attackCharacter)
	{
		attackCharacter.ap -= this.GetCorrectedAp(attackCharacter);
	}

	public Strength GetSkillStrength(Tolerance tolerance)
	{
		int num = 0;
		foreach (AffectEffectProperty affectEffectProperty in this.affectEffect)
		{
			if (affectEffectProperty != null)
			{
				Strength strength = Strength.None;
				if (affectEffectProperty.ThisSkillIsAttack && affectEffectProperty.powerType == PowerType.Percentage)
				{
					strength = tolerance.GetAttributeStrength(affectEffectProperty.attribute);
				}
				else if (Tolerance.OnInfluenceToleranceAffectEffect(affectEffectProperty.type))
				{
					strength = tolerance.GetAffectEffectStrength(affectEffectProperty.type);
				}
				switch (strength)
				{
				case Strength.Strong:
					num--;
					break;
				case Strength.Weak:
					num++;
					break;
				case Strength.Drain:
					return Strength.Drain;
				case Strength.Invalid:
					return Strength.Weak;
				}
			}
		}
		if (num < 0)
		{
			return Strength.Weak;
		}
		if (num > 0)
		{
			return Strength.Strong;
		}
		return Strength.None;
	}

	public Strength[] GetSkillStrengthList(Tolerance tolerance)
	{
		List<Strength> list = new List<Strength>();
		foreach (AffectEffectProperty affectEffectProperty in this.affectEffect)
		{
			if (affectEffectProperty != null)
			{
				if (affectEffectProperty.ThisSkillIsAttack && affectEffectProperty.canUseAttribute && affectEffectProperty.powerType == PowerType.Percentage)
				{
					Strength attributeStrength = tolerance.GetAttributeStrength(affectEffectProperty.attribute);
					if (!list.Contains(attributeStrength))
					{
						list.Add(attributeStrength);
					}
				}
			}
		}
		if (list.Count == 0)
		{
			list.Add(Strength.None);
		}
		return list.ToArray();
	}

	public int GetCorrectedAp(CharacterStateControl user)
	{
		int num = this.needAp;
		SufferStateProperty sufferStateProperty = user.currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.ApConsumptionUp);
		if (sufferStateProperty.isActive)
		{
			num += sufferStateProperty.upPower;
		}
		SufferStateProperty sufferStateProperty2 = user.currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.ApConsumptionDown);
		if (sufferStateProperty2.isActive)
		{
			num -= sufferStateProperty2.downPower;
		}
		return num;
	}

	public bool IsNotingAffectEffect()
	{
		foreach (AffectEffectProperty affectEffectProperty in this.affectEffect)
		{
			if (affectEffectProperty != null && affectEffectProperty.type == AffectEffect.Nothing)
			{
				return true;
			}
		}
		return false;
	}
}
