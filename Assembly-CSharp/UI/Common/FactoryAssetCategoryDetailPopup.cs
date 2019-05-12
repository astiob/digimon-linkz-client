using FarmData;
using Monster;
using System;
using System.Runtime.CompilerServices;

namespace UI.Common
{
	public static class FactoryAssetCategoryDetailPopup
	{
		[CompilerGenerated]
		private static Func<string, string, CommonDialog> <>f__mg$cache0;

		[CompilerGenerated]
		private static Func<string, string, CommonDialog> <>f__mg$cache1;

		[CompilerGenerated]
		private static Func<string, string, CommonDialog> <>f__mg$cache2;

		[CompilerGenerated]
		private static Func<string, string, CommonDialog> <>f__mg$cache3;

		[CompilerGenerated]
		private static Func<string, string, CommonDialog> <>f__mg$cache4;

		[CompilerGenerated]
		private static Func<string, string, CommonDialog> <>f__mg$cache5;

		[CompilerGenerated]
		private static Func<string, string, CommonDialog> <>f__mg$cache6;

		[CompilerGenerated]
		private static Func<string, string, CommonDialog> <>f__mg$cache7;

		[CompilerGenerated]
		private static Func<string, string, CommonDialog> <>f__mg$cache8;

		[CompilerGenerated]
		private static Func<string, string, CommonDialog> <>f__mg$cache9;

		[CompilerGenerated]
		private static Func<string, string, CommonDialog> <>f__mg$cacheA;

		private static CommonDialog CreatePopupMonsterDetail(string assetCategoryId, string assetValue)
		{
			return GUIMain.ShowCommonDialog(null, "CMD_MonsterParamPop", null);
		}

		private static CommonDialog CreatePopupDigistoneDetail(string assetCategoryId, string assetValue)
		{
			GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM assetCategory = MasterDataMng.Instance().RespDataMA_AssetCategoryM.GetAssetCategory(assetCategoryId);
			return CMD_QuestItemPOP.Create(assetCategory);
		}

		private static CommonDialog CreatePopupLinkPointDetail(string assetCategoryId, string assetValue)
		{
			GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM assetCategory = MasterDataMng.Instance().RespDataMA_AssetCategoryM.GetAssetCategory(assetCategoryId);
			return CMD_QuestItemPOP.Create(assetCategory);
		}

		private static CommonDialog CreatePopupClusterDetail(string assetCategoryId, string assetValue)
		{
			GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM assetCategory = MasterDataMng.Instance().RespDataMA_AssetCategoryM.GetAssetCategory(assetCategoryId);
			return CMD_QuestItemPOP.Create(assetCategory);
		}

		private static CommonDialog CreatePopupItemDetail(string assetCategoryId, string assetValue)
		{
			GameWebAPI.RespDataMA_GetItemM.ItemM itemM = MasterDataMng.Instance().RespDataMA_ItemM.GetItemM(assetValue);
			return CMD_QuestItemPOP.Create(itemM);
		}

		private static CommonDialog CreatePopupMeatDetail(string assetCategoryId, string assetValue)
		{
			return null;
		}

		private static CommonDialog CreatePopupSoulDetail(string assetCategoryId, string assetValue)
		{
			GameWebAPI.RespDataMA_GetSoulM.SoulM soul = MasterDataMng.Instance().RespDataMA_SoulM.GetSoul(assetValue);
			return CMD_QuestItemPOP.Create(soul);
		}

		private static CommonDialog CreatePopupFacilityKeyDetail(string assetCategoryId, string assetValue)
		{
			FacilityConditionM[] facilityCondition = FarmDataManager.GetFacilityCondition(assetValue);
			FacilityConditionM facilityConditionM = null;
			foreach (FacilityConditionM facilityConditionM2 in facilityCondition)
			{
				if (facilityConditionM2.conditionType == "1")
				{
					facilityConditionM = facilityConditionM2;
					break;
				}
			}
			CommonDialog result = null;
			if (facilityConditionM != null)
			{
				FacilityM facilityMasterByReleaseId = FarmDataManager.GetFacilityMasterByReleaseId(facilityConditionM.releaseId);
				result = CMD_QuestItemPOP.Create(facilityConditionM, assetValue, facilityMasterByReleaseId);
			}
			return result;
		}

		private static CommonDialog CreatePopupChipDetail(string assetCategoryId, string assetValue)
		{
			GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(assetValue);
			return CMD_QuestItemPOP.Create(chipMainData);
		}

		private static CommonDialog CreatePopupDungeonTicketDetail(string assetCategoryId, string assetValue)
		{
			GameWebAPI.RespDataMA_DungeonTicketMaster respDataMA_DungeonTicketMaster = MasterDataMng.Instance().RespDataMA_DungeonTicketMaster;
			GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM dungeonTicketM = null;
			foreach (GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM dungeonTicketM3 in respDataMA_DungeonTicketMaster.dungeonTicketM)
			{
				if (dungeonTicketM3.dungeonTicketId == assetValue)
				{
					dungeonTicketM = dungeonTicketM3;
					break;
				}
			}
			CommonDialog result = null;
			if (dungeonTicketM != null)
			{
				result = CMD_QuestItemPOP.Create(dungeonTicketM);
			}
			return result;
		}

		private static CommonDialog CreatePopupTitleDetail(string assetCategoryId, string assetValue)
		{
			return null;
		}

		private static FactoryAssetCategoryDetailPopup.PopupCreator GetPopCreator(int assetCategoryId)
		{
			FactoryAssetCategoryDetailPopup.PopupCreator[] array = new FactoryAssetCategoryDetailPopup.PopupCreator[11];
			int num = 0;
			FactoryAssetCategoryDetailPopup.PopupCreator popupCreator = new FactoryAssetCategoryDetailPopup.PopupCreator();
			popupCreator.assetCategoryId = 1;
			FactoryAssetCategoryDetailPopup.PopupCreator popupCreator2 = popupCreator;
			if (FactoryAssetCategoryDetailPopup.<>f__mg$cache0 == null)
			{
				FactoryAssetCategoryDetailPopup.<>f__mg$cache0 = new Func<string, string, CommonDialog>(FactoryAssetCategoryDetailPopup.CreatePopupMonsterDetail);
			}
			popupCreator2.func = FactoryAssetCategoryDetailPopup.<>f__mg$cache0;
			array[num] = popupCreator;
			int num2 = 1;
			popupCreator = new FactoryAssetCategoryDetailPopup.PopupCreator();
			popupCreator.assetCategoryId = 2;
			FactoryAssetCategoryDetailPopup.PopupCreator popupCreator3 = popupCreator;
			if (FactoryAssetCategoryDetailPopup.<>f__mg$cache1 == null)
			{
				FactoryAssetCategoryDetailPopup.<>f__mg$cache1 = new Func<string, string, CommonDialog>(FactoryAssetCategoryDetailPopup.CreatePopupDigistoneDetail);
			}
			popupCreator3.func = FactoryAssetCategoryDetailPopup.<>f__mg$cache1;
			array[num2] = popupCreator;
			int num3 = 2;
			popupCreator = new FactoryAssetCategoryDetailPopup.PopupCreator();
			popupCreator.assetCategoryId = 3;
			FactoryAssetCategoryDetailPopup.PopupCreator popupCreator4 = popupCreator;
			if (FactoryAssetCategoryDetailPopup.<>f__mg$cache2 == null)
			{
				FactoryAssetCategoryDetailPopup.<>f__mg$cache2 = new Func<string, string, CommonDialog>(FactoryAssetCategoryDetailPopup.CreatePopupLinkPointDetail);
			}
			popupCreator4.func = FactoryAssetCategoryDetailPopup.<>f__mg$cache2;
			array[num3] = popupCreator;
			int num4 = 3;
			popupCreator = new FactoryAssetCategoryDetailPopup.PopupCreator();
			popupCreator.assetCategoryId = 4;
			FactoryAssetCategoryDetailPopup.PopupCreator popupCreator5 = popupCreator;
			if (FactoryAssetCategoryDetailPopup.<>f__mg$cache3 == null)
			{
				FactoryAssetCategoryDetailPopup.<>f__mg$cache3 = new Func<string, string, CommonDialog>(FactoryAssetCategoryDetailPopup.CreatePopupClusterDetail);
			}
			popupCreator5.func = FactoryAssetCategoryDetailPopup.<>f__mg$cache3;
			array[num4] = popupCreator;
			int num5 = 4;
			popupCreator = new FactoryAssetCategoryDetailPopup.PopupCreator();
			popupCreator.assetCategoryId = 6;
			FactoryAssetCategoryDetailPopup.PopupCreator popupCreator6 = popupCreator;
			if (FactoryAssetCategoryDetailPopup.<>f__mg$cache4 == null)
			{
				FactoryAssetCategoryDetailPopup.<>f__mg$cache4 = new Func<string, string, CommonDialog>(FactoryAssetCategoryDetailPopup.CreatePopupItemDetail);
			}
			popupCreator6.func = FactoryAssetCategoryDetailPopup.<>f__mg$cache4;
			array[num5] = popupCreator;
			int num6 = 5;
			popupCreator = new FactoryAssetCategoryDetailPopup.PopupCreator();
			popupCreator.assetCategoryId = 13;
			FactoryAssetCategoryDetailPopup.PopupCreator popupCreator7 = popupCreator;
			if (FactoryAssetCategoryDetailPopup.<>f__mg$cache5 == null)
			{
				FactoryAssetCategoryDetailPopup.<>f__mg$cache5 = new Func<string, string, CommonDialog>(FactoryAssetCategoryDetailPopup.CreatePopupMeatDetail);
			}
			popupCreator7.func = FactoryAssetCategoryDetailPopup.<>f__mg$cache5;
			array[num6] = popupCreator;
			int num7 = 6;
			popupCreator = new FactoryAssetCategoryDetailPopup.PopupCreator();
			popupCreator.assetCategoryId = 14;
			FactoryAssetCategoryDetailPopup.PopupCreator popupCreator8 = popupCreator;
			if (FactoryAssetCategoryDetailPopup.<>f__mg$cache6 == null)
			{
				FactoryAssetCategoryDetailPopup.<>f__mg$cache6 = new Func<string, string, CommonDialog>(FactoryAssetCategoryDetailPopup.CreatePopupSoulDetail);
			}
			popupCreator8.func = FactoryAssetCategoryDetailPopup.<>f__mg$cache6;
			array[num7] = popupCreator;
			int num8 = 7;
			popupCreator = new FactoryAssetCategoryDetailPopup.PopupCreator();
			popupCreator.assetCategoryId = 16;
			FactoryAssetCategoryDetailPopup.PopupCreator popupCreator9 = popupCreator;
			if (FactoryAssetCategoryDetailPopup.<>f__mg$cache7 == null)
			{
				FactoryAssetCategoryDetailPopup.<>f__mg$cache7 = new Func<string, string, CommonDialog>(FactoryAssetCategoryDetailPopup.CreatePopupFacilityKeyDetail);
			}
			popupCreator9.func = FactoryAssetCategoryDetailPopup.<>f__mg$cache7;
			array[num8] = popupCreator;
			int num9 = 8;
			popupCreator = new FactoryAssetCategoryDetailPopup.PopupCreator();
			popupCreator.assetCategoryId = 17;
			FactoryAssetCategoryDetailPopup.PopupCreator popupCreator10 = popupCreator;
			if (FactoryAssetCategoryDetailPopup.<>f__mg$cache8 == null)
			{
				FactoryAssetCategoryDetailPopup.<>f__mg$cache8 = new Func<string, string, CommonDialog>(FactoryAssetCategoryDetailPopup.CreatePopupChipDetail);
			}
			popupCreator10.func = FactoryAssetCategoryDetailPopup.<>f__mg$cache8;
			array[num9] = popupCreator;
			int num10 = 9;
			popupCreator = new FactoryAssetCategoryDetailPopup.PopupCreator();
			popupCreator.assetCategoryId = 18;
			FactoryAssetCategoryDetailPopup.PopupCreator popupCreator11 = popupCreator;
			if (FactoryAssetCategoryDetailPopup.<>f__mg$cache9 == null)
			{
				FactoryAssetCategoryDetailPopup.<>f__mg$cache9 = new Func<string, string, CommonDialog>(FactoryAssetCategoryDetailPopup.CreatePopupDungeonTicketDetail);
			}
			popupCreator11.func = FactoryAssetCategoryDetailPopup.<>f__mg$cache9;
			array[num10] = popupCreator;
			int num11 = 10;
			popupCreator = new FactoryAssetCategoryDetailPopup.PopupCreator();
			popupCreator.assetCategoryId = 19;
			FactoryAssetCategoryDetailPopup.PopupCreator popupCreator12 = popupCreator;
			if (FactoryAssetCategoryDetailPopup.<>f__mg$cacheA == null)
			{
				FactoryAssetCategoryDetailPopup.<>f__mg$cacheA = new Func<string, string, CommonDialog>(FactoryAssetCategoryDetailPopup.CreatePopupTitleDetail);
			}
			popupCreator12.func = FactoryAssetCategoryDetailPopup.<>f__mg$cacheA;
			array[num11] = popupCreator;
			FactoryAssetCategoryDetailPopup.PopupCreator[] array2 = array;
			FactoryAssetCategoryDetailPopup.PopupCreator result = null;
			foreach (FactoryAssetCategoryDetailPopup.PopupCreator popupCreator13 in array2)
			{
				if (popupCreator13.assetCategoryId == assetCategoryId)
				{
					result = popupCreator13;
				}
			}
			return result;
		}

		public static CommonDialog Create(string assetCategoryId, string assetValue)
		{
			CommonDialog result = null;
			int assetCategoryId2;
			if (int.TryParse(assetCategoryId, out assetCategoryId2))
			{
				FactoryAssetCategoryDetailPopup.PopupCreator popCreator = FactoryAssetCategoryDetailPopup.GetPopCreator(assetCategoryId2);
				result = popCreator.func(assetCategoryId, assetValue);
			}
			return result;
		}

		public static CommonDialog CreateForExchange(string assetCategoryId, string assetValue, string monsterFixedValueId, string maxExtraSlotNum, string eventExchangeId)
		{
			CommonDialog commonDialog = null;
			int num;
			if (int.TryParse(assetCategoryId, out num))
			{
				FactoryAssetCategoryDetailPopup.PopupCreator popCreator = FactoryAssetCategoryDetailPopup.GetPopCreator(num);
				commonDialog = popCreator.func(assetCategoryId, assetValue);
				if (null != commonDialog && num == 1 && !string.IsNullOrEmpty(monsterFixedValueId))
				{
					CMD_MonsterParamPop cmd_MonsterParamPop = commonDialog as CMD_MonsterParamPop;
					if (null != cmd_MonsterParamPop)
					{
						MonsterFixedM monsterFixedMaster = MonsterFixedData.GetMonsterFixedMaster(monsterFixedValueId);
						if (monsterFixedMaster != null)
						{
							MonsterData monsterData = MonsterDataMng.Instance().CreateMonsterDataByMID(assetValue);
							GameWebAPI.RespDataMA_GetSkillM respDataMA_SkillM = MasterDataMng.Instance().RespDataMA_SkillM;
							GameWebAPI.RespDataMA_GetSkillM.SkillM skillM = null;
							foreach (GameWebAPI.RespDataMA_GetSkillM.SkillM skillM3 in respDataMA_SkillM.skillM)
							{
								if (skillM3.skillGroupId == monsterData.monsterM.skillGroupId && skillM3.skillGroupSubId == monsterFixedMaster.defaultSkillGroupSubId)
								{
									skillM = skillM3;
									break;
								}
							}
							if (int.Parse(monsterFixedMaster.level) > int.Parse(monsterData.monsterM.maxLevel))
							{
								monsterFixedMaster.level = monsterData.monsterM.maxLevel;
							}
							int lvMAXExperienceInfo = DataMng.Instance().GetLvMAXExperienceInfo(int.Parse(monsterFixedMaster.level));
							DataMng.ExperienceInfo experienceInfo = DataMng.Instance().GetExperienceInfo(lvMAXExperienceInfo);
							monsterData.userMonster.luck = monsterFixedMaster.luck;
							monsterData.userMonster.friendship = "0";
							monsterData.userMonster.level = monsterFixedMaster.level;
							monsterData.userMonster.hpAbility = monsterFixedMaster.hpAbility;
							monsterData.userMonster.hpAbilityFlg = monsterFixedMaster.hpAbilityFlg.ToString();
							monsterData.userMonster.attackAbility = monsterFixedMaster.attackAbility;
							monsterData.userMonster.attackAbilityFlg = monsterFixedMaster.attackAbilityFlg.ToString();
							monsterData.userMonster.defenseAbility = monsterFixedMaster.defenseAbility;
							monsterData.userMonster.defenseAbilityFlg = monsterFixedMaster.defenseAbilityFlg.ToString();
							monsterData.userMonster.spAttackAbility = monsterFixedMaster.spAttackAbility;
							monsterData.userMonster.spAttackAbilityFlg = monsterFixedMaster.spAttackAbilityFlg.ToString();
							monsterData.userMonster.spDefenseAbility = monsterFixedMaster.spDefenseAbility;
							monsterData.userMonster.spDefenseAbilityFlg = monsterFixedMaster.spDefenseAbilityFlg.ToString();
							monsterData.userMonster.speedAbility = monsterFixedMaster.speedAbility;
							monsterData.userMonster.speedAbilityFlg = monsterFixedMaster.speedAbilityFlg.ToString();
							monsterData.userMonster.commonSkillId = monsterFixedMaster.commonSkillId;
							monsterData.userMonster.leaderSkillId = monsterFixedMaster.leaderSkillId;
							monsterData.userMonster.defaultSkillGroupSubId = monsterFixedMaster.defaultSkillGroupSubId;
							monsterData.userMonster.uniqueSkillId = skillM.skillId;
							monsterData.InitSkillInfo();
							cmd_MonsterParamPop.MonsterDataSet(monsterData, experienceInfo, int.Parse(maxExtraSlotNum), eventExchangeId);
						}
					}
				}
			}
			return commonDialog;
		}

		private sealed class PopupCreator
		{
			public int assetCategoryId;

			public Func<string, string, CommonDialog> func;
		}
	}
}
