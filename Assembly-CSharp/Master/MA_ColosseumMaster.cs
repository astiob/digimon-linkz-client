using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_ColosseumMaster : MasterBaseData<GameWebAPI.RespDataMA_ColosseumM>
	{
		public MA_ColosseumMaster()
		{
			base.ID = MasterId.COLOSSEUM;
		}

		public override string GetTableName()
		{
			return "colosseum_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_ColosseumMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_ColosseumM>(base.SetResponse)
			};
		}
	}
}
