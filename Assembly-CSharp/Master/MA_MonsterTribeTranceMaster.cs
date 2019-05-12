using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MonsterTribeTranceMaster : MasterBaseData<GameWebAPI.RespDataMA_GetMonsterTribeTranceM>
	{
		public MA_MonsterTribeTranceMaster()
		{
			base.ID = MasterId.TRIBE_TRANCE;
		}

		public override string GetTableName()
		{
			return "monster_tribe_trance_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_MonsterTribeTranceMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetMonsterTribeTranceM>(base.SetResponse)
			};
		}
	}
}
