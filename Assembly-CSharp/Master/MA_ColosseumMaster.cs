using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_ColosseumMaster : MasterBaseData<GameWebAPI.RespDataMA_ColosseumM>
	{
		public MA_ColosseumMaster()
		{
			base.ID = MasterId.COLOSSEUM;
		}

		public override string GetTableName()
		{
			return "colosseum_m";
		}

		public override RequestBase CreateRequest()
		{
			GameWebAPI.RequestMA_ColosseumMaster requestMA_ColosseumMaster = new GameWebAPI.RequestMA_ColosseumMaster();
			requestMA_ColosseumMaster.SetSendData = delegate(GameWebAPI.RequestMA_ColosseumM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestMA_ColosseumMaster.OnReceived = new Action<GameWebAPI.RespDataMA_ColosseumM>(base.SetResponse);
			return requestMA_ColosseumMaster;
		}
	}
}
