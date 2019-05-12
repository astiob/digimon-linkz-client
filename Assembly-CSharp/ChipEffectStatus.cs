using System;
using System.Collections.Generic;
using System.Linq;

public class ChipEffectStatus
{
	public static GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] GetInvocationList(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, ChipEffectStatus.EffectTriggerType targetType, int monsterGroupId, CharacterStateControl characterStateControl, int triggerValue = 0)
	{
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
		foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in chipEffects)
		{
			if (chipEffect.effectType.ToInt32() != 56 || (!(characterStateControl == null) && !(characterStateControl == BattleStateManager.current.battleStateData.currentSelectCharacterState) && !characterStateControl.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Stun) && !characterStateControl.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Sleep)))
			{
				int num = 0;
				if (chipEffect.monsterGroupId != null && chipEffect.monsterGroupId.Length > 0)
				{
					num = chipEffect.monsterGroupId.ToInt32();
				}
				if (num <= 0 || num == monsterGroupId)
				{
					if (chipEffect.effectTrigger.ToInt32() == (int)targetType)
					{
						bool flag = false;
						if (targetType == ChipEffectStatus.EffectTriggerType.HpPercentage)
						{
							string[] array = chipEffect.effectTriggerValue.Split(new char[]
							{
								','
							});
							if (array.Length == 2 && array[0].ToInt32() <= triggerValue && array[1].ToInt32() >= triggerValue)
							{
								flag = true;
							}
						}
						else if (targetType == ChipEffectStatus.EffectTriggerType.HpFixed)
						{
							if (chipEffect.effectTriggerValue.ToInt32() == triggerValue)
							{
								flag = true;
							}
						}
						else if (targetType == ChipEffectStatus.EffectTriggerType.Area)
						{
							if (triggerValue == chipEffect.effectTriggerValue.ToInt32())
							{
								flag = true;
							}
						}
						else if (targetType == ChipEffectStatus.EffectTriggerType.AttackHit)
						{
							if (characterStateControl.currentSkillStatus.skillType == SkillType.Attack)
							{
								BattleServerControl serverControl = BattleStateManager.current.serverControl;
								List<AffectEffectProperty> affectEffectPropertyList = serverControl.GetAffectEffectPropertyList(chipEffect.effectValue);
								foreach (AffectEffectProperty addAffectEffect in affectEffectPropertyList)
								{
									characterStateControl.currentSkillStatus.AddAffectEffect(addAffectEffect);
								}
							}
						}
						else if (targetType == ChipEffectStatus.EffectTriggerType.Suffer)
						{
							if (characterStateControl != null)
							{
								string[] array2 = chipEffect.effectTriggerValue.Split(new char[]
								{
									','
								});
								List<SufferStateProperty.SufferType> list2 = new List<SufferStateProperty.SufferType>();
								for (int j = 0; j < array2.Length; j++)
								{
									SufferStateProperty.SufferType item = (SufferStateProperty.SufferType)array2[j].ToInt32();
									list2.Add(item);
								}
								foreach (SufferStateProperty.SufferType sufferType in list2)
								{
									foreach (SufferStateProperty sufferStateProperty in characterStateControl.hitSufferList)
									{
										if (sufferStateProperty.sufferTypeCache == sufferType)
										{
											flag = true;
											break;
										}
									}
								}
							}
						}
						else
						{
							flag = true;
						}
						if (flag)
						{
							list.Add(chipEffect);
						}
					}
				}
			}
		}
		return list.ToArray();
	}

	public static int GetChipEffectValue(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, int baseValue, CharacterStateControl character, ExtraEffectStatus.ExtraEffectType effectType)
	{
		return (int)ChipEffectStatus.GetChipEffectValueToFloat(chipEffects, (float)baseValue, character, effectType);
	}

	public static float GetChipEffectValueToFloat(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, float baseValue, CharacterStateControl character, ExtraEffectStatus.ExtraEffectType effectType)
	{
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
		ExtraEffectStatus.ExtraTargetType targetType = ExtraEffectStatus.ExtraTargetType.Player;
		if (character != null && character.isEnemy)
		{
			targetType = ExtraEffectStatus.ExtraTargetType.Enemy;
		}
		if (BattleStateManager.current != null && BattleStateManager.current.battleMode == BattleMode.PvP)
		{
			targetType = ExtraEffectStatus.ExtraTargetType.All;
		}
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.Non, 0, ConstValue.ResistanceType.NONE, effectType));
		ConstValue.ResistanceType resistanceType = ConstValue.ResistanceType.NONE;
		if (character != null)
		{
			List<ConstValue.ResistanceType> attributeStrengthList = character.tolerance.GetAttributeStrengthList();
			foreach (ConstValue.ResistanceType resistanceType2 in attributeStrengthList)
			{
				list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterResistance, 0, resistanceType2, effectType));
				list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterResistance, (int)resistanceType2, ConstValue.ResistanceType.NONE, effectType));
				list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterResistance, (int)resistanceType2, resistanceType2, effectType));
			}
		}
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterTribe, 0, resistanceType, effectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterTribe, (int)character.species, ConstValue.ResistanceType.NONE, effectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterGroup, 0, ConstValue.ResistanceType.NONE, effectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterGroup, character.prefabId.ToInt32(), ConstValue.ResistanceType.NONE, effectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.GrowStep, 0, ConstValue.ResistanceType.NONE, effectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.GrowStep, ExtraEffectStatus.EvolutionStepToMasterId(character.evolutionStep), ConstValue.ResistanceType.NONE, effectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.Suffer, 0, ConstValue.ResistanceType.NONE, effectType));
		foreach (object obj in Enum.GetValues(typeof(SufferStateProperty.SufferType)))
		{
			SufferStateProperty.SufferType sufferType = (SufferStateProperty.SufferType)((int)obj);
			if (sufferType != SufferStateProperty.SufferType.Null)
			{
				if (character.currentSufferState.FindSufferState(sufferType))
				{
					list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.Suffer, (int)sufferType, ConstValue.ResistanceType.NONE, effectType));
				}
			}
		}
		if (list.Count > 0)
		{
			float correctionValue = ChipEffectStatus.GetCorrectionValue(baseValue, list);
			return correctionValue - baseValue;
		}
		return 0f;
	}

	public static int GetSkillPowerCorrectionValue(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, AffectEffectProperty skillPropety, CharacterStateControl character)
	{
		return (int)ChipEffectStatus.GetSkillCorrectionValue(chipEffects, skillPropety, character, ExtraEffectStatus.ExtraEffectType.SkillDamage);
	}

	public static float GetSkillHitRateCorrectionValue(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, AffectEffectProperty skillPropety, CharacterStateControl character)
	{
		return ChipEffectStatus.GetSkillCorrectionValue(chipEffects, skillPropety, character, ExtraEffectStatus.ExtraEffectType.SkillHit);
	}

	private static float GetSkillCorrectionValue(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, AffectEffectProperty skillPropety, CharacterStateControl character, ExtraEffectStatus.ExtraEffectType extraEffectType)
	{
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
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
			list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.Skill, 0, skillResistanceType, extraEffectType));
		}
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.Skill, skillPropety.skillId, ConstValue.ResistanceType.NONE, extraEffectType));
		if (character != null)
		{
			List<ConstValue.ResistanceType> attributeStrengthList = character.tolerance.GetAttributeStrengthList();
			foreach (ConstValue.ResistanceType resistanceType in attributeStrengthList)
			{
				list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterResistance, 0, resistanceType, extraEffectType));
				list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterResistance, (int)resistanceType, ConstValue.ResistanceType.NONE, extraEffectType));
				list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterResistance, (int)resistanceType, resistanceType, extraEffectType));
			}
		}
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterTribe, 0, ConstValue.ResistanceType.NONE, extraEffectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterTribe, (int)character.species, ConstValue.ResistanceType.NONE, extraEffectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterGroup, 0, ConstValue.ResistanceType.NONE, extraEffectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterGroup, character.prefabId.ToInt32(), ConstValue.ResistanceType.NONE, extraEffectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.GrowStep, 0, ConstValue.ResistanceType.NONE, extraEffectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.GrowStep, ExtraEffectStatus.EvolutionStepToMasterId(character.evolutionStep), ConstValue.ResistanceType.NONE, extraEffectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.Suffer, 0, ConstValue.ResistanceType.NONE, extraEffectType));
		foreach (object obj in Enum.GetValues(typeof(SufferStateProperty.SufferType)))
		{
			SufferStateProperty.SufferType sufferType = (SufferStateProperty.SufferType)((int)obj);
			if (sufferType != SufferStateProperty.SufferType.Null)
			{
				if (character.currentSufferState.FindSufferState(sufferType))
				{
					list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.Suffer, (int)sufferType, ConstValue.ResistanceType.NONE, extraEffectType));
				}
			}
		}
		if (list.Count > 0)
		{
			if (extraEffectType == ExtraEffectStatus.ExtraEffectType.SkillDamage)
			{
				return (float)(ChipEffectStatus.GetCorrectionValue(skillPropety.power, list) - skillPropety.power);
			}
			if (extraEffectType == ExtraEffectStatus.ExtraEffectType.SkillHit)
			{
				return ChipEffectStatus.GetCorrectionValue(skillPropety.hitRate, list) - skillPropety.hitRate;
			}
		}
		return 0f;
	}

	public static float GetDamageRateCorrectionValue(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, float baseValue, CharacterStateControl character, CharacterStateControl targetCharacter)
	{
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
		foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in chipEffects)
		{
			if (chipEffect.effectType.ToInt32() == 10)
			{
				Species species = (Species)chipEffect.targetValue2.ToInt32();
				if (targetCharacter.species == species)
				{
					list.Add(chipEffect);
				}
			}
		}
		return ChipEffectStatus.GetChipEffectValueToFloat(list.ToArray(), baseValue, character, ExtraEffectStatus.ExtraEffectType.Damage);
	}

	public static int GetCorrectionValue(int baseValue, List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> extraEffectStatusList)
	{
		int num = baseValue;
		float num2 = 0f;
		bool flag = false;
		float num3 = 0f;
		float num4 = 0f;
		foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in extraEffectStatusList)
		{
			if (chipEffect.effectSubType.ToInt32() == 1)
			{
				num2 = ((num2 <= chipEffect.effectValue.ToFloat()) ? chipEffect.effectValue.ToFloat() : num2);
				flag = true;
			}
			else if (chipEffect.effectSubType.ToInt32() == 2)
			{
				num3 += chipEffect.effectValue.ToFloat();
			}
			else if (chipEffect.effectSubType.ToInt32() == 3)
			{
				num4 += chipEffect.effectValue.ToFloat();
			}
		}
		num += (int)num4;
		num += (int)((float)num * num3);
		if (flag)
		{
			num = (int)num2;
		}
		return num;
	}

	private static float GetCorrectionValue(float baseValue, List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> extraEffectStatusList)
	{
		float num = baseValue;
		float num2 = 0f;
		bool flag = false;
		float num3 = 0f;
		float num4 = 0f;
		foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in extraEffectStatusList)
		{
			if (chipEffect.effectSubType.ToInt32() == 1)
			{
				num2 = ((num2 <= chipEffect.effectValue.ToFloat()) ? chipEffect.effectValue.ToFloat() : num2);
				flag = true;
			}
			else if (chipEffect.effectSubType.ToInt32() == 2)
			{
				num3 += chipEffect.effectValue.ToFloat();
			}
			else if (chipEffect.effectSubType.ToInt32() == 3)
			{
				num4 += chipEffect.effectValue.ToFloat();
			}
		}
		num += num4;
		num += num * num3;
		if (flag)
		{
			num = num2;
		}
		return num;
	}

	public static List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> GetChipEffectList(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, ExtraEffectStatus.ExtraTargetType targetType, ExtraEffectStatus.ExtraTargetSubType targetSubType, int targetValue, ConstValue.ResistanceType resistanceType, ExtraEffectStatus.ExtraEffectType effectType)
	{
		ChipEffectStatus.<GetChipEffectList>c__AnonStorey2DA <GetChipEffectList>c__AnonStorey2DA = new ChipEffectStatus.<GetChipEffectList>c__AnonStorey2DA();
		<GetChipEffectList>c__AnonStorey2DA.effectType = effectType;
		<GetChipEffectList>c__AnonStorey2DA.targetType = targetType;
		<GetChipEffectList>c__AnonStorey2DA.targetSubType = targetSubType;
		<GetChipEffectList>c__AnonStorey2DA.targetValue = targetValue;
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
		if (chipEffects.Length == 0)
		{
			return list;
		}
		<GetChipEffectList>c__AnonStorey2DA.searchEffectType = ((int x) => x == (int)<GetChipEffectList>c__AnonStorey2DA.effectType);
		if (ExtraEffectStatus.ExtraEffectType.Atk <= <GetChipEffectList>c__AnonStorey2DA.effectType && <GetChipEffectList>c__AnonStorey2DA.effectType < ExtraEffectStatus.ExtraEffectType.AllStatus)
		{
			<GetChipEffectList>c__AnonStorey2DA.searchEffectType = ((int x) => x == (int)<GetChipEffectList>c__AnonStorey2DA.effectType || x == 27);
		}
		IEnumerable<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> enumerable;
		if (resistanceType == ConstValue.ResistanceType.NONE)
		{
			enumerable = chipEffects.Where((GameWebAPI.RespDataMA_ChipEffectM.ChipEffect x) => (x.targetType.ToInt32() == (int)<GetChipEffectList>c__AnonStorey2DA.targetType || x.targetType.ToInt32() == 0) && x.targetSubType.ToInt32() == (int)<GetChipEffectList>c__AnonStorey2DA.targetSubType && x.targetValue.ToInt32() == <GetChipEffectList>c__AnonStorey2DA.targetValue && <GetChipEffectList>c__AnonStorey2DA.searchEffectType(x.effectType.ToInt32()));
		}
		else
		{
			ChipEffectStatus.<GetChipEffectList>c__AnonStorey2DB <GetChipEffectList>c__AnonStorey2DB = new ChipEffectStatus.<GetChipEffectList>c__AnonStorey2DB();
			<GetChipEffectList>c__AnonStorey2DB.<>f__ref$730 = <GetChipEffectList>c__AnonStorey2DA;
			ChipEffectStatus.<GetChipEffectList>c__AnonStorey2DB <GetChipEffectList>c__AnonStorey2DB2 = <GetChipEffectList>c__AnonStorey2DB;
			int num = (int)resistanceType;
			<GetChipEffectList>c__AnonStorey2DB2.resistance = num.ToString();
			enumerable = chipEffects.Where((GameWebAPI.RespDataMA_ChipEffectM.ChipEffect x) => (x.targetType.ToInt32() == (int)<GetChipEffectList>c__AnonStorey2DB.<>f__ref$730.targetType || x.targetType.ToInt32() == 0) && x.targetSubType.ToInt32() == (int)<GetChipEffectList>c__AnonStorey2DB.<>f__ref$730.targetSubType && x.targetValue.ToInt32() == <GetChipEffectList>c__AnonStorey2DB.<>f__ref$730.targetValue && x.targetValue2.Contains(<GetChipEffectList>c__AnonStorey2DB.resistance) && <GetChipEffectList>c__AnonStorey2DB.<>f__ref$730.searchEffectType(x.effectType.ToInt32()));
		}
		foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect item in enumerable)
		{
			list.Add(item);
		}
		return list;
	}

	public enum EffectTriggerType
	{
		Usually,
		TurnStarted,
		TurnEnd,
		WaveStarted,
		WaveEnd,
		HpPercentage,
		Dead,
		HpFixed,
		RoundStarted,
		RoundEnd,
		Kill,
		Area,
		AttackHit,
		Suffer
	}
}
