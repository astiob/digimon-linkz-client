using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityExtension;

[Serializable]
public class AffectEffectProperty
{
	private const int IntValueLength = 2;

	private const int FloatValueLength = 6;

	private int _skillId;

	private float _hitRate;

	[SerializeField]
	[FormerlySerializedAs("_numbers")]
	private EffectNumbers _effectNumbers;

	[SerializeField]
	private global::Attribute _attribute;

	[SerializeField]
	private AffectEffect _type;

	[SerializeField]
	private PowerType _powerType;

	[SerializeField]
	private TechniqueType _techniqueType;

	[SerializeField]
	[FormerlySerializedAs("intValue")]
	private int[] _intValue = new int[2];

	[SerializeField]
	[FormerlySerializedAs("floatValue")]
	private float[] _floatValue = new float[6];

	private System.Random _random = new System.Random();

	private bool _useDrain;

	public AffectEffectProperty()
	{
	}

	public AffectEffectProperty(AffectEffect type, int skillId, float hitRate, EffectTarget target, EffectNumbers effectNumbers, int[] intValue, float[] floatValue, bool onDrain, PowerType powerType, TechniqueType techniqueType, global::Attribute attribute, bool isMissThrough)
	{
		this._type = type;
		this._skillId = skillId;
		this._hitRate = hitRate;
		this._techniqueType = techniqueType;
		this._attribute = attribute;
		this.target = target;
		this._effectNumbers = effectNumbers;
		this._powerType = powerType;
		this._floatValue = floatValue;
		this._intValue = intValue;
		this._useDrain = onDrain;
		this.isMissThrough = isMissThrough;
		this._random = new System.Random();
	}

	public EffectTarget target { get; private set; }

	public AffectEffect type
	{
		get
		{
			return this._type;
		}
		set
		{
			this._type = value;
		}
	}

	public TechniqueType techniqueType
	{
		get
		{
			return this._techniqueType;
		}
		set
		{
			this._techniqueType = value;
		}
	}

	public int skillId
	{
		get
		{
			return this._skillId;
		}
		set
		{
			this._skillId = Mathf.Clamp(value, 0, int.MaxValue);
		}
	}

	public int power
	{
		get
		{
			return this._intValue[1];
		}
		set
		{
			this._intValue[1] = Mathf.Clamp(value, 0, int.MaxValue);
		}
	}

	public int upPower
	{
		get
		{
			return this._intValue[1];
		}
		set
		{
			this._intValue[1] = Mathf.Clamp(value, 0, int.MaxValue);
		}
	}

	public int downPower
	{
		get
		{
			return this._intValue[1];
		}
		set
		{
			this._intValue[1] = Mathf.Clamp(value, 0, int.MaxValue);
		}
	}

	public int revivalPower
	{
		get
		{
			return this._intValue[1];
		}
		set
		{
			this._intValue[1] = Mathf.Clamp(value, 0, int.MaxValue);
		}
	}

	public int damagePower
	{
		get
		{
			return this._intValue[1];
		}
		set
		{
			this._intValue[1] = Mathf.Clamp(value, 0, int.MaxValue);
		}
	}

	public float damagePercent
	{
		get
		{
			return this._floatValue[0];
		}
		set
		{
			this._floatValue[0] = Mathf.Clamp01(value);
		}
	}

	public float hitRate
	{
		get
		{
			return this._hitRate;
		}
		set
		{
			this._hitRate = Mathf.Clamp01(value);
		}
	}

	public float satisfactionRate
	{
		get
		{
			return this._floatValue[1];
		}
		set
		{
			this._floatValue[1] = Mathf.Clamp01(value);
		}
	}

	public float incidenceRate
	{
		get
		{
			return this._floatValue[0];
		}
		set
		{
			this._floatValue[0] = Mathf.Clamp01(value);
		}
	}

	public float upPercent
	{
		get
		{
			return this._floatValue[0];
		}
		set
		{
			this._floatValue[0] = Mathf.Clamp01(value);
		}
	}

	public float downPercent
	{
		get
		{
			return this._floatValue[0];
		}
		set
		{
			this._floatValue[0] = Mathf.Clamp01(value);
		}
	}

	public float revivalPercent
	{
		get
		{
			return this._floatValue[0];
		}
		set
		{
			this._floatValue[0] = Mathf.Clamp01(value);
		}
	}

	public float physicUpPercent
	{
		get
		{
			return this._floatValue[0];
		}
		set
		{
			this._floatValue[0] = Mathf.Clamp(value, 1f, float.PositiveInfinity);
		}
	}

	public float specialUpPercent
	{
		get
		{
			return this._floatValue[1];
		}
		set
		{
			this._floatValue[1] = Mathf.Clamp(value, 1f, float.PositiveInfinity);
		}
	}

	public float recieveDamageRate
	{
		get
		{
			return this._floatValue[0];
		}
		set
		{
			this._floatValue[0] = Mathf.Clamp(value, 0f, 1f);
		}
	}

	public float damageGetupIncidenceRate
	{
		get
		{
			return this._floatValue[1];
		}
		set
		{
			this._floatValue[1] = Mathf.Clamp01(value);
		}
	}

	public float selfGetupIncidenceRate
	{
		get
		{
			return this._floatValue[0];
		}
		set
		{
			this._floatValue[0] = Mathf.Clamp01(value);
		}
	}

	public int keepRoundNumber
	{
		get
		{
			return this._intValue[0];
		}
		set
		{
			this._intValue[0] = Mathf.Clamp(value, 1, int.MaxValue);
		}
	}

	public int chargeRoundNumber
	{
		get
		{
			return this._intValue[0];
		}
		set
		{
			this._intValue[0] = Mathf.Clamp(value, 1, int.MaxValue);
		}
	}

	public int hitNumber
	{
		get
		{
			return this._intValue[0];
		}
		set
		{
			this._intValue[0] = Mathf.Clamp(value, 1, int.MaxValue);
		}
	}

	public PowerType powerType
	{
		get
		{
			return this._powerType;
		}
		set
		{
			this._powerType = value;
		}
	}

	public EffectNumbers effectNumbers
	{
		get
		{
			return this._effectNumbers;
		}
		set
		{
			this._effectNumbers = value;
		}
	}

	public global::Attribute attribute
	{
		get
		{
			return this._attribute;
		}
		set
		{
			this._attribute = value;
		}
	}

	public float clearPoisonIncidenceRate
	{
		get
		{
			return this._floatValue[0];
		}
		set
		{
			this._floatValue[0] = Mathf.Clamp01(value);
		}
	}

	public float clearConfusionIncidenceRate
	{
		get
		{
			return this._floatValue[1];
		}
		set
		{
			this._floatValue[1] = Mathf.Clamp01(value);
		}
	}

	public float clearParalysisIncidenceRate
	{
		get
		{
			return this._floatValue[2];
		}
		set
		{
			this._floatValue[2] = Mathf.Clamp01(value);
		}
	}

	public float clearSleepIncidenceRate
	{
		get
		{
			return this._floatValue[3];
		}
		set
		{
			this._floatValue[3] = Mathf.Clamp01(value);
		}
	}

	public float clearStunIncidenceRate
	{
		get
		{
			return this._floatValue[4];
		}
		set
		{
			this._floatValue[4] = Mathf.Clamp01(value);
		}
	}

	public float clearSkillLockIncidenceRate
	{
		get
		{
			return this._floatValue[5];
		}
		set
		{
			this._floatValue[5] = Mathf.Clamp01(value);
		}
	}

	public bool useDrain
	{
		get
		{
			return this._useDrain;
		}
		set
		{
			this._useDrain = value;
		}
	}

	public float damageRateForPhantomStudents
	{
		get
		{
			return this._floatValue[0];
		}
	}

	public float damageRateForHeatHaze
	{
		get
		{
			return this._floatValue[1];
		}
	}

	public float damageRateForGlacier
	{
		get
		{
			return this._floatValue[2];
		}
	}

	public float damageRateForElectromagnetic
	{
		get
		{
			return this._floatValue[3];
		}
	}

	public float damageRateForEarth
	{
		get
		{
			return this._floatValue[4];
		}
	}

	public float damageRateForShaftOfLight
	{
		get
		{
			return this._floatValue[5];
		}
	}

	public float damageRateForAbyss
	{
		get
		{
			return this._floatValue[6];
		}
	}

	public int apDrainPower
	{
		get
		{
			return this._intValue[1];
		}
		set
		{
			this._intValue[1] = Mathf.Clamp(value, 0, int.MaxValue);
		}
	}

	public float turnRate
	{
		get
		{
			return this._floatValue[1];
		}
	}

	public float maxValue
	{
		get
		{
			return this._floatValue[2];
		}
	}

	public bool ThisSkillIsAttack
	{
		get
		{
			return this.type == AffectEffect.Damage;
		}
	}

	public bool ThisSkillIsHpRevival
	{
		get
		{
			return this.type == AffectEffect.HpRevival;
		}
	}

	public bool isMissThrough { get; private set; }

	public bool OnHit(CharacterStateControl attacker, CharacterStateControl target)
	{
		if (SkillStatus.onHitRate100Percent)
		{
			return true;
		}
		float num = this.hitRate;
		List<ExtraEffectStatus> extraEffectStatus = BattleStateManager.current.battleStateData.extraEffectStatus;
		List<ExtraEffectStatus> invocationList = ExtraEffectStatus.GetInvocationList(extraEffectStatus, ChipEffectStatus.EffectTriggerType.Usually);
		num = ExtraEffectStatus.GetSkillHitRateCorrectionValue(invocationList, this, attacker);
		num += attacker.chipAddHit;
		foreach (int num2 in attacker.potencyChipIdList.Keys)
		{
			GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffectDataToId = ChipDataMng.GetChipEffectDataToId(num2.ToString());
			GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects = new GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[]
			{
				chipEffectDataToId
			};
			num += ChipEffectStatus.GetSkillHitRateCorrectionValue(chipEffects, this, attacker);
		}
		if (this.powerType != PowerType.Fixable)
		{
			if (attacker.currentSufferState.FindSufferState(SufferStateProperty.SufferType.HitRateUp))
			{
				num += attacker.currentSufferState.onHitRateUp.upPercent;
			}
			if (attacker.currentSufferState.FindSufferState(SufferStateProperty.SufferType.HitRateDown))
			{
				num -= attacker.currentSufferState.onHitRateDown.downPercent;
			}
			num += attacker.leaderSkillResult.hitRateUpPercent;
			if (Tolerance.OnInfluenceToleranceAffectEffect(this.type))
			{
				Tolerance tolerance = target.tolerance;
				Strength affectEffectStrength = tolerance.GetAffectEffectStrength(this.type);
				if (affectEffectStrength == Strength.Strong)
				{
					num *= 0.5f;
				}
				else if (affectEffectStrength == Strength.Weak)
				{
					num *= 1.5f;
				}
				else if (affectEffectStrength == Strength.Invalid)
				{
					num *= 0f;
				}
			}
		}
		return RandomExtension.Switch(Mathf.Clamp01(num), this._random);
	}

	public bool OnHit(CharacterStateControl target)
	{
		if (SkillStatus.onHitRate100Percent)
		{
			return true;
		}
		float num = this.hitRate;
		if (this.powerType != PowerType.Fixable && Tolerance.OnInfluenceToleranceAffectEffect(this.type))
		{
			Tolerance tolerance = target.tolerance;
			Strength affectEffectStrength = tolerance.GetAffectEffectStrength(this.type);
			if (affectEffectStrength == Strength.Strong)
			{
				num *= 0.5f;
			}
			else if (affectEffectStrength == Strength.Weak)
			{
				num *= 1.5f;
			}
			else if (affectEffectStrength == Strength.Invalid)
			{
				num *= 0f;
			}
		}
		return RandomExtension.Switch(Mathf.Clamp01(num), this._random);
	}

	public int GetHate()
	{
		if (this.type == AffectEffect.Damage)
		{
			return 20;
		}
		if (this.type == AffectEffect.Poison || this.type == AffectEffect.Paralysis || this.type == AffectEffect.Sleep || this.type == AffectEffect.SkillLock || this.type == AffectEffect.InstantDeath || this.type == AffectEffect.Confusion || this.type == AffectEffect.Stun)
		{
			return 10;
		}
		if (this.type == AffectEffect.HpRevival)
		{
			return 15;
		}
		return 5;
	}

	public void SetRandomSeed(int seed)
	{
		this._random = new System.Random(seed);
	}
}
