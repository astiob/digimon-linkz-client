using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MonsterStatusAilmentMaster : MasterBaseData<GameWebAPI.RespDataMA_MonsterStatusAilmentMaster>
	{
		public MA_MonsterStatusAilmentMaster()
		{
			base.ID = MasterId.MONSTER_STATUS_AILMENT;
		}

		public override string GetTableName()
		{
			return "monster_status_ailment_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMonsterStatusAilmentMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_MonsterStatusAilmentMaster>(base.SetResponse)
			};
		}
	}
}
