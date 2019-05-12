using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_WorldStageForceOpenMaster : MasterBaseData<GameWebAPI.ResponseWorldStageForceOpenMaster>
	{
		public MA_WorldStageForceOpenMaster()
		{
			base.ID = MasterId.WORLD_STAGE_FORCE_OPEN;
		}

		public override string GetTableName()
		{
			return "world_stage_force_open_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_WorldStageForceOpenMaster
			{
				OnReceived = new Action<GameWebAPI.ResponseWorldStageForceOpenMaster>(base.SetResponse)
			};
		}
	}
}
