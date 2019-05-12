using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ExtraEffectStatus
{
	[SerializeField]
	private ExtraEffectStatus.ExtraEffectType _extraEffectType;

	[SerializeField]
	private int _targetType;

	[SerializeField]
	private int _targetSubType;

	[SerializeField]
	private int _targetValue;

	[SerializeField]
	private string _targetValue2;

	[SerializeField]
	private int _effectType;

	[SerializeField]
	private int _effectSubType;

	[SerializeField]
	private float _effectValue;

	[SerializeField]
	private int _effectTrigger;

	[SerializeField]
	private string _effectTriggerValue;

	public ExtraEffectStatus()
	{
	}

	public ExtraEffectStatus(GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM worldDungeonExtraEffectM)
	{
		this._targetType = worldDungeonExtraEffectM.targetType.ToInt32();
		this._targetSubType = worldDungeonExtraEffectM.targetSubType.ToInt32();
		this._targetValue = worldDungeonExtraEffectM.targetValue.ToInt32();
		this._targetValue2 = worldDungeonExtraEffectM.targetValue2;
		this._effectType = worldDungeonExtraEffectM.effectType.ToInt32();
		this._effectSubType = worldDungeonExtraEffectM.effectSubType.ToInt32();
		this._effectValue = worldDungeonExtraEffectM.effectValue.ToFloat();
		this._effectTrigger = worldDungeonExtraEffectM.effectTrigger.ToInt32();
		this._effectTriggerValue = worldDungeonExtraEffectM.effectTriggerValue;
	}

	public string name
	{
		get
		{
			return this._extraEffectType.ToString();
		}
	}

	public int TargetType
	{
		get
		{
			return this._targetType;
		}
	}

	public int TargetSubType
	{
		get
		{
			return this._targetSubType;
		}
	}

	public int TargetValue
	{
		get
		{
			return this._targetValue;
		}
	}

	public string TargetValue2
	{
		get
		{
			return this._targetValue2;
		}
	}

	public int EffectType
	{
		get
		{
			return this._effectType;
		}
	}

	public int EffectSubType
	{
		get
		{
			return this._effectSubType;
		}
	}

	public float EffectValue
	{
		get
		{
			return this._effectValue;
		}
	}

	public int EffectTrigger
	{
		get
		{
			return this._effectTrigger;
		}
	}

	public string EffectTriggerValue
	{
		get
		{
			return this._effectTriggerValue;
		}
	}

	public static List<ExtraEffectStatus> GetInvocationList(List<ExtraEffectStatus> extraEffectStatuses, ChipEffectStatus.EffectTriggerType effectTriggerType)
	{
		List<ExtraEffectStatus> list = new List<ExtraEffectStatus>();
		foreach (ExtraEffectStatus extraEffectStatus in extraEffectStatuses)
		{
			ChipEffectStatus.EffectTriggerType effectTrigger = (ChipEffectStatus.EffectTriggerType)extraEffectStatus.EffectTrigger;
			if (effectTrigger == effectTriggerType)
			{
				list.Add(extraEffectStatus);
			}
		}
		List<ExtraEffectStatus> list2 = new List<ExtraEffectStatus>();
		if (effectTriggerType != ChipEffectStatus.EffectTriggerType.WaveStarted)
		{
			if (effectTriggerType != ChipEffectStatus.EffectTriggerType.RoundEnd)
			{
				list2.AddRange(list);
			}
			else
			{
				List<ExtraEffectStatus> loopPlayExtraEffectStatusList = ExtraEffectStatus.GetLoopPlayExtraEffectStatusList(list, BattleStateManager.current.battleStateData.currentRoundNumber);
				list2.AddRange(loopPlayExtraEffectStatusList);
			}
		}
		else
		{
			int targetNumber = BattleStateManager.current.battleStateData.currentWaveNumber + 1;
			List<ExtraEffectStatus> loopPlayExtraEffectStatusList2 = ExtraEffectStatus.GetLoopPlayExtraEffectStatusList(list, targetNumber);
			list2.AddRange(loopPlayExtraEffectStatusList2);
		}
		return list2;
	}

	private static List<ExtraEffectStatus> GetLoopPlayExtraEffectStatusList(List<ExtraEffectStatus> list, int targetNumber)
	{
		List<ExtraEffectStatus> list2 = new List<ExtraEffectStatus>();
		foreach (ExtraEffectStatus extraEffectStatus in list)
		{
			if (string.IsNullOrEmpty(extraEffectStatus.EffectTriggerValue) || extraEffectStatus.EffectTriggerValue == "0")
			{
				list2.Add(extraEffectStatus);
			}
			else
			{
				string[] array = extraEffectStatus.EffectTriggerValue.Split(new char[]
				{
					','
				});
				if (array.Length == 1)
				{
					if (array[0].ToInt32() == targetNumber)
					{
						list2.Add(extraEffectStatus);
					}
				}
				else
				{
					int num = array[1].ToInt32();
					int num2 = targetNumber % num;
					if (num2 == 0)
					{
						num2 = num;
					}
					if (array[0].ToInt32() == num2)
					{
						list2.Add(extraEffectStatus);
					}
				}
			}
		}
		return list2;
	}

	public static List<ExtraEffectStatus> GetExtraEffectStatusList(List<ExtraEffectStatus> extraEffectStatusList, ExtraEffectStatus.ExtraTargetType targetType, ExtraEffectStatus.ExtraTargetSubType targetSubType, int targetValue, ConstValue.ResistanceType resistanceType, ExtraEffectStatus.ExtraEffectType effectType)
	{
		ExtraEffectStatus.<GetExtraEffectStatusList>c__AnonStorey2DC <GetExtraEffectStatusList>c__AnonStorey2DC = new ExtraEffectStatus.<GetExtraEffectStatusList>c__AnonStorey2DC();
		<GetExtraEffectStatusList>c__AnonStorey2DC.effectType = effectType;
		<GetExtraEffectStatusList>c__AnonStorey2DC.targetType = targetType;
		<GetExtraEffectStatusList>c__AnonStorey2DC.targetSubType = targetSubType;
		<GetExtraEffectStatusList>c__AnonStorey2DC.targetValue = targetValue;
		List<ExtraEffectStatus> list = new List<ExtraEffectStatus>();
		if (extraEffectStatusList.Count == 0)
		{
			return list;
		}
		<GetExtraEffectStatusList>c__AnonStorey2DC.searchEffectType = ((int x) => x == (int)<GetExtraEffectStatusList>c__AnonStorey2DC.effectType);
		if (ExtraEffectStatus.ExtraEffectType.Atk <= <GetExtraEffectStatusList>c__AnonStorey2DC.effectType && <GetExtraEffectStatusList>c__AnonStorey2DC.effectType < ExtraEffectStatus.ExtraEffectType.AllStatus)
		{
			<GetExtraEffectStatusList>c__AnonStorey2DC.searchEffectType = ((int x) => x == (int)<GetExtraEffectStatusList>c__AnonStorey2DC.effectType || x == 27);
		}
		IEnumerable<ExtraEffectStatus> enumerable;
		if (resistanceType == ConstValue.ResistanceType.NONE)
		{
			enumerable = extraEffectStatusList.Where((ExtraEffectStatus x) => (x.TargetType == (int)<GetExtraEffectStatusList>c__AnonStorey2DC.targetType || x.TargetType == 0) && x.TargetSubType == (int)<GetExtraEffectStatusList>c__AnonStorey2DC.targetSubType && x.TargetValue == <GetExtraEffectStatusList>c__AnonStorey2DC.targetValue && <GetExtraEffectStatusList>c__AnonStorey2DC.searchEffectType(x.EffectType));
		}
		else
		{
			ExtraEffectStatus.<GetExtraEffectStatusList>c__AnonStorey2DD <GetExtraEffectStatusList>c__AnonStorey2DD = new ExtraEffectStatus.<GetExtraEffectStatusList>c__AnonStorey2DD();
			<GetExtraEffectStatusList>c__AnonStorey2DD.<>f__ref$732 = <GetExtraEffectStatusList>c__AnonStorey2DC;
			ExtraEffectStatus.<GetExtraEffectStatusList>c__AnonStorey2DD <GetExtraEffectStatusList>c__AnonStorey2DD2 = <GetExtraEffectStatusList>c__AnonStorey2DD;
			int num = (int)resistanceType;
			<GetExtraEffectStatusList>c__AnonStorey2DD2.resistance = num.ToString();
			enumerable = extraEffectStatusList.Where((ExtraEffectStatus x) => (x.TargetType == (int)<GetExtraEffectStatusList>c__AnonStorey2DD.<>f__ref$732.targetType || x.TargetType == 0) && x.TargetSubType == (int)<GetExtraEffectStatusList>c__AnonStorey2DD.<>f__ref$732.targetSubType && x.TargetValue == <GetExtraEffectStatusList>c__AnonStorey2DD.<>f__ref$732.targetValue && x.TargetValue2.Contains(<GetExtraEffectStatusList>c__AnonStorey2DD.resistance) && <GetExtraEffectStatusList>c__AnonStorey2DD.<>f__ref$732.searchEffectType(x.EffectType));
		}
		foreach (ExtraEffectStatus item in enumerable)
		{
			list.Add(item);
		}
		return list;
	}

	public static int GetExtraEffectValue(List<ExtraEffectStatus> extraEffectStatusList, int baseValue, CharacterStateControl character, ExtraEffectStatus.ExtraEffectType effectType)
	{
		List<ExtraEffectStatus> totalExtraEffectStatusList = ExtraEffectStatus.GetTotalExtraEffectStatusList(extraEffectStatusList, character, effectType);
		if (totalExtraEffectStatusList.Count > 0)
		{
			return ExtraEffectStatus.GetCorrectionValue(baseValue, totalExtraEffectStatusList);
		}
		return baseValue;
	}

	public static int GetSkillPowerCorrectionValue(List<ExtraEffectStatus> extraEffectStatusList, AffectEffectProperty skillPropety, CharacterStateControl character)
	{
		List<ExtraEffectStatus> list = new List<ExtraEffectStatus>();
		ExtraEffectStatus.ExtraTargetType targetType = ExtraEffectStatus.ExtraTargetType.Player;
		if (character != null && character.isEnemy)
		{
			targetType = ExtraEffectStatus.ExtraTargetType.Enemy;
		}
		if (BattleStateManager.current != null && BattleStateManager.current.battleMode == BattleMode.PvP)
		{
			targetType = ExtraEffectStatus.ExtraTargetType.All;
		}
		ConstValue.ResistanceType skillResistanceType = ExtraEffectStatus.GetSkillResistanceType(skillPropety);
		if (skillResistanceType != ConstValue.ResistanceType.NONE)
		{
			list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.Skill, 0, skillResistanceType, ExtraEffectStatus.ExtraEffectType.SkillDamage));
		}
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.Skill, skillPropety.skillId, ConstValue.ResistanceType.NONE, ExtraEffectStatus.ExtraEffectType.SkillDamage));
		if (character != null)
		{
			List<ConstValue.ResistanceType> attributeStrengthList = character.tolerance.GetAttributeStrengthList();
			foreach (ConstValue.ResistanceType resistanceType in attributeStrengthList)
			{
				list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterResistance, 0, resistanceType, ExtraEffectStatus.ExtraEffectType.SkillDamage));
				list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterResistance, (int)resistanceType, ConstValue.ResistanceType.NONE, ExtraEffectStatus.ExtraEffectType.SkillDamage));
				list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterResistance, (int)resistanceType, resistanceType, ExtraEffectStatus.ExtraEffectType.SkillDamage));
			}
		}
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterTribe, 0, ConstValue.ResistanceType.NONE, ExtraEffectStatus.ExtraEffectType.SkillDamage));
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterTribe, (int)character.species, ConstValue.ResistanceType.NONE, ExtraEffectStatus.ExtraEffectType.SkillDamage));
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterGroup, 0, ConstValue.ResistanceType.NONE, ExtraEffectStatus.ExtraEffectType.SkillDamage));
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterGroup, character.prefabId.ToInt32(), ConstValue.ResistanceType.NONE, ExtraEffectStatus.ExtraEffectType.SkillDamage));
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.GrowStep, 0, ConstValue.ResistanceType.NONE, ExtraEffectStatus.ExtraEffectType.SkillDamage));
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.GrowStep, ExtraEffectStatus.EvolutionStepToMasterId(character.evolutionStep), ConstValue.ResistanceType.NONE, ExtraEffectStatus.ExtraEffectType.SkillDamage));
		if (list.Count > 0)
		{
			return ExtraEffectStatus.GetCorrectionValue(skillPropety.power, list);
		}
		return skillPropety.power;
	}

	public static float GetSkillHitRateCorrectionValue(List<ExtraEffectStatus> extraEffectStatusList, AffectEffectProperty skillPropety, CharacterStateControl character)
	{
		List<ExtraEffectStatus> list = new List<ExtraEffectStatus>();
		ExtraEffectStatus.ExtraTargetType targetType = ExtraEffectStatus.ExtraTargetType.Player;
		if (character != null && character.isEnemy)
		{
			targetType = ExtraEffectStatus.ExtraTargetType.Enemy;
		}
		if (BattleStateManager.current != null && BattleStateManager.current.battleMode == BattleMode.PvP)
		{
			targetType = ExtraEffectStatus.ExtraTargetType.All;
		}
		ConstValue.ResistanceType skillResistanceType = ExtraEffectStatus.GetSkillResistanceType(skillPropety);
		if (skillResistanceType != ConstValue.ResistanceType.NONE)
		{
			list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.Skill, 0, skillResistanceType, ExtraEffectStatus.ExtraEffectType.SkillHit));
		}
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.Skill, skillPropety.skillId, ConstValue.ResistanceType.NONE, ExtraEffectStatus.ExtraEffectType.SkillHit));
		if (character != null)
		{
			List<ConstValue.ResistanceType> attributeStrengthList = character.tolerance.GetAttributeStrengthList();
			foreach (ConstValue.ResistanceType resistanceType in attributeStrengthList)
			{
				list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterResistance, 0, resistanceType, ExtraEffectStatus.ExtraEffectType.SkillHit));
				list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterResistance, (int)resistanceType, ConstValue.ResistanceType.NONE, ExtraEffectStatus.ExtraEffectType.SkillHit));
				list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterResistance, (int)resistanceType, resistanceType, ExtraEffectStatus.ExtraEffectType.SkillHit));
			}
		}
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterTribe, 0, ConstValue.ResistanceType.NONE, ExtraEffectStatus.ExtraEffectType.SkillHit));
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterTribe, (int)character.species, ConstValue.ResistanceType.NONE, ExtraEffectStatus.ExtraEffectType.SkillHit));
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterGroup, 0, ConstValue.ResistanceType.NONE, ExtraEffectStatus.ExtraEffectType.SkillHit));
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterGroup, character.prefabId.ToInt32(), ConstValue.ResistanceType.NONE, ExtraEffectStatus.ExtraEffectType.SkillHit));
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.GrowStep, 0, ConstValue.ResistanceType.NONE, ExtraEffectStatus.ExtraEffectType.SkillHit));
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.GrowStep, ExtraEffectStatus.EvolutionStepToMasterId(character.evolutionStep), ConstValue.ResistanceType.NONE, ExtraEffectStatus.ExtraEffectType.SkillHit));
		if (list.Count > 0)
		{
			return ExtraEffectStatus.GetCorrectionValue(skillPropety.hitRate, list);
		}
		return skillPropety.hitRate;
	}

	private static int GetCorrectionValue(int baseValue, List<ExtraEffectStatus> extraEffectStatusList)
	{
		int num = baseValue;
		float num2 = 0f;
		bool flag = false;
		float num3 = 0f;
		float num4 = 0f;
		foreach (ExtraEffectStatus extraEffectStatus in extraEffectStatusList)
		{
			if (extraEffectStatus.EffectSubType == 1)
			{
				num2 = ((num2 <= extraEffectStatus.EffectValue) ? extraEffectStatus.EffectValue : num2);
				flag = true;
			}
			else if (extraEffectStatus.EffectSubType == 2)
			{
				num3 += extraEffectStatus.EffectValue;
			}
			else if (extraEffectStatus.EffectSubType == 3)
			{
				num4 += extraEffectStatus.EffectValue;
			}
		}
		num += (int)num4;
		num += (int)((float)num * (num3 / 100f));
		if (flag)
		{
			num = (int)num2;
		}
		return Mathf.Clamp(num, 1, num);
	}

	private static float GetCorrectionValue(float baseValue, List<ExtraEffectStatus> extraEffectStatusList)
	{
		float num = baseValue;
		float num2 = 0f;
		bool flag = false;
		float num3 = 0f;
		float num4 = 0f;
		foreach (ExtraEffectStatus extraEffectStatus in extraEffectStatusList)
		{
			if (extraEffectStatus.EffectSubType == 1)
			{
				num2 = ((num2 <= extraEffectStatus.EffectValue) ? extraEffectStatus.EffectValue : num2);
				flag = true;
			}
			else if (extraEffectStatus.EffectSubType == 2)
			{
				num3 += extraEffectStatus.EffectValue;
			}
			else if (extraEffectStatus.EffectSubType == 3)
			{
				num4 += extraEffectStatus.EffectValue;
			}
		}
		num += num4 / 100f;
		num += num * (num3 / 100f);
		if (flag)
		{
			num = num2;
		}
		return num;
	}

	public static ConstValue.ResistanceType GetSkillResistanceType(AffectEffectProperty skillPropety)
	{
		ConstValue.ResistanceType result = ConstValue.ResistanceType.NONE;
		if (skillPropety.type != AffectEffect.Damage)
		{
			switch (skillPropety.type)
			{
			case AffectEffect.Paralysis:
				result = ConstValue.ResistanceType.PARALYSIS;
				break;
			case AffectEffect.Poison:
				result = ConstValue.ResistanceType.POISON;
				break;
			case AffectEffect.Sleep:
				result = ConstValue.ResistanceType.SLEEP;
				break;
			case AffectEffect.SkillLock:
				result = ConstValue.ResistanceType.SKILL_LOCK;
				break;
			case AffectEffect.InstantDeath:
				result = ConstValue.ResistanceType.DEATH;
				break;
			case AffectEffect.Confusion:
				result = ConstValue.ResistanceType.CONFUSION;
				break;
			case AffectEffect.Stun:
				result = ConstValue.ResistanceType.STUN;
				break;
			}
		}
		else
		{
			switch (skillPropety.attribute)
			{
			case global::Attribute.None:
				result = ConstValue.ResistanceType.NOTHINGNESS;
				break;
			case global::Attribute.Red:
				result = ConstValue.ResistanceType.FIRE;
				break;
			case global::Attribute.Blue:
				result = ConstValue.ResistanceType.WATER;
				break;
			case global::Attribute.Yellow:
				result = ConstValue.ResistanceType.THUNDER;
				break;
			case global::Attribute.Green:
				result = ConstValue.ResistanceType.NATURE;
				break;
			case global::Attribute.White:
				result = ConstValue.ResistanceType.LIGHT;
				break;
			case global::Attribute.Black:
				result = ConstValue.ResistanceType.DARK;
				break;
			}
		}
		return result;
	}

	private static ConstValue.ResistanceType GetResistanceType(Species species)
	{
		ConstValue.ResistanceType result = ConstValue.ResistanceType.NONE;
		if (species == Species.PhantomStudents)
		{
			result = ConstValue.ResistanceType.NOTHINGNESS;
		}
		return result;
	}

	public static int EvolutionStepToMasterId(EvolutionStep evolutionStep)
	{
		return (int)(evolutionStep + 2);
	}

	public bool IsHitExtraEffect(CharacterStateControl character, ExtraEffectStatus.ExtraEffectType extraEffectType)
	{
		List<ExtraEffectStatus> totalExtraEffectStatusList = ExtraEffectStatus.GetTotalExtraEffectStatusList(new List<ExtraEffectStatus>
		{
			this
		}, character, extraEffectType);
		return totalExtraEffectStatusList.Count > 0;
	}

	private static List<ExtraEffectStatus> GetTotalExtraEffectStatusList(List<ExtraEffectStatus> extraEffectStatusList, CharacterStateControl character, ExtraEffectStatus.ExtraEffectType effectType)
	{
		List<ExtraEffectStatus> list = new List<ExtraEffectStatus>();
		ConstValue.ResistanceType resistanceType = ConstValue.ResistanceType.NONE;
		ExtraEffectStatus.ExtraTargetType targetType = ExtraEffectStatus.ExtraTargetType.Player;
		if (character != null && character.isEnemy)
		{
			targetType = ExtraEffectStatus.ExtraTargetType.Enemy;
		}
		if (BattleStateManager.current != null && BattleStateManager.current.battleMode == BattleMode.PvP)
		{
			targetType = ExtraEffectStatus.ExtraTargetType.All;
		}
		if (character != null)
		{
			List<ConstValue.ResistanceType> attributeStrengthList = character.tolerance.GetAttributeStrengthList();
			foreach (ConstValue.ResistanceType resistanceType2 in attributeStrengthList)
			{
				list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterResistance, 0, resistanceType2, effectType));
				list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterResistance, (int)resistanceType2, ConstValue.ResistanceType.NONE, effectType));
				list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterResistance, (int)resistanceType2, resistanceType2, effectType));
			}
		}
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterTribe, 0, resistanceType, effectType));
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterTribe, (int)character.species, ConstValue.ResistanceType.NONE, effectType));
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterGroup, 0, ConstValue.ResistanceType.NONE, effectType));
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterGroup, character.prefabId.ToInt32(), ConstValue.ResistanceType.NONE, effectType));
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.GrowStep, 0, ConstValue.ResistanceType.NONE, effectType));
		list.AddRange(ExtraEffectStatus.GetExtraEffectStatusList(extraEffectStatusList, targetType, ExtraEffectStatus.ExtraTargetSubType.GrowStep, ExtraEffectStatus.EvolutionStepToMasterId(character.evolutionStep), ConstValue.ResistanceType.NONE, effectType));
		return list;
	}

	public enum ExtraTargetType
	{
		All,
		Player,
		Enemy
	}

	public enum ExtraTargetSubType
	{
		Non,
		MonsterResistance,
		MonsterTribe,
		MonsterGroup,
		GrowStep,
		Quest,
		Skill,
		Suffer
	}

	public enum ExtraEffectType
	{
		Non,
		Damage = 10,
		SkillDamage,
		SkillHit,
		Atk = 21,
		Def,
		Hp,
		Speed,
		Satk,
		Sdef,
		AllStatus,
		Cluster = 31,
		Critical = 40,
		Hit,
		Poison = 50,
		Confusion,
		Sleep,
		Paralysis,
		Stun,
		SkillLock,
		Counter,
		Guts,
		HittingTheTarget,
		Skill = 60,
		LeaderChange = 70
	}

	public enum ExtraEffectSubType
	{
		Non,
		Overwrite,
		Ratio,
		Fixed
	}
}
