using Master;
using System;
using System.Collections.Generic;

namespace Monster
{
	public static class MonsterGrowStepData
	{
		private static Dictionary<string, GameWebAPI.RespDataMA_GetMonsterGrowStepM.MonsterGrowStepM> growStepTable;

		public static void Initialize()
		{
			if (MonsterGrowStepData.growStepTable == null)
			{
				MonsterGrowStepData.growStepTable = new Dictionary<string, GameWebAPI.RespDataMA_GetMonsterGrowStepM.MonsterGrowStepM>();
			}
			else
			{
				MonsterGrowStepData.growStepTable.Clear();
			}
		}

		public static string GetGrowStepName(string growStep)
		{
			string result = StringMaster.GetString("CharaStatus-03");
			GameWebAPI.RespDataMA_GetMonsterGrowStepM.MonsterGrowStepM growStepMaster = MonsterGrowStepData.GetGrowStepMaster(growStep);
			if (growStepMaster != null)
			{
				result = growStepMaster.monsterGrowStepName;
			}
			return result;
		}

		public static GameWebAPI.RespDataMA_GetMonsterGrowStepM.MonsterGrowStepM GetGrowStepMaster(string growStep)
		{
			GameWebAPI.RespDataMA_GetMonsterGrowStepM.MonsterGrowStepM result = null;
			if (!MonsterGrowStepData.growStepTable.TryGetValue(growStep, out result))
			{
				GameWebAPI.RespDataMA_GetMonsterGrowStepM.MonsterGrowStepM[] monsterGrowStepM = MasterDataMng.Instance().RespDataMA_MonsterGrowStepM.monsterGrowStepM;
				for (int i = 0; i < monsterGrowStepM.Length; i++)
				{
					if (monsterGrowStepM[i].monsterGrowStepId == growStep)
					{
						result = monsterGrowStepM[i];
						MonsterGrowStepData.growStepTable.Add(growStep, monsterGrowStepM[i]);
						break;
					}
				}
			}
			return result;
		}

		public static GrowStep ToGrowStep(string growStep)
		{
			int result = 0;
			if (!int.TryParse(growStep, out result))
			{
				Debug.Log("成長帯IDの変換に失敗");
			}
			return (GrowStep)result;
		}

		public static string ToGrowStepString(GrowStep growStep)
		{
			return string.Format("{0}", (int)growStep);
		}

		public static bool IsEggScope(int growStep)
		{
			return MonsterGrowStepData.IsEggScope((GrowStep)growStep);
		}

		public static bool IsEggScope(string growStep)
		{
			return MonsterGrowStepData.IsEggScope(MonsterGrowStepData.ToGrowStep(growStep));
		}

		public static bool IsEggScope(GrowStep growStep)
		{
			return GrowStep.EGG == growStep;
		}

		public static bool IsChild1Scope(int growStep)
		{
			return MonsterGrowStepData.IsChild1Scope((GrowStep)growStep);
		}

		public static bool IsChild1Scope(string growStep)
		{
			return MonsterGrowStepData.IsChild1Scope(MonsterGrowStepData.ToGrowStep(growStep));
		}

		public static bool IsChild1Scope(GrowStep growStep)
		{
			return GrowStep.CHILD_1 == growStep;
		}

		public static bool IsChild2Scope(int growStep)
		{
			return MonsterGrowStepData.IsChild2Scope((GrowStep)growStep);
		}

		public static bool IsChild2Scope(string growStep)
		{
			return MonsterGrowStepData.IsChild2Scope(MonsterGrowStepData.ToGrowStep(growStep));
		}

		public static bool IsChild2Scope(GrowStep growStep)
		{
			return GrowStep.CHILD_2 == growStep;
		}

		public static bool IsGrowingScope(int growStep)
		{
			return MonsterGrowStepData.IsGrowingScope((GrowStep)growStep);
		}

		public static bool IsGrowingScope(string growStep)
		{
			return MonsterGrowStepData.IsGrowingScope(MonsterGrowStepData.ToGrowStep(growStep));
		}

		public static bool IsGrowingScope(GrowStep growStep)
		{
			return growStep == GrowStep.GROWING || GrowStep.HYBRID_GROWING == growStep;
		}

		public static bool IsRipeScope(int growStep)
		{
			return MonsterGrowStepData.IsRipeScope((GrowStep)growStep);
		}

		public static bool IsRipeScope(string growStep)
		{
			return MonsterGrowStepData.IsRipeScope(MonsterGrowStepData.ToGrowStep(growStep));
		}

		public static bool IsRipeScope(GrowStep growStep)
		{
			return growStep == GrowStep.RIPE || growStep == GrowStep.ARMOR_1 || GrowStep.HYBRID_RIPE == growStep;
		}

		public static bool IsPerfectScope(int growStep)
		{
			return MonsterGrowStepData.IsPerfectScope((GrowStep)growStep);
		}

		public static bool IsPerfectScope(string growStep)
		{
			return MonsterGrowStepData.IsPerfectScope(MonsterGrowStepData.ToGrowStep(growStep));
		}

		public static bool IsPerfectScope(GrowStep growStep)
		{
			return growStep == GrowStep.PERFECT || GrowStep.HYBRID_PERFECT == growStep;
		}

		public static bool IsUltimateScope(int growStep)
		{
			return MonsterGrowStepData.IsUltimateScope((GrowStep)growStep);
		}

		public static bool IsUltimateScope(string growStep)
		{
			return MonsterGrowStepData.IsUltimateScope(MonsterGrowStepData.ToGrowStep(growStep));
		}

		public static bool IsUltimateScope(GrowStep growStep)
		{
			return growStep == GrowStep.ULTIMATE || growStep == GrowStep.ARMOR_2 || GrowStep.HYBRID_ULTIMATE == growStep;
		}

		public static bool IsChildScope(int growStep)
		{
			return MonsterGrowStepData.IsChildScope((GrowStep)growStep);
		}

		public static bool IsChildScope(string growStep)
		{
			return MonsterGrowStepData.IsChildScope(MonsterGrowStepData.ToGrowStep(growStep));
		}

		public static bool IsChildScope(GrowStep growStep)
		{
			return growStep == GrowStep.CHILD_1 || GrowStep.CHILD_2 == growStep;
		}

		public static bool IsGardenDigimonScope(int growStep)
		{
			return MonsterGrowStepData.IsGardenDigimonScope((GrowStep)growStep);
		}

		public static bool IsGardenDigimonScope(string growStep)
		{
			return MonsterGrowStepData.IsGardenDigimonScope(MonsterGrowStepData.ToGrowStep(growStep));
		}

		public static bool IsGardenDigimonScope(GrowStep growStep)
		{
			return growStep == GrowStep.EGG || growStep == GrowStep.CHILD_1 || GrowStep.CHILD_2 == growStep;
		}

		public static bool IsGrowStepHigh(string growStep)
		{
			int growStep2 = int.Parse(growStep);
			return MonsterGrowStepData.IsGrowStepHigh(growStep2);
		}

		public static bool IsGrowStepHigh(int growStep)
		{
			bool result = false;
			switch (growStep)
			{
			case 6:
			case 7:
			case 9:
			case 12:
			case 13:
				result = true;
				break;
			}
			return result;
		}

		public static int GetGrowStepSortValue(int growStep)
		{
			switch (growStep)
			{
			case 1:
				return 0;
			case 2:
				return 1;
			case 3:
				return 2;
			case 4:
				return 3;
			case 5:
				return 5;
			case 6:
				return 8;
			case 7:
				return 10;
			case 8:
				return 7;
			case 9:
				return 11;
			case 10:
				return 4;
			case 11:
				return 6;
			case 12:
				return 9;
			case 13:
				return 12;
			default:
				return 0;
			}
		}

		public static bool IsGrowingGroup(int growStep)
		{
			return MonsterGrowStepData.IsGrowingGroup((GrowStep)growStep);
		}

		public static bool IsGrowingGroup(string growStep)
		{
			return MonsterGrowStepData.IsGrowingGroup(MonsterGrowStepData.ToGrowStep(growStep));
		}

		public static bool IsGrowingGroup(GrowStep growStep)
		{
			return GrowStep.GROWING == growStep;
		}

		public static bool IsRipeGroup(int growStep)
		{
			return MonsterGrowStepData.IsRipeGroup((GrowStep)growStep);
		}

		public static bool IsRipeGroup(string growStep)
		{
			return MonsterGrowStepData.IsRipeGroup(MonsterGrowStepData.ToGrowStep(growStep));
		}

		public static bool IsRipeGroup(GrowStep growStep)
		{
			return GrowStep.RIPE == growStep;
		}

		public static bool IsPerfectGroup(int growStep)
		{
			return MonsterGrowStepData.IsPerfectGroup((GrowStep)growStep);
		}

		public static bool IsPerfectGroup(string growStep)
		{
			return MonsterGrowStepData.IsPerfectGroup(MonsterGrowStepData.ToGrowStep(growStep));
		}

		public static bool IsPerfectGroup(GrowStep growStep)
		{
			return GrowStep.PERFECT == growStep;
		}

		public static bool IsUltimateGroup(int growStep)
		{
			return MonsterGrowStepData.IsUltimateGroup((GrowStep)growStep);
		}

		public static bool IsUltimateGroup(string growStep)
		{
			return MonsterGrowStepData.IsUltimateGroup(MonsterGrowStepData.ToGrowStep(growStep));
		}

		public static bool IsUltimateGroup(GrowStep growStep)
		{
			return GrowStep.ULTIMATE == growStep;
		}

		public static bool IsArmorGroup(int growStep)
		{
			return MonsterGrowStepData.IsArmorGroup((GrowStep)growStep);
		}

		public static bool IsArmorGroup(string growStep)
		{
			return MonsterGrowStepData.IsArmorGroup(MonsterGrowStepData.ToGrowStep(growStep));
		}

		public static bool IsArmorGroup(GrowStep growStep)
		{
			return growStep == GrowStep.ARMOR_1 || GrowStep.ARMOR_2 == growStep;
		}

		public static bool IsHybridGroup(int growStep)
		{
			return MonsterGrowStepData.IsHybridGroup((GrowStep)growStep);
		}

		public static bool IsHybridGroup(string growStep)
		{
			return MonsterGrowStepData.IsHybridGroup(MonsterGrowStepData.ToGrowStep(growStep));
		}

		public static bool IsHybridGroup(GrowStep growStep)
		{
			return growStep == GrowStep.HYBRID_GROWING || growStep == GrowStep.HYBRID_RIPE || growStep == GrowStep.HYBRID_PERFECT || GrowStep.HYBRID_ULTIMATE == growStep;
		}
	}
}
