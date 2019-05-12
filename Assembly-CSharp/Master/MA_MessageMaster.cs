using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MessageMaster : MasterBaseData<GameWebAPI.RespDataMA_MessageM>
	{
		public MA_MessageMaster()
		{
			base.ID = MasterId.MESSAGE_MASTER;
		}

		public override string GetTableName()
		{
			return "message_m";
		}

		public override RequestBase CreateRequest()
		{
			GameWebAPI.RequestMA_MessageMaster requestMA_MessageMaster = new GameWebAPI.RequestMA_MessageMaster();
			requestMA_MessageMaster.SetSendData = delegate(GameWebAPI.RequestMA_MessageM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestMA_MessageMaster.OnReceived = new Action<GameWebAPI.RespDataMA_MessageM>(base.SetResponse);
			return requestMA_MessageMaster;
		}
	}
}
