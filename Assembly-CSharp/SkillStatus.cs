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

	public SkillResults OnAttack(int index, CharacterStateControl attackerCharacter, CharacterStateControl targetCharacter, bool onRandomAttack = true)
	{
		SkillResults skillResults = new SkillResults();
		LeaderSkillResult leaderSkillResult = attackerCharacter.leaderSkillResult;
		HaveSufferState currentSufferState = attackerCharacter.currentSufferState;
		HaveSufferState currentSufferState2 = targetCharacter.currentSufferState;
		float[] array = new float[2];
		skillResults.targetCharacter = targetCharacter;
		if (targetCharacter.isDied)
		{
			return skillResults;
		}
		skillResults.onWeakHit = targetCharacter.tolerance.GetAttributeStrength(this.affectEffect[index].attribute);
		if (attackerCharacter.hittingTheTargetType == BattleInvariant.Type.Non)
		{
			skillResults.onMissHit = !this.affectEffect[index].OnHit(attackerCharacter, targetCharacter);
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
				float num = this.affectEffect[index].satisfactionRate;
				if (currentSufferState.FindSufferState(SufferStateProperty.SufferType.SatisfactionRateUp))
				{
					num += currentSufferState.onSatisfactionRateUp.upPercent;
				}
				if (currentSufferState.FindSufferState(SufferStateProperty.SufferType.SatisfactionRateDown))
				{
					num -= currentSufferState.onSatisfactionRateDown.downPercent;
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
			if (this.affectEffect[index].type == AffectEffect.ReferenceTargetHpRate)
			{
				float reduceDamageRate = SkillStatus.GetReduceDamageRate(currentSufferState2);
				float attributeDamegeResult = this.GetAttributeDamegeResult(skillResults.onWeakHit);
				float num2 = 1f;
				if (skillResults.onCriticalHit)
				{
					num2 = 1.2f;
				}
				int num3 = (int)(this.affectEffect[index].damagePercent * (float)targetCharacter.hp);
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = (float)num3 * attributeDamegeResult * num2 * reduceDamageRate;
				}
			}
			else if (this.affectEffect[index].powerType == PowerType.Percentage)
			{
				bool flag = this.affectEffect[index].techniqueType == TechniqueType.Physics;
				float attributeDamegeResult2 = this.GetAttributeDamegeResult(skillResults.onWeakHit);
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
					if (targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Counter))
					{
						num6 *= currentSufferState2.onCounter.recieveDamageRate;
					}
				}
				else if (targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Reflection))
				{
					num6 *= currentSufferState2.onReflection.recieveDamageRate;
				}
				float num7 = 1f;
				if (currentSufferState.FindSufferState(SufferStateProperty.SufferType.PowerCharge))
				{
					if (flag)
					{
						num7 *= currentSufferState.onPowerCharge.physicUpPercent;
					}
					else
					{
						num7 *= currentSufferState.onPowerCharge.specialUpPercent;
					}
				}
				float num8 = 1f;
				float num9 = 0f;
				float num10 = 0f;
				if (currentSufferState.FindSufferState(SufferStateProperty.SufferType.DamageRateUp))
				{
					int num11 = targetCharacter.characterDatas.tribe.ToInt32();
					float[] array2 = new float[]
					{
						0f,
						currentSufferState.onDamageRateUp.damageRateForPhantomStudents,
						currentSufferState.onDamageRateUp.damageRateForHeatHaze,
						currentSufferState.onDamageRateUp.damageRateForGlacier,
						currentSufferState.onDamageRateUp.damageRateForElectromagnetic,
						currentSufferState.onDamageRateUp.damageRateForEarth,
						currentSufferState.onDamageRateUp.damageRateForShaftOfLight,
						currentSufferState.onDamageRateUp.damageRateForAbyss
					};
					num9 += array2[num11];
				}
				if (currentSufferState.FindSufferState(SufferStateProperty.SufferType.DamageRateDown))
				{
					int num12 = targetCharacter.characterDatas.tribe.ToInt32();
					float[] array3 = new float[]
					{
						0f,
						currentSufferState.onDamageRateDown.damageRateForPhantomStudents,
						currentSufferState.onDamageRateDown.damageRateForHeatHaze,
						currentSufferState.onDamageRateDown.damageRateForGlacier,
						currentSufferState.onDamageRateDown.damageRateForElectromagnetic,
						currentSufferState.onDamageRateDown.damageRateForEarth,
						currentSufferState.onDamageRateDown.damageRateForShaftOfLight,
						currentSufferState.onDamageRateDown.damageRateForAbyss
					};
					num10 += array3[num12];
				}
				num8 = Mathf.Max(0f, num8 + num9 - num10);
				for (int j = 0; j < array.Length; j++)
				{
					float num13 = 0f;
					float num14 = 0f;
					float num15 = 0f;
					float num16 = 0f;
					int num17 = 0;
					float num18 = 1f;
					foreach (int num19 in attackerCharacter.potencyChipIdList.Keys)
					{
						GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffectDataToId = ChipDataMng.GetChipEffectDataToId(num19.ToString());
						if (j != 0 || chipEffectDataToId.effectTrigger.ToInt32() != 11)
						{
							GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects = new GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[]
							{
								chipEffectDataToId
							};
							num17 += ChipEffectStatus.GetSkillPowerCorrectionValue(chipEffects, this.affectEffect[index], attackerCharacter);
							num18 += ChipEffectStatus.GetChipEffectValueToFloat(chipEffects, num18, attackerCharacter, EffectStatusBase.ExtraEffectType.Damage);
						}
					}
					float num20 = 1f;
					foreach (int num21 in targetCharacter.potencyChipIdList.Keys)
					{
						GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffectDataToId2 = ChipDataMng.GetChipEffectDataToId(num21.ToString());
						if (j != 0 || chipEffectDataToId2.effectTrigger.ToInt32() != 11)
						{
							GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects2 = new GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[]
							{
								chipEffectDataToId2
							};
							num20 += ChipEffectStatus.GetChipEffectValueToFloat(chipEffects2, num20, targetCharacter, EffectStatusBase.ExtraEffectType.Damage);
						}
					}
					int num24;
					float num25;
					int num26;
					float num27;
					if (flag)
					{
						int num22 = (j != 0) ? attackerCharacter.extraAttackPower : attackerCharacter.attackPower;
						int num23 = (j != 0) ? targetCharacter.extraDefencePower : targetCharacter.defencePower;
						num24 = num22;
						num25 = leaderSkillResult.attackUpPercent * (float)num24;
						num26 = num23;
						num27 = targetCharacter.leaderSkillResult.defenceUpPercent * (float)num26;
						if (currentSufferState.FindSufferState(SufferStateProperty.SufferType.AttackUp))
						{
							num13 = currentSufferState.onAttackUp.upPercent * (float)num24;
						}
						if (currentSufferState.FindSufferState(SufferStateProperty.SufferType.AttackDown))
						{
							num14 = currentSufferState.onAttackDown.downPercent * (float)num24;
						}
						if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.DefenceUp))
						{
							num15 = currentSufferState2.onDefenceUp.upPercent * (float)num26;
						}
						if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.DefenceDown))
						{
							num16 = currentSufferState2.onDefenceDown.downPercent * (float)num26;
						}
					}
					else
					{
						int num28 = (j != 0) ? attackerCharacter.extraSpecialAttackPower : attackerCharacter.specialAttackPower;
						int num29 = (j != 0) ? targetCharacter.extraSpecialDefencePower : targetCharacter.specialDefencePower;
						num24 = num28;
						num25 = leaderSkillResult.specialAttackUpPercent * (float)num24;
						num26 = num29;
						num27 = targetCharacter.leaderSkillResult.specialDefenceUpPercent * (float)num26;
						if (currentSufferState.FindSufferState(SufferStateProperty.SufferType.SpAttackUp))
						{
							num13 = currentSufferState.onSpAttackUp.upPercent * (float)num24;
						}
						if (currentSufferState.FindSufferState(SufferStateProperty.SufferType.SpAttackDown))
						{
							num14 = currentSufferState.onSpAttackDown.downPercent * (float)num24;
						}
						if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.SpDefenceUp))
						{
							num15 = currentSufferState2.onSpDefenceUp.upPercent * (float)num26;
						}
						if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.SpDefenceDown))
						{
							num16 = currentSufferState2.onSpDefenceDown.downPercent * (float)num26;
						}
					}
					int num30;
					if (j == 0)
					{
						num30 = this.affectEffect[index].GetPower(attackerCharacter);
					}
					else
					{
						List<ExtraEffectStatus> extraEffectStatus = BattleStateManager.current.battleStateData.extraEffectStatus;
						List<ExtraEffectStatus> invocationList = ExtraEffectStatus.GetInvocationList(extraEffectStatus, EffectStatusBase.EffectTriggerType.Usually);
						num30 = ExtraEffectStatus.GetSkillPowerCorrectionValue(invocationList, this.affectEffect[index], attackerCharacter);
					}
					float num31 = (float)num24 + num13 - num14 + num25;
					float num32 = (float)num26 + num15 - num16 + num27;
					if (this.affectEffect[index].type == AffectEffect.DefenseThroughDamage || this.affectEffect[index].type == AffectEffect.SpDefenseThroughDamage)
					{
						num32 = (float)this.affectEffect[index].DefenseThrough;
					}
					if (num31 <= 0f)
					{
						array[j] = 0f;
					}
					else
					{
						num31 = Mathf.Max(num31, 0f);
						num32 = Mathf.Max(num32, 1f);
						float num33 = (float)attackerCharacter.level * 0.01f + 1f;
						float num34 = (float)(num30 + num17) * (1f + leaderSkillResult.damageUpPercent);
						float num35 = num33 * num34 * num31 * num7 / num32 + 2f;
						float num36 = num18 * num20 * reduceDamageRate2 * num6 * num5 * num4 * attributeDamegeResult2 * num8;
						array[j] = num35 * num36;
					}
				}
			}
			else if (this.affectEffect[index].powerType == PowerType.Fixable)
			{
				float reduceDamageRate3 = SkillStatus.GetReduceDamageRate(currentSufferState2);
				for (int k = 0; k < array.Length; k++)
				{
					array[k] = (float)this.affectEffect[index].damagePower * reduceDamageRate3;
				}
			}
		}
		skillResults.originalAttackPower = Mathf.FloorToInt(array[0]);
		skillResults.attackPower = Mathf.FloorToInt(array[1]);
		if (skillResults.attackPower <= 0)
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

	public static float GetReduceDamageRate(HaveSufferState targetSufferState)
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
			result = 1f - targetSufferState.onCountGuard.damagePercent;
		}
		return result;
	}

	private float GetAttributeDamegeResult(Strength strength)
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
		if (user.currentSufferState.FindSufferState(SufferStateProperty.SufferType.ApConsumptionUp))
		{
			num += user.currentSufferState.onApConsumptionUp.upPower;
		}
		if (user.currentSufferState.FindSufferState(SufferStateProperty.SufferType.ApConsumptionDown))
		{
			num -= user.currentSufferState.onApConsumptionUp.downPower;
		}
		return num;
	}
}
