using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MonsterStatusAilmentGroupMaster : MasterBaseData<GameWebAPI.RespDataMA_MonsterStatusAilmentGroupMaster>
	{
		public MA_MonsterStatusAilmentGroupMaster()
		{
			base.ID = MasterId.MONSTER_STATUS_AILMENT_MATERIAL;
		}

		public override string GetTableName()
		{
			return "monster_status_ailment_material_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMonsterStatusAilmentGroupMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_MonsterStatusAilmentGroupMaster>(base.SetResponse)
			};
		}
	}
}
