using Evolution;
using FarmData;
using Monster;
using System;
using System.Linq;

namespace UI.Common
{
	public static class UIAssetName
	{
		private static string GetAssetValueName(int assetCategoryId, string assetValue)
		{
			string result = string.Empty;
			switch (assetCategoryId)
			{
			case 14:
			{
				GameWebAPI.RespDataMA_GetSoulM.SoulM soulMasterBySoulId = VersionUpMaterialData.GetSoulMasterBySoulId(assetValue);
				if (soulMasterBySoulId != null)
				{
					result = soulMasterBySoulId.soulName;
				}
				break;
			}
			default:
				if (assetCategoryId != 1)
				{
					if (assetCategoryId == 6)
					{
						GameWebAPI.RespDataMA_GetItemM.ItemM itemM = MasterDataMng.Instance().RespDataMA_ItemM.GetItemM(assetValue);
						if (itemM != null)
						{
							result = itemM.name;
						}
					}
				}
				else
				{
					GameWebAPI.RespDataMA_GetMonsterMS.MonsterM simple = MonsterMaster.GetMonsterMasterByMonsterId(assetValue).Simple;
					if (simple != null)
					{
						GameWebAPI.RespDataMA_GetMonsterMG.MonsterM group = MonsterMaster.GetMonsterMasterByMonsterGroupId(simple.monsterGroupId).Group;
						if (group != null)
						{
							result = group.monsterName;
						}
					}
				}
				break;
			case 16:
			{
				FacilityKeyM facilityKeyMaster = FarmDataManager.GetFacilityKeyMaster(assetValue);
				if (facilityKeyMaster != null)
				{
					result = facilityKeyMaster.facilityKeyName;
				}
				break;
			}
			case 17:
			{
				int chipId = int.Parse(assetValue);
				GameWebAPI.RespDataMA_ChipM.Chip chipMaster = ChipDataMng.GetChipMaster(chipId);
				if (chipMaster != null)
				{
					result = chipMaster.name;
				}
				break;
			}
			case 18:
			{
				GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM dungeonTicketM = MasterDataMng.Instance().RespDataMA_DungeonTicketMaster.dungeonTicketM.FirstOrDefault((GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM x) => assetValue == x.dungeonTicketId);
				if (dungeonTicketM != null)
				{
					result = dungeonTicketM.name;
				}
				break;
			}
			case 19:
			{
				GameWebAPI.RespDataMA_TitleMaster.TitleM titleM = TitleDataMng.GetDictionaryTitleM()[int.Parse(assetValue)];
				if (titleM != null)
				{
					result = titleM.name;
				}
				break;
			}
			}
			return result;
		}

		public static string GetAssetName(string assetCategoryId, string assetValue)
		{
			string result = string.Empty;
			GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM assetCategory = MasterDataMng.Instance().RespDataMA_AssetCategoryM.GetAssetCategory(assetCategoryId);
			if (assetCategory != null)
			{
				result = assetCategory.assetTitle;
				int assetCategoryId2;
				if (int.TryParse(assetCategory.assetCategoryId, out assetCategoryId2))
				{
					string assetValueName = UIAssetName.GetAssetValueName(assetCategoryId2, assetValue);
					if (!string.IsNullOrEmpty(assetValueName))
					{
						result = assetValueName;
					}
				}
			}
			return result;
		}
	}
}
