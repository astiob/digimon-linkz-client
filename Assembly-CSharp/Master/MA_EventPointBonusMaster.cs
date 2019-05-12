using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_EventPointBonusMaster : MasterBaseData<GameWebAPI.RespDataMA_EventPointBonusM>
	{
		public MA_EventPointBonusMaster()
		{
			base.ID = MasterId.EVENT_POINT_BONUS;
		}

		public override string GetTableName()
		{
			return "event_point_bonus_m";
		}

		public override RequestBase CreateRequest()
		{
			GameWebAPI.RequestDataMA_EventPointBonusMaster requestDataMA_EventPointBonusMaster = new GameWebAPI.RequestDataMA_EventPointBonusMaster();
			requestDataMA_EventPointBonusMaster.SetSendData = delegate(GameWebAPI.RequestDataMA_EventPointBonusM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestDataMA_EventPointBonusMaster.OnReceived = new Action<GameWebAPI.RespDataMA_EventPointBonusM>(base.SetResponse);
			return requestDataMA_EventPointBonusMaster;
		}
	}
}
