using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_HelpMaster : MasterBaseData<GameWebAPI.RespDataMA_GetHelpM>
	{
		public MA_HelpMaster()
		{
			base.ID = MasterId.HELP;
		}

		public override string GetTableName()
		{
			return "help_m";
		}

		public override RequestBase CreateRequest()
		{
			GameWebAPI.RequestMA_HelpMaster requestMA_HelpMaster = new GameWebAPI.RequestMA_HelpMaster();
			requestMA_HelpMaster.SetSendData = delegate(GameWebAPI.RequestMA_HelpM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestMA_HelpMaster.OnReceived = new Action<GameWebAPI.RespDataMA_GetHelpM>(base.SetResponse);
			return requestMA_HelpMaster;
		}
	}
}
