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

	public SkillStatus(string prefabId, string seId, SkillType skillType, string name, string description, EffectTarget target, EffectNumbers numbers, int needAp, params AffectEffectProperty[] affectEffect)
	{
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

	public bool ThisSkillIsHpRevival
	{
		get
		{
			foreach (AffectEffectProperty affectEffectProperty in this.affectEffect)
			{
				if (affectEffectProperty.ThisSkillIsHpRevival)
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

	public int power
	{
		get
		{
			AffectEffectProperty affectEffectAttackFirst = this.GetAffectEffectAttackFirst();
			return (affectEffectAttackFirst == null) ? 0 : affectEffectAttackFirst.power;
		}
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

	public float satisfactionRate
	{
		get
		{
			return (this.affectEffect.Count <= 0) ? 0f : this.affectEffect[0].satisfactionRate;
		}
	}

	public int hitNumber
	{
		get
		{
			AffectEffectProperty affectEffectAttackFirst = this.GetAffectEffectAttackFirst();
			return (affectEffectAttackFirst == null) ? 1 : affectEffectAttackFirst.hitNumber;
		}
	}

	public int keepRoundNumber
	{
		get
		{
			AffectEffectProperty affectEffectSufferInfluenceFirst = this.GetAffectEffectSufferInfluenceFirst();
			return (affectEffectSufferInfluenceFirst == null) ? 1 : affectEffectSufferInfluenceFirst.keepRoundNumber;
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

	private AffectEffectProperty GetAffectEffectSufferInfluenceFirst()
	{
		foreach (AffectEffectProperty affectEffectProperty in this.affectEffect)
		{
			if (affectEffectProperty != null && SufferStateProperty.OnInfluenceSufferAffectEffect(affectEffectProperty.type))
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
		float[] array = new float[2];
		skillResults.targetCharacter = targetCharacter;
		skillResults.onIgnoreTarget = false;
		if (targetCharacter.isDied)
		{
			return skillResults;
		}
		if (!attackerCharacter.IsHittingTheTargetChip && !this.affectEffect[index].OnHit(attackerCharacter, targetCharacter))
		{
			skillResults.onMissHit = true;
		}
		if (!skillResults.onMissHit)
		{
			if (this.affectEffect[index].type == AffectEffect.ReferenceTargetHpRate)
			{
				HaveSufferState currentSufferState = targetCharacter.currentSufferState;
				float num = 1f;
				if (currentSufferState.FindSufferState(SufferStateProperty.SufferType.TurnBarrier))
				{
					num = 0f;
				}
				else if (currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountBarrier))
				{
					num = 0f;
				}
				if (currentSufferState.FindSufferState(SufferStateProperty.SufferType.TurnEvasion))
				{
					num = 0f;
				}
				else if (currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountEvasion))
				{
					num = 0f;
				}
				else if (currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountGuard) && currentSufferState.onCountGuard.currentKeepRound > 0)
				{
					num = 1f - currentSufferState.onCountGuard.damagePercent;
					currentSufferState.onCountGuard.currentKeepRound--;
					if (currentSufferState.onCountGuard.currentKeepRound <= 0)
					{
						currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.CountGuard);
					}
				}
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.affectEffect[index].damagePercent * (float)targetCharacter.hp * num;
				}
			}
			else if (this.affectEffect[index].powerType == PowerType.Percentage)
			{
				LeaderSkillResult leaderSkillResult = attackerCharacter.leaderSkillResult;
				bool flag = this.affectEffect[index].techniqueType == TechniqueType.Physics;
				HaveSufferState currentSufferState2 = attackerCharacter.currentSufferState;
				HaveSufferState currentSufferState3 = targetCharacter.currentSufferState;
				float num2 = 1f;
				Tolerance shiftedTolerance = targetCharacter.leaderSkillResult.toleranceUp.GetShiftedTolerance(targetCharacter.tolerance);
				int attributeGoodWeak = shiftedTolerance.GetAttributeGoodWeak(this.affectEffect[index].attribute);
				switch (attributeGoodWeak + 1)
				{
				case 0:
					num2 = 0.5f;
					skillResults.onWeakHit = Strength.Strong;
					break;
				default:
					if (attributeGoodWeak == -99)
					{
						num2 = 0f;
						skillResults.onWeakHit = Strength.Invalid;
					}
					break;
				case 2:
					num2 = 1.5f;
					skillResults.onWeakHit = Strength.Weak;
					break;
				}
				float num3 = 1f;
				if (skillResults.onWeakHit == Strength.None || skillResults.onWeakHit == Strength.Weak)
				{
					float num4 = this.affectEffect[index].satisfactionRate;
					if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.SatisfactionRateUp))
					{
						num4 += currentSufferState2.onSatisfactionRateUp.upPercent;
					}
					if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.SatisfactionRateDown))
					{
						num4 -= currentSufferState2.onSatisfactionRateDown.downPercent;
					}
					if (attackerCharacter.isSelectSkill > 0)
					{
						num4 += leaderSkillResult.satisfactionRateUpPercent;
					}
					num4 += attackerCharacter.chipAddCritical;
					float percentage = Mathf.Clamp(num4, 0f, 1f);
					skillResults.onCriticalHit = RandomExtension.Switch(percentage, null);
					if (skillResults.onCriticalHit)
					{
						num3 = 1.2f;
					}
				}
				float num5 = 1f;
				if (onRandomAttack)
				{
					num5 = UnityEngine.Random.Range(0.85f, 1f);
				}
				float num6 = 1f;
				if (currentSufferState3.FindSufferState(SufferStateProperty.SufferType.TurnBarrier))
				{
					num6 = 0f;
				}
				else if (currentSufferState3.FindSufferState(SufferStateProperty.SufferType.CountBarrier))
				{
					num6 = 0f;
				}
				if (currentSufferState3.FindSufferState(SufferStateProperty.SufferType.TurnEvasion))
				{
					num6 = 0f;
				}
				else if (currentSufferState3.FindSufferState(SufferStateProperty.SufferType.CountEvasion))
				{
					num6 = 0f;
				}
				else if (currentSufferState3.FindSufferState(SufferStateProperty.SufferType.CountGuard) && currentSufferState3.onCountGuard.currentKeepRound > 0)
				{
					num6 = 1f - currentSufferState3.onCountGuard.damagePercent;
					currentSufferState3.onCountGuard.currentKeepRound--;
					if (currentSufferState3.onCountGuard.currentKeepRound <= 0)
					{
						currentSufferState3.RemoveSufferState(SufferStateProperty.SufferType.CountGuard);
					}
				}
				float num7 = 1f;
				if (flag)
				{
					if (targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Counter))
					{
						num7 *= currentSufferState3.onCounter.recieveDamageRate;
					}
				}
				else if (targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Reflection))
				{
					num7 *= currentSufferState3.onReflection.recieveDamageRate;
				}
				float num8 = 1f;
				if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.PowerCharge))
				{
					if (flag)
					{
						num8 *= currentSufferState2.onPowerCharge.physicUpPercent;
					}
					else
					{
						num8 *= currentSufferState2.onPowerCharge.specialUpPercent;
					}
				}
				float num9 = 1f;
				float num10 = 0f;
				float num11 = 0f;
				if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.DamageRateUp))
				{
					if (targetCharacter.species == Species.PhantomStudents)
					{
						num10 += currentSufferState2.onDamageRateUp.damageRateForPhantomStudents;
					}
					else if (targetCharacter.species == Species.HeatHaze)
					{
						num10 += currentSufferState2.onDamageRateUp.damageRateForHeatHaze;
					}
					else if (targetCharacter.species == Species.Glacier)
					{
						num10 += currentSufferState2.onDamageRateUp.damageRateForGlacier;
					}
					else if (targetCharacter.species == Species.Electromagnetic)
					{
						num10 += currentSufferState2.onDamageRateUp.damageRateForElectromagnetic;
					}
					else if (targetCharacter.species == Species.Earth)
					{
						num10 += currentSufferState2.onDamageRateUp.damageRateForEarth;
					}
					else if (targetCharacter.species == Species.ShaftOfLight)
					{
						num10 += currentSufferState2.onDamageRateUp.damageRateForShaftOfLight;
					}
					else if (targetCharacter.species == Species.Abyss)
					{
						num10 += currentSufferState2.onDamageRateUp.damageRateForAbyss;
					}
				}
				if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.DamageRateDown))
				{
					if (targetCharacter.species == Species.PhantomStudents)
					{
						num11 += currentSufferState2.onDamageRateDown.damageRateForPhantomStudents;
					}
					else if (targetCharacter.species == Species.HeatHaze)
					{
						num11 += currentSufferState2.onDamageRateDown.damageRateForHeatHaze;
					}
					else if (targetCharacter.species == Species.Glacier)
					{
						num11 += currentSufferState2.onDamageRateDown.damageRateForGlacier;
					}
					else if (targetCharacter.species == Species.Electromagnetic)
					{
						num11 += currentSufferState2.onDamageRateDown.damageRateForElectromagnetic;
					}
					else if (targetCharacter.species == Species.Earth)
					{
						num11 += currentSufferState2.onDamageRateDown.damageRateForEarth;
					}
					else if (targetCharacter.species == Species.ShaftOfLight)
					{
						num11 += currentSufferState2.onDamageRateDown.damageRateForShaftOfLight;
					}
					else if (targetCharacter.species == Species.Abyss)
					{
						num11 += currentSufferState2.onDamageRateDown.damageRateForAbyss;
					}
				}
				num9 = Mathf.Max(0f, num9 + num10 - num11);
				for (int j = 0; j < array.Length; j++)
				{
					float num12 = 0f;
					float num13 = 0f;
					float num14 = 0f;
					float num15 = 0f;
					int num16 = 0;
					float num17 = 1f;
					foreach (int num18 in attackerCharacter.potencyChipIdList.Keys)
					{
						GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffectDataToId = ChipDataMng.GetChipEffectDataToId(num18.ToString());
						if (j != 0 || chipEffectDataToId.effectTrigger.ToInt32() != 11)
						{
							GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects = new GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[]
							{
								chipEffectDataToId
							};
							num16 += ChipEffectStatus.GetSkillPowerCorrectionValue(chipEffects, this.affectEffect[index], attackerCharacter);
							num17 += ChipEffectStatus.GetDamageRateCorrectionValue(chipEffects, num17, attackerCharacter, targetCharacter);
						}
					}
					int num21;
					float num22;
					int num23;
					float num24;
					if (flag)
					{
						int num19 = (j != 0) ? attackerCharacter.extraAttackPower : attackerCharacter.attackPower;
						int num20 = (j != 0) ? targetCharacter.extraDefencePower : targetCharacter.defencePower;
						num21 = num19;
						num22 = leaderSkillResult.attackUpPercent * (float)num21;
						num23 = num20;
						num24 = targetCharacter.leaderSkillResult.defenceUpPercent * (float)num23;
						if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.AttackUp))
						{
							num12 = currentSufferState2.onAttackUp.upPercent * (float)num21;
						}
						if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.AttackDown))
						{
							num13 = currentSufferState2.onAttackDown.downPercent * (float)num21;
						}
						if (currentSufferState3.FindSufferState(SufferStateProperty.SufferType.DefenceUp))
						{
							num14 = currentSufferState3.onDefenceUp.upPercent * (float)num23;
						}
						if (currentSufferState3.FindSufferState(SufferStateProperty.SufferType.DefenceDown))
						{
							num15 = currentSufferState3.onDefenceDown.downPercent * (float)num23;
						}
					}
					else
					{
						int num25 = (j != 0) ? attackerCharacter.extraSpecialAttackPower : attackerCharacter.specialAttackPower;
						int num26 = (j != 0) ? targetCharacter.extraSpecialDefencePower : targetCharacter.specialDefencePower;
						num21 = num25;
						num22 = leaderSkillResult.specialAttackUpPercent * (float)num21;
						num23 = num26;
						num24 = targetCharacter.leaderSkillResult.specialDefenceUpPercent * (float)num23;
						if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.SpAttackUp))
						{
							num12 = currentSufferState2.onSpAttackUp.upPercent * (float)num21;
						}
						if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.SpAttackDown))
						{
							num13 = currentSufferState2.onSpAttackDown.downPercent * (float)num21;
						}
						if (currentSufferState3.FindSufferState(SufferStateProperty.SufferType.SpDefenceUp))
						{
							num14 = currentSufferState3.onSpDefenceUp.upPercent * (float)num23;
						}
						if (currentSufferState3.FindSufferState(SufferStateProperty.SufferType.SpDefenceDown))
						{
							num15 = currentSufferState3.onSpDefenceDown.downPercent * (float)num23;
						}
					}
					int num27;
					if (j == 0)
					{
						num27 = this.affectEffect[index].power;
					}
					else
					{
						num27 = ExtraEffectStatus.GetSkillPowerCorrectionValue(BattleStateManager.current.battleStateData.extraEffectStatus, this.affectEffect[index], attackerCharacter);
					}
					float num28 = (float)num21 + num12 - num13 + num22;
					float num29 = (float)num23 + num14 - num15 + num24;
					if (num28 <= 0f)
					{
						array[j] = 0f;
					}
					else
					{
						num28 = Mathf.Max(num28, 0f);
						num29 = Mathf.Max(num29, 1f);
						float num30 = (float)attackerCharacter.level * 0.01f + 1f;
						float num31 = (float)(num27 + num16) * (1f + leaderSkillResult.damageUpPercent);
						float num32 = num30 * num31 * num28 * num8 / num29 + 2f;
						float num33 = num17 * num6 * num7 * num5 * num3 * num2 * num9;
						array[j] = num32 * num33;
					}
				}
			}
			else if (this.affectEffect[index].powerType == PowerType.Fixable)
			{
				HaveSufferState currentSufferState4 = targetCharacter.currentSufferState;
				float num34 = 1f;
				if (currentSufferState4.FindSufferState(SufferStateProperty.SufferType.TurnBarrier))
				{
					num34 = 0f;
				}
				else if (currentSufferState4.FindSufferState(SufferStateProperty.SufferType.CountBarrier))
				{
					num34 = 0f;
				}
				if (currentSufferState4.FindSufferState(SufferStateProperty.SufferType.TurnEvasion))
				{
					num34 = 0f;
				}
				else if (currentSufferState4.FindSufferState(SufferStateProperty.SufferType.CountEvasion))
				{
					num34 = 0f;
				}
				if (RandomExtension.Switch(this.affectEffect[index].satisfactionRate, null))
				{
					skillResults.onCriticalHit = true;
				}
				for (int k = 0; k < array.Length; k++)
				{
					array[k] = (float)this.affectEffect[index].damagePower * num34;
				}
			}
		}
		skillResults.attackPowerNormal = Mathf.FloorToInt(array[1]);
		skillResults.originalAttackPower = Mathf.FloorToInt(array[0]);
		skillResults.attackPower = Mathf.FloorToInt(array[1]);
		if (skillResults.attackPower <= 0)
		{
			skillResults.onMissHit = true;
			skillResults.onCriticalHit = false;
		}
		targetCharacter.hp -= skillResults.attackPower;
		return skillResults;
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
				if (affectEffectProperty.type == AffectEffect.Damage)
				{
					num += tolerance.GetAttributeStrength(affectEffectProperty.attribute).ToInverseInt();
				}
				else if (Tolerance.OnInfluenceToleranceAffectEffect(affectEffectProperty.type))
				{
					num += tolerance.GetAffectEffectStrength(affectEffectProperty.type).ToInverseInt();
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
