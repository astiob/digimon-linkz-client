using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_AssetCategoryMaster : MasterBaseData<GameWebAPI.RespDataMA_GetAssetCategoryM>
	{
		public MA_AssetCategoryMaster()
		{
			base.ID = MasterId.ASSET_CATEGORY;
		}

		public override string GetTableName()
		{
			return "asset_category_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_AssetCategoryMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetAssetCategoryM>(base.SetResponse)
			};
		}
	}
}
