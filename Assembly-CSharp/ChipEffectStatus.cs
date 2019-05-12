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
				int num = 10000;
				if (chipEffect.lot != null && chipEffect.lot.Length > 0)
				{
					num = chipEffect.lot.ToInt32();
				}
				if (num > 0)
				{
					if (num != 10000)
					{
						int num2 = UnityEngine.Random.Range(0, 10000);
						if (num2 > num)
						{
							goto IL_119;
						}
					}
					if (ChipEffectStatus.CheckInvocation(chipEffect.targetType, chipEffect.targetSubType, chipEffect.targetValue, chipEffect.targetValue2, chipEffect.effectType, chipEffect.effectValue, chipEffect.effectTrigger, chipEffect.effectTriggerValue, monsterGroupId, characterStateControl, areaId) && ChipEffectStatus.CheckInvocation(chipEffect.targetType, chipEffect.targetSubType, chipEffect.targetValue, chipEffect.targetValue2, chipEffect.effectType, chipEffect.effectValue, chipEffect.effectTrigger2, chipEffect.effectTriggerValue2, monsterGroupId, characterStateControl, areaId))
					{
						list.Add(chipEffect);
					}
				}
			}
			IL_119:;
		}
		return list.ToArray();
	}

	private static bool CheckInvocation(string targetType, string targetSubType, string targetValue, string targetValue2, string effectType, string effectValue, string effectTrigger, string effectTriggerValue, int monsterGroupId, CharacterStateControl characterStateControl, int areaId)
	{
		if (effectType.ToInt32() == 56 && !ChipEffectStatus.CheckCounter(characterStateControl))
		{
			return false;
		}
		switch (effectTrigger.ToInt32())
		{
		case 3:
		case 4:
			return ChipEffectStatus.CheckWaveLoop(effectTriggerValue);
		case 5:
			return ChipEffectStatus.CheckHpPercentage(effectTriggerValue, characterStateControl.hp, characterStateControl.extraMaxHp);
		case 7:
			return ChipEffectStatus.CheckHpFixed(effectTriggerValue, characterStateControl.hp);
		case 8:
		case 9:
			return ChipEffectStatus.CheckRoundLoop(effectTriggerValue);
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
			return ChipEffectStatus.CheckSkillStartedSendAttribute(targetSubType, targetValue, targetValue2, effectTriggerValue, characterStateControl);
		case 20:
			return ChipEffectStatus.CheckSkillStartedRecieveAttribute(targetSubType, targetValue, targetValue2, effectTriggerValue, characterStateControl);
		case 22:
			return ChipEffectStatus.CheckMonsterGroupId(effectTriggerValue, monsterGroupId);
		case 23:
			return ChipEffectStatus.CheckMonsterIntegrationGroupId(effectTriggerValue, characterStateControl.characterStatus.monsterIntegrationIds);
		case 24:
			return ChipEffectStatus.CheckSkillDamageSend(effectTriggerValue, characterStateControl);
		case 25:
			return ChipEffectStatus.CheckSkillDamageRecieve(effectTriggerValue, characterStateControl);
		case 28:
		case 29:
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
			CharacterStateControl[] targets = ChipEffectStatus.GetTargets();
			if (targets.Where((CharacterStateControl item) => item.skillOrder < characterStateControl.skillOrder).Any<CharacterStateControl>())
			{
				return true;
			}
		}
		return false;
	}

	private static bool CheckSkillSpecies(string effectTriggerValue, CharacterStateControl characterStateControl)
	{
		return ChipEffectStatus.CheckSkillSendBase(characterStateControl) && (effectTriggerValue == "0" || characterStateControl.characterDatas.tribe == effectTriggerValue);
	}

	private static bool CheckSkillTargetSpecies(string effectTriggerValue, CharacterStateControl characterStateControl)
	{
		if (ChipEffectStatus.CheckSkillDamageSend(effectTriggerValue, characterStateControl))
		{
			string tribe = effectTriggerValue;
			if (tribe == "0")
			{
				return true;
			}
			CharacterStateControl[] targets = ChipEffectStatus.GetTargets();
			if (targets.Where((CharacterStateControl item) => item.characterDatas.tribe == tribe).Any<CharacterStateControl>())
			{
				return true;
			}
		}
		return false;
	}

	private static bool CheckSkillStartedSendAttribute(string targetSubType, string targetValue, string targetValue2, string effectTriggerValue, CharacterStateControl characterStateControl)
	{
		return ChipEffectStatus.CheckSkillDamageSend(effectTriggerValue, characterStateControl) && ChipEffectStatus.CheckSkillAttribute(targetSubType, targetValue, targetValue2, effectTriggerValue, characterStateControl);
	}

	private static bool CheckSkillStartedRecieveAttribute(string targetSubType, string targetValue, string targetValue2, string effectTriggerValue, CharacterStateControl characterStateControl)
	{
		return ChipEffectStatus.CheckSkillDamageRecieve(effectTriggerValue, characterStateControl) && ChipEffectStatus.CheckSkillAttribute(targetSubType, targetValue, targetValue2, effectTriggerValue, characterStateControl);
	}

	private static bool CheckSkillAttribute(string targetSubType, string targetValue, string targetValue2, string effectTriggerValue, CharacterStateControl characterStateControl)
	{
		if (effectTriggerValue.ToInt32() == 0)
		{
			return true;
		}
		if (targetSubType.ToInt32() != 9)
		{
			CharacterStateControl currentSelectCharacterState = BattleStateManager.current.battleStateData.currentSelectCharacterState;
			global::Attribute attribute = ServerToBattleUtility.IntToAttribute(effectTriggerValue.ToInt32());
			return currentSelectCharacterState.currentSkillStatus.attribute == attribute;
		}
		CharacterStateControl currentSelectCharacterState2 = BattleStateManager.current.battleStateData.currentSelectCharacterState;
		global::Attribute attribute2 = ServerToBattleUtility.IntToAttribute(targetValue.ToInt32());
		if (targetValue2 == "0")
		{
			return currentSelectCharacterState2.currentSkillStatus.attribute == attribute2;
		}
		if (targetValue2 == "1")
		{
			return currentSelectCharacterState2.currentSkillStatus.attribute == attribute2 && currentSelectCharacterState2.currentSkillStatus.skillType == SkillType.Attack;
		}
		if (targetValue2 == "2")
		{
			return currentSelectCharacterState2.currentSkillStatus.attribute == attribute2 && currentSelectCharacterState2.currentSkillStatus.skillType != SkillType.Attack;
		}
		return currentSelectCharacterState2.currentSkillStatus.attribute == attribute2;
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
		return ChipEffectStatus.CheckSkillSendBase(characterStateControl) && ChipEffectStatus.CheckSkillDamage(effectTriggerValue, characterStateControl);
	}

	private static bool CheckSkillDamageRecieve(string effectTriggerValue, CharacterStateControl characterStateControl)
	{
		return ChipEffectStatus.CheckSkillRecieveBase(characterStateControl) && ChipEffectStatus.CheckSkillDamage(effectTriggerValue, characterStateControl);
	}

	private static bool CheckSkillDamage(string effectTriggerValue, CharacterStateControl characterStateControl)
	{
		CharacterStateControl currentSelectCharacterState = BattleStateManager.current.battleStateData.currentSelectCharacterState;
		return currentSelectCharacterState.currentSkillStatus.ThisSkillIsAttack;
	}

	private static bool CheckSkillSendBase(CharacterStateControl characterStateControl)
	{
		return characterStateControl != null && characterStateControl == BattleStateManager.current.battleStateData.currentSelectCharacterState;
	}

	private static bool CheckSkillRecieveBase(CharacterStateControl characterStateControl)
	{
		if (characterStateControl != null)
		{
			CharacterStateControl[] targets = ChipEffectStatus.GetTargets();
			if (targets.Where((CharacterStateControl item) => item == characterStateControl).Any<CharacterStateControl>())
			{
				return true;
			}
		}
		return false;
	}

	private static CharacterStateControl[] GetTargets()
	{
		List<CharacterStateControl> list = new List<CharacterStateControl>();
		CharacterStateControl currentSelectCharacterState = BattleStateManager.current.battleStateData.currentSelectCharacterState;
		if (currentSelectCharacterState.currentSkillStatus.numbers == EffectNumbers.Simple)
		{
			list.Add(currentSelectCharacterState.targetCharacter);
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

	private static bool CheckWaveLoop(string effectTriggerValue)
	{
		int targetNumber = BattleStateManager.current.battleStateData.currentWaveNumber + 1;
		return ChipEffectStatus.CheckLoop(effectTriggerValue, targetNumber);
	}

	private static bool CheckRoundLoop(string effectTriggerValue)
	{
		int currentRoundNumber = BattleStateManager.current.battleStateData.currentRoundNumber;
		return ChipEffectStatus.CheckLoop(effectTriggerValue, currentRoundNumber);
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
		return (int)ChipEffectStatus.GetChipEffectValueToFloat(chipEffects, (float)baseValue, character.isEnemy, character.characterStatus.monsterIntegrationIds, character.groupId, character.tolerance, character.characterDatas.tribe, character.characterDatas.growStep, null, character.currentSufferState, ChipEffectStatus.GetTargetType(character, character), effectType, 0);
	}

	public static float GetChipEffectValueToFloat(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, float baseValue, CharacterStateControl character, EffectStatusBase.ExtraEffectType effectType)
	{
		return ChipEffectStatus.GetChipEffectValueToFloat(chipEffects, baseValue, character.isEnemy, character.characterStatus.monsterIntegrationIds, character.groupId, character.tolerance, character.characterDatas.tribe, character.characterDatas.growStep, null, character.currentSufferState, ChipEffectStatus.GetTargetType(character, character), effectType, 0);
	}

	public static float GetChipEffectValueToFloat(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, float baseValue, bool isEnemy, string[] monsterIntegrationIds, string groupId, Tolerance tolerance, string tribe, GrowStep growStep, AffectEffectProperty skillPropety, HaveSufferState currentSufferState, ChipEffectStatus.TargetType targetType, EffectStatusBase.ExtraEffectType effectType, int attackNum = 0)
	{
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> totalChipEffectStatusList = ChipEffectStatus.GetTotalChipEffectStatusList(chipEffects, isEnemy, monsterIntegrationIds, groupId, tolerance, tribe, growStep, skillPropety, currentSufferState, targetType, effectType);
		if (totalChipEffectStatusList.Count > 0)
		{
			float correctionValue = ChipEffectStatus.GetCorrectionValue(baseValue, totalChipEffectStatusList, attackNum);
			return correctionValue - baseValue;
		}
		return 0f;
	}

	private static List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> GetTotalChipEffectStatusList(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, bool isEnemy, string[] monsterIntegrationIds, string groupId, Tolerance tolerance, string tribe, GrowStep growStep, AffectEffectProperty skillPropety, HaveSufferState currentSufferState, ChipEffectStatus.TargetType targetType, EffectStatusBase.ExtraEffectType effectType)
	{
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, EffectStatusBase.ExtraTargetSubType.Non, 0, isEnemy, effectType));
		if (skillPropety != null)
		{
			bool flag = skillPropety.skillId.ToString() == BattleStateManager.PublicAttackSkillId;
			List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list2 = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
			foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in chipEffects)
			{
				if (chipEffect.targetValue2 == "0")
				{
					list2.Add(chipEffect);
				}
				else if (chipEffect.targetValue2 == "1")
				{
					if (flag)
					{
						list2.Add(chipEffect);
					}
				}
				else if (chipEffect.targetValue2 == "2" && !flag)
				{
					list2.Add(chipEffect);
				}
			}
			ConstValue.ResistanceType skillResistanceType = EffectStatusBase.GetSkillResistanceType(skillPropety);
			list.AddRange(ChipEffectStatus.GetChipEffectList(list2.ToArray(), targetType, EffectStatusBase.ExtraTargetSubType.SkillAttribute, 0, isEnemy, effectType));
			list.AddRange(ChipEffectStatus.GetChipEffectList(list2.ToArray(), targetType, EffectStatusBase.ExtraTargetSubType.SkillAttribute, (int)skillResistanceType, isEnemy, effectType));
			list.AddRange(ChipEffectStatus.GetChipEffectList(list2.ToArray(), targetType, EffectStatusBase.ExtraTargetSubType.SkillId, 0, isEnemy, effectType));
			list.AddRange(ChipEffectStatus.GetChipEffectList(list2.ToArray(), targetType, EffectStatusBase.ExtraTargetSubType.SkillId, skillPropety.skillId, isEnemy, effectType));
		}
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, EffectStatusBase.ExtraTargetSubType.MonsterResistance, 0, isEnemy, effectType));
		List<ConstValue.ResistanceType> attributeStrengthList = tolerance.GetAttributeStrengthList();
		foreach (ConstValue.ResistanceType targetValue in attributeStrengthList)
		{
			list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, EffectStatusBase.ExtraTargetSubType.MonsterResistance, (int)targetValue, isEnemy, effectType));
		}
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, EffectStatusBase.ExtraTargetSubType.MonsterTribe, 0, isEnemy, effectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, EffectStatusBase.ExtraTargetSubType.MonsterTribe, tribe.ToInt32(), isEnemy, effectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, EffectStatusBase.ExtraTargetSubType.MonsterGroup, 0, isEnemy, effectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, EffectStatusBase.ExtraTargetSubType.MonsterGroup, groupId.ToInt32(), isEnemy, effectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, EffectStatusBase.ExtraTargetSubType.GrowStep, 0, isEnemy, effectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, EffectStatusBase.ExtraTargetSubType.GrowStep, (int)growStep, isEnemy, effectType));
		if (currentSufferState != null)
		{
			list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, EffectStatusBase.ExtraTargetSubType.Suffer, 0, isEnemy, effectType));
			foreach (object obj in Enum.GetValues(typeof(SufferStateProperty.SufferType)))
			{
				SufferStateProperty.SufferType sufferType = (SufferStateProperty.SufferType)((int)obj);
				if (sufferType != SufferStateProperty.SufferType.Null)
				{
					if (currentSufferState.FindSufferState(sufferType))
					{
						list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, EffectStatusBase.ExtraTargetSubType.Suffer, (int)sufferType, isEnemy, effectType));
					}
				}
			}
		}
		list.AddRange(ChipEffectStatus.GetMonsterIntegrationGroupList(chipEffects, isEnemy, monsterIntegrationIds, targetType, effectType));
		return list;
	}

	public static int GetSkillPowerCorrectionValue(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, AffectEffectProperty skillPropety, CharacterStateControl character, int attackNum = 0)
	{
		return (int)ChipEffectStatus.GetChipEffectValueToFloat(chipEffects, (float)skillPropety.GetPower(character), character.isEnemy, character.characterStatus.monsterIntegrationIds, character.groupId, character.tolerance, character.characterDatas.tribe, character.characterDatas.growStep, skillPropety, character.currentSufferState, ChipEffectStatus.GetTargetType(character, character), EffectStatusBase.ExtraEffectType.SkillPower, attackNum);
	}

	public static float GetSkillHitRateCorrectionValue(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, AffectEffectProperty skillPropety, CharacterStateControl character)
	{
		return ChipEffectStatus.GetChipEffectValueToFloat(chipEffects, skillPropety.hitRate, character.isEnemy, character.characterStatus.monsterIntegrationIds, character.groupId, character.tolerance, character.characterDatas.tribe, character.characterDatas.growStep, skillPropety, character.currentSufferState, ChipEffectStatus.GetTargetType(character, character), EffectStatusBase.ExtraEffectType.SkillHit, 0);
	}

	public static float GetSkillDamageCorrectionValue(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, AffectEffectProperty skillPropety, CharacterStateControl character)
	{
		return ChipEffectStatus.GetChipEffectValueToFloat(chipEffects, 1f, character.isEnemy, character.characterStatus.monsterIntegrationIds, character.groupId, character.tolerance, character.characterDatas.tribe, character.characterDatas.growStep, skillPropety, character.currentSufferState, ChipEffectStatus.GetTargetType(character, character), EffectStatusBase.ExtraEffectType.SkillDamage, 0);
	}

	private static float GetCorrectionValue(float baseValue, List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> extraEffectStatusList, int attackNum = 0)
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
			else if (chipEffect.effectSubType.ToInt32() == 13 && attackNum > 0)
			{
				float num5 = Mathf.Floor(baseValue / Mathf.Pow((float)attackNum, 1.8f) * chipEffect.effectValue.ToFloat());
				num4 += num5;
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

	private static List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> GetChipEffectList(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, ChipEffectStatus.TargetType targetType, EffectStatusBase.ExtraTargetSubType targetSubType, int targetValue, bool isEnemy, EffectStatusBase.ExtraEffectType effectType)
	{
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
		if (chipEffects.Length == 0)
		{
			return list;
		}
		Func<int, bool> searchEffectType = (int x) => x == (int)effectType;
		if (EffectStatusBase.ExtraEffectType.Atk <= effectType && effectType < EffectStatusBase.ExtraEffectType.AllStatus)
		{
			searchEffectType = ((int x) => x == (int)effectType || x == 27);
		}
		Func<int, bool> searchTargetType = (int x) => x == (int)targetType || (!isEnemy && x == 1) || (isEnemy && x == 2);
		IEnumerable<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> enumerable = chipEffects.Where((GameWebAPI.RespDataMA_ChipEffectM.ChipEffect x) => searchTargetType(x.targetType.ToInt32()) && x.targetSubType.ToInt32() == (int)targetSubType && x.targetValue.ToInt32() == targetValue && searchEffectType(x.effectType.ToInt32()));
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
				List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> chipEffectList = ChipEffectStatus.GetChipEffectList(list3.ToArray(), targetType, EffectStatusBase.ExtraTargetSubType.MonsterIntegrationGroup, chipEffect2.targetValue.ToInt32(), isEnemy, effectType);
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
