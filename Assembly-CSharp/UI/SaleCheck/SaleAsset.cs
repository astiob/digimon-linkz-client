using System;
using System.Collections.Generic;
using System.Linq;

namespace UI.SaleCheck
{
	public sealed class SaleAsset
	{
		private List<SaleAsset.Info> saleAssets = new List<SaleAsset.Info>();

		public void AddSaleAsset(string assetCategoryId, string assetValue)
		{
			SaleAsset.Info item = new SaleAsset.Info
			{
				saleAssetCategoryId = assetCategoryId,
				saleAssetValue = assetValue,
				saleAssetNum = 1
			};
			this.saleAssets.Add(item);
		}

		public bool ExistSaleAsset()
		{
			return 0 < this.saleAssets.Count;
		}

		public SaleBonus GetSalesBonus()
		{
			SaleAsset.<GetSalesBonus>c__AnonStorey0 <GetSalesBonus>c__AnonStorey = new SaleAsset.<GetSalesBonus>c__AnonStorey0();
			SaleBonus saleBonus = new SaleBonus();
			<GetSalesBonus>c__AnonStorey.salesBonuses = MasterDataMng.Instance().AssetSalesBonusMaster.assetSalesBonusM;
			int i;
			for (i = 0; i < <GetSalesBonus>c__AnonStorey.salesBonuses.Length; i++)
			{
				SaleAsset.Info[] array = this.saleAssets.Where((SaleAsset.Info x) => x.saleAssetCategoryId == <GetSalesBonus>c__AnonStorey.salesBonuses[i].baseAssetCategoryId && x.saleAssetValue == <GetSalesBonus>c__AnonStorey.salesBonuses[i].baseAssetValue).ToArray<SaleAsset.Info>();
				foreach (SaleAsset.Info info in array)
				{
					saleBonus.AddBonus(<GetSalesBonus>c__AnonStorey.salesBonuses[i], info.saleAssetNum);
				}
			}
			return saleBonus;
		}

		private sealed class Info
		{
			public string saleAssetCategoryId;

			public string saleAssetValue;

			public int saleAssetNum;
		}
	}
}
