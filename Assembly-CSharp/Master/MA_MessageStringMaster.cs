using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MessageStringMaster : MasterBaseData<GameWebAPI.RespDataMA_MessageStringM>
	{
		public MA_MessageStringMaster()
		{
			base.ID = MasterId.MESSAGE_STRING_MASTER;
		}

		public override string GetTableName()
		{
			return "message_string_m";
		}

		public override RequestBase CreateRequest()
		{
			GameWebAPI.RequestMA_MessageStringMaster requestMA_MessageStringMaster = new GameWebAPI.RequestMA_MessageStringMaster();
			requestMA_MessageStringMaster.SetSendData = delegate(GameWebAPI.RequestMA_MessageStringM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestMA_MessageStringMaster.OnReceived = new Action<GameWebAPI.RespDataMA_MessageStringM>(base.SetResponse);
			return requestMA_MessageStringMaster;
		}
	}
}
