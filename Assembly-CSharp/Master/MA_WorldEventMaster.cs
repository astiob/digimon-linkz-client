using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_WorldEventMaster : MasterBaseData<GameWebAPI.RespDataMA_WorldEventMaster>
	{
		public MA_WorldEventMaster()
		{
			base.ID = MasterId.WORLD_EVENT;
		}

		public override string GetTableName()
		{
			return "world_event_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestDataMA_WorldEventMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_WorldEventMaster>(base.SetResponse)
			};
		}
	}
}
