using System;
using System.Collections.Generic;

[Serializable]
public class HaveSufferState
{
	private Dictionary<SufferStateProperty.SufferType, SufferStateProperty> sufferStatePropertyDictionary;

	public List<SufferStateProperty.SufferType> sufferOrderList = new List<SufferStateProperty.SufferType>();

	public HaveSufferState()
	{
		this.sufferStatePropertyDictionary = new Dictionary<SufferStateProperty.SufferType, SufferStateProperty>();
		Array values = Enum.GetValues(typeof(SufferStateProperty.SufferType));
		foreach (object obj in values)
		{
			SufferStateProperty.SufferType sufferType = (SufferStateProperty.SufferType)((int)obj);
			SufferStateProperty value = new SufferStateProperty(sufferType);
			this.sufferStatePropertyDictionary.Add(sufferType, value);
		}
	}

	public SufferStateProperty GetSufferStateProperty(SufferStateProperty.SufferType sufferType)
	{
		return this.sufferStatePropertyDictionary[sufferType];
	}

	public SufferStateProperty onPoison
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.Poison];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.Poison] = value;
		}
	}

	public SufferStateProperty onConfusion
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.Confusion];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.Confusion] = value;
		}
	}

	public SufferStateProperty onParalysis
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.Paralysis];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.Paralysis] = value;
		}
	}

	public SufferStateProperty onSleep
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.Sleep];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.Sleep] = value;
		}
	}

	public SufferStateProperty onStun
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.Stun];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.Stun] = value;
		}
	}

	public SufferStateProperty onSkillLock
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.SkillLock];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.SkillLock] = value;
		}
	}

	public SufferStateProperty onAttackUp
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.AttackUp];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.AttackUp] = value;
		}
	}

	public SufferStateProperty onAttackDown
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.AttackDown];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.AttackDown] = value;
		}
	}

	public SufferStateProperty onDefenceUp
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.DefenceUp];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.DefenceUp] = value;
		}
	}

	public SufferStateProperty onDefenceDown
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.DefenceDown];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.DefenceDown] = value;
		}
	}

	public SufferStateProperty onSpAttackUp
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.SpAttackUp];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.SpAttackUp] = value;
		}
	}

	public SufferStateProperty onSpAttackDown
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.SpAttackDown];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.SpAttackDown] = value;
		}
	}

	public SufferStateProperty onSpDefenceUp
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.SpDefenceUp];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.SpDefenceUp] = value;
		}
	}

	public SufferStateProperty onSpDefenceDown
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.SpDefenceDown];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.SpDefenceDown] = value;
		}
	}

	public SufferStateProperty onSpeedUp
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.SpeedUp];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.SpeedUp] = value;
		}
	}

	public SufferStateProperty onSpeedDown
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.SpeedDown];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.SpeedDown] = value;
		}
	}

	public SufferStateProperty onCounter
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.Counter];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.Counter] = value;
		}
	}

	public SufferStateProperty onReflection
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.Reflection];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.Reflection] = value;
		}
	}

	public SufferStateProperty onProtect
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.Protect];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.Protect] = value;
		}
	}

	public SufferStateProperty onPowerCharge
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.PowerCharge];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.PowerCharge] = value;
		}
	}

	public SufferStateProperty onHitRateUp
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.HitRateUp];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.HitRateUp] = value;
		}
	}

	public SufferStateProperty onHitRateDown
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.HitRateDown];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.HitRateDown] = value;
		}
	}

	public SufferStateProperty onSatisfactionRateUp
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.SatisfactionRateUp];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.SatisfactionRateUp] = value;
		}
	}

	public SufferStateProperty onSatisfactionRateDown
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.SatisfactionRateDown];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.SatisfactionRateDown] = value;
		}
	}

	public SufferStateProperty onApRevival
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.ApRevival];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.ApRevival] = value;
		}
	}

	public SufferStateProperty onApConsumptionUp
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.ApConsumptionUp];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.ApConsumptionUp] = value;
		}
	}

	public SufferStateProperty onApConsumptionDown
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.ApConsumptionDown];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.ApConsumptionDown] = value;
		}
	}

	public SufferStateProperty onCountGuard
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.CountGuard];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.CountGuard] = value;
		}
	}

	public SufferStateProperty onTurnBarrier
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.TurnBarrier];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.TurnBarrier] = value;
		}
	}

	public SufferStateProperty onCountBarrier
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.CountBarrier];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.CountBarrier] = value;
		}
	}

	public SufferStateProperty onDamageRateUp
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.DamageRateUp];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.DamageRateUp] = value;
		}
	}

	public SufferStateProperty onDamageRateDown
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.DamageRateDown];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.DamageRateDown] = value;
		}
	}

	public SufferStateProperty onRegenerate
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.Regenerate];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.Regenerate] = value;
		}
	}

	public SufferStateProperty onTurnEvasion
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.TurnEvasion];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.TurnEvasion] = value;
		}
	}

	public SufferStateProperty onCountEvasion
	{
		get
		{
			return this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.CountEvasion];
		}
		set
		{
			this.sufferStatePropertyDictionary[SufferStateProperty.SufferType.CountEvasion] = value;
		}
	}

	public bool FindSufferState(SufferStateProperty.SufferType type)
	{
		return this.GetSufferStateProperty(type).isActive;
	}

	public void RemoveSufferState(SufferStateProperty.SufferType type)
	{
		if (this.sufferOrderList.Contains(type))
		{
			this.sufferOrderList.Remove(type);
		}
		this.GetSufferStateProperty(type).SetNull();
	}

	public void SetSufferState(SufferStateProperty suffer, CharacterStateControl status = null)
	{
		if (status != null)
		{
			suffer.TriggerCharacter = status;
		}
		this.RemoveSufferOnOrderList(suffer);
		if (!suffer.isActive)
		{
			return;
		}
		switch (suffer.sufferTypeCache)
		{
		case SufferStateProperty.SufferType.Poison:
			this.onPoison = suffer;
			break;
		case SufferStateProperty.SufferType.Confusion:
			this.onConfusion = suffer;
			break;
		case SufferStateProperty.SufferType.Paralysis:
			this.onParalysis = suffer;
			break;
		case SufferStateProperty.SufferType.Sleep:
			this.onSleep = suffer;
			break;
		case SufferStateProperty.SufferType.Stun:
			this.onStun = suffer;
			break;
		case SufferStateProperty.SufferType.SkillLock:
			this.onSkillLock = suffer;
			break;
		case SufferStateProperty.SufferType.AttackUp:
			this.onAttackUp = suffer;
			break;
		case SufferStateProperty.SufferType.AttackDown:
			this.onAttackDown = suffer;
			break;
		case SufferStateProperty.SufferType.DefenceUp:
			this.onDefenceUp = suffer;
			break;
		case SufferStateProperty.SufferType.DefenceDown:
			this.onDefenceDown = suffer;
			break;
		case SufferStateProperty.SufferType.SpAttackUp:
			this.onSpAttackUp = suffer;
			break;
		case SufferStateProperty.SufferType.SpAttackDown:
			this.onSpAttackDown = suffer;
			break;
		case SufferStateProperty.SufferType.SpDefenceUp:
			this.onSpDefenceUp = suffer;
			break;
		case SufferStateProperty.SufferType.SpDefenceDown:
			this.onSpDefenceDown = suffer;
			break;
		case SufferStateProperty.SufferType.SpeedUp:
			this.onSpeedUp = suffer;
			break;
		case SufferStateProperty.SufferType.SpeedDown:
			this.onSpeedDown = suffer;
			break;
		case SufferStateProperty.SufferType.Counter:
			this.onCounter = suffer;
			break;
		case SufferStateProperty.SufferType.Reflection:
			this.onReflection = suffer;
			break;
		case SufferStateProperty.SufferType.Protect:
			this.onProtect = suffer;
			break;
		case SufferStateProperty.SufferType.PowerCharge:
			this.onPowerCharge = suffer;
			break;
		case SufferStateProperty.SufferType.HitRateUp:
			this.onHitRateUp = suffer;
			break;
		case SufferStateProperty.SufferType.HitRateDown:
			this.onHitRateDown = suffer;
			break;
		case SufferStateProperty.SufferType.SatisfactionRateUp:
			this.onSatisfactionRateUp = suffer;
			break;
		case SufferStateProperty.SufferType.SatisfactionRateDown:
			this.onSatisfactionRateDown = suffer;
			break;
		case SufferStateProperty.SufferType.ApRevival:
			this.onApRevival = suffer;
			break;
		case SufferStateProperty.SufferType.ApConsumptionUp:
			this.onApConsumptionUp = suffer;
			break;
		case SufferStateProperty.SufferType.ApConsumptionDown:
			this.onApConsumptionDown = suffer;
			break;
		case SufferStateProperty.SufferType.CountGuard:
			this.onCountGuard = suffer;
			break;
		case SufferStateProperty.SufferType.TurnBarrier:
			this.onTurnBarrier = suffer;
			break;
		case SufferStateProperty.SufferType.CountBarrier:
			this.onCountBarrier = suffer;
			break;
		case SufferStateProperty.SufferType.DamageRateUp:
			this.onDamageRateUp = suffer;
			break;
		case SufferStateProperty.SufferType.DamageRateDown:
			this.onDamageRateDown = suffer;
			break;
		case SufferStateProperty.SufferType.Regenerate:
			this.onRegenerate = suffer;
			break;
		case SufferStateProperty.SufferType.TurnEvasion:
			this.onTurnEvasion = suffer;
			break;
		case SufferStateProperty.SufferType.CountEvasion:
			this.onCountEvasion = suffer;
			break;
		}
		this.sufferOrderList.Add(suffer.sufferTypeCache);
	}

	public void RoundCount()
	{
		this.RoundCountInternal(new SufferStateProperty[]
		{
			this.onPoison,
			this.onConfusion,
			this.onParalysis,
			this.onSleep,
			this.onStun,
			this.onSkillLock,
			this.onAttackUp,
			this.onAttackDown,
			this.onDefenceUp,
			this.onDefenceDown,
			this.onSpAttackUp,
			this.onSpAttackDown,
			this.onSpDefenceUp,
			this.onSpDefenceDown,
			this.onSpeedUp,
			this.onSpeedDown,
			this.onCounter,
			this.onReflection,
			this.onProtect,
			this.onPowerCharge,
			this.onHitRateUp,
			this.onHitRateDown,
			this.onSatisfactionRateUp,
			this.onSatisfactionRateDown,
			this.onApRevival,
			this.onApConsumptionUp,
			this.onApConsumptionDown,
			this.onTurnBarrier,
			this.onDamageRateUp,
			this.onDamageRateDown,
			this.onRegenerate,
			this.onTurnEvasion
		});
	}

	private void RoundCountInternal(params SufferStateProperty[] suffers)
	{
		foreach (SufferStateProperty sufferStateProperty in suffers)
		{
			if (sufferStateProperty.isActive)
			{
				sufferStateProperty.currentKeepRound--;
				if (sufferStateProperty.OnDisappearance)
				{
					this.RemoveSufferOnOrderList(sufferStateProperty);
					sufferStateProperty.SetNull();
				}
			}
			else
			{
				this.RemoveSufferOnOrderList(sufferStateProperty);
			}
		}
	}

	private void RemoveSufferOnOrderList(SufferStateProperty sufferState)
	{
		if (this.sufferOrderList.Contains(sufferState.sufferTypeCache))
		{
			this.sufferOrderList.Remove(sufferState.sufferTypeCache);
		}
	}
}
