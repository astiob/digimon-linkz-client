using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MonsterFixedMaster : MasterBaseData<GameWebAPI.RespDataMA_MonsterFixedM>
	{
		public MA_MonsterFixedMaster()
		{
			base.ID = MasterId.MONSTER_FIXED;
		}

		public override string GetTableName()
		{
			return "monster_fixed_value_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMonsterFixedMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_MonsterFixedM>(base.SetResponse)
			};
		}
	}
}
