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

	public void SetSufferState(SufferStateProperty.Data data, CharacterStateControl status = null)
	{
		SufferStateProperty sufferStateProperty = this.sufferStatePropertyDictionary[data.sufferType];
		sufferStateProperty.AddSufferStateProperty(data);
	}

	public void RoundCount()
	{
		SufferStateProperty.SufferType[] array = new SufferStateProperty.SufferType[]
		{
			SufferStateProperty.SufferType.Poison,
			SufferStateProperty.SufferType.Confusion,
			SufferStateProperty.SufferType.Paralysis,
			SufferStateProperty.SufferType.Sleep,
			SufferStateProperty.SufferType.Stun,
			SufferStateProperty.SufferType.SkillLock,
			SufferStateProperty.SufferType.AttackUp,
			SufferStateProperty.SufferType.AttackDown,
			SufferStateProperty.SufferType.DefenceUp,
			SufferStateProperty.SufferType.DefenceDown,
			SufferStateProperty.SufferType.SpAttackUp,
			SufferStateProperty.SufferType.SpAttackDown,
			SufferStateProperty.SufferType.SpDefenceUp,
			SufferStateProperty.SufferType.SpDefenceDown,
			SufferStateProperty.SufferType.SpeedUp,
			SufferStateProperty.SufferType.SpeedDown,
			SufferStateProperty.SufferType.Counter,
			SufferStateProperty.SufferType.Reflection,
			SufferStateProperty.SufferType.Protect,
			SufferStateProperty.SufferType.PowerCharge,
			SufferStateProperty.SufferType.HitRateUp,
			SufferStateProperty.SufferType.HitRateDown,
			SufferStateProperty.SufferType.SatisfactionRateUp,
			SufferStateProperty.SufferType.SatisfactionRateDown,
			SufferStateProperty.SufferType.ApRevival,
			SufferStateProperty.SufferType.ApConsumptionUp,
			SufferStateProperty.SufferType.ApConsumptionDown,
			SufferStateProperty.SufferType.TurnBarrier,
			SufferStateProperty.SufferType.DamageRateUp,
			SufferStateProperty.SufferType.DamageRateDown,
			SufferStateProperty.SufferType.Regenerate,
			SufferStateProperty.SufferType.TurnEvasion
		};
		foreach (SufferStateProperty.SufferType key in array)
		{
			this.sufferStatePropertyDictionary[key].AddCurrentKeepRound(-1);
		}
	}

	public HaveSufferStateStore Get()
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
					key = key,
					values = this.sufferStatePropertyDictionary[key].Get()
				});
			}
		}
		haveSufferStateStore.sufferStatePropertys = list.ToArray();
		return haveSufferStateStore;
	}

	public void Set(HaveSufferStateStore haveSufferStateStore)
	{
		Array values = Enum.GetValues(typeof(SufferStateProperty.SufferType));
		foreach (object obj in values)
		{
			SufferStateProperty.SufferType key = (SufferStateProperty.SufferType)((int)obj);
			HaveSufferStateStore.Data data = haveSufferStateStore.sufferStatePropertys.Where((HaveSufferStateStore.Data item) => item.key == key).SingleOrDefault<HaveSufferStateStore.Data>();
			if (data != null)
			{
				if (!this.sufferStatePropertyDictionary.ContainsKey(key))
				{
					this.sufferStatePropertyDictionary.Add(key, new SufferStateProperty(key));
				}
				this.sufferStatePropertyDictionary[key].Set(data.values);
			}
		}
	}
}
