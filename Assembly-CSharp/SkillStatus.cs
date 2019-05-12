using System;
using System.Collections.Generic;
using System.Linq;
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

	public SkillResults OnAttack(int count, int index, CharacterStateControl attackerCharacter, CharacterStateControl targetCharacter, bool onRandomAttack = true)
	{
		SkillResults[] array = new SkillResults[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = this.OnAttack(index, attackerCharacter, targetCharacter, onRandomAttack);
		}
		SkillResults skillResults = new SkillResults();
		skillResults.onMissHit = (array.Where((SkillResults item) => !item.onMissHit).Count<SkillResults>() == 0);
		if (skillResults.onMissHit)
		{
			skillResults.originalAttackPower = 0;
			skillResults.attackPower = 0;
		}
		else
		{
			for (int j = 0; j < array.Length; j++)
			{
				skillResults.originalAttackPower += array[j].originalAttackPower;
				skillResults.attackPower += array[j].attackPower;
			}
			skillResults.onCriticalHit = (array.Where((SkillResults item) => item.onCriticalHit).Count<SkillResults>() > 0);
			skillResults.onWeakHit = array[0].onWeakHit;
		}
		skillResults.targetCharacter = array[0].targetCharacter;
		return skillResults;
	}

	public SkillResults OnAttack(int index, CharacterStateControl attackerCharacter, CharacterStateControl targetCharacter, bool onRandomAttack = true)
	{
		SkillResults skillResults = new SkillResults();
		float[] array = new float[2];
		skillResults.targetCharacter = targetCharacter;
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
				if (RandomExtension.Switch(this.affectEffect[index].satisfactionRate))
				{
					skillResults.onCriticalHit = true;
				}
				int num = (int)(this.affectEffect[index].damagePercent * (float)targetCharacter.hp);
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = (float)num * reduceDamageRate;
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
				float num2 = 1f;
				if (skillResults.onWeakHit == Strength.None || skillResults.onWeakHit == Strength.Weak)
				{
					float num3 = this.affectEffect[index].satisfactionRate;
					if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.SatisfactionRateUp))
					{
						num3 += currentSufferState2.onSatisfactionRateUp.upPercent;
					}
					if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.SatisfactionRateDown))
					{
						num3 -= currentSufferState2.onSatisfactionRateDown.downPercent;
					}
					if (attackerCharacter.isSelectSkill > 0)
					{
						num3 += leaderSkillResult.satisfactionRateUpPercent;
					}
					num3 += attackerCharacter.chipAddCritical;
					float percentage = Mathf.Clamp(num3, 0f, 1f);
					skillResults.onCriticalHit = RandomExtension.Switch(percentage);
					if (skillResults.onCriticalHit)
					{
						num2 = 1.2f;
					}
				}
				float num4 = 1f;
				if (onRandomAttack)
				{
					num4 = UnityEngine.Random.Range(0.85f, 1f);
				}
				float reduceDamageRate2 = SkillStatus.GetReduceDamageRate(currentSufferState3);
				float num5 = 1f;
				if (flag)
				{
					if (targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Counter))
					{
						num5 *= currentSufferState3.onCounter.recieveDamageRate;
					}
				}
				else if (targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Reflection))
				{
					num5 *= currentSufferState3.onReflection.recieveDamageRate;
				}
				float num6 = 1f;
				if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.PowerCharge))
				{
					if (flag)
					{
						num6 *= currentSufferState2.onPowerCharge.physicUpPercent;
					}
					else
					{
						num6 *= currentSufferState2.onPowerCharge.specialUpPercent;
					}
				}
				float num7 = 1f;
				float num8 = 0f;
				float num9 = 0f;
				if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.DamageRateUp))
				{
					if (targetCharacter.species == Species.PhantomStudents)
					{
						num8 += currentSufferState2.onDamageRateUp.damageRateForPhantomStudents;
					}
					else if (targetCharacter.species == Species.HeatHaze)
					{
						num8 += currentSufferState2.onDamageRateUp.damageRateForHeatHaze;
					}
					else if (targetCharacter.species == Species.Glacier)
					{
						num8 += currentSufferState2.onDamageRateUp.damageRateForGlacier;
					}
					else if (targetCharacter.species == Species.Electromagnetic)
					{
						num8 += currentSufferState2.onDamageRateUp.damageRateForElectromagnetic;
					}
					else if (targetCharacter.species == Species.Earth)
					{
						num8 += currentSufferState2.onDamageRateUp.damageRateForEarth;
					}
					else if (targetCharacter.species == Species.ShaftOfLight)
					{
						num8 += currentSufferState2.onDamageRateUp.damageRateForShaftOfLight;
					}
					else if (targetCharacter.species == Species.Abyss)
					{
						num8 += currentSufferState2.onDamageRateUp.damageRateForAbyss;
					}
				}
				if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.DamageRateDown))
				{
					if (targetCharacter.species == Species.PhantomStudents)
					{
						num9 += currentSufferState2.onDamageRateDown.damageRateForPhantomStudents;
					}
					else if (targetCharacter.species == Species.HeatHaze)
					{
						num9 += currentSufferState2.onDamageRateDown.damageRateForHeatHaze;
					}
					else if (targetCharacter.species == Species.Glacier)
					{
						num9 += currentSufferState2.onDamageRateDown.damageRateForGlacier;
					}
					else if (targetCharacter.species == Species.Electromagnetic)
					{
						num9 += currentSufferState2.onDamageRateDown.damageRateForElectromagnetic;
					}
					else if (targetCharacter.species == Species.Earth)
					{
						num9 += currentSufferState2.onDamageRateDown.damageRateForEarth;
					}
					else if (targetCharacter.species == Species.ShaftOfLight)
					{
						num9 += currentSufferState2.onDamageRateDown.damageRateForShaftOfLight;
					}
					else if (targetCharacter.species == Species.Abyss)
					{
						num9 += currentSufferState2.onDamageRateDown.damageRateForAbyss;
					}
				}
				num7 = Mathf.Max(0f, num7 + num8 - num9);
				for (int j = 0; j < array.Length; j++)
				{
					float num10 = 0f;
					float num11 = 0f;
					float num12 = 0f;
					float num13 = 0f;
					int num14 = 0;
					float num15 = 1f;
					foreach (int num16 in attackerCharacter.potencyChipIdList.Keys)
					{
						GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffectDataToId = ChipDataMng.GetChipEffectDataToId(num16.ToString());
						if (j != 0 || chipEffectDataToId.effectTrigger.ToInt32() != 11)
						{
							GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects = new GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[]
							{
								chipEffectDataToId
							};
							num14 += ChipEffectStatus.GetSkillPowerCorrectionValue(chipEffects, this.affectEffect[index], attackerCharacter);
							num15 += ChipEffectStatus.GetDamageRateCorrectionValue(chipEffects, num15, attackerCharacter, targetCharacter);
						}
					}
					int num19;
					float num20;
					int num21;
					float num22;
					if (flag)
					{
						int num17 = (j != 0) ? attackerCharacter.extraAttackPower : attackerCharacter.attackPower;
						int num18 = (j != 0) ? targetCharacter.extraDefencePower : targetCharacter.defencePower;
						num19 = num17;
						num20 = leaderSkillResult.attackUpPercent * (float)num19;
						num21 = num18;
						num22 = targetCharacter.leaderSkillResult.defenceUpPercent * (float)num21;
						if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.AttackUp))
						{
							num10 = currentSufferState2.onAttackUp.upPercent * (float)num19;
						}
						if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.AttackDown))
						{
							num11 = currentSufferState2.onAttackDown.downPercent * (float)num19;
						}
						if (currentSufferState3.FindSufferState(SufferStateProperty.SufferType.DefenceUp))
						{
							num12 = currentSufferState3.onDefenceUp.upPercent * (float)num21;
						}
						if (currentSufferState3.FindSufferState(SufferStateProperty.SufferType.DefenceDown))
						{
							num13 = currentSufferState3.onDefenceDown.downPercent * (float)num21;
						}
					}
					else
					{
						int num23 = (j != 0) ? attackerCharacter.extraSpecialAttackPower : attackerCharacter.specialAttackPower;
						int num24 = (j != 0) ? targetCharacter.extraSpecialDefencePower : targetCharacter.specialDefencePower;
						num19 = num23;
						num20 = leaderSkillResult.specialAttackUpPercent * (float)num19;
						num21 = num24;
						num22 = targetCharacter.leaderSkillResult.specialDefenceUpPercent * (float)num21;
						if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.SpAttackUp))
						{
							num10 = currentSufferState2.onSpAttackUp.upPercent * (float)num19;
						}
						if (currentSufferState2.FindSufferState(SufferStateProperty.SufferType.SpAttackDown))
						{
							num11 = currentSufferState2.onSpAttackDown.downPercent * (float)num19;
						}
						if (currentSufferState3.FindSufferState(SufferStateProperty.SufferType.SpDefenceUp))
						{
							num12 = currentSufferState3.onSpDefenceUp.upPercent * (float)num21;
						}
						if (currentSufferState3.FindSufferState(SufferStateProperty.SufferType.SpDefenceDown))
						{
							num13 = currentSufferState3.onSpDefenceDown.downPercent * (float)num21;
						}
					}
					int num26;
					if (this.affectEffect[index].type == AffectEffect.HpBorderlineDamage)
					{
						float num25 = (float)attackerCharacter.hp / (float)attackerCharacter.maxHp;
						if (num25 >= this.affectEffect[index].borderlineRange1)
						{
							num26 = (int)this.affectEffect[index].borderlineDamage1;
						}
						else if (num25 >= this.affectEffect[index].borderlineRange2)
						{
							num26 = (int)this.affectEffect[index].borderlineDamage2;
						}
						else
						{
							num26 = (int)this.affectEffect[index].defaultDamage;
						}
					}
					else if (j == 0)
					{
						num26 = this.affectEffect[index].power;
					}
					else
					{
						List<ExtraEffectStatus> extraEffectStatus = BattleStateManager.current.battleStateData.extraEffectStatus;
						List<ExtraEffectStatus> invocationList = ExtraEffectStatus.GetInvocationList(extraEffectStatus, ChipEffectStatus.EffectTriggerType.Usually);
						num26 = ExtraEffectStatus.GetSkillPowerCorrectionValue(invocationList, this.affectEffect[index], attackerCharacter);
					}
					float num27 = (float)num19 + num10 - num11 + num20;
					float num28 = (float)num21 + num12 - num13 + num22;
					if (num27 <= 0f)
					{
						array[j] = 0f;
					}
					else
					{
						num27 = Mathf.Max(num27, 0f);
						num28 = Mathf.Max(num28, 1f);
						float num29 = (float)attackerCharacter.level * 0.01f + 1f;
						float num30 = (float)(num26 + num14) * (1f + leaderSkillResult.damageUpPercent);
						float num31 = num29 * num30 * num27 * num6 / num28 + 2f;
						float num32 = num15 * reduceDamageRate2 * num5 * num4 * num2 * attributeDamegeResult * num7;
						array[j] = num31 * num32;
					}
				}
			}
			else if (this.affectEffect[index].powerType == PowerType.Fixable)
			{
				HaveSufferState currentSufferState4 = targetCharacter.currentSufferState;
				float reduceDamageRate3 = SkillStatus.GetReduceDamageRate(currentSufferState4);
				skillResults.onWeakHit = targetCharacter.tolerance.GetAttributeStrength(this.affectEffect[index].attribute);
				if (RandomExtension.Switch(this.affectEffect[index].satisfactionRate))
				{
					skillResults.onCriticalHit = true;
				}
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
