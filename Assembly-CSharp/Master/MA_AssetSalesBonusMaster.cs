using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_AssetSalesBonusMaster : MasterBaseData<GameWebAPI.ResponseAssetSalesBonusMaster>
	{
		public MA_AssetSalesBonusMaster()
		{
			base.ID = MasterId.ASSET_SALES_BONUS;
		}

		public override string GetTableName()
		{
			return "asset_sales_bonus_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_AssetSalesBonusMaster
			{
				OnReceived = new Action<GameWebAPI.ResponseAssetSalesBonusMaster>(base.SetResponse)
			};
		}
	}
}
