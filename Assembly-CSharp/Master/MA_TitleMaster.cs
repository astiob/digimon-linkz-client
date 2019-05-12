using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_TitleMaster : MasterBaseData<GameWebAPI.RespDataMA_TitleMaster>
	{
		public MA_TitleMaster()
		{
			base.ID = MasterId.TITLE;
		}

		public override string GetTableName()
		{
			return "title_m";
		}

		public override RequestBase CreateRequest()
		{
			GameWebAPI.RequestDataMA_TitleMaster requestDataMA_TitleMaster = new GameWebAPI.RequestDataMA_TitleMaster();
			requestDataMA_TitleMaster.SetSendData = delegate(GameWebAPI.RequestDataMA_TitleM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestDataMA_TitleMaster.OnReceived = new Action<GameWebAPI.RespDataMA_TitleMaster>(base.SetResponse);
			return requestDataMA_TitleMaster;
		}
	}
}
