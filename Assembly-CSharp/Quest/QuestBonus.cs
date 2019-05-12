using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Quest
{
	public static class QuestBonus
	{
		[CompilerGenerated]
		private static Comparison<int> <>f__mg$cache0;

		public static int SortAscValue(int left, int right)
		{
			return left - right;
		}

		public static List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus> GetTargetEventPoiontBonus(string dungeonId)
		{
			List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus> list = new List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus>();
			GameWebAPI.RespDataMA_EventPointBonusM respDataMA_EventPointBonusMaster = MasterDataMng.Instance().RespDataMA_EventPointBonusMaster;
			for (int i = 0; i < respDataMA_EventPointBonusMaster.eventPointBonusM.Length; i++)
			{
				if (respDataMA_EventPointBonusMaster.eventPointBonusM[i].worldDungeonId == dungeonId && !string.IsNullOrEmpty(respDataMA_EventPointBonusMaster.eventPointBonusM[i].effectType) && respDataMA_EventPointBonusMaster.eventPointBonusM[i].effectType != "0")
				{
					list.Add(respDataMA_EventPointBonusMaster.eventPointBonusM[i]);
				}
			}
			return list;
		}

		public static List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus> GetTargetEventPointUpBonus(string dungeonId)
		{
			List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus> list = new List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus>();
			GameWebAPI.RespDataMA_EventPointBonusM respDataMA_EventPointBonusMaster = MasterDataMng.Instance().RespDataMA_EventPointBonusMaster;
			for (int i = 0; i < respDataMA_EventPointBonusMaster.eventPointBonusM.Length; i++)
			{
				float num;
				if (respDataMA_EventPointBonusMaster.eventPointBonusM[i].worldDungeonId == dungeonId && !string.IsNullOrEmpty(respDataMA_EventPointBonusMaster.eventPointBonusM[i].effectType) && respDataMA_EventPointBonusMaster.eventPointBonusM[i].effectType != "0" && float.TryParse(respDataMA_EventPointBonusMaster.eventPointBonusM[i].effectValue, out num) && 0f < num)
				{
					list.Add(respDataMA_EventPointBonusMaster.eventPointBonusM[i]);
				}
			}
			return list;
		}

		public static int SortAscEffectType(GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus left, GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus right)
		{
			int num = int.Parse(left.effectType);
			int num2 = int.Parse(right.effectType);
			return num - num2;
		}

		public static bool MatchChipBonusPoint(GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus pointBonus)
		{
			return "6" == pointBonus.targetSubType;
		}

		public static int SortAscTargetSubType(GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus left, GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus right)
		{
			int num = int.Parse(left.targetSubType);
			int num2 = int.Parse(right.targetSubType);
			return num - num2;
		}

		public static List<string> GetActivateChipIdList(string areaId)
		{
			List<string> list = new List<string>();
			GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffectM = MasterDataMng.Instance().RespDataMA_ChipEffectMaster.chipEffectM;
			if (chipEffectM.Length > 0)
			{
				for (int i = 0; i < chipEffectM.Length; i++)
				{
					if (chipEffectM[i].effectTriggerValue == areaId && chipEffectM[i].effectTrigger == "11" && !list.Contains(chipEffectM[i].chipId))
					{
						list.Add(chipEffectM[i].chipId);
					}
				}
			}
			return list;
		}

		public static List<string> GetActivateEquipChipIdList(List<string> targetBonusChipIds, int[] monsterEquipChipIds)
		{
			List<string> list = new List<string>();
			List<int> list2 = new List<int>(monsterEquipChipIds);
			List<int> list3 = list2;
			if (QuestBonus.<>f__mg$cache0 == null)
			{
				QuestBonus.<>f__mg$cache0 = new Comparison<int>(QuestBonus.SortAscValue);
			}
			list3.Sort(QuestBonus.<>f__mg$cache0);
			for (int i = 0; i < list2.Count; i++)
			{
				string item = list2[i].ToString();
				if (targetBonusChipIds.Contains(item))
				{
					list.Add(item);
				}
			}
			return list;
		}

		public static int SortAscTargetSubType(GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM left, GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM right)
		{
			int num = int.Parse(left.targetSubType);
			int num2 = int.Parse(right.targetSubType);
			return num - num2;
		}

		public static List<string> GetBonusText(List<string> activateChipIds, List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus> eventBonusPoints, List<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM> stageGimmickBonusPoints)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < activateChipIds.Count; i++)
			{
				GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(activateChipIds[i]);
				if (chipMainData != null)
				{
					list.Add(chipMainData.name);
				}
			}
			for (int j = 0; j < eventBonusPoints.Count; j++)
			{
				list.Add(eventBonusPoints[j].detail);
			}
			for (int k = 0; k < stageGimmickBonusPoints.Count; k++)
			{
				list.Add(stageGimmickBonusPoints[k].detail);
			}
			return list;
		}
	}
}
