using BattleStateMachineInternal;
using Monster;
using System;
using System.Collections.Generic;
using System.Linq;

public class ChipEffectStatus : EffectStatusBase
{
	public static GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] GetInvocationList(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, EffectStatusBase.EffectTriggerType targetType, int monsterGroupId, CharacterStateControl characterStateControl, int triggerValue = 0)
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
						if (targetType == EffectStatusBase.EffectTriggerType.HpPercentage)
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
						else if (targetType == EffectStatusBase.EffectTriggerType.HpFixed)
						{
							if (chipEffect.effectTriggerValue.ToInt32() == triggerValue)
							{
								flag = true;
							}
						}
						else if (targetType == EffectStatusBase.EffectTriggerType.Kill)
						{
							if (chipEffect.effectTriggerValue == "1")
							{
								if (characterStateControl != null)
								{
									CharacterStateControl[] source;
									if (characterStateControl.targetCharacter.isEnemy)
									{
										source = BattleStateManager.current.battleStateData.enemies;
									}
									else
									{
										source = BattleStateManager.current.battleStateData.playerCharacters;
									}
									if (source.Where((CharacterStateControl item) => !item.isDied).Any<CharacterStateControl>())
									{
										flag = true;
									}
								}
							}
							else
							{
								flag = true;
							}
						}
						else if (targetType == EffectStatusBase.EffectTriggerType.Area)
						{
							if (triggerValue == chipEffect.effectTriggerValue.ToInt32())
							{
								flag = true;
							}
						}
						else if (targetType == EffectStatusBase.EffectTriggerType.AttackHit)
						{
							if (characterStateControl == BattleStateManager.current.battleStateData.currentSelectCharacterState && characterStateControl.currentSkillStatus.skillType == SkillType.Attack)
							{
								if (chipEffect.effectType.ToInt32() == 61)
								{
									BattleServerControl serverControl = BattleStateManager.current.serverControl;
									List<AffectEffectProperty> affectEffectPropertyList = serverControl.GetAffectEffectPropertyList(chipEffect.effectValue);
									if (affectEffectPropertyList != null)
									{
										foreach (AffectEffectProperty addAffectEffect in affectEffectPropertyList)
										{
											characterStateControl.currentSkillStatus.AddAffectEffect(addAffectEffect);
										}
									}
									flag = true;
								}
								else
								{
									flag = true;
								}
							}
						}
						else if (targetType == EffectStatusBase.EffectTriggerType.SkillStartedApMax)
						{
							if (characterStateControl == BattleStateManager.current.battleStateData.currentSelectCharacterState && characterStateControl.ap == characterStateControl.maxAp)
							{
								if (chipEffect.effectType.ToInt32() == 61)
								{
									BattleServerControl serverControl2 = BattleStateManager.current.serverControl;
									List<AffectEffectProperty> affectEffectPropertyList2 = serverControl2.GetAffectEffectPropertyList(chipEffect.effectValue);
									if (affectEffectPropertyList2 != null)
									{
										foreach (AffectEffectProperty addAffectEffect2 in affectEffectPropertyList2)
										{
											characterStateControl.currentSkillStatus.AddAffectEffect(addAffectEffect2);
										}
									}
									flag = true;
								}
								else
								{
									flag = true;
								}
							}
						}
						else if (targetType == EffectStatusBase.EffectTriggerType.AttackCommandedTarget)
						{
							if (characterStateControl == BattleStateManager.current.battleStateData.currentSelectCharacterState && characterStateControl.currentSkillStatus.ThisSkillIsAttack)
							{
								if (characterStateControl.currentSkillStatus.numbers == EffectNumbers.Simple)
								{
									if (characterStateControl.targetCharacter.skillOrder < characterStateControl.skillOrder)
									{
										flag = true;
									}
								}
								else
								{
									CharacterStateControl[] source2;
									if (characterStateControl.targetCharacter.isEnemy)
									{
										source2 = BattleStateManager.current.battleStateData.enemies.Where((CharacterStateControl item) => !item.isDied).ToArray<CharacterStateControl>();
									}
									else
									{
										source2 = BattleStateManager.current.battleStateData.playerCharacters.Where((CharacterStateControl item) => !item.isDied).ToArray<CharacterStateControl>();
									}
									if (source2.Where((CharacterStateControl item) => item.skillOrder < characterStateControl.skillOrder).Any<CharacterStateControl>())
									{
										flag = true;
									}
								}
							}
						}
						else if (targetType == EffectStatusBase.EffectTriggerType.SkillSpecies)
						{
							if (characterStateControl == BattleStateManager.current.battleStateData.currentSelectCharacterState && (chipEffect.effectTriggerValue == "0" || characterStateControl.characterDatas.tribe == chipEffect.effectTriggerValue))
							{
								flag = true;
							}
						}
						else if (targetType == EffectStatusBase.EffectTriggerType.SkillTargetSpecies)
						{
							if (characterStateControl == BattleStateManager.current.battleStateData.currentSelectCharacterState)
							{
								string tribe = chipEffect.effectTriggerValue;
								if (tribe == "0")
								{
									flag = true;
								}
								else if (characterStateControl.currentSkillStatus.numbers == EffectNumbers.Simple)
								{
									if (characterStateControl.targetCharacter.characterDatas.tribe == tribe)
									{
										flag = true;
									}
								}
								else
								{
									CharacterStateControl[] source3;
									if (characterStateControl.targetCharacter.isEnemy)
									{
										source3 = BattleStateManager.current.battleStateData.enemies.Where((CharacterStateControl item) => !item.isDied).ToArray<CharacterStateControl>();
									}
									else
									{
										source3 = BattleStateManager.current.battleStateData.playerCharacters.Where((CharacterStateControl item) => !item.isDied).ToArray<CharacterStateControl>();
									}
									if (source3.Where((CharacterStateControl item) => item.characterDatas.tribe == tribe).Any<CharacterStateControl>())
									{
										flag = true;
									}
								}
							}
						}
						else if (targetType == EffectStatusBase.EffectTriggerType.SkillAttribute)
						{
							if (characterStateControl == BattleStateManager.current.battleStateData.currentSelectCharacterState)
							{
								global::Attribute attribute = ServerToBattleUtility.IntToAttribute(chipEffect.effectTriggerValue.ToInt32());
								if (chipEffect.effectTriggerValue.ToInt32() == 0 || characterStateControl.currentSkillStatus.attribute == attribute)
								{
									flag = true;
								}
							}
						}
						else if (targetType == EffectStatusBase.EffectTriggerType.SkillRecieveAttribute)
						{
							CharacterStateControl currentSelectCharacterState = BattleStateManager.current.battleStateData.currentSelectCharacterState;
							global::Attribute attribute2 = ServerToBattleUtility.IntToAttribute(chipEffect.effectTriggerValue.ToInt32());
							if (chipEffect.effectTriggerValue.ToInt32() == 0)
							{
								flag = true;
							}
							else if (currentSelectCharacterState.currentSkillStatus.attribute == attribute2)
							{
								if (currentSelectCharacterState.currentSkillStatus.numbers == EffectNumbers.Simple)
								{
									if (currentSelectCharacterState.targetCharacter == characterStateControl)
									{
										flag = true;
									}
								}
								else
								{
									CharacterStateControl[] source4;
									if (currentSelectCharacterState.targetCharacter.isEnemy)
									{
										source4 = BattleStateManager.current.battleStateData.enemies.Where((CharacterStateControl item) => !item.isDied).ToArray<CharacterStateControl>();
									}
									else
									{
										source4 = BattleStateManager.current.battleStateData.playerCharacters.Where((CharacterStateControl item) => !item.isDied).ToArray<CharacterStateControl>();
									}
									if (source4.Where((CharacterStateControl item) => item == characterStateControl).Any<CharacterStateControl>())
									{
										flag = true;
									}
								}
							}
						}
						else if (targetType == EffectStatusBase.EffectTriggerType.Suffer)
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
									SufferStateProperty.SufferType item2 = (SufferStateProperty.SufferType)array2[j].ToInt32();
									list2.Add(item2);
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

	public static int GetChipEffectValue(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, int baseValue, CharacterStateControl character, EffectStatusBase.ExtraEffectType effectType)
	{
		return (int)ChipEffectStatus.GetChipEffectValueToFloat(chipEffects, (float)baseValue, character.isEnemy, character.characterStatus.monsterIntegrationIds, character.groupId, character.tolerance, character.characterDatas.tribe, character.characterDatas.growStep, null, character.currentSufferState, ChipEffectStatus.GetTargetType(character, character), effectType);
	}

	public static float GetChipEffectValueToFloat(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, float baseValue, CharacterStateControl character, EffectStatusBase.ExtraEffectType effectType)
	{
		return ChipEffectStatus.GetChipEffectValueToFloat(chipEffects, baseValue, character.isEnemy, character.characterStatus.monsterIntegrationIds, character.groupId, character.tolerance, character.characterDatas.tribe, character.characterDatas.growStep, null, character.currentSufferState, ChipEffectStatus.GetTargetType(character, character), effectType);
	}

	public static float GetChipEffectValueToFloat(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, float baseValue, bool isEnemy, string[] monsterIntegrationIds, string groupId, Tolerance tolerance, string tribe, GrowStep growStep, AffectEffectProperty skillPropety, HaveSufferState currentSufferState, ChipEffectStatus.TargetType targetType, EffectStatusBase.ExtraEffectType effectType)
	{
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> totalChipEffectStatusList = ChipEffectStatus.GetTotalChipEffectStatusList(chipEffects, isEnemy, monsterIntegrationIds, groupId, tolerance, tribe, growStep, skillPropety, currentSufferState, targetType, effectType);
		if (totalChipEffectStatusList.Count > 0)
		{
			float correctionValue = ChipEffectStatus.GetCorrectionValue(baseValue, totalChipEffectStatusList);
			return correctionValue - baseValue;
		}
		return 0f;
	}

	private static List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> GetTotalChipEffectStatusList(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, bool isEnemy, string[] monsterIntegrationIds, string groupId, Tolerance tolerance, string tribe, GrowStep growStep, AffectEffectProperty skillPropety, HaveSufferState currentSufferState, ChipEffectStatus.TargetType targetType, EffectStatusBase.ExtraEffectType effectType)
	{
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, EffectStatusBase.ExtraTargetSubType.Non, 0, isEnemy, ConstValue.ResistanceType.NONE, effectType));
		if (skillPropety != null)
		{
			ConstValue.ResistanceType skillResistanceType = EffectStatusBase.GetSkillResistanceType(skillPropety);
			if (skillResistanceType != ConstValue.ResistanceType.NONE)
			{
				list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, EffectStatusBase.ExtraTargetSubType.Skill, 0, isEnemy, skillResistanceType, effectType));
			}
			list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, EffectStatusBase.ExtraTargetSubType.Skill, skillPropety.skillId, isEnemy, ConstValue.ResistanceType.NONE, effectType));
		}
		List<ConstValue.ResistanceType> attributeStrengthList = tolerance.GetAttributeStrengthList();
		foreach (ConstValue.ResistanceType resistanceType in attributeStrengthList)
		{
			list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, EffectStatusBase.ExtraTargetSubType.MonsterResistance, 0, isEnemy, resistanceType, effectType));
			list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, EffectStatusBase.ExtraTargetSubType.MonsterResistance, (int)resistanceType, isEnemy, ConstValue.ResistanceType.NONE, effectType));
			list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, EffectStatusBase.ExtraTargetSubType.MonsterResistance, (int)resistanceType, isEnemy, resistanceType, effectType));
		}
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, EffectStatusBase.ExtraTargetSubType.MonsterTribe, 0, isEnemy, ConstValue.ResistanceType.NONE, effectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, EffectStatusBase.ExtraTargetSubType.MonsterTribe, tribe.ToInt32(), isEnemy, ConstValue.ResistanceType.NONE, effectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, EffectStatusBase.ExtraTargetSubType.MonsterGroup, 0, isEnemy, ConstValue.ResistanceType.NONE, effectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, EffectStatusBase.ExtraTargetSubType.MonsterGroup, groupId.ToInt32(), isEnemy, ConstValue.ResistanceType.NONE, effectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, EffectStatusBase.ExtraTargetSubType.GrowStep, 0, isEnemy, ConstValue.ResistanceType.NONE, effectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, EffectStatusBase.ExtraTargetSubType.GrowStep, (int)growStep, isEnemy, ConstValue.ResistanceType.NONE, effectType));
		if (currentSufferState != null)
		{
			list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, EffectStatusBase.ExtraTargetSubType.Suffer, 0, isEnemy, ConstValue.ResistanceType.NONE, effectType));
			foreach (object obj in Enum.GetValues(typeof(SufferStateProperty.SufferType)))
			{
				SufferStateProperty.SufferType sufferType = (SufferStateProperty.SufferType)((int)obj);
				if (sufferType != SufferStateProperty.SufferType.Null)
				{
					if (currentSufferState.FindSufferState(sufferType))
					{
						list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, EffectStatusBase.ExtraTargetSubType.Suffer, (int)sufferType, isEnemy, ConstValue.ResistanceType.NONE, effectType));
					}
				}
			}
		}
		list.AddRange(ChipEffectStatus.GetMonsterIntegrationGroupList(chipEffects, isEnemy, monsterIntegrationIds, targetType, effectType));
		return list;
	}

	public static int GetSkillPowerCorrectionValue(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, AffectEffectProperty skillPropety, CharacterStateControl character)
	{
		EffectStatusBase.ExtraEffectType effectType;
		if (skillPropety.skillId.ToString() == BattleStateManager.current.publicAttackSkillId)
		{
			effectType = EffectStatusBase.ExtraEffectType.DefaultAttackDamage;
		}
		else
		{
			effectType = EffectStatusBase.ExtraEffectType.SkillDamage;
		}
		return (int)ChipEffectStatus.GetChipEffectValueToFloat(chipEffects, (float)skillPropety.GetPower(character), character.isEnemy, character.characterStatus.monsterIntegrationIds, character.groupId, character.tolerance, character.characterDatas.tribe, character.characterDatas.growStep, skillPropety, character.currentSufferState, ChipEffectStatus.GetTargetType(character, character), effectType);
	}

	public static float GetSkillHitRateCorrectionValue(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, AffectEffectProperty skillPropety, CharacterStateControl character)
	{
		EffectStatusBase.ExtraEffectType effectType;
		if (skillPropety.skillId.ToString() == BattleStateManager.current.publicAttackSkillId)
		{
			effectType = EffectStatusBase.ExtraEffectType.DefaultAttackHit;
		}
		else
		{
			effectType = EffectStatusBase.ExtraEffectType.SkillHit;
		}
		return ChipEffectStatus.GetChipEffectValueToFloat(chipEffects, skillPropety.hitRate, character.isEnemy, character.characterStatus.monsterIntegrationIds, character.groupId, character.tolerance, character.characterDatas.tribe, character.characterDatas.growStep, skillPropety, character.currentSufferState, ChipEffectStatus.GetTargetType(character, character), effectType);
	}

	private static int GetCorrectionValue(int baseValue, List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> extraEffectStatusList)
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

	private static List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> GetChipEffectList(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, ChipEffectStatus.TargetType targetType, EffectStatusBase.ExtraTargetSubType targetSubType, int targetValue, bool isEnemy, ConstValue.ResistanceType resistanceType, EffectStatusBase.ExtraEffectType effectType)
	{
		ChipEffectStatus.<GetChipEffectList>c__AnonStorey2ED <GetChipEffectList>c__AnonStorey2ED = new ChipEffectStatus.<GetChipEffectList>c__AnonStorey2ED();
		<GetChipEffectList>c__AnonStorey2ED.effectType = effectType;
		<GetChipEffectList>c__AnonStorey2ED.targetType = targetType;
		<GetChipEffectList>c__AnonStorey2ED.isEnemy = isEnemy;
		<GetChipEffectList>c__AnonStorey2ED.targetSubType = targetSubType;
		<GetChipEffectList>c__AnonStorey2ED.targetValue = targetValue;
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
		if (chipEffects.Length == 0)
		{
			return list;
		}
		<GetChipEffectList>c__AnonStorey2ED.searchEffectType = ((int x) => x == (int)<GetChipEffectList>c__AnonStorey2ED.effectType);
		if (EffectStatusBase.ExtraEffectType.Atk <= <GetChipEffectList>c__AnonStorey2ED.effectType && <GetChipEffectList>c__AnonStorey2ED.effectType < EffectStatusBase.ExtraEffectType.AllStatus)
		{
			<GetChipEffectList>c__AnonStorey2ED.searchEffectType = ((int x) => x == (int)<GetChipEffectList>c__AnonStorey2ED.effectType || x == 27);
		}
		<GetChipEffectList>c__AnonStorey2ED.searchTargetType = ((int x) => x == (int)<GetChipEffectList>c__AnonStorey2ED.targetType || (!<GetChipEffectList>c__AnonStorey2ED.isEnemy && x == 1) || (<GetChipEffectList>c__AnonStorey2ED.isEnemy && x == 2));
		IEnumerable<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> enumerable;
		if (resistanceType == ConstValue.ResistanceType.NONE)
		{
			enumerable = chipEffects.Where((GameWebAPI.RespDataMA_ChipEffectM.ChipEffect x) => <GetChipEffectList>c__AnonStorey2ED.searchTargetType(x.targetType.ToInt32()) && x.targetSubType.ToInt32() == (int)<GetChipEffectList>c__AnonStorey2ED.targetSubType && x.targetValue.ToInt32() == <GetChipEffectList>c__AnonStorey2ED.targetValue && <GetChipEffectList>c__AnonStorey2ED.searchEffectType(x.effectType.ToInt32()));
		}
		else
		{
			ChipEffectStatus.<GetChipEffectList>c__AnonStorey2EE <GetChipEffectList>c__AnonStorey2EE = new ChipEffectStatus.<GetChipEffectList>c__AnonStorey2EE();
			<GetChipEffectList>c__AnonStorey2EE.<>f__ref$749 = <GetChipEffectList>c__AnonStorey2ED;
			ChipEffectStatus.<GetChipEffectList>c__AnonStorey2EE <GetChipEffectList>c__AnonStorey2EE2 = <GetChipEffectList>c__AnonStorey2EE;
			int num = (int)resistanceType;
			<GetChipEffectList>c__AnonStorey2EE2.resistance = num.ToString();
			enumerable = chipEffects.Where((GameWebAPI.RespDataMA_ChipEffectM.ChipEffect x) => <GetChipEffectList>c__AnonStorey2EE.<>f__ref$749.searchTargetType(x.targetType.ToInt32()) && x.targetSubType.ToInt32() == (int)<GetChipEffectList>c__AnonStorey2EE.<>f__ref$749.targetSubType && x.targetValue.ToInt32() == <GetChipEffectList>c__AnonStorey2EE.<>f__ref$749.targetValue && x.targetValue2.Contains(<GetChipEffectList>c__AnonStorey2EE.resistance) && <GetChipEffectList>c__AnonStorey2EE.<>f__ref$749.searchEffectType(x.effectType.ToInt32()));
		}
		foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect item in enumerable)
		{
			list.Add(item);
		}
		return list;
	}

	private static List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> GetMonsterIntegrationGroupList(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, bool isEnemy, string[] monsterIntegrationIds, ChipEffectStatus.TargetType targetType, EffectStatusBase.ExtraEffectType effectType)
	{
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list2 = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
		foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in chipEffects)
		{
			EffectStatusBase.ExtraTargetSubType extraTargetSubType = (EffectStatusBase.ExtraTargetSubType)chipEffect.targetSubType.ToInt32();
			if (extraTargetSubType == EffectStatusBase.ExtraTargetSubType.MonsterIntegrationGroup)
			{
				list2.Add(chipEffect);
			}
		}
		while (list2.Count > 0)
		{
			GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect2 = list2[0];
			List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list3 = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
			List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list4 = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
			foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect3 in list2)
			{
				if (chipEffect3.targetValue == chipEffect2.targetValue)
				{
					list3.Add(chipEffect3);
				}
				else
				{
					list4.Add(chipEffect3);
				}
			}
			string id = chipEffect2.targetValue;
			bool flag = monsterIntegrationIds.Where((string item) => item == id).Any<string>();
			if (flag)
			{
				List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> chipEffectList = ChipEffectStatus.GetChipEffectList(list3.ToArray(), targetType, EffectStatusBase.ExtraTargetSubType.MonsterIntegrationGroup, chipEffect2.targetValue.ToInt32(), isEnemy, ConstValue.ResistanceType.NONE, effectType);
				if (chipEffectList.Count > 0)
				{
					list.AddRange(chipEffectList);
				}
			}
			list2 = list4;
		}
		return list;
	}

	public static bool CheckStageEffectInvalid(List<ExtraEffectStatus> extraEffectStatusList, CharacterStateControl characterStateControl)
	{
		foreach (ExtraEffectStatus extraEffectStatus in extraEffectStatusList)
		{
			if (ChipEffectStatus.CheckStageEffectInvalid(characterStateControl, extraEffectStatus))
			{
				return true;
			}
		}
		return false;
	}

	public static bool CheckStageEffectInvalid(int areaId, List<ExtraEffectStatus> extraEffectStatusList, MonsterData[] chipPlayers, MonsterData[] chipEnemys, MonsterData chipTarget)
	{
		bool flag = chipEnemys.Where((MonsterData item) => item.userMonster.userMonsterId == chipTarget.userMonster.userMonsterId).Any<MonsterData>();
		GameWebAPI.RespDataMA_GetMonsterMG.MonsterM monsterGroupMasterByMonsterGroupId = MonsterDataMng.Instance().GetMonsterGroupMasterByMonsterGroupId(chipTarget.monsterM.monsterGroupId);
		GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster responseMonsterIntegrationGroupMaster = MasterDataMng.Instance().ResponseMonsterIntegrationGroupMaster;
		GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup[] source = responseMonsterIntegrationGroupMaster.monsterIntegrationGroupM.Where((GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup item) => item.monsterId == chipTarget.monsterM.monsterId).ToArray<GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup>();
		string[] monsterIntegrationIds = source.Select((GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup item) => item.monsterIntegrationId).ToArray<string>();
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM resistanceMaster = MonsterData.SerchResistanceById(chipTarget.monsterM.resistanceId);
		List<GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM> uniqueResistanceList = MonsterResistanceData.GetUniqueResistanceList(chipTarget.GetResistanceIdList());
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM data = MonsterResistanceData.AddResistanceFromMultipleTranceData(resistanceMaster, uniqueResistanceList);
		Tolerance tolerance = ServerToBattleUtility.ResistanceToTolerance(data);
		GrowStep growStep = MonsterGrowStepData.ToGrowStep(monsterGroupMasterByMonsterGroupId.growStep);
		foreach (ExtraEffectStatus extraEffectStatus in extraEffectStatusList)
		{
			foreach (MonsterData chipActer in chipPlayers)
			{
				if (chipActer != null)
				{
					ChipEffectStatus.TargetType targetType;
					if (chipActer.userMonster.userMonsterId == chipTarget.userMonster.userMonsterId)
					{
						targetType = ChipEffectStatus.TargetType.Actor;
					}
					else if (!chipEnemys.Where((MonsterData item) => item.userMonster.userMonsterId == chipActer.userMonster.userMonsterId).Any<MonsterData>())
					{
						if (!flag)
						{
							targetType = ChipEffectStatus.TargetType.Player;
						}
						else
						{
							targetType = ChipEffectStatus.TargetType.Enemy;
						}
					}
					else if (flag)
					{
						targetType = ChipEffectStatus.TargetType.Player;
					}
					else
					{
						targetType = ChipEffectStatus.TargetType.Enemy;
					}
					foreach (int num in chipActer.GetChipIdList())
					{
						GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] array = ChipDataMng.GetChipEffectData(num.ToString());
						array = ChipEffectStatus.GetStageEffectInvalidList(areaId, array, extraEffectStatus).ToArray();
						if (array.Length > 0)
						{
							List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> totalChipEffectStatusList = ChipEffectStatus.GetTotalChipEffectStatusList(array, flag, monsterIntegrationIds, chipTarget.monsterM.monsterGroupId, tolerance, monsterGroupMasterByMonsterGroupId.tribe, growStep, null, null, targetType, EffectStatusBase.ExtraEffectType.StageEffextInvalid);
							if (totalChipEffectStatusList.Count > 0)
							{
								return true;
							}
						}
					}
				}
			}
		}
		return false;
	}

	private static bool CheckStageEffectInvalid(CharacterStateControl chipTarget, ExtraEffectStatus extraEffectStatus)
	{
		BattleStateManager current = BattleStateManager.current;
		int areaId = current.hierarchyData.areaId;
		CharacterStateControl[] totalCharacters = current.battleStateData.GetTotalCharacters();
		foreach (CharacterStateControl characterStateControl in totalCharacters)
		{
			if (!(characterStateControl == null))
			{
				ChipEffectStatus.TargetType targetType = ChipEffectStatus.GetTargetType(characterStateControl, chipTarget);
				foreach (int num in characterStateControl.chipIds)
				{
					GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] array2 = ChipDataMng.GetChipEffectData(num.ToString());
					array2 = ChipEffectStatus.GetStageEffectInvalidList(BattleStateManager.current.hierarchyData.areaId, array2, extraEffectStatus).ToArray();
					if (array2.Length > 0)
					{
						List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> totalChipEffectStatusList = ChipEffectStatus.GetTotalChipEffectStatusList(array2, chipTarget.isEnemy, chipTarget.characterStatus.monsterIntegrationIds, chipTarget.groupId, chipTarget.tolerance, chipTarget.characterDatas.tribe, chipTarget.characterDatas.growStep, null, null, targetType, EffectStatusBase.ExtraEffectType.StageEffextInvalid);
						if (totalChipEffectStatusList.Count > 0)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	private static List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> GetStageEffectInvalidList(int areaId, GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, ExtraEffectStatus extraEffectStatus)
	{
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
		foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in chipEffects)
		{
			bool flag = false;
			bool flag2 = false;
			if (chipEffect.effectTrigger.ToInt32() == 11 && chipEffect.effectTriggerValue.ToInt32() == areaId)
			{
				flag = true;
			}
			if (chipEffect.effectType.ToInt32() == 80 && chipEffect.effectValue == extraEffectStatus.WorldDungeonExtraEffectId)
			{
				flag2 = true;
			}
			if (flag && flag2)
			{
				list.Add(chipEffect);
			}
		}
		return list;
	}

	private static ChipEffectStatus.TargetType GetTargetType(CharacterStateControl actor, CharacterStateControl target)
	{
		ChipEffectStatus.TargetType result;
		if (actor == target)
		{
			result = ChipEffectStatus.TargetType.Actor;
		}
		else if (!actor.isEnemy)
		{
			if (!target.isEnemy)
			{
				result = ChipEffectStatus.TargetType.Player;
			}
			else
			{
				result = ChipEffectStatus.TargetType.Enemy;
			}
		}
		else if (target.isEnemy)
		{
			result = ChipEffectStatus.TargetType.Player;
		}
		else
		{
			result = ChipEffectStatus.TargetType.Enemy;
		}
		return result;
	}

	public enum TargetType
	{
		Actor,
		Player,
		Enemy
	}
}
