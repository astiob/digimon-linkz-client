using Master;
using System;

namespace UI.Common
{
	public static class FactoryPayConfirmNotice
	{
		private static string GetInfoText(MasterDataMng.AssetCategory category, int payValue, int gashaCount)
		{
			string result = string.Empty;
			string arg = string.Empty;
			GameWebAPI.RespDataMA_GetAssetCategoryM respDataMA_AssetCategoryM = MasterDataMng.Instance().RespDataMA_AssetCategoryM;
			if (respDataMA_AssetCategoryM != null)
			{
				GameWebAPI.RespDataMA_GetAssetCategoryM respDataMA_GetAssetCategoryM = respDataMA_AssetCategoryM;
				int num = (int)category;
				GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM assetCategory = respDataMA_GetAssetCategoryM.GetAssetCategory(num.ToString());
				if (assetCategory != null)
				{
					arg = assetCategory.assetTitle;
				}
			}
			if (category == MasterDataMng.AssetCategory.DIGI_STONE || category != MasterDataMng.AssetCategory.LINK_POINT)
			{
				result = string.Format(StringMaster.GetString("GashaDigistone"), arg, payValue, gashaCount);
			}
			else
			{
				result = string.Format(StringMaster.GetString("GashaLinkpoint"), arg, payValue, gashaCount);
			}
			return result;
		}

		public static CMD CreateDialog(MasterDataMng.AssetCategory category, string assetsValue, string title, int assetsNumber, int payValue, Action OnPushedYes, int gashaCount, object useDetail)
		{
			CMD result;
			if (category != MasterDataMng.AssetCategory.DIGI_STONE)
			{
				if (category != MasterDataMng.AssetCategory.LINK_POINT)
				{
				}
				CMD_ChangePOP cmd_ChangePOP = GUIMain.ShowCommonDialog(null, "CMD_ChangePOP", null) as CMD_ChangePOP;
				cmd_ChangePOP.Title = title;
				cmd_ChangePOP.OnPushedYesAction = OnPushedYes;
				cmd_ChangePOP.Info = FactoryPayConfirmNotice.GetInfoText(category, payValue, gashaCount);
				cmd_ChangePOP.SetPoint(assetsNumber, payValue);
				cmd_ChangePOP.SetUseDetail(useDetail);
				cmd_ChangePOP.SetAsset(category, assetsValue);
				result = cmd_ChangePOP;
			}
			else
			{
				CMD_ChangePOP_STONE cmd_ChangePOP_STONE = GUIMain.ShowCommonDialog(null, "CMD_ChangePOP_STONE", null) as CMD_ChangePOP_STONE;
				cmd_ChangePOP_STONE.Title = title;
				cmd_ChangePOP_STONE.OnPushedYesAction = OnPushedYes;
				cmd_ChangePOP_STONE.Info = FactoryPayConfirmNotice.GetInfoText(category, payValue, gashaCount);
				cmd_ChangePOP_STONE.SetDigistone(assetsNumber, payValue);
				cmd_ChangePOP_STONE.SetUseDetail(useDetail);
				result = cmd_ChangePOP_STONE;
			}
			return result;
		}
	}
}
