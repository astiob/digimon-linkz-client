using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_NavigationMessageMaster : MasterBaseData<GameWebAPI.RespDataMA_NavigationMessageMaster>
	{
		public MA_NavigationMessageMaster()
		{
			base.ID = MasterId.NAVIGATION_MESSAGE;
		}

		public override string GetTableName()
		{
			return "navigation_message_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestDataMA_NavigationMessageMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_NavigationMessageMaster>(base.SetResponse)
			};
		}
	}
}
