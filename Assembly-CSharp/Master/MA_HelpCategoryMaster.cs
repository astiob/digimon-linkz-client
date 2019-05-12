using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_HelpCategoryMaster : MasterBaseData<GameWebAPI.RespDataMA_GetHelpCategoryM>
	{
		public MA_HelpCategoryMaster()
		{
			base.ID = MasterId.HELP_CATEGORY;
		}

		public override string GetTableName()
		{
			return "help_category_m";
		}

		public override RequestBase CreateRequest()
		{
			GameWebAPI.RequestMA_HelpCategoryMaster requestMA_HelpCategoryMaster = new GameWebAPI.RequestMA_HelpCategoryMaster();
			requestMA_HelpCategoryMaster.SetSendData = delegate(GameWebAPI.RequestMA_HelpCategoryM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestMA_HelpCategoryMaster.OnReceived = new Action<GameWebAPI.RespDataMA_GetHelpCategoryM>(base.SetResponse);
			return requestMA_HelpCategoryMaster;
		}
	}
}
