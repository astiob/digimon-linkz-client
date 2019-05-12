using System;
using System.Collections.Generic;
using UnityEngine;
using UnityExtension;

[Serializable]
public class SkillStatus
{
	public const int maxAp = 5;

	public static bool onHitRate100Percent;

	[SerializeField]
	private string _name = string.Empty;

	[Multiline(2)]
	[SerializeField]
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

	public SkillStatus(string skillId, string prefabId, string seId, SkillType skillType, string name, string description, EffectTarget target, EffectNumbers numbers, int needAp, params AffectEffectProperty[] affectEffect)
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
		this._affectEffect = new List<AffectEffectProperty>(affectEffect);
		this._returnAffectEffect.AddRange(this._affectEffect);
	}

	public string skillId { get; private set; }

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
		skillResults.onWeakHit = targetCharacter.tolerance.GetAttributeStrength(affectEffectProperty.attribute);
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
			if (skillResults.onWeakHit == Strength.Invalid)
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
				float reduceDamageRate = SkillStatus.GetReduceDamageRate(currentSufferState2);
				float attributeDamegeResult = SkillStatus.GetAttributeDamegeResult(skillResults.onWeakHit);
				float num2 = 1f;
				if (skillResults.onCriticalHit)
				{
					num2 = 1.2f;
				}
				int num3 = (int)(affectEffectProperty.damagePercent * (float)targetCharacter.hp);
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = (float)num3 * attributeDamegeResult * num2 * reduceDamageRate;
				}
			}
			else if (affectEffectProperty.powerType == PowerType.Percentage)
			{
				bool flag = affectEffectProperty.techniqueType == TechniqueType.Physics;
				float attributeDamegeResult2 = SkillStatus.GetAttributeDamegeResult(skillResults.onWeakHit);
				float num4 = 1f;
				if (skillResults.onCriticalHit)
				{
					num4 = 1.2f;
				}
				float num5 = 1f;
				if (onRandomAttack)
				{
					num5 = UnityEngine.Random.Range(0.85f, 1f);
				}
				float reduceDamageRate2 = SkillStatus.GetReduceDamageRate(currentSufferState2);
				float num6 = 1f;
				if (flag)
				{
					SufferStateProperty sufferStateProperty3 = targetCharacter.currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.Counter);
					if (sufferStateProperty3.isActive)
					{
						num6 *= sufferStateProperty3.recieveDamageRate;
					}
				}
				else
				{
					SufferStateProperty sufferStateProperty4 = targetCharacter.currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.Reflection);
					if (sufferStateProperty4.isActive)
					{
						num6 *= sufferStateProperty4.recieveDamageRate;
					}
				}
				float num7 = 1f;
				SufferStateProperty sufferStateProperty5 = currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.PowerCharge);
				if (sufferStateProperty5.isActive)
				{
					if (flag)
					{
						num7 *= sufferStateProperty5.physicUpPercent;
					}
					else
					{
						num7 *= sufferStateProperty5.specialUpPercent;
					}
				}
				float num8 = 1f;
				float num9 = 0f;
				float num10 = 0f;
				SufferStateProperty sufferStateProperty6 = currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.DamageRateUp);
				if (sufferStateProperty6.isActive)
				{
					num9 += sufferStateProperty6.GetTribeDamageRate(targetCharacter);
				}
				SufferStateProperty sufferStateProperty7 = currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.DamageRateDown);
				if (sufferStateProperty7.isActive)
				{
					num10 += sufferStateProperty7.GetTribeDamageRate(targetCharacter);
				}
				num8 = Mathf.Max(0f, num8 + num9 - num10);
				for (int j = 0; j < array.Length; j++)
				{
					int num11 = 0;
					float num12 = 1f;
					foreach (int num13 in attackerCharacter.potencyChipIdList.Keys)
					{
						GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffectDataToId = ChipDataMng.GetChipEffectDataToId(num13.ToString());
						if (j != 0 || chipEffectDataToId.effectTrigger.ToInt32() != 11)
						{
							GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects = new GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[]
							{
								chipEffectDataToId
							};
							num11 += ChipEffectStatus.GetSkillPowerCorrectionValue(chipEffects, affectEffectProperty, attackerCharacter, attackNum);
							num12 += ChipEffectStatus.GetSkillDamageCorrectionValue(chipEffects, affectEffectProperty, attackerCharacter);
						}
					}
					float num14 = 1f;
					foreach (int num15 in targetCharacter.potencyChipIdList.Keys)
					{
						GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffectDataToId2 = ChipDataMng.GetChipEffectDataToId(num15.ToString());
						if (j != 0 || chipEffectDataToId2.effectTrigger.ToInt32() != 11)
						{
							GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects2 = new GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[]
							{
								chipEffectDataToId2
							};
							num14 += ChipEffectStatus.GetSkillDamageCorrectionValue(chipEffects2, affectEffectProperty, targetCharacter);
						}
					}
					float num24;
					float num25;
					if (flag)
					{
						int num16 = (j != 0) ? attackerCharacter.extraAttackPower : attackerCharacter.attackPower;
						int num17 = (j != 0) ? targetCharacter.extraDefencePower : targetCharacter.defencePower;
						float num18 = leaderSkillResult.attackUpPercent * (float)num16;
						float num19 = targetCharacter.leaderSkillResult.defenceUpPercent * (float)num17;
						float num20 = 0f;
						float num21 = 0f;
						float num22 = 0f;
						float num23 = 0f;
						SufferStateProperty sufferStateProperty8 = currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.AttackUp);
						if (sufferStateProperty8.isActive)
						{
							num20 = sufferStateProperty8.upPercent * (float)num16;
						}
						SufferStateProperty sufferStateProperty9 = currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.AttackDown);
						if (sufferStateProperty9.isActive)
						{
							num21 = sufferStateProperty9.downPercent * (float)num16;
						}
						SufferStateProperty sufferStateProperty10 = currentSufferState2.GetSufferStateProperty(SufferStateProperty.SufferType.DefenceUp);
						if (sufferStateProperty10.isActive)
						{
							num22 = sufferStateProperty10.upPercent * (float)num17;
						}
						SufferStateProperty sufferStateProperty11 = currentSufferState2.GetSufferStateProperty(SufferStateProperty.SufferType.DefenceDown);
						if (sufferStateProperty11.isActive)
						{
							num23 = sufferStateProperty11.downPercent * (float)num17;
						}
						num24 = (float)num16 + num20 - num21 + num18;
						num25 = (float)num17 + num22 - num23 + num19;
						num24 = Mathf.Min(num24, (float)num16 * 2.2f);
						num24 = Mathf.Max(num24, (float)num16 * 0.6f);
						num25 = Mathf.Min(num25, (float)num17 * 2.2f);
						num25 = Mathf.Max(num25, (float)num17 * 0.6f);
					}
					else
					{
						int num26 = (j != 0) ? attackerCharacter.extraSpecialAttackPower : attackerCharacter.specialAttackPower;
						int num27 = (j != 0) ? targetCharacter.extraSpecialDefencePower : targetCharacter.specialDefencePower;
						float num28 = leaderSkillResult.specialAttackUpPercent * (float)num26;
						float num29 = targetCharacter.leaderSkillResult.specialDefenceUpPercent * (float)num27;
						float num30 = 0f;
						float num31 = 0f;
						float num32 = 0f;
						float num33 = 0f;
						SufferStateProperty sufferStateProperty12 = currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.SpAttackUp);
						if (sufferStateProperty12.isActive)
						{
							num30 = sufferStateProperty12.upPercent * (float)num26;
						}
						SufferStateProperty sufferStateProperty13 = currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.SpAttackDown);
						if (sufferStateProperty13.isActive)
						{
							num31 = sufferStateProperty13.downPercent * (float)num26;
						}
						SufferStateProperty sufferStateProperty14 = currentSufferState2.GetSufferStateProperty(SufferStateProperty.SufferType.SpDefenceUp);
						if (sufferStateProperty14.isActive)
						{
							num32 = sufferStateProperty14.upPercent * (float)num27;
						}
						SufferStateProperty sufferStateProperty15 = currentSufferState2.GetSufferStateProperty(SufferStateProperty.SufferType.SpDefenceDown);
						if (sufferStateProperty15.isActive)
						{
							num33 = sufferStateProperty15.downPercent * (float)num27;
						}
						num24 = (float)num26 + num30 - num31 + num28;
						num25 = (float)num27 + num32 - num33 + num29;
						num24 = Mathf.Min(num24, (float)num26 * 2.2f);
						num24 = Mathf.Max(num24, (float)num26 * 0.6f);
						num25 = Mathf.Min(num25, (float)num27 * 2.2f);
						num25 = Mathf.Max(num25, (float)num27 * 0.6f);
					}
					int num34;
					if (j == 0)
					{
						num34 = affectEffectProperty.GetPower(attackerCharacter);
					}
					else
					{
						List<ExtraEffectStatus> extraEffectStatus = BattleStateManager.current.battleStateData.extraEffectStatus;
						List<ExtraEffectStatus> invocationList = ExtraEffectStatus.GetInvocationList(extraEffectStatus, EffectStatusBase.EffectTriggerType.Usually);
						num34 = ExtraEffectStatus.GetSkillPowerCorrectionValue(invocationList, affectEffectProperty, attackerCharacter);
					}
					if (affectEffectProperty.type == AffectEffect.DefenseThroughDamage || affectEffectProperty.type == AffectEffect.SpDefenseThroughDamage)
					{
						num25 = (float)affectEffectProperty.DefenseThrough;
					}
					if (num24 <= 0f)
					{
						array[j] = 0f;
					}
					else
					{
						num24 = Mathf.Max(num24, 0f);
						num25 = Mathf.Max(num25, 1f);
						float num35 = (float)attackerCharacter.level * 0.01f + 1f;
						float num36 = (float)(num34 + num11) * (1f + leaderSkillResult.damageUpPercent);
						float num37 = num35 * num36 * num24 * num7 / num25 + 2f;
						float num38 = num12 * num14 * reduceDamageRate2 * num6 * num5 * num4 * attributeDamegeResult2 * num8;
						array[j] = num37 * num38;
					}
				}
			}
			else if (affectEffectProperty.powerType == PowerType.Fixable)
			{
				float reduceDamageRate3 = SkillStatus.GetReduceDamageRate(currentSufferState2);
				for (int k = 0; k < array.Length; k++)
				{
					array[k] = (float)affectEffectProperty.damagePower * reduceDamageRate3;
				}
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
		if (skillResults.onWeakHit == Strength.Invalid)
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
			float reduceDamageRate = SkillStatus.GetReduceDamageRate(targetCharacter.currentSufferState);
			float attributeDamegeResult = SkillStatus.GetAttributeDamegeResult(skillResults.onWeakHit);
			int num;
			if (affectEffectProperty.powerType == PowerType.Fixable)
			{
				num = (int)((float)affectEffectProperty.damagePower * reduceDamageRate * attributeDamegeResult);
			}
			else
			{
				num = (int)(affectEffectProperty.damagePercent * (float)targetCharacter.hp * reduceDamageRate * attributeDamegeResult);
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

	private static float GetReduceDamageRate(HaveSufferState targetSufferState)
	{
		float result = 1f;
		if (targetSufferState.FindSufferState(SufferStateProperty.SufferType.TurnBarrier))
		{
			result = 0f;
		}
		else if (targetSufferState.FindSufferState(SufferStateProperty.SufferType.CountBarrier))
		{
			result = 0f;
		}
		if (targetSufferState.FindSufferState(SufferStateProperty.SufferType.TurnEvasion))
		{
			result = 0f;
		}
		else if (targetSufferState.FindSufferState(SufferStateProperty.SufferType.CountEvasion))
		{
			result = 0f;
		}
		else if (targetSufferState.FindSufferState(SufferStateProperty.SufferType.CountGuard))
		{
			result = 1f - targetSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.CountGuard).damagePercent;
		}
		return result;
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
				if (affectEffectProperty.ThisSkillIsAttack)
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
				if (affectEffectProperty.ThisSkillIsAttack)
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
}
