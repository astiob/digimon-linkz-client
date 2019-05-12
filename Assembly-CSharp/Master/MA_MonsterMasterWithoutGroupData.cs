using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MonsterMasterWithoutGroupData : MasterBaseData<GameWebAPI.RespDataMA_GetMonsterMS>
	{
		public MA_MonsterMasterWithoutGroupData()
		{
			base.ID = MasterId.MONSTERS;
		}

		public override string GetTableName()
		{
			return "monster_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_MonsterMasterWithoutGroupData
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetMonsterMS>(base.SetResponse)
			};
		}
	}
}
