using BattleStateMachineInternal;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class HaveSufferState
{
	private Dictionary<SufferStateProperty.SufferType, SufferStateProperty> sufferStatePropertyDictionary;

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

	public List<SufferStateProperty.SufferType> GetSufferOrderList()
	{
		IEnumerable<KeyValuePair<SufferStateProperty.SufferType, SufferStateProperty>> source = this.sufferStatePropertyDictionary.Where((KeyValuePair<SufferStateProperty.SufferType, SufferStateProperty> item) => item.Value.isActive);
		return source.Select((KeyValuePair<SufferStateProperty.SufferType, SufferStateProperty> item) => item.Key).ToList<SufferStateProperty.SufferType>();
	}

	public bool FindSufferState(SufferStateProperty.SufferType type)
	{
		return this.sufferStatePropertyDictionary[type].isActive;
	}

	public void RemoveSufferState(SufferStateProperty.SufferType type)
	{
		this.sufferStatePropertyDictionary[type].SetNull();
	}

	public void SetSufferState(SufferStateProperty suffer, CharacterStateControl status = null)
	{
		if (!suffer.isActive)
		{
			return;
		}
		this.sufferStatePropertyDictionary[suffer.sufferTypeCache] = suffer;
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
					sufferStateProperty.SetNull();
				}
			}
		}
	}

	private string CheckNull(SufferStateProperty sufferStateProperty)
	{
		if (sufferStateProperty != null)
		{
			return sufferStateProperty.GetParams();
		}
		return string.Empty;
	}

	public HaveSufferStateStore GetCurrentSufferState()
	{
		HaveSufferStateStore haveSufferStateStore = new HaveSufferStateStore();
		List<HaveSufferStateStore.Data> list = new List<HaveSufferStateStore.Data>();
		Array values = Enum.GetValues(typeof(SufferStateProperty.SufferType));
		foreach (object obj in values)
		{
			SufferStateProperty.SufferType key = (SufferStateProperty.SufferType)((int)obj);
			if (this.sufferStatePropertyDictionary.ContainsKey(key))
			{
				list.Add(new HaveSufferStateStore.Data
				{
					key = (int)key,
					value = this.sufferStatePropertyDictionary[key].GetParams()
				});
			}
		}
		haveSufferStateStore.sufferStatePropertys = list.ToArray();
		return haveSufferStateStore;
	}

	public void SetCurrentSufferState(HaveSufferStateStore haveSufferStateStore, BattleStateData battleStateData)
	{
		Array values = Enum.GetValues(typeof(SufferStateProperty.SufferType));
		foreach (object obj in values)
		{
			SufferStateProperty.SufferType key = (SufferStateProperty.SufferType)((int)obj);
			HaveSufferStateStore.Data data = haveSufferStateStore.sufferStatePropertys.Where((HaveSufferStateStore.Data item) => item.key == (int)key).SingleOrDefault<HaveSufferStateStore.Data>();
			if (data != null)
			{
				if (!this.sufferStatePropertyDictionary.ContainsKey(key))
				{
					this.sufferStatePropertyDictionary.Add(key, new SufferStateProperty());
				}
				this.sufferStatePropertyDictionary[key].SetParams(data.value, battleStateData);
			}
		}
	}
}
