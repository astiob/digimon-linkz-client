using BattleStateMachineInternal;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ExtraEffectUtil
{
	public static bool IsExtraEffectMonster(MonsterData monsterData, GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM[] effectArray)
	{
		int triggerValue = 0;
		if (CMD_MultiRecruitPartyWait.StageDataBk != null)
		{
			triggerValue = CMD_MultiRecruitPartyWait.StageDataBk.worldAreaId.ToInt32();
		}
		else if (CMD_QuestTOP.instance != null && CMD_QuestTOP.instance.StageDataBk != null)
		{
			GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM[] worldStageM = MasterDataMng.Instance().RespDataMA_WorldStageM.worldStageM;
			foreach (GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM worldStageM2 in worldStageM)
			{
				if (CMD_QuestTOP.instance.StageDataBk.worldDungeonM.worldStageId == worldStageM2.worldStageId)
				{
					triggerValue = worldStageM2.worldAreaId.ToInt32();
					break;
				}
			}
		}
		foreach (int num in monsterData.GetChipIdList())
		{
			GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffectData = ChipDataMng.GetChipEffectData(num.ToString());
			if (chipEffectData != null)
			{
				GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] invocationList = ChipEffectStatus.GetInvocationList(chipEffectData, ChipEffectStatus.EffectTriggerType.Area, monsterData.monsterM.monsterGroupId.ToInt32(), null, triggerValue);
				if (invocationList.Length > 0)
				{
					return true;
				}
			}
		}
		int num2;
		int num3;
		return effectArray != null && effectArray.Length != 0 && (ExtraEffectUtil.GetExtraEffectFluctuationValue(out num2, out num3, monsterData, effectArray, ExtraEffectUtil.EffectType.Atk, 0) || ExtraEffectUtil.GetExtraEffectFluctuationValue(out num2, out num3, monsterData, effectArray, ExtraEffectUtil.EffectType.Def, 0) || ExtraEffectUtil.GetExtraEffectFluctuationValue(out num2, out num3, monsterData, effectArray, ExtraEffectUtil.EffectType.Hp, 0) || ExtraEffectUtil.GetExtraEffectFluctuationValue(out num2, out num3, monsterData, effectArray, ExtraEffectUtil.EffectType.Spd, 0) || ExtraEffectUtil.GetExtraEffectFluctuationValue(out num2, out num3, monsterData, effectArray, ExtraEffectUtil.EffectType.Satk, 0) || ExtraEffectUtil.GetExtraEffectFluctuationValue(out num2, out num3, monsterData, effectArray, ExtraEffectUtil.EffectType.Sdef, 0) || ExtraEffectUtil.GetExtraEffectFluctuationValue(out num2, out num3, monsterData, effectArray, ExtraEffectUtil.EffectType.SkillDamage, 1) || ExtraEffectUtil.GetExtraEffectFluctuationValue(out num2, out num3, monsterData, effectArray, ExtraEffectUtil.EffectType.SkillDamage, 2) || ExtraEffectUtil.GetExtraEffectFluctuationValue(out num2, out num3, monsterData, effectArray, ExtraEffectUtil.EffectType.SkillHitRate, 1) || ExtraEffectUtil.GetExtraEffectFluctuationValue(out num2, out num3, monsterData, effectArray, ExtraEffectUtil.EffectType.SkillHitRate, 2));
	}

	public static bool GetExtraEffectFluctuationValue(out int res, out int change, MonsterData monsterData, GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM[] effectArray, ExtraEffectUtil.EffectType effectType, int skillType = 0)
	{
		int extraChipValue = ExtraEffectUtil.GetExtraChipValue(monsterData, effectType);
		if (effectArray == null || effectArray.Length == 0)
		{
			res = (int)ExtraEffectUtil.GetStatusValue(effectType, monsterData, skillType);
			res += extraChipValue;
			change = 0;
			if (extraChipValue > 0)
			{
				change = 1;
			}
			else if (extraChipValue < 0)
			{
				change = -1;
			}
			return false;
		}
		List<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM> list = new List<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM>();
		List<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM> list2 = new List<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM>();
		foreach (GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM worldDungeonExtraEffectM in effectArray)
		{
			if (worldDungeonExtraEffectM.effectType == effectType.ToInteger().ToString() || (worldDungeonExtraEffectM.effectType == ExtraEffectUtil.EffectType.All.ToInteger().ToString() && effectType != ExtraEffectUtil.EffectType.SkillDamage && effectType != ExtraEffectUtil.EffectType.SkillHitRate))
			{
				list.Add(worldDungeonExtraEffectM);
			}
		}
		if (list.Count == 0)
		{
			res = (int)ExtraEffectUtil.GetStatusValue(effectType, monsterData, skillType);
			res += extraChipValue;
			change = 0;
			if (extraChipValue > 0)
			{
				change = 1;
			}
			else if (extraChipValue < 0)
			{
				change = -1;
			}
			return false;
		}
		list2 = new List<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM>(list);
		list = new List<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM>();
		foreach (GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM worldDungeonExtraEffectM2 in list2)
		{
			if (worldDungeonExtraEffectM2.targetType == ExtraEffectUtil.TargetType.All.ToInteger().ToString() || worldDungeonExtraEffectM2.targetType == ExtraEffectUtil.TargetType.Player.ToInteger().ToString())
			{
				list.Add(worldDungeonExtraEffectM2);
			}
		}
		if (list.Count == 0)
		{
			res = (int)ExtraEffectUtil.GetStatusValue(effectType, monsterData, skillType);
			res += extraChipValue;
			change = 0;
			if (extraChipValue > 0)
			{
				change = 1;
			}
			else if (extraChipValue < 0)
			{
				change = -1;
			}
			return false;
		}
		list2 = new List<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM>(list);
		list = new List<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM>();
		foreach (GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM worldDungeonExtraEffectM3 in list2)
		{
			switch (int.Parse(worldDungeonExtraEffectM3.targetSubType))
			{
			case 1:
				if ((monsterData.resistanceM.monsterResistanceId == worldDungeonExtraEffectM3.targetValue && ExtraEffectUtil.IsMatchResistance(monsterData.resistanceM, worldDungeonExtraEffectM3.targetValue2)) || ("0" == worldDungeonExtraEffectM3.targetValue && ExtraEffectUtil.IsMatchResistance(monsterData.resistanceM, worldDungeonExtraEffectM3.targetValue2)))
				{
					list.Add(worldDungeonExtraEffectM3);
				}
				break;
			case 2:
				if (worldDungeonExtraEffectM3.targetValue == "0" || worldDungeonExtraEffectM3.targetValue == monsterData.tribeM.monsterTribeId)
				{
					list.Add(worldDungeonExtraEffectM3);
				}
				break;
			case 3:
				if (worldDungeonExtraEffectM3.targetValue == "0" || worldDungeonExtraEffectM3.targetValue == monsterData.monsterM.monsterGroupId)
				{
					list.Add(worldDungeonExtraEffectM3);
				}
				break;
			case 4:
				if (worldDungeonExtraEffectM3.targetValue == "0" || worldDungeonExtraEffectM3.targetValue == monsterData.growStepM.monsterGrowStepId)
				{
					list.Add(worldDungeonExtraEffectM3);
				}
				break;
			case 6:
				if (skillType == 1)
				{
					if ((monsterData.actionSkillM.skillId == worldDungeonExtraEffectM3.targetValue && (worldDungeonExtraEffectM3.targetValue2 == "0" || worldDungeonExtraEffectM3.targetValue2 == monsterData.actionSkillDetailM.attribute.ToString())) || ("0" == worldDungeonExtraEffectM3.targetValue && (worldDungeonExtraEffectM3.targetValue2 == "0" || worldDungeonExtraEffectM3.targetValue2 == monsterData.actionSkillDetailM.attribute.ToString())))
					{
						list.Add(worldDungeonExtraEffectM3);
					}
				}
				else if (skillType == 2 && ((monsterData.commonSkillM.skillId == worldDungeonExtraEffectM3.targetValue && (worldDungeonExtraEffectM3.targetValue2 == "0" || worldDungeonExtraEffectM3.targetValue2 == monsterData.commonSkillDetailM.attribute.ToString())) || ("0" == worldDungeonExtraEffectM3.targetValue && (worldDungeonExtraEffectM3.targetValue2 == "0" || worldDungeonExtraEffectM3.targetValue2 == monsterData.commonSkillDetailM.attribute.ToString()))))
				{
					list.Add(worldDungeonExtraEffectM3);
				}
				break;
			}
		}
		if (list.Count == 0)
		{
			res = (int)ExtraEffectUtil.GetStatusValue(effectType, monsterData, skillType);
			res += extraChipValue;
			change = 0;
			if (extraChipValue > 0)
			{
				change = 1;
			}
			else if (extraChipValue < 0)
			{
				change = -1;
			}
			return false;
		}
		list2 = new List<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM>(list);
		list = new List<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM>();
		foreach (GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM worldDungeonExtraEffectM4 in list2)
		{
			if (worldDungeonExtraEffectM4.effectSubType == "2")
			{
				list.Add(worldDungeonExtraEffectM4);
			}
		}
		foreach (GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM worldDungeonExtraEffectM5 in list2)
		{
			if (worldDungeonExtraEffectM5.effectSubType == "3")
			{
				list.Add(worldDungeonExtraEffectM5);
			}
		}
		foreach (GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM worldDungeonExtraEffectM6 in list2)
		{
			if (worldDungeonExtraEffectM6.effectSubType == "1")
			{
				list.Add(worldDungeonExtraEffectM6);
			}
		}
		float statusValue = ExtraEffectUtil.GetStatusValue(effectType, monsterData, skillType);
		float num = statusValue;
		foreach (GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM worldDungeonExtraEffectM7 in list)
		{
			string effectSubType = worldDungeonExtraEffectM7.effectSubType;
			switch (effectSubType)
			{
			case "1":
				num = float.Parse(worldDungeonExtraEffectM7.effectValue);
				break;
			case "2":
				num += num * (float.Parse(worldDungeonExtraEffectM7.effectValue) * 0.01f);
				break;
			case "3":
				num += float.Parse(worldDungeonExtraEffectM7.effectValue);
				break;
			}
		}
		if (effectType != ExtraEffectUtil.EffectType.SkillDamage && effectType != ExtraEffectUtil.EffectType.SkillHitRate)
		{
			num = Mathf.Clamp(num, 1f, num);
		}
		num += (float)extraChipValue;
		if (statusValue < num)
		{
			change = 1;
		}
		else if (statusValue > num)
		{
			change = -1;
		}
		else
		{
			change = 0;
		}
		res = (int)num;
		return true;
	}

	private static bool IsMatchResistance(GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM resistanceM, string targetValue2)
	{
		if (targetValue2 == "0")
		{
			return true;
		}
		switch (targetValue2)
		{
		case "1":
			return resistanceM.none == "1";
		case "2":
			return resistanceM.fire == "1";
		case "3":
			return resistanceM.water == "1";
		case "4":
			return resistanceM.thunder == "1";
		case "5":
			return resistanceM.nature == "1";
		case "6":
			return resistanceM.light == "1";
		case "7":
			return resistanceM.dark == "1";
		case "8":
			return resistanceM.stun == "1";
		case "9":
			return resistanceM.skillLock == "1";
		case "10":
			return resistanceM.sleep == "1";
		case "11":
			return resistanceM.paralysis == "1";
		case "12":
			return resistanceM.confusion == "1";
		case "13":
			return resistanceM.poison == "1";
		case "14":
			return resistanceM.death == "1";
		}
		return false;
	}

	private static float GetStatusValue(ExtraEffectUtil.EffectType type, MonsterData data, int skillType = 0)
	{
		switch (type)
		{
		case ExtraEffectUtil.EffectType.SkillDamage:
		{
			float result = 0f;
			if (skillType == 1)
			{
				result = (float)data.actionSkillDetailM.effect2;
			}
			else if (skillType == 2)
			{
				result = (float)data.commonSkillDetailM.effect2;
			}
			return result;
		}
		case ExtraEffectUtil.EffectType.SkillHitRate:
		{
			float result2 = 0f;
			if (skillType == 1)
			{
				result2 = (float)data.actionSkillDetailM.hitRate;
			}
			else if (skillType == 2)
			{
				result2 = (float)data.commonSkillDetailM.hitRate;
			}
			return result2;
		}
		case ExtraEffectUtil.EffectType.Atk:
			return float.Parse(data.userMonster.attack);
		case ExtraEffectUtil.EffectType.Def:
			return float.Parse(data.userMonster.defense);
		case ExtraEffectUtil.EffectType.Hp:
			return float.Parse(data.userMonster.hp);
		case ExtraEffectUtil.EffectType.Spd:
			return float.Parse(data.userMonster.speed);
		case ExtraEffectUtil.EffectType.Satk:
			return float.Parse(data.userMonster.spAttack);
		case ExtraEffectUtil.EffectType.Sdef:
			return float.Parse(data.userMonster.spDefense);
		}
		return 0f;
	}

	private static int GetExtraChipValue(MonsterData monsterData, ExtraEffectUtil.EffectType effectType)
	{
		int num = 0;
		int triggerValue = 0;
		if (CMD_MultiRecruitPartyWait.StageDataBk != null)
		{
			triggerValue = CMD_MultiRecruitPartyWait.StageDataBk.worldAreaId.ToInt32();
		}
		else if (CMD_QuestTOP.instance != null && CMD_QuestTOP.instance.StageDataBk != null)
		{
			GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM[] worldStageM = MasterDataMng.Instance().RespDataMA_WorldStageM.worldStageM;
			foreach (GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM worldStageM2 in worldStageM)
			{
				if (CMD_QuestTOP.instance.StageDataBk.worldDungeonM.worldStageId == worldStageM2.worldStageId)
				{
					triggerValue = worldStageM2.worldAreaId.ToInt32();
					break;
				}
			}
		}
		foreach (int num2 in monsterData.GetChipIdList())
		{
			GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffectData = ChipDataMng.GetChipEffectData(num2.ToString());
			if (chipEffectData != null)
			{
				GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] invocationList = ChipEffectStatus.GetInvocationList(chipEffectData, ChipEffectStatus.EffectTriggerType.Area, monsterData.monsterM.monsterGroupId.ToInt32(), null, triggerValue);
				num += ExtraEffectUtil.GetChipEffectValue(invocationList, monsterData, effectType);
			}
		}
		return num;
	}

	private static int GetChipEffectValue(GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffects, MonsterData monsterData, ExtraEffectUtil.EffectType type)
	{
		int num = 0;
		ExtraEffectStatus.ExtraEffectType effectType = ExtraEffectStatus.ExtraEffectType.Non;
		switch (type)
		{
		case ExtraEffectUtil.EffectType.Atk:
			num = monsterData.userMonster.attack.ToInt32();
			effectType = ExtraEffectStatus.ExtraEffectType.Atk;
			break;
		case ExtraEffectUtil.EffectType.Def:
			num = monsterData.userMonster.defense.ToInt32();
			effectType = ExtraEffectStatus.ExtraEffectType.Def;
			break;
		case ExtraEffectUtil.EffectType.Hp:
			num = monsterData.userMonster.hp.ToInt32();
			effectType = ExtraEffectStatus.ExtraEffectType.Hp;
			break;
		case ExtraEffectUtil.EffectType.Spd:
			num = monsterData.userMonster.speed.ToInt32();
			effectType = ExtraEffectStatus.ExtraEffectType.Speed;
			break;
		case ExtraEffectUtil.EffectType.Satk:
			num = monsterData.userMonster.spAttack.ToInt32();
			effectType = ExtraEffectStatus.ExtraEffectType.Satk;
			break;
		case ExtraEffectUtil.EffectType.Sdef:
			num = monsterData.userMonster.spDefense.ToInt32();
			effectType = ExtraEffectStatus.ExtraEffectType.Sdef;
			break;
		}
		List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect> list = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
		ExtraEffectStatus.ExtraTargetType targetType = ExtraEffectStatus.ExtraTargetType.Player;
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.Non, 0, ConstValue.ResistanceType.NONE, effectType));
		ConstValue.ResistanceType resistanceType = ConstValue.ResistanceType.NONE;
		if (monsterData != null)
		{
			Dictionary<string, Tolerance> dictionary = new Dictionary<string, Tolerance>();
			string resistanceId = monsterData.monsterM.resistanceId;
			if (!dictionary.ContainsKey(resistanceId.Trim()))
			{
				dictionary.Add(resistanceId.Trim(), ExtraEffectUtil.ResistanceToTolerance(resistanceId));
			}
			Tolerance tolerance;
			if (dictionary.TryGetValue(resistanceId.Trim(), out tolerance))
			{
				List<ConstValue.ResistanceType> attributeStrengthList = tolerance.GetAttributeStrengthList();
				foreach (ConstValue.ResistanceType resistanceType2 in attributeStrengthList)
				{
					list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterResistance, 0, resistanceType2, effectType));
					list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterResistance, (int)resistanceType2, ConstValue.ResistanceType.NONE, effectType));
					list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterResistance, (int)resistanceType2, resistanceType2, effectType));
				}
			}
		}
		GameWebAPI.RespDataMA_GetMonsterMG.MonsterM monsterGroupMasterByMonsterGroupId = MonsterDataMng.Instance().GetMonsterGroupMasterByMonsterGroupId(monsterData.monsterM.monsterGroupId);
		Species targetValue = ServerToBattleUtility.IntToSpecies(monsterGroupMasterByMonsterGroupId.tribe);
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterTribe, 0, resistanceType, effectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterTribe, (int)targetValue, ConstValue.ResistanceType.NONE, effectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterGroup, 0, ConstValue.ResistanceType.NONE, effectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.MonsterGroup, monsterData.monsterM.monsterGroupId.ToInt32(), ConstValue.ResistanceType.NONE, effectType));
		EvolutionStep evolutionStep = ServerToBattleUtility.IntToEvolutionStep(monsterGroupMasterByMonsterGroupId.growStep);
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.GrowStep, 0, ConstValue.ResistanceType.NONE, effectType));
		list.AddRange(ChipEffectStatus.GetChipEffectList(chipEffects, targetType, ExtraEffectStatus.ExtraTargetSubType.GrowStep, ExtraEffectStatus.EvolutionStepToMasterId(evolutionStep), ConstValue.ResistanceType.NONE, effectType));
		if (list.Count > 0)
		{
			float num2 = (float)ChipEffectStatus.GetCorrectionValue(num, list);
			return (int)(num2 - (float)num);
		}
		return 0;
	}

	public static GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM[] GetExtraEffectArray(string worldDungeonId)
	{
		GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectManageM.WorldDungeonExtraEffectManageM[] worldDungeonExtraEffectManageM = MasterDataMng.Instance().RespDataMA_WorldDungeonExtraEffectManageM.worldDungeonExtraEffectManageM;
		GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM[] worldDungeonExtraEffectM = MasterDataMng.Instance().RespDataMA_WorldDungeonExtraEffectM.worldDungeonExtraEffectM;
		List<string> list = new List<string>();
		foreach (GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectManageM.WorldDungeonExtraEffectManageM worldDungeonExtraEffectManageM2 in worldDungeonExtraEffectManageM)
		{
			if (worldDungeonExtraEffectManageM2.worldDungeonId == worldDungeonId)
			{
				list.Add(worldDungeonExtraEffectManageM2.worldDungeonExtraEffectId);
			}
		}
		List<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM> list2 = new List<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM>();
		foreach (string b in list)
		{
			foreach (GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM worldDungeonExtraEffectM2 in worldDungeonExtraEffectM)
			{
				if (worldDungeonExtraEffectM2.worldDungeonExtraEffectId == b)
				{
					list2.Add(worldDungeonExtraEffectM2);
					break;
				}
			}
		}
		return list2.ToArray();
	}

	private static Tolerance ResistanceToTolerance(string resistanceId)
	{
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM[] monsterResistanceM = MasterDataMng.Instance().RespDataMA_MonsterResistanceM.monsterResistanceM;
		Tolerance tolerance = null;
		for (int i = 0; i < monsterResistanceM.Length; i++)
		{
			if (monsterResistanceM[i].monsterResistanceId.Equals(resistanceId))
			{
				Strength noneValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].none);
				Strength redValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].fire);
				Strength blueValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].water);
				Strength yellowValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].thunder);
				Strength greenValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].nature);
				Strength whiteValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].light);
				Strength blackValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].dark);
				Strength poisonValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].poison);
				Strength confusionValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].confusion);
				Strength paralysisValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].paralysis);
				Strength sleepValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].sleep);
				Strength stunValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].stun);
				Strength skillLockValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].skillLock);
				Strength instantDeathValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].death);
				tolerance = new Tolerance(noneValue, redValue, blueValue, yellowValue, greenValue, whiteValue, blackValue, poisonValue, confusionValue, paralysisValue, sleepValue, stunValue, skillLockValue, instantDeathValue);
				break;
			}
		}
		if (tolerance == null)
		{
			global::Debug.LogError("Toleranceの生成に失敗しました. (" + resistanceId + ")");
			return null;
		}
		return tolerance;
	}

	public enum EffectType
	{
		SkillDamage = 11,
		SkillHitRate,
		Atk = 21,
		Def,
		Hp,
		Spd,
		Satk,
		Sdef,
		All
	}

	public enum TargetType
	{
		All,
		Player,
		Enemy
	}

	public enum TargetSubType
	{
		Resistance = 1,
		Tribe,
		GroupId,
		Grow,
		Facility,
		Skill
	}
}
