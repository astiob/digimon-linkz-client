using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_SoulMaster : MasterBaseData<GameWebAPI.RespDataMA_GetSoulM>
	{
		public MA_SoulMaster()
		{
			base.ID = MasterId.SOUL;
		}

		public override string GetTableName()
		{
			return "soul_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_SoulMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetSoulM>(base.SetResponse)
			};
		}
	}
}
