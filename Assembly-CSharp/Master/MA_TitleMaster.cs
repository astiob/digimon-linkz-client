using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_TitleMaster : MasterBaseData<GameWebAPI.RespDataMA_TitleMaster>
	{
		public MA_TitleMaster()
		{
			base.ID = MasterId.TITLE;
		}

		public override string GetTableName()
		{
			return "title_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestDataMA_TitleMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_TitleMaster>(base.SetResponse)
			};
		}
	}
}
