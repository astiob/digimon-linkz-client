using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Quest
{
	public sealed class QuestBonusPack
	{
		public List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus> eventBonuses;

		public List<string> bonusChipIds;

		public List<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM> dungeonBonuses;

		[CompilerGenerated]
		private static Comparison<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus> <>f__mg$cache0;

		[CompilerGenerated]
		private static Predicate<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus> <>f__mg$cache1;

		[CompilerGenerated]
		private static Predicate<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus> <>f__mg$cache2;

		[CompilerGenerated]
		private static Comparison<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus> <>f__mg$cache3;

		[CompilerGenerated]
		private static Comparison<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM> <>f__mg$cache4;

		public void CreateQuestBonus(string areaId, string stageId, string dungeonId)
		{
			if (string.IsNullOrEmpty(areaId) || string.IsNullOrEmpty(stageId) || string.IsNullOrEmpty(dungeonId))
			{
				this.eventBonuses = new List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus>();
				this.bonusChipIds = new List<string>();
				this.dungeonBonuses = new List<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM>();
			}
			else
			{
				this.eventBonuses = QuestBonus.GetTargetEventPointUpBonus(dungeonId);
				List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus> list = this.eventBonuses;
				if (QuestBonusPack.<>f__mg$cache0 == null)
				{
					QuestBonusPack.<>f__mg$cache0 = new Comparison<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus>(QuestBonus.SortAscEffectType);
				}
				list.Sort(QuestBonusPack.<>f__mg$cache0);
				List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus> list2 = this.eventBonuses;
				if (QuestBonusPack.<>f__mg$cache1 == null)
				{
					QuestBonusPack.<>f__mg$cache1 = new Predicate<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus>(QuestBonus.MatchChipBonusPoint);
				}
				List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus> list3 = list2.FindAll(QuestBonusPack.<>f__mg$cache1);
				List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus> list4 = this.eventBonuses;
				if (QuestBonusPack.<>f__mg$cache2 == null)
				{
					QuestBonusPack.<>f__mg$cache2 = new Predicate<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus>(QuestBonus.MatchChipBonusPoint);
				}
				list4.RemoveAll(QuestBonusPack.<>f__mg$cache2);
				List<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus> list5 = this.eventBonuses;
				if (QuestBonusPack.<>f__mg$cache3 == null)
				{
					QuestBonusPack.<>f__mg$cache3 = new Comparison<GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus>(QuestBonus.SortAscTargetSubType);
				}
				list5.Sort(QuestBonusPack.<>f__mg$cache3);
				this.bonusChipIds = QuestBonus.GetActivateChipIdList(areaId);
				for (int i = 0; i < list3.Count; i++)
				{
					string targetValue = list3[i].targetValue;
					if (!this.bonusChipIds.Contains(targetValue))
					{
						this.bonusChipIds.Add(targetValue);
					}
				}
				this.dungeonBonuses = DataMng.Instance().StageGimmick.GetExtraEffectUpBonusList(stageId, dungeonId);
				List<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM> list6 = this.dungeonBonuses;
				if (QuestBonusPack.<>f__mg$cache4 == null)
				{
					QuestBonusPack.<>f__mg$cache4 = new Comparison<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM>(QuestBonus.SortAscTargetSubType);
				}
				list6.Sort(QuestBonusPack.<>f__mg$cache4);
			}
		}

		public bool ExistBonus()
		{
			return (this.bonusChipIds != null && 0 < this.bonusChipIds.Count) || (this.dungeonBonuses != null && 0 < this.dungeonBonuses.Count) || (this.eventBonuses != null && 0 < this.eventBonuses.Count);
		}
	}
}
