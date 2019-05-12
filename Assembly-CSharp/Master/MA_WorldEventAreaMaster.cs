using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_WorldEventAreaMaster : MasterBaseData<GameWebAPI.RespDataMA_WorldEventAreaMaster>
	{
		public MA_WorldEventAreaMaster()
		{
			base.ID = MasterId.WORLD_EVENT_AREA;
		}

		public override string GetTableName()
		{
			return "world_event_area_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestDataMA_WorldEventAreaMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_WorldEventAreaMaster>(base.SetResponse)
			};
		}
	}
}
