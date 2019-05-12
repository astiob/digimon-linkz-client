using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_ColosseumRankMaster : MasterBaseData<GameWebAPI.RespDataMA_ColosseumRankM>
	{
		public MA_ColosseumRankMaster()
		{
			base.ID = MasterId.COLOSSEUM_RANK;
		}

		public override string GetTableName()
		{
			return "colosseum_rank_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_ColosseumRankMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_ColosseumRankM>(base.SetResponse)
			};
		}
	}
}
