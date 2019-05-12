using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MonsterTranceMaster : MasterBaseData<GameWebAPI.RespDataMA_GetMonsterTranceM>
	{
		public MA_MonsterTranceMaster()
		{
			base.ID = MasterId.TRANCE;
		}

		public override string GetTableName()
		{
			return "monster_trance_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_MonsterTranceMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetMonsterTranceM>(base.SetResponse)
			};
		}
	}
}
