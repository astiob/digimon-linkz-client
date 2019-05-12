using Monster;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Quest
{
	public static class QuestBonusFilter
	{
		[CompilerGenerated]
		private static Comparison<int> <>f__mg$cache0;

		public static List<string> GetActivateBonusChips(MonsterData monsterData, List<string> references)
		{
			List<string> list = new List<string>();
			list = QuestBonus.GetActivateEquipChipIdList(references, monsterData.GetChipEquip().GetChipIdList());
			List<int> chipList = MonsterAutoLoadChipData.GetChipList(monsterData.monsterM.monsterId);
			List<int> list2 = chipList;
			if (QuestBonusFilter.<>f__mg$cache0 == null)
			{
				QuestBonusFilter.<>f__mg$cache0 = new Comparison<int>(QuestBonus.SortAscValue);
			}
			list2.Sort(QuestBonusFilter.<>f__mg$cache0);
			for (int i = 0; i < chipList.Count; i++)
			{
				string item = chipList[i].ToString();
				if (references.Contains(item) && !list.Contains(item))
				{
					list.Add(item);
				}
			}
			return list;
		}

		public static List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus> GetActivateEventBonuses(QuestBonusTargetCheck targetChecker, MonsterData monsterData, List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus> references)
		{
			List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus> list = new List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus>();
			for (int i = 0; i < references.Count; i++)
			{
				Func<MonsterData, string, bool> monsterChecker = targetChecker.GetMonsterChecker(references[i].targetSubType);
				if (monsterChecker != null && monsterChecker(monsterData, references[i].targetValue))
				{
					list.Add(references[i]);
				}
			}
			return list;
		}

		public static List<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM> GetActivateDungeonBonuses(QuestBonusTargetCheck targetChecker, MonsterData monsterData, List<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM> references)
		{
			List<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM> list = new List<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM>();
			for (int i = 0; i < references.Count; i++)
			{
				Func<MonsterData, string, bool> monsterChecker = targetChecker.GetMonsterChecker(references[i].targetSubType);
				if (monsterChecker != null && monsterChecker(monsterData, references[i].targetValue))
				{
					list.Add(references[i]);
				}
			}
			return list;
		}
	}
}
