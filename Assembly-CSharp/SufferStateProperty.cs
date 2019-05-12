using BattleStateMachineInternal;
using JsonFx.Json;
using System;
using System.Linq;
using UnityEngine;
using UnityExtension;

[Serializable]
public class SufferStateProperty
{
	private const int maxIntValueLength = 3;

	private const int maxFloatValueLength = 7;

	public bool isActive;

	private PowerType _powerType;

	[SerializeField]
	private int[] intValue = new int[3];

	[SerializeField]
	private float[] floatValue = new float[7];

	[NonSerialized]
	private SufferStateProperty.SufferType _sufferTypecache;

	private int keepRoundNumber;

	public SufferStateProperty(SufferStateProperty.SufferType typeCache)
	{
		this._sufferTypecache = typeCache;
		this.SetNull();
	}

	public SufferStateProperty()
	{
	}

	public SufferStateProperty(AffectEffectProperty affectProperty, int lastGenerationStartTiming = 0)
	{
		this.isActive = true;
		SufferStateProperty.SufferType sufferTypecache = SufferStateProperty.SufferType.Null;
		this.generationStartTiming = lastGenerationStartTiming;
		switch (affectProperty.type)
		{
		case AffectEffect.AttackUp:
			sufferTypecache = SufferStateProperty.SufferType.AttackUp;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.upPercent = affectProperty.upPercent;
			this.turnRate = affectProperty.turnRate;
			this.maxValue = affectProperty.maxValue;
			break;
		case AffectEffect.AttackDown:
			sufferTypecache = SufferStateProperty.SufferType.AttackDown;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.downPercent = affectProperty.downPercent;
			this.turnRate = affectProperty.turnRate;
			this.maxValue = affectProperty.maxValue;
			break;
		case AffectEffect.DefenceUp:
			sufferTypecache = SufferStateProperty.SufferType.DefenceUp;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.upPercent = affectProperty.upPercent;
			this.turnRate = affectProperty.turnRate;
			this.maxValue = affectProperty.maxValue;
			break;
		case AffectEffect.DefenceDown:
			sufferTypecache = SufferStateProperty.SufferType.DefenceDown;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.downPercent = affectProperty.downPercent;
			this.turnRate = affectProperty.turnRate;
			this.maxValue = affectProperty.maxValue;
			break;
		case AffectEffect.SpAttackUp:
			sufferTypecache = SufferStateProperty.SufferType.SpAttackUp;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.upPercent = affectProperty.upPercent;
			this.turnRate = affectProperty.turnRate;
			this.maxValue = affectProperty.maxValue;
			break;
		case AffectEffect.SpAttackDown:
			sufferTypecache = SufferStateProperty.SufferType.SpAttackDown;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.downPercent = affectProperty.downPercent;
			this.turnRate = affectProperty.turnRate;
			this.maxValue = affectProperty.maxValue;
			break;
		case AffectEffect.SpDefenceUp:
			sufferTypecache = SufferStateProperty.SufferType.SpDefenceUp;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.upPercent = affectProperty.upPercent;
			this.turnRate = affectProperty.turnRate;
			this.maxValue = affectProperty.maxValue;
			break;
		case AffectEffect.SpDefenceDown:
			sufferTypecache = SufferStateProperty.SufferType.SpDefenceDown;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.downPercent = affectProperty.downPercent;
			this.turnRate = affectProperty.turnRate;
			this.maxValue = affectProperty.maxValue;
			break;
		case AffectEffect.SpeedUp:
			sufferTypecache = SufferStateProperty.SufferType.SpeedUp;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.upPercent = affectProperty.upPercent;
			this.turnRate = affectProperty.turnRate;
			this.maxValue = affectProperty.maxValue;
			break;
		case AffectEffect.SpeedDown:
			sufferTypecache = SufferStateProperty.SufferType.SpeedDown;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.downPercent = affectProperty.downPercent;
			this.turnRate = affectProperty.turnRate;
			this.maxValue = affectProperty.maxValue;
			break;
		case AffectEffect.Counter:
			sufferTypecache = SufferStateProperty.SufferType.Counter;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.damagePercent = affectProperty.damagePercent;
			this.recieveDamageRate = affectProperty.recieveDamageRate;
			break;
		case AffectEffect.Reflection:
			sufferTypecache = SufferStateProperty.SufferType.Reflection;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.damagePercent = affectProperty.damagePercent;
			this.recieveDamageRate = affectProperty.recieveDamageRate;
			break;
		case AffectEffect.Protect:
			sufferTypecache = SufferStateProperty.SufferType.Protect;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			break;
		case AffectEffect.PowerCharge:
			sufferTypecache = SufferStateProperty.SufferType.PowerCharge;
			this.keepRoundNumber = affectProperty.chargeRoundNumber;
			this.currentKeepRound = affectProperty.chargeRoundNumber;
			this.physicUpPercent = affectProperty.physicUpPercent;
			this.specialUpPercent = affectProperty.specialUpPercent;
			break;
		case AffectEffect.Paralysis:
			sufferTypecache = SufferStateProperty.SufferType.Paralysis;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.incidenceRate = affectProperty.incidenceRate;
			break;
		case AffectEffect.Poison:
			sufferTypecache = SufferStateProperty.SufferType.Poison;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.powerType = affectProperty.powerType;
			this.damagePower = affectProperty.damagePower;
			this.damagePercent = affectProperty.damagePercent;
			this.turnRate = affectProperty.turnRate;
			this.maxValue = affectProperty.maxValue;
			break;
		case AffectEffect.Sleep:
			sufferTypecache = SufferStateProperty.SufferType.Sleep;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.incidenceRate = affectProperty.incidenceRate;
			this.damageGetupIncidenceRate = affectProperty.damageGetupIncidenceRate;
			this.selfGetupIncidenceRate = affectProperty.selfGetupIncidenceRate;
			break;
		case AffectEffect.SkillLock:
			sufferTypecache = SufferStateProperty.SufferType.SkillLock;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			break;
		case AffectEffect.HitRateUp:
			sufferTypecache = SufferStateProperty.SufferType.HitRateUp;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.upPercent = affectProperty.upPercent;
			this.turnRate = affectProperty.turnRate;
			this.maxValue = affectProperty.maxValue;
			break;
		case AffectEffect.HitRateDown:
			sufferTypecache = SufferStateProperty.SufferType.HitRateDown;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.downPercent = affectProperty.downPercent;
			this.turnRate = affectProperty.turnRate;
			this.maxValue = affectProperty.maxValue;
			break;
		case AffectEffect.Confusion:
			sufferTypecache = SufferStateProperty.SufferType.Confusion;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.incidenceRate = affectProperty.incidenceRate;
			break;
		case AffectEffect.Stun:
			sufferTypecache = SufferStateProperty.SufferType.Stun;
			this.keepRoundNumber = 1;
			this.currentKeepRound = 1;
			break;
		case AffectEffect.SatisfactionRateUp:
			sufferTypecache = SufferStateProperty.SufferType.SatisfactionRateUp;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.upPercent = affectProperty.upPercent;
			this.turnRate = affectProperty.turnRate;
			this.maxValue = affectProperty.maxValue;
			break;
		case AffectEffect.SatisfactionRateDown:
			sufferTypecache = SufferStateProperty.SufferType.SatisfactionRateDown;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.downPercent = affectProperty.downPercent;
			this.turnRate = affectProperty.turnRate;
			this.maxValue = affectProperty.maxValue;
			break;
		case AffectEffect.ApRevival:
			sufferTypecache = SufferStateProperty.SufferType.ApRevival;
			this.powerType = affectProperty.powerType;
			this.revivalPower = affectProperty.revivalPower;
			this.revivalPercent = affectProperty.revivalPercent;
			this.currentKeepRound = 0;
			break;
		case AffectEffect.ApConsumptionUp:
			sufferTypecache = SufferStateProperty.SufferType.ApConsumptionUp;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.upPower = affectProperty.upPower;
			break;
		case AffectEffect.ApConsumptionDown:
			sufferTypecache = SufferStateProperty.SufferType.ApConsumptionDown;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.downPower = affectProperty.downPower;
			break;
		case AffectEffect.CountGuard:
			sufferTypecache = SufferStateProperty.SufferType.CountGuard;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.damagePercent = affectProperty.damagePercent;
			this.isMultiHitThrough = affectProperty.isMultiHitThrough;
			break;
		case AffectEffect.TurnBarrier:
			sufferTypecache = SufferStateProperty.SufferType.TurnBarrier;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			break;
		case AffectEffect.CountBarrier:
			sufferTypecache = SufferStateProperty.SufferType.CountBarrier;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.isMultiHitThrough = affectProperty.isMultiHitThrough;
			break;
		case AffectEffect.DamageRateUp:
			sufferTypecache = SufferStateProperty.SufferType.DamageRateUp;
			this.keepRoundNumber = affectProperty.damageRateKeepRoundNumber;
			this.currentKeepRound = affectProperty.damageRateKeepRoundNumber;
			this.damageRateForPhantomStudents = affectProperty.damageRateForPhantomStudents;
			this.damageRateForHeatHaze = affectProperty.damageRateForHeatHaze;
			this.damageRateForGlacier = affectProperty.damageRateForGlacier;
			this.damageRateForElectromagnetic = affectProperty.damageRateForElectromagnetic;
			this.damageRateForEarth = affectProperty.damageRateForEarth;
			this.damageRateForShaftOfLight = affectProperty.damageRateForShaftOfLight;
			this.damageRateForAbyss = affectProperty.damageRateForAbyss;
			break;
		case AffectEffect.DamageRateDown:
			sufferTypecache = SufferStateProperty.SufferType.DamageRateDown;
			this.keepRoundNumber = affectProperty.damageRateKeepRoundNumber;
			this.currentKeepRound = affectProperty.damageRateKeepRoundNumber;
			this.damageRateForPhantomStudents = affectProperty.damageRateForPhantomStudents;
			this.damageRateForHeatHaze = affectProperty.damageRateForHeatHaze;
			this.damageRateForGlacier = affectProperty.damageRateForGlacier;
			this.damageRateForElectromagnetic = affectProperty.damageRateForElectromagnetic;
			this.damageRateForEarth = affectProperty.damageRateForEarth;
			this.damageRateForShaftOfLight = affectProperty.damageRateForShaftOfLight;
			this.damageRateForAbyss = affectProperty.damageRateForAbyss;
			break;
		case AffectEffect.Regenerate:
			sufferTypecache = SufferStateProperty.SufferType.Regenerate;
			this.powerType = affectProperty.powerType;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.revivalPower = affectProperty.revivalPower;
			this.revivalPercent = affectProperty.revivalPercent;
			this.turnRate = affectProperty.turnRate;
			this.maxValue = affectProperty.maxValue;
			break;
		case AffectEffect.TurnEvasion:
			sufferTypecache = SufferStateProperty.SufferType.TurnEvasion;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			break;
		case AffectEffect.CountEvasion:
			sufferTypecache = SufferStateProperty.SufferType.CountEvasion;
			this.keepRoundNumber = affectProperty.keepRoundNumber;
			this.currentKeepRound = affectProperty.keepRoundNumber;
			this.isMultiHitThrough = affectProperty.isMultiHitThrough;
			break;
		}
		this._sufferTypecache = sufferTypecache;
	}

	public SufferStateProperty.SufferType sufferTypeCache
	{
		get
		{
			return this._sufferTypecache;
		}
	}

	public string GetParams()
	{
		string str = string.Join(",", this.intValue.Select((int x) => x.ToString()).ToArray<string>());
		string str2 = string.Join(",", this.floatValue.Select((float x) => x.ToString()).ToArray<string>());
		string str3 = str + "#" + str2;
		string strVal = str3 + "#" + ((!this.isActive) ? "0" : "1");
		SufferStatePropertyStore value = new SufferStatePropertyStore
		{
			strVal = strVal
		};
		return JsonWriter.Serialize(value);
	}

	public void SetParams(string serializedStr, BattleStateData battleStateData)
	{
		if (string.IsNullOrEmpty(serializedStr))
		{
			return;
		}
		SufferStatePropertyStore sufferStatePropertyStore = JsonReader.Deserialize<SufferStatePropertyStore>(serializedStr);
		string[] array = sufferStatePropertyStore.strVal.Split(new char[]
		{
			'#'
		});
		if (array.Length != 3)
		{
			return;
		}
		string text = array[0];
		string text2 = array[1];
		if (string.IsNullOrEmpty(text))
		{
			this.intValue = new int[3];
		}
		else
		{
			this.intValue = text.Split(new char[]
			{
				','
			}).Select((string i) => int.Parse(i)).ToArray<int>();
		}
		if (string.IsNullOrEmpty(text2))
		{
			this.floatValue = new float[7];
		}
		else
		{
			this.floatValue = text2.Split(new char[]
			{
				','
			}).Select((string i) => float.Parse(i)).ToArray<float>();
		}
		string text3 = array[2];
		if (!string.IsNullOrEmpty(text3))
		{
			this.isActive = Convert.ToBoolean(text3.ToInt32());
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

	public int damagePower
	{
		get
		{
			return this.intValue[1];
		}
		set
		{
			this.intValue[1] = Mathf.Clamp(value, 0, int.MaxValue);
		}
	}

	public float damagePercent
	{
		get
		{
			return this.floatValue[0];
		}
		set
		{
			this.floatValue[0] = Mathf.Clamp01(value);
		}
	}

	public int upPower
	{
		get
		{
			return this.intValue[1];
		}
		set
		{
			this.intValue[1] = Mathf.Clamp(value, 0, int.MaxValue);
		}
	}

	public int downPower
	{
		get
		{
			return this.intValue[1];
		}
		set
		{
			this.intValue[1] = Mathf.Clamp(value, 0, int.MaxValue);
		}
	}

	public int revivalPower
	{
		get
		{
			return this.intValue[1];
		}
		set
		{
			this.intValue[1] = Mathf.Clamp(value, 0, int.MaxValue);
		}
	}

	public float upPercent
	{
		get
		{
			return Mathf.Min(this.floatValue[0] + (float)(this.keepRoundNumber - this.currentKeepRound) * this.turnRate, this.maxValue);
		}
		set
		{
			this.floatValue[0] = Mathf.Clamp(value, 0f, 9f);
		}
	}

	public float downPercent
	{
		get
		{
			return Mathf.Min(this.floatValue[0] + (float)(this.keepRoundNumber - this.currentKeepRound) * this.turnRate, this.maxValue);
		}
		set
		{
			this.floatValue[0] = Mathf.Clamp01(value);
		}
	}

	public float revivalPercent
	{
		get
		{
			return this.floatValue[0];
		}
		set
		{
			this.floatValue[0] = Mathf.Clamp(value, 0f, 1f);
		}
	}

	public float incidenceRate
	{
		get
		{
			return this.floatValue[0];
		}
		set
		{
			this.floatValue[0] = Mathf.Clamp01(value);
		}
	}

	public float physicUpPercent
	{
		get
		{
			return this.floatValue[0];
		}
		set
		{
			this.floatValue[0] = Mathf.Clamp(value, 0f, 9f);
		}
	}

	public float specialUpPercent
	{
		get
		{
			return this.floatValue[1];
		}
		set
		{
			this.floatValue[1] = Mathf.Clamp(value, 0f, 9f);
		}
	}

	public float damageGetupIncidenceRate
	{
		get
		{
			return this.floatValue[1];
		}
		set
		{
			this.floatValue[1] = Mathf.Clamp01(value);
		}
	}

	public float selfGetupIncidenceRate
	{
		get
		{
			return this.floatValue[0];
		}
		set
		{
			this.floatValue[0] = Mathf.Clamp01(value);
		}
	}

	public float recieveDamageRate
	{
		get
		{
			return this.floatValue[1];
		}
		set
		{
			this.floatValue[1] = Mathf.Clamp(value, 0f, 1f);
		}
	}

	public int currentKeepRound
	{
		get
		{
			return this.intValue[0];
		}
		set
		{
			this.intValue[0] = value;
		}
	}

	public int currentChargeRound
	{
		get
		{
			return this.intValue[0];
		}
		set
		{
			this.intValue[0] = value;
		}
	}

	public bool isMultiHitThrough { get; private set; }

	public int generationStartTiming
	{
		get
		{
			return this.intValue[2];
		}
		set
		{
			this.intValue[2] = Mathf.Clamp(value, 0, int.MaxValue);
		}
	}

	public float damageRateForPhantomStudents
	{
		get
		{
			return this.floatValue[0];
		}
		set
		{
			this.floatValue[0] = value;
		}
	}

	public float damageRateForHeatHaze
	{
		get
		{
			return this.floatValue[1];
		}
		set
		{
			this.floatValue[1] = value;
		}
	}

	public float damageRateForGlacier
	{
		get
		{
			return this.floatValue[2];
		}
		set
		{
			this.floatValue[2] = value;
		}
	}

	public float damageRateForElectromagnetic
	{
		get
		{
			return this.floatValue[3];
		}
		set
		{
			this.floatValue[3] = value;
		}
	}

	public float damageRateForEarth
	{
		get
		{
			return this.floatValue[4];
		}
		set
		{
			this.floatValue[4] = value;
		}
	}

	public float damageRateForShaftOfLight
	{
		get
		{
			return this.floatValue[5];
		}
		set
		{
			this.floatValue[5] = value;
		}
	}

	public float damageRateForAbyss
	{
		get
		{
			return this.floatValue[6];
		}
		set
		{
			this.floatValue[6] = value;
		}
	}

	public float turnRate
	{
		get
		{
			return this.floatValue[1];
		}
		set
		{
			this.floatValue[1] = value;
		}
	}

	public float maxValue
	{
		get
		{
			return this.floatValue[2];
		}
		set
		{
			this.floatValue[2] = value;
		}
	}

	public bool OnInvocationPowerChargeAttack
	{
		get
		{
			return this.currentChargeRound < 1;
		}
	}

	public bool OnDisappearance
	{
		get
		{
			return this.currentKeepRound < 0;
		}
	}

	public bool GetOccurrenceFreeze()
	{
		return RandomExtension.Switch(this.floatValue[0]);
	}

	public int GetPoisonDamageFluctuation(CharacterStateControl status)
	{
		if (!status.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Poison))
		{
			return 0;
		}
		if (this.powerType == PowerType.Fixable)
		{
			return this.damagePower;
		}
		float num = Mathf.Min(this.damagePercent + (float)(this.keepRoundNumber - this.currentKeepRound) * this.turnRate, this.maxValue);
		return Mathf.FloorToInt((float)status.extraMaxHp * num);
	}

	public int GetReflectDamage(int damage)
	{
		return Mathf.FloorToInt((float)damage * this.damagePercent);
	}

	public int GetRecieveReflectDamage(int damage)
	{
		return Mathf.FloorToInt((float)damage * this.recieveDamageRate);
	}

	public bool GetSleepGetupOccurrence()
	{
		return RandomExtension.Switch(this.floatValue[0]);
	}

	public bool GetSleepGetupOccurrenceDamage()
	{
		return RandomExtension.Switch(this.floatValue[1]);
	}

	public int GetRevivalAp(int maxAp)
	{
		if (this.powerType == PowerType.Fixable)
		{
			return this.revivalPower;
		}
		return Mathf.FloorToInt((float)maxAp * this.revivalPercent);
	}

	public int GetRegenerate(CharacterStateControl status)
	{
		int result;
		if (this.powerType == PowerType.Fixable)
		{
			result = this.revivalPower;
		}
		else
		{
			float num = Mathf.Min(this.revivalPercent + (float)(this.keepRoundNumber - this.currentKeepRound) * this.turnRate, this.maxValue);
			result = Mathf.FloorToInt((float)status.extraMaxHp * num);
		}
		return result;
	}

	public void SetNull()
	{
		this.isActive = false;
		this._powerType = PowerType.Fixable;
		this.intValue = new int[3];
		this.floatValue = new float[7];
	}

	public enum SufferType
	{
		Null,
		Poison,
		Confusion,
		Paralysis,
		Sleep,
		Stun,
		SkillLock,
		AttackUp,
		AttackDown,
		DefenceUp,
		DefenceDown,
		SpAttackUp,
		SpAttackDown,
		SpDefenceUp,
		SpDefenceDown,
		SpeedUp,
		SpeedDown,
		Counter,
		Reflection,
		Protect,
		PowerCharge,
		HitRateUp,
		HitRateDown,
		SatisfactionRateUp,
		SatisfactionRateDown,
		ApRevival,
		ApConsumptionUp,
		ApConsumptionDown,
		CountGuard,
		TurnBarrier,
		CountBarrier,
		DamageRateUp,
		DamageRateDown,
		Regenerate,
		TurnEvasion,
		CountEvasion
	}
}
