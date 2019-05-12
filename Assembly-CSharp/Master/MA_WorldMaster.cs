using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_WorldMaster : MasterBaseData<GameWebAPI.RespDataMA_GetWorldAreaM>
	{
		public MA_WorldMaster()
		{
			base.ID = MasterId.WORLD_AREA;
		}

		public override string GetTableName()
		{
			return "world_area_m";
		}

		public override RequestBase CreateRequest()
		{
			GameWebAPI.RequestMA_WorldAreaMaster requestMA_WorldAreaMaster = new GameWebAPI.RequestMA_WorldAreaMaster();
			requestMA_WorldAreaMaster.SetSendData = delegate(GameWebAPI.RequestMA_WorldAreaM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestMA_WorldAreaMaster.OnReceived = new Action<GameWebAPI.RespDataMA_GetWorldAreaM>(base.SetResponse);
			return requestMA_WorldAreaMaster;
		}
	}
}
