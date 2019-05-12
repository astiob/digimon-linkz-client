using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityExtension;

[Serializable]
public class SufferStateProperty
{
	public Dictionary<string, List<SufferStateProperty.Data>> dataDictionary = new Dictionary<string, List<SufferStateProperty.Data>>();

	private SufferStateProperty.SufferType sufferType;

	public SufferStateProperty(SufferStateProperty.SufferType typeCache)
	{
		this.sufferType = typeCache;
	}

	public SufferStateProperty(AffectEffectProperty affectProperty, int lastGenerationStartTiming = 0)
	{
		SufferStateProperty.Data data = new SufferStateProperty.Data(affectProperty, lastGenerationStartTiming);
		this.AddSufferStateProperty(data);
	}

	public bool isActive
	{
		get
		{
			foreach (List<SufferStateProperty.Data> source in this.dataDictionary.Values)
			{
				if (source.Where((SufferStateProperty.Data item) => item.isActive).Any<SufferStateProperty.Data>())
				{
					return true;
				}
			}
			return false;
		}
	}

	public SufferStateProperty.SufferType sufferTypeCache
	{
		get
		{
			return this.sufferType;
		}
	}

	public float damagePercent
	{
		get
		{
			float num = 0f;
			foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
			{
				foreach (SufferStateProperty.Data data in list)
				{
					if (data.isActive)
					{
						num += data.damagePercent;
					}
				}
			}
			return num;
		}
	}

	public int upPower
	{
		get
		{
			int num = 0;
			foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
			{
				foreach (SufferStateProperty.Data data in list)
				{
					if (data.isActive)
					{
						num += data.upPower;
					}
				}
			}
			return num;
		}
	}

	public int downPower
	{
		get
		{
			int num = 0;
			foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
			{
				foreach (SufferStateProperty.Data data in list)
				{
					if (data.isActive)
					{
						num += data.downPower;
					}
				}
			}
			return num;
		}
	}

	public float upPercent
	{
		get
		{
			float num = 0f;
			foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
			{
				float num2 = 0f;
				foreach (SufferStateProperty.Data data in list)
				{
					if (data.isActive)
					{
						num2 += data.upPercent + (float)(data.keepRoundNumber - data.currentKeepRound) * data.turnRate;
						num2 = Mathf.Min(num2, data.maxValue);
					}
				}
				num += num2;
			}
			return num;
		}
	}

	public float downPercent
	{
		get
		{
			float num = 0f;
			foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
			{
				float num2 = 0f;
				foreach (SufferStateProperty.Data data in list)
				{
					if (data.isActive)
					{
						num2 += data.downPercent + (float)(data.keepRoundNumber - data.currentKeepRound) * data.turnRate;
						num2 = Mathf.Min(num2, data.maxValue);
					}
				}
				num += num2;
			}
			return num;
		}
	}

	public float physicUpPercent
	{
		get
		{
			float num = 0f;
			foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
			{
				foreach (SufferStateProperty.Data data in list)
				{
					if (data.isActive)
					{
						num += data.physicUpPercent;
					}
				}
			}
			return num;
		}
	}

	public float specialUpPercent
	{
		get
		{
			float num = 0f;
			foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
			{
				foreach (SufferStateProperty.Data data in list)
				{
					if (data.isActive)
					{
						num += data.specialUpPercent;
					}
				}
			}
			return num;
		}
	}

	public float recieveDamageRate
	{
		get
		{
			float num = 0f;
			foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
			{
				foreach (SufferStateProperty.Data data in list)
				{
					if (data.isActive)
					{
						num += data.recieveDamageRate;
					}
				}
			}
			return num;
		}
	}

	public SufferStateProperty.Data[] GetIsMultiHitThroughDatas()
	{
		List<SufferStateProperty.Data> list = new List<SufferStateProperty.Data>();
		foreach (List<SufferStateProperty.Data> list2 in this.dataDictionary.Values)
		{
			foreach (SufferStateProperty.Data data in list2)
			{
				if (data.isActive && data.isMultiHitThrough)
				{
					list.Add(data);
				}
			}
		}
		return list.ToArray();
	}

	public SufferStateProperty.Data[] GetNotIsMultiHitThroughDatas()
	{
		List<SufferStateProperty.Data> list = new List<SufferStateProperty.Data>();
		foreach (List<SufferStateProperty.Data> list2 in this.dataDictionary.Values)
		{
			foreach (SufferStateProperty.Data data in list2)
			{
				if (data.isActive && !data.isMultiHitThrough)
				{
					list.Add(data);
				}
			}
		}
		return list.ToArray();
	}

	public int generationStartTiming
	{
		get
		{
			int num = 0;
			foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
			{
				foreach (SufferStateProperty.Data data in list)
				{
					if (data.isActive)
					{
						num = Mathf.Max(num, data.generationStartTiming);
					}
				}
			}
			return num;
		}
	}

	public bool isTurnRate
	{
		get
		{
			foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
			{
				foreach (SufferStateProperty.Data data in list)
				{
					if (data.isActive && data.turnRate > 0f)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public bool OnInvocationPowerChargeAttack
	{
		get
		{
			foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
			{
				foreach (SufferStateProperty.Data data in list)
				{
					if (data.isActive && data.currentKeepRound < 1)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public bool GetOccurrenceFreeze()
	{
		foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
		{
			foreach (SufferStateProperty.Data data in list)
			{
				if (data.isActive && RandomExtension.Switch(data.incidenceRate))
				{
					return true;
				}
			}
		}
		return false;
	}

	public int GetPoisonDamageFluctuation(CharacterStateControl status)
	{
		int num = 0;
		foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
		{
			int num2 = 0;
			float num3 = 0f;
			foreach (SufferStateProperty.Data data in list)
			{
				if (data.isActive)
				{
					if (data.powerType == PowerType.Fixable)
					{
						num2 += data.damagePower;
					}
					else
					{
						num3 += data.damagePercent + (float)(data.keepRoundNumber - data.currentKeepRound) * data.turnRate;
						num3 = Mathf.Min(num3, data.maxValue);
						num2 += Mathf.FloorToInt((float)status.extraMaxHp * num3);
					}
				}
			}
			num += num2;
		}
		return num;
	}

	public int GetReflectDamage(int damage)
	{
		int num = 0;
		foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
		{
			foreach (SufferStateProperty.Data data in list)
			{
				if (data.isActive)
				{
					num += Mathf.FloorToInt((float)damage * this.damagePercent);
				}
			}
		}
		return num;
	}

	public int GetRecieveReflectDamage(int damage)
	{
		int num = 0;
		foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
		{
			foreach (SufferStateProperty.Data data in list)
			{
				if (data.isActive)
				{
					num += Mathf.FloorToInt((float)damage * data.recieveDamageRate);
				}
			}
		}
		return num;
	}

	public bool GetSleepGetupOccurrence()
	{
		foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
		{
			foreach (SufferStateProperty.Data data in list)
			{
				if (data.isActive && RandomExtension.Switch(data.selfGetupIncidenceRate))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool GetSleepGetupOccurrenceDamage()
	{
		foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
		{
			foreach (SufferStateProperty.Data data in list)
			{
				if (data.isActive && RandomExtension.Switch(data.damageGetupIncidenceRate))
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool GetEscapeResult()
	{
		foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
		{
			foreach (SufferStateProperty.Data data in list)
			{
				if (data.isActive && RandomExtension.Switch(data.escapeRate))
				{
					return true;
				}
			}
		}
		return false;
	}

	public void SaveAheadEscapeResult()
	{
		foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
		{
			foreach (SufferStateProperty.Data data in list)
			{
				data.isEscape = this.GetEscapeResult();
			}
		}
	}

	public bool GetAheadEscapeResult()
	{
		foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
		{
			foreach (SufferStateProperty.Data data in list)
			{
				if (data.isEscape)
				{
					return data.isEscape;
				}
			}
		}
		return false;
	}

	public int GetRevivalAp(int maxAp)
	{
		int num = 0;
		foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
		{
			foreach (SufferStateProperty.Data data in list)
			{
				if (data.isActive)
				{
					if (data.powerType == PowerType.Fixable)
					{
						num += data.revivalPower;
					}
					else
					{
						num += Mathf.FloorToInt((float)maxAp * data.revivalPercent);
					}
				}
			}
		}
		return num;
	}

	public int GetRegenerate(CharacterStateControl status)
	{
		int num = 0;
		foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
		{
			int num2 = 0;
			float num3 = 0f;
			foreach (SufferStateProperty.Data data in list)
			{
				if (data.isActive)
				{
					if (data.powerType == PowerType.Fixable)
					{
						num2 = data.revivalPower;
					}
					else
					{
						num3 += data.revivalPercent + (float)(data.keepRoundNumber - data.currentKeepRound) * data.turnRate;
						num3 = Mathf.Min(num3, data.maxValue);
						num2 = Mathf.FloorToInt((float)status.extraMaxHp * num3);
					}
				}
			}
			num += num2;
		}
		return num;
	}

	public void SetNull()
	{
		foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
		{
			foreach (SufferStateProperty.Data data in list)
			{
				if (!data.isParmanent)
				{
					data.isActive = false;
					data.currentKeepRound = -1;
				}
			}
		}
	}

	public int GetNearCurrentKeepRound()
	{
		int num = int.MaxValue;
		foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
		{
			foreach (SufferStateProperty.Data data in list)
			{
				if (data.isActive)
				{
					num = Mathf.Min(num, data.currentKeepRound);
				}
			}
		}
		return num;
	}

	public void AddCurrentKeepRound(int value)
	{
		foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
		{
			foreach (SufferStateProperty.Data data in list)
			{
				if (data.isActive && !data.isParmanent)
				{
					data.currentKeepRound += value;
					if (data.currentKeepRound < 0)
					{
						data.isActive = false;
						data.currentKeepRound = -1;
					}
				}
			}
		}
	}

	public void AddCurrentKeepCount(int value)
	{
		foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
		{
			this.AddCurrentKeepCount(list.ToArray(), value);
		}
	}

	public void AddCurrentKeepCount(SufferStateProperty.Data[] datas, int value)
	{
		foreach (SufferStateProperty.Data data in datas)
		{
			if (data.isActive && !data.isParmanent)
			{
				data.currentKeepRound += value;
				if (data.currentKeepRound <= 0)
				{
					data.isActive = false;
					data.currentKeepRound = -1;
				}
			}
		}
	}

	public void AddSufferStateProperty(SufferStateProperty.Data data)
	{
		if (data.setType == SufferStateProperty.SetType.Override)
		{
			if (this.dataDictionary.ContainsKey(data.id))
			{
				List<SufferStateProperty.Data> list = this.dataDictionary[data.id];
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].setType == SufferStateProperty.SetType.Override)
					{
						list[i] = data;
						break;
					}
				}
			}
			else
			{
				List<SufferStateProperty.Data> list2 = new List<SufferStateProperty.Data>();
				list2.Add(data);
				this.dataDictionary.Add(data.id, list2);
			}
		}
		else if (this.dataDictionary.ContainsKey(data.id))
		{
			this.dataDictionary[data.id].Add(data);
		}
		else
		{
			List<SufferStateProperty.Data> list3 = new List<SufferStateProperty.Data>();
			list3.Add(data);
			this.dataDictionary.Add(data.id, list3);
		}
		foreach (List<SufferStateProperty.Data> list4 in this.dataDictionary.Values)
		{
			list4.RemoveAll((SufferStateProperty.Data item) => !item.isActive);
		}
	}

	public SufferStateProperty.DamageRateResult GetCaseDamageRate(AffectEffectProperty affectEffectProperty, CharacterStateControl characterStateControl)
	{
		SufferStateProperty.DamageRateResult damageRateResult = new SufferStateProperty.DamageRateResult();
		bool flag = affectEffectProperty.skillId.ToString() == BattleStateManager.PublicAttackSkillId;
		foreach (List<SufferStateProperty.Data> list in this.dataDictionary.Values)
		{
			foreach (SufferStateProperty.Data data in list)
			{
				if (data.isActive)
				{
					if (data.recieveSkillType == 0 || (data.recieveSkillType == 1 && flag) || (data.recieveSkillType == 2 && !flag))
					{
						bool flag2 = data.recieveSkillTargetSubType == 1;
						bool flag3 = data.recieveSkillTargetSubType == 2;
						float[] array = new float[]
						{
							data.damagePercent,
							data.damageRateForPhantomStudents,
							data.damageRateForHeatHaze,
							data.damageRateForGlacier,
							data.damageRateForElectromagnetic,
							data.damageRateForEarth,
							data.damageRateForShaftOfLight,
							data.damageRateForAbyss
						};
						int num = 0;
						if (flag2)
						{
							num = (int)(affectEffectProperty.attribute + 1);
						}
						else if (flag3)
						{
							num = ((!(characterStateControl != null)) ? 0 : characterStateControl.characterDatas.tribe.ToInt32());
						}
						damageRateResult.damageRate += array[num];
						if (array[num] != 0f)
						{
							damageRateResult.dataList.Add(data);
						}
					}
				}
			}
		}
		return damageRateResult;
	}

	public HaveSufferStateStore.DataChild[] Get()
	{
		List<HaveSufferStateStore.DataChild> list = new List<HaveSufferStateStore.DataChild>();
		foreach (KeyValuePair<string, List<SufferStateProperty.Data>> keyValuePair in this.dataDictionary)
		{
			list.Add(new HaveSufferStateStore.DataChild
			{
				key = keyValuePair.Key,
				values = keyValuePair.Value.ToArray()
			});
		}
		return list.ToArray();
	}

	public void Set(HaveSufferStateStore.DataChild[] dataChildren)
	{
		this.dataDictionary.Clear();
		foreach (HaveSufferStateStore.DataChild dataChild in dataChildren)
		{
			if (this.dataDictionary.ContainsKey(dataChild.key))
			{
				this.dataDictionary[dataChild.key] = new List<SufferStateProperty.Data>(dataChild.values);
			}
			else
			{
				this.dataDictionary.Add(dataChild.key, new List<SufferStateProperty.Data>(dataChild.values));
			}
		}
	}

	[Serializable]
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
		HitRateUp,
		HitRateDown,
		SatisfactionRateUp,
		SatisfactionRateDown,
		Counter,
		Reflection,
		Protect,
		PowerCharge,
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
		CountEvasion,
		Escape
	}

	[Serializable]
	public enum SetType
	{
		Override,
		Add
	}

	[Serializable]
	public class Data
	{
		public string id = string.Empty;

		public bool isActive;

		public SufferStateProperty.SufferType sufferType;

		public PowerType powerType;

		public int damagePower;

		public float damagePercent;

		public int upPower;

		public int downPower;

		public int revivalPower;

		public float upPercent;

		public float downPercent;

		public float revivalPercent;

		public float incidenceRate;

		public float physicUpPercent;

		public float specialUpPercent;

		public float damageGetupIncidenceRate;

		public float selfGetupIncidenceRate;

		public float recieveDamageRate;

		public int keepRoundNumber = -1;

		public int currentKeepRound = -1;

		public bool isMultiHitThrough;

		public int generationStartTiming;

		public float damageRateForPhantomStudents;

		public float damageRateForHeatHaze;

		public float damageRateForGlacier;

		public float damageRateForElectromagnetic;

		public float damageRateForEarth;

		public float damageRateForShaftOfLight;

		public float damageRateForAbyss;

		public float turnRate;

		public float maxValue;

		public SufferStateProperty.SetType setType;

		public int recieveSkillType;

		public int recieveSkillTargetSubType;

		public float escapeRate;

		public bool isEscape;

		public Data()
		{
		}

		public Data(AffectEffectProperty affectProperty, int lastGenerationStartTiming = 0)
		{
			if (affectProperty.sufferSetType == SufferStateProperty.SetType.Override)
			{
				this.id = "Override";
			}
			else
			{
				this.id = SufferStateProperty.Data.CreateId(affectProperty.skillId, affectProperty.subSkillId);
			}
			this.isActive = true;
			this.generationStartTiming = lastGenerationStartTiming;
			switch (affectProperty.type)
			{
			case AffectEffect.AttackUp:
				this.sufferType = SufferStateProperty.SufferType.AttackUp;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.upPercent = affectProperty.upPercent;
				this.turnRate = affectProperty.turnRate;
				this.maxValue = affectProperty.maxValue;
				this.setType = affectProperty.sufferSetType;
				break;
			case AffectEffect.AttackDown:
				this.sufferType = SufferStateProperty.SufferType.AttackDown;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.downPercent = affectProperty.downPercent;
				this.turnRate = affectProperty.turnRate;
				this.maxValue = affectProperty.maxValue;
				this.setType = affectProperty.sufferSetType;
				break;
			case AffectEffect.DefenceUp:
				this.sufferType = SufferStateProperty.SufferType.DefenceUp;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.upPercent = affectProperty.upPercent;
				this.turnRate = affectProperty.turnRate;
				this.maxValue = affectProperty.maxValue;
				this.setType = affectProperty.sufferSetType;
				break;
			case AffectEffect.DefenceDown:
				this.sufferType = SufferStateProperty.SufferType.DefenceDown;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.downPercent = affectProperty.downPercent;
				this.turnRate = affectProperty.turnRate;
				this.maxValue = affectProperty.maxValue;
				this.setType = affectProperty.sufferSetType;
				break;
			case AffectEffect.SpAttackUp:
				this.sufferType = SufferStateProperty.SufferType.SpAttackUp;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.upPercent = affectProperty.upPercent;
				this.turnRate = affectProperty.turnRate;
				this.maxValue = affectProperty.maxValue;
				this.setType = affectProperty.sufferSetType;
				break;
			case AffectEffect.SpAttackDown:
				this.sufferType = SufferStateProperty.SufferType.SpAttackDown;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.downPercent = affectProperty.downPercent;
				this.turnRate = affectProperty.turnRate;
				this.maxValue = affectProperty.maxValue;
				this.setType = affectProperty.sufferSetType;
				break;
			case AffectEffect.SpDefenceUp:
				this.sufferType = SufferStateProperty.SufferType.SpDefenceUp;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.upPercent = affectProperty.upPercent;
				this.turnRate = affectProperty.turnRate;
				this.maxValue = affectProperty.maxValue;
				this.setType = affectProperty.sufferSetType;
				break;
			case AffectEffect.SpDefenceDown:
				this.sufferType = SufferStateProperty.SufferType.SpDefenceDown;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.downPercent = affectProperty.downPercent;
				this.turnRate = affectProperty.turnRate;
				this.maxValue = affectProperty.maxValue;
				this.setType = affectProperty.sufferSetType;
				break;
			case AffectEffect.SpeedUp:
				this.sufferType = SufferStateProperty.SufferType.SpeedUp;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.upPercent = affectProperty.upPercent;
				this.turnRate = affectProperty.turnRate;
				this.maxValue = affectProperty.maxValue;
				this.setType = affectProperty.sufferSetType;
				break;
			case AffectEffect.SpeedDown:
				this.sufferType = SufferStateProperty.SufferType.SpeedDown;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.downPercent = affectProperty.downPercent;
				this.turnRate = affectProperty.turnRate;
				this.maxValue = affectProperty.maxValue;
				this.setType = affectProperty.sufferSetType;
				break;
			case AffectEffect.Counter:
				this.sufferType = SufferStateProperty.SufferType.Counter;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.damagePercent = affectProperty.damagePercent;
				this.recieveDamageRate = affectProperty.recieveDamageRate;
				break;
			case AffectEffect.Reflection:
				this.sufferType = SufferStateProperty.SufferType.Reflection;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.damagePercent = affectProperty.damagePercent;
				this.recieveDamageRate = affectProperty.recieveDamageRate;
				break;
			case AffectEffect.Protect:
				this.sufferType = SufferStateProperty.SufferType.Protect;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				break;
			case AffectEffect.PowerCharge:
				this.sufferType = SufferStateProperty.SufferType.PowerCharge;
				this.keepRoundNumber = affectProperty.chargeRoundNumber;
				this.currentKeepRound = affectProperty.chargeRoundNumber;
				this.physicUpPercent = affectProperty.physicUpPercent;
				this.specialUpPercent = affectProperty.specialUpPercent;
				break;
			case AffectEffect.Paralysis:
				this.sufferType = SufferStateProperty.SufferType.Paralysis;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.incidenceRate = affectProperty.incidenceRate;
				break;
			case AffectEffect.Poison:
				this.sufferType = SufferStateProperty.SufferType.Poison;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.powerType = affectProperty.powerType;
				this.damagePower = affectProperty.damagePower;
				this.damagePercent = affectProperty.damagePercent;
				this.turnRate = affectProperty.turnRate;
				this.maxValue = affectProperty.maxValue;
				break;
			case AffectEffect.Sleep:
				this.sufferType = SufferStateProperty.SufferType.Sleep;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.incidenceRate = affectProperty.incidenceRate;
				this.damageGetupIncidenceRate = affectProperty.damageGetupIncidenceRate;
				this.selfGetupIncidenceRate = affectProperty.selfGetupIncidenceRate;
				break;
			case AffectEffect.SkillLock:
				this.sufferType = SufferStateProperty.SufferType.SkillLock;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				break;
			case AffectEffect.HitRateUp:
				this.sufferType = SufferStateProperty.SufferType.HitRateUp;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.upPercent = affectProperty.upPercent;
				this.turnRate = affectProperty.turnRate;
				this.maxValue = affectProperty.maxValue;
				this.setType = affectProperty.sufferSetType;
				break;
			case AffectEffect.HitRateDown:
				this.sufferType = SufferStateProperty.SufferType.HitRateDown;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.downPercent = affectProperty.downPercent;
				this.turnRate = affectProperty.turnRate;
				this.maxValue = affectProperty.maxValue;
				this.setType = affectProperty.sufferSetType;
				break;
			case AffectEffect.Confusion:
				this.sufferType = SufferStateProperty.SufferType.Confusion;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.incidenceRate = affectProperty.incidenceRate;
				break;
			case AffectEffect.Stun:
				this.sufferType = SufferStateProperty.SufferType.Stun;
				this.keepRoundNumber = 1;
				this.currentKeepRound = 1;
				break;
			case AffectEffect.SatisfactionRateUp:
				this.sufferType = SufferStateProperty.SufferType.SatisfactionRateUp;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.upPercent = affectProperty.upPercent;
				this.turnRate = affectProperty.turnRate;
				this.maxValue = affectProperty.maxValue;
				this.setType = affectProperty.sufferSetType;
				break;
			case AffectEffect.SatisfactionRateDown:
				this.sufferType = SufferStateProperty.SufferType.SatisfactionRateDown;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.downPercent = affectProperty.downPercent;
				this.turnRate = affectProperty.turnRate;
				this.maxValue = affectProperty.maxValue;
				this.setType = affectProperty.sufferSetType;
				break;
			case AffectEffect.ApRevival:
				this.sufferType = SufferStateProperty.SufferType.ApRevival;
				this.powerType = affectProperty.powerType;
				this.revivalPower = affectProperty.revivalPower;
				this.revivalPercent = affectProperty.revivalPercent;
				this.currentKeepRound = 0;
				break;
			case AffectEffect.ApConsumptionUp:
				this.sufferType = SufferStateProperty.SufferType.ApConsumptionUp;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.upPower = affectProperty.upPower;
				break;
			case AffectEffect.ApConsumptionDown:
				this.sufferType = SufferStateProperty.SufferType.ApConsumptionDown;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.downPower = affectProperty.downPower;
				break;
			case AffectEffect.CountGuard:
				this.sufferType = SufferStateProperty.SufferType.CountGuard;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.damagePercent = affectProperty.damagePercent;
				this.isMultiHitThrough = affectProperty.isMultiHitThrough;
				this.damageRateForPhantomStudents = affectProperty.damageRateForPhantomStudents;
				this.damageRateForHeatHaze = affectProperty.damageRateForHeatHaze;
				this.damageRateForGlacier = affectProperty.damageRateForGlacier;
				this.damageRateForElectromagnetic = affectProperty.damageRateForElectromagnetic;
				this.damageRateForEarth = affectProperty.damageRateForEarth;
				this.damageRateForShaftOfLight = affectProperty.damageRateForShaftOfLight;
				this.damageRateForAbyss = affectProperty.damageRateForAbyss;
				this.recieveSkillType = affectProperty.recieveSkillType;
				this.recieveSkillTargetSubType = affectProperty.recieveSkillTargetSubType;
				this.setType = affectProperty.sufferSetType;
				break;
			case AffectEffect.TurnBarrier:
				this.sufferType = SufferStateProperty.SufferType.TurnBarrier;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				break;
			case AffectEffect.CountBarrier:
				this.sufferType = SufferStateProperty.SufferType.CountBarrier;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.isMultiHitThrough = affectProperty.isMultiHitThrough;
				break;
			case AffectEffect.DamageRateUp:
				this.sufferType = SufferStateProperty.SufferType.DamageRateUp;
				this.keepRoundNumber = affectProperty.damageRateKeepRoundNumber;
				this.currentKeepRound = affectProperty.damageRateKeepRoundNumber;
				this.damagePercent = affectProperty.damagePercent;
				this.damageRateForPhantomStudents = affectProperty.damageRateForPhantomStudents;
				this.damageRateForHeatHaze = affectProperty.damageRateForHeatHaze;
				this.damageRateForGlacier = affectProperty.damageRateForGlacier;
				this.damageRateForElectromagnetic = affectProperty.damageRateForElectromagnetic;
				this.damageRateForEarth = affectProperty.damageRateForEarth;
				this.damageRateForShaftOfLight = affectProperty.damageRateForShaftOfLight;
				this.damageRateForAbyss = affectProperty.damageRateForAbyss;
				this.recieveSkillType = affectProperty.recieveSkillType;
				this.recieveSkillTargetSubType = affectProperty.recieveSkillTargetSubType;
				this.setType = affectProperty.sufferSetType;
				break;
			case AffectEffect.DamageRateDown:
				this.sufferType = SufferStateProperty.SufferType.DamageRateDown;
				this.keepRoundNumber = affectProperty.damageRateKeepRoundNumber;
				this.currentKeepRound = affectProperty.damageRateKeepRoundNumber;
				this.damagePercent = affectProperty.damagePercent;
				this.damageRateForPhantomStudents = affectProperty.damageRateForPhantomStudents;
				this.damageRateForHeatHaze = affectProperty.damageRateForHeatHaze;
				this.damageRateForGlacier = affectProperty.damageRateForGlacier;
				this.damageRateForElectromagnetic = affectProperty.damageRateForElectromagnetic;
				this.damageRateForEarth = affectProperty.damageRateForEarth;
				this.damageRateForShaftOfLight = affectProperty.damageRateForShaftOfLight;
				this.damageRateForAbyss = affectProperty.damageRateForAbyss;
				this.recieveSkillType = affectProperty.recieveSkillType;
				this.recieveSkillTargetSubType = affectProperty.recieveSkillTargetSubType;
				this.setType = affectProperty.sufferSetType;
				break;
			case AffectEffect.Regenerate:
				this.sufferType = SufferStateProperty.SufferType.Regenerate;
				this.powerType = affectProperty.powerType;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.revivalPower = affectProperty.revivalPower;
				this.revivalPercent = affectProperty.revivalPercent;
				this.turnRate = affectProperty.turnRate;
				this.maxValue = affectProperty.maxValue;
				break;
			case AffectEffect.TurnEvasion:
				this.sufferType = SufferStateProperty.SufferType.TurnEvasion;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				break;
			case AffectEffect.CountEvasion:
				this.sufferType = SufferStateProperty.SufferType.CountEvasion;
				this.keepRoundNumber = affectProperty.keepRoundNumber;
				this.currentKeepRound = affectProperty.keepRoundNumber;
				this.isMultiHitThrough = affectProperty.isMultiHitThrough;
				break;
			case AffectEffect.Escape:
				this.sufferType = SufferStateProperty.SufferType.Escape;
				this.keepRoundNumber = affectProperty.escapeRound;
				this.currentKeepRound = affectProperty.escapeRound;
				this.escapeRate = affectProperty.escapeRate;
				break;
			}
		}

		public bool isParmanent
		{
			get
			{
				return this.keepRoundNumber >= 100 || this.currentKeepRound >= 100;
			}
		}

		public static string CreateId(int skillId, int subSkillId)
		{
			return skillId + "_" + subSkillId;
		}
	}

	public class DamageRateResult
	{
		public float damageRate;

		public List<SufferStateProperty.Data> dataList = new List<SufferStateProperty.Data>();
	}
}
