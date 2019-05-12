using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_WorldMaster : MasterBaseData<GameWebAPI.RespDataMA_GetWorldAreaM>
	{
		public MA_WorldMaster()
		{
			base.ID = MasterId.WORLD_AREA;
		}

		public override string GetTableName()
		{
			return "world_area_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_WorldAreaMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetWorldAreaM>(base.SetResponse)
			};
		}
	}
}
