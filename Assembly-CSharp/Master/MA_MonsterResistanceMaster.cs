using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MonsterResistanceMaster : MasterBaseData<GameWebAPI.RespDataMA_GetMonsterResistanceM>
	{
		public MA_MonsterResistanceMaster()
		{
			base.ID = MasterId.RESISTANCE;
		}

		public override string GetTableName()
		{
			return "monster_resistance_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_MonsterResistanceMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetMonsterResistanceM>(base.SetResponse)
			};
		}
	}
}
