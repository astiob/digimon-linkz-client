using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_WorldAreaMaster : MasterBaseData<GameWebAPI.RespDataMA_GetWorldStageM>
	{
		public MA_WorldAreaMaster()
		{
			base.ID = MasterId.WORLD_STAGE;
		}

		public override string GetTableName()
		{
			return "world_stage_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_WorldStageMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetWorldStageM>(base.SetResponse)
			};
		}
	}
}
