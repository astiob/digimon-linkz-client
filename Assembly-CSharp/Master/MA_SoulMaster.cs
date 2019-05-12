using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_SoulMaster : MasterBaseData<GameWebAPI.RespDataMA_GetSoulM>
	{
		public MA_SoulMaster()
		{
			base.ID = MasterId.SOUL;
		}

		public override string GetTableName()
		{
			return "soul_m";
		}

		public override RequestBase CreateRequest()
		{
			GameWebAPI.RequestMA_SoulMaster requestMA_SoulMaster = new GameWebAPI.RequestMA_SoulMaster();
			requestMA_SoulMaster.SetSendData = delegate(GameWebAPI.RequestMA_SoulM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestMA_SoulMaster.OnReceived = new Action<GameWebAPI.RespDataMA_GetSoulM>(base.SetResponse);
			return requestMA_SoulMaster;
		}
	}
}
