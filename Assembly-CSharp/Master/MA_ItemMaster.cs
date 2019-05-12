using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_ItemMaster : MasterBaseData<GameWebAPI.RespDataMA_GetItemM>
	{
		public MA_ItemMaster()
		{
			base.ID = MasterId.ITEM;
		}

		public override string GetTableName()
		{
			return "item_m";
		}

		public override RequestBase CreateRequest()
		{
			GameWebAPI.RequestMA_ItemMaster requestMA_ItemMaster = new GameWebAPI.RequestMA_ItemMaster();
			requestMA_ItemMaster.SetSendData = delegate(GameWebAPI.RequestMA_ItemM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestMA_ItemMaster.OnReceived = new Action<GameWebAPI.RespDataMA_GetItemM>(base.SetResponse);
			return requestMA_ItemMaster;
		}
	}
}
