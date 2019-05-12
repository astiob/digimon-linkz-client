using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MonsterArousalMaster : MasterBaseData<GameWebAPI.RespDataMA_MonsterArousalMaster>
	{
		public MA_MonsterArousalMaster()
		{
			base.ID = MasterId.MONSTER_AROUSAL;
		}

		public override string GetTableName()
		{
			return "monster_rare_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMonsterArousalMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_MonsterArousalMaster>(base.SetResponse)
			};
		}
	}
}
