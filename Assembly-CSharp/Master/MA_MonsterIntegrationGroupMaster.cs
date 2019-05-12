using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MonsterIntegrationGroupMaster : MasterBaseData<GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster>
	{
		public MA_MonsterIntegrationGroupMaster()
		{
			base.ID = MasterId.MONSTER_INTEGRATION_GROUP;
		}

		public override string GetTableName()
		{
			return "monster_integration_group_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestDataMA_MonsterIntegrationGroupMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster>(base.SetResponse)
			};
		}
	}
}
