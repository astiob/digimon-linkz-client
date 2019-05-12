using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Quest
{
	public sealed class QuestBonusTargetCheck
	{
		private Dictionary<ExtraEffectUtil.EventPointBonusTargetSubType, Func<MonsterData, string, bool>> monsterCheckerTable;

		[CompilerGenerated]
		private static Func<MonsterData, string, bool> <>f__mg$cache0;

		[CompilerGenerated]
		private static Func<MonsterData, string, bool> <>f__mg$cache1;

		[CompilerGenerated]
		private static Func<MonsterData, string, bool> <>f__mg$cache2;

		[CompilerGenerated]
		private static Func<MonsterData, string, bool> <>f__mg$cache3;

		[CompilerGenerated]
		private static Func<MonsterData, string, bool> <>f__mg$cache4;

		public QuestBonusTargetCheck()
		{
			Dictionary<ExtraEffectUtil.EventPointBonusTargetSubType, Func<MonsterData, string, bool>> dictionary = new Dictionary<ExtraEffectUtil.EventPointBonusTargetSubType, Func<MonsterData, string, bool>>();
			Dictionary<ExtraEffectUtil.EventPointBonusTargetSubType, Func<MonsterData, string, bool>> dictionary2 = dictionary;
			ExtraEffectUtil.EventPointBonusTargetSubType key = ExtraEffectUtil.EventPointBonusTargetSubType.MonsterTribe;
			if (QuestBonusTargetCheck.<>f__mg$cache0 == null)
			{
				QuestBonusTargetCheck.<>f__mg$cache0 = new Func<MonsterData, string, bool>(QuestBonusTargetCheck.CheckTribe);
			}
			dictionary2.Add(key, QuestBonusTargetCheck.<>f__mg$cache0);
			Dictionary<ExtraEffectUtil.EventPointBonusTargetSubType, Func<MonsterData, string, bool>> dictionary3 = dictionary;
			ExtraEffectUtil.EventPointBonusTargetSubType key2 = ExtraEffectUtil.EventPointBonusTargetSubType.MonsterGroup;
			if (QuestBonusTargetCheck.<>f__mg$cache1 == null)
			{
				QuestBonusTargetCheck.<>f__mg$cache1 = new Func<MonsterData, string, bool>(QuestBonusTargetCheck.CheckMonsterGroup);
			}
			dictionary3.Add(key2, QuestBonusTargetCheck.<>f__mg$cache1);
			Dictionary<ExtraEffectUtil.EventPointBonusTargetSubType, Func<MonsterData, string, bool>> dictionary4 = dictionary;
			ExtraEffectUtil.EventPointBonusTargetSubType key3 = ExtraEffectUtil.EventPointBonusTargetSubType.GrowStep;
			if (QuestBonusTargetCheck.<>f__mg$cache2 == null)
			{
				QuestBonusTargetCheck.<>f__mg$cache2 = new Func<MonsterData, string, bool>(QuestBonusTargetCheck.CheckGrowStep);
			}
			dictionary4.Add(key3, QuestBonusTargetCheck.<>f__mg$cache2);
			Dictionary<ExtraEffectUtil.EventPointBonusTargetSubType, Func<MonsterData, string, bool>> dictionary5 = dictionary;
			ExtraEffectUtil.EventPointBonusTargetSubType key4 = ExtraEffectUtil.EventPointBonusTargetSubType.SkillId;
			if (QuestBonusTargetCheck.<>f__mg$cache3 == null)
			{
				QuestBonusTargetCheck.<>f__mg$cache3 = new Func<MonsterData, string, bool>(QuestBonusTargetCheck.CheckSkill);
			}
			dictionary5.Add(key4, QuestBonusTargetCheck.<>f__mg$cache3);
			Dictionary<ExtraEffectUtil.EventPointBonusTargetSubType, Func<MonsterData, string, bool>> dictionary6 = dictionary;
			ExtraEffectUtil.EventPointBonusTargetSubType key5 = ExtraEffectUtil.EventPointBonusTargetSubType.MonsterIntegrationGroup;
			if (QuestBonusTargetCheck.<>f__mg$cache4 == null)
			{
				QuestBonusTargetCheck.<>f__mg$cache4 = new Func<MonsterData, string, bool>(QuestBonusTargetCheck.CheckMonsterIntegrationGroup);
			}
			dictionary6.Add(key5, QuestBonusTargetCheck.<>f__mg$cache4);
			this.monsterCheckerTable = dictionary;
		}

		private static bool CheckTribe(MonsterData monsterData, string targetValue)
		{
			return monsterData.monsterMG.tribe == targetValue;
		}

		private static bool CheckMonsterGroup(MonsterData monsterData, string targetValue)
		{
			return monsterData.monsterMG.monsterGroupId == targetValue;
		}

		private static bool CheckGrowStep(MonsterData monsterData, string targetValue)
		{
			return monsterData.monsterMG.growStep == targetValue;
		}

		private static bool CheckSkill(MonsterData monsterData, string targetValue)
		{
			bool result = false;
			if (monsterData.GetCommonSkill() != null && monsterData.GetCommonSkill().skillId == targetValue)
			{
				result = true;
			}
			else if (monsterData.GetLeaderSkill() != null && monsterData.GetLeaderSkill().skillId == targetValue)
			{
				result = true;
			}
			return result;
		}

		private static bool CheckMonsterIntegrationGroup(MonsterData monsterData, string targetValue)
		{
			bool result = false;
			GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup[] monsterIntegrationGroupM = MasterDataMng.Instance().ResponseMonsterIntegrationGroupMaster.monsterIntegrationGroupM;
			for (int i = 0; i < monsterIntegrationGroupM.Length; i++)
			{
				if (monsterIntegrationGroupM[i].monsterIntegrationId == targetValue && monsterIntegrationGroupM[i].monsterId == monsterData.monsterM.monsterId)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		public Func<MonsterData, string, bool> GetMonsterChecker(string targetSubType)
		{
			ExtraEffectUtil.EventPointBonusTargetSubType key = (ExtraEffectUtil.EventPointBonusTargetSubType)int.Parse(targetSubType);
			Func<MonsterData, string, bool> result;
			if (!this.monsterCheckerTable.TryGetValue(key, out result))
			{
				result = null;
			}
			return result;
		}
	}
}
