using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MonsterGrowStepMaster : MasterBaseData<GameWebAPI.RespDataMA_GetMonsterGrowStepM>
	{
		public MA_MonsterGrowStepMaster()
		{
			base.ID = MasterId.GROWSTEP;
		}

		public override string GetTableName()
		{
			return "monster_grow_step_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_MonsterGrowStepMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetMonsterGrowStepM>(base.SetResponse)
			};
		}
	}
}
