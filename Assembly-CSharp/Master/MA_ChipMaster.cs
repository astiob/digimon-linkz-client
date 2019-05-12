using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_ChipMaster : MasterBaseData<GameWebAPI.RespDataMA_ChipM>
	{
		public MA_ChipMaster()
		{
			base.ID = MasterId.CHIP;
		}

		public override string GetTableName()
		{
			return "chip_m";
		}

		public override RequestBase CreateRequest()
		{
			GameWebAPI.RequestMA_ChipMaster requestMA_ChipMaster = new GameWebAPI.RequestMA_ChipMaster();
			requestMA_ChipMaster.SetSendData = delegate(GameWebAPI.RequestMA_ChipM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestMA_ChipMaster.OnReceived = new Action<GameWebAPI.RespDataMA_ChipM>(base.SetResponse);
			return requestMA_ChipMaster;
		}
	}
}
