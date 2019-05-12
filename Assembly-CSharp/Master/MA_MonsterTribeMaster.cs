using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MonsterTribeMaster : MasterBaseData<GameWebAPI.RespDataMA_GetMonsterTribeM>
	{
		public MA_MonsterTribeMaster()
		{
			base.ID = MasterId.TRIBE;
		}

		public override string GetTableName()
		{
			return "monster_tribe_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_MonsterTribeMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetMonsterTribeM>(base.SetResponse)
			};
		}
	}
}
