using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_ColosseumTimeScheduleMaster : MasterBaseData<GameWebAPI.RespDataMA_ColosseumTimeScheduleM>
	{
		public MA_ColosseumTimeScheduleMaster()
		{
			base.ID = MasterId.COLOSSEUM_TIME_SCHEDULE;
		}

		public override string GetTableName()
		{
			return "colosseum_time_schedule_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_ColosseumTimeScheduleMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_ColosseumTimeScheduleM>(base.SetResponse)
			};
		}
	}
}
