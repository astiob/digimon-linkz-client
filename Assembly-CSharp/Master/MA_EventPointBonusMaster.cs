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
			return new GameWebAPI.RequestDataMA_EventPointBonusM
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_EventPointBonusM>(base.SetResponse)
			};
		}
	}
}
