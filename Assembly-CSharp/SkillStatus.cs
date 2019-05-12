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
				float reduceDamageRate = SkillStatus.GetReduceDamageRate(currentSufferState);
				skillResults.onWeakHit = targetCharacter.tolerance.GetAttributeStrength(this.affectEffect[index].attribute);
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.affectEffect[index].damagePercent * (float)targetCharacter.hp * reduceDamageRate;
				}
			}
			else if (this.affectEffect[index].powerType == PowerType.Percentage)
			{
				LeaderSkillResult leaderSkillResult = attackerCharacter.leaderSkillResult;
				bool flag = this.affectEffect[index].techniqueType == TechniqueType.Physics;
				HaveSufferState currentSufferState2 = attackerCharacter.currentSufferState;
				HaveSufferState currentSufferState3 = targetCharacter.currentSufferState;
				skillResults.onWeakHit = targetCharacter.tolerance.GetAttributeStrength(this.affectEffect[index].attribute);
				float attributeDamegeResult = this.GetAttributeDamegeResult(skillResults.onWeakHit);
				float num = 1f;
				if (skillResults.onWeakHit == Strength.None || skillResults.onWeakHit == Strength.Weak)
				{
					float num2 = this.affectEffect[index].satisfactionRate;
					if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.SatisfactionRateUp))
					{
						num2 += currentSufferState2.onSatisfactionRateUp.upPercent;
					}
					if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.SatisfactionRateDown))
					{
						num2 -= currentSufferState2.onSatisfactionRateDown.downPercent;
					}
					if (attackerCharacter.isSelectSkill > 0)
					{
						num2 += leaderSkillResult.satisfactionRateUpPercent;
					}
					num2 += attackerCharacter.chipAddCritical;
					float percentage = Mathf.Clamp(num2, 0f, 1f);
					skillResults.onCriticalHit = RandomExtension.Switch(percentage, null);
					if (skillResults.onCriticalHit)
					{
						num = 1.2f;
					}
				}
				float num3 = 1f;
				if (onRandomAttack)
				{
					num3 = UnityEngine.Random.Range(0.85f, 1f);
				}
				float reduceDamageRate2 = SkillStatus.GetReduceDamageRate(currentSufferState3);
				float num4 = 1f;
				if (flag)
				{
					if (targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Counter))
					{
						num4 *= currentSufferState3.onCounter.recieveDamageRate;
					}
				}
				else if (targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Reflection))
				{
					num4 *= currentSufferState3.onReflection.recieveDamageRate;
				}
				float num5 = 1f;
				if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.PowerCharge))
				{
					if (flag)
					{
						num5 *= currentSufferState2.onPowerCharge.physicUpPercent;
					}
					else
					{
						num5 *= currentSufferState2.onPowerCharge.specialUpPercent;
					}
				}
				float num6 = 1f;
				float num7 = 0f;
				float num8 = 0f;
				if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.DamageRateUp))
				{
					if (targetCharacter.species == Species.PhantomStudents)
					{
						num7 += currentSufferState2.onDamageRateUp.damageRateForPhantomStudents;
					}
					else if (targetCharacter.species == Species.HeatHaze)
					{
						num7 += currentSufferState2.onDamageRateUp.damageRateForHeatHaze;
					}
					else if (targetCharacter.species == Species.Glacier)
					{
						num7 += currentSufferState2.onDamageRateUp.damageRateForGlacier;
					}
					else if (targetCharacter.species == Species.Electromagnetic)
					{
						num7 += currentSufferState2.onDamageRateUp.damageRateForElectromagnetic;
					}
					else if (targetCharacter.species == Species.Earth)
					{
						num7 += currentSufferState2.onDamageRateUp.damageRateForEarth;
					}
					else if (targetCharacter.species == Species.ShaftOfLight)
					{
						num7 += currentSufferState2.onDamageRateUp.damageRateForShaftOfLight;
					}
					else if (targetCharacter.species == Species.Abyss)
					{
						num7 += currentSufferState2.onDamageRateUp.damageRateForAbyss;
					}
				}
				if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.DamageRateDown))
				{
					if (targetCharacter.species == Species.PhantomStudents)
					{
						num8 += currentSufferState2.onDamageRateDown.damageRateForPhantomStudents;
					}
					else if (targetCharacter.species == Species.HeatHaze)
					{
						num8 += currentSufferState2.onDamageRateDown.damageRateForHeatHaze;
					}
					else if (targetCharacter.species == Species.Glacier)
					{
						num8 += currentSufferState2.onDamageRateDown.damageRateForGlacier;
					}
					else if (targetCharacter.species == Species.Electromagnetic)
					{
						num8 += currentSufferState2.onDamageRateDown.damageRateForElectromagnetic;
					}
					else if (targetCharacter.species == Species.Earth)
					{
						num8 += currentSufferState2.onDamageRateDown.damageRateForEarth;
					}
					else if (targetCharacter.species == Species.ShaftOfLight)
					{
						num8 += currentSufferState2.onDamageRateDown.damageRateForShaftOfLight;
					}
					else if (targetCharacter.species == Species.Abyss)
					{
						num8 += currentSufferState2.onDamageRateDown.damageRateForAbyss;
					}
				}
				num6 = Mathf.Max(0f, num6 + num7 - num8);
				for (int j = 0; j < array.Length; j++)
				{
					float num9 = 0f;
					float num10 = 0f;
					float num11 = 0f;
					float num12 = 0f;
					int num13 = 0;
					float num14 = 1f;
					foreach (int num15 in attackerCharacter.potencyChipIdList.Keys)
					{
						GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffectDataToId = ChipDataMng.GetChipEffectDataToId(num15.ToString());
						if (j != 0 || chipEffectDataToId.effectTrigger.ToInt32() != 11)
						{
							GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects = new GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[]
							{
								chipEffectDataToId
							};
							num13 += ChipEffectStatus.GetSkillPowerCorrectionValue(chipEffects, this.affectEffect[index], attackerCharacter);
							num14 += ChipEffectStatus.GetDamageRateCorrectionValue(chipEffects, num14, attackerCharacter, targetCharacter);
						}
					}
					int num18;
					float num19;
					int num20;
					float num21;
					if (flag)
					{
						int num16 = (j != 0) ? attackerCharacter.extraAttackPower : attackerCharacter.attackPower;
						int num17 = (j != 0) ? targetCharacter.extraDefencePower : targetCharacter.defencePower;
						num18 = num16;
						num19 = leaderSkillResult.attackUpPercent * (float)num18;
						num20 = num17;
						num21 = targetCharacter.leaderSkillResult.defenceUpPercent * (float)num20;
						if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.AttackUp))
						{
							num9 = currentSufferState2.onAttackUp.upPercent * (float)num18;
						}
						if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.AttackDown))
						{
							num10 = currentSufferState2.onAttackDown.downPercent * (float)num18;
						}
						if (currentSufferState3.FindSufferState(SufferStateProperty.SufferType.DefenceUp))
						{
							num11 = currentSufferState3.onDefenceUp.upPercent * (float)num20;
						}
						if (currentSufferState3.FindSufferState(SufferStateProperty.SufferType.DefenceDown))
						{
							num12 = currentSufferState3.onDefenceDown.downPercent * (float)num20;
						}
					}
					else
					{
						int num22 = (j != 0) ? attackerCharacter.extraSpecialAttackPower : attackerCharacter.specialAttackPower;
						int num23 = (j != 0) ? targetCharacter.extraSpecialDefencePower : targetCharacter.specialDefencePower;
						num18 = num22;
						num19 = leaderSkillResult.specialAttackUpPercent * (float)num18;
						num20 = num23;
						num21 = targetCharacter.leaderSkillResult.specialDefenceUpPercent * (float)num20;
						if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.SpAttackUp))
						{
							num9 = currentSufferState2.onSpAttackUp.upPercent * (float)num18;
						}
						if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.SpAttackDown))
						{
							num10 = currentSufferState2.onSpAttackDown.downPercent * (float)num18;
						}
						if (currentSufferState3.FindSufferState(SufferStateProperty.SufferType.SpDefenceUp))
						{
							num11 = currentSufferState3.onSpDefenceUp.upPercent * (float)num20;
						}
						if (currentSufferState3.FindSufferState(SufferStateProperty.SufferType.SpDefenceDown))
						{
							num12 = currentSufferState3.onSpDefenceDown.downPercent * (float)num20;
						}
					}
					int num24;
					if (j == 0)
					{
						num24 = this.affectEffect[index].power;
					}
					else
					{
						List<ExtraEffectStatus> extraEffectStatus = BattleStateManager.current.battleStateData.extraEffectStatus;
						List<ExtraEffectStatus> invocationList = ExtraEffectStatus.GetInvocationList(extraEffectStatus, ChipEffectStatus.EffectTriggerType.Usually);
						num24 = ExtraEffectStatus.GetSkillPowerCorrectionValue(invocationList, this.affectEffect[index], attackerCharacter);
					}
					float num25 = (float)num18 + num9 - num10 + num19;
					float num26 = (float)num20 + num11 - num12 + num21;
					if (num25 <= 0f)
					{
						array[j] = 0f;
					}
					else
					{
						num25 = Mathf.Max(num25, 0f);
						num26 = Mathf.Max(num26, 1f);
						float num27 = (float)attackerCharacter.level * 0.01f + 1f;
						float num28 = (float)(num24 + num13) * (1f + leaderSkillResult.damageUpPercent);
						float num29 = num27 * num28 * num25 * num5 / num26 + 2f;
						float num30 = num14 * reduceDamageRate2 * num4 * num3 * num * attributeDamegeResult * num6;
						array[j] = num29 * num30;
					}
				}
			}
			else if (this.affectEffect[index].powerType == PowerType.Fixable)
			{
				HaveSufferState currentSufferState4 = targetCharacter.currentSufferState;
				float reduceDamageRate3 = SkillStatus.GetReduceDamageRate(currentSufferState4);
				skillResults.onWeakHit = targetCharacter.tolerance.GetAttributeStrength(this.affectEffect[index].attribute);
				if (RandomExtension.Switch(this.affectEffect[index].satisfactionRate, null))
				{
					skillResults.onCriticalHit = true;
				}
				for (int k = 0; k < array.Length; k++)
				{
					array[k] = (float)this.affectEffect[index].damagePower * reduceDamageRate3;
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
		if (skillResults.onWeakHit != Strength.Drain)
		{
			targetCharacter.hp -= skillResults.attackPower;
		}
		else
		{
			targetCharacter.hp += skillResults.attackPower;
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
		else if (targetSufferState.FindSufferState(SufferStateProperty.SufferType.CountGuard) && targetSufferState.onCountGuard.currentKeepRound > 0)
		{
			result = 1f - targetSufferState.onCountGuard.damagePercent;
			targetSufferState.onCountGuard.currentKeepRound--;
			if (targetSufferState.onCountGuard.currentKeepRound <= 0)
			{
				targetSufferState.RemoveSufferState(SufferStateProperty.SufferType.CountGuard);
			}
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
