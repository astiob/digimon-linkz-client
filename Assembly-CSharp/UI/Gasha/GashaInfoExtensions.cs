using Master;
using System;

namespace UI.Gasha
{
	public static class GashaInfoExtensions
	{
		public static MasterDataMng.AssetCategory GetCostAssetsCategory(this GameWebAPI.RespDataGA_GetGachaInfo.PriceType priceType)
		{
			MasterDataMng.AssetCategory assetCategory = MasterDataMng.AssetCategory.NO_DATA_ID;
			int num = 0;
			if (int.TryParse(priceType.category, out num))
			{
				assetCategory = (MasterDataMng.AssetCategory)num;
			}
			Debug.Assert(MasterDataMng.AssetCategory.NO_DATA_ID != assetCategory, "支払うアセットカテゴリーIDの取得に失敗.");
			return assetCategory;
		}

		public static string GetCostAssetsValue(this GameWebAPI.RespDataGA_GetGachaInfo.PriceType priceType)
		{
			return priceType.value;
		}

		public static MasterDataMng.AssetCategory GetPrizeAssetsCategory(this GameWebAPI.RespDataGA_GetGachaInfo.Result gashaInfo)
		{
			MasterDataMng.AssetCategory assetCategory = MasterDataMng.AssetCategory.NO_DATA_ID;
			int num = 0;
			if (int.TryParse(gashaInfo.prize, out num))
			{
				assetCategory = (MasterDataMng.AssetCategory)num;
			}
			Debug.Assert(MasterDataMng.AssetCategory.NO_DATA_ID != assetCategory, "ガシャ景品のアセットカテゴリーの取得失敗.");
			return assetCategory;
		}

		public static GameWebAPI.RespDataGA_GetGachaInfo.Detail GetDetail(this GameWebAPI.RespDataGA_GetGachaInfo.Detail[] detailList, int playCount)
		{
			GameWebAPI.RespDataGA_GetGachaInfo.Detail detail = null;
			string b = playCount.ToString();
			for (int i = 0; i < detailList.Length; i++)
			{
				if (detailList[i].count == b)
				{
					detail = detailList[i];
					break;
				}
			}
			Debug.Assert(null != detail, "ガシャ実行情報の取得に失敗.");
			return detail;
		}

		public static string GetPrice(this GameWebAPI.RespDataGA_GetGachaInfo.Detail detail)
		{
			string result;
			if (detail.isFirst == 1)
			{
				result = detail.priceFirst;
			}
			else if (detail.isTodayFirst == 1 && "1" == detail.dailyResetFirst)
			{
				result = detail.priceFirst;
			}
			else
			{
				result = detail.price;
			}
			return result;
		}

		public static void UpdatePlayCount(this GameWebAPI.RespDataGA_GetGachaInfo.Result gashaInfo, int playCount)
		{
			if ("0" != gashaInfo.totalPlayLimitCount)
			{
				gashaInfo.totalPlayCount = (int.Parse(gashaInfo.totalPlayCount) + playCount).ToString();
			}
			string b = playCount.ToString();
			for (int i = 0; i < gashaInfo.details.Length; i++)
			{
				if (gashaInfo.details[i].count == b)
				{
					if (gashaInfo.details[i].isFirst == 1)
					{
						gashaInfo.details[i].isFirst = 0;
						gashaInfo.details[i].isTodayFirst = 0;
					}
					else if (gashaInfo.details[i].isTodayFirst == 1)
					{
						gashaInfo.details[i].isTodayFirst = 0;
					}
					break;
				}
			}
		}

		public static bool ExistLimitPlayCount(this GameWebAPI.RespDataGA_GetGachaInfo.Result gashaInfo)
		{
			int num = 0;
			int.TryParse(gashaInfo.totalPlayLimitCount, out num);
			return 0 < num;
		}

		public static int GetRemainingPlayCount(this GameWebAPI.RespDataGA_GetGachaInfo.Result gashaInfo)
		{
			int num = 0;
			int.TryParse(gashaInfo.totalPlayLimitCount, out num);
			int num2 = 0;
			int.TryParse(gashaInfo.totalPlayCount, out num2);
			int num3 = num - num2;
			return (0 <= num3) ? num3 : 0;
		}

		public static string GetRemainingPlayCountText(this GameWebAPI.RespDataGA_GetGachaInfo.Result gashaInfo, int playCount)
		{
			string result = string.Empty;
			if (gashaInfo.ExistLimitPlayCount())
			{
				int remainingPlayCount = gashaInfo.GetRemainingPlayCount();
				result = string.Format(StringMaster.GetString("RemainingPlayCount"), remainingPlayCount / playCount);
			}
			return result;
		}

		public static int GetOnceRemainingPlayCount(this GameWebAPI.RespDataGA_GetGachaInfo.Detail detail, int assetsNumber)
		{
			int num = 0;
			int num2 = assetsNumber;
			if (detail.isFirst == 1)
			{
				int num3 = 0;
				if (int.TryParse(detail.priceFirst, out num3) && num3 <= num2)
				{
					num++;
					num2 -= num3;
				}
			}
			else if (detail.isTodayFirst == 1 && "1" == detail.dailyResetFirst)
			{
				int num4 = 0;
				if (int.TryParse(detail.priceFirst, out num4) && num4 <= num2)
				{
					num++;
					num2 -= num4;
				}
			}
			int num5 = 0;
			if (int.TryParse(detail.price, out num5) && 0 < num5)
			{
				num += num2 / num5;
			}
			return num;
		}

		public static void GetUrlPickUpPage(this GameWebAPI.RespDataGA_GetGachaInfo.Result gashaInfo, out string title, out string url)
		{
			MasterDataMng.AssetCategory prizeAssetsCategory = gashaInfo.GetPrizeAssetsCategory();
			if (prizeAssetsCategory != MasterDataMng.AssetCategory.CHIP)
			{
				if (prizeAssetsCategory != MasterDataMng.AssetCategory.DUNGEON_TICKET)
				{
					if (prizeAssetsCategory != MasterDataMng.AssetCategory.MONSTER)
					{
						title = string.Empty;
						url = string.Empty;
					}
					else
					{
						title = StringMaster.GetString("GashaLineupCaution");
						url = WebAddress.EXT_ADR_GASHA_INCIDENCE + gashaInfo.gachaId;
					}
				}
				else
				{
					title = StringMaster.GetString("TicketGashaLineupCaution");
					url = WebAddress.EXT_ADR_TICKET_GASHA_INCIDENCE + gashaInfo.gachaId;
				}
			}
			else
			{
				title = StringMaster.GetString("ChipGashaLineupCaution");
				url = WebAddress.EXT_ADR_CHIP_GASHA_INCIDENCE + gashaInfo.gachaId;
			}
		}

		public static bool IsCampaignNotDisplayType(this GameWebAPI.RespDataGA_GetGachaInfo.Detail detail)
		{
			return "0" == detail.appealTextDisplayType;
		}

		public static bool IsCampaignAlwaysDisplayType(this GameWebAPI.RespDataGA_GetGachaInfo.Detail detail)
		{
			return "1" == detail.appealTextDisplayType;
		}

		public static bool IsCampaignFirstDisplayType(this GameWebAPI.RespDataGA_GetGachaInfo.Detail detail)
		{
			return "2" == detail.appealTextDisplayType;
		}

		public static bool IsCampaignDailyFirstDisplayType(this GameWebAPI.RespDataGA_GetGachaInfo.Detail detail)
		{
			return "3" == detail.appealTextDisplayType;
		}

		public static bool IsCampaignNullText(this GameWebAPI.RespDataGA_GetGachaInfo.Detail detail)
		{
			return string.IsNullOrEmpty(detail.appealText) || "null" == detail.appealText;
		}

		public static string GetCampaignText(this GameWebAPI.RespDataGA_GetGachaInfo.Detail detail)
		{
			string result = string.Empty;
			if (!detail.IsCampaignNullText())
			{
				bool flag = true;
				if (detail.isFirst == 0 && detail.isTodayFirst == 0)
				{
					if (!detail.IsCampaignAlwaysDisplayType())
					{
						flag = false;
					}
				}
				else if (detail.IsCampaignNotDisplayType())
				{
					flag = false;
				}
				else if (detail.isFirst == 0 && detail.IsCampaignFirstDisplayType())
				{
					flag = false;
				}
				if (flag)
				{
					result = detail.appealText;
				}
			}
			return result;
		}
	}
}
