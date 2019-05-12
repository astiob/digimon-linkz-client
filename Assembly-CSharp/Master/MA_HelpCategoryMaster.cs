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
			return new GameWebAPI.RequestMA_HelpCategoryMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetHelpCategoryM>(base.SetResponse)
			};
		}
	}
}
