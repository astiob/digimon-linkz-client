using BattleStateMachineInternal;
using Monster;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExtraEffectUtil
{
	public static bool IsExtraEffectMonster(MonsterData monsterData, GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM[] effectArray)
	{
		if (MonsterGrowStepData.IsGardenDigimonScope(monsterData.monsterMG.growStep))
		{
			return false;
		}
		int areaId = ExtraEffectUtil.GetAreaId();
		foreach (int num in monsterData.GetChipEquip().GetChipIdList())
		{
			GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffectData = ChipDataMng.GetChipEffectData(num.ToString());
			if (chipEffectData != null)
			{
				GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] invocationList = ChipEffectStatus.GetInvocationList(chipEffectData, EffectStatusBase.EffectTriggerType.Area, monsterData.monsterM.monsterGroupId.ToInt32(), null, areaId);
				if (invocationList.Length > 0)
				{
					return true;
				}
			}
		}
		GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus[] eventPointBonuses = ExtraEffectUtil.GetEventPointBonuses(ExtraEffectUtil.GetDungeonId().ToString());
		foreach (GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus eventPointBonus in eventPointBonuses)
		{
			bool flag = ExtraEffectUtil.CheckExtraParams(monsterData, eventPointBonus);
			if (flag)
			{
				return true;
			}
		}
		return ExtraEffectUtil.CheckExtraStageParams(monsterData, effectArray);
	}

	private static bool CheckExtraParams(MonsterData monsterData, GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus eventPointBonus)
	{
		if (float.Parse(eventPointBonus.effectValue) < 0f)
		{
			return false;
		}
		ExtraEffectUtil.EventPointBonusTargetSubType eventPointBonusTargetSubType = (ExtraEffectUtil.EventPointBonusTargetSubType)int.Parse(eventPointBonus.targetSubType);
		switch (eventPointBonusTargetSubType)
		{
		case ExtraEffectUtil.EventPointBonusTargetSubType.MonsterTribe:
			if (monsterData.monsterMG.tribe.Equals(eventPointBonus.targetValue))
			{
				return true;
			}
			break;
		case ExtraEffectUtil.EventPointBonusTargetSubType.MonsterGroup:
			if (monsterData.monsterMG.monsterGroupId.Equals(eventPointBonus.targetValue))
			{
				return true;
			}
			break;
		case ExtraEffectUtil.EventPointBonusTargetSubType.GrowStep:
			if (monsterData.monsterMG.growStep.Equals(eventPointBonus.targetValue))
			{
				return true;
			}
			break;
		case ExtraEffectUtil.EventPointBonusTargetSubType.SkillId:
			if (monsterData.GetCommonSkill() != null && monsterData.GetCommonSkill().skillId.Equals(eventPointBonus.targetValue))
			{
				return true;
			}
			if (monsterData.GetLeaderSkill() != null && monsterData.GetLeaderSkill().skillId.Equals(eventPointBonus.targetValue))
			{
				return true;
			}
			break;
		case ExtraEffectUtil.EventPointBonusTargetSubType.ChipId:
		{
			MonsterChipEquipData chipEquip = monsterData.GetChipEquip();
			int[] chipIdList = chipEquip.GetChipIdList();
			if (chipIdList.Where((int item) => item == eventPointBonus.targetValue.ToInt32()).Any<int>())
			{
				return true;
			}
			break;
		}
		default:
			if (eventPointBonusTargetSubType == ExtraEffectUtil.EventPointBonusTargetSubType.MonsterIntegrationGroup)
			{
				GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup[] monsterIntegrationGroupM = MasterDataMng.Instance().ResponseMonsterIntegrationGroupMaster.monsterIntegrationGroupM;
				for (int i = 0; i < monsterIntegrationGroupM.Length; i++)
				{
					if (eventPointBonus.targetValue.Equals(monsterIntegrationGroupM[i].monsterIntegrationId) && monsterIntegrationGroupM[i].monsterId.Equals(monsterData.monsterM.monsterId))
					{
						return true;
					}
				}
			}
			break;
		}
		return false;
	}

	public static bool CheckExtraStageParams(MonsterData monsterData, GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM effect)
	{
		GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM[] effectArray = new GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM[]
		{
			effect
		};
		return ExtraEffectUtil.CheckExtraStageParams(monsterData, effectArray);
	}

	private static bool CheckExtraStageParams(MonsterData monsterData, GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM[] effectArray)
	{
		EffectStatusBase.ExtraEffectType[] array = new EffectStatusBase.ExtraEffectType[]
		{
			EffectStatusBase.ExtraEffectType.Atk,
			EffectStatusBase.ExtraEffectType.Def,
			EffectStatusBase.ExtraEffectType.Hp,
			EffectStatusBase.ExtraEffectType.Speed,
			EffectStatusBase.ExtraEffectType.Satk,
			EffectStatusBase.ExtraEffectType.Sdef,
			EffectStatusBase.ExtraEffectType.SkillPower,
			EffectStatusBase.ExtraEffectType.SkillHit
		};
		foreach (EffectStatusBase.ExtraEffectType extraEffectType in array)
		{
			int num = 0;
			int num2 = 0;
			if (extraEffectType == EffectStatusBase.ExtraEffectType.SkillPower || extraEffectType == EffectStatusBase.ExtraEffectType.SkillHit)
			{
				for (int j = 1; j <= 3; j++)
				{
					ExtraEffectUtil.GetExtraEffectFluctuationValue(out num, out num2, monsterData, effectArray, extraEffectType, j, false);
					if (num2 != 0)
					{
						return true;
					}
				}
			}
			else
			{
				ExtraEffectUtil.GetExtraEffectFluctuationValue(out num, out num2, monsterData, effectArray, extraEffectType, 0, false);
				if (num2 != 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static void GetExtraEffectFluctuationValue(out int result, out int change, MonsterData monsterData, GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM[] effectArray, EffectStatusBase.ExtraEffectType extraEffectType, int skillType = 0, bool isChipValue = true)
	{
		int num = (int)ExtraEffectUtil.GetStatusValue(extraEffectType, monsterData, skillType);
		int num2 = 0;
		if (isChipValue)
		{
			num2 = ExtraEffectUtil.GetExtraChipValue(monsterData, extraEffectType);
		}
		MonsterData[] party = new MonsterData[]
		{
			monsterData
		};
		if (CMD_PartyEdit.instance != null)
		{
			MonsterData[] array = CMD_PartyEdit.instance.GetSelectedMD().ToArray();
			bool flag = array.Where((MonsterData item) => item.userMonster.userMonsterId == monsterData.userMonster.userMonsterId).Any<MonsterData>();
			if (flag)
			{
				party = array;
			}
			else
			{
				GameWebAPI.RespDataMN_GetDeckList.DeckList[] deckList = DataMng.Instance().RespDataMN_DeckList.deckList;
				string deckNoText = DataMng.Instance().RespDataMN_DeckList.selectDeckNum;
				GameWebAPI.RespDataMN_GetDeckList.DeckList deckList2 = deckList.Where((GameWebAPI.RespDataMN_GetDeckList.DeckList item) => item.deckNum == deckNoText.ToString()).SingleOrDefault<GameWebAPI.RespDataMN_GetDeckList.DeckList>();
				bool flag2 = deckList2.monsterList.Where((GameWebAPI.RespDataMN_GetDeckList.MonsterList item) => item.userMonsterId == monsterData.userMonster.userMonsterId).Any<GameWebAPI.RespDataMN_GetDeckList.MonsterList>();
				if (flag2)
				{
					List<MonsterData> list = new List<MonsterData>();
					foreach (GameWebAPI.RespDataMN_GetDeckList.MonsterList monsterList2 in deckList2.monsterList)
					{
						MonsterData monsterDataByUserMonsterID = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(monsterList2.userMonsterId, false);
						list.Add(monsterDataByUserMonsterID);
					}
					party = list.ToArray();
				}
			}
		}
		int num3 = 0;
		AffectEffectProperty affectEffectProperty = null;
		if (extraEffectType == EffectStatusBase.ExtraEffectType.SkillPower)
		{
			List<AffectEffectProperty> affectEffectPropertyListForUtil = BattleServerControl.GetAffectEffectPropertyListForUtil(num.ToString());
			if (affectEffectPropertyListForUtil != null && affectEffectPropertyListForUtil.Count<AffectEffectProperty>() > 0)
			{
				affectEffectProperty = affectEffectPropertyListForUtil[0];
				num = affectEffectProperty.GetPower(null);
				num3 = ExtraEffectUtil.GetExtraStageValue(num, monsterData, party, effectArray, affectEffectProperty, extraEffectType);
			}
			else
			{
				num = num3;
			}
		}
		else if (extraEffectType == EffectStatusBase.ExtraEffectType.SkillHit)
		{
			List<AffectEffectProperty> affectEffectPropertyListForUtil2 = BattleServerControl.GetAffectEffectPropertyListForUtil(num.ToString());
			if (affectEffectPropertyListForUtil2 != null && affectEffectPropertyListForUtil2.Count<AffectEffectProperty>() > 0)
			{
				affectEffectProperty = affectEffectPropertyListForUtil2[0];
				num = (int)(affectEffectProperty.hitRate * 100f);
				num3 = ExtraEffectUtil.GetExtraStageValue(num, monsterData, party, effectArray, affectEffectProperty, extraEffectType);
			}
			else
			{
				num = num3;
			}
		}
		else
		{
			num3 = ExtraEffectUtil.GetExtraStageValue(num, monsterData, party, effectArray, affectEffectProperty, extraEffectType);
		}
		result = num3 + num2;
		change = 0;
		if (num2 > 0 || num < num3)
		{
			change = 1;
		}
		else if (num2 < 0 || num > num3)
		{
			change = -1;
		}
	}

	private static int GetExtraStageValue(int baseValue, MonsterData monsterData, MonsterData[] party, GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM[] effectArray, AffectEffectProperty affectEffectProperty, EffectStatusBase.ExtraEffectType extraEffectType)
	{
		if (effectArray == null || effectArray.Length == 0)
		{
			return baseValue;
		}
		List<ExtraEffectStatus> list = new List<ExtraEffectStatus>();
		foreach (GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM worldDungeonExtraEffectM in effectArray)
		{
			ExtraEffectStatus item = new ExtraEffectStatus(worldDungeonExtraEffectM);
			list.Add(item);
		}
		if (extraEffectType == EffectStatusBase.ExtraEffectType.SkillPower)
		{
			return ExtraEffectStatus.GetExtraEffectCorrectionValue(ExtraEffectUtil.GetAreaId(), list, affectEffectProperty.GetPower(null), party, new MonsterData[0], monsterData, affectEffectProperty, extraEffectType);
		}
		if (extraEffectType == EffectStatusBase.ExtraEffectType.SkillHit)
		{
			return ExtraEffectStatus.GetExtraEffectCorrectionValue(ExtraEffectUtil.GetAreaId(), list, (int)(affectEffectProperty.hitRate * 100f), party, new MonsterData[0], monsterData, affectEffectProperty, extraEffectType);
		}
		return ExtraEffectStatus.GetExtraEffectCorrectionValue(ExtraEffectUtil.GetAreaId(), list, baseValue, party, new MonsterData[0], monsterData, null, extraEffectType);
	}

	private static float GetStatusValue(EffectStatusBase.ExtraEffectType type, MonsterData data, int skillType = 0)
	{
		switch (type)
		{
		case EffectStatusBase.ExtraEffectType.Atk:
			return float.Parse(data.userMonster.attack);
		case EffectStatusBase.ExtraEffectType.Def:
			return float.Parse(data.userMonster.defense);
		case EffectStatusBase.ExtraEffectType.Hp:
			return float.Parse(data.userMonster.hp);
		case EffectStatusBase.ExtraEffectType.Speed:
			return float.Parse(data.userMonster.speed);
		case EffectStatusBase.ExtraEffectType.Satk:
			return float.Parse(data.userMonster.spAttack);
		case EffectStatusBase.ExtraEffectType.Sdef:
			return float.Parse(data.userMonster.spDefense);
		default:
		{
			if (type != EffectStatusBase.ExtraEffectType.SkillPower && type != EffectStatusBase.ExtraEffectType.SkillHit)
			{
				return 0f;
			}
			float result = 0f;
			if (skillType == 1)
			{
				result = (float)data.GetUniqueSkillDetail().skillId.ToInt32();
			}
			else if (skillType == 2)
			{
				result = (float)data.GetCommonSkillDetail().skillId.ToInt32();
			}
			else if (skillType == 3 && data.GetExtraCommonSkillDetail() != null)
			{
				result = (float)data.GetExtraCommonSkillDetail().skillId.ToInt32();
			}
			return result;
		}
		}
	}

	private static int GetExtraChipValue(MonsterData monsterData, EffectStatusBase.ExtraEffectType effectType)
	{
		float num = 0f;
		int areaId = ExtraEffectUtil.GetAreaId();
		GameWebAPI.RespDataMA_GetMonsterMG.MonsterM group = MonsterMaster.GetMonsterMasterByMonsterGroupId(monsterData.monsterM.monsterGroupId).Group;
		foreach (int num2 in monsterData.GetChipEquip().GetChipIdList())
		{
			GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffectData = ChipDataMng.GetChipEffectData(num2.ToString());
			if (chipEffectData != null)
			{
				GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] invocationList = ChipEffectStatus.GetInvocationList(chipEffectData, EffectStatusBase.EffectTriggerType.Area, monsterData.monsterM.monsterGroupId.ToInt32(), null, areaId);
				int num3 = 0;
				EffectStatusBase.ExtraEffectType effectType2 = EffectStatusBase.ExtraEffectType.Non;
				switch (effectType)
				{
				case EffectStatusBase.ExtraEffectType.Atk:
					num3 = monsterData.userMonster.attack.ToInt32();
					effectType2 = EffectStatusBase.ExtraEffectType.Atk;
					break;
				case EffectStatusBase.ExtraEffectType.Def:
					num3 = monsterData.userMonster.defense.ToInt32();
					effectType2 = EffectStatusBase.ExtraEffectType.Def;
					break;
				case EffectStatusBase.ExtraEffectType.Hp:
					num3 = monsterData.userMonster.hp.ToInt32();
					effectType2 = EffectStatusBase.ExtraEffectType.Hp;
					break;
				case EffectStatusBase.ExtraEffectType.Speed:
					num3 = monsterData.userMonster.speed.ToInt32();
					effectType2 = EffectStatusBase.ExtraEffectType.Speed;
					break;
				case EffectStatusBase.ExtraEffectType.Satk:
					num3 = monsterData.userMonster.spAttack.ToInt32();
					effectType2 = EffectStatusBase.ExtraEffectType.Satk;
					break;
				case EffectStatusBase.ExtraEffectType.Sdef:
					num3 = monsterData.userMonster.spDefense.ToInt32();
					effectType2 = EffectStatusBase.ExtraEffectType.Sdef;
					break;
				default:
					if (effectType == EffectStatusBase.ExtraEffectType.SkillPower || effectType == EffectStatusBase.ExtraEffectType.SkillHit)
					{
						num3 = 0;
						effectType2 = EffectStatusBase.ExtraEffectType.Non;
					}
					break;
				}
				GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster responseMonsterIntegrationGroupMaster = MasterDataMng.Instance().ResponseMonsterIntegrationGroupMaster;
				GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup[] source = responseMonsterIntegrationGroupMaster.monsterIntegrationGroupM.Where((GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup item) => item.monsterId == monsterData.monsterM.monsterId).ToArray<GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup>();
				string[] monsterIntegrationIds = source.Select((GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup item) => item.monsterIntegrationId).ToArray<string>();
				num += ChipEffectStatus.GetChipEffectValueToFloat(invocationList, (float)num3, false, monsterIntegrationIds, monsterData.monsterM.monsterGroupId, ExtraEffectUtil.ResistanceToTolerance(monsterData), group.tribe, MonsterGrowStepData.ToGrowStep(group.growStep), null, null, ChipEffectStatus.TargetType.Actor, effectType2, 0);
			}
		}
		return Mathf.FloorToInt(num);
	}

	private static int GetAreaId()
	{
		int result = 0;
		if (CMD_MultiRecruitPartyWait.StageDataBk != null)
		{
			result = CMD_MultiRecruitPartyWait.StageDataBk.worldAreaId.ToInt32();
		}
		else if (CMD_QuestTOP.instance != null && CMD_QuestTOP.instance.StageDataBk != null)
		{
			GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM[] worldStageM = MasterDataMng.Instance().RespDataMA_WorldStageM.worldStageM;
			foreach (GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM worldStageM2 in worldStageM)
			{
				if (CMD_QuestTOP.instance.StageDataBk.worldDungeonM.worldStageId == worldStageM2.worldStageId)
				{
					result = worldStageM2.worldAreaId.ToInt32();
					break;
				}
			}
		}
		else if (CMD_QuestTOP.instance == null && CMD_MultiRecruitPartyWait.StageDataBk == null)
		{
			GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM[] worldStageM3 = MasterDataMng.Instance().RespDataMA_WorldStageM.worldStageM;
			foreach (GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM worldStageM4 in worldStageM3)
			{
				if (CMD_PartyEdit.replayMultiStageId == worldStageM4.worldStageId)
				{
					result = worldStageM4.worldAreaId.ToInt32();
					break;
				}
			}
		}
		return result;
	}

	private static int GetDungeonId()
	{
		int result = 0;
		if (CMD_MultiRecruitPartyWait.StageDataBk != null)
		{
			result = CMD_MultiRecruitPartyWait.StageDataBk.worldDungeonId.ToInt32();
		}
		else if (CMD_QuestTOP.instance != null && CMD_QuestTOP.instance.StageDataBk != null)
		{
			result = CMD_QuestTOP.instance.StageDataBk.worldDungeonM.worldDungeonId.ToInt32();
		}
		return result;
	}

	private static Tolerance ResistanceToTolerance(MonsterData monsterData)
	{
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM resistanceMaster = MonsterResistanceData.GetResistanceMaster(monsterData.monsterM.resistanceId);
		List<GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM> uniqueResistanceList = MonsterResistanceData.GetUniqueResistanceList(monsterData.GetResistanceIdList());
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM data = MonsterResistanceData.AddResistanceFromMultipleTranceData(resistanceMaster, uniqueResistanceList);
		return ServerToBattleUtility.ResistanceToTolerance(data);
	}

	private static GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus[] GetEventPointBonuses(string worldDungeonId)
	{
		List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus> list = new List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus>();
		GameWebAPI.RespDataMA_EventPointBonusM respDataMA_EventPointBonusMaster = MasterDataMng.Instance().RespDataMA_EventPointBonusMaster;
		foreach (GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus eventPointBonus in respDataMA_EventPointBonusMaster.eventPointBonusM)
		{
			if (eventPointBonus.worldDungeonId == worldDungeonId && eventPointBonus.effectType != "0")
			{
				list.Add(eventPointBonus);
			}
		}
		return list.ToArray();
	}

	public enum EventPointBonusTargetSubType
	{
		Non,
		MonsterResistance,
		MonsterTribe,
		MonsterGroup,
		GrowStep,
		SkillId,
		ChipId,
		NoContinue = 10,
		ExtraWave,
		MultiOwnerOld,
		Luck,
		Solo,
		MultiOwner,
		MultiGuest,
		EnemyKill,
		MonsterIntegrationGroup
	}
}
