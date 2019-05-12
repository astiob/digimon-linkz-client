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
			GameWebAPI.RequestMA_AssetCategoryMaster requestMA_AssetCategoryMaster = new GameWebAPI.RequestMA_AssetCategoryMaster();
			requestMA_AssetCategoryMaster.SetSendData = delegate(GameWebAPI.RequestMA_AssetCategoryM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestMA_AssetCategoryMaster.OnReceived = new Action<GameWebAPI.RespDataMA_GetAssetCategoryM>(base.SetResponse);
			return requestMA_AssetCategoryMaster;
		}
	}
}
