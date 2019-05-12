using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MonsterMasterOnlyGroupData : MasterBaseData<GameWebAPI.RespDataMA_GetMonsterMG>
	{
		public MA_MonsterMasterOnlyGroupData()
		{
			base.ID = MasterId.MONSTERG;
		}

		public override string GetTableName()
		{
			return "monster_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_MonsterMasterOnlyGroupData
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetMonsterMG>(base.SetResponse)
			};
		}
	}
}
