using BattleStateMachineInternal;
using Monster;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChipEffectStatus : EffectStatusBase
{
	public static GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] GetInvocationList(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, EffectStatusBase.EffectTriggerType targetType, int monsterGroupId, CharacterStateControl characterStateControl, int areaId)
	{
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
		foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in chipEffects)
		{
			EffectStatusBase.EffectTriggerType effectTriggerType = (EffectStatusBase.EffectTriggerType)chipEffect.effectTrigger.ToInt32();
			if (targetType == effectTriggerType)
			{
				if (ChipEffectStatus.CheckInvocation(chipEffect.effectType, chipEffect.effectValue, chipEffect.effectTrigger, chipEffect.effectTriggerValue, targetType, monsterGroupId, characterStateControl, areaId) && ChipEffectStatus.CheckInvocation(chipEffect.effectType, chipEffect.effectValue, chipEffect.effectTrigger2, chipEffect.effectTriggerValue2, targetType, monsterGroupId, characterStateControl, areaId))
				{
					list.Add(chipEffect);
				}
			}
		}
		return list.ToArray();
	}

	private static bool CheckInvocation(string effectType, string effectValue, string effectTrigger, string effectTriggerValue, EffectStatusBase.EffectTriggerType targetType, int monsterGroupId, CharacterStateControl characterStateControl, int areaId)
	{
		if (effectType.ToInt32() == 56 && !ChipEffectStatus.CheckCounter(characterStateControl))
		{
			return false;
		}
		BattleStateManager current = BattleStateManager.current;
		switch (effectTrigger.ToInt32())
		{
		case 3:
		case 4:
		{
			int targetNumber = current.battleStateData.currentWaveNumber + 1;
			return ChipEffectStatus.CheckLoop(effectTriggerValue, targetNumber);
		}
		case 5:
			return ChipEffectStatus.CheckHpPercentage(effectTriggerValue, characterStateControl.hp, characterStateControl.extraMaxHp);
		case 7:
			return ChipEffectStatus.CheckHpFixed(effectTriggerValue, characterStateControl.hp);
		case 8:
		case 9:
		{
			int currentRoundNumber = current.battleStateData.currentRoundNumber;
			return ChipEffectStatus.CheckLoop(effectTriggerValue, currentRoundNumber);
		}
		case 10:
			return ChipEffectStatus.CheckKill(effectTriggerValue, characterStateControl);
		case 11:
			return ChipEffectStatus.CheckArea(effectTriggerValue, areaId);
		case 12:
			return ChipEffectStatus.CheckAttackStarted(effectType, effectValue, characterStateControl);
		case 13:
			return ChipEffectStatus.CheckSufferHit(effectTriggerValue, characterStateControl);
		case 15:
			return ChipEffectStatus.CheckSkillStartedApMax(effectType, effectValue, characterStateControl);
		case 16:
			return ChipEffectStatus.CheckAttackCommandedTarget(characterStateControl);
		case 17:
			return ChipEffectStatus.CheckSkillSpecies(effectTriggerValue, characterStateControl);
		case 18:
			return ChipEffectStatus.CheckSkillTargetSpecies(effectTriggerValue, characterStateControl);
		case 19:
			return ChipEffectStatus.CheckSkillStartedSendAttribute(effectTriggerValue, characterStateControl);
		case 20:
			return ChipEffectStatus.CheckSkillStartedRecieveAttribute(effectTriggerValue, characterStateControl);
		case 22:
			return ChipEffectStatus.CheckMonsterGroupId(effectTriggerValue, monsterGroupId);
		case 23:
			return ChipEffectStatus.CheckMonsterIntegrationGroupId(effectTriggerValue, characterStateControl.characterStatus.monsterIntegrationIds);
		case 24:
			return ChipEffectStatus.CheckSkillDamageSend(effectTriggerValue, characterStateControl);
		case 25:
			return ChipEffectStatus.CheckSkillDamageRecieve(effectTriggerValue, characterStateControl);
		case 26:
		case 27:
			return true;
		}
		return true;
	}

	private static bool CheckCounter(CharacterStateControl characterStateControl)
	{
		return !(characterStateControl == null) && !(characterStateControl == BattleStateManager.current.battleStateData.currentSelectCharacterState) && !characterStateControl.isDied && !characterStateControl.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Stun) && !characterStateControl.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Sleep);
	}

	private static bool CheckMonsterGroupId(string effectTriggerValue, int monsterGroupId)
	{
		int num = effectTriggerValue.ToInt32();
		return num == 0 || num == monsterGroupId;
	}

	private static bool CheckMonsterIntegrationGroupId(string effectTriggerValue, string[] monsterIntegrationIds)
	{
		return effectTriggerValue == "0" || monsterIntegrationIds.Where((string item) => item == effectTriggerValue).Any<string>();
	}

	private static bool CheckHpPercentage(string effectTriggerValue, int currentHp, int maxHp)
	{
		string[] array = effectTriggerValue.Split(new char[]
		{
			','
		});
		if (array.Length != 2)
		{
			return false;
		}
		int num = Mathf.FloorToInt((float)(maxHp * array[0].ToInt32()) / 100f);
		int num2 = Mathf.FloorToInt((float)(maxHp * array[1].ToInt32()) / 100f);
		return num <= currentHp && currentHp <= num2;
	}

	private static bool CheckHpFixed(string effectTriggerValue, int currentHp)
	{
		return effectTriggerValue.ToInt32() == currentHp;
	}

	private static bool CheckKill(string effectTriggerValue, CharacterStateControl characterStateControl)
	{
		if (characterStateControl != null)
		{
			if (!(effectTriggerValue == "1"))
			{
				return true;
			}
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
				return true;
			}
		}
		return false;
	}

	private static bool CheckArea(string effectTriggerValue, int areaId)
	{
		return areaId == effectTriggerValue.ToInt32();
	}

	private static bool CheckAttackStarted(string effectType, string effectValue, CharacterStateControl characterStateControl)
	{
		return characterStateControl != null && characterStateControl == BattleStateManager.current.battleStateData.currentSelectCharacterState && characterStateControl.currentSkillStatus.skillType == SkillType.Attack;
	}

	private static bool CheckSkillStartedApMax(string effectType, string effectValue, CharacterStateControl characterStateControl)
	{
		return characterStateControl != null && characterStateControl == BattleStateManager.current.battleStateData.currentSelectCharacterState && characterStateControl.ap == characterStateControl.maxAp;
	}

	private static bool CheckAttackCommandedTarget(CharacterStateControl characterStateControl)
	{
		if (characterStateControl != null && characterStateControl == BattleStateManager.current.battleStateData.currentSelectCharacterState && characterStateControl.currentSkillStatus.ThisSkillIsAttack)
		{
			CharacterStateControl[] targets = ChipEffectStatus.GetTargets(characterStateControl);
			if (targets.Where((CharacterStateControl item) => item.skillOrder < characterStateControl.skillOrder).Any<CharacterStateControl>())
			{
				return true;
			}
		}
		return false;
	}

	private static bool CheckSkillSpecies(string effectTriggerValue, CharacterStateControl characterStateControl)
	{
		return ChipEffectStatus.CheckSkillSendBase(effectTriggerValue, characterStateControl) && (effectTriggerValue == "0" || characterStateControl.characterDatas.tribe == effectTriggerValue);
	}

	private static bool CheckSkillTargetSpecies(string effectTriggerValue, CharacterStateControl characterStateControl)
	{
		if (ChipEffectStatus.CheckSkillSendBase(effectTriggerValue, characterStateControl))
		{
			string tribe = effectTriggerValue;
			if (tribe == "0")
			{
				return true;
			}
			CharacterStateControl[] targets = ChipEffectStatus.GetTargets(characterStateControl);
			if (targets.Where((CharacterStateControl item) => item.characterDatas.tribe == tribe).Any<CharacterStateControl>())
			{
				return true;
			}
		}
		return false;
	}

	private static bool CheckSkillStartedSendAttribute(string effectTriggerValue, CharacterStateControl characterStateControl)
	{
		return ChipEffectStatus.CheckSkillSendBase(effectTriggerValue, characterStateControl) && ChipEffectStatus.CheckSkillAttribute(effectTriggerValue, characterStateControl);
	}

	private static bool CheckSkillStartedRecieveAttribute(string effectTriggerValue, CharacterStateControl characterStateControl)
	{
		return ChipEffectStatus.CheckSkillRecieveBase(effectTriggerValue, characterStateControl) && ChipEffectStatus.CheckSkillAttribute(effectTriggerValue, characterStateControl);
	}

	private static bool CheckSkillAttribute(string effectTriggerValue, CharacterStateControl characterStateControl)
	{
		if (effectTriggerValue.ToInt32() == 0)
		{
			return true;
		}
		CharacterStateControl currentSelectCharacterState = BattleStateManager.current.battleStateData.currentSelectCharacterState;
		global::Attribute attribute = ServerToBattleUtility.IntToAttribute(effectTriggerValue.ToInt32());
		return currentSelectCharacterState.currentSkillStatus.attribute == attribute;
	}

	private static bool CheckSufferHit(string effectTriggerValue, CharacterStateControl characterStateControl)
	{
		if (characterStateControl != null)
		{
			string[] array = effectTriggerValue.Split(new char[]
			{
				','
			});
			List<SufferStateProperty.SufferType> list = new List<SufferStateProperty.SufferType>();
			for (int i = 0; i < array.Length; i++)
			{
				SufferStateProperty.SufferType item = (SufferStateProperty.SufferType)array[i].ToInt32();
				list.Add(item);
			}
			foreach (SufferStateProperty.SufferType sufferType in list)
			{
				foreach (SufferStateProperty sufferStateProperty in characterStateControl.hitSufferList)
				{
					if (sufferStateProperty.sufferTypeCache == sufferType)
					{
						return true;
					}
				}
			}
			return false;
		}
		return false;
	}

	private static bool CheckSkillDamageSend(string effectTriggerValue, CharacterStateControl characterStateControl)
	{
		return ChipEffectStatus.CheckSkillSendBase(effectTriggerValue, characterStateControl) && ChipEffectStatus.CheckSkillDamage(effectTriggerValue, characterStateControl);
	}

	private static bool CheckSkillDamageRecieve(string effectTriggerValue, CharacterStateControl characterStateControl)
	{
		return ChipEffectStatus.CheckSkillRecieveBase(effectTriggerValue, characterStateControl) && ChipEffectStatus.CheckSkillDamage(effectTriggerValue, characterStateControl);
	}

	private static bool CheckSkillDamage(string effectTriggerValue, CharacterStateControl characterStateControl)
	{
		CharacterStateControl currentSelectCharacterState = BattleStateManager.current.battleStateData.currentSelectCharacterState;
		return currentSelectCharacterState.currentSkillStatus.ThisSkillIsAttack;
	}

	private static bool CheckSkillSendBase(string effectTriggerValue, CharacterStateControl characterStateControl)
	{
		return characterStateControl != null && characterStateControl == BattleStateManager.current.battleStateData.currentSelectCharacterState;
	}

	private static bool CheckSkillRecieveBase(string effectTriggerValue, CharacterStateControl characterStateControl)
	{
		if (characterStateControl != null)
		{
			CharacterStateControl[] targets = ChipEffectStatus.GetTargets(characterStateControl);
			if (targets.Where((CharacterStateControl item) => item == characterStateControl).Any<CharacterStateControl>())
			{
				return true;
			}
		}
		return false;
	}

	private static CharacterStateControl[] GetTargets(CharacterStateControl characterStateControl)
	{
		List<CharacterStateControl> list = new List<CharacterStateControl>();
		CharacterStateControl currentSelectCharacterState = BattleStateManager.current.battleStateData.currentSelectCharacterState;
		if (currentSelectCharacterState.currentSkillStatus.numbers == EffectNumbers.Simple)
		{
			list.Add(characterStateControl.targetCharacter);
		}
		else
		{
			CharacterStateControl[] collection;
			if (currentSelectCharacterState.targetCharacter.isEnemy)
			{
				collection = BattleStateManager.current.battleStateData.enemies.Where((CharacterStateControl item) => !item.isDied).ToArray<CharacterStateControl>();
			}
			else
			{
				collection = BattleStateManager.current.battleStateData.playerCharacters.Where((CharacterStateControl item) => !item.isDied).ToArray<CharacterStateControl>();
			}
			list.AddRange(collection);
		}
		return list.ToArray();
	}

	private static bool CheckLoop(string effectTriggerValue, int targetNumber)
	{
		if (string.IsNullOrEmpty(effectTriggerValue) || effectTriggerValue == "0")
		{
			return true;
		}
		string[] array = effectTriggerValue.Split(new char[]
		{
			','
		});
		if (array.Length == 1)
		{
			if (array[0].ToInt32() == targetNumber)
			{
				return true;
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
				return true;
			}
		}
		return false;
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
		ChipEffectStatus.<GetChipEffectList>c__AnonStorey2E2 <GetChipEffectList>c__AnonStorey2E = new ChipEffectStatus.<GetChipEffectList>c__AnonStorey2E2();
		<GetChipEffectList>c__AnonStorey2E.effectType = effectType;
		<GetChipEffectList>c__AnonStorey2E.targetType = targetType;
		<GetChipEffectList>c__AnonStorey2E.isEnemy = isEnemy;
		<GetChipEffectList>c__AnonStorey2E.targetSubType = targetSubType;
		<GetChipEffectList>c__AnonStorey2E.targetValue = targetValue;
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
		if (chipEffects.Length == 0)
		{
			return list;
		}
		<GetChipEffectList>c__AnonStorey2E.searchEffectType = ((int x) => x == (int)<GetChipEffectList>c__AnonStorey2E.effectType);
		if (EffectStatusBase.ExtraEffectType.Atk <= <GetChipEffectList>c__AnonStorey2E.effectType && <GetChipEffectList>c__AnonStorey2E.effectType < EffectStatusBase.ExtraEffectType.AllStatus)
		{
			<GetChipEffectList>c__AnonStorey2E.searchEffectType = ((int x) => x == (int)<GetChipEffectList>c__AnonStorey2E.effectType || x == 27);
		}
		<GetChipEffectList>c__AnonStorey2E.searchTargetType = ((int x) => x == (int)<GetChipEffectList>c__AnonStorey2E.targetType || (!<GetChipEffectList>c__AnonStorey2E.isEnemy && x == 1) || (<GetChipEffectList>c__AnonStorey2E.isEnemy && x == 2));
		IEnumerable<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> enumerable;
		if (resistanceType == ConstValue.ResistanceType.NONE)
		{
			enumerable = chipEffects.Where((GameWebAPI.RespDataMA_ChipEffectM.ChipEffect x) => <GetChipEffectList>c__AnonStorey2E.searchTargetType(x.targetType.ToInt32()) && x.targetSubType.ToInt32() == (int)<GetChipEffectList>c__AnonStorey2E.targetSubType && x.targetValue.ToInt32() == <GetChipEffectList>c__AnonStorey2E.targetValue && <GetChipEffectList>c__AnonStorey2E.searchEffectType(x.effectType.ToInt32()));
		}
		else
		{
			ChipEffectStatus.<GetChipEffectList>c__AnonStorey2E3 <GetChipEffectList>c__AnonStorey2E2 = new ChipEffectStatus.<GetChipEffectList>c__AnonStorey2E3();
			<GetChipEffectList>c__AnonStorey2E2.<>f__ref$738 = <GetChipEffectList>c__AnonStorey2E;
			ChipEffectStatus.<GetChipEffectList>c__AnonStorey2E3 <GetChipEffectList>c__AnonStorey2E3 = <GetChipEffectList>c__AnonStorey2E2;
			int num = (int)resistanceType;
			<GetChipEffectList>c__AnonStorey2E3.resistance = num.ToString();
			enumerable = chipEffects.Where((GameWebAPI.RespDataMA_ChipEffectM.ChipEffect x) => <GetChipEffectList>c__AnonStorey2E2.<>f__ref$738.searchTargetType(x.targetType.ToInt32()) && x.targetSubType.ToInt32() == (int)<GetChipEffectList>c__AnonStorey2E2.<>f__ref$738.targetSubType && x.targetValue.ToInt32() == <GetChipEffectList>c__AnonStorey2E2.<>f__ref$738.targetValue && x.targetValue2.Contains(<GetChipEffectList>c__AnonStorey2E2.resistance) && <GetChipEffectList>c__AnonStorey2E2.<>f__ref$738.searchEffectType(x.effectType.ToInt32()));
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

	public static List<ExtraEffectStatus> CheckStageEffectInvalid(List<ExtraEffectStatus> extraEffectStatusList, CharacterStateControl characterStateControl)
	{
		List<ExtraEffectStatus> list = new List<ExtraEffectStatus>();
		foreach (ExtraEffectStatus extraEffectStatus in extraEffectStatusList)
		{
			if (!ChipEffectStatus.CheckStageEffectInvalid(characterStateControl, extraEffectStatus))
			{
				list.Add(extraEffectStatus);
			}
		}
		return list;
	}

	public static List<ExtraEffectStatus> CheckStageEffectInvalid(int areaId, List<ExtraEffectStatus> extraEffectStatusList, MonsterData[] chipPlayers, MonsterData[] chipEnemys, MonsterData chipTarget)
	{
		List<ExtraEffectStatus> list = new List<ExtraEffectStatus>();
		bool flag = chipEnemys.Where((MonsterData item) => item.userMonster.userMonsterId == chipTarget.userMonster.userMonsterId).Any<MonsterData>();
		GameWebAPI.RespDataMA_GetMonsterMG.MonsterM group = MonsterMaster.GetMonsterMasterByMonsterGroupId(chipTarget.monsterM.monsterGroupId).Group;
		GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster responseMonsterIntegrationGroupMaster = MasterDataMng.Instance().ResponseMonsterIntegrationGroupMaster;
		GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup[] source = responseMonsterIntegrationGroupMaster.monsterIntegrationGroupM.Where((GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup item) => item.monsterId == chipTarget.monsterM.monsterId).ToArray<GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup>();
		string[] monsterIntegrationIds = source.Select((GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup item) => item.monsterIntegrationId).ToArray<string>();
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM resistanceMaster = MonsterResistanceData.GetResistanceMaster(chipTarget.monsterM.resistanceId);
		List<GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM> uniqueResistanceList = MonsterResistanceData.GetUniqueResistanceList(chipTarget.GetResistanceIdList());
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM data = MonsterResistanceData.AddResistanceFromMultipleTranceData(resistanceMaster, uniqueResistanceList);
		Tolerance tolerance = ServerToBattleUtility.ResistanceToTolerance(data);
		GrowStep growStep = MonsterGrowStepData.ToGrowStep(group.growStep);
		foreach (ExtraEffectStatus extraEffectStatus in extraEffectStatusList)
		{
			bool flag2 = true;
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
					foreach (int num in chipActer.GetChipEquip().GetChipIdList())
					{
						GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] array = ChipDataMng.GetChipEffectData(num.ToString());
						array = ChipEffectStatus.GetStageEffectInvalidList(areaId, array, extraEffectStatus).ToArray();
						if (array.Length > 0)
						{
							List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> totalChipEffectStatusList = ChipEffectStatus.GetTotalChipEffectStatusList(array, flag, monsterIntegrationIds, chipTarget.monsterM.monsterGroupId, tolerance, group.tribe, growStep, null, null, targetType, EffectStatusBase.ExtraEffectType.StageEffextInvalid);
							if (totalChipEffectStatusList.Count > 0)
							{
								flag2 = false;
								break;
							}
						}
					}
				}
			}
			if (flag2)
			{
				list.Add(extraEffectStatus);
			}
		}
		return list;
	}

	private static bool CheckStageEffectInvalid(CharacterStateControl chipTarget, ExtraEffectStatus extraEffectStatus)
	{
		BattleStateManager current = BattleStateManager.current;
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
