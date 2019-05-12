using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Monster
{
	public static class MonsterFriendshipData
	{
		private const float UP_RATE_FRENDSHIPSTATUS = 0.01f;

		private static Dictionary<string, string> friendshipTable;

		private static int GetBonusValue(int statusValue, int bonusStep)
		{
			return Mathf.FloorToInt((float)(statusValue * bonusStep) * 0.01f);
		}

		private static int GetBonusValue(string statusValue, int bonusStep)
		{
			int statusValue2 = int.Parse(statusValue);
			return MonsterFriendshipData.GetBonusValue(statusValue2, bonusStep);
		}

		public static void Initialize()
		{
			if (MonsterFriendshipData.friendshipTable == null)
			{
				MonsterFriendshipData.friendshipTable = new Dictionary<string, string>();
			}
			else
			{
				MonsterFriendshipData.friendshipTable.Clear();
			}
		}

		public static string GetFriendshipMax(string growStep)
		{
			string text = "0";
			if (!MonsterFriendshipData.friendshipTable.TryGetValue(growStep, out text))
			{
				GameWebAPI.RespDataMA_GetMonsterGrowStepM.MonsterGrowStepM[] monsterGrowStepM = MasterDataMng.Instance().RespDataMA_MonsterGrowStepM.monsterGrowStepM;
				for (int i = 0; i < monsterGrowStepM.Length; i++)
				{
					if (monsterGrowStepM[i].monsterGrowStepId == growStep)
					{
						text = monsterGrowStepM[i].maxFriendship;
						MonsterFriendshipData.friendshipTable.Add(growStep, text);
						break;
					}
				}
			}
			return text;
		}

		public static int GetFriendshipMaxValue(string growStep)
		{
			string friendshipMax = MonsterFriendshipData.GetFriendshipMax(growStep);
			return friendshipMax.ToInt32();
		}

		public static string GetMaxFriendshipFormat(string nowFriendship, string growStep)
		{
			string friendshipMax = MonsterFriendshipData.GetFriendshipMax(growStep);
			return string.Format(StringMaster.GetString("SystemFraction"), nowFriendship, friendshipMax);
		}

		public static int GetFriendshipBonusStep(int beforeFriendship, int afterFriendship)
		{
			int num = afterFriendship / ConstValue.RIZE_CONDITION_FRENDSHIPSTATUS;
			int num2 = beforeFriendship / ConstValue.RIZE_CONDITION_FRENDSHIPSTATUS;
			int num3 = num - num2;
			if (0 > num3)
			{
				num3 = 0;
			}
			return num3;
		}

		public static StatusValue GetFriendshipBonusValue(GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monsterMaster, int bonusStep)
		{
			return new StatusValue
			{
				hp = MonsterFriendshipData.GetBonusValue(monsterMaster.maxHp, bonusStep),
				attack = MonsterFriendshipData.GetBonusValue(monsterMaster.maxAttack, bonusStep),
				defense = MonsterFriendshipData.GetBonusValue(monsterMaster.maxDefense, bonusStep),
				magicAttack = MonsterFriendshipData.GetBonusValue(monsterMaster.maxSpAttack, bonusStep),
				magicDefense = MonsterFriendshipData.GetBonusValue(monsterMaster.maxSpDefense, bonusStep),
				speed = MonsterFriendshipData.GetBonusValue(monsterMaster.speed, bonusStep)
			};
		}
	}
}
